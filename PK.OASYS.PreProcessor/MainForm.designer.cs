using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace PhotonKinetics.OASYS.Examples
{
    internal partial class MainForm : Form
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components = null;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnMeasure = new System.Windows.Forms.Button();
            this._cmbFiberTypes = new System.Windows.Forms.ComboBox();
            this._lblFiberTypes = new System.Windows.Forms.Label();
            this._cmbOtdr = new System.Windows.Forms.ComboBox();
            this._lblOtdr = new System.Windows.Forms.Label();
            this._lblFibers = new System.Windows.Forms.Label();
            this._txtFibers = new System.Windows.Forms.TextBox();
            this._lblTubes = new System.Windows.Forms.Label();
            this._txtTubes = new System.Windows.Forms.TextBox();
            this._lblAnalysis = new System.Windows.Forms.Label();
            this._cmbAnalysis = new System.Windows.Forms.ComboBox();
            this._gpbSettings = new System.Windows.Forms.GroupBox();
            this._cmbRibbonType = new System.Windows.Forms.ComboBox();
            this._lblRibbonType = new System.Windows.Forms.Label();
            this._txtRibbons = new System.Windows.Forms.TextBox();
            this._lblRibbons = new System.Windows.Forms.Label();
            this._txtOperatorID = new System.Windows.Forms.TextBox();
            this._lblOperatorID = new System.Windows.Forms.Label();
            this._txtLength = new System.Windows.Forms.TextBox();
            this._lblLength = new System.Windows.Forms.Label();
            this._txtCableID = new System.Windows.Forms.TextBox();
            this._lblID = new System.Windows.Forms.Label();
            this._gpbFiles = new System.Windows.Forms.GroupBox();
            this._txtFileLocation = new System.Windows.Forms.TextBox();
            this._lblFileLocation = new System.Windows.Forms.Label();
            this._gpbCableType = new System.Windows.Forms.GroupBox();
            this._rdbRibbon = new System.Windows.Forms.RadioButton();
            this._rdbLoose = new System.Windows.Forms.RadioButton();
            this._btnAbout = new System.Windows.Forms.Button();
            this._gpbSettings.SuspendLayout();
            this._gpbFiles.SuspendLayout();
            this._gpbCableType.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnCancel
            // 
            this._btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._btnCancel.Location = new System.Drawing.Point(254, 500);
            this._btnCancel.Name = "_btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(118, 48);
            this._btnCancel.TabIndex = 3;
            this._btnCancel.Text = "&Cancel";
            this._btnCancel.UseVisualStyleBackColor = true;
            this._btnCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // _btnMeasure
            // 
            this._btnMeasure.Location = new System.Drawing.Point(120, 500);
            this._btnMeasure.Name = "_btnMeasure";
            this._btnMeasure.Size = new System.Drawing.Size(118, 48);
            this._btnMeasure.TabIndex = 2;
            this._btnMeasure.Text = "&Measure";
            this._btnMeasure.UseVisualStyleBackColor = true;
            this._btnMeasure.Click += new System.EventHandler(this.Measure_Click);
            // 
            // _cmbFiberTypes
            // 
            this._cmbFiberTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbFiberTypes.FormattingEnabled = true;
            this._cmbFiberTypes.Location = new System.Drawing.Point(190, 232);
            this._cmbFiberTypes.Name = "_cmbFiberTypes";
            this._cmbFiberTypes.Size = new System.Drawing.Size(268, 29);
            this._cmbFiberTypes.Sorted = true;
            this._cmbFiberTypes.TabIndex = 7;
            // 
            // _lblFiberTypes
            // 
            this._lblFiberTypes.AutoSize = true;
            this._lblFiberTypes.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblFiberTypes.Location = new System.Drawing.Point(121, 236);
            this._lblFiberTypes.Name = "_lblFiberTypes";
            this._lblFiberTypes.Size = new System.Drawing.Size(101, 21);
            this._lblFiberTypes.TabIndex = 3;
            this._lblFiberTypes.Text = "Fiber Type:";
            // 
            // _cmbOtdr
            // 
            this._cmbOtdr.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbOtdr.FormattingEnabled = true;
            this._cmbOtdr.Location = new System.Drawing.Point(190, 265);
            this._cmbOtdr.Name = "_cmbOtdr";
            this._cmbOtdr.Size = new System.Drawing.Size(268, 29);
            this._cmbOtdr.Sorted = true;
            this._cmbOtdr.TabIndex = 8;
            // 
            // _lblOtdr
            // 
            this._lblOtdr.AutoSize = true;
            this._lblOtdr.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblOtdr.Location = new System.Drawing.Point(95, 269);
            this._lblOtdr.Name = "_lblOtdr";
            this._lblOtdr.Size = new System.Drawing.Size(137, 21);
            this._lblOtdr.TabIndex = 5;
            this._lblOtdr.Text = "OTDR Settings:";
            // 
            // _lblFibers
            // 
            this._lblFibers.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblFibers.Location = new System.Drawing.Point(14, 172);
            this._lblFibers.Name = "_lblFibers";
            this._lblFibers.Size = new System.Drawing.Size(175, 15);
            this._lblFibers.TabIndex = 6;
            this._lblFibers.Text = "Number of Fibers in Tube:";
            this._lblFibers.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // _txtFibers
            // 
            this._txtFibers.Location = new System.Drawing.Point(190, 169);
            this._txtFibers.Name = "_txtFibers";
            this._txtFibers.Size = new System.Drawing.Size(48, 28);
            this._txtFibers.TabIndex = 5;
            this._txtFibers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnIntegerTextBoxKeyPress);
            // 
            // _lblTubes
            // 
            this._lblTubes.AutoSize = true;
            this._lblTubes.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblTubes.Location = new System.Drawing.Point(33, 112);
            this._lblTubes.Name = "_lblTubes";
            this._lblTubes.Size = new System.Drawing.Size(224, 21);
            this._lblTubes.TabIndex = 8;
            this._lblTubes.Text = "Number of Tubes in Cable:";
            // 
            // _txtTubes
            // 
            this._txtTubes.Location = new System.Drawing.Point(190, 109);
            this._txtTubes.Name = "_txtTubes";
            this._txtTubes.Size = new System.Drawing.Size(48, 28);
            this._txtTubes.TabIndex = 3;
            this._txtTubes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnIntegerTextBoxKeyPress);
            // 
            // _lblAnalysis
            // 
            this._lblAnalysis.AutoSize = true;
            this._lblAnalysis.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblAnalysis.Location = new System.Drawing.Point(95, 302);
            this._lblAnalysis.Name = "_lblAnalysis";
            this._lblAnalysis.Size = new System.Drawing.Size(132, 21);
            this._lblAnalysis.TabIndex = 10;
            this._lblAnalysis.Text = "Analysis Limits:";
            // 
            // _cmbAnalysis
            // 
            this._cmbAnalysis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbAnalysis.FormattingEnabled = true;
            this._cmbAnalysis.Location = new System.Drawing.Point(190, 298);
            this._cmbAnalysis.Name = "_cmbAnalysis";
            this._cmbAnalysis.Size = new System.Drawing.Size(268, 29);
            this._cmbAnalysis.Sorted = true;
            this._cmbAnalysis.TabIndex = 9;
            // 
            // _gpbSettings
            // 
            this._gpbSettings.BackColor = System.Drawing.SystemColors.Control;
            this._gpbSettings.Controls.Add(this._cmbOtdr);
            this._gpbSettings.Controls.Add(this._cmbRibbonType);
            this._gpbSettings.Controls.Add(this._lblRibbonType);
            this._gpbSettings.Controls.Add(this._txtRibbons);
            this._gpbSettings.Controls.Add(this._lblRibbons);
            this._gpbSettings.Controls.Add(this._txtOperatorID);
            this._gpbSettings.Controls.Add(this._lblOperatorID);
            this._gpbSettings.Controls.Add(this._txtLength);
            this._gpbSettings.Controls.Add(this._cmbAnalysis);
            this._gpbSettings.Controls.Add(this._lblLength);
            this._gpbSettings.Controls.Add(this._lblAnalysis);
            this._gpbSettings.Controls.Add(this._txtCableID);
            this._gpbSettings.Controls.Add(this._txtTubes);
            this._gpbSettings.Controls.Add(this._lblTubes);
            this._gpbSettings.Controls.Add(this._lblID);
            this._gpbSettings.Controls.Add(this._txtFibers);
            this._gpbSettings.Controls.Add(this._cmbFiberTypes);
            this._gpbSettings.Controls.Add(this._lblFibers);
            this._gpbSettings.Controls.Add(this._lblFiberTypes);
            this._gpbSettings.Controls.Add(this._lblOtdr);
            this._gpbSettings.ForeColor = System.Drawing.SystemColors.ControlText;
            this._gpbSettings.Location = new System.Drawing.Point(13, 81);
            this._gpbSettings.Name = "_gpbSettings";
            this._gpbSettings.Size = new System.Drawing.Size(479, 330);
            this._gpbSettings.TabIndex = 1;
            this._gpbSettings.TabStop = false;
            this._gpbSettings.Text = "Cable Structure and Settings";
            // 
            // _cmbRibbonType
            // 
            this._cmbRibbonType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbRibbonType.FormattingEnabled = true;
            this._cmbRibbonType.Location = new System.Drawing.Point(190, 198);
            this._cmbRibbonType.Name = "_cmbRibbonType";
            this._cmbRibbonType.Size = new System.Drawing.Size(268, 29);
            this._cmbRibbonType.TabIndex = 6;
            // 
            // _lblRibbonType
            // 
            this._lblRibbonType.AutoSize = true;
            this._lblRibbonType.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblRibbonType.Location = new System.Drawing.Point(109, 202);
            this._lblRibbonType.Name = "_lblRibbonType";
            this._lblRibbonType.Size = new System.Drawing.Size(117, 21);
            this._lblRibbonType.TabIndex = 15;
            this._lblRibbonType.Text = "Ribbon Type:";
            // 
            // _txtRibbons
            // 
            this._txtRibbons.Location = new System.Drawing.Point(190, 139);
            this._txtRibbons.Name = "_txtRibbons";
            this._txtRibbons.Size = new System.Drawing.Size(48, 28);
            this._txtRibbons.TabIndex = 4;
            this._txtRibbons.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnIntegerTextBoxKeyPress);
            // 
            // _lblRibbons
            // 
            this._lblRibbons.AutoSize = true;
            this._lblRibbons.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblRibbons.Location = new System.Drawing.Point(25, 142);
            this._lblRibbons.Name = "_lblRibbons";
            this._lblRibbons.Size = new System.Drawing.Size(234, 21);
            this._lblRibbons.TabIndex = 13;
            this._lblRibbons.Text = "Number of Ribbons in Tube:";
            // 
            // _txtOperatorID
            // 
            this._txtOperatorID.Location = new System.Drawing.Point(190, 79);
            this._txtOperatorID.Name = "_txtOperatorID";
            this._txtOperatorID.Size = new System.Drawing.Size(268, 28);
            this._txtOperatorID.TabIndex = 2;
            // 
            // _lblOperatorID
            // 
            this._lblOperatorID.AutoSize = true;
            this._lblOperatorID.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblOperatorID.Location = new System.Drawing.Point(115, 82);
            this._lblOperatorID.Name = "_lblOperatorID";
            this._lblOperatorID.Size = new System.Drawing.Size(108, 21);
            this._lblOperatorID.TabIndex = 12;
            this._lblOperatorID.Text = "Operator ID:";
            // 
            // _txtLength
            // 
            this._txtLength.Location = new System.Drawing.Point(190, 49);
            this._txtLength.Name = "_txtLength";
            this._txtLength.Size = new System.Drawing.Size(268, 28);
            this._txtLength.TabIndex = 1;
            this._txtLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnDoubleTextBoxKeyPress);
            // 
            // _lblLength
            // 
            this._lblLength.AutoSize = true;
            this._lblLength.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblLength.Location = new System.Drawing.Point(103, 52);
            this._lblLength.Name = "_lblLength";
            this._lblLength.Size = new System.Drawing.Size(122, 21);
            this._lblLength.TabIndex = 2;
            this._lblLength.Text = "Cable Length:";
            // 
            // _txtCableID
            // 
            this._txtCableID.Location = new System.Drawing.Point(190, 19);
            this._txtCableID.Name = "_txtCableID";
            this._txtCableID.Size = new System.Drawing.Size(268, 28);
            this._txtCableID.TabIndex = 0;
            // 
            // _lblID
            // 
            this._lblID.AutoSize = true;
            this._lblID.ForeColor = System.Drawing.SystemColors.ControlText;
            this._lblID.Location = new System.Drawing.Point(129, 22);
            this._lblID.Name = "_lblID";
            this._lblID.Size = new System.Drawing.Size(84, 21);
            this._lblID.TabIndex = 0;
            this._lblID.Text = "Cable ID:";
            // 
            // _gpbFiles
            // 
            this._gpbFiles.Controls.Add(this._txtFileLocation);
            this._gpbFiles.Controls.Add(this._lblFileLocation);
            this._gpbFiles.Enabled = false;
            this._gpbFiles.Location = new System.Drawing.Point(13, 419);
            this._gpbFiles.Name = "_gpbFiles";
            this._gpbFiles.Size = new System.Drawing.Size(479, 56);
            this._gpbFiles.TabIndex = 13;
            this._gpbFiles.TabStop = false;
            this._gpbFiles.Text = "Cable Definition File Location:";
            // 
            // _txtFileLocation
            // 
            this._txtFileLocation.Location = new System.Drawing.Point(72, 21);
            this._txtFileLocation.Name = "_txtFileLocation";
            this._txtFileLocation.ReadOnly = true;
            this._txtFileLocation.Size = new System.Drawing.Size(401, 28);
            this._txtFileLocation.TabIndex = 3;
            // 
            // _lblFileLocation
            // 
            this._lblFileLocation.AutoSize = true;
            this._lblFileLocation.Location = new System.Drawing.Point(7, 24);
            this._lblFileLocation.Name = "_lblFileLocation";
            this._lblFileLocation.Size = new System.Drawing.Size(93, 21);
            this._lblFileLocation.TabIndex = 2;
            this._lblFileLocation.Text = "Stored To:";
            // 
            // _gpbCableType
            // 
            this._gpbCableType.BackColor = System.Drawing.SystemColors.Control;
            this._gpbCableType.Controls.Add(this._rdbRibbon);
            this._gpbCableType.Controls.Add(this._rdbLoose);
            this._gpbCableType.Location = new System.Drawing.Point(14, 23);
            this._gpbCableType.Name = "_gpbCableType";
            this._gpbCableType.Size = new System.Drawing.Size(478, 50);
            this._gpbCableType.TabIndex = 0;
            this._gpbCableType.TabStop = false;
            this._gpbCableType.Text = "Cable Design Type";
            // 
            // _rdbRibbon
            // 
            this._rdbRibbon.AutoSize = true;
            this._rdbRibbon.Location = new System.Drawing.Point(266, 20);
            this._rdbRibbon.Name = "_rdbRibbon";
            this._rdbRibbon.Size = new System.Drawing.Size(92, 25);
            this._rdbRibbon.TabIndex = 1;
            this._rdbRibbon.TabStop = true;
            this._rdbRibbon.Text = "Ribbon";
            this._rdbRibbon.UseVisualStyleBackColor = true;
            // 
            // _rdbLoose
            // 
            this._rdbLoose.AutoSize = true;
            this._rdbLoose.Location = new System.Drawing.Point(132, 20);
            this._rdbLoose.Name = "_rdbLoose";
            this._rdbLoose.Size = new System.Drawing.Size(130, 25);
            this._rdbLoose.TabIndex = 0;
            this._rdbLoose.TabStop = true;
            this._rdbLoose.Text = "Loose Tube";
            this._rdbLoose.UseVisualStyleBackColor = true;
            this._rdbLoose.CheckedChanged += new System.EventHandler(this.RadioButton_CheckedChanged);
            // 
            // _btnAbout
            // 
            this._btnAbout.Location = new System.Drawing.Point(433, 513);
            this._btnAbout.Name = "_btnAbout";
            this._btnAbout.Size = new System.Drawing.Size(47, 23);
            this._btnAbout.TabIndex = 17;
            this._btnAbout.Text = "About";
            this._btnAbout.UseVisualStyleBackColor = true;
            this._btnAbout.Click += new System.EventHandler(this.About_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this._btnMeasure;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._btnCancel;
            this.ClientSize = new System.Drawing.Size(504, 570);
            this.Controls.Add(this._btnAbout);
            this.Controls.Add(this._gpbCableType);
            this.Controls.Add(this._gpbFiles);
            this.Controls.Add(this._btnMeasure);
            this.Controls.Add(this._btnCancel);
            this.Controls.Add(this._gpbSettings);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Oasys.NET Pre-Processor Example";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this._gpbSettings.ResumeLayout(false);
            this._gpbSettings.PerformLayout();
            this._gpbFiles.ResumeLayout(false);
            this._gpbFiles.PerformLayout();
            this._gpbCableType.ResumeLayout(false);
            this._gpbCableType.PerformLayout();
            this.ResumeLayout(false);

        }
        private Button _btnCancel;

        internal Button btnCancel
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnCancel;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnCancel != null)
                {
                    _btnCancel.Click -= Cancel_Click;
                }

                _btnCancel = value;
                if (_btnCancel != null)
                {
                    _btnCancel.Click += Cancel_Click;
                }
            }
        }

        private Button _btnMeasure;

        internal Button btnCreateFile
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnMeasure;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnMeasure != null)
                {
                    _btnMeasure.Click -= Measure_Click;
                }

                _btnMeasure = value;
                if (_btnMeasure != null)
                {
                    _btnMeasure.Click += Measure_Click;
                }
            }
        }

        private ComboBox _cmbFiberTypes;

        internal ComboBox cmbFiberTypes
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbFiberTypes;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbFiberTypes != null)
                {
                }

                _cmbFiberTypes = value;
                if (_cmbFiberTypes != null)
                {
                }
            }
        }

        private Label _lblFiberTypes;

        internal Label lblFiberTypes
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblFiberTypes;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblFiberTypes != null)
                {
                }

                _lblFiberTypes = value;
                if (_lblFiberTypes != null)
                {
                }
            }
        }

        private ComboBox _cmbOtdr;

        internal ComboBox cmbOtdr
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbOtdr;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbOtdr != null)
                {
                }

                _cmbOtdr = value;
                if (_cmbOtdr != null)
                {
                }
            }
        }

        private Label _lblOtdr;

        internal Label lblOtdr
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblOtdr;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblOtdr != null)
                {
                }

                _lblOtdr = value;
                if (_lblOtdr != null)
                {
                }
            }
        }

        private Label _lblFibers;

        internal Label lblFibers
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblFibers;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblFibers != null)
                {
                }

                _lblFibers = value;
                if (_lblFibers != null)
                {
                }
            }
        }

        private TextBox _txtFibers;

        internal TextBox txtFibers
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtFibers;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtFibers != null)
                {
                    _txtFibers.KeyPress -= OnIntegerTextBoxKeyPress;
                }

                _txtFibers = value;
                if (_txtFibers != null)
                {
                    _txtFibers.KeyPress += OnIntegerTextBoxKeyPress;
                }
            }
        }

        private Label _lblTubes;

        internal Label lblTubes
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblTubes;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblTubes != null)
                {
                }

                _lblTubes = value;
                if (_lblTubes != null)
                {
                }
            }
        }

        private TextBox _txtTubes;

        internal TextBox txtTubes
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtTubes;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtTubes != null)
                {
                    _txtTubes.KeyPress -= OnIntegerTextBoxKeyPress;
                }

                _txtTubes = value;
                if (_txtTubes != null)
                {
                    _txtTubes.KeyPress += OnIntegerTextBoxKeyPress;
                }
            }
        }

        private Label _lblAnalysis;

        internal Label lblAnalysis
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblAnalysis;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblAnalysis != null)
                {
                }

                _lblAnalysis = value;
                if (_lblAnalysis != null)
                {
                }
            }
        }

        private ComboBox _cmbAnalysis;

        internal ComboBox cmbAnalysis
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbAnalysis;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbAnalysis != null)
                {
                }

                _cmbAnalysis = value;
                if (_cmbAnalysis != null)
                {
                }
            }
        }

        private GroupBox _gpbSettings;

        internal GroupBox gpbSettings
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _gpbSettings;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_gpbSettings != null)
                {
                }

                _gpbSettings = value;
                if (_gpbSettings != null)
                {
                }
            }
        }

        private GroupBox _gpbFiles;

        internal GroupBox gpbFiles
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _gpbFiles;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_gpbFiles != null)
                {
                }

                _gpbFiles = value;
                if (_gpbFiles != null)
                {
                }
            }
        }

        private TextBox _txtFileLocation;

        internal TextBox txtFileLocation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtFileLocation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtFileLocation != null)
                {
                }

                _txtFileLocation = value;
                if (_txtFileLocation != null)
                {
                }
            }
        }

        private Label _lblFileLocation;

        internal Label lblFileLocation
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblFileLocation;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblFileLocation != null)
                {
                }

                _lblFileLocation = value;
                if (_lblFileLocation != null)
                {
                }
            }
        }

        private TextBox _txtCableID;

        internal TextBox txtCableID
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtCableID;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtCableID != null)
                {
                }

                _txtCableID = value;
                if (_txtCableID != null)
                {
                }
            }
        }

        private Label _lblID;

        internal Label lblID
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblID;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblID != null)
                {
                }

                _lblID = value;
                if (_lblID != null)
                {
                }
            }
        }

        private TextBox _txtLength;

        internal TextBox txtLength
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtLength;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtLength != null)
                {
                    _txtLength.KeyPress -= OnDoubleTextBoxKeyPress;
                }

                _txtLength = value;
                if (_txtLength != null)
                {
                    _txtLength.KeyPress += OnDoubleTextBoxKeyPress;
                }
            }
        }

        private Label _lblLength;

        internal Label lblLength
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblLength;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblLength != null)
                {
                }

                _lblLength = value;
                if (_lblLength != null)
                {
                }
            }
        }

        private TextBox _txtOperatorID;

        internal TextBox txtOperatorID
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtOperatorID;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtOperatorID != null)
                {
                }

                _txtOperatorID = value;
                if (_txtOperatorID != null)
                {
                }
            }
        }

        private Label _lblOperatorID;

        internal Label lblOperatorID
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblOperatorID;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblOperatorID != null)
                {
                }

                _lblOperatorID = value;
                if (_lblOperatorID != null)
                {
                }
            }
        }

        private GroupBox _gpbCableType;

        internal GroupBox gpbCableType
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _gpbCableType;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_gpbCableType != null)
                {
                }

                _gpbCableType = value;
                if (_gpbCableType != null)
                {
                }
            }
        }

        private RadioButton _rdbRibbon;

        internal RadioButton rdbRibbon
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdbRibbon;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdbRibbon != null)
                {
                    _rdbRibbon.CheckedChanged -= RadioButton_CheckedChanged;
                }

                _rdbRibbon = value;
                if (_rdbRibbon != null)
                {
                    _rdbRibbon.CheckedChanged += RadioButton_CheckedChanged;
                }
            }
        }

        private RadioButton _rdbLoose;

        internal RadioButton rdbLoose
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _rdbLoose;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_rdbLoose != null)
                {
                    _rdbLoose.CheckedChanged -= RadioButton_CheckedChanged;
                }

                _rdbLoose = value;
                if (_rdbLoose != null)
                {
                    _rdbLoose.CheckedChanged += RadioButton_CheckedChanged;
                }
            }
        }

        private Label _lblRibbons;

        internal Label lblRibbons
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblRibbons;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblRibbons != null)
                {
                }

                _lblRibbons = value;
                if (_lblRibbons != null)
                {
                }
            }
        }

        private TextBox _txtRibbons;

        internal TextBox txtRibbons
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _txtRibbons;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_txtRibbons != null)
                {
                    _txtRibbons.KeyPress -= OnIntegerTextBoxKeyPress;
                }

                _txtRibbons = value;
                if (_txtRibbons != null)
                {
                    _txtRibbons.KeyPress += OnIntegerTextBoxKeyPress;
                }
            }
        }

        private ComboBox _cmbRibbonType;

        internal ComboBox cmbRibbonType
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _cmbRibbonType;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_cmbRibbonType != null)
                {
                }

                _cmbRibbonType = value;
                if (_cmbRibbonType != null)
                {
                }
            }
        }

        private Label _lblRibbonType;

        internal Label lblRibbonType
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblRibbonType;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblRibbonType != null)
                {
                }

                _lblRibbonType = value;
                if (_lblRibbonType != null)
                {
                }
            }
        }

        private Button _btnAbout;

        internal Button btnAbout
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _btnAbout;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_btnAbout != null)
                {
                    _btnAbout.Click -= About_Click;
                }

                _btnAbout = value;
                if (_btnAbout != null)
                {
                    _btnAbout.Click += About_Click;
                }
            }
        }
    }
}
