#region Namespaces

using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedCode;
using SharedCode.Resources;

using Document = Autodesk.Revit.DB.Document;

using static DuplicateSheets2018.Command;
using static DuplicateSheets2018.SettingsUser;

#endregion

namespace DuplicateSheets2018
{

	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{

		public static WpfSelViewSheet WpfSelViewSheetWin;

		public static UIApplication _uiapp;
		public static ShDbMgr _DBMgr;
		private UIDocument _uidoc;
		private Document _document;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public Result Execute(ExternalCommandData commandData,
		  ref string message, ElementSet elements)
		{

			_uiapp = commandData.Application;
			_uidoc = _uiapp.ActiveUIDocument;
			_document = _uidoc.Document;

			_DBMgr = new ShDbMgr(commandData);

			string TransactionDesc = AppStrings.R_ButtonNameTop + " " + AppStrings.R_ButtonNameBott;

			if (_DBMgr.SheetCount > 0)
			{
				// determine if we can proceed
				using (Transaction tx = new Transaction(_document))
				{
					tx.Start(TransactionDesc);

					WpfSelViewSheetWin = new WpfSelViewSheet(commandData);

					//				AppStrings.Culture = new CultureInfo("fr");
					//				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr");

					// this works
					//				Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
					//				Debug.WriteLine("hello? " + AppStrings.Hello);

					try
					{
						if ((WpfSelViewSheetWin.ShowDialog() ?? false) == false)
						{
							// user canceled the operation - return canceled result
							tx.Dispose();
							return Result.Cancelled;
						}

						tx.Commit();
					}
					catch (Exception e)
					{
						tx.Dispose();

						ShUtil.ShowExceptionDialog(e, USet.Basic, _DBMgr.ParentLeft, _DBMgr.ParentTop);

						return Result.Cancelled;
					}
				}
			}
			else
			{
				ShUtil.ShowErrorDialog(AppStrings.R_ErrNoSheetsTitle,
					AppStrings.R_ErrNoSheetsMainMsg_DupShts, AppStrings.R_ErrNoSheetsContent);

				return Result.Failed;
			}

			return Result.Succeeded;
		}

	}


	[Transaction(TransactionMode.Manual)]
	public class Command_1Click : IExternalCommand
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public Result Execute(ExternalCommandData commandData,
		  ref string message, ElementSet elements)
		{
			_uiapp = commandData.Application;
			UIDocument _uidoc = _uiapp.ActiveUIDocument;
			Document _document = _uidoc.Document;

			_DBMgr = new ShDbMgr(commandData);

			string TransactionDesc = AppStrings.R_ButtonNameTop + " " + AppStrings.R_ButtonNameBott;

			if (_DBMgr.SheetCount > 0)
			{
				using (Transaction tx = new Transaction(_document))
				{
					tx.Start(TransactionDesc);

					OneClick oneClick = new OneClick();

					try
					{
						if (!oneClick.Process(commandData))
						{
							tx.Dispose();

							return Result.Failed;
						}

						tx.Commit();
					}
					catch (Exception e)
					{
						tx.Dispose();

						ShUtil.ShowExceptionDialog(e, USet.OneClick, _DBMgr.ParentLeft, _DBMgr.ParentTop);

						return Result.Failed;
					}
				}
			}
			else
			{
				ShUtil.ShowErrorDialog(AppStrings.R_ErrNoSheetsTitle,
					AppStrings.R_ErrNoSheetsMainMsg_OneClick, AppStrings.R_ErrNoSheetsContent);

				return Result.Failed;
			}

			return Result.Succeeded;
		}
	}

}
