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
    using System.Windows.Forms;

    /// <summary>
    /// A class to encapsulate the pre-processor main entry point
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        [STAThread]
        internal static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
