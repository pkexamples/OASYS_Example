//-----------------------------------------------------------------------
// <copyright file="SimpleCableData.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.Examples
{
    /// <summary>
    /// Business object containing simple cable information
    /// </summary>
    internal class SimpleCableData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleCableData"/> class.
        /// </summary>
        internal SimpleCableData()
        {
            this.CableID = "New Cable";
            this.CableType = CableTypes.Loose;
            this.NumberOfFibers = 1;
            this.NumberOfTubes = 1;
            this.NumberOfRibbons = 1;
            this.CableLength = 8000d;
        }

        /// <summary>
        /// Indicates whether the pre-processor will have ribbons or loose fibers
        /// </summary>
        internal enum CableTypes
        {
            /// <summary>
            /// Indicates the cable has only loose fibers within tubes
            /// </summary>
            Loose,

            /// <summary>
            /// Indicates the cable has ribbons within tubes
            /// </summary>
            Ribbon
        }

        /// <summary>
        /// Gets or sets a string representing the root level cable identifier
        /// </summary>
        internal string CableID { get; set; }

        /// <summary>
        /// Gets or sets a string representing the operator's identification
        /// </summary>
        internal string OperatorID { get; set; }

        /// <summary>
        /// Gets or sets the number of fibers in each tube or ribbon
        /// </summary>
        internal int NumberOfFibers { get; set; }

        /// <summary>
        /// Gets or sets the number of tubes in the cable
        /// </summary>
        internal int NumberOfTubes { get; set; }

        /// <summary>
        /// Gets or sets the number of ribbons in each tube (if cableType is CableTypes.Ribbon)
        /// </summary>
        internal int NumberOfRibbons { get; set; }

        /// <summary>
        /// Gets or sets the ribbon / loose tube cable type
        /// </summary>
        internal CableTypes CableType { get; set; }

        /// <summary>
        /// Gets or sets the cable length in meters
        /// </summary>
        internal double CableLength { get; set; }

        /// <summary>
        /// Gets or sets the ID of the selected Fiber Type setup
        /// </summary>
        internal string FiberTypeID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the selected Ribbon Type setup
        /// </summary>
        internal string RibbonTypeID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the selected OTDR setup
        /// </summary>
        internal string OtdrSetupID { get; set; }

        /// <summary>
        /// Gets or sets the ID of the selected Analysis setup
        /// </summary>
        internal string AnalysisSetupID { get; set; }
    }
}
