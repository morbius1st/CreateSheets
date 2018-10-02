#region Using directives

using DuplicateSheets2017.Resources;
using SharedResources;
using static DuplicateSheets2017.Resources.AppLocalStrings;
using static SharedCode.ShUtil;




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
		public static string Command => R_AppName + "." + Resources.AppStrings.R_Command;
		public static string WinTitleErrorReport => R_AppName + "  " + Resources.AppStrings.R_WindowTitleErrorReport;

	}
}
