using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using SharedCode;
using SharedCode.Resources;


namespace CreateSheets
{

	public partial class FormSelViewSheet : System.Windows.Forms.Form
	{
		private ShDbMgr _DBMgr;

		private int intSelItemIdx = 0;
		private int intFromSheetIdx;

		private const string SHEETTITLEBLOCK = "<from sheet>";
		private const string NOTITLEBLOCK = "<no titleblock>";

		private const string FMTVIEWNAME_DIVIDESTRING = "-";
		private const string FMTVIEWNAME_SURROUNDSTRING_LEFT = "(";
		private const string FMTVIEWNAME_SURROUNDSTRING_RIGHT = ")";

		private string exampleViewName = "Floor Plan";

		enum OpType {DUPLICATESHEET, DUPLICATESHEETANDVIEWS, CREATESHEET };

		private ShDbMgr.FmtViewName _groupDivideOrNone;
		private static OpType _operation = OpType.DUPLICATESHEET;
		private static bool _addSpaces = true;


		private bool ProcessKeyPressCheck = false;


		public FormSelViewSheet(ExternalCommandData commandData)
		{
			_DBMgr = new ShDbMgr(commandData);

//			Debug.Print("********** Initializing Form **********");

			InitializeComponent();

			// pre-select the sheet number format combo box
			cbxShtNumFormat.SelectedIndex = 1;

			tbNewViewDivChars.Text = FMTVIEWNAME_DIVIDESTRING;
			tbNewViewGroupCharsLeft.Text = FMTVIEWNAME_SURROUNDSTRING_LEFT;
			tbNewViewGroupCharsRight.Text = FMTVIEWNAME_SURROUNDSTRING_RIGHT;

			tbDate.Text = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location).ToString("G");

			cxbAddSpaces.Checked = true;

			rbNewViewNoAddedChars.Checked = true;

			SetNewViewNameFmtOpts();
		}

		private void SetNewViewNameFmtOpts()
		{
			_DBMgr.SetFmtAddSpace = cxbAddSpaces.Checked;
			_DBMgr.SetDivideString = tbNewViewDivChars.Text;
			_DBMgr.SetGroupStringLeft = tbNewViewGroupCharsLeft.Text;
			_DBMgr.SetGroupStringRight = tbNewViewGroupCharsRight.Text;
			_DBMgr.SetGroupDivideOrNone = _groupDivideOrNone;

			_DBMgr.SheetNameIncrement = cxbIncShtName.Checked;
			_DBMgr.SheetNamePrefix = tbShtNamePrefix.Text;
			_DBMgr.SheetNumPrefix = tbShtNumPrefix.Text;
			_DBMgr.SheetNumFormat = cbxShtNumFormat.Text;

		}

		private bool SetupSheetList()
		{

			ListViewItem lvItem;
			ListViewItem lvItemSave = null;

			bool boolIsViewSheet = false;

			// make sure that we have some sheets to show
			if (_DBMgr.SheetCount == 0)
			{
				return false;
			}
			
			
			// get the ActiveView
			Autodesk.Revit.DB.View vActive = _DBMgr.ActiveView;

			if (vActive.ViewType == ViewType.DrawingSheet)
			{
				boolIsViewSheet = true;
			}

			

			// clear the list box
			listViewSheets.Items.Clear();

			// fill the list box
			foreach (ViewSheet vs in _DBMgr.AllViewSheets())
			{
				// create the ListViewItem for the row - sheet number first
				lvItem = new ListViewItem(vs.SheetNumber);
				// add the sheet name
				lvItem.SubItems.Add(vs.Name);

				// add the ListViewItem to the ListView control
				listViewSheets.Items.Add(lvItem);

				// check to see if ViewSheet matches the ActiveView
				if ((boolIsViewSheet) && (vActive.UniqueId.Equals(vs.UniqueId)))
				{
					lvItemSave = lvItem;
				}
			}

			// do we pre-select a sheet
			if (lvItemSave != null)
			{
				// a sheetview is the active view - preselect
				intSelItemIdx = listViewSheets.Items.IndexOf(lvItemSave);
				listViewSheets.TopItem = lvItemSave;
			}

			SelectSheet(intSelItemIdx);

			// setup and populated
			return true;
		}

		private void SelectSheet(int intSelItemIdx)
		{
			// select an item in the list
			listViewSheets.Items[intSelItemIdx].Selected = true;
			listViewSheets.Select();
		}

		private void DisableNewViewNameOptions()
		{
			gbNewViewNameOptions.Enabled = false;
		}

		private void EnableNewViewNameOptions()
		{
			gbNewViewNameOptions.Enabled = true;
		}


		private void SetupCopies()
		{
			int i;

			// setup the number of copies combo box
			// clear the contents to start
			cbxCopies.Items.Clear();

			// fill with numbers 1 to 99
			for (i = 1; i < 26; i++)
			{
				cbxCopies.Items.Add(i.ToString());
			}

			// pre-select the first item
			cbxCopies.SelectedIndex = 0;
		}

		// fill in the combo box with the list of title blocks
		// along with the one or two special options
		private void SetupTitleBlockList()
		{

			int offset = 0;

			intFromSheetIdx = -1;
//			intFirstTbIdx = -1;

			// add the 2 special options
			// option 1: no title block
			cbxTitleBlocks.Items.Add(NOTITLEBLOCK);

			// option 2: currently selected sheet
			// we have some sheets - list that as an option
			if (_DBMgr.SheetCount > 0)
			{
				offset++;
				intFromSheetIdx = offset;
				cbxTitleBlocks.Items.Add(SHEETTITLEBLOCK);
			}

			// got title blocks to add to the list?
			if (_DBMgr.TitleBlockCount > 0)
			{
				offset++;
//				intFirstTbIdx = offset;
				// fill-in the list of titleblocks
				foreach (FamilySymbol fs in _DBMgr.GetAllTitleBlocks())
				{
					cbxTitleBlocks.Items.Add(ShDbMgr.FormatTitleBlockName(fs));
				}
			}

			// pre-select the first item in the list
			// which may be the only item
			cbxTitleBlocks.SelectedIndex = 0 + offset;

			//return true;
		}

		// track which radio button has been seleted
		private OpType ProcessOppRadioButtons()
		{
			// assume rbCreateSheets is checked
			// check for other radio buttons being checked.
			OpType Operation = OpType.CREATESHEET;

			if (rbOppDuplicateSheets.Checked)
			{
				Operation = OpType.DUPLICATESHEET;
			}

			// note rbCreateSheets radio button is the default (=0)
			// so if none of the above is checked, the default
			// value is returned
			return Operation;
		}

		private void SelTitleBlock()
		{
			if (rbOppDuplicateSheets.Checked && intFromSheetIdx != -1)
			{
				cbxTitleBlocks.SelectedIndex = intFromSheetIdx;
			}
		}

		private void ShowSelectSheetErrorMessage()
		{
			MessageBox.Show("A sheet is not selected\n\nPlease select a sheet to proceed",
				"Duplicate Sheets",
				MessageBoxButtons.OK,
				MessageBoxIcon.Error,
				MessageBoxDefaultButton.Button1);
		}

		// method to validate the copies combo box - only numbers (and editing)
		private void comboBoxCopies_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (ProcessKeyPressCheck)
			{
				// only allow numeric entry
				if ((e.KeyChar < '0') || (e.KeyChar > '9'))
				{
					// non-numeric - tell system that the keypress was "handled"
					e.Handled = true;
				}
			}
		}

		// method to validate copies combo box - don't prevent editing keys
		private void comboBoxCopies_KeyDown(object sender, KeyEventArgs e)
		{
			ProcessKeyPressCheck = true;
	
			if (e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete
				|| e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
			{
				ProcessKeyPressCheck = false;
			}
		}

		// method to validate copies combo box - don't allow copies to be 0
		private void comboBoxCopies_Validating(object sender, CancelEventArgs e)
		{
			labCopyMessage.Text = "";

			if (cbxCopies.Text == "" || int.Parse(cbxCopies.Text) == 0)
			{
				cbxCopies.Text = "1";
			}
		}

		private void rbOppDuplicateSheets_Click(object sender, EventArgs e)
		{
			_operation = OpType.DUPLICATESHEET;

			SelTitleBlock();
			DisableNewViewNameOptions();
		}


		private void rbOppDuplicateSheetsAndViews_Click(object sender, EventArgs e)
		{
			_operation = OpType.DUPLICATESHEETANDVIEWS;

			SelTitleBlock();
			EnableNewViewNameOptions();
		}

		private void rbOppCreateSheets_CheckedChanged(object sender, EventArgs e)
		{
			_operation = OpType.CREATESHEET;

			DisableNewViewNameOptions();
		}

		private void rbNewViewNoAddedChars_CheckedChanged(object sender, EventArgs e)
		{
			_groupDivideOrNone = ShDbMgr.FmtViewName.NONE;

			UpdateExampleText();
		}

		private void rbNewViewDivChars_CheckedChanged(object sender, EventArgs e)
		{
			_groupDivideOrNone = ShDbMgr.FmtViewName.DIVIDE;

			UpdateExampleText();
		}

		private void rbNewViewGrpChars_CheckedChanged(object sender, EventArgs e)
		{
			_groupDivideOrNone = ShDbMgr.FmtViewName.GROUP;

			UpdateExampleText();
		}

		private void cxbAddSpaces_Click(object sender, EventArgs e)
		{
			UpdateExampleText();
		}

		private void tb_TextChanged(object sender, EventArgs e)
		{
			UpdateExampleText();
		}

		private void UpdateExampleText()
		{
			SetNewViewNameFmtOpts();

			lblExampleText.Text = _DBMgr.FormatViewName(exampleViewName, 
				_DBMgr.FormatSheetNumber(0), 0);
		}

		private void btnDupSheetsHelp_Click(object sender, EventArgs e)
		{
//			showHelpMessage(Properties.Resources.HelpDuplicateSheet, "Duplicate Sheet");

			TaskDialog.Show("Duplicate Sheet", AppStrings.R_HelpDuplicateSheet);
		}

		private void btnHelpDupSheetsAndViews_Click(object sender, EventArgs e)
		{
//			showHelpMessage(Properties.Resources.HelpDuplicateSheetAndViews, "Duplicate Sheet and Views");

			TaskDialog.Show("Duplicate Sheet and Views", AppStrings.R_HelpDuplicateSheetAndViews);
		}

		private void btnCreateSheetHelp_Click(object sender, EventArgs e)
		{
//			showHelpMessage(Properties.Resources.HelpCreateSheets, "Create Sheets");

			TaskDialog.Show("Create Sheets", AppStrings.R_HelpCreateSheets);
		}

		private void showHelpMessage(string message, string title)
		{
			MessageBox.Show(message, title,
							MessageBoxButtons.OK,
							MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button1);
		}

		private void bthAbout_Click(object sender, EventArgs e)
		{
			using (FormAbout formAbout = new FormAbout())
			{
				formAbout.ShowDialog();
			}
		}

		// process the OK button click - make sure that we can finish the dialog
		private void btnOK_Click(object sender, EventArgs e)
		{

			// there are 2 modes of operation
			// create new blank sheets or
			// duplicate existing sheets

			// determine which radio button is checked
			OpType Operation = ProcessOppRadioButtons();

			// make sure that we can proceed first - must compensate for
			// dialog box querks - list view can "deselect" a sheet without
			// the user realizing that this has been done

			// here we prevent proceeding when we cannot
			// condition one - if no sheets are selected but the CreateSheet operation
			// wants to use a selected sheet to determine the title block - cannot proceed

			// condition two - if no sheets are selected but the DuplicateSheet operation
			// is selected - cannot proceed

			if (listViewSheets.SelectedItems.Count == 0)
			{
				// can this ever happen?
				if ((Operation == OpType.CREATESHEET) && (cbxTitleBlocks.Text.Equals(SHEETTITLEBLOCK)))
				{
					// cannot proceed - tell user and cancel exiting form
					ShowSelectSheetErrorMessage();
					return;
				}
				else
				{
					if (Operation == OpType.DUPLICATESHEET)
					{
						// cannot proceed - tell user and cancel exiting form
						ShowSelectSheetErrorMessage();
						return;
					}
				}
			}

			// ok to proceed
			DialogResult = DialogResult.OK;

		}

		private void SetNewViewNameFormat()
		{
			switch (_groupDivideOrNone)
			{
				case ShDbMgr.FmtViewName.NONE:
					rbNewViewNoAddedChars.Checked = true;
					break;
				case ShDbMgr.FmtViewName.DIVIDE:
					rbNewViewDivChars.Checked = true;
					break;
				case ShDbMgr.FmtViewName.GROUP:
					rbNewViewGrpChars.Checked = true;
					break;
			}
		}


		private void SetOperation()
		{
			switch (_operation)
			{
				case OpType.DUPLICATESHEET:
					rbOppDuplicateSheets.Checked = true;
					DisableNewViewNameOptions();
					break;
				case OpType.DUPLICATESHEETANDVIEWS:
					rbOppDuplicateSheetsAndViews.Checked = true;
					EnableNewViewNameOptions();
					break;
				case OpType.CREATESHEET:
					rbOppCreateSheets.Checked = true;
					DisableNewViewNameOptions();
					break;
			}
		}

		private void SelViewSheetForm_Load(object sender, EventArgs e)
		{
			// setup the list of title blocks
			SetupTitleBlockList();

			// configure based on current operation type
			SetOperation();
			// confiture based on current format type
			SetNewViewNameFormat();

			SelTitleBlock();

			// setup and populate the Sheet ListView
			if (!SetupSheetList())
			{
				// no sheets available - disable some controls
				// the list view is disabled
				listViewSheets.Enabled = false;
				// the group box get disabled
				gbSheets.Enabled = false;
				// the radio button to duplicate sheets gets disabled
				rbOppDuplicateSheets.Enabled = false;
				// track - no sheets
				//gotSheets = false;
			}

			// setup the Copies ComboBox
			SetupCopies();

			

			// pre-set the message to no message
			labCopyMessage.Text = "";

			// need to load the image into the picture box
			pbBanner.Image = ShUtil.GetImage("AO-LOGO-Bar-Sm.png");

		}

		private void SelViewSheetForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// if the form was canceled - quit
			if (DialogResult == DialogResult.Cancel)
			{
				return;
			}

			// get here - we are ready to proceed

			// there are 2 modes of operation
			// create new blank sheets or
			// duplicate existing sheets

			// determine which radio button is checked
//			OpType Operation = ProcessOppRadioButtons();

			ViewSheet vs = null;

			string tbName = cbxTitleBlocks.Text;
			int copies = int.Parse(cbxCopies.Text);


			try
			{
				// must determine a title block (and get the ViewSheet)
				if (tbName.Equals(SHEETTITLEBLOCK)
					|| _operation == OpType.DUPLICATESHEET
					|| _operation == OpType.DUPLICATESHEETANDVIEWS)
				{
					vs = _DBMgr.FindExistSheet(listViewSheets.SelectedItems[0].Text);
				}

				if (tbName.Equals(SHEETTITLEBLOCK))
				{
					tbName = _DBMgr.GetTitleBlockFromSheet(vs);
				}
				else if (tbName.Equals(NOTITLEBLOCK))
				{
					tbName = null;
				}

//				// process the operation
//				switch (_operation)
//				{
//				case OpType.CREATESHEET:
//					_DBMgr.CreateEmptySheets(tbName, copies, tbShtNumPrefix.Text, cbxShtNumFormat.Text, tbShtNamePrefix.Text,
//						cxbIncShtName.Checked);
//					break;
//				case OpType.DUPLICATESHEET:
//					_DBMgr.DuplicateSheet(vs, tbName, copies, tbShtNumPrefix.Text, cbxShtNumFormat.Text, tbShtNamePrefix.Text,
//						cxbIncShtName.Checked, false);
//					break;
//				case OpType.DUPLICATESHEETANDVIEWS:
//						SetNewViewNameFmtOpts();
//					_DBMgr.DuplicateSheet(vs, tbName, copies, tbShtNumPrefix.Text, cbxShtNumFormat.Text, tbShtNamePrefix.Text,
//						cxbIncShtName.Checked, true);
//					break;
//				}
				// process the operation
//				switch (_operation)
//				{
//				case OpType.CREATESHEET:
//					_DBMgr.CreateEmptySheets(tbName, copies);
//					break;
//				case OpType.DUPLICATESHEET:
//					_DBMgr.DuplicateSheet(vs, tbName, copies, false);
//					break;
//				case OpType.DUPLICATESHEETANDVIEWS:
//						SetNewViewNameFmtOpts();
//					_DBMgr.DuplicateSheet(vs, tbName, copies, true);
//					break;
//				}
			}
			// catch all exceptions at thie point - anything that has gone
			// wrong means that we cancel the operation & dialog & transaction
			catch (Exception)
			{
				DialogResult = DialogResult.Cancel;
			}

		}

	}
}
