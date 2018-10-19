﻿#region Using directives

using static DuplicateSheets2017.Resources.AppLocalStrings;

using Autodesk.Revit.DB;

#endregion

// itemname:	ResxMgr
// username:	jeffs
// created:		4/1/2018 6:20:51 AM


namespace SharedCode
{
	// provide access to local resources
	public static class LocalResMgr
	{
		public static string AppName => R_AppName;
		public static string NamespacePrefix => R_AppName + "." + Resources.AppStrings.R_NamespacePrefixSubject;
		public static string WinTitleErrorReport => R_AppName + "  " + Resources.AppStrings.R_WindowTitleErrorReport;
		public const string XMLNS = @"http://schemas.datacontract.org/2004/07/DuplicateSheets2017";

		public static string ButtonName => AppName;
		public static string ButtonName_1Click => AppName + "_1Click";

		public static string Command_1Click => R_AppName + "." + Resources.AppStrings.R_Command_1Click;
		public static string Command => R_AppName + "." + Resources.AppStrings.R_Command;
	}
}
