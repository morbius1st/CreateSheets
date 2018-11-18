using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

using SharedCode.Resources;
using SharedResources;

using static UtilityLibrary.MessageUtilities;

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

		public static void ShowErrorDialog(string title, string main, string content)
		{
			TaskDialog err = new TaskDialog(LocalResMgr.AppName + ", " + title);

			err.MainInstruction = main;
			err.MainContent = content;

			err.CommonButtons = TaskDialogCommonButtons.Close;
			err.DefaultButton = TaskDialogResult.Close;
			err.MainIcon = TaskDialogIcon.TaskDialogIconWarning;

			err.Show();
		}

		public static void ShowExceptionDialog(Exception e, NewSheetFormat nsf)
		{
			SharedResources.ErrorReport errorReport = new ErrorReport();

			errorReport.WindowTitle = LocalResMgr.WinTitleErrorReport;

			errorReport.tblkMessage.Text = AppStrings.R_ErrCannotProceed;

			StringBuilder sb = new StringBuilder();

			sb.AppendLine(AppStrings.R_ErrEmailTo);
			sb.AppendLine(AppStrings.R_EmailAddress).AppendLine();
			sb.Append(logMsgDbS("root exception")).AppendLine(AppStrings.R_ErrCreateSheetFailDesc ?? "none");
			sb.Append(logMsgDbS("primary exception")).AppendLine(e.Message ?? "none");
			sb.Append(logMsgDbS("inner exception")).AppendLine(e.InnerException?.Message ?? "none");
			sb.AppendLine(logMsgDbS("stack trace")).AppendLine(e.StackTrace ?? "none").AppendLine();
			sb.AppendLine(logMsgDbS("nsf")).Append(nsf?.ToString() ?? "is null").AppendLine();

			errorReport.ErrReport = sb.ToString();

			errorReport.ShowDialog();
		}

		public static void ShowHelpMessage(string resourceName)
		{
			string helpSubject = AppStrings.ResourceManager.GetString("R_" + resourceName + "Title") + " Help";

			TaskDialog help = new TaskDialog(LocalResMgr.AppName + ", " + helpSubject);

			help.MainInstruction = helpSubject;
			help.MainContent = AppStrings.ResourceManager.GetString("R_" + resourceName);

			help.CommonButtons = TaskDialogCommonButtons.Close;
			help.DefaultButton = TaskDialogResult.Close;
			help.MainIcon = TaskDialogIcon.TaskDialogIconNone;

			help.Show();

		}

	}
}
