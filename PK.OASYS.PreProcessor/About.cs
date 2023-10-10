//-----------------------------------------------------------------------
// <copyright file="About.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.Examples
{
    using System;
    using System.Drawing;
    using System.Reflection;
    using System.Windows.Forms;
    using PhotonKinetics.WinFormStyle;

    /// <summary>
    /// The about box for the program
    /// </summary>
    public partial class AboutForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutForm" /> class.
        /// </summary>
        public AboutForm()
        {
            InitializeComponent();

            // set the Form's Icon from the project resources
            Icon = new Icon(Properties.Resources.PhotonKineticsAppIcons, 32, 32);

            // set colors
            BackColor = PKColor.FormBackColor;
            btnOK.BackColor = PKColor.FormButtonColor;
            btnOK.ForeColor = PKColor.FormButtonFontColor;

            // make not resize-able
            MaximumSize = Size;
            MinimumSize = Size;

            // set text from assembly attributes
            lblProduct.Text = (Assembly.GetExecutingAssembly().GetCustomAttributes(
                    typeof(AssemblyTitleAttribute), false)[0] as AssemblyTitleAttribute).Title;
            lblVersion.Text = "Version: " + Application.ProductVersion;
        }

        /// <summary>
        /// Handles the OKButton click event.
        /// </summary>
        /// <param name="sender">The OKButton Button.</param>
        /// <param name="e">EventArgs from the Click event.</param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
