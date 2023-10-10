//-----------------------------------------------------------------------
// <copyright file="AnalysisDatabase.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace PhotonKinetics.OASYS
{
    using System;
    using System.Collections.Generic;
    using System.Data.OleDb;

    /// <summary>
    /// Data access layer for the 8000 Front Panel database analysis settings
    /// </summary>
    public class AnalysisDatabase
    {
        /// <summary>
        /// The connection string to connect to the OTDR8000DB database.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// The speed of light in vacuum in m/s
        /// </summary>
        private readonly double speedOfLight = 299792458d;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalysisDatabase"/> class.
        /// </summary>
        /// <param name="udlFile">The full file path to the UDL file
        /// that contains the connection string for the database.
        /// </param>
        public AnalysisDatabase(string udlFile)
        {
            if (string.IsNullOrEmpty(udlFile))
            {
                throw new ArgumentException("Must provide valid UDL file.", "udlFile");
            }
            else
            {
                connectionString = "File Name=" + udlFile;
            }
        }

        #region Public Methods
        /// <summary>
        /// Get an array of all Analysis entries
        /// </summary>
        /// <returns> An array of strings containing the names of all Analysis
        /// setups in the database.
        /// </returns>
        public string[] AvailableAnalysisEntries()
        {
            string cmd = "SELECT analysisName FROM AnalysisTbl WHERE (resultsValid = 0)";
            var entries = new List<string>();
            using (var conn = new OleDbConnection(connectionString))
            {
                using (var command = new OleDbCommand(cmd, conn))
                {
                    command.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            entries.Add(reader["analysisName"].ToString());
                        }
                    }
                }
            }

            return entries.ToArray();
        }

        /// <summary>
        /// Get the <see cref="AnalysisSetupType"/> for the provided primary key value.
        /// </summary>
        /// <param name="databaseID">The <c>analysisID</c> in the <c>TestWavelengthTbl</c> for the <see cref="AnalysisSetupType"/>.</param>
        /// <returns>Returns the <see cref="AnalysisSetupType"/> for the provided ID value.</returns>
        public AnalysisSetupType GetAnalysisSetup(int databaseID)
        {
            var setups = new List<AnalysisWavelengthSetupType>();
            string cmd = "SELECT * FROM TestWavelengthTbl WHERE analysisID = " + databaseID.ToString() + " Order By wavelengthIndex Asc";
            using (var conn = new OleDbConnection(connectionString))
            {
                using (var command = new OleDbCommand(cmd, conn))
                {
                    command.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Top-level initialization of wavelength
                            var setup = new AnalysisWavelengthSetupType();
                            double gi = (double)reader["groupIndex"];
                            int wavelengthKey = (int)reader["wavelengthId"];
                            setup.WavelengthID = reader["wavelength"].ToString();

                            // Read parameters for wavelength
                            ReadAnalysisParams(setup, wavelengthKey, gi);
                            ReadSlidingWindowParams(setup, wavelengthKey, gi);
                            setup.Limits = new LimitSetType[2];
                            ReadLimitSet(setup, wavelengthKey, gi, DirectionalityAspectType.Unidirectional);
                            ReadLimitSet(setup, wavelengthKey, gi, DirectionalityAspectType.Bidirectional);
                            setups.Add(setup);
                        }
                    }
                }
            }

            return new AnalysisSetupType() { AnalysisWavelengthSetup = setups.ToArray() };
        }

        /// <summary>
        /// Get a database ID (primary key) value for a given Analysis setup name
        /// </summary>
        /// <param name="name">The name of the Analysis setup for which to retrieve the ID.</param>
        /// <returns> The database ID as an int.</returns>
        public int DatabaseID(string name)
        {
            string qry = "SELECT analysisID FROM AnalysisTbl WHERE analysisName = '" + name + "'";
            using (var conn = new OleDbConnection(connectionString))
            {
                using (var command = new OleDbCommand(qry, conn))
                {
                    command.Connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return (int)reader["analysisId"];
                        }
                        else
                        {
                            throw new ApplicationException("Name " + name + "  not found in database");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Delete an Analysis Limit set
        /// </summary>
        /// <param name="limitSet">
        /// The name of the limit set to delete.
        /// </param>
        public void DeleteLimitSet(string limitSet)
        {
            int analysisID = DatabaseID(limitSet);

            // Rather a pain. If an item is a key held by another item, that other item has to go first.
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();

                // Delete spectral wavelengths and their limits
                DeleteSpectral(conn, analysisID);
                using (var command = new OleDbCommand())
                {
                    command.Connection = conn;

                    // Delete sliding window params
                    command.CommandText = "DELETE FROM SlidingWindowParamTbl WHERE wavelengthId IN (SELECT wavelengthId FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString() + ")";
                    command.ExecuteNonQuery();

                    // Delete fiber analysis params
                    command.CommandText = "DELETE FROM FiberAnalysisParamTbl WHERE wavelengthId In (SELECT wavelengthId FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString() + ")";
                    command.ExecuteNonQuery();

                    // Delete wad of limits. There must be a better way...
                    command.CommandText = "SELECT * FROM TestLimitsTbl WHERE wavelengthId IN (SELECT wavelengthId FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString() + ")";
                    var limitIDs = new List<int>();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            limitIDs.Add((int)reader["biAttenCoef"]);
                            limitIDs.Add((int)reader["biEventLoss"]);
                            limitIDs.Add((int)reader["bufferLoss"]);
                            limitIDs.Add((int)reader["bufferRefl"]);
                            limitIDs.Add((int)reader["endMFD"]);
                            limitIDs.Add((int)reader["eventRefl"]);
                            limitIDs.Add((int)reader["fiberLength"]);
                            limitIDs.Add((int)reader["lsaDeviation"]);
                            limitIDs.Add((int)reader["slidingWindowAtten"]);
                            limitIDs.Add((int)reader["uniAttenCoef"]);
                            limitIDs.Add((int)reader["uniEventLoss"]);
                            limitIDs.Add((int)reader["slidingWindowUni"]);
                        }
                    }

                    command.CommandText = "DELETE FROM TestLimitsTbl WHERE wavelengthId IN " + "(SELECT wavelengthId FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString() + ")";
                    command.ExecuteNonQuery();
                    foreach (int limitID in limitIDs)
                    {
                        DeleteLimits(conn, limitID);
                    }

                    // Delete buffer fibers
                    command.CommandText = "DELETE FROM BufferFiberTbl WHERE wavelengthId IN " + "(SELECT wavelengthId FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString() + ")";
                    command.ExecuteNonQuery();

                    // Delete Test Parameters
                    command.CommandText = "DELETE FROM TestParametersTbl WHERE wavelengthId IN " + "(SELECT wavelengthId FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString() + ")";
                    command.ExecuteNonQuery();

                    // Delete Test Wavelengths
                    command.CommandText = "DELETE FROM TestWavelengthTbl WHERE analysisId = " + analysisID.ToString();
                    command.ExecuteNonQuery();

                    // Finally, delete the analysis row
                    command.CommandText = "DELETE FROM AnalysisTbl WHERE analysisId = " + analysisID.ToString();
                    command.ExecuteNonQuery();
                }
            }
        }
        #endregion

        #region Private helper methods
        /// <summary>
        /// Deletes a spectral model.
        /// </summary>
        /// <param name="conn">An open database connection.</param>
        /// <param name="analysisID">The <c>analysisId</c> primary key of the record to delete from the <c>SpectralWavelengthTbl</c> table.</param>
        private void DeleteSpectral(OleDbConnection conn, int analysisID)
        {
            int limitsID = -1;
            using (var command = new OleDbCommand())
            {
                // the connection is already open
                command.Connection = conn;

                // do not delete any spectral models for this analysisID if any other analysisIDs use this spectral model
                command.CommandText = "SELECT spectralId FROM AnalysisTbl WHERE spectralId IN (Select spectralId FROM AnalysisTbl WHERE analysisId = " + analysisID + ")";
                int numAnalysisUsingSpectralModel = 0;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        numAnalysisUsingSpectralModel++;
                        if (numAnalysisUsingSpectralModel > 1)
                        {
                            return;
                        }
                    }
                }

                command.CommandText = "SELECT limitsId FROM SpectralWavelengthTbl WHERE analysisId=" + analysisID.ToString();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        limitsID = (int)reader.GetInt32(0);
                    }
                }

                command.CommandText = "DELETE FROM SpectralWavelengthTbl WHERE analysisId=" + analysisID.ToString();
                command.ExecuteNonQuery();
                if (limitsID > -1)
                {
                    DeleteLimits(conn, limitsID);
                }
            }
        }

        /// <summary>
        /// Deletes a set of analysis limits.
        /// </summary>
        /// <param name="conn">An open database connection</param>
        /// <param name="limitsID">The <c>numLimitId</c> primary key of the record to delete from the <c>numericLimitTbl</c> table.</param>
        private void DeleteLimits(OleDbConnection conn, int limitsID)
        {
            string cmd = "DELETE FROM numericLimitTbl WHERE numLimitId=" + limitsID.ToString();
            using (var command = new OleDbCommand(cmd, conn))
            {
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Reads the Sliding Window parameters into an <see cref="AnalysisWavelengthSetupType"/> for a given wavelength.
        /// </summary>
        /// <param name="setup">The <see cref="AnalysisWavelengthSetupType"/> upon which to set the sliding window parameters.</param>
        /// <param name="wavelengthKey">The <c>wavelengthId</c> primary key in <c>SlidingWindowParamTbl</c> table.</param>
        /// <param name="gi">The group index of the fiber for the wavelength in question.</param>
        private void ReadSlidingWindowParams(AnalysisWavelengthSetupType setup, int wavelengthKey, double gi)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string qry = "SELECT * FROM SlidingWindowParamTbl WHERE wavelengthId = " + wavelengthKey.ToString();
                using (var cmd = new OleDbCommand(qry, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        setup.AnalysisParams.WindowStep = SecToMeters((double)reader["windowStep"], gi);
                        setup.AnalysisParams.WindowWidth = SecToMeters((double)reader["windowWidth"], gi);
                    }
                    else
                    {
                        throw new ApplicationException("Database entry for window parameters not found.");
                    }
                }
            }
        }

        /// <summary>
        /// Read a complete set of analysis parameters from the database for a particular wavelength.
        /// </summary>
        /// <param name="setup">The <see cref="AnalysisWavelengthSetupType"/> to set the <c>AnalysisParams</c>.
        /// property on once the parameters have been read.</param>
        /// <param name="wavelengthKey">The <c>wavelengthId</c> primary key value in the <c>FiberAnalysisParamTbl</c> to retrieve.</param>
        /// <param name="gi">The group index of the fiber at the wavelength in question.</param>
        private void ReadAnalysisParams(AnalysisWavelengthSetupType setup, int wavelengthKey, double gi)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string qry = "SELECT * FROM FiberAnalysisParamTbl WHERE wavelengthId = " + wavelengthKey.ToString();
                using (var cmd = new OleDbCommand(qry, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        setup.AnalysisParams = new AnalysisParamsType();
                        setup.AnalysisParams.Wavelength = int.Parse(setup.WavelengthID.Split(' ')[0]);
                        if (reader.Read())
                        {
                            setup.AnalysisParams.MinAttenuation = 1000.0D / SecToMeters(1 / (double)reader["minAtten"], gi);
                            setup.AnalysisParams.MaxAttenuation = 1000.0D / SecToMeters(1 / (double)reader["maxAtten"], gi);
                            setup.AnalysisParams.Sensitivity = (double)reader["sensitivity"];
                            setup.AnalysisParams.EndLossThresh = (double)reader["endLossThresh"];
                            setup.AnalysisParams.WindowStartOffset = SecToMeters((double)reader["bottomEventGuardLen"], gi);
                            setup.AnalysisParams.WindowStopOffset = SecToMeters((double)reader["topEventGuardLen"], gi);
                        }
                        else
                        {
                            throw new ApplicationException("Database entry for analysis parameters not found.");
                        }
                    }
                }

                qry = "SELECT * FROM TestParametersTbl WHERE wavelengthId = " + wavelengthKey.ToString();
                using (var cmd = new OleDbCommand(qry, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            setup.AnalysisParams.WindowStepAround = (bool)reader["stepAroundEvents"];
                        }
                        else
                        {
                            throw new ApplicationException("Database entry for analysis parameters not found.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Read a complete set of limits from database for a particular wavelength.
        /// </summary>
        /// <param name="setup">The <see cref="AnalysisWavelengthSetupType"/> to which the limits will be added.</param>
        /// <param name="wavelengthKey">The <c>wavelengthId</c>primary key value for the wavelength limits in the <c>TestLimitsTbl</c>.</param>
        /// <param name="gi">The group index of the fiber at the wavelength in question.</param>
        /// <param name="directionality"><see cref="DirectionalityAspectType.Unidirectional"/> or <see cref="DirectionalityAspectType.Bidirectional"/></param>
        private void ReadLimitSet(AnalysisWavelengthSetupType setup, int wavelengthKey, double gi, DirectionalityAspectType directionality)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string qry = "SELECT * FROM TestLimitsTbl WHERE wavelengthId = " + wavelengthKey.ToString();
                using (var cmd = new OleDbCommand(qry, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        // create new LimitsSetType
                        setup.Limits[(int)directionality] = new LimitSetType();
                        LimitSetType limitSet = setup.Limits[(int)directionality] as LimitSetType;
                        limitSet.Aspect = directionality;
                        if (reader.Read())
                        {
                            int key;
                            double scale = SecToMeters(1, gi);

                            // Limits are funny in the database...only some of them have separate unidirectional
                            // and bidirectional values, while OASYS treats them as two distinct sets.
                            if (directionality == (int)DirectionalityAspectType.Unidirectional)
                            {
                                key = (int)reader["uniAttenCoef"];
                                limitSet.Attenuation = ReadLimits(key, 1000.0 / scale);
                                key = (int)reader["uniEventLoss"];
                                limitSet.EventLoss = ReadLimits(key);
                            }
                            else
                            {
                                key = (int)reader["biAttenCoef"];
                                limitSet.Attenuation = ReadLimits(key, 1000.0D / scale);
                                key = (int)reader["biEventLoss"];
                                limitSet.EventLoss = ReadLimits(key);
                            }

                            key = (int)reader["bufferLoss"];
                            limitSet.BufferLoss = ReadLimits(key);
                            key = (int)reader["bufferRefl"];
                            limitSet.BufferReflectance = ReadLimits(key);
                            key = (int)reader["eventRefl"];
                            limitSet.EventReflectance = ReadLimits(key);
                            key = (int)reader["fiberLength"];
                            limitSet.FiberLength = ReadLimits(key);
                            key = (int)reader["lsaDeviation"];
                            limitSet.LsaPointDeviation = ReadLimits(key);
                            key = (int)reader["slidingWindowAtten"];
                            limitSet.WindowAttenuation = ReadLimits(key, 1000.0D / scale);
                            key = (int)reader["slidingWindowUni"];
                            limitSet.WindowUniformity = ReadLimits(key, 1000.0D / scale);
                        }
                        else
                        {
                            throw new ApplicationException("Database entry for limit set not found.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads a set of Flag/Fail limits from the database into a <see cref="LimitsType"/> object.
        /// </summary>
        /// <param name="limitKey">The <c>numLimitId</c> primary key of the row to look up in the <c>NumericLimitTbl</c>.</param>
        /// <param name="scaleFactor">Optional scale factor if necessary to convert units.</param>
        /// <returns>The requested <see cref="LimitsType"/>.</returns>
        private LimitsType ReadLimits(int limitKey, double scaleFactor = 1)
        {
            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                string qry = "SELECT * FROM NumericLimitTbl WHERE numLimitId = " + limitKey.ToString();
                using (var cmd = new OleDbCommand(qry, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var limits = new LimitsType();
                            limits.MaxFail = scaleFactor * (double)reader["maxFail"];
                            limits.MaxFlag = scaleFactor * (double)reader["maxFlag"];
                            limits.MinFail = scaleFactor * (double)reader["minFail"];
                            limits.MinFlag = scaleFactor * (double)reader["minFlag"];
                            limits.MaxFailSpecified = (bool)reader["maxFailEnabled"];
                            limits.MaxFlagSpecified = (bool)reader["maxFlagEnabled"];
                            limits.MinFailSpecified = (bool)reader["minFailEnabled"];
                            limits.MinFlagSpecified = (bool)reader["minFlagEnabled"];
                            return limits;
                        }
                        else
                        {
                            throw new ApplicationException("Database entry for limits not found.");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Convert from time in seconds to distance in meters.
        /// </summary>
        /// <param name="t">The time in seconds.</param>
        /// <param name="gi">The Group Index of the fiber at a particular wavelength.</param>
        /// <returns>The distance based on speed of light in the fiber at a particular wavelength.</returns>
        private double SecToMeters(double t, double gi)
        {
            return 0.5 * speedOfLight * t / gi;
        }
        #endregion
    }
}
