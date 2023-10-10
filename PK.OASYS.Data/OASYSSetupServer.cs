//-----------------------------------------------------------------------
// <copyright file="OASYSSetupServer.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.IO
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml;
    using System.Xml.Serialization;

    /// <summary>
    /// Simple data access layer for OASYS.net saved Fiber Type setups <see cref="FiberTypeType"/>, OTDR Setups <see cref="OTDRSetupType"/>,
    /// Ribbon Setups <see cref="RibbonTypeType"/>, and Analysis Setups <see cref="AnalysisSetupType"/>.
    /// Recommended usage: Call <c>LoadAllSetupIDs</c>. This populates four read-only string collections <see cref="ReadOnlyCollection{T}"/>:
    /// <c>FiberTypeIDs</c>, <c>OTDRSetupIDs</c>, <c>AnalysisSetupIDs</c>, and <c>RibbonTypeIDs</c> containing the IDs of all
    /// of the saved setup in OASYS.net files and the OTDR8000DB Analysis database. When needed, call <c>GetSetup</c>,
    /// passing in the ID of the setup you want to load, and specifying the setup type.
    /// </summary>
    public class OASYSSetupServer
    {
        /// <summary>
        /// Provides access to the OTDR8000DB Analysis database settings.
        /// </summary>
        private static readonly AnalysisDatabase AnalysisDB = new AnalysisDatabase(OASYSPaths.UDLFile);

        /// <summary>
        /// Gets a read-only collection of all Fiber Type setups.
        /// </summary>
        public static ReadOnlyCollection<string> FiberTypeIDs { get; private set; }

        /// <summary>
        /// Gets a read-only collection of all OTDR Setups
        /// </summary>
        public static ReadOnlyCollection<string> OTDRSetupIDs { get; private set; }

        /// <summary>
        /// Gets a read-only collection of all Analysis Setups
        /// </summary>
        public static ReadOnlyCollection<string> AnalysisSetupIDs { get; private set; }

        /// <summary>
        /// Gets a read-only collection of all Ribbon Type Setups
        /// </summary>
        public static ReadOnlyCollection<string> RibbonTypeIDs { get; private set; }

        /// <summary>
        /// Gets a Dictionary of all saved <see cref="FiberTypeType"/> setups keyed by their ID strings
        /// </summary>
        /// <remarks>
        /// Not loaded by default. A client must first call and await
        /// <c>LoadAllCableMeasurementPrameters</c> from an <c>async</c> method.
        /// </remarks>
        public static IReadOnlyDictionary<string, FiberTypeType> FiberTypes { get; private set; }

        /// <summary>
        /// Gets a Dictionary of all saved <see cref="OTDRSetupType"/> setups keyed by their ID strings
        /// </summary>
        /// <remarks>
        /// Not loaded by default. A client must first call and await
        /// <c>LoadAllCableMeasurementPrameters</c> from an <c>async</c> method.
        /// </remarks>
        public static IReadOnlyDictionary<string, OTDRSetupType> OTDRSetups { get; private set; }

        /// <summary>
        /// Gets a Dictionary of all saved <see cref="AnalysisSetupType"/> setups keyed by their ID strings
        /// </summary>
        /// <remarks>
        /// Not loaded by default. A client must first call and await
        /// <c>LoadAllCableMeasurementPrameters</c> from an <c>async</c> method.
        /// </remarks>
        public static IReadOnlyDictionary<string, AnalysisSetupType> AnalysisSetups { get; private set; }

        /// <summary>
        /// Gets a Dictionary of all saved <see cref="RibbonTypeType"/> setups keyed by their ID strings
        /// </summary>
        /// <remarks>
        /// Not loaded by default. A client must first call and await
        /// <c>LoadAllCableMeasurementPrameters</c> from an <c>async</c> method.
        /// </remarks>
        public static IReadOnlyDictionary<string, RibbonTypeType> RibbonTypes { get; private set; }

        /// <summary>
        /// Fills the AnalysisSetupIDs, FiberTypeIDs, OTDRSetupIDs, and RibbonTypeIDs collections
        /// from the saved setups in the OTDR8000DB analysis database and the <c>*.pkfbt</c>,
        /// <c>*.pkosp</c>, and <c>*.pkrbt</c> files.
        /// </summary>
        public static void LoadAllSetupIDs()
        {
            // Analysis setup IDs are retrieved from the database
            AnalysisSetupIDs = AnalysisDB.AvailableAnalysisEntries().ToList().AsReadOnly();

            // Use RegEx to find all setup files
            var regex = new Regex(@"\.pk[ofr](?(?<=o)sp|bt)", RegexOptions.IgnoreCase);
            var setupFiles = Directory.GetFiles(OASYSPaths.OasysFilesDirectory, "*.pk*").
                                Where(file => regex.IsMatch(Path.GetExtension(file)));

            var otdrSetupIDs = new List<string>();
            var fiberTypeSetupIDs = new List<string>();
            var ribbonTypeSetupIDs = new List<string>();
            Parallel.ForEach(
                setupFiles,
                file =>
                    {
                        var id = GetSetupID(Path.GetFileName(file));
                        switch (Path.GetExtension(file).ToLower())
                        {
                            case ".pkfbt":
                                fiberTypeSetupIDs.Add(id);
                                break;
                            case ".pkrbt":
                                ribbonTypeSetupIDs.Add(id);
                                break;
                            case ".pkosp":
                                otdrSetupIDs.Add(id);
                                break;
                            default:
                                throw new InvalidOperationException("Invalid file found through filters. " +
                                    "Should never get here!");
                        }
                    });

            FiberTypeIDs = fiberTypeSetupIDs.AsReadOnly();
            OTDRSetupIDs = otdrSetupIDs.AsReadOnly();
            RibbonTypeIDs = ribbonTypeSetupIDs.AsReadOnly();
        }

        /// <summary>
        /// Asynchronously fills the AnalysisSetups, FiberTypes, OTDRSetups, and RibbonTypes
        /// dictionaries.
        /// </summary>
        /// <returns>An await-able Task.</returns>
        public static async Task LoadAllSetupsAsync()
        {
            // Analysis setup IDs are retrieved from the database
            AnalysisSetupIDs = AnalysisDB.AvailableAnalysisEntries().ToList().AsReadOnly();

            var regex = new Regex(@"\.pk[ofr](?(?<=o)sp|bt)", RegexOptions.IgnoreCase);
            var setupFiles = Directory.GetFiles(OASYSPaths.OasysFilesDirectory, "*.pk*").
                                Where(file => regex.IsMatch(Path.GetExtension(file)));

            var otdrSetupIDs = new Dictionary<string, OTDRSetupType>();
            var fiberTypeSetupIDs = new Dictionary<string, FiberTypeType>();
            var ribbonTypeSetupIDs = new Dictionary<string, RibbonTypeType>();

            Parallel.ForEach(
                setupFiles,
                file =>
                    {
                        var id = GetSetupID(Path.GetFileName(file));
                        switch (Path.GetExtension(file).ToLower())
                        {
                            case ".pkfbt":
                                fiberTypeSetupIDs.Add(id, GetSetup<FiberTypeType>(id));
                                break;
                            case ".pkrbt":
                                ribbonTypeSetupIDs.Add(id, GetSetup<RibbonTypeType>(id));
                                break;
                            case ".pkosp":
                                otdrSetupIDs.Add(id, GetSetup<OTDRSetupType>(id));
                                break;
                            default:
                                throw new InvalidOperationException("Invalid file found through filters. Should never get here!");
                        }
                    });

            AnalysisSetups = await GetAllAnalysisSetupsAsync();
            FiberTypes = fiberTypeSetupIDs;
            OTDRSetups = otdrSetupIDs;
            RibbonTypes = ribbonTypeSetupIDs;
        }

        /// <summary>
        /// Get an OASYS.net <see cref="IDTaggedSetupType"/> by its ID.
        /// </summary>
        /// <typeparam name="T">The specific <see cref="IDTaggedSetupType"/> sub-type to deserialize</typeparam>
        /// <param name="id">The id of the setup to retrieve.</param>
        /// <returns>An <see cref="IDTaggedSetupType"/> object.</returns>
        public static T GetSetup<T>(string id) where T : IDTaggedSetupType
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T setup;

            if (typeof(T) == typeof(AnalysisSetupType))
            {
                int key = AnalysisDB.DatabaseID(id);
                setup = AnalysisDB.GetAnalysisSetup(key) as T;
            }
            else
            {
                string fileName = id;
                if (typeof(T) == typeof(FiberTypeType))
                {
                    fileName += ".pkfbt";
                }
                else if (typeof(T) == typeof(OTDRSetupType))
                {
                    fileName += ".pkosp";
                }
                else if (typeof(T) == typeof(RibbonTypeType))
                {
                    fileName += ".pkrbt";
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T), OASYSPaths.OasysXMLNamespace);
                var path = Path.Combine(OASYSPaths.OasysFilesDirectory, fileName);
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    setup = serializer.Deserialize(stream) as T;

                    // check for a ID / file name mismatch
                    if (string.Compare(setup.ID, Path.GetFileNameWithoutExtension(fileName), true) != 0)
                    {
                        var message = string.Format(
                            "Setup type ID {0} does not match file name {1}, skipping...",
                            setup.ID,
                            Path.GetFileName(fileName));

                        throw new MismatchedSetupNameException(message);
                    }
                }
            }

            return setup;
        }

        /// <summary>
        /// Gets the ID field from a <c>*.pkfbt</c>, <c>*.pkosp</c>, and <c>*.pkrbt</c> file.
        /// </summary>
        /// <param name="fileName">The setup file name with extension.</param>
        /// <returns>The ID of the setup.</returns>
        public static string GetSetupID(string fileName)
        {
            if (fileName is null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            var doc = new XmlDocument();
            var path = Path.Combine(OASYSPaths.OasysFilesDirectory, fileName);
            doc.Load(path);
            var id = doc.DocumentElement.GetAttribute("ID");
            if (id != Path.GetFileNameWithoutExtension(fileName))
            {
                var msg = string.Format(
                    "The ID ({0}) does not match the file name of setup file {1}",
                    id,
                    fileName);
                // Uncomment this line to enforce the file name = setup name rule
                //throw new MismatchedSetupNameException(msg);
            }

            return id;
        }

        /// <summary>
        /// Reads the 8000 database and reads in all analysis limits settings and updates the Analysis combo box.
        /// </summary>
        /// <returns>A Dictionary of <see cref="AnalysisSetupType"/>s keyed by their ID strings.</returns>
        private static async Task<Dictionary<string, AnalysisSetupType>> GetAllAnalysisSetupsAsync()
        {
            var dict = new Dictionary<string, AnalysisSetupType>();

            // NOTE: The database connection string is found automatically by finding
            // the 8000 OTDR UDL file declared in OASYS.xml configuration file
            // See the function 'GetDBConnectionString' to see how the connection string is found.
            var database = new AnalysisDatabase(OASYSPaths.UDLFile);
            AnalysisSetupType limits = null;

            foreach (string entry in database.AvailableAnalysisEntries())
            {
                await Task.Run(() =>
                {
                    limits = database.GetAnalysisSetup(database.DatabaseID(entry));
                    limits.ID = entry;
                });
                dict.Add(entry, limits);
            }

            return dict;
        }
    }
}
