using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit.UI;
using SharedCode;
using static SharedCode.ShUtil;
using SharedCode.Resources;


// v 3.0.0.0	Adjust to duplicate views when needed
// v 3.0.2.0	Adjust to make dependent views when applies (2017: works  | 2018: works)

// this is the code to add a ribbon tab / panel / button



namespace SharedDuplicate
{
	class AppRibbonDuplicate : IExternalApplication
	{
		public Result OnStartup(UIControlledApplication app)
		{
			try
			{
				// this will always use the add-ins tab - don't need to make the tab first
				RibbonPanel m_RibbonPanel = app.CreateRibbonPanel(AppStrings.R_UiPanelName);

				// create a button for the 'copy sheet' command
				if (!AddSplitPushButtons(m_RibbonPanel))
				{
					// creating the pushbutton failed
					MessageBox.Show(AppStrings.R_ErrMakeButtonFailTitle, AppStrings.R_ErrMakeButtonFailDesc,
						MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
					return Result.Failed;
				}

				return Result.Succeeded;
			}
			catch
			{
				return Result.Failed;
			}
		} // end OnStartup

		public Result OnShutdown(UIControlledApplication a)
		{
			try
			{
				// begin code here
				return Result.Succeeded;
			}
			catch
			{
				return Result.Failed;
			}
		} // end OnShutdown


		private bool AddSplitPushButtons(RibbonPanel rPanel)
		{
			ContextualHelp help = new ContextualHelp(ContextualHelpType.Url, AppStrings.R_CyberStudioDupSheetsAddr);

			try
			{
				// make push button 1
				PushButtonData pbData1 = MakePushButton(
					rPanel, LocalResMgr.ButtonName,
					AppStrings.R_ButtonNameTop + nl + AppStrings.R_ButtonNameBott,
					AppStrings.R_ButtonImage16,
					AppStrings.R_ButtonImage32,
					Assembly.GetExecutingAssembly().Location,
					LocalResMgr.Command, AppStrings.R_CommandDescription);

				if (pbData1 == null) return false;

				pbData1.SetContextualHelp(help);

				PushButtonData pbData2 = MakePushButton(
					rPanel, LocalResMgr.ButtonName_1Click,
					AppStrings.R_ButtonNameTopOneClick + nl + AppStrings.R_ButtonNameBottOneClick,
					AppStrings.R_ButtonImageOneClick16,
					AppStrings.R_ButtonImageOneClick32,
					Assembly.GetExecutingAssembly().Location,
					LocalResMgr.Command_1Click, AppStrings.R_CommandDescriptionOneClick);

				if (pbData2 == null) return false;

				SplitButtonData sbd = new SplitButtonData("splitButton", "DupSheets");
				SplitButton     sb  = rPanel.AddItem(sbd) as SplitButton;

				pbData2.SetContextualHelp(help);


				sb.SetContextualHelp(help);

				sb.AddPushButton(pbData1);
				sb.AddPushButton(pbData2);
			}
			catch
			{
				return false;
			}

			return true;
		}


//		// method to add a pushbutton to the ribbon
//		private bool AddPushButton(RibbonPanel Panel,
//			string ButtonName,
//			string ButtonText,
//			string Image16,
//			string Image32,
//			string dllPath,
//			string dllClass,
//			string ToolTip)
//		{
//			try
//			{
//				PushButtonData m_pdData = new PushButtonData(ButtonName,
//					ButtonText, dllPath, dllClass);
//				// if we have a path for a small image, try to load the image
//				if (Image16.Length != 0)
//				{
//					try
//					{
//						// load the image
//						m_pdData.Image = ShUtil.GetBitmapImage(Image16);
//					}
//					catch
//					{
//						// could not locate the image
//					}
//				}
//
//				// if have a path for a large image, try to load the image
//				if (Image32.Length != 0)
//				{
//					try
//					{
//						// load the image
//						m_pdData.LargeImage = ShUtil.GetBitmapImage(Image32);
//					}
//					catch
//					{
//						// could not locate the image
//					}
//				}
//
//				// set the tooltip
//				m_pdData.ToolTip = ToolTip;
//
//				// add it to the panel
//				PushButton m_pb = Panel.AddItem(m_pdData) as PushButton;
//
//				return true;
//			}
//			catch
//			{
//				return false;
//			}
//		}

		// method to make a pushbutton for the ribbon
		private PushButtonData MakePushButton(RibbonPanel Panel,
			string ButtonName,
			string ButtonText,
			string Image16,
			string Image32,
			string dllPath,
			string dllClass,
			string ToolTip)
		{
			try
			{
				PushButtonData pdData = new PushButtonData(ButtonName,
					ButtonText, dllPath, dllClass);
				// if we have a path for a small image, try to load the image
				if (Image16.Length != 0)
				{
					try
					{
						// load the image
						pdData.Image = ShUtil.GetBitmapImage(Image16);
					}
					catch
					{
						// could not locate the image
					}
				}

				// if have a path for a large image, try to load the image
				if (Image32.Length != 0)
				{
					try
					{
						// load the image
						pdData.LargeImage = ShUtil.GetBitmapImage(Image32);
					}
					catch
					{
						// could not locate the image
					}
				}

				// set the tooltip
				pdData.ToolTip = ToolTip;

//				ContextualHelp cHelp = new ContextualHelp(ContextualHelpType.Url, 
//					AppStrings.R_CyberStudioAddr);
//
//				pdData.SetContextualHelp(cHelp);

				return pdData;
			}
			catch
			{
				return null;
			}
		}
	}
}