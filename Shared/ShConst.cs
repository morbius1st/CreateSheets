using SharedCode.Resources;

namespace SharedCode
{
	public static class ShConst
	{
		public const int SET_NUMSUFX = 0;

		public const int COPIES_START = 1;
		public  const int COPIES_END = 100;

		public static string WebSiteReference = "<a href=\"" + AppStrings.R_CyberStudioAddr + "\">" + AppStrings.R_CyberStudioTitle + "</a>";

		public static string App_Name => LocalResMgr.AppName;

	}
}