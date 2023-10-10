using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace PhotonKinetics.OASYS.Examples
{
    public partial class AboutForm : Form
    {

        // Form overrides dispose to clean up the component list.
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                    components.Dispose();
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components = null;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.OKButton = new System.Windows.Forms.Button();
            this._PictureBox1 = new System.Windows.Forms.PictureBox();
            this._lblVersion = new System.Windows.Forms.Label();
            this._Copyright1 = new System.Windows.Forms.Label();
            this._lblReserved = new System.Windows.Forms.Label();
            this._lblProduct = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._PictureBox1)).BeginInit();
            this.SuspendLayout();
            //
            // OKButton
            //
            this.OKButton.Location = new System.Drawing.Point(111, 238);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 0;
            this.OKButton.Text = "&OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            //
            // _PictureBox1
            //
            this._PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("_PictureBox1.Image")));
            this._PictureBox1.Location = new System.Drawing.Point(0, 0);
            this._PictureBox1.Name = "_PictureBox1";
            this._PictureBox1.Size = new System.Drawing.Size(302, 132);
            this._PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this._PictureBox1.TabIndex = 2;
            this._PictureBox1.TabStop = false;
            //
            // _lblVersion
            //
            this._lblVersion.AutoSize = true;
            this._lblVersion.Location = new System.Drawing.Point(105, 162);
            this._lblVersion.Name = "_lblVersion";
            this._lblVersion.Size = new System.Drawing.Size(91, 15);
            this._lblVersion.TabIndex = 3;
            this._lblVersion.Text = "Version: X.X.X.X";
            //
            // _Copyright1
            //
            this._Copyright1.AutoSize = true;
            this._Copyright1.Location = new System.Drawing.Point(70, 200);
            this._Copyright1.Name = "_Copyright1";
            this._Copyright1.Size = new System.Drawing.Size(161, 15);
            this._Copyright1.TabIndex = 4;
            this._Copyright1.Text = "© 2019 Photon Kinetics, Inc.";
            //
            // _lblReserved
            //
            this._lblReserved.AutoSize = true;
            this._lblReserved.Location = new System.Drawing.Point(92, 216);
            this._lblReserved.Name = "_lblReserved";
            this._lblReserved.Size = new System.Drawing.Size(118, 15);
            this._lblReserved.TabIndex = 5;
            this._lblReserved.Text = "All Rights Reserved.";
            //
            // _lblProduct
            //
            this._lblProduct.AutoSize = true;
            this._lblProduct.Location = new System.Drawing.Point(36, 142);
            this._lblProduct.Name = "_lblProduct";
            this._lblProduct.Size = new System.Drawing.Size(190, 15);
            this._lblProduct.TabIndex = 6;
            this._lblProduct.Text = "Example Name: PreProcExample";
            //
            // AboutForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(302, 270);
            this.Controls.Add(this._lblProduct);
            this.Controls.Add(this._lblReserved);
            this.Controls.Add(this._Copyright1);
            this.Controls.Add(this._lblVersion);
            this.Controls.Add(this._PictureBox1);
            this.Controls.Add(this.OKButton);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Oasys.NET Pre-Processor Example";
            ((System.ComponentModel.ISupportInitialize)(this._PictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private Button OKButton;

        internal Button btnOK
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return OKButton;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (OKButton != null)
                {
                    OKButton.Click -= OKButton_Click;
                }

                OKButton = value;
                if (OKButton != null)
                {
                    OKButton.Click += OKButton_Click;
                }
            }
        }

        private PictureBox _PictureBox1;

        internal PictureBox PictureBox1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _PictureBox1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_PictureBox1 != null)
                {
                }

                _PictureBox1 = value;
                if (_PictureBox1 != null)
                {
                }
            }
        }

        private Label _lblVersion;

        internal Label lblVersion
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblVersion;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblVersion != null)
                {
                }

                _lblVersion = value;
                if (_lblVersion != null)
                {
                }
            }
        }

        private Label _Copyright1;

        internal Label Copyright1
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _Copyright1;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_Copyright1 != null)
                {
                }

                _Copyright1 = value;
                if (_Copyright1 != null)
                {
                }
            }
        }

        private Label _lblReserved;

        internal Label lblReserved
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblReserved;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblReserved != null)
                {
                }

                _lblReserved = value;
                if (_lblReserved != null)
                {
                }
            }
        }

        private Label _lblProduct;

        internal Label lblProduct
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return _lblProduct;
            }

            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                if (_lblProduct != null)
                {
                }

                _lblProduct = value;
                if (_lblProduct != null)
                {
                }
            }
        }
    }
}
