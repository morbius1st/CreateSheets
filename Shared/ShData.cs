using System;
using System.Collections.Generic;
using System.Text;
using static SharedCode.Resources.AppStrings;
using static SharedCode.CbxType;

namespace SharedCode
{
	public class ShData
	{
		public string[] CustomLabel =
		{
			R_EnterDivChars, // num div chars
			R_EnterSuffix,   // num suffix
			R_EnterDivChars, // name div chars
			R_EnterSuffix    // name suffix
		};
		public string[] CustomTitle =
		{
			R_WindowTitleDivChars,
			R_WindowTitleSuffix,
			R_WindowTitleDivChars,
			R_WindowTitleSuffix
		};
	}

	public enum RbNewShtOptions
	{
		PerSettings,
		FromCurrent
	}

	public enum OperOpType
	{
		DupSheet,
		DupSheetAndViews,
		CreateEmptySheets
	}

	public enum CbxType
	{
		SET_NUMSUFX = 0,
		CUR_NUMDIVCHARS = 0,
		CUR_NUMSUFFIX = 1,
		CUR_NAMEDIVCHARS = 2,
		CUR_NAMESUFFIX = 3,
		SET_NUMSUFFIX = 4
	}

	public enum CbxItemCode
	{
		// < 0
		UNDEFINED = -3,
		CUSTOMSET = -2,
		CUSTOMSELECT = -1,

		// 0+
		C_DV_NONE = CUR_NUMDIVCHARS * 100 + 0,
		C_DV_SPACE = CUR_NUMDIVCHARS * 100 + 1,
		C_DV_PERIOD = CUR_NUMDIVCHARS * 100 + 2,
		C_DV_DASH = CUR_NUMDIVCHARS * 100 + 3,

		// 100+
		C_SX_NUMNUM1 = CUR_NUMSUFFIX * 100 + 1,
		C_SX_NUMNUM2 = CUR_NUMSUFFIX * 100 + 2,
		C_SX_NUMNUM3 = CUR_NUMSUFFIX * 100 + 3,
		C_SX_NUMNUM4 = CUR_NUMSUFFIX * 100 + 4,
		C_SX_NUMNUM5 = CUR_NUMSUFFIX * 100 + 5,
		C_SX_NUMALPLC = CUR_NUMSUFFIX * 100 + 11,
		C_SX_NUMALPUC = CUR_NUMSUFFIX * 100 + 12,

		// 300+
		C_SX_NAMNONE = CUR_NAMESUFFIX * 100 + 0,
		C_SX_NAMCOPY1 = CUR_NAMESUFFIX * 100 + 1,
		C_SX_NAMCOPYA = CUR_NAMESUFFIX * 100 + 2,
		C_SX_NAMALPLC = CUR_NAMESUFFIX * 100 + 11,
		C_SX_NAMALPUC = CUR_NAMESUFFIX * 100 + 12,
		C_SX_NAMNUM1 = CUR_NAMESUFFIX * 100 + 21,

		// 400+
		S_SX_NUMNUM1 = SET_NUMSUFFIX * 100 + 1,
		S_SX_NUMNUM2 = SET_NUMSUFFIX * 100 + 2,
		S_SX_NUMNUM3 = SET_NUMSUFFIX * 100 + 3,
		S_SX_NUMNUM4 = SET_NUMSUFFIX * 100 + 4,
		S_SX_NUMNUM5 = SET_NUMSUFFIX * 100 + 5
	}

}
