using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using ComboBox = System.Windows.Controls.ComboBox;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using static DuplicateSheets2017.SettingsUser;

using SharedCode;
using SharedCode.Resources;
using UtilityLibrary;
using static SharedCode.ShNamePartType;
using static SharedCode.ShNamePartItemCode;
using static SharedCode.ShSheetDataList;
using static SharedCode.ShNewSheetMgr;

using static DuplicateSheets2017.Command;

using Binding = System.Windows.Data.Binding;

using static UtilityLibrary.MessageUtilities2;
using static UtilityLibrary.Balloon;


namespace DuplicateSheets2017
{
	/// <summary>
	/// Interaction logic for WpfSelViewSheet.xaml
	/// </summary>
	public partial class WpfSelViewSheet : Window, INotifyPropertyChanged
	{
		#region + Data

		// elements to track per custom string
		// ShNamePartItems
		// custom string
		// current selected index
		// label text
		// combo box

		private const int COPIES_START = 1;
		private  const int COPIES_END = 100;

//		private ShDbMgr _DBMgr;
		private ShNamePartItemsTables cbi;
		private NewSheetFormat SavedSettings;

		private readonly ComboBox[] _comboBoxes      = new ComboBox[ShNamePartItemsTables.CBX_QTY];
		private string[]   _customText      = new string[ShNamePartItemsTables.CBX_QTY];

//		private int _copies;
		private bool _initalized;

		public SharedCode.ShData shData = new SharedCode.ShData();

		#endregion

		#region + Constructor

		/// <summary>
		/// Entry point
		/// </summary>
		/// <param name="commandData"></param>
		public WpfSelViewSheet(ExternalCommandData commandData)
		{
			// keep track of window up and running
			_initalized = false;

			// needs to be setup before initalization of the controls
			cbi = ShNamePartItemsTables.Instance;

			//			Thread.CurrentThread.CurrentUICulture = new CultureInfo("fr");
			//
			//			WpfWindows.Culture = new CultureInfo("fr");

			// initalize all of the components
			InitializeComponent();

			InitWinLocation();
		}

		#endregion

		#region + initalize

		private void InitCbx()
		{
			// setup the comboboxes
			ConfigCbxItems(cbxCurNumDvChars, CUR_NUMDIVCHARS_TBL);
			ConfigCbxItems(cbxCurNumSx, CUR_NUMSUFFIX_TBL);
			ConfigCbxItems(cbxCurNamDvChars, CUR_NAMEDIVCHARS_TBL);
			ConfigCbxItems(cbxCurNamSx, CUR_NAMESUFFIX_TBL);
			ConfigCbxItems(cbxSetNumFmt, SET_NUMSUFFIX_TBL);


//			cbxCurNumDvChars.SetBinding(Selector.SelectedItemProperty,
//				new Binding("NumDivCharsCbxSelected"));

		}

		private void InitShtList()
		{
			if (!InitSheetList())
			{
				// no sheets - disable some controls
				lbxSheets.IsEnabled = false;

				// disable ok button
				btnProceed.IsEnabled = false;
			}
			else
			{
				// make sure that these controls are enabled
				lbxSheets.IsEnabled = true;

				// enable ok button
				btnProceed.IsEnabled = true;
			}
		}

		// create a list of sheets in the model
		// auto loaded into the listbox
		private bool InitSheetList()
		{
			int idx = -1;
//			int result = -1;

//			string activeViewUniqueId = _DBMgr.ActiveGraphicalView.UniqueId;

			SheetData sheetCurrent = null;

			// clear the current list of sheets
			SheetList.Clear();

			

			foreach (ViewSheet vs in _DBMgr.AllViewSheets())
			{
				sheetCurrent = new SheetData(vs.SheetNumber, vs.Name, vs);

				SheetList.Add(sheetCurrent);

//				if (vs.UniqueId.Equals(activeViewUniqueId))
//				{
//					result = idx;
//				}
//
//				idx++;
			}
//
//			if (result == -1) result = 0;
//
//			lbxSheets.SelectedIndex = result;


			lbxSheets.Items.SortDescriptions.Add(
				new SortDescription("SheetNumber", 
					ListSortDirection.Ascending));

			if (_DBMgr.ActiveGraphicalView.ViewType == ViewType.DrawingSheet)
			{
				idx = FindSheetInList(
					((ViewSheet) _DBMgr.ActiveGraphicalView).SheetNumber);
			}

			lbxSheets.SelectedIndex = idx;

			return true;
		}

		private void InitTitleBlocks()
		{
			int idx = 0;

			TitleBlockList = new ObservableCollection<string>();

			TitleBlockList.Clear();
			TitleBlockList.Add(AppStrings.R_TBlkNone);

			if (_DBMgr.SheetCount > 0)
			{
				TitleBlockList.Add(AppStrings.R_TBlkFromSelSheet);
			}

			if (_DBMgr.TitleBlockCount > 0)
			{
				idx++;

				foreach (FamilySymbol tb in _DBMgr.GetAllTitleBlocks())
				{
					TitleBlockList.Add(ShDbMgr.FormatTitleBlockName(tb));
				}
			}

			cbxTitleBlock.SelectedIndex = idx;

			OnPropertyChange("TitleBlockList");
		}

		private void InitCopies()
		{
			cbxNumOfCopies.Items.Clear();

			for (int i = COPIES_START; i < COPIES_END; i++)
			{
				cbxNumOfCopies.Items.Add(i.ToString());
			}

			Copies = 1;
		}

		private void InitWinLocation()
		{
			Top  = USet.WinLocMainWin.Top;
			Left = USet.WinLocMainWin.Left;
			Width = USet.WinLocMainWin.Width;
		}

		#endregion

		#region + Properties

		// copies is not not a saved setting
		// value returned is

		public ObservableCollection<string> TitleBlockList { get; set; }

		public SheetData SelectedSheet
		{
			get => SheetList.SelectedSheet;
			set
			{
				if (value != null)
				{
					SheetList.SelectedSheet = value;

					if (UseTemplateSheet == false)
					{
						if (_initalized)
							TemplateSheet = SheetList.SelectedSheet;
					}
				}
			}
		}

		public string SelectedTitleBlock
		{
			get => USet.Basic.TitleBlockName;
			set
			{
				if (USet.Basic.TitleBlockName == null || 
					!USet.Basic.TitleBlockName.Equals(value))
				{
					USet.Basic.TitleBlockName = value;
				}
			}
		}


		// need to keep in mind that the viewsheet data is null
		public SheetData TemplateSheet
		{
			get => USet.Basic.TemplateSheet;

			set
			{
				

				if ((USet.Basic.TemplateSheet != null && !value.Equals(USet.Basic.TemplateSheet))
					|| USet.Basic.TemplateSheet == null )
				{
					USet.Basic.TemplateSheet = value;
					OnUiPropertyChange();
				}

				if (USet.Basic.TemplateSheet == null)
				{
					tblkTemplateSheet.Text = ((SheetData) lbxSheets.Items[lbxSheets.SelectedIndex]).ToString();
				}
				else
				{
					tblkTemplateSheet.Text = USet.Basic.TemplateSheet.ToString();
				}
			}
		}

		public bool UseTemplateSheet
		{
			get => USet.Basic.UseTemplateSheet;

			set
			{
				if (!value == USet.Basic.UseTemplateSheet)
				{
					USet.Basic.UseTemplateSheet = value;
					OnUiPropertyChange();
				}

				if (USet.Basic.UseTemplateSheet == false)
				{
					TemplateSheet = null;
				}
				else
				{
					if (lbxSheets.SelectedIndex > -1)
					{
						TemplateSheet = (SheetData) lbxSheets.Items[lbxSheets.SelectedIndex];
					}
				}
			}
		}


		#region + Settings

//		public int Copies { 
//			get => _copies;
//			set
//			{
//				if (value < COPIES_START || value > COPIES_END)
//				{
//					value = 1;
//				}
//
//				_copies = value;
//
//				cbxNumOfCopies.SelectedIndex = value - COPIES_START;
//
//			}
//		}
		
		public int Copies { 
			get => USet.Basic.Copies;
			set
			{
				if (value < COPIES_START || value > COPIES_END)
				{
					value = 1;
				}

				USet.Basic.Copies = value;

				cbxNumOfCopies.SelectedIndex = value - COPIES_START;

			}
		}

		public bool UseParameters
		{
			get => USet.Basic.UseParameters;
			set
			{
				if (!value == USet.Basic.UseParameters)
				{
					USet.Basic.UseParameters = value;
					OnUiPropertyChange();
				}
			}
		}

		public OperOpType OperationOpts
		{
			get
			{
				return USet.Basic.OperationOption;

			}
			set
			{
				USet.Basic.OperationOption = value; 
				OnPropertyChange();
			}
		}

		public NewShtOptions NewShtOpts
		{
			get => USet.Basic.NewSheetOption;
			set
			{
				USet.Basic.NewSheetOption = value;
				OnUiPropertyChange();
			}
		}

		public string SampleSequence
		{
			get { return USet.SheetFormatSample.Sequence.ToString(); }
			set
			{
				bool answer = int.TryParse(value, out int result);

				if (!answer || !ValidateSequence(result)) result = 1;

				if (result != USet.SheetFormatSample.Sequence)
				{
					USet.SheetFormatSample.Sequence = result;
					OnUiPropertyChange();
				}
			}
		}

		public int SampleSequenceInteger
		{
			get => USet.SheetFormatSample.Sequence;
			set
			{
				if (ValidateSequence(value))
				{
					USet.SheetFormatSample.Sequence = value;
				}
			}
		}

		private bool ValidateSequence(int seq)
		{
			return seq > 0 && seq < 10001;
		}

		public string SampleShtNumber
		{
			get => USet.SheetFormatSample.SampleSheetNumber;
			set
			{
				if (!USet.SheetFormatSample.SampleSheetNumber.Equals(value))
				{
					USet.SheetFormatSample.SampleSheetNumber = value;
					OnPropertyChange();
					UpdateShtNumExample();
				}
			}
		}

		public string SampleShtName
		{
			get => USet.SheetFormatSample.SampleSheetName;
			set
			{
				if (!USet.SheetFormatSample.SampleSheetName.Equals(value))
				{
					USet.SheetFormatSample.SampleSheetName = value;
					OnUiPropertyChange();
				}
			}
		}

		// saved / read directly from the settings class

		// ***** per settings *****

		public string PsNumberPrefix
		{
			get => USet.Basic.SheetFormatPs.NumberPrefix;
			set
			{
				if (!value.Equals(USet.Basic.SheetFormatPs.NumberPrefix))
				{
					USet.Basic.SheetFormatPs.NumberPrefix = value;
					OnUiPropertyChange();
					
				}
			}
		}

		public string PsSheetNamePrefix
		{
			get => USet.Basic.SheetFormatPs.SheetNamePrefix;
			set
			{
				if (!value.Equals(USet.Basic.SheetFormatPs.SheetNamePrefix))
				{
					USet.Basic.SheetFormatPs.SheetNamePrefix = value;
					OnUiPropertyChange();

				}
			}
		}
		
		public bool PsIncSheetName
		{
			get => USet.Basic.SheetFormatPs.IncSheetName;
			set
			{
				if (!value.Equals(USet.Basic.SheetFormatPs.IncSheetName))
				{
					USet.Basic.SheetFormatPs.IncSheetName = value;
					OnUiPropertyChange();
				}
			}
		}


		// this gets the collection
		public ShNamePartItems PsNumFmtCbxItems => 
			cbi.Tables[(int) SET_NUMSUFFIX_TBL];

		// this gets / returns the currently selected box item
		// only the CbxItemCode is saved in the settings file
		public ShNamePartItem PsNumFmtCbxSelected
		{
//			get => _numFmtCbxSelected;
			get => 
				cbi.FindByCode(USet.Basic.SheetFormatPs.
					NamePartSelItem[ShConst.SET_NUMSUFX]);

			set
			{
				if (value != null && !USet.Basic.SheetFormatPs
					.NamePartSelItem[ShConst.SET_NUMSUFX].Equals(value.Code))
				{
					USet.Basic.SheetFormatPs
						.NamePartSelItem[ShConst.SET_NUMSUFX] = value.Code;

					OnUiPropertyChange();
				} 
			}
		}

		// ***** from current *****

		// this gets the collection
		public ShNamePartItems FcNameDivCharsCbxItems => 
			cbi.Tables[(int) CUR_NAMEDIVCHARS_TBL];

		// this gets / returns the currently selected box item
		// only the CbxItemCode is saved in the settings file
		public ShNamePartItem FcNameDivCharsCbxSelected
		{
			get => cbi.FindByCode(USet.Basic.SheetFormatFc.
					NamePartSelItem[(int)CUR_NAMEDIVCHARS_TBL]);
			set
			{
				if (value != null && USet.Basic.SheetFormatFc
					.NamePartSelItem[(int) CUR_NAMEDIVCHARS_TBL] != value.Code)
				{
					USet.Basic.SheetFormatFc
						.NamePartSelItem[(int) CUR_NAMEDIVCHARS_TBL] = value.Code;

					OnPropertyChange();
				}
			}
		}

		public ShNamePartItems FcNameSufxCbxItems => 
			cbi.Tables[(int) CUR_NAMESUFFIX_TBL];

		public ShNamePartItem FcNameSufxCbxSelected
		{
//			get => _nameSufxCbxSelected;
			get => cbi.FindByCode(USet.Basic.SheetFormatFc.
					NamePartSelItem[(int)CUR_NAMESUFFIX_TBL]);
			set
			{
				if (value != null && !USet.Basic.SheetFormatFc
					.NamePartSelItem[(int) CUR_NAMESUFFIX_TBL].Equals(value.Code))
				{
					USet.Basic.SheetFormatFc
						.NamePartSelItem[(int) CUR_NAMESUFFIX_TBL] = value.Code;

					OnPropertyChange();
				}
			}
		}

		public ShNamePartItems FcNumSufxCbxItems => 
			cbi.Tables[(int) CUR_NUMSUFFIX_TBL];

		public ShNamePartItem FcNumSufxCbxSelected
		{
			//			get => _numSufxCbxSelected;
			get => cbi.FindByCode(USet.Basic.SheetFormatFc.
				NamePartSelItem[(int)CUR_NUMSUFFIX_TBL]);
			set
			{
				if (value != null && !USet.Basic.SheetFormatFc
					.NamePartSelItem[(int) CUR_NUMSUFFIX_TBL].Equals(value.Code))
				{
					USet.Basic.SheetFormatFc
						.NamePartSelItem[(int) CUR_NUMSUFFIX_TBL] = value.Code;

					OnUiPropertyChange();
				}
			}
		}

		public ShNamePartItems FcNumDivCharsCbxItems => 
			cbi.Tables[(int) CUR_NUMDIVCHARS_TBL];

		public ShNamePartItem FcNumDivCharsCbxSelected
		{
			get => cbi.FindByCode(USet.Basic.SheetFormatFc.NamePartSelItem[(int) CUR_NUMDIVCHARS_TBL]);
			set
			{
				if (value != null && !USet.Basic.SheetFormatFc
					.NamePartSelItem[(int) CUR_NUMDIVCHARS_TBL].Equals(value.Code))
				{
					USet.Basic.SheetFormatFc
						.NamePartSelItem[(int) CUR_NUMDIVCHARS_TBL] = value.Code;

					OnPropertyChange();
				}
			}
		}

		#endregion

		#endregion

		#region + ReadSettings

		private void ApplySettings()
		{
			// ************
			// read the radio button info
			SelectRadioButton((int) USet.Basic.OperationOption,
				rbOpDuplicateSheets, rbOpDuplicateSheetsAndViews,
				rbOpCreateSheets);

			SelectRadioButton((int) USet.Basic.NewSheetOption,
				rbPerSettingss, rbFromCurrent);

			UseParameters = USet.Basic.UseParameters;

			// ************
			// read the sample info
			SampleSequence = USet.SheetFormatSample.Sequence.ToString();
			SampleShtName = USet.SheetFormatSample.SampleSheetName;
			SampleShtNumber = USet.SheetFormatSample.SampleSheetNumber;

			// ************
			// read the from current settings
			ReadCbxSettings(ShNamePartItemsTables.CBX_CURR_START, ShNamePartItemsTables.CBX_CURR_END, USet.Basic.SheetFormatFc);

			// ************
			// read the from current settings
			ReadCbxSettings(ShNamePartItemsTables.CBX_SET_START, ShNamePartItemsTables.CBX_SET_END, USet.Basic.SheetFormatPs);

			PsNumberPrefix = USet.Basic.SheetFormatPs.NumberPrefix;
			PsSheetNamePrefix = USet.Basic.SheetFormatPs.SheetNamePrefix;
			PsIncSheetName = USet.Basic.SheetFormatPs.IncSheetName;

			TemplateSheet = USet.Basic.TemplateSheet;
			UseTemplateSheet = USet.Basic.UseTemplateSheet;
		}

		private void ReadCbxSettings(int start, int end, BaseInfo stgInfo)
		{
			// represents the index in the settings array (stgInfo)
			int i = 0;

			// read from current info
			// cbxIdx = combobox number
			for (int cbxIdx = start; cbxIdx <= end; cbxIdx++)
			{
				i = cbxIdx - start;

				ShNamePartItemCode cbxItemCode = stgInfo.NamePartSelItem[i];

				_customText[cbxIdx] =
					stgInfo.CustomText[i];

				if (cbxItemCode == CUSTOMSET)
				{
					_comboBoxes[cbxIdx].SelectedIndex =
						cbi.Tables[cbxIdx].UpdateCustom(_customText[cbxIdx]);
				}
				else
				{
					_comboBoxes[cbxIdx].SelectedIndex =
						cbi.Tables[cbxIdx].FindCode((int) cbxItemCode);
				}
			}
		}

		private void SelectRadioButton(int which, params RadioButton[] rbs)
		{
			foreach (RadioButton rb in rbs)
			{
				rb.IsChecked = false;
			}
			rbs[which].IsChecked = true;
		}

		#endregion

		#region + SaveSettings

		private void SaveSettings()
		{
			// ************
			// save "from current" settings
			SaveCbxSettings(ShNamePartItemsTables.CBX_CURR_START, 
				ShNamePartItemsTables.CBX_CURR_END, USet.Basic.SheetFormatFc);

			// *************
			// save "per settings" settings
			SaveCbxSettings(ShNamePartItemsTables.CBX_SET_START, 
				ShNamePartItemsTables.CBX_SET_END, USet.Basic.SheetFormatPs);

			// save the settings
			USettings.Save();

			Debug.Print(USet.Basic.ToString());
			Debug.Print(USet.OneClick.ToString());
		}

		private void SaveCbxSettings(int start, int end, BaseInfo stgInfo)
		{
			// represents the index in the settings array (stgInfo)
			int i = 0;

			// save from current info
			// cbxIdx = combobox number
			for (int cbxIdx = start; cbxIdx <= end; cbxIdx++)
			{
				i = cbxIdx - start;

				stgInfo.NamePartSelItem[i] =
					((ShNamePartItem) _comboBoxes[cbxIdx].SelectedItem).Code;

				stgInfo.CustomText[i] = _customText[cbxIdx];
			}
		}

		private int GetSelectedRadioButton(params RadioButton[] rbs)
		{
			for (int i = 0; i < rbs.Length; i++)
			{
				if ((bool) rbs[i].IsChecked)
				{
					return i;
				}
			}
			return 0;
		}
		

		#endregion

		#region + Utility

		private int FindSheetInList(string sheetNumber)
		{
			int item = 0;

			foreach (SheetData sd in lbxSheets.Items)
			{
				if (sd.SheetNumber.Equals(sheetNumber))
				{
					return item;
				}

				// next item number
				item++;
			}

			return -1;
		}

		private void ConfigCbxItems(ComboBox cbx, ShNamePartType ct)
		{
			int idx = (int)ct;

			_comboBoxes[idx] = cbx;
			cbx.Tag = idx;
			
			cbx.SelectedIndex = 0;	// pre-select the top item
		}

		public string FormatSheetNumber(string origShtNum)
		{
			string result = AppStrings.R_FormatSheetNumberUndef;

			switch (NewShtOpts)
			{
			case NewShtOptions.PerSettings:
				{
					result = FormatShtNumber(PsNumberPrefix, 
						PsNumFmtCbxSelected.Code, SampleSequenceInteger);
					break;
				}
			case NewShtOptions.FromCurrent:
				{
					result = FormatShtNumber(origShtNum,
						FcNumDivCharsCbxSelected.Code,
						_customText[(int) CUR_NUMDIVCHARS_TBL],
						FcNumSufxCbxSelected.Code, SampleSequenceInteger);
					break;
				}
			}

			return result;
		}

		public string FormatSheetName(string origShtName)
		{
			string result = AppStrings.R_FormatSheetNumberUndef;

			switch (NewShtOpts)
			{
			case NewShtOptions.PerSettings:
				{
					result = FormatShtName(PsSheetNamePrefix, 
						PsIncSheetName, PsNumFmtCbxSelected.Code,
						SampleSequenceInteger);
					break;
				}
			case NewShtOptions.FromCurrent:
				{
					result = FormatShtName(origShtName,
						FcNameDivCharsCbxSelected.Code,
						_customText[(int) CUR_NAMEDIVCHARS_TBL],
						FcNameSufxCbxSelected.Code,
						_customText[(int) CUR_NAMESUFFIX_TBL], SampleSequenceInteger);
					break;
				}
			}

			return result;
		}

		private void UpdateShtNumExample()
		{
			if (_initalized)
			{
				tbExampleShtNum.Text = FormatSheetNumber(tbSampleShtNum.Text);
			}
		}

		private void UpdateShtNameExample()
		{
			if (_initalized)
			{
				tbExampleShtName.Text = FormatSheetName(tbSampleShtName.Text);
			}
		}

		private string GetCustText(int cbxIndex)
		{
			int idx = cbxIndex > 1 ? cbxIndex -= 2 : cbxIndex;

			WpfCustomText w = new WpfCustomText(_customText[cbxIndex],
				shData.CustomTitle[idx],
				shData.CustomLabel[idx],
				shData.CustomInvalidTitle[idx],
				shData.CustomInvalidChars[idx]
				);

			if (w.ShowDialog() ?? false)
			{
				return w.tbCustomText.Text;
			}

			return _customText[cbxIndex];
		}

		private void GetCustomText(int cbxIndex)
		{
			// request the custom text from the user
			_customText[cbxIndex] = GetCustText(cbxIndex);

			// find the index of the custom divider text and
			// update the combobox items with the custom text
			// idx == -1 if invalid custom text string
			// idx == -2 if custom string was blank
			int idx = cbi.Tables[cbxIndex].UpdateCustom(_customText[cbxIndex]);

			_comboBoxes[cbxIndex].Items.Refresh();

			// save the value in case must revert
			int savedIdx = _comboBoxes[cbxIndex].SelectedIndex;

			// user provided custom text
			// update the combobox and the example
			if (idx > -1)
			{
				// custom text is valid
				_comboBoxes[cbxIndex].SelectedIndex = idx;

				USet.Basic.SheetFormatFc.CustomText[cbxIndex] =
					_customText[cbxIndex];
			}
			else
			{
				if (idx == -1)
				{
					// custom text is not valid - reset index
					// to the previous value
					_comboBoxes[cbxIndex].SelectedIndex = savedIdx;
				}
				else
				{
					// idx == -2
					// custom string is blank
					cbi.Tables[cbxIndex].SetToDefault();

					_comboBoxes[cbxIndex].SelectedIndex 
						= cbi.Tables[cbxIndex].DefaultItemIdx;
				}
			}
			UpdateShtNumExample();
			UpdateShtNameExample();
		}

		#endregion

		#region + Control routines

		// *******************  control routines ************************ //


		private void winSelViewSheet_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.F1)
			{
				System.Diagnostics.Process.Start(@"http://www.cyberstudioapps.com/index.html");
			}
		}

		private void btnAbout_Click(object sender, RoutedEventArgs e)
		{
			new SharedResources.About().ShowDialog();

			e.Handled = true;
		}

		private void btnProceed_Click(object sender, RoutedEventArgs e)
		{
			// save the current settings first
			SaveSettings();

			// must verify if it ok to proceed

			// there are 3 modes of operation

			// OperationOpts determines which operation
			// NewShtOpts determined the sheet naming style

			// must verify:
			// if no sheet selected then
			// 	operation must be "create empty sheets" and
			//	the title block setting cannot be "from selected sheet"

			if (lbxSheets.SelectedItems.Count == 0)
			{
				// no sheets selected
				if (OperationOpts == OperOpType.CreateEmptySheets &&
					SelectedTitleBlock.Equals(AppStrings.R_TBlkFromSelSheet))
				{
					ShUtil.ShowSelectSheetErrMsg();
					return;
				}
				else
				{
					ShUtil.ShowSelectSheetErrMsg();
					return;
				}
			}

			DialogResult = true;

		}

		// save the one click settings
		private void btnOneClickSave_Click(object sender, RoutedEventArgs e)
		{
			Balloon b = new Balloon(this, btnOneClickSave, AppStrings.R_OneClickSaved);

			USet.OneClick = USet.Basic.Clone();
			USet.OneClick.Name = NewSheetFormat.OneClickName;

			SaveSettings();

			b.Orientation = BalloonOrientation.TOP_RIGHT;
			b.Show();
		}

		// restore the one click settings
		// save a copy of the current settings first
		private void btnOneClickRestore_Click(object sender, RoutedEventArgs e)
		{
			Balloon b = new Balloon(this, btnOneClickRestore, AppStrings.R_OneClickRestored);

			if (!btnSavedRestore.IsEnabled)
			{
				// save a copy to be restored later
				SavedSettings = USet.Basic.Clone();
				btnSavedRestore.IsEnabled = true;
			}

			USet.Basic = USet.OneClick.Clone();
			USet.Basic.Name = NewSheetFormat.BasicName;

			ApplySettings();

			b.Orientation = BalloonOrientation.TOP_RIGHT;
			b.Show();
		}

		// restore the saved settings
		private void btnSavedRestore_Click(object sender, RoutedEventArgs e)
		{
			USet.Basic = SavedSettings.Clone();

			btnSavedRestore.IsEnabled = false;

			ApplySettings();
			SaveSettings();
		}

		public void btnHelp_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;

			string resource = (string) ((Button) sender).Tag;

			ShUtil.ShowHelpMessage(resource);

		}


		private void btnOneClickHelp_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;


		}

		private void cbxCurNumDvChars_DropDownClosed(object sender, EventArgs e)
		{
			if (USet.Basic.SheetFormatFc
				.NamePartSelItem[(int) CUR_NUMDIVCHARS_TBL] == CUSTOMSELECT)
			{
				GetCustomText((int) CUR_NUMDIVCHARS_TBL);
			}

			UpdateShtNumExample();
		}

		private void cbxCurNamDvChars_DropDownClosed(object sender, EventArgs e)
		{
			if (USet.Basic.SheetFormatFc
				.NamePartSelItem[(int) CUR_NAMEDIVCHARS_TBL] == CUSTOMSELECT)
			{
				GetCustomText((int) CUR_NAMEDIVCHARS_TBL);
			}

			UpdateShtNameExample();
		}
		
		private void cbxCurNamSx_DropDownClosed(object sender, EventArgs e)
		{
			if (USet.Basic.SheetFormatFc
				.NamePartSelItem[(int)CUR_NAMESUFFIX_TBL] == CUSTOMSELECT)
			{
				GetCustomText((int)CUR_NAMESUFFIX_TBL);
			}

			UpdateShtNameExample();
		}

		private void winSelViewSheet_Loaded(object sender, RoutedEventArgs e)
		{
			// setup the copies combobox
			InitCopies();

			InitCbx();

			// note - although the data for the comboboxes has
			// been configured, the comboboxes are not setup
			// until loaded

			//			rbPerSettingss.DataContext = this;
			//			rbFromCurrent.DataContext = this;

			// setup and populate sheet list
			InitShtList();

			// read the saved settings
			USettings.Read();

			InitTitleBlocks();

			// setup the controls before read / apply settings
			ApplySettings();

			// flat that ui is configured
			_initalized = true;

			// update the display of the examples and samples
			UpdateShtNumExample();
			UpdateShtNameExample();

		}

		private void winSelViewSheet_Unloaded(object sender, RoutedEventArgs e)
		{
			USet.WinLocMainWin.Top = (int)Top;
			USet.WinLocMainWin.Left = (int)Left;
			USet.WinLocMainWin.Width = Width;
			USettings.Save();
		}

		private void winSelViewSheet_Closing(object sender, CancelEventArgs e)
		{
			if (!(DialogResult ?? false)) return;

			// if here - ready to proceed

			// hand-off to db mgr to complete the processing

			USet.Basic.Copies = Copies;
			USet.Basic.TitleBlockName = SelectedTitleBlock;
			USet.Basic.SelectedSheet = SelectedSheet;


#if DEBUG
			logMsgLn2("at closing", SelectedSheet.ToString());
#endif

			// process the selected operation - return the result
			DialogResult = _DBMgr.Process2(USet.Basic);
		}

		#endregion

		private void App_UnhandledExecption(object sender,
			System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			throw e.Exception;
		}


		public event PropertyChangedEventHandler PropertyChanged;

		private void OnUiPropertyChange([CallerMemberName] string memberName = "")
		{
			UpdateShtNumExample();
			UpdateShtNameExample();
			OnPropertyChange(memberName);
		}

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	}





	public class ComparisonCovnerter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{

			}
			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.Equals(true) ? parameter : Binding.DoNothing;
		}
	}

	public class EnumToBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, 
			object parameter, CultureInfo culture) => value != null && value.Equals(parameter);

		public object ConvertBack(object value, Type targetType, 
			object parameter, CultureInfo culture) => value.Equals(true) ? parameter : Binding.DoNothing;
	}
}
