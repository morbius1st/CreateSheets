#region Using directives

using CreateSheets.Resources;
using SharedResources;
using static SharedCode.ShUtil;
using static CreateSheets.Properties.Resources;

#endregion

// itemname:	Resources
// username:	jeffs
// created:		3/31/2018 10:48:50 PM


namespace SharedCode
{
	public static class LocalResMgr
	{
		public static string AppName => R_AppName;
		public static string NamespacePrefix => R_AppName + "." + R_NamespacePrefixSubject;
		public static string Command => R_AppName + "." + R_Command;
		public static string WinTitleErrorReport => R_AppName + "  " + Resources.AppStrings.R_WindowTitleErrorReport;

	}
}
