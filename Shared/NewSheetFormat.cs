using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using Autodesk.Revit.DB;
using SharedCode.Resources;

namespace SharedCode
{
	[DataContract(Namespace = LocalResMgr.XMLNS)]
	[KnownType(typeof(string[]))]
	public class NewSheetFormat
	{
		public NewSheetFormat(bool defined)
		{
			Defined = defined;
		}

		private ShNamePartItemsTables npit = ShNamePartItemsTables.Instance;

		public int       Copies;
		
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
		public string TitleBlockName { get; set; } = null;

		[DataMember(Order = 4)]
		public bool UseParameters { get; set; } = true;

		[DataMember(Order = 5)]
		public bool UseTemplateSheet { get; set; } = false;

		[DataMember(Order = 6)]
		public string TemplateSheetNumber { get; set; } = null;

		[DataMember(Order = 7)]
		public string TemplateSheetName { get; set; } = null;

		[DataMember(Order = 10)]
		public ShtFmtFrmCurrent SheetFormatFc { get; set; } = new ShtFmtFrmCurrent();

		[DataMember(Order = 11)]
		public ShtFmtPerSetting SheetFormatPs { get; set; } = new ShtFmtPerSetting();

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

		#region + Properties

		// general
		
		// save the  Template Sheet
		public SheetData TemplateSheet
		{
			get
			{
				if (TemplateSheetNumber == null) return null;

				return new SheetData(TemplateSheetNumber,
					TemplateSheetName, null);
			}

			set
			{
				if (value == null)
				{
					TemplateSheetNumber = null;
					TemplateSheetName = null;
				} 
				else
				{
					TemplateSheetNumber = value.SheetNumber;
					TemplateSheetName   = value.SheetName;
				}
			}

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
		public string FcSelShtName
		{
			get => SelectedShtName;
		}

		public string FcSelShtNum
		{
			get => SelecedShtNumber;
		}

		public ViewSheet FcSelViewSht
		{
			get => SelectedViewSheet;
		}

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

	public interface BaseInfo
	{
		ShNamePartItemCode[] NamePartSelItem { get; set; }
		string[]             CustomText      { get; set; }
	}

	[DataContract(Namespace = LocalResMgr.XMLNS)]
	public class ShtFmtSample
	{
		[DataMember (Order = 1)]
		public int Sequence { get; set; } = 1;

		[DataMember (Order = 2)]
		public string SampleSheetNumber { get; set; } = AppStrings.R_ShtOpCurSampleShtNum;

		[DataMember (Order = 3)]
		public string SampleSheetName { get; set; } = AppStrings.R_ShtOpCurSampleShtName;
	}

	[DataContract(Namespace = LocalResMgr.XMLNS)]
	public class ShtFmtPerSetting : BaseInfo
	{
		[DataMember(Order = 1)]
		public string NumberPrefix { get; set; } = ".X-";

		[DataMember(Order = 2)]
		public string SheetNamePrefix { get; set; } = AppStrings.R_SheetName;

		[DataMember(Order = 3)]
		public bool IncSheetName { get; set; } = false;

		[DataMember(Order = 4)]
		public ShNamePartItemCode[] NamePartSelItem { get; set; } = { ShNamePartItemCode.S_SX_NUMNUM1 };

		[DataMember(Order = 5)]
		public string[] CustomText { get; set; } = { "" };
	}

	[DataContract(Namespace = LocalResMgr.XMLNS)]
	public class ShtFmtFrmCurrent : BaseInfo
	{
		[DataMember(Order = 1)]
		public ShNamePartItemCode[] NamePartSelItem { get; set; } = { ShNamePartItemCode.C_DV_PERIOD, ShNamePartItemCode.C_SX_NUMNUM2, ShNamePartItemCode.C_DV_PERIOD, ShNamePartItemCode.C_SX_NAMCOPY1 };

		[DataMember(Order = 2)]
		public string[] CustomText { get; set; } = { "", "", "", "" };
	}


	public static class SettingExtensions
	{
		public static NewSheetFormat Clone(this NewSheetFormat orig)
		{
			NewSheetFormat copy = new NewSheetFormat(orig.Defined);

			copy.OperationOption = orig.OperationOption;
			copy.NewSheetOption = orig.NewSheetOption;
			copy.TitleBlockName = orig.TitleBlockName;
			copy.UseParameters = orig.UseParameters;

			copy.UseTemplateSheet = orig.UseTemplateSheet;

			copy.TemplateSheet = orig.TemplateSheet?.Clone();

			copy.SelectedSheet = orig.SelectedSheet.Clone();

			copy.SheetFormatFc = orig.SheetFormatFc.Clone();
			copy.SheetFormatPs = orig.SheetFormatPs.Clone();

			return copy;
		}

		public static ShtFmtPerSetting Clone(this ShtFmtPerSetting orig)
		{
			ShtFmtPerSetting copy = new ShtFmtPerSetting();

			copy.IncSheetName = orig.IncSheetName;
			copy.NumberPrefix = orig.NumberPrefix;
			copy.SheetNamePrefix = orig.SheetNamePrefix;

			for (var i = 0; i < orig.NamePartSelItem.Length; i++)
			{
				copy.NamePartSelItem[i] = orig.NamePartSelItem[i];
			}

			for (var i = 0; i < orig.CustomText.Length; i++)
			{
				copy.CustomText[i] = orig.CustomText[i];
			}

			return copy;
		}

		public static ShtFmtFrmCurrent Clone(this ShtFmtFrmCurrent orig)
		{
			ShtFmtFrmCurrent copy = new ShtFmtFrmCurrent();

			for (var i = 0; i < orig.NamePartSelItem.Length; i++)
			{
				copy.NamePartSelItem[i] = orig.NamePartSelItem[i];
			}

			for (var i = 0; i < orig.CustomText.Length; i++)
			{
				copy.CustomText[i] = orig.CustomText[i];
			}

			return copy;
		}
	}




}