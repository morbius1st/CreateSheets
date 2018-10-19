using System;
using SharedCode.Resources;
using static SharedCode.ShNamePartItemCode;

namespace SharedCode
{
	public static class ShNewSheetMgr 
	{
		private static ShNamePartItemsTables npit = ShNamePartItemsTables.Instance;


		// format a sheet number

		// per settings
		public static string FormatShtNumber(string prefix, ShNamePartItemCode code, int seq)
		{
			return prefix + FormatPsSeq(code, seq);
		}

		// for from current
		public static string FormatShtNumber(string origShtNum,
			ShNamePartItemCode divCode, string customDivider, ShNamePartItemCode suffixCode,
			int seq)
		{
			string divider = customDivider;
			string suffix  = GetNumSuffix(suffixCode, seq);

			if ((int) divCode > (int) CUSTOMSELECT) divider = GetDivider(divCode);

			return origShtNum + divider + suffix;

		}

		// format a sheet name

		// per settings
		public static string FormatShtName(string name, bool inc, 
			ShNamePartItemCode code, int seq)
		{
			string result = name;

			if (inc)
			{
				result += " " + FormatPsSeq(code, seq);
			}

			return result;
		}

		// for from current
		public static string FormatShtName(  string origShtName,
			ShNamePartItemCode divCode,
			string customDivider, ShNamePartItemCode suffixCode, string customSuffix,
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

		private static string FormatPsSeq(ShNamePartItemCode code, int seq)
		{
			return seq.ToString(npit.FindTitleByCode(code));
		}




		private static string GetNameSuffix(ShNamePartItemCode suffixCode, int suffixSequence)
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

		private static string GetNumSuffix(ShNamePartItemCode suffixCode, int suffixSequence)
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

		public static string GetLetterCodeSerial(int sequence, char start)
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

		private static string GetDivider(ShNamePartItemCode divCode)
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
}
