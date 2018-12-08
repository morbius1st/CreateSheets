#region Using directives

using static DuplicateSheets2019.Resources.AppLocalStrings;

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
		public static string AppName => RL_AppName;
		public static string NamespacePrefix => RL_AppName + "." + Resources.AppStrings.R_NamespacePrefixSubject;
		public static string WinTitleErrorReport => RL_AppName + "  " + Resources.AppStrings.R_WindowTitleErrorReport;
		public const string XMLNS = @"http://schemas.datacontract.org/2004/07/DuplicateSheets2019";

		public static string ButtonName => AppName;
		public static string ButtonName_1Click => AppName + "_1Click";

		public static string Command_1Click => RL_AppName + "." + Resources.AppStrings.R_Command1Click;
		public static string Command => RL_AppName + "." + Resources.AppStrings.R_Command;
	}
}
