using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.Revit.DB;
using SharedCode.Resources;
using static SharedCode.ShConst;

using static UtilityLibrary.MessageUtilities;

namespace SharedCode
{
	[DataContract(Namespace = LocalResMgr.XMLNS)]
	[KnownType(typeof(string[]))]
	public class NewSheetFormat
	{
		public const string BasicName = "Basic";
		public const string OneClickName = "OneClick";

		public NewSheetFormat(bool defined, string name)
		{
			Defined = defined;
			Name = name;
		}

		private static ShNamePartItemsTables namePartTables = ShNamePartItemsTables.Instance;
		
		public string     newSheetNumber;
		public string     newSheetName;
		private ViewSheet SelectedViewSheet;
		private string    SelectedShtName;
		private string    SelecedShtNumber;
		public int        seq;
		private int      copies;

		[DataMember (Order = 0)]
		public bool Defined { get; set; } = false;
		
		[DataMember (Order = 5)]
		public string Name { get; set; }

		[DataMember(Order = 10)]
		public int Copies
		{
			get => copies;
			set
			{
				if (value < COPIES_START || value > COPIES_END)
				{
					value = 1;
				}
				copies = value;
			}
		}

		

		[DataMember (Order = 15)]
		public OperOpType OperationOption { get; set; } = OperOpType.DupSheetAndViews;

		[DataMember(Order = 20)]
		public NewShtOptions NewSheetOption { get; set; } = NewShtOptions.FromCurrent;

		[DataMember(Order = 30)]
		public string TitleBlockName { get; set; } = null;

		[DataMember(Order = 40)]
		public bool UseParameters { get; set; } = true;

		[DataMember(Order = 50)]
		public bool UseTemplateSheet { get; set; } = false;

		[DataMember(Order = 60)]
		public string TemplateSheetNumber { get; set; } = null;

		[DataMember(Order = 70)]
		public string TemplateSheetName { get; set; } = null;

		public ViewSheet TemplateSheetView { get; set; } = null;

		[DataMember(Order = 100)]
		public ShtFmtFrmCurrent SheetFormatFc { get; set; } = new ShtFmtFrmCurrent();

		[DataMember(Order = 110)]
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

		// get the per setg number format name partitem
		public ShNamePartItem PsNumFmtNamePartItem
		{
			get => namePartTables.FindByCode(PsNumFmtCode);
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

		// get the new sheet name and file name parts
		public ShNamePartItemCode FcNumDivChar
		{
			// number division character
			get => SheetFormatFc.NamePartSelItem[(int) ShNamePartType.CUR_NUMDIVCHARS_TBL];
		}

		public ShNamePartItemCode FcNumSuffix
		{
			// number suffix code
			get => SheetFormatFc.NamePartSelItem[(int)ShNamePartType.CUR_NUMSUFFIX_TBL];
		}
		
		public ShNamePartItemCode FcNameDivChar
		{
			// name div char code
			get => SheetFormatFc.NamePartSelItem[(int)ShNamePartType.CUR_NAMEDIVCHARS_TBL];
		}

		
		public ShNamePartItemCode FcNameSuffix
		{
			// name suffix code
			get => SheetFormatFc.NamePartSelItem[(int)ShNamePartType.CUR_NAMESUFFIX_TBL];
		}

		
		public string FcNumDivCharCustom
		{
			// custom number div char
			get => SheetFormatFc.CustomText[(int)ShNamePartType.CUR_NUMDIVCHARS_TBL];
		}

		
		public string FcNameDivCharCustom
		{
			// name div char code
			get => SheetFormatFc.CustomText[(int)ShNamePartType.CUR_NAMEDIVCHARS_TBL];
		}

		
		public string FcNameSuffixCustom
		{
			// name suffix code
			get => SheetFormatFc.CustomText[(int)ShNamePartType.CUR_NAMESUFFIX_TBL];
		}

		#endregion

		#region + Overrides

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.Append("\n");
			sb.AppendLine(logMsgDbS("New Sheet Format", "-- Persistent Parameters Only --"));
			sb.AppendLine(logMsgDbS("For Structure", Name));
			sb.Append(logMsgDbS("Is Defined?"));

			if (Defined)
			{
				sb.AppendLine("Yes");
			}
			else
			{
				sb.AppendLine("No");
				return sb.ToString();
			}

			sb.AppendLine(logMsgDbS("Operation", OperationOption.ToString()));
			sb.AppendLine(logMsgDbS("New Sheet Option", NewSheetOption.ToString()));
			sb.AppendLine(logMsgDbS("Title Block Name", TitleBlockName ?? "is null"));
			sb.AppendLine(logMsgDbS("Re-use Parameters", UseParameters.ToString()));


			sb.Append(logMsgDbS("Basis for New Sheets"));

			if (UseTemplateSheet)
			{
				sb.AppendLine("Using Template| ").AppendLine(UseTemplateSheet.ToString());
				sb.AppendLine(logMsgDbS("Template Sheet Number", TemplateSheetNumber ?? "is Undefined"));
				sb.AppendLine(logMsgDbS("Template Sheet Name", TemplateSheetName ?? "is Undefined"));
			}
			else
			{
				sb.AppendLine("Using Current / Selected Sheet");
			}


			sb.Append(logMsgDbS("New Sheet Numbers / Names Basis"));

			if (NewSheetOption == NewShtOptions.FromCurrent)
			{
				// from current
				sb.AppendLine("From Current");

				sb.AppendLine(logMsgDbS("Sheet Number","-- New Name Parts --"));
				sb.AppendLine(logMsgDbS("Sheet Number Divisor", namePartTables.FindTitleByCode(FcNumDivChar)));
				sb.AppendLine(logMsgDbS("Sheet Number Suffix", namePartTables.FindTitleByCode(FcNumSuffix)));
				sb.AppendLine(logMsgDbS("Sheet Number Custom Divisor", 
					string.IsNullOrWhiteSpace(FcNumDivCharCustom) ? "is Undefined" : FcNumDivCharCustom));

				sb.AppendLine(logMsgDbS("Sheet Name", "-- New Name Parts --"));
				sb.AppendLine(logMsgDbS("Sheet Name Divisor", namePartTables.FindTitleByCode(FcNameDivChar)));
				sb.AppendLine(logMsgDbS("Sheet Name Suffix", namePartTables.FindTitleByCode(FcNameSuffix)));
				sb.AppendLine(logMsgDbS("Sheet Name Custom Divisor", 
					string.IsNullOrWhiteSpace(FcNameDivCharCustom) ? "is Undefined" : FcNameDivCharCustom));
				sb.AppendLine(logMsgDbS("Sheet Name Custom Suffix", 
					string.IsNullOrWhiteSpace(FcNameSuffixCustom) ? "is Undefined" : FcNameSuffixCustom));
			}
			else
			{
				// per settings	
				sb.AppendLine("Per Settings");

				sb.AppendLine(logMsgDbS("Sheet Number", "-- New Name Parts --"));
				sb.AppendLine(logMsgDbS("Sheet Number Format Name", PsNumFmtTitle));
				sb.AppendLine(logMsgDbS("Sheet Number Format Code", namePartTables.FindTitleByCode(PsNumFmtCode)));

				sb.AppendLine(logMsgDbS("Sheet Name", "-- New Name Parts --"));
				sb.AppendLine(logMsgDbS("Sheet Name Prefix", 
					string.IsNullOrWhiteSpace(PsShtNamePrefix) ? "Is Undefined" : PsShtNamePrefix));
				sb.AppendLine(logMsgDbS("Sheet Name Increment?", PsIncShtName.ToString()));
			}

			return sb.ToString();
		}

		#endregion
	}

	public interface BaseInfo
	{
		ShNamePartItemCode[] NamePartSelItem { get; set; }
		string[]             CustomText      { get; set; }
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
		public ShNamePartItemCode[] NamePartSelItem { get; set; } = { ShNamePartItemCode.C_DV_NUMPERIOD, ShNamePartItemCode.C_SX_NUMNUM2, ShNamePartItemCode.C_DV_NUMPERIOD, ShNamePartItemCode.C_SX_NAMCOPY1 };

		[DataMember(Order = 2)]
		public string[] CustomText { get; set; } = { "", "", "", "" };
	}


	public static class SettingExtensions
	{
		public static NewSheetFormat Clone(this NewSheetFormat orig)
		{
			NewSheetFormat copy = new NewSheetFormat(orig.Defined, orig.Name);

			copy.OperationOption = orig.OperationOption;
			copy.NewSheetOption = orig.NewSheetOption;
			copy.Copies = orig.Copies;
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