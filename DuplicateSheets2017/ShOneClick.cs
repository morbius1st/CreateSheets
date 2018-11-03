#region + Using Directives

using System.Windows;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedCode;
using static DuplicateSheets2017.SettingsUser;
using static UtilityLibrary.MessageUtilities2;
#endregion


// projname: DuplicateSheets2017
// itemname: ShOneClick
// username: jeffs
// created:  11/3/2018 2:09:11 PM


namespace DuplicateSheets2017
{
	public class ShOneClick
	{
		private NewSheetFormat _nsf;


		public ShOneClick()
		{
			USettings.Read();

			_nsf = USet.OneClick;
		}

		public bool Process(ExternalCommandData commandData)
		{
			ViewSheet vs = null;

			ShDbMgr dbMgr = new ShDbMgr(commandData);

			View v = dbMgr.ActiveGraphicalView;

			if (v.ViewType == ViewType.DrawingSheet)
			{
				vs = v as ViewSheet;
			}

			NewSheetFormat nsf = USet.OneClick;

			ViewSheet tempVs = dbMgr.FindExistSheet(nsf.TemplateSheetNumber);

			MessageBox.Show("One Click is defined| " + nsf.Defined
				+ nl + " title block| " + (nsf.TitleBlockName ?? "is null")
				+ nl + " act graph view| " + (vs?.Name ?? "is null")
				+ nl + " use temp sheet| " + nsf.UseTemplateSheet
				+ nl + " temp sht number| " + (nsf.TemplateSheetNumber ?? "is null")
				+ nl + " temp sht name| " + (nsf.TemplateSheetName ?? "is null")
				+ nl + " temp viewsheet found| " + (tempVs != null)


				,
				"Got one click"
				, MessageBoxButton.OK);


			return false;
		}


	}
}