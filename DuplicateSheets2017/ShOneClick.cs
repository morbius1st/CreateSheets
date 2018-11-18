#region + Using Directives

using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedCode;
using SharedCode.Resources;
using static DuplicateSheets2017.SettingsUser;
using static UtilityLibrary.MessageUtilities2;
#endregion


// projname: DuplicateSheets2017
// itemname: ShOneClick
// username: jeffs
// created:  11/3/2018 2:09:11 PM

/*
 * in order to proceed
 * one click must have
 * 1.  valid method to number and name sheets
 * 2.  direction about which sheet to duplicate
 * 3.  direction about which title block to use
 * 4.  direction / information about the template sheet
 * 5.  if template, the template sheet must exist
 */


namespace DuplicateSheets2017
{
	public class ShOneClick
	{

		public bool Process(ExternalCommandData commandData)
		{
			USettings.Read();

			ViewSheet vs = null;

			ShDbMgr dbMgr = new ShDbMgr(commandData);

			NewSheetFormat nsf = USet.OneClick;


			if (nsf.Defined)
			{
//				MessageBox.Show(nsf.ToString(), "Got one click", MessageBoxButton.OK);

				if (nsf.UseTemplateSheet)
				{
					nsf.TemplateSheetView = dbMgr.FindExistSheet(nsf.TemplateSheetNumber);
				}

				View v = dbMgr.ActiveGraphicalView;

				if (nsf.NewSheetOption == NewShtOptions.FromCurrent)
				{
					if (v.ViewType == ViewType.DrawingSheet)
					{
						vs = v as ViewSheet;

						nsf.SelectedSheet = new SheetData(vs.SheetNumber, vs.ViewName, vs);
					}
					else
					{
						TaskDialog td = new TaskDialog("");

						td.MainInstruction = "Current View is not a Sheet";
						td.MainContent = "New Sheet Option is to use the current view "
							+ "as the basis for the new sheet's number and name. "
							+ "Since the current view is not a sheet view, I don't have "
							+ "a sheet number or name to use as the basis for the number "
							+ "and name of the new sheets.\n\n"
							+ "Please make a sheet view as the current view and select One Click again.";
						td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
						td.CommonButtons = TaskDialogCommonButtons.Ok;
						td.DefaultButton = TaskDialogResult.Ok;
						td.FooterText = ShConst.WebSiteReference;

						td.Show();

						return false;
					}
				}

				dbMgr.Process2(nsf);
			}
			else
			{
				TaskDialog td = new TaskDialog("");

				td.MainInstruction = "One Click Settings are not defined";
				td.MainContent = "One click settings have not been configured and saved.  Please open the "
					+ "duplicate sheets dialog, configure your one click settings, and save the settings";
				td.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
				td.CommonButtons = TaskDialogCommonButtons.Ok;
				td.DefaultButton = TaskDialogResult.Ok;
				td.FooterText = ShConst.WebSiteReference;

				TaskDialogResult result = td.Show();

				return false;
			}

			return true;
		}


	}
}