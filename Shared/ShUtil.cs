using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

using SharedCode.Resources;
using SharedLibrary;
using SharedResources;

using static UtilityLibrary.MessageUtilities;

namespace SharedCode
{
	internal static class ShUtil
	{
#pragma warning disable CS0414 // The field 'ShUtil.traceAppName' is assigned but its value is never used
		private static string traceAppName;
#pragma warning restore CS0414 // The field 'ShUtil.traceAppName' is assigned but its value is never used
		private static ShTraceLogging logger;

		public static string nl = Environment.NewLine;

		// load an image from embeded resource
		public static BitmapImage GetBitmapImage(string imageName)
		{
			return GetBitmapImage(imageName, Resources.AppStrings.R_NamespacePrefixSubject);
		}

		// load an image from embeded resource
		public static BitmapImage GetBitmapImage(string imageName, string folder)
		{
			traceAppName = nameof(ShUtil);
			logger = ShTraceLogging.Instance;

			Assembly b = Assembly.GetExecutingAssembly();

			Stream s = Assembly.GetExecutingAssembly().
				GetManifestResourceStream(LocalResMgr.AppIdentifier + "." + folder + "." + imageName);

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
				LocalResMgr.AppIdentifier,
				MessageBoxButtons.OK,
				MessageBoxIcon.Error,
				MessageBoxDefaultButton.Button1);
		}

		public static void ShowSelectSheetErrMsg()
		{
			TaskDialog errDialog = new TaskDialog(AppStrings.R_ErrSelShtTitle);

			errDialog.MainInstruction = AppStrings.R_ErrSelShtMainInst;
			errDialog.MainContent = AppStrings.R_ErrSelShtMainCont;
			errDialog.CommonButtons = TaskDialogCommonButtons.Close;
			errDialog.DefaultButton = TaskDialogResult.Close;
			errDialog.TitleAutoPrefix = false;

			errDialog.Show();
		}

		public static void ShowErrorDialog(string title, string main, string content)
		{
			TaskDialog errDialog = new TaskDialog(title);

			errDialog.MainInstruction = main;
			errDialog.MainContent = content;

			errDialog.CommonButtons = TaskDialogCommonButtons.Close;
			errDialog.DefaultButton = TaskDialogResult.Close;
			errDialog.MainIcon = TaskDialogIcon.TaskDialogIconWarning;
			errDialog.FooterText = ShConst.WebSiteReference;
			errDialog.TitleAutoPrefix = false;

			errDialog.Show();
		}

		public static void ShowExceptionDialog(Exception e, NewSheetFormat nsf, double parentLeft, double parentTop)
		{

			ErrorReport errorReport = new ErrorReport();

			errorReport.ParentLeft = parentLeft;
			errorReport.ParentTop = parentTop;

			errorReport.WindowTitle = LocalResMgr.WinTitleErrorReport;

			errorReport.tblkMessage.Text = AppStrings.R_ErrCannotProceed;

			StringBuilder sb = new StringBuilder();

			sb.AppendLine(AppStrings.R_ErrEmailTo);
			sb.AppendLine(AppStrings.R_EmailAddress).AppendLine();
			sb.Append(logMsgDbS("root exception")).AppendLine(AppStrings.R_ErrCreateSheetFailDesc ?? "none");
			sb.Append(logMsgDbS("primary exception")).AppendLine(e.Message ?? "none");
			sb.Append(logMsgDbS("inner exception")).AppendLine(e.InnerException?.Message ?? "none");
			sb.Append(logMsgDbS("source")).AppendLine(e.Source ?? "is null");
			sb.AppendLine(logMsgDbS("stack trace")).AppendLine(e.StackTrace ?? "none").AppendLine();
			sb.AppendLine(logMsgDbS("nsf")).Append(nsf?.ToString() ?? "is null").AppendLine();

			errorReport.ErrReport = sb.ToString();

			errorReport.ShowDialog();
		}
	}
}
