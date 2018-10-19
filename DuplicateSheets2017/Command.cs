#region Namespaces

using System;
using System.Windows;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using SharedCode;
using SharedCode.Resources;

using Document = Autodesk.Revit.DB.Document;

using static DuplicateSheets2017.Command;
using static DuplicateSheets2017.SettingsUser;

using static UtilityLibrary.MessageUtilities2;

#endregion

namespace DuplicateSheets2017
{

	[Transaction(TransactionMode.Manual)]
	public class Command : IExternalCommand
	{
		// private ShDBMgr m_DBMgr;

		public static WpfSelViewSheet WpfSelViewSheetWin;

		public static UIApplication _uiapp;
		private UIDocument _uidoc;
		private Document _document;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public Result Execute(ExternalCommandData commandData,
		  ref string message, ElementSet elements)
		{
		
			_uiapp = commandData.Application;
			_uidoc = _uiapp.ActiveUIDocument;
			_document = _uidoc.Document;

			string TransactionDesc = AppStrings.R_ButtonNameTop + " " + AppStrings.R_ButtonNameBott;

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

					ShUtil.ShowErrorDialog(e);

					return Result.Cancelled;
				}
			}

			return Result.Succeeded;
		}

	}


	[Transaction(TransactionMode.Manual)]
	public class Command_1Click : IExternalCommand
	{
		// private ShDBMgr m_DBMgr;

//		public static UIApplication _uiapp;
		private UIDocument _uidoc;
		private Document _document;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
		public Result Execute(ExternalCommandData commandData,
		  ref string message, ElementSet elements)
		{
		
			_uiapp = commandData.Application;
			_uidoc = _uiapp.ActiveUIDocument;
			_document = _uidoc.Document;

			ShDbMgr _DBMgr = new ShDbMgr(commandData);

			ViewSheet vs = null;

			string TransactionDesc = AppStrings.R_ButtonNameTop_1Click + " " + AppStrings.R_ButtonNameBott_1Click;

			
			USettings.Read();

			View v = _DBMgr.ActiveGraphicalView;

			if (v.ViewType == ViewType.DrawingSheet)
			{
				vs = v as ViewSheet;
			}

			MessageBox.Show("One Click is defined| " + USet.OneClick.Defined
				+ nl + " title block| " + (USet.OneClick.TitleBlockName ?? "is null")
				+ nl + " sheet title| " + (vs?.Name ?? "is null"), 
				"Got one click"
				, MessageBoxButton.OK);


			// determine if we can proceed
//			using (Transaction tx = new Transaction(_document))
//			{
//				tx.Start(TransactionDesc);
//
//				WpfSelViewSheetWin = new WpfSelViewSheet(commandData);
//
//
////				AppStrings.Culture = new CultureInfo("fr");
////				Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("fr");
//
//				// this works
////				Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr");
////				Debug.WriteLine("hello? " + AppStrings.Hello);
//
//				try
//				{
//					if ((WpfSelViewSheetWin.ShowDialog() ?? false) == false)
//					{
//						// user canceled the operation - return canceled result
//						tx.Dispose();
//						return Result.Cancelled;
//					}
//
//					tx.Commit();
//
//				}
//				catch (Exception e)
//				{
//					tx.Dispose();
//
//					ShUtil.ShowErrorDialog(e);
//
//					return Result.Cancelled;
//				}
//			}

			return Result.Succeeded;
		}

	}

}
