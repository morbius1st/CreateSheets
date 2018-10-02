using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

using SharedCode.Resources;
using SharedResources;

namespace SharedCode
{
	internal static class ShUtil
	{
		public static string nl = Environment.NewLine;

		// load an image from embeded resource
		public static BitmapImage GetBitmapImage(string imageName)
		{
			Stream s = Assembly.GetExecutingAssembly().
				GetManifestResourceStream(LocalResMgr.NamespacePrefix + "." + imageName);

			BitmapImage img = new BitmapImage();

			img.BeginInit();
			img.StreamSource = s;
			img.EndInit();

			return img;

		}

		public static Image GetImage(string imageName)
		{
			Stream s = Assembly.GetExecutingAssembly().
				GetManifestResourceStream(LocalResMgr.NamespacePrefix + "." + imageName);

			return 	s == null ? null : new Bitmap(s);
		}

		private static void ShowSelectSheetErrorMessage()
		{
			MessageBox.Show(AppStrings.R_ErrSelectSheetFailDesc,
				LocalResMgr.AppName,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error,
				MessageBoxDefaultButton.Button1);
		}

		public static void ShowSelectSheetErrMsg()
		{
			TaskDialog errMsg = new TaskDialog(LocalResMgr.AppName);

			errMsg.MainInstruction = AppStrings.R_ErrSelShtMainInst;
			errMsg.MainContent = AppStrings.R_ErrSelShtMainCont;
			errMsg.CommonButtons = TaskDialogCommonButtons.Close;
			errMsg.DefaultButton = TaskDialogResult.Close;

			errMsg.Show();
		}

		public static void ShowErrorDialog(Exception e)
		{
			SharedResources.ErrorReport errorReport = new ErrorReport();

			errorReport.WindowTitle = LocalResMgr.WinTitleErrorReport;

			errorReport.tblkMessage.Text = AppStrings.R_ErrCannotProceed;

			errorReport.StackTrace =
				AppStrings.R_ErrEmailTo + nl 
				+ AppStrings.R_EmailAddress + nl + nl
				+ "root exception| "
				+ (AppStrings.R_ErrCreateSheetFailDesc ?? "none") + nl
				+ "primary exception| "
				+ (e.Message ?? "none") + nl
				+ "inner exception| "
				+ (e.InnerException?.Message ?? "none") + nl
				+ "stack trace| " + nl
				+ (e.StackTrace ?? "none");

			errorReport.ShowDialog();
		}

	}
}
