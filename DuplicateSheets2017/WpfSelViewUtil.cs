using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DuplicateSheets2017.Resources;

using SharedCode;
using SharedResources;
using static SharedCode.ShNamePartItemCode;

namespace DuplicateSheets2017
{
	public class WpfSelViewUtil
	{

//		public static string FormatShtName(string origShtName,
//			int suffixSequence, CbxItemCode divCode,
//			string customDivider, CbxItemCode suffixCode, string customSuffix)
//		{
//			if (suffixCode == C_SX_NAMNONE) return origShtName;
//
//			string divider = customDivider;
//			string suffix = customSuffix;
//
//			if ((int) divCode > (int) CUSTOMSELECT) divider = GetDivider(divCode);
//			if ((int) suffixCode > (int) CUSTOMSELECT) suffix = GetNameSuffix(suffixCode, suffixSequence);
//
//			return origShtName + divider + suffix;
//		}
//
//
//		public static string FormatShtName(string name, bool inc, int seq)
//		{
//			string result = name;
//
//			if (inc)
//			{
//				result += " " + seq;
//			}
//
//			return result;
//		}
//		
//		// sheet number cannot have a custom suffix
//		// for "from current"
//		public static string FormatShtNum(string origShtNum,
//			int suffixSequence, CbxItemCode divCode,
//			string customDivider, CbxItemCode suffixCode)
//		{
//			string divider = customDivider;
//			string suffix = GetNumSuffix(suffixCode, suffixSequence);
//
//			if ((int) divCode > (int) CUSTOMSELECT) divider = GetDivider(divCode);
//
//			return origShtNum + divider + suffix;
//		}
//
//		public static string FormatShtNum(string prefix, string format, int seq)
//		{
//			string result = prefix;
//			result += seq.ToString(format);
//
//			return result;
//		}
//
//		private static string GetNameSuffix(CbxItemCode suffixCode, int suffixSequence)
//		{
//			string result = "error";
//
//			switch (suffixCode)
//			{
//			case C_SX_NAMNONE:
//				{
//					result = "";
//					break;
//				}
//			case C_SX_NAMCOPY1:
//				{
//					result = $"{WpfWindows.R_Copy} {suffixSequence:D}";
//					break;
//				}
//			case C_SX_NAMCOPYA:
//				{
//					result = WpfWindows.R_Copy + " " + GetLetterCodeSequenced(suffixSequence, 'A');
//					break;
//				}
//			case C_SX_NAMNUM1:
//				{
//					result = $"{suffixSequence:D}";
//					break;
//				}
//			case C_SX_NAMALPLC:
//				{
//					result = GetLetterCodeSequenced(suffixSequence, 'a');
//					break;
//				}
//			case C_SX_NAMALPUC:
//				{
//					result = GetLetterCodeSequenced(suffixSequence, 'A');
//					break;
//				}
//			}
//
//			return result;
//		}
//
//		private static string GetNumSuffix(CbxItemCode suffixCode, int suffixSequence)
//		{
//			string result = "error";
//			int i = 1;
//
//			switch (suffixCode)
//			{
//			
//			case C_SX_NUMALPLC:
//				{
//					result = GetLetterCodeSequenced(suffixSequence, 'a');
//					break;
//				}
//			case C_SX_NUMALPUC:
//				{
//					result = GetLetterCodeSequenced(suffixSequence, 'A');
//					break;
//				}
//			default:
//				{
//					switch (suffixCode)
//					{
//					// default case - pre-assigned
////					case C_SX_NUMNUM1:
////						{
////							i = 1;
////							break;
////						}
//					case C_SX_NUMNUM2:
//						{
//							i = 2;
//							break;
//						}
//					case C_SX_NUMNUM3:
//						{
//							i = 3;
//							break;
//						}
//					case C_SX_NUMNUM4:
//						{
//							i = 4;
//							break;
//						}
//					case C_SX_NUMNUM5:
//						{
//							i = 5;
//							break;
//						}
//					}
//
//					string temp = $"{"{0:D"}{i:D}{"}"}";
//					result = String.Format(temp, suffixSequence);
//
//					break;
//				}
//			}
//
//			return result;
//		}
//
//		public static string GetLetterCodeSequenced(int sequence, char start)
//		{
//			string result = "";
//			int num = sequence - 1; // 1 = A or a
//
//			while (num >= 0)
//			{
//				char x = (char) (num % 26 + start);
//				result = x + result;
//				num = (num / 26) - 1;
//			}
//
//			return result;
//		}
//
//		public static string GetLetteerCodeSerial(int sequence, char start)
//		{
//			string result = "";
//			int qty = sequence / 26 + 1;
//			int code = sequence % 26;
//
//			for (int i = 0; i < qty; i++)
//			{
//				result += ((char) (code + start)).ToString();
//			}
//
//			return result;
//		}
//
//		private static string GetDivider(CbxItemCode divCode)
//		{
//			string result = "-error-";
//
//			switch (divCode)
//			{
//			case C_DV_NONE: // none
//				{
//					result = "";
//					break;
//				}
//			case C_DV_SPACE: // space
//				{
//					result = " ";
//					break;
//				}
//			case C_DV_PERIOD: // space
//				{
//					result = ".";
//					break;
//				}
//			case C_DV_DASH: // space
//				{
//					result = "-";
//					break;
//				}
//			}
//			
//			return result;
//		}

	}

}
