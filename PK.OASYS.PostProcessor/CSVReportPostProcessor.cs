//-----------------------------------------------------------------------
// <copyright file="CSVReportPostProcessor.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.Examples
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using PhotonKinetics.OASYS;

    /// <summary>
    /// An OASYS.net PostProcessor that generates a CSV summary report.
    /// </summary>
    public class CSVReportPostProcessor
    {
        /// <summary>
        /// A <c>TextWriter</c> with which to write the report.
        /// </summary>
        private readonly TextWriter writer;

        /// <summary>
        /// A field into which the cable result data will be deserialized
        /// </summary>
        private readonly CableSystemType cable;

        /// <summary>
        /// A flag to specify whether or not the center wavelength value(s) will be included in the report.
        /// </summary>
        private bool printCenterWavelength;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVReportPostProcessor"/> class.
        /// </summary>
        /// <param name="inputStream">A <c>Stream</c> containing the serialized PKCBR cable result data.</param>
        /// <param name="outputStream">A <c>TextWriter</c> with which to write the report.</param>
        public CSVReportPostProcessor(Stream inputStream, StreamWriter outputStream)
        {
            writer = outputStream as TextWriter;
            XmlSerializer ser = new XmlSerializer(typeof(CableSystemType));
            cable = ser.Deserialize(inputStream) as CableSystemType;
        }

        /// <summary>
        /// Generates a CSV report from the cable result data.
        /// </summary>
        public void GenerateReport()
        {
            WriteHeader();
            WalkCableTree(cable.Item);
        }

        /// <summary>
        /// Writes the report header.
        /// </summary>
        private void WriteHeader()
        {
            var info = cable.Item.CableInfo;
            writer.WriteLine("Station ID," + info.StationID);
            foreach (FiberIDType id in info.FiberID)
            {
                writer.WriteLine(id.Label + "," + id.Value);
            }

            writer.WriteLine("Cable Sequential," + info.CableSequential.ToString());
            writer.WriteLine("Entered Length(m)," + info.FiberLength.ToString());
            writer.WriteLine("Measurement Date/Time," + info.TestDateTime.ToString());
            writer.WriteLine("Selected Cable," + cable.Item.ID);
            writer.WriteLine("Operator Name" + "," + info.OperatorName);

            // Correct for Center Wavelength?
            printCenterWavelength = false;
            foreach (OTDRSetupType otdrSetup in cable.Item.CableSetup.OTDRSetups)
            {
                foreach (OTDRWavelengthSetupType waveSetup in otdrSetup.OTDRWavelengthSetup)
                {
                    if (waveSetup.CorrectForCenterWavelength)
                    {
                        printCenterWavelength = true;
                    }
                }
            }

            if (printCenterWavelength)
            {
                writer.WriteLine("FiberInfo,Direction,Wavelength," +
                    "Center Wavelength,RefIndex,Status,Atten,Length,CouplingLoss," +
                    "MaxWindowUniformity,MaxWindowUniformityLoc");
            }
            else
            {
                writer.WriteLine("FiberInfo,Direction,Wavelength,RefIndex,Status," +
                    "Atten,Length,CouplingLoss,MaxWindowUniformity," +
                    "MaxWindowUniformityLoc");
            }
        }

        /// <summary>
        /// Helper method to recursively crawl through the cable structure, which contains
        /// <see cref="GroupType"/> nodes, which contain other <c>GroupType</c> nodes,
        /// and <see cref="FiberType"/> nodes, which are the "leaves" of the cable. The
        /// method finds the <see cref="AnalysisSetupType"/>, <see cref="OTDRSetupType"/>,
        /// <see cref="FiberTypeType"/>, and <see cref="RibbonTypeType"/> attached to each
        /// node and passes any found recursively through to nested nodes, so that when
        /// <c>OutputNode</c> is called on a fiber node, that method has access to the
        /// setup fragments applied to that node.
        /// </summary>
        /// <param name="node">A <see cref="GroupType"/> or a <see cref="FiberType"/></param>
        /// <param name="analysisSetups">A name of an analysis setup or null.</param>
        /// <param name="otdrSetups">A name of an OTDR setup or null.</param>
        /// <param name="fiberType">A name of a fiber type setup or null.</param>
        /// <param name="ribbonType">The optional <see cref="RibbonTypeType"/> settings applied to the fiber.</param>
        private void WalkCableTree(
            CableNodeType node,
            AnalysisSetupType analysisSetups = null,
            OTDRSetupType otdrSetups = null,
            FiberTypeType fiberType = null,
            RibbonTypeType ribbonType = null)
        {
            if (!string.IsNullOrEmpty(node.AnalysisSetup))
            {
                analysisSetups = FindAnalysisSetups(node);
            }

            if (!string.IsNullOrEmpty(node.OTDRSetup))
            {
                otdrSetups = FindOTDRSetups(node);
            }

            if (!string.IsNullOrEmpty(node.FiberType))
            {
                fiberType = FindFiberType(node);
            }

            if (!string.IsNullOrEmpty(node.RibbonType))
            {
                ribbonType = FindRibbonType(node);
            }

            if (node is FiberType)
            {
                OutputNode((FiberType)node, analysisSetups, otdrSetups, fiberType, ribbonType);
            }
            else if (node is GroupType)
            {
                foreach (CableNodeType child in ((GroupType)node).Items)
                {
                    WalkCableTree(child, analysisSetups, otdrSetups, fiberType, ribbonType);
                }
            }
        }

        /// <summary>
        /// Writes the report data from a single fiber.
        /// </summary>
        /// <param name="node">A <see cref="FiberType"/> node containing fiber result data.</param>
        /// <param name="analysisSetup">The <see cref="AnalysisSetupType"/> settings applied to the fiber.</param>
        /// <param name="otdrSetup">The <see cref="OTDRSetupType"/> settings applied to the fiber.</param>
        /// <param name="fiberType">The <see cref="FiberTypeType"/> settings applied to the fiber.</param>
        /// <param name="ribbonType">The optional <see cref="RibbonTypeType"/> settings applied to the fiber.</param>
        private void OutputNode(
            FiberType node,
            AnalysisSetupType analysisSetup,
            OTDRSetupType otdrSetup,
            FiberTypeType fiberType,
            RibbonTypeType ribbonType = null)
        {
            // Don't generate anything if there are no results
            TestResultsType[] results = node.Status.TestResults;
            if (results == null || results.Length == 0)
            {
                return;
            }

            foreach (TestResultsType wavelengthResults in results)
            {
                foreach (ResultType directionResult in wavelengthResults.Result)
                {
                    // Average direction has no otdr info. Top will always come first so save its otdr
                    // to use on average
                    ResultsOTDRInfoType saveTopOTDRInfo = null;
                    var otdrInfo = directionResult.OTDR;
                    if (directionResult.Aspect == (int)ResultAspectType.Top)
                    {
                        saveTopOTDRInfo = otdrInfo;
                    }
                    else if ((int)directionResult.Aspect == (int)ResultAspectType.Average)
                    {
                        otdrInfo = saveTopOTDRInfo;
                    }

                    var fiberStatus = SeverityType.Pass;
                    if (!(node.Status.NonCompliances == null))
                    {
                        foreach (NonComplianceType noncom in node.Status.NonCompliances)
                        {
                            if ((noncom.Result ?? string.Empty) == (directionResult.ID ?? string.Empty) & noncom.Type == (int)NonComplianceResultType.Fiber)
                            {
                                fiberStatus = (SeverityType)Math.Max((int)fiberStatus, (int)noncom.Severity);
                            }
                        }
                    }

                    string waveIDwithoutTag = wavelengthResults.WavelengthID.Split(' ')[0];
                    int wavelength_nm = int.Parse(waveIDwithoutTag);
                    double groupIndex = fiberType.WavelengthProperties.Single(
                        (x) => x.Wavelength == wavelength_nm).GroupIndex;
                    OutputFiberData(
                        directionResult,
                        wavelengthResults.WavelengthID,
                        fiberStatus,
                        otdrInfo.CenterWavelength,
                        groupIndex);
                }
            }
        }

        /// <summary>
        /// Write the fiber result details to the report.
        /// </summary>
        /// <param name="result">The <see cref="ResultType"/> containing test result data
        /// from the analysis performed on a single signature at a particular wavelength
        /// and direction.</param>
        /// <param name="wavelengthID">The Wavelength ID identifier of the OTDR laser used
        /// to acquire the signature.</param>
        /// <param name="status">The Pass / Flag / Fail status of the fiber.</param>
        /// <param name="centerWavelength">The center wavelength of the OTDR laser.</param>
        /// <param name="groupIndex">The group index specified from the Analysis Setup settings
        /// for fiber at the wavelength of the OTDR laser.</param>
        private void OutputFiberData(
            ResultType result,
            string wavelengthID,
            SeverityType status,
            double centerWavelength,
            double groupIndex)
        {
            {
                string fiberName = result.ID.Split(',')[0];
                writer.Write("\"" + fiberName + "\"");
                if ((int)result.Aspect == (int)ResultAspectType.Average)
                {
                    writer.Write("," + result.Aspect.ToString());
                }
                else
                {
                    writer.Write("," + result.FiberEnd);
                }

                writer.Write(",{0:####}", wavelengthID);
                if (printCenterWavelength)
                {
                    writer.Write(",{0:####.#}", centerWavelength);
                }

                writer.Write(",{0:#.####}", groupIndex);
                writer.Write("," + status.ToString());
                if (!(result.FiberMeasurements == null))
                {
                    writer.Write("," + "{0:#.####}", result.FiberMeasurements.Attenuation);
                    writer.Write("," + "{0:#####.#}", result.FiberMeasurements.FiberLength);
                }
                else
                {
                    writer.Write(",,");
                }

                if (!(result.Coupling == null))
                {
                    writer.Write("," + "{0:G2}", result.Coupling.Loss);
                }
                else
                {
                    writer.Write(",");
                }

                if (!(result.FiberMeasurements == null || result.FiberMeasurements.SlidingWindows == null))
                {
                    writer.Write("," + "{0:G2}", result.FiberMeasurements.SlidingWindows.MaxWindowUniformity);
                    writer.Write("," + "{0:#####.#}", result.FiberMeasurements.SlidingWindows.MaxWindowUniformityLocation);
                }
                else
                {
                    writer.Write(",,");
                }

                writer.WriteLine();
            }
        }

        /// <summary>
        /// Finds the <see cref="AnalysisSetupType"/> of a particular node in the cable structure.
        /// </summary>
        /// <param name="node">The <c>CableNodeType</c> node.</param>
        /// <returns>The <see cref="AnalysisSetupType"/> explicitly assigned to the node, or
        /// <c>null</c> if the node inherits this from a parent node.</returns>
        private AnalysisSetupType FindAnalysisSetups(CableNodeType node)
        {
            foreach (AnalysisSetupType analysisSetup in cable.Item.CableSetup.AnalysisSetups)
            {
                if ((analysisSetup.ID ?? string.Empty) == (node.AnalysisSetup ?? string.Empty))
                {
                    return analysisSetup;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the <see cref="OTDRSetupType"/> of a particular node in the cable structure.
        /// </summary>
        /// <param name="node">The <c>CableNodeType</c> node.</param>
        /// <returns>The <see cref="OTDRSetupType"/> explicitly assigned to the node, or
        /// <c>null</c> if the node inherits this from a parent node.</returns>
        private OTDRSetupType FindOTDRSetups(CableNodeType node)
        {
            foreach (OTDRSetupType otdrSetup in cable.Item.CableSetup.OTDRSetups)
            {
                if ((otdrSetup.ID ?? string.Empty) == (node.OTDRSetup ?? string.Empty))
                {
                    return otdrSetup;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the <see cref="FiberTypeType"/> of a particular node in the cable structure.
        /// </summary>
        /// <param name="node">The <c>CableNodeType</c> node.</param>
        /// <returns>The <see cref="FiberTypeType"/> explicitly assigned to the node, or
        /// <c>null</c> if the node inherits this from a parent node.</returns>
        private FiberTypeType FindFiberType(CableNodeType node)
        {
            foreach (FiberTypeType fiberTypes in cable.Item.CableSetup.FiberTypes)
            {
                if ((fiberTypes.ID ?? string.Empty) == (node.FiberType ?? string.Empty))
                {
                    return fiberTypes;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the <see cref="RibbonTypeType"/> of a particular node in the cable structure.
        /// </summary>
        /// <param name="node">The <c>CableNodeType</c> node.</param>
        /// <returns>The <see cref="RibbonTypeType"/> explicitly assigned to the node, or
        /// <c>null</c> if the node inherits this from a parent node.</returns>
        private RibbonTypeType FindRibbonType(CableNodeType node)
        {
            foreach (RibbonTypeType ribbonType in cable.Item.CableSetup.RibbonTypes)
            {
                if ((ribbonType.ID ?? string.Empty) == (node.FiberType ?? string.Empty))
                {
                    return ribbonType;
                }
            }

            return null;
        }
    }
}
