//-----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="Photon Kinetics, Inc.">
//     Copyright (c) Photon Kinetics, Inc.
//     Licensed under the MIT License. See License.txt in the project
//     root for license information.
// </copyright>
//-----------------------------------------------------------------------
namespace PhotonKinetics.OASYS.Examples
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using PhotonKinetics.OASYS.IO;
    using PhotonKinetics.WinFormStyle;

    /// <summary>
    /// The main form of the Pre-processor.
    /// </summary>
    internal partial class MainForm : Form
    {
        #region Fields
        /// <summary>
        /// Object to store information needed to build a simple cable.
        /// </summary>
        private readonly SimpleCableData data;

        /// <summary>
        /// A FileStream to provide logging service.
        /// </summary>
        private FileStream log;

        /// <summary>
        /// A TextWriterTraceListener to attach to Trace output for logging.
        /// </summary>
        private TextWriterTraceListener traceListener;

        /// <summary>
        /// State holding flag to indicate the application can write to its log file.
        /// </summary>
        private bool canLog = true;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            // run the code emitted by the WinForms designer
            InitializeComponent();

            // set the Form's Icon from the project resources
            Icon = new Icon(Properties.Resources.PhotonKineticsAppIcons, 32, 32);

            // create a new business data object
            data = new SimpleCableData();
        }
        #endregion

        #region IDisposable
        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        /// <param name="disposing">Flag that indicates if Dispose was already called.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (components != null)
                    {
                        components.Dispose();
                    }

                    if (log != null)
                    {
                        if (log.CanWrite && traceListener != null)
                        {
                            traceListener.Dispose();
                        }

                        log.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion

        #region Form Load / Closing event handlers and other initialization
        /// <summary>
        /// Handler for the FormClosing event
        /// </summary>
        /// <param name="sender">A reference to this form.</param>
        /// <param name="e">The <c>FormClosingEventArgs</c> from the event.</param>
        /// <remarks>
        /// This is here to pick up when someone closes by clicking the big red X, or
        /// pressing Alt-F4, or killing it with Task Manager, or other means other than
        /// clicking a button or hitting Escape (which activates the Cancel button.) It
        /// ensures a proper "cancel" is sent to OASYS.net. All closes that occur due to
        /// the program cause a specific dialog result to be set, so DialogResult.None
        /// will indicate an external cause.
        /// </remarks>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == (int)DialogResult.None)
            {
                CancelApplication(true);
            }
        }

        /// <summary>
        /// Handler for the Load event
        /// </summary>
        /// <param name="sender">A reference to this form.</param>
        /// <param name="e">The <c>EventArgs</c> from the Load event.</param>
        /// <remarks>
        /// For Version 3.x.x of OASYS.net and later the following three command line tokens are
        /// accessible via Environment.GetCommandLineArgs():
        /// 1) The file path of the pre-processor application being launched
        /// 2) The directory location/file name of where to store the new cable definition
        ///     (this is how OASYS knows what <c>.pkcbd</c> file to use for the next measurement)
        ///     a)  Note: this is set under the OASYS software <c>Configure ->
        ///         OASYS.NET Setup -> Local Stations settings -> Preprocessor Cable File</c>
        /// 3) The location and name of cable definition file SENT to the pre-processor
        ///     (this is an optional <c>.pkcbd</c> file location for pre-processor to consume and use)
        ///     a)  Note: this is only included if the 'Send cable file to pre-processor'
        ///         option is selected.  If that option is NOT selected, then
        ///         only two pass parameters will be sent.
        ///     b)  Note: the actual cable definition file sent to the pre-processor depends
        ///         upon how the measurement was invoked:
        ///         i)  If 'Next Test' is selected the <c>.pkcbd</c> file will be the file that was
        ///             used for the previous measurement.
        ///         ii) If 'New Test' is selected and a cable manually formed (or loaded),
        ///             it will send that specific <c>.pkcbd</c> file.
        /// </remarks>
        private void MainForm_Load(object sender, EventArgs e)
        {
            Enabled = false;
            var logFileFullPath = Path.Combine(
                OASYSPaths.OasysFilesDirectory,
                "OASYSPreProcessor.log");
            log = new FileStream(logFileFullPath, FileMode.Create);
            traceListener = new TextWriterTraceListener(log);

            try
            {
                Trace.Listeners.Add(traceListener);
                Trace.Flush();
                Trace.AutoFlush = true;
                try
                {
                    LogTraceEntry("Entered Load Form event");
                }
                catch (Exception)
                {
                    canLog = false;
                }

                // not really necessary; OASYS.net always sends the target .pkcbd path
                if (Environment.GetCommandLineArgs().Length < 2)
                {
                    throw new IndexOutOfRangeException("Pre-processor must receive at least one command line argument.");
                }

                // Set form colors
                InitFormColors();

                // make form not resize-able
                MaximumSize = Size;
                MinimumSize = Size;

                // Set Ribbon / Loose radio button state based on default value of cableType
                if (data.CableType == SimpleCableData.CableTypes.Loose)
                {
                    rdbLoose.Checked = true;
                }
                else
                {
                    rdbRibbon.Checked = true;
                }

                // update controls based on current "CableType" (Loose or Ribbon)
                UpdateCableTypeDisplay();

                // initialize text boxes
                txtFibers.Text = data.NumberOfFibers.ToString();
                txtTubes.Text = data.NumberOfTubes.ToString();
                txtRibbons.Text = data.NumberOfRibbons.ToString();

                // Target output PKCBD file path is always the first command line argument
                txtFileLocation.Text = Environment.GetCommandLineArgs()[1];
                txtCableID.Text = data.CableID;
                txtLength.Text = data.CableLength.ToString();
                txtOperatorID.Text = SystemInformation.UserName;

                // Load all setups into combo boxes
                OASYSSetupServer.LoadAllSetupIDs();
                cmbFiberTypes.Items.AddRange(OASYSSetupServer.FiberTypeIDs.ToArray());
                cmbOtdr.Items.AddRange(OASYSSetupServer.OTDRSetupIDs.ToArray());
                cmbRibbonType.Items.AddRange(OASYSSetupServer.RibbonTypeIDs.ToArray());
                cmbAnalysis.Items.AddRange(OASYSSetupServer.AnalysisSetupIDs.ToArray());

                ComboBox[] comboBoxes = new[] { cmbRibbonType, cmbFiberTypes, cmbOtdr, cmbAnalysis };
                foreach (var box in comboBoxes)
                {
                    if (box.Items.Count > 0)
                    {
                        box.SelectedIndex = 0;
                    }
                    else
                    {
                        // This preprocessor is useless if you don't have at least one of each
                        // type of setup saved.
                        throw new ApplicationException(string.Format(
                            "Unable to populate the combo box {0} because there are no\r\n" +
                                "corresponding saved setups with which to fill it with.",
                            box.Name));
                    }
                }

                Enabled = true;
                LogTraceEntry("Success with entire form load");
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format(
                    "An error has occurred loading the pre-processor:\r\n\t{0}\r\n" +
                        "See log file for details",
                    ex.Message);
                MessageBox.Show(errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogTraceEntry(errorMessage);
                CloseApplicationWithError(ex);
            }
        }

        /// <summary>
        /// Updates GUI for selected cable type (loose or ribbon)
        /// </summary>
        private void UpdateCableTypeDisplay()
        {
            if (rdbLoose.Checked)
            {
                data.CableType = SimpleCableData.CableTypes.Loose;
                lblRibbons.ForeColor = Color.Gray;
                txtRibbons.BackColor = Color.Gray;
                txtRibbons.Enabled = false;
                lblRibbonType.ForeColor = Color.Gray;
                cmbRibbonType.BackColor = Color.Gray;
                cmbRibbonType.Enabled = false;
                lblFibers.Text = "Number of Fibers in Tube:";
            }
            else
            {
                data.CableType = SimpleCableData.CableTypes.Ribbon;
                lblRibbons.Enabled = true;
                lblRibbons.ForeColor = Color.Black;
                txtRibbons.BackColor = SystemColors.Window;
                txtRibbons.Enabled = true;
                lblRibbonType.Enabled = true;
                lblRibbonType.ForeColor = Color.Black;
                cmbRibbonType.BackColor = SystemColors.Window;
                cmbRibbonType.Enabled = true;
                lblFibers.Text = "Number of Fibers in Ribbon:";
            }
        }

        /// <summary>
        /// Sets colors on controls
        /// </summary>
        private void InitFormColors()
        {
            BackColor = PKColor.FormBackColor;
            btnCancel.BackColor = PKColor.FormButtonColor;
            btnCancel.ForeColor = PKColor.FormButtonFontColor;
            btnCreateFile.BackColor = PKColor.FormButtonColor;
            btnCreateFile.ForeColor = PKColor.FormButtonFontColor;
            btnAbout.BackColor = PKColor.FormButtonColor;
            btnAbout.ForeColor = PKColor.FormButtonFontColor;
            gpbCableType.BackColor = PKColor.FormBackColor;
            gpbSettings.BackColor = PKColor.FormBackColor;
            gpbFiles.BackColor = PKColor.FormBackColor;
            lblRibbons.BackColor = PKColor.FormBackColor;
        }
        #endregion

        #region Methods for button click / check-changed event handling
        /// <summary>
        /// Handler for Cancel button's Click event.
        /// </summary>
        /// <param name="sender">The Cancel button.</param>
        /// <param name="e">The EventArgs from the Click event</param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            CancelApplication();
        }

        /// <summary>
        /// Handler for Measure button
        /// </summary>
        /// <param name="sender">The Measure button.</param>
        /// <param name="e">The EventArgs from the Click event.</param>
        /// <remarks>
        /// Validates user inputs, builds the cable, writes the cable definition file,
        /// and closes the pre-processor.
        /// </remarks>
        private void Measure_Click(object sender, EventArgs e)
        {
            try
            {
                data.CableID = GetCableID();
                var cableResultDirectory = Path.Combine(OASYSPaths.DataDirectory, data.CableID);

                // Prompt to overwrite cable data directory if it already exists
                if (Directory.Exists(cableResultDirectory))
                {
                    var proceed = MessageBox.Show(
                        "The cable data directory already exists!\r\n" +
                        "Do you want to overwrite the existing data?",
                        Text,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Exclamation);

                    if (proceed != DialogResult.Yes)
                    {
                        return;
                    }
                }

                // Update data from user input to send to the SimpleCableBuilder
                data.OperatorID = txtOperatorID.Text.Trim();
                data.NumberOfFibers = GetFiberNumber();
                data.NumberOfTubes = GetTubeNumber();
                if (rdbLoose.Checked)
                {
                    data.CableType = SimpleCableData.CableTypes.Loose;
                }
                else
                {
                    data.CableType = SimpleCableData.CableTypes.Ribbon;
                    data.RibbonTypeID = cmbRibbonType.SelectedItem.ToString();
                    data.NumberOfRibbons = GetRibbonNumber();
                }

                data.CableLength = GetLength();
                data.FiberTypeID = cmbFiberTypes.SelectedItem.ToString();
                data.OtdrSetupID = cmbOtdr.SelectedItem.ToString();
                data.AnalysisSetupID = cmbAnalysis.SelectedItem.ToString();

                var cableBuilder = new SimpleCableWriter(data);
                cableBuilder.WriteCableFile();
                CloseApplicationSuccess();
            }
            catch (Exception ex)
            {
                var msg = "Error creating cable file\r\n\t" + ex.Message;
                LogTraceEntry(msg);
                MessageBox.Show(msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                CloseApplicationWithError(ex);
            }
        }

        /// <summary>
        /// Handler for `Loose` and `Ribbon` radio button CheckChanged event.
        /// </summary>
        /// <param name="sender">The <c>RadioButton</c> that changed state.</param>
        /// <param name="e">The <c>EventArgs</c> from the CheckChanged event.</param>
        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateCableTypeDisplay();
        }

        /// <summary>
        /// Handler for About button Click event.
        /// </summary>
        /// <param name="sender">The About button.</param>
        /// <param name="e">The EventArgs from the Click event.</param>
        private void About_Click(object sender, EventArgs e)
        {
            using (var aboutBox = new AboutForm())
            {
                aboutBox.ShowDialog();
            }
        }
        #endregion

        #region Methods for fetching input from controls
        /// <summary>
        /// Gets the number of fibers from the <c>txtFibers</c> TextBox.
        /// </summary>
        /// <returns>An integer representing the number of fibers specified by the user.</returns>
        private int GetFiberNumber()
        {
            int val;
            if (!int.TryParse(txtFibers.Text, out val) || val <= 0)
            {
                throw new ApplicationException("Number of fibers must be greater than zero.");
            }

            return val;
        }

        /// <summary>
        /// Gets the number of tubes from the <c>txtTubes</c> TextBox.
        /// </summary>
        /// <returns>An integer representing the number of tubes specified by the user.</returns>
        private int GetTubeNumber()
        {
            int val;
            if (!int.TryParse(txtTubes.Text, out val) || val <= 0)
            {
                throw new ApplicationException("Number of tubes must be greater than zero.");
            }

            return val;
        }

        /// <summary>
        /// Gets cable length from the <c>txtLength</c> TextBox.
        /// </summary>
        /// <returns>A <c>double</c> representing the cable length specified by the user.</returns>
        private double GetLength()
        {
            double val;
            if (!double.TryParse(txtLength.Text, out val) || val <= 0)
            {
                throw new ApplicationException("Length must be greater than zero.");
            }

            return val;
        }

        /// <summary>
        /// Gets the cable ID from the <c>txtCableID</c> TextBox.
        /// </summary>
        /// <returns>A string representing the cable ID</returns>
        private string GetCableID()
        {
            string id = txtCableID.Text.Trim();
            if (string.IsNullOrEmpty(id))
            {
                throw new ApplicationException("Cable ID must be supplied.");
            }

            return id;
        }

        /// <summary>
        /// Gets the number of ribbons from the <c>txtRibbons</c> TextBox.
        /// </summary>
        /// <returns>An integer representing the number of ribbons specified by the user.</returns>
        private int GetRibbonNumber()
        {
            int val;
            if (!int.TryParse(txtRibbons.Text, out val) || val <= 0)
            {
                throw new ApplicationException("Number of ribbons must be greater than zero.");
            }

            return val;
        }
        #endregion

        #region Methods for exiting the program in different states
        /// <summary>
        /// Closes the program with ExitCode set to `success` to indicate to
        /// OASYS.net that the pre-processor exited normally.
        /// </summary>
        private void CloseApplicationSuccess()
        {
            LogTraceEntry("Success exiting entire program");
            Environment.ExitCode = 1;
            CloseLogFile();

            // Signifies "normal" close
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Closes the program and logs the Exception passed in. Sets ExitCode to 1
        /// to notify OASYS.net that the post-processor closed with an error.
        /// </summary>
        /// <param name="ex">The Exception to log.</param>
        private void CloseApplicationWithError(Exception ex = null)
        {
            string msg = "Exiting pre-processor with error.";
            if (ex != null)
            {
                msg += string.Format(
                    "\r\nError: {0}\r\n\t{1}",
                ex.Message,
                ex.StackTrace);
            }

            if (canLog)
            {
                LogTraceEntry(msg);
            }

            CloseLogFile();

            // Signifies closing due to error
            DialogResult = DialogResult.Abort;
            Environment.ExitCode = 1;
            Close();
        }

        /// <summary>
        /// Closes the program with ExitCode set to `cancel` to indicate to
        /// OASYS.net that the pre-processor was canceled by the user.
        /// </summary>
        /// <param name="alreadyClosing">A flag to indicate if the Form is already closing.</param>
        private void CancelApplication(bool alreadyClosing = false)
        {
            // When the pre-processor shuts down, it sets the Environment exit code to notify OASYS of pre-processor execution status.
            // Use for a cancellation of pre-processor (no error, just cancel).  Must set exitCode to two to let OASYS know it was canceled
            LogTraceEntry("Program canceled by operator.");
            Environment.ExitCode = 2;
            CloseLogFile();

            // Signifies close due to cancel
            DialogResult = DialogResult.Cancel;
            if (!alreadyClosing)
            {
                Close();
            }
        }
        #endregion

        #region Methods for logging
        /// <summary>
        /// Flushes the Trace output and closes the log file.
        /// </summary>
        private void CloseLogFile()
        {
            if (log != null && log.CanWrite)
            {
                Trace.Flush();
                log.Close();
            }
        }

        /// <summary>
        /// Logs a message to the log file with call stack information.
        /// </summary>
        /// <param name="message">The text of the message to log.</param>
        private void LogTraceEntry(string message)
        {
            // Logs message with current method name on call stack
            var mb = new StackFrame(1, true).GetMethod();
            Trace.WriteLine(string.Format("{1}.{2}: {0}", message, mb.ReflectedType.Name, mb.Name));
        }
        #endregion

        #region Methods for keyboard input validation
        /// <summary>
        /// Validates text box entry upon key press when only positive integers are allowed.
        /// </summary>
        /// <param name="sender">The <c>TextBox</c> handling the event.</param>
        /// <param name="e">The <c>KeyPressEventArgs</c> from the KeyPress event.</param>
        private void OnIntegerTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            PosIntegerOnly(e);
        }

        /// <summary>
        /// Validates text box entry upon key press when only positive real numbers are allowed.
        /// </summary>
        /// <param name="sender">The <c>TextBox</c> handling the event.</param>
        /// <param name="e">The <c>KeyPressEventArgs</c> from the KeyPress event.</param>
        private void OnDoubleTextBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            PosFloatOnly(e);
        }

        /// <summary>
        /// Catches the key selected and only allows positive integer key presses to be
        /// accepted ("0,1,2,3,4,5,6,7,8,9" only - no other char)
        /// </summary>
        /// <param name="e">The <c>KeyPressEventArgs</c> forwarded from the key press event
        /// handling method.</param>
        /// <remarks>
        /// This is not a complete validation, as it allows some special characters.
        /// </remarks>
        private void PosIntegerOnly(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Back | e.KeyChar == (char)Keys.End)
            {
                return;
            }

            if (e.KeyChar < '0' | e.KeyChar > '9')
            {
                e.KeyChar = (char)0;
            }
        }

        /// <summary>
        /// Catches the key selected and only allows positive real value key presses to be accepted
        /// </summary>
        /// <param name="e">The <c>KeyPressEventArgs</c> forwarded from the key press event
        /// handling method.</param>
        /// <remarks>
        /// This is not a complete validation, as it allows some special characters.
        /// </remarks>
        private void PosFloatOnly(KeyPressEventArgs e)
        {
            // NOTE: 44 is the comma, which is accepted here, regardless of CurrentCulture
            if (e.KeyChar == (char)Keys.Back | e.KeyChar == (char)Keys.End | e.KeyChar == (char)44)
            {
                return;
            }

            if (e.KeyChar < '.' | e.KeyChar > '9' | e.KeyChar == '/')
            {
                e.KeyChar = (char)0;
            }
        }
        #endregion
    }
}
