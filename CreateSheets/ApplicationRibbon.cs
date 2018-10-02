using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using Autodesk.Revit.UI;


// v 3.0.0.0	Adjust to duplicate views when needed
// v 3.0.2.0	Adjust to make dependent views when applies (2017: works  | 2018: works)

// this is the code to add a ribbon tab / panel / button
namespace CreateSheets
{
	class ApplicationRibbon : IExternalApplication
	{
		private const string NAMESPACE_PREFIX = "CreateSheets.Resources";

		private const string BUTTON_NAME = "  Create  \nSheets";
		private const string PANEL_NAME = "AO Tools";
		private const string TAB_NAME = "AO Tools";

		private static bool textFixed = false;


		public Result OnStartup(UIControlledApplication app)
		{

			try
			{
				// create the ribbon tab first - this is the top level
				// UI item, below this will be the panel that is "on" the tab
				// and below this will be a button that is "on" the panel
				// give the tab a name
				string m_tabName = TAB_NAME;

				// first create the tab
				try 
				{ 
					// try to create the ribbon panel
					app.CreateRibbonTab(m_tabName);
				}
				catch (Exception)
				{
					// might already exist - do nothing
				}
				
				// got the tab now

				// create the ribbon panel if needed
				// give the panel a name
				string m_panelName = PANEL_NAME;

				RibbonPanel m_RibbonPanel = null;

				// check to see if the panel alrady exists
				// get the Panel within the tab by name
				List<RibbonPanel> m_RP = new List<RibbonPanel>();

				m_RP = app.GetRibbonPanels(m_tabName);

				foreach (RibbonPanel xRP in m_RP)
				{
					if (xRP.Name.ToUpper().Equals(m_panelName.ToUpper()))
					{
						m_RibbonPanel = xRP;
						break;
					}
				}

				// if
				// add the panel if it does not exist
				if (m_RibbonPanel == null)
				{
					// create the ribbon panel on the tab given the tab's name
					// FYI - leave off the ribbon panel's name to put onto the "add-in" tab
					m_RibbonPanel = app.CreateRibbonPanel(m_tabName, m_panelName);
				}


				// create a button for the 'copy sheet' command
				if (!AddPushButton(m_RibbonPanel, "CreateSheets", BUTTON_NAME,
					"CreateSheets_16.png",
					"CreateSheets_32.png",
					Assembly.GetExecutingAssembly().Location, "CreateSheets.Command", "Duplicate Existing Sheet"))
				
				{
					// creating the pushbutton failed
					MessageBox.Show("Failed to Add Button!", "Duplicate Sheet Warning",
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

		// method to add a pushbutton to the ribbon
		private bool AddPushButton(RibbonPanel Panel, string ButtonName,
			string ButtonText, string Image16, string Image32,
			string dllPath, string dllClass, string ToolTip)
		{
			try
			{
				PushButtonData m_pdData = new PushButtonData(ButtonName,
					ButtonText, dllPath, dllClass);
				// if we have a path for a small image, try to load the image
				if (Image16.Length != 0)
				{
					try
					{
						// load the image
						m_pdData.Image = Util.getBitmapImage(Image16);
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
						m_pdData.LargeImage = Util.getBitmapImage(Image32);
					}
					catch
					{
						// could not locate the image
					}
				}

				// set the tooltip
				m_pdData.ToolTip = ToolTip;

				// add it to the panel
				PushButton m_pb = Panel.AddItem(m_pdData) as PushButton;

				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}
