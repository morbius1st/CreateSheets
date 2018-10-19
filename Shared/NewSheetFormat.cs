using System.Runtime.Serialization;
using Autodesk.Revit.DB;
using SharedCode.Resources;

namespace SharedCode
{
	[DataContract(Namespace = LocalResMgr.XMLNS)]
	[KnownType(typeof(string[]))]
	public class NewSheetFormat
	{
		private ShNamePartItemsTables npit = ShNamePartItemsTables.Instance;

		public int       Copies;
		public string    TitleBlockName;
		public string    newSheetNumber;
		public string    newSheetName;
		private ViewSheet SelectedViewSheet;
		private string    SelectedShtName;
		private string    SelecedShtNumber;
		public int       seq;		
		
		[DataMember]
		public bool Defined { get; set; } = false;


		[DataMember (Order = 1)]
		public OperOpType OperationOption { get; set; } = OperOpType.DupSheetAndViews;

		[DataMember(Order = 2)]
		public NewShtOptions NewSheetOption { get; set; } = NewShtOptions.FromCurrent;

		[DataMember(Order = 3)]
		public bool UseParameters { get; set; } = true;

		[DataMember]
		public ShtFmtFrmCurrent SheetFormatFc { get; set; } = new ShtFmtFrmCurrent();

		[DataMember]
		public ShtFmtPerSetting SheetFormatPs { get; set; } = new ShtFmtPerSetting();

		public NewSheetFormat(bool defined)
		{
			Defined = defined;
		}

		public SheetData SelectedSheet
		{
			get { return new SheetData(SelecedShtNumber, SelectedShtName, SelectedViewSheet); }
			set
			{
				SelectedViewSheet = (ViewSheet) value.SheetView;
				SelecedShtNumber  = value.SheetNumber;
				SelectedShtName   = value.SheetName;
			}
		}

		#region + Name Part Info

		// general

		// get the selected sheet name
		public string FcSelShtName
		{
			get => SelectedShtName;
		}
		
		// get the selected sheet name
		public string FcSelShtNum
		{
			get => SelecedShtNumber;
		}
		
		// get the selected sheet name
		public ViewSheet FcSelViewSht
		{
			get => SelectedViewSheet;
		}


		// per setting

		// get the per setg sheet name prefix
		public string PsShtNamePrefix
		{
			get => SheetFormatPs.SheetNamePrefix;
		}

		// get the per setg increment sheet number
		public bool PsIncShtName
		{
			get => SheetFormatPs.IncSheetName;
		}

		// get the per setting number format code
		public ShNamePartItemCode PsNumFmtCode
		{
			get => SheetFormatPs.NamePartSelItem[ShConst.SET_NUMSUFX];
		}

		// get the per setg number format namepartitem
		public ShNamePartItem PsNumFmtNamePartItem
		{
			get => npit.FindByCode(PsNumFmtCode);
		}

		// get the per setg number format title
		public string PsNumFmtTitle
		{
			get => PsNumFmtNamePartItem.Title;
		}


		// from current

		// get the from curr number div char code
		public ShNamePartItemCode FcNumDivChar
		{
			get => SheetFormatFc.NamePartSelItem[(int) ShNamePartType.CUR_NUMDIVCHARS_TBL];
		}

		// get the from curr number suffix code
		public ShNamePartItemCode FcNumSuffix
		{
			get => SheetFormatFc.NamePartSelItem[(int)ShNamePartType.CUR_NUMSUFFIX_TBL];
		}

		// get the from curr name div char code
		public ShNamePartItemCode FcNameDivChar
		{
			get => SheetFormatFc.NamePartSelItem[(int)ShNamePartType.CUR_NAMEDIVCHARS_TBL];
		}

		// get the from curr name suffix code
		public ShNamePartItemCode FcNameSuffix
		{
			get => SheetFormatFc.NamePartSelItem[(int)ShNamePartType.CUR_NAMESUFFIX_TBL];
		}

		// get the custom number div char
		public string FcNumDivCharCustom
		{
			get => SheetFormatFc.CustomText[(int)ShNamePartType.CUR_NUMDIVCHARS_TBL];
		}

		// get the from curr name div char code
		public string FcNameDivCharCustom
		{
			get => SheetFormatFc.CustomText[(int)ShNamePartType.CUR_NAMEDIVCHARS_TBL];
		}

		// get the from curr name suffix code
		public string FcNameSuffixCustom
		{
			get => SheetFormatFc.CustomText[(int)ShNamePartType.CUR_NAMESUFFIX_TBL];
		}



		#endregion
	}

	public interface CbxInfo
	{
		ShNamePartItemCode[] NamePartSelItem { get; set; }
		string[]             CustomText      { get; set; }
	}

	[DataContract(Namespace = LocalResMgr.XMLNS)]
	public class ShtFmtSample
	{
		[DataMember]
		public int Sequence { get; set; } = 1;

		[DataMember]
		public string SampleSheetNumber { get; set; } = AppStrings.R_ShtOpCurSampleShtNum;

		[DataMember]
		public string SampleSheetName { get; set; } = AppStrings.R_ShtOpCurSampleShtName;
	}

	[DataContract(Namespace = LocalResMgr.XMLNS)]
	public class ShtFmtPerSetting : CbxInfo
	{
		[DataMember]
		public string NumberPrefix { get; set; } = ".X-";

		[DataMember]
		public string SheetNamePrefix { get; set; } = AppStrings.R_SheetName;

		[DataMember]
		public bool IncSheetName { get; set; } = false;

		[DataMember]
		public ShNamePartItemCode[] NamePartSelItem { get; set; } = { ShNamePartItemCode.S_SX_NUMNUM1 };

		[DataMember]
		public string[] CustomText { get; set; } = { "" };
	}

	[DataContract(Namespace = LocalResMgr.XMLNS)]
	public class ShtFmtFrmCurrent : CbxInfo
	{
		[DataMember]
		public ShNamePartItemCode[] NamePartSelItem { get; set; } = { ShNamePartItemCode.C_DV_PERIOD, ShNamePartItemCode.C_SX_NUMNUM2, ShNamePartItemCode.C_DV_PERIOD, ShNamePartItemCode.C_SX_NAMCOPY1 };

		[DataMember]
		public string[] CustomText { get; set; } = { "", "", "", "" };
	}
}