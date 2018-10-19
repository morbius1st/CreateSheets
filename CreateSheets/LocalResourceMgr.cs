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
		public static string WinTitleErrorReport => R_AppName + "  " + Resources.AppStrings.R_WindowTitleErrorReport;
		public const string XMLNS = @"";

		public static string Command => R_AppName + "." + R_Command;

		// the below have not been verified and may be wrong
		public static string ButtonName => AppName;
		public static string ButtonName_1Click => AppName + "_1Click";

		public static string Command_1Click => R_AppName + "." + Resources.AppStrings.R_Command_1Click;


	}
}
