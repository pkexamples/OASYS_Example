//-----------------------------------------------------------------------
// <copyright file="SimpleCableWriter.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.Examples
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using PhotonKinetics.OASYS.IO;

    /// <summary>
    /// Writes a cable definition file using inputs from a simple pre-processor
    /// </summary>
    internal class SimpleCableWriter
    {
        /// <summary>
        /// Colors to be used as identifiers to the cable elements
        /// </summary>
        private readonly string[] colors =
            new[]
            {
                "Blue", "Orange", "Green", "Brown", "Slate", "White",
                "Red", "Black", "Yellow", "Violet", "Rose", "Aqua",
                "Bl Str", "Or Str", "Gr Str", "Br Str", "Sl Str", "Wh Str",
                "Rd Str", "Bk Str", "Yl Str", "Vi Str", "Ro Str", "Aq Str"
            };

        /// <summary>
        /// The <see cref="CableSetupType"/> object this object is responsible for building
        /// </summary>
        private readonly CableSystemType newCable;

        /// <summary>
        /// The input data required to build the cable.
        /// </summary>
        private readonly SimpleCableData preProcData;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCableWriter"/> class.
        /// </summary>
        /// <param name="data">A <see cref="SimpleCableData"/> instance.</param>
        internal SimpleCableWriter(SimpleCableData data)
        {
            this.newCable = new CableSystemType();
            this.preProcData = data;
        }

        /// <summary>
        /// Writes the <c>.pkcbd</c> file to the output path specified in the OASYS.net
        /// Local Station Settings.
        /// </summary>
        internal void WriteCableFile()
        {
            this.SetupCableObjectParts();
            this.CreateNewUserLabels();
            this.FormCableStructure();
            this.SaveCableDefinitionFile();
        }

        #region Methods for cable building
        /// <summary>
        /// Creates and root cable node and its high-level properties
        /// </summary>
        private void SetupCableObjectParts()
        {
            this.newCable.Item = new GroupType();
            this.newCable.ItemElementName = ItemChoiceType.Cable;
            this.newCable.Item.CableSetup = new CableSetupType();
            this.newCable.Item.CableInfo = new CableInfoType();
            this.newCable.Item.ID = this.preProcData.CableID;
            this.newCable.Item.Desc = "Cable";
            this.newCable.Item.CableInfo.FiberLength = this.preProcData.CableLength;
        }

        /// <summary>
        /// Build the cable structure and assign measurement parameters
        /// </summary>
        /// <remarks>
        /// The cable measurement parameters are hierarchical and can be assigned at any
        /// level in the cable structure.  This program only assigns measurement
        /// parameters at the root cable level, but shows how parameters can be assigned
        /// at any fiber group or individual fiber level in commented lines. Any
        /// parameters assigned at a lower level will override parameters assigned at a
        /// higher level. If you assign measurement parameters to a fiber, then those
        /// fiber settings will be used and override anything assigned to the tube level,
        /// but for that fiber only.  For a cable definition to be valid for measurement,
        /// then at a minimum all fiber nodes must either inherit measurement parameters
        /// from a higher level, and any fiber nodes that do not inherit parameters from
        /// a higher level group node must have parameters assigned to them explicitly.
        /// Further validation would check that all of the parameters provided are
        /// compatible with the OTDR hardware, for example available wavelengths, etc.
        /// </remarks>
        private void FormCableStructure()
        {
            // In this example, the cable only has one each of AnalysisSetupType,
            // FiberTypeType, and OTDRSetupType, although there is no limit to the length of the
            // AnalysisSetups, FiberTypes, and OTDRSetups arrays.
            var cableSetup = this.newCable.Item.CableSetup;

            // The cable setup node contains all parameter setups to be used in the
            // measurement.  Their unique string identifiers are then used in the cable
            // structure nodes to refer to the setups.
            // Get each of these from the OASYSSetupServer and put them in the corresponding arrays
            // in the cableSetup.
            var analysisSetup = OASYSSetupServer.GetSetup<AnalysisSetupType>(this.preProcData.AnalysisSetupID);
            cableSetup.AnalysisSetups = new AnalysisSetupType[1] { analysisSetup };

            var fiberType = OASYSSetupServer.GetSetup<FiberTypeType>(this.preProcData.FiberTypeID);
            cableSetup.FiberTypes = new FiberTypeType[1] { fiberType };

            var otdrSetup = OASYSSetupServer.GetSetup<OTDRSetupType>(this.preProcData.OtdrSetupID);
            cableSetup.OTDRSetups = new OTDRSetupType[1] { otdrSetup };

            // If "Ribbon" was selected, add a RibbonTypeType to the setup
            if (this.preProcData.CableType == SimpleCableData.CableTypes.Ribbon)
            {
                var ribbonType = OASYSSetupServer.GetSetup<RibbonTypeType>(this.preProcData.RibbonTypeID);
                cableSetup.RibbonTypes = new RibbonTypeType[1] { ribbonType };
            }
            else
            {
                cableSetup.RibbonTypes = new RibbonTypeType[0];
            }

            // Set the most common values for measurement parameters at the root level.
            // They will be inherited by all contained elements except where they are
            // overridden at a lower level from which they will in turn be inherited by
            // contained elements. Note that these properties refer to the Name of each
            // setup, not the actual object.
            this.newCable.Item.AnalysisSetup = this.preProcData.AnalysisSetupID;
            this.newCable.Item.OTDRSetup = this.preProcData.OtdrSetupID;
            this.newCable.Item.FiberType = this.preProcData.FiberTypeID;
            if (this.preProcData.CableType == SimpleCableData.CableTypes.Ribbon)
            {
                this.newCable.Item.RibbonType = this.preProcData.RibbonTypeID;
            }

            // Cable structure building starts here
            // Add tubes to the cable
            for (int tubeIndex = 0; tubeIndex <= this.preProcData.NumberOfTubes - 1; tubeIndex++)
            {
                // Take any existing cable nodes, copy in to current cable nodes
                var cable = (GroupType)this.newCable.Item;
                var cableNodes = cable.Items;
                cable.Items = new CableNodeType[tubeIndex + 1];
                if (cableNodes != null)
                {
                    Array.Copy(cableNodes, cable.Items, Math.Min(tubeIndex + 1, cableNodes.Length));
                }

                // Create the tube
                cable.Items[tubeIndex] = new GroupType();
                cable.Items[tubeIndex].ID = this.colors[tubeIndex % this.colors.Length];
                cable.Items[tubeIndex].Desc = "Tube";

                // OPTIONAL tube level assignment of measurement parameters
                // These would override any settings applied at a higher level
                ////cable.Items[tubeIndex].AnalysisSetup = analysisSetupID;
                ////cable.Items[tubeIndex].FiberType = fiberTypeID;
                ////cable.Items[tubeIndex].OTDRSetup = otdrSetupID;
                ////if (this.preProcData.CableType == SimpleCableData.CableTypes.Ribbon)
                ////{
                ////    cable.Items[tubeIndex].RibbonType = this.preProcData.SelectedRibbonTypeID;
                ////}

                // If cable has ribbons
                if (this.preProcData.CableType == SimpleCableData.CableTypes.Ribbon)
                {
                    // Add ribbons to the tube
                    var tube = (GroupType)cable.Items[tubeIndex];
                    for (int ribbonIndex = 0; ribbonIndex <= this.preProcData.NumberOfRibbons - 1; ribbonIndex++)
                    {
                        var oldRibbons = tube.Items;
                        tube.Items = new CableNodeType[ribbonIndex + 1];
                        if (oldRibbons != null)
                        {
                            Array.Copy(oldRibbons, tube.Items, Math.Min(ribbonIndex + 1, oldRibbons.Length));
                        }

                        // Create new ribbon
                        tube.Items[ribbonIndex] = new GroupType();
                        tube.Items[ribbonIndex].ID = this.colors[ribbonIndex % this.colors.Length];
                        tube.Items[ribbonIndex].Desc = "Ribbon";
                        tube.Items[ribbonIndex].RibbonType = this.preProcData.RibbonTypeID;

                        // OPTIONAL ribbon level assignment of measurement parameters
                        // These would override any settings applied at a higher level
                        ////tube.Items[ribbonIndex].AnalysisSetup = analysisSetupID;
                        ////tube.Items[ribbonIndex].FiberType = fiberTypeID;
                        ////tube.Items[ribbonIndex].OTDRSetup = otdrSetupID;
                        ////tube.Items[ribbonIndex].RibbonType = ribbonTypeID;

                        // Add fibers to the ribbon
                        var ribbon = (GroupType)tube.Items[ribbonIndex];
                        for (int fiberIndex = 0; fiberIndex <= this.preProcData.NumberOfFibers - 1; fiberIndex++)
                        {
                            var oldFibers = ribbon.Items;
                            ribbon.Items = new CableNodeType[fiberIndex + 1];
                            if (oldFibers != null)
                            {
                                Array.Copy(oldFibers, ribbon.Items, Math.Min(fiberIndex + 1, oldFibers.Length));
                            }

                            // Create new fiber
                            ribbon.Items[fiberIndex] = new FiberType();
                            ribbon.Items[fiberIndex].ID = this.colors[fiberIndex % this.colors.Length];
                            ribbon.Items[fiberIndex].Desc = "Fiber";

                            // OPTIONAL fiber level assignment of measurement parameters
                            // These would override any settings applied at a higher level
                            ////ribbon.Items[fiberIndex].AnalysisSetup = analysisSetupID;
                            ////ribbon.Items[fiberIndex].FiberType = fiberTypeID;
                            ////ribbon.Items[fiberIndex].OTDRSetup = otdrSetupID;
                        }
                    }
                }
                else
                {
                    // cable does not have ribbons, so add fibers directly to tubes
                    var tube = (GroupType)cable.Items[tubeIndex];
                    for (int fiberIndex = 0; fiberIndex <= this.preProcData.NumberOfFibers - 1; fiberIndex++)
                    {
                        var oldFibers = tube.Items;
                        tube.Items = new CableNodeType[fiberIndex + 1];
                        if (oldFibers != null)
                        {
                            Array.Copy(oldFibers, tube.Items, Math.Min(fiberIndex + 1, oldFibers.Length));
                        }

                        // Create new fiber
                        tube.Items[fiberIndex] = new FiberType();
                        tube.Items[fiberIndex].ID = this.colors[fiberIndex % this.colors.Length];
                        tube.Items[fiberIndex].Desc = "Fiber";

                        // OPTIONAL fiber level assignment of measurement parameters
                        // These would override any settings applied at a higher level
                        ////tube.Items[fiberIndex].AnalysisSetup = analysisSetupID;
                        ////tube.Items[fiberIndex].FiberType = fiberTypeID;
                        ////tube.Items[fiberIndex].OTDRSetup = otdrSetupID;
                    }
                }
            }
        }

        /// <summary>
        /// Creates the FiberID entries
        /// </summary>
        /// <remarks>
        /// <para>
        /// NOTE 1: It is not necessary to provide a "value" (the response to a user label) if it is desired
        /// for the operator to input a response to the user prompt at measurement time.
        /// </para>
        /// <para>
        /// NOTE 2: The first user label (i.e .FiberID[0]) always defines the "Cable ID" which is used as the
        /// data output directory name and cable results file name.  The FiberID[0].Label can be set to any desired
        /// text string, but whatever the FiberID[0].Value is set to will always be used as the data output
        /// directory name and cable results file name.
        /// </para>
        /// </remarks>
        private void CreateNewUserLabels()
        {
            // Create two new user labels and set their values.
            {
                var info = this.newCable.Item.CableInfo;

                // Create new array with number of desired items
                info.FiberID = new FiberIDType[2];

                info.FiberID[0] = new FiberIDType();
                info.FiberID[0].Label = "Cable ID";
                info.FiberID[0].Value = this.preProcData.CableID;

                info.FiberID[1] = new FiberIDType();
                info.FiberID[1].Label = "Operator ID";
                info.FiberID[1].Value = this.preProcData.OperatorID;
            }
        }

        /// <summary>
        /// Writes the cable definition file
        /// </summary>
        private void SaveCableDefinitionFile()
        {
            var cableSerializer = new XmlSerializer(typeof(CableSystemType));
            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, OASYSPaths.OasysXMLNamespace);
            using (var stream = new FileStream(Environment.GetCommandLineArgs()[1], FileMode.Create))
            {
                cableSerializer.Serialize(stream, this.newCable, xmlns);
            }
        }
        #endregion
    }
}
