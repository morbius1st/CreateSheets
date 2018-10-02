#region Namespaces

using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedCode.Resources;

#endregion

namespace CreateSheets
{
	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{
		internal static FormSelViewSheet FormSelViewShtDlg;

		public Result Execute(ExternalCommandData commandData,
		  ref string message, ElementSet elements)
		{
			UIApplication uiapp = commandData.Application;
			UIDocument uidoc = uiapp.ActiveUIDocument;
			Document doc = uidoc.Document;

			string TransactionDesc = AppStrings.R_ButtonNameTop + " " + AppStrings.R_ButtonNameBott;

			// determine if we can proceed
			using (Transaction tx = new Transaction(doc))
			{
				tx.Start(TransactionDesc);
				
				using (FormSelViewShtDlg = new FormSelViewSheet(commandData))
				{
					if (FormSelViewShtDlg.ShowDialog() == DialogResult.Cancel)
					{
						// user canceled the operation - return canceled result
						return Autodesk.Revit.UI.Result.Cancelled;
					}
				}
				tx.Commit();
			}
			return Result.Succeeded;
		}
	}

}
