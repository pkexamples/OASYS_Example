//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.Examples
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// Main program execution and error handling
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Entry point for program execution
        /// </summary>
        /// <param name="args">Arguments received from the command line as an array of strings.</param>
        internal static void Main(string[] args)
        {
            // Takes 3 command line arguments:
            // CSV file name, log file name, cable results file name.
            string csvFile, logFile, pkcbrFile;
            try
            {
                if (args.Length != 3)
                {
                    throw new ArgumentException(
                        "Post-processor requires 3 command line arguments:" +
                        "CSV file name, log file name, cable results file name.",
                        "Command line arguments");
                }

                csvFile = args[0];
                logFile = args[1];
                pkcbrFile = args[2];

                // Create stream objects for input (PKCBR), output (CSV report), and log file.
                CSVReportPostProcessor pp = null;
                using (var input = new FileStream(pkcbrFile, FileMode.Open, FileAccess.Read))
                {
                    // Overwrite the existing CSV file written by OASYS.net
                    using (var output = new StreamWriter(csvFile, false))
                    {
                        pp = new CSVReportPostProcessor(input, output);
                        pp.GenerateReport();
                    }
                }

                // Write to log upon successful exit
                using (var log = new StreamWriter(logFile, true))
                {
                    log.WriteLine("Post-Processor complete at " + DateTime.Now.ToString());
                }

                // Open report in Notepad
                Process.Start("notepad", string.Format("\"{0}\"", csvFile));

                // Signal successful exit
                Environment.ExitCode = 0;
            }
            catch (Exception ex)
            {
                // Signal "error" exit code
                Environment.ExitCode = 1;

                // Build error message
                string message = ex.Message;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                    message += Environment.NewLine + ex.Message;
                }

                // Write to log -- if valid log file received from command line
                try
                {
                    using (var log = new StreamWriter(args[1], true))
                    {
                        log.WriteLine("Post-Processor failed");
                        log.WriteLine("Error message: " + message);
                        log.WriteLine("Source: " + ex.Source);
                    }
                }
                catch
                {
                    // cannot write to log file
                }
            }
        }
    }
}
