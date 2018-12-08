using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedCode.Resources;
using static SharedCode.ShUtil;


namespace SharedCode
{
	class ShOneClick
	{
		public static bool Process(ExternalCommandData commandData, NewSheetFormat nsf)
		{
//			USettings.Read();

			ViewSheet vs = null;

			ShDbMgr dbMgr = new ShDbMgr(commandData);

//			NewSheetFormat nsf = USet.OneClick;


			if (nsf.Defined)
			{
				View v;

				if (nsf.UseTemplateSheet)
				{
					nsf.TemplateSheetView = dbMgr.FindExistSheet(nsf.TemplateSheetNumber);

					if (nsf.TemplateSheetView == null)
					{
						ErrorTemplateNotFound(nsf.TemplateSheetNumber, nsf.TemplateSheetName);
						return false;
					}

					vs = nsf.TemplateSheetView;
				}
				else
				{
					v = dbMgr.ActiveGraphicalView;

					if (nsf.NewSheetOption == NewShtOptions.FromCurrent)
					{
						if (v.ViewType == ViewType.DrawingSheet)
						{
							vs = v as ViewSheet;
						}
						else
						{
							ErrorNotSheetView();
							return false;
						}
					}
				}

//				nsf.SelectedSheet = new SheetData(vs.SheetNumber, vs.ViewName, vs);
				nsf.SelectedSheet = new SheetData(vs.SheetNumber, vs.Name, vs);

				dbMgr.Process2(nsf);
			}
			else
			{
				ErrorSettingsNotDefined();
				return false;
			}

			return true;
		}

		private static void ErrorTemplateNotFound(string tempShtNum, string tempShtName)
		{
			ShowErrorDialog(AppStrings.R_ErrNoTemplateTitle, AppStrings.R_ErrNoTemplateMain,
				AppStrings.R_ErrNoTemplateContent + "\n" + tempShtNum + " :: " + tempShtName);
		}

		private static void ErrorSettingsNotDefined()
		{
			ShowErrorDialog(AppStrings.R_ErrOneClickSettingsMissingTitle, AppStrings.R_ErrOneClickSettingsMissingMain,
				AppStrings.R_ErrOneClickSettingsMissingContent);
		}

		private static void ErrorNotSheetView()
		{
			ShowErrorDialog(AppStrings.R_ErrNotViewSheetTitle, AppStrings.R_ErrNotViewSheetMain,
				AppStrings.R_ErrNotViewSheetContent);
		}
	}
}
