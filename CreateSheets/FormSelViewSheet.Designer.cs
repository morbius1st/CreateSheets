namespace CreateSheets
{
	partial class FormSelViewSheet
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelViewSheet));
			this.btnOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labNumberCopies = new System.Windows.Forms.Label();
			this.cbxCopies = new System.Windows.Forms.ComboBox();
			this.listViewSheets = new System.Windows.Forms.ListView();
			this.colHeadSheetNumber = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHeadSheetName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.labTitleBlockTitle = new System.Windows.Forms.Label();
			this.cbxTitleBlocks = new System.Windows.Forms.ComboBox();
			this.rbOppCreateSheets = new System.Windows.Forms.RadioButton();
			this.rbOppDuplicateSheets = new System.Windows.Forms.RadioButton();
			this.gbOperation = new System.Windows.Forms.GroupBox();
			this.btnHelpDupSheetsAndViews = new System.Windows.Forms.Button();
			this.rbOppDuplicateSheetsAndViews = new System.Windows.Forms.RadioButton();
			this.btnHelpCreateSheet = new System.Windows.Forms.Button();
			this.btnHelpDupSheets = new System.Windows.Forms.Button();
			this.gbSheets = new System.Windows.Forms.GroupBox();
			this.labCopyMessage = new System.Windows.Forms.Label();
			this.pbBanner = new System.Windows.Forms.PictureBox();
			this.gbNewSheetOptions = new System.Windows.Forms.GroupBox();
			this.cxbIncShtName = new System.Windows.Forms.CheckBox();
			this.lblIncShtName = new System.Windows.Forms.Label();
			this.cbxShtNumFormat = new System.Windows.Forms.ComboBox();
			this.tbShtNamePrefix = new System.Windows.Forms.TextBox();
			this.tbShtNumPrefix = new System.Windows.Forms.TextBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.labShtName = new System.Windows.Forms.Label();
			this.labShtNumFormat = new System.Windows.Forms.Label();
			this.labShtNumPrefix = new System.Windows.Forms.Label();
			this.btnAbout = new System.Windows.Forms.Button();
			this.gbNewViewNameOptions = new System.Windows.Forms.GroupBox();
			this.lblExampleText = new System.Windows.Forms.Label();
			this.lblExample = new System.Windows.Forms.Label();
			this.lblLine2 = new System.Windows.Forms.Label();
			this.lblLine1 = new System.Windows.Forms.Label();
			this.tbNewViewDivChars = new System.Windows.Forms.TextBox();
			this.tbNewViewGroupCharsRight = new System.Windows.Forms.TextBox();
			this.tbNewViewGroupCharsLeft = new System.Windows.Forms.TextBox();
			this.lblSepGrpChar = new System.Windows.Forms.Label();
			this.rbNewViewGrpChars = new System.Windows.Forms.RadioButton();
			this.rbNewViewDivChars = new System.Windows.Forms.RadioButton();
			this.rbNewViewNoAddedChars = new System.Windows.Forms.RadioButton();
			this.cxbAddSpaces = new System.Windows.Forms.CheckBox();
			this.lblAddSpaces = new System.Windows.Forms.Label();
			this.tbDate = new System.Windows.Forms.TextBox();
			this.gbOperation.SuspendLayout();
			this.gbSheets.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbBanner)).BeginInit();
			this.gbNewSheetOptions.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.gbNewViewNameOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			resources.ApplyResources(this.btnOK, "btnOK");
			this.btnOK.Name = "btnOK";
			this.btnOK.UseVisualStyleBackColor = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// buttonCancel
			// 
			resources.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			// 
			// labNumberCopies
			// 
			resources.ApplyResources(this.labNumberCopies, "labNumberCopies");
			this.labNumberCopies.Name = "labNumberCopies";
			// 
			// cbxCopies
			// 
			resources.ApplyResources(this.cbxCopies, "cbxCopies");
			this.cbxCopies.Name = "cbxCopies";
			this.cbxCopies.KeyDown += new System.Windows.Forms.KeyEventHandler(this.comboBoxCopies_KeyDown);
			this.cbxCopies.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxCopies_KeyPress);
			this.cbxCopies.Validating += new System.ComponentModel.CancelEventHandler(this.comboBoxCopies_Validating);
			// 
			// listViewSheets
			// 
			resources.ApplyResources(this.listViewSheets, "listViewSheets");
			this.listViewSheets.AutoArrange = false;
			this.listViewSheets.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.listViewSheets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colHeadSheetNumber,
            this.colHeadSheetName});
			this.listViewSheets.FullRowSelect = true;
			this.listViewSheets.GridLines = true;
			this.listViewSheets.HideSelection = false;
			this.listViewSheets.MultiSelect = false;
			this.listViewSheets.Name = "listViewSheets";
			this.listViewSheets.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.listViewSheets.UseCompatibleStateImageBehavior = false;
			this.listViewSheets.View = System.Windows.Forms.View.Details;
			// 
			// colHeadSheetNumber
			// 
			resources.ApplyResources(this.colHeadSheetNumber, "colHeadSheetNumber");
			// 
			// colHeadSheetName
			// 
			resources.ApplyResources(this.colHeadSheetName, "colHeadSheetName");
			// 
			// labTitleBlockTitle
			// 
			resources.ApplyResources(this.labTitleBlockTitle, "labTitleBlockTitle");
			this.labTitleBlockTitle.Name = "labTitleBlockTitle";
			// 
			// cbxTitleBlocks
			// 
			resources.ApplyResources(this.cbxTitleBlocks, "cbxTitleBlocks");
			this.cbxTitleBlocks.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.cbxTitleBlocks.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			this.cbxTitleBlocks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxTitleBlocks.FormattingEnabled = true;
			this.cbxTitleBlocks.Name = "cbxTitleBlocks";
			// 
			// rbOppCreateSheets
			// 
			resources.ApplyResources(this.rbOppCreateSheets, "rbOppCreateSheets");
			this.rbOppCreateSheets.Name = "rbOppCreateSheets";
			this.rbOppCreateSheets.TabStop = true;
			this.rbOppCreateSheets.UseVisualStyleBackColor = true;
			this.rbOppCreateSheets.CheckedChanged += new System.EventHandler(this.rbOppCreateSheets_CheckedChanged);
			// 
			// rbOppDuplicateSheets
			// 
			resources.ApplyResources(this.rbOppDuplicateSheets, "rbOppDuplicateSheets");
			this.rbOppDuplicateSheets.Name = "rbOppDuplicateSheets";
			this.rbOppDuplicateSheets.TabStop = true;
			this.rbOppDuplicateSheets.UseVisualStyleBackColor = true;
			this.rbOppDuplicateSheets.Click += new System.EventHandler(this.rbOppDuplicateSheets_Click);
			// 
			// gbOperation
			// 
			this.gbOperation.Controls.Add(this.btnHelpDupSheetsAndViews);
			this.gbOperation.Controls.Add(this.rbOppDuplicateSheetsAndViews);
			this.gbOperation.Controls.Add(this.btnHelpCreateSheet);
			this.gbOperation.Controls.Add(this.btnHelpDupSheets);
			this.gbOperation.Controls.Add(this.rbOppDuplicateSheets);
			this.gbOperation.Controls.Add(this.rbOppCreateSheets);
			this.gbOperation.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.gbOperation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			resources.ApplyResources(this.gbOperation, "gbOperation");
			this.gbOperation.Name = "gbOperation";
			this.gbOperation.TabStop = false;
			// 
			// btnHelpDupSheetsAndViews
			// 
			resources.ApplyResources(this.btnHelpDupSheetsAndViews, "btnHelpDupSheetsAndViews");
			this.btnHelpDupSheetsAndViews.Name = "btnHelpDupSheetsAndViews";
			this.btnHelpDupSheetsAndViews.UseVisualStyleBackColor = true;
			this.btnHelpDupSheetsAndViews.Click += new System.EventHandler(this.btnHelpDupSheetsAndViews_Click);
			// 
			// rbOppDuplicateSheetsAndViews
			// 
			resources.ApplyResources(this.rbOppDuplicateSheetsAndViews, "rbOppDuplicateSheetsAndViews");
			this.rbOppDuplicateSheetsAndViews.Name = "rbOppDuplicateSheetsAndViews";
			this.rbOppDuplicateSheetsAndViews.TabStop = true;
			this.rbOppDuplicateSheetsAndViews.UseVisualStyleBackColor = true;
			this.rbOppDuplicateSheetsAndViews.Click += new System.EventHandler(this.rbOppDuplicateSheetsAndViews_Click);
			// 
			// btnHelpCreateSheet
			// 
			resources.ApplyResources(this.btnHelpCreateSheet, "btnHelpCreateSheet");
			this.btnHelpCreateSheet.Name = "btnHelpCreateSheet";
			this.btnHelpCreateSheet.UseVisualStyleBackColor = true;
			this.btnHelpCreateSheet.Click += new System.EventHandler(this.btnCreateSheetHelp_Click);
			// 
			// btnHelpDupSheets
			// 
			resources.ApplyResources(this.btnHelpDupSheets, "btnHelpDupSheets");
			this.btnHelpDupSheets.Name = "btnHelpDupSheets";
			this.btnHelpDupSheets.UseVisualStyleBackColor = true;
			this.btnHelpDupSheets.Click += new System.EventHandler(this.btnDupSheetsHelp_Click);
			// 
			// gbSheets
			// 
			resources.ApplyResources(this.gbSheets, "gbSheets");
			this.gbSheets.Controls.Add(this.listViewSheets);
			this.gbSheets.Name = "gbSheets";
			this.gbSheets.TabStop = false;
			// 
			// labCopyMessage
			// 
			resources.ApplyResources(this.labCopyMessage, "labCopyMessage");
			this.labCopyMessage.Name = "labCopyMessage";
			// 
			// pbBanner
			// 
			resources.ApplyResources(this.pbBanner, "pbBanner");
			this.pbBanner.Name = "pbBanner";
			this.pbBanner.TabStop = false;
			// 
			// gbNewSheetOptions
			// 
			this.gbNewSheetOptions.Controls.Add(this.cxbIncShtName);
			this.gbNewSheetOptions.Controls.Add(this.lblIncShtName);
			this.gbNewSheetOptions.Controls.Add(this.cbxShtNumFormat);
			this.gbNewSheetOptions.Controls.Add(this.tbShtNamePrefix);
			this.gbNewSheetOptions.Controls.Add(this.tbShtNumPrefix);
			this.gbNewSheetOptions.Controls.Add(this.pictureBox2);
			this.gbNewSheetOptions.Controls.Add(this.pictureBox1);
			this.gbNewSheetOptions.Controls.Add(this.labShtName);
			this.gbNewSheetOptions.Controls.Add(this.labShtNumFormat);
			this.gbNewSheetOptions.Controls.Add(this.labShtNumPrefix);
			this.gbNewSheetOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			resources.ApplyResources(this.gbNewSheetOptions, "gbNewSheetOptions");
			this.gbNewSheetOptions.Name = "gbNewSheetOptions";
			this.gbNewSheetOptions.TabStop = false;
			// 
			// cxbIncShtName
			// 
			resources.ApplyResources(this.cxbIncShtName, "cxbIncShtName");
			this.cxbIncShtName.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.cxbIncShtName.FlatAppearance.BorderSize = 2;
			this.cxbIncShtName.Name = "cxbIncShtName";
			this.cxbIncShtName.UseVisualStyleBackColor = true;
			this.cxbIncShtName.Click += new System.EventHandler(this.tb_TextChanged);
			// 
			// lblIncShtName
			// 
			resources.ApplyResources(this.lblIncShtName, "lblIncShtName");
			this.lblIncShtName.Name = "lblIncShtName";
			// 
			// cbxShtNumFormat
			// 
			this.cbxShtNumFormat.CausesValidation = false;
			this.cbxShtNumFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbxShtNumFormat.DropDownWidth = 104;
			resources.ApplyResources(this.cbxShtNumFormat, "cbxShtNumFormat");
			this.cbxShtNumFormat.FormattingEnabled = true;
			this.cbxShtNumFormat.Items.AddRange(new object[] {
            resources.GetString("cbxShtNumFormat.Items"),
            resources.GetString("cbxShtNumFormat.Items1"),
            resources.GetString("cbxShtNumFormat.Items2"),
            resources.GetString("cbxShtNumFormat.Items3"),
            resources.GetString("cbxShtNumFormat.Items4")});
			this.cbxShtNumFormat.Name = "cbxShtNumFormat";
			this.cbxShtNumFormat.TextChanged += new System.EventHandler(this.tb_TextChanged);
			// 
			// tbShtNamePrefix
			// 
			this.tbShtNamePrefix.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tbShtNamePrefix, "tbShtNamePrefix");
			this.tbShtNamePrefix.Name = "tbShtNamePrefix";
			this.tbShtNamePrefix.TextChanged += new System.EventHandler(this.tb_TextChanged);
			// 
			// tbShtNumPrefix
			// 
			this.tbShtNumPrefix.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tbShtNumPrefix, "tbShtNumPrefix");
			this.tbShtNumPrefix.Name = "tbShtNumPrefix";
			this.tbShtNumPrefix.TextChanged += new System.EventHandler(this.tb_TextChanged);
			// 
			// pictureBox2
			// 
			this.pictureBox2.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.pictureBox2, "pictureBox2");
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.TabStop = false;
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// labShtName
			// 
			resources.ApplyResources(this.labShtName, "labShtName");
			this.labShtName.Name = "labShtName";
			// 
			// labShtNumFormat
			// 
			resources.ApplyResources(this.labShtNumFormat, "labShtNumFormat");
			this.labShtNumFormat.Name = "labShtNumFormat";
			// 
			// labShtNumPrefix
			// 
			resources.ApplyResources(this.labShtNumPrefix, "labShtNumPrefix");
			this.labShtNumPrefix.Name = "labShtNumPrefix";
			// 
			// btnAbout
			// 
			resources.ApplyResources(this.btnAbout, "btnAbout");
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.UseVisualStyleBackColor = true;
			this.btnAbout.Click += new System.EventHandler(this.bthAbout_Click);
			// 
			// gbNewViewNameOptions
			// 
			resources.ApplyResources(this.gbNewViewNameOptions, "gbNewViewNameOptions");
			this.gbNewViewNameOptions.Controls.Add(this.lblExampleText);
			this.gbNewViewNameOptions.Controls.Add(this.lblExample);
			this.gbNewViewNameOptions.Controls.Add(this.lblLine2);
			this.gbNewViewNameOptions.Controls.Add(this.lblLine1);
			this.gbNewViewNameOptions.Controls.Add(this.tbNewViewDivChars);
			this.gbNewViewNameOptions.Controls.Add(this.tbNewViewGroupCharsRight);
			this.gbNewViewNameOptions.Controls.Add(this.tbNewViewGroupCharsLeft);
			this.gbNewViewNameOptions.Controls.Add(this.lblSepGrpChar);
			this.gbNewViewNameOptions.Controls.Add(this.rbNewViewGrpChars);
			this.gbNewViewNameOptions.Controls.Add(this.rbNewViewDivChars);
			this.gbNewViewNameOptions.Controls.Add(this.rbNewViewNoAddedChars);
			this.gbNewViewNameOptions.Controls.Add(this.cxbAddSpaces);
			this.gbNewViewNameOptions.Controls.Add(this.lblAddSpaces);
			this.gbNewViewNameOptions.Name = "gbNewViewNameOptions";
			this.gbNewViewNameOptions.TabStop = false;
			// 
			// lblExampleText
			// 
			resources.ApplyResources(this.lblExampleText, "lblExampleText");
			this.lblExampleText.Name = "lblExampleText";
			// 
			// lblExample
			// 
			resources.ApplyResources(this.lblExample, "lblExample");
			this.lblExample.Name = "lblExample";
			// 
			// lblLine2
			// 
			resources.ApplyResources(this.lblLine2, "lblLine2");
			this.lblLine2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLine2.Name = "lblLine2";
			// 
			// lblLine1
			// 
			resources.ApplyResources(this.lblLine1, "lblLine1");
			this.lblLine1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblLine1.Name = "lblLine1";
			// 
			// tbNewViewDivChars
			// 
			this.tbNewViewDivChars.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tbNewViewDivChars, "tbNewViewDivChars");
			this.tbNewViewDivChars.Name = "tbNewViewDivChars";
			this.tbNewViewDivChars.TextChanged += new System.EventHandler(this.tb_TextChanged);
			// 
			// tbNewViewGroupCharsRight
			// 
			this.tbNewViewGroupCharsRight.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tbNewViewGroupCharsRight, "tbNewViewGroupCharsRight");
			this.tbNewViewGroupCharsRight.Name = "tbNewViewGroupCharsRight";
			this.tbNewViewGroupCharsRight.TextChanged += new System.EventHandler(this.tb_TextChanged);
			// 
			// tbNewViewGroupCharsLeft
			// 
			this.tbNewViewGroupCharsLeft.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tbNewViewGroupCharsLeft, "tbNewViewGroupCharsLeft");
			this.tbNewViewGroupCharsLeft.Name = "tbNewViewGroupCharsLeft";
			this.tbNewViewGroupCharsLeft.TextChanged += new System.EventHandler(this.tb_TextChanged);
			// 
			// lblSepGrpChar
			// 
			resources.ApplyResources(this.lblSepGrpChar, "lblSepGrpChar");
			this.lblSepGrpChar.Name = "lblSepGrpChar";
			// 
			// rbNewViewGrpChars
			// 
			resources.ApplyResources(this.rbNewViewGrpChars, "rbNewViewGrpChars");
			this.rbNewViewGrpChars.Name = "rbNewViewGrpChars";
			this.rbNewViewGrpChars.TabStop = true;
			this.rbNewViewGrpChars.UseVisualStyleBackColor = true;
			this.rbNewViewGrpChars.CheckedChanged += new System.EventHandler(this.rbNewViewGrpChars_CheckedChanged);
			// 
			// rbNewViewDivChars
			// 
			resources.ApplyResources(this.rbNewViewDivChars, "rbNewViewDivChars");
			this.rbNewViewDivChars.Name = "rbNewViewDivChars";
			this.rbNewViewDivChars.TabStop = true;
			this.rbNewViewDivChars.UseVisualStyleBackColor = true;
			this.rbNewViewDivChars.CheckedChanged += new System.EventHandler(this.rbNewViewDivChars_CheckedChanged);
			// 
			// rbNewViewNoAddedChars
			// 
			resources.ApplyResources(this.rbNewViewNoAddedChars, "rbNewViewNoAddedChars");
			this.rbNewViewNoAddedChars.Name = "rbNewViewNoAddedChars";
			this.rbNewViewNoAddedChars.TabStop = true;
			this.rbNewViewNoAddedChars.UseVisualStyleBackColor = true;
			this.rbNewViewNoAddedChars.CheckedChanged += new System.EventHandler(this.rbNewViewNoAddedChars_CheckedChanged);
			// 
			// cxbAddSpaces
			// 
			resources.ApplyResources(this.cxbAddSpaces, "cxbAddSpaces");
			this.cxbAddSpaces.FlatAppearance.BorderColor = System.Drawing.Color.White;
			this.cxbAddSpaces.FlatAppearance.BorderSize = 2;
			this.cxbAddSpaces.Name = "cxbAddSpaces";
			this.cxbAddSpaces.UseVisualStyleBackColor = true;
			this.cxbAddSpaces.Click += new System.EventHandler(this.cxbAddSpaces_Click);
			// 
			// lblAddSpaces
			// 
			resources.ApplyResources(this.lblAddSpaces, "lblAddSpaces");
			this.lblAddSpaces.Name = "lblAddSpaces";
			// 
			// tbDate
			// 
			this.tbDate.BackColor = System.Drawing.SystemColors.Control;
			this.tbDate.BorderStyle = System.Windows.Forms.BorderStyle.None;
			resources.ApplyResources(this.tbDate, "tbDate");
			this.tbDate.Name = "tbDate";
			// 
			// FormSelViewSheet
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ControlBox = false;
			this.Controls.Add(this.tbDate);
			this.Controls.Add(this.gbNewViewNameOptions);
			this.Controls.Add(this.btnAbout);
			this.Controls.Add(this.gbNewSheetOptions);
			this.Controls.Add(this.pbBanner);
			this.Controls.Add(this.labCopyMessage);
			this.Controls.Add(this.gbSheets);
			this.Controls.Add(this.gbOperation);
			this.Controls.Add(this.cbxTitleBlocks);
			this.Controls.Add(this.labTitleBlockTitle);
			this.Controls.Add(this.cbxCopies);
			this.Controls.Add(this.labNumberCopies);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "FormSelViewSheet";
			this.ShowInTaskbar = false;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelViewSheetForm_FormClosing);
			this.Load += new System.EventHandler(this.SelViewSheetForm_Load);
			this.gbOperation.ResumeLayout(false);
			this.gbOperation.PerformLayout();
			this.gbSheets.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbBanner)).EndInit();
			this.gbNewSheetOptions.ResumeLayout(false);
			this.gbNewSheetOptions.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.gbNewViewNameOptions.ResumeLayout(false);
			this.gbNewViewNameOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Label labNumberCopies;
		private System.Windows.Forms.ComboBox cbxCopies;
		private System.Windows.Forms.ListView listViewSheets;
		private System.Windows.Forms.ColumnHeader colHeadSheetNumber;
		private System.Windows.Forms.ColumnHeader colHeadSheetName;
		private System.Windows.Forms.Label labTitleBlockTitle;
		private System.Windows.Forms.ComboBox cbxTitleBlocks;
		private System.Windows.Forms.RadioButton rbOppCreateSheets;
		private System.Windows.Forms.RadioButton rbOppDuplicateSheets;
		private System.Windows.Forms.GroupBox gbOperation;
		private System.Windows.Forms.GroupBox gbSheets;
		private System.Windows.Forms.Label labCopyMessage;
		private System.Windows.Forms.PictureBox pbBanner;
		private System.Windows.Forms.Button btnHelpCreateSheet;
		private System.Windows.Forms.Button btnHelpDupSheets;
		private System.Windows.Forms.GroupBox gbNewSheetOptions;
		private System.Windows.Forms.Label labShtNumPrefix;
		private System.Windows.Forms.Label labShtName;
		private System.Windows.Forms.Label labShtNumFormat;
		private System.Windows.Forms.PictureBox pictureBox2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ComboBox cbxShtNumFormat;
		private System.Windows.Forms.TextBox tbShtNamePrefix;
		private System.Windows.Forms.TextBox tbShtNumPrefix;
		private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.CheckBox cxbIncShtName;
		private System.Windows.Forms.Label lblIncShtName;
		private System.Windows.Forms.Button btnHelpDupSheetsAndViews;
		private System.Windows.Forms.RadioButton rbOppDuplicateSheetsAndViews;
		private System.Windows.Forms.GroupBox gbNewViewNameOptions;
		private System.Windows.Forms.Label lblLine1;
		private System.Windows.Forms.TextBox tbNewViewDivChars;
		private System.Windows.Forms.TextBox tbNewViewGroupCharsRight;
		private System.Windows.Forms.TextBox tbNewViewGroupCharsLeft;
		private System.Windows.Forms.Label lblSepGrpChar;
		private System.Windows.Forms.RadioButton rbNewViewGrpChars;
		private System.Windows.Forms.RadioButton rbNewViewDivChars;
		private System.Windows.Forms.RadioButton rbNewViewNoAddedChars;
		private System.Windows.Forms.CheckBox cxbAddSpaces;
		private System.Windows.Forms.Label lblAddSpaces;
		private System.Windows.Forms.Label lblExample;
		private System.Windows.Forms.Label lblLine2;
		private System.Windows.Forms.Label lblExampleText;
		private System.Windows.Forms.TextBox tbDate;
	}
}