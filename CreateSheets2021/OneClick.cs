#region + Using Directives

using Autodesk.Revit.UI;
using static CreateSheets2021.SettingsUser;

#endregion


// projname: CreateSheets2021
// itemname: OneClick
// username: jeffs
// created:  11/3/2019 2:09:11 PM

/*
 * in order to proceed
 * one click must have
 * 1.  valid method to number and name sheets
 * 2.  direction about which sheet to duplicate
 * 3.  direction about which title block to use
 * 4.  direction / information about the template sheet
 * 5.  if template, the template sheet must exist
 */


namespace CreateSheets2021
{
	public class OneClick
	{
		public bool Process(ExternalCommandData commandData)
		{
			USettings.Read();

			return SharedCode.ShOneClick.Process(commandData, USet.OneClick);
//
//			
//
//			ViewSheet vs = null;
//
//			ShDbMgr dbMgr = new ShDbMgr(commandData);
//
//			
//
//
//			if (nsf.Defined)
//			{
//				View v;
//
//				if (nsf.UseTemplateSheet)
//				{
//					nsf.TemplateSheetView = dbMgr.FindExistSheet(nsf.TemplateSheetNumber);
//
//					if (nsf.TemplateSheetView == null)
//					{
//						ErrorTemplateNotFound(nsf.TemplateSheetNumber, nsf.TemplateSheetName);
//						return false;
//					}
//
//					vs = nsf.TemplateSheetView;
//				}
//				else 
//				{
//					v = dbMgr.ActiveGraphicalView;
//
//					if (nsf.NewSheetOption == NewShtOptions.FromCurrent)
//					{
//						if (v.ViewType == ViewType.DrawingSheet)
//						{
//							vs = v as ViewSheet;
//						}
//						else
//						{
//							ErrorNotSheetView();
//							return false;
//						}
//					}
//				}
//
//				nsf.SelectedSheet = new SheetData(vs.SheetNumber, vs.ViewName, vs);
//
//				dbMgr.Process2(nsf);
//			}
//			else
//			{
//				ErrorSettingsNotDefined();

//				return false;
//			}
//
//			return true;
		}
//
//		private static void ErrorTemplateNotFound(string tempShtNum, string tempShtName)
//		{
//			ShowErrorDialog(AppStrings.R_ErrNoTemplateTitle, AppStrings.R_ErrNoTemplateMain,
//				AppStrings.R_ErrNoTemplateContent + "\n" + tempShtNum + " :: " + tempShtName);
//		}
//		
//		private static void ErrorSettingsNotDefined()
//		{
//			ShowErrorDialog(AppStrings.R_ErrOneClickSettingsMissingTitle, AppStrings.R_ErrOneClickSettingsMissingMain,
//				AppStrings.R_ErrOneClickSettingsMissingContent);
//		}
//
//		private static void ErrorNotSheetView()
//		{
//			ShowErrorDialog(AppStrings.R_ErrNotViewSheetTitle, AppStrings.R_ErrNotViewSheetMain,
//				AppStrings.R_ErrNotViewSheetContent);
//		}
	}
}