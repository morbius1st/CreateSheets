namespace CreateSheets
{
	partial class FormAbout
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
			this.pbCreateSheetsLogo = new System.Windows.Forms.PictureBox();
			this.pbCyberStudioLogo = new System.Windows.Forms.PictureBox();
			this.tbAbout = new System.Windows.Forms.TextBox();
			this.linkToCyberStudio = new System.Windows.Forms.LinkLabel();
			this.btnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pbCreateSheetsLogo)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pbCyberStudioLogo)).BeginInit();
			this.SuspendLayout();
			// 
			// pbCreateSheetsLogo
			// 
			this.pbCreateSheetsLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbCreateSheetsLogo.Image")));
			this.pbCreateSheetsLogo.Location = new System.Drawing.Point(75, 0);
			this.pbCreateSheetsLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pbCreateSheetsLogo.Name = "pbCreateSheetsLogo";
			this.pbCreateSheetsLogo.Size = new System.Drawing.Size(328, 140);
			this.pbCreateSheetsLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbCreateSheetsLogo.TabIndex = 1;
			this.pbCreateSheetsLogo.TabStop = false;
			// 
			// pbCyberStudioLogo
			// 
			this.pbCyberStudioLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbCyberStudioLogo.Image")));
			this.pbCyberStudioLogo.Location = new System.Drawing.Point(0, 0);
			this.pbCyberStudioLogo.Margin = new System.Windows.Forms.Padding(0);
			this.pbCyberStudioLogo.Name = "pbCyberStudioLogo";
			this.pbCyberStudioLogo.Size = new System.Drawing.Size(77, 140);
			this.pbCyberStudioLogo.TabIndex = 0;
			this.pbCyberStudioLogo.TabStop = false;
			// 
			// tbAbout
			// 
			this.tbAbout.BackColor = System.Drawing.SystemColors.Control;
			this.tbAbout.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tbAbout.Enabled = false;
			this.tbAbout.Location = new System.Drawing.Point(8, 146);
			this.tbAbout.Multiline = true;
			this.tbAbout.Name = "tbAbout";
			this.tbAbout.Size = new System.Drawing.Size(387, 65);
			this.tbAbout.TabIndex = 2;
			this.tbAbout.Text = resources.GetString("tbAbout.Text");
			// 
			// linkToCyberStudio
			// 
			this.linkToCyberStudio.AutoSize = true;
			this.linkToCyberStudio.Location = new System.Drawing.Point(6, 219);
			this.linkToCyberStudio.Margin = new System.Windows.Forms.Padding(3, 3, 0, 0);
			this.linkToCyberStudio.Name = "linkToCyberStudio";
			this.linkToCyberStudio.Size = new System.Drawing.Size(91, 13);
			this.linkToCyberStudio.TabIndex = 3;
			this.linkToCyberStudio.TabStop = true;
			this.linkToCyberStudio.Text = "CyberStudio Apps";
			this.linkToCyberStudio.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkToCyberStudio_LinkClicked);
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(317, 218);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// FormAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 250);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.linkToCyberStudio);
			this.Controls.Add(this.tbAbout);
			this.Controls.Add(this.pbCreateSheetsLogo);
			this.Controls.Add(this.pbCyberStudioLogo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAbout";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			((System.ComponentModel.ISupportInitialize)(this.pbCreateSheetsLogo)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pbCyberStudioLogo)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pbCreateSheetsLogo;
		private System.Windows.Forms.PictureBox pbCyberStudioLogo;
		private System.Windows.Forms.TextBox tbAbout;
		private System.Windows.Forms.LinkLabel linkToCyberStudio;
		private System.Windows.Forms.Button btnOK;

	}
}
