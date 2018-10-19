﻿using System;
using System.Collections.Generic;
using System.Text;
using RevitLibrary;
using static SharedCode.Resources.AppStrings;

namespace SharedCode
{
	public class ShData
	{
		public static int test = 0;

		public string[] CustomLabel =
		{
			R_EnterDivChars, // num div chars	[0]
			R_EnterSuffix   // num suffix		[1]
		};
		public string[] CustomTitle =
		{
			R_WindowTitleDivChars,
			R_WindowTitleSuffix
		};

		public string[] CustomInvalidTitle =
		{
			R_CharactersInvalid,
			""
		};

		public string[] CustomInvalidChars =
		{
			RvtIdentifiers.INVALID_NAME_CHARACTERS,
			""
		};
	}

	public static class ShConst
	{
		public const int SET_NUMSUFX = 0;

	}

	public enum NewShtOptions
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
}
