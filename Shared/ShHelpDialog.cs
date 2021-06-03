using Autodesk.Revit.UI;
using SharedCode.Resources;

namespace SharedCode
{
	static internal class ShHelpDialog
	{
		public static void ShowHelpMessage(string resourceName)
		{
			string helpSubject = AppStrings.ResourceManager.GetString("R_" + resourceName + "Title") + " Help";

//			TaskDialog help = new TaskDialog(LocalResMgr.AppIdentifier + ", " + helpSubject);
			TaskDialog help = new TaskDialog(AppStrings.R_HelpTitle);

			help.MainInstruction = helpSubject;
			help.MainContent = AppStrings.ResourceManager.GetString("R_" + resourceName);

			help.CommonButtons = TaskDialogCommonButtons.Close;
			help.DefaultButton = TaskDialogResult.Close;
			help.MainIcon = TaskDialogIcon.TaskDialogIconNone;
			help.TitleAutoPrefix = false;
			help.FooterText = ShConst.WebSiteReference;

			help.Show();

		}
	}
}