﻿#region Using directives

using static CreateSheets2019.Resources.AppLocalStrings;

#endregion

// itemname:	ResxMgr
// username:	jeffs
// created:		4/1/2019 6:20:51 AM


namespace SharedCode
{
	// provide access to local resources
	public static class LocalResMgr
	{
		public static string AppIdentifier => RL_AppIdentifier;
		public static string AppName => RL_AppName;
		public static string NamespacePrefix => RL_AppIdentifier + "." + Resources.AppStrings.R_NamespacePrefixSubject;
		public static string WinTitleErrorReport => RL_AppIdentifier + "  " + Resources.AppStrings.R_WindowTitleErrorReport;
		public const string XMLNS = @"http://schemas.datacontract.org/2004/07/CreateSheets2019";

		public static string ButtonName => AppIdentifier;
		public static string ButtonName_1Click => AppIdentifier + "_1Click";

		public static string Command_1Click => RL_AppIdentifier + "." + Resources.AppStrings.R_Command1Click;
		public static string Command => RL_AppIdentifier + "." + Resources.AppStrings.R_Command;
	}
}
