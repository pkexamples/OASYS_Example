//-----------------------------------------------------------------------
// <copyright file="OASYSPaths.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------

namespace PhotonKinetics.OASYS.IO
{
    using System;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Provides access to OASYS.net configuration file and directory paths.
    /// </summary>
    public static class OASYSPaths
    {
        /// <summary>
        /// XML namespace for OASYS.net XML files.
        /// </summary>
        public const string OasysXMLNamespace = "http://pkinetics.com/oasys.net";

        /// <summary>
        /// Path to the OASYS.net configuration files directory.
        /// </summary>
        public static readonly string OasysFilesDirectory;

        /// <summary>
        /// Path to the OASYS.net cable result data directory.
        /// </summary>
        public static readonly string DataDirectory;

        /// <summary>
        /// Path to the pre-defined OASYS.net cable definition files directory.
        /// </summary>
        public static readonly string CableFilesDirectory;

        /// <summary>
        /// Path to the <c>OTDR.udl</c> file, which contains the OTDR8000DB analysis database connection string.
        /// </summary>
        public static readonly string UDLFile;

        /// <summary>
        /// Path to the <c>LocalStation.xml</c> file.
        /// </summary>
        public static readonly string LocalStationFile;

        /// <summary>
        /// Path to the <c>OASYS.xml</c> configuration file.
        /// </summary>
        public static readonly string OASYSConfigFile;

        /// <summary>
        /// Initializes static members of the <see cref="OASYSPaths"/> class.
        /// </summary>
        static OASYSPaths()
        {
            var localSettings = new XmlDocument();
            var oasysXML = new XmlDocument();
            var nameTable = new NameTable();
            nameTable.Add(OasysXMLNamespace);
            var namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("pk", OasysXMLNamespace);
            LocalStationFile = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                @"Photon Kinetics\OASYS\LocalStation.xml");
            localSettings.Load(LocalStationFile);

            // Now get the config directory name and load OASYS.xml
            OasysFilesDirectory = localSettings.DocumentElement.SelectSingleNode(
                "pk:ConfigFilePath", namespaceManager).InnerText;

            OASYSConfigFile = Path.Combine(OasysFilesDirectory, "OASYS.xml");
            oasysXML.Load(OASYSConfigFile);

            // ...And find the two items we need there: the base data directory and the
            // database UDL file path
            DataDirectory = oasysXML.DocumentElement.SelectSingleNode(
                "pk:DataPath", namespaceManager).InnerText;
            CableFilesDirectory = oasysXML.DocumentElement.SelectSingleNode(
                "pk:PreDefinedCableFilesDir", namespaceManager).InnerText;
            UDLFile = oasysXML.DocumentElement.SelectSingleNode(
                "pk:PKOTDR_udlFile", namespaceManager).InnerText;
        }
    }
}
