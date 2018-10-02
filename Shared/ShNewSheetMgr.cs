using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Autodesk.Revit.DB;
using SharedCode.Resources;
using static SharedCode.CbxItemCode;




namespace SharedCode
{
	public static class ShNewSheetMgr 
	{

		// format a sheet number

		// per settings
		public static string FormatShtNumber(string prefix, string format, int seq)
		{
			return prefix + seq.ToString(format);
		}

		// for from current
		public static string FormatShtNumber( string origShtNum,
			CbxItemCode divCode, string customDivider, CbxItemCode suffixCode,
			int seq)
		{
			string divider = customDivider;
			string suffix  = GetNumSuffix(suffixCode, seq);

			if ((int) divCode > (int) CUSTOMSELECT) divider = GetDivider(divCode);

			return origShtNum + divider + suffix;

		}

		// format a sheet name

		// per settings
		public static string FormatShtName(string name, bool inc, int seq)
		{
			string result = name;

			if (inc)
			{
				result += " " + seq;
			}

			return result;
		}

		// for from current
		public static string FormatShtName(  string origShtName,
			CbxItemCode divCode,
			string customDivider, CbxItemCode suffixCode, string customSuffix,
			int seq)
		{
			if (suffixCode == C_SX_NAMNONE) return origShtName;

			string divider = customDivider;
			string suffix  = customSuffix;

			if ((int) divCode    > (int) CUSTOMSELECT) divider = GetDivider(divCode);
			if ((int) suffixCode > (int) CUSTOMSELECT) suffix  = GetNameSuffix(suffixCode, seq);

			return origShtName + divider + suffix;
		}



		#region + Utilities

		private static string GetNameSuffix(CbxItemCode suffixCode, int suffixSequence)
		{

			string result = AppStrings.R_ErrError;

			switch (suffixCode)
			{
			case C_SX_NAMNONE:
				{
					result = "";
					break;
				}
			case C_SX_NAMCOPY1:
				{
					result = $"{AppStrings.R_Copy} {suffixSequence:D}";
					break;
				}
			case C_SX_NAMCOPYA:
				{
					result = AppStrings.R_Copy + " " + GetLetterCodeSequenced(suffixSequence, 'A');
					break;
				}
			case C_SX_NAMNUM1:
				{
					result = $"{suffixSequence:D}";
					break;
				}
			case C_SX_NAMALPLC:
				{
					result = GetLetterCodeSequenced(suffixSequence, 'a');
					break;
				}
			case C_SX_NAMALPUC:
				{
					result = GetLetterCodeSequenced(suffixSequence, 'A');
					break;
				}
			}

			return result;
		}

		private static string GetNumSuffix(CbxItemCode suffixCode, int suffixSequence)
		{
			string result = AppStrings.R_ErrError;
			int    i      = 1;

			switch (suffixCode)
			{

			case C_SX_NUMALPLC:
				{
					result = GetLetterCodeSequenced(suffixSequence, 'a');
					break;
				}
			case C_SX_NUMALPUC:
				{
					result = GetLetterCodeSequenced(suffixSequence, 'A');
					break;
				}
			default:
				{
					switch (suffixCode)
					{
					// default case - pre-assigned
					//case C_SX_NUMNUM1:
					case C_SX_NUMNUM2:
						{
							i = 2;
							break;
						}
					case C_SX_NUMNUM3:
						{
							i = 3;
							break;
						}
					case C_SX_NUMNUM4:
						{
							i = 4;
							break;
						}
					case C_SX_NUMNUM5:
						{
							i = 5;
							break;
						}
					}

					string temp = $"{"{0:D"}{i:D}{"}"}";
					result = String.Format(temp, suffixSequence);

					break;
				}
			}

			return result;
		}

		public static string GetLetterCodeSequenced(int sequence, char start)
		{
			string result = "";
			int    num    = sequence - 1; // 1 = A or a

			while (num >= 0)
			{
				char x = (char)(num % 26 + start);
				result = x          + result;
				num    = (num / 26) - 1;
			}

			return result;
		}

		public static string GetLetteerCodeSerial(int sequence, char start)
		{
			string result = "";
			int    qty    = sequence / 26 + 1;
			int    code   = sequence % 26;

			for (int i = 0; i < qty; i++)
			{
				result += ((char)(code + start)).ToString();
			}

			return result;
		}

		private static string GetDivider(CbxItemCode divCode)
		{
			string result = "-" + AppStrings.R_ErrError + "-";

			switch (divCode)
			{
			case C_DV_NONE: // none
				{
					result = "";
					break;
				}
			case C_DV_SPACE: // space
				{
					result = " ";
					break;
				}
			case C_DV_PERIOD: // space
				{
					result = ".";
					break;
				}
			case C_DV_DASH: // space
				{
					result = "-";
					break;
				}
			}

			return result;
		}

		#endregion
	}

	//	[DataContract(Namespace = "sample/name/space", Name = "NewSheetFormatSettings")]
	[DataContract (Namespace = "")]
	public class NewSheetFormat
	{
		[DataMember]
		public bool Defined { get; set; } = false;

		public int Copies;
		public string TitleBlockName;
		public SheetData SelectedSheet;
		public string newSheetNumber;
		public string newSheetName;
//		public ViewSheet vs;

		[DataMember (Order = 1)]
		public OperOpType OperationOption { get; set; } = OperOpType.DupSheetAndViews;

		[DataMember(Order = 2)]
		public RbNewShtOptions NewSheetOption { get; set; } = RbNewShtOptions.FromCurrent;

		[DataMember(Order = 3)]
		public bool UseParameters { get; set; } = true;

		[DataMember]
		public ShtFmtFrmCurrent SheetFormatFrmCurrent { get; set; } = new ShtFmtFrmCurrent();

		[DataMember]
		public ShtFmtPerSetting SheetFormatPerSetting { get; set; } = new ShtFmtPerSetting();

		public NewSheetFormat(bool defined)
		{
			Defined = defined;
		}
	}

	[DataContract(Namespace = "")]
	public class ShtFmtSample
	{
		[DataMember]
		public int Sequence { get; set; } = 1;

		[DataMember]
		public string SampleSheetNumber { get; set; } = AppStrings.R_ShtOpCurSampleShtNum;

		[DataMember]
		public string SampleSheetName { get; set; }  = AppStrings.R_ShtOpCurSampleShtName;
	}

//	[DataContract(Namespace = "")]
	public class ShtFmtFrmCurrent : CbxInfo
	{
//		[DataMember]
		public CbxItemCode[] CbxSelItem { get; set; } = { C_DV_PERIOD, C_SX_NUMNUM2, C_DV_PERIOD, C_SX_NAMCOPY1 };

//		[DataMember]
		public string[] CustomText { get; set; } = { "", "", "", "" };
	}

	[DataContract(Namespace = "")]
	public class ShtFmtPerSetting : CbxInfo
	{
		[DataMember]
		public string NumberPrefix { get; set; } = ".X-";

		[DataMember]
		public string SheetNamePrefix { get; set; } = AppStrings.R_SheetName;

		[DataMember]
		public bool IncSheetName { get; set; } = false;

		[DataMember]
		public CbxItemCode[] CbxSelItem { get; set; } = { S_SX_NUMNUM1 };

		[DataMember]
		public string[] CustomText { get; set; } = { "" };
	}

	
	public interface CbxInfo
	{
		CbxItemCode[] CbxSelItem { get; set; }
		string[] CustomText { get; set; }
	}
}
