#region Using directives

using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;

using SharedCode.Resources;


#endregion

// itemname:	SxUiMgr
// username:	jeffs
// created:		4/1/2018 7:04:27 AM


// this is saved code that is no longer needed - this will make a 
// ribbon tab (if it does not exist) and panel (if it does not exist)

namespace SharedCode
{
	internal static class SxUiMgr
	{
		// this needs to be within a try / catch block
		public static RibbonPanel GetRibbonPanel(UIControlledApplication app)
		{
			// create the ribbon tab first - this is the top level
			// UI item, below this will be the panel that is "on" the tab
			// and below this will be a button that is "on" the panel
			// give the tab a name
			string m_tabName = AppStrings.R_UiTabName;

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
			string m_panelName = AppStrings.R_UiPanelName;

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

			return m_RibbonPanel;
		}

	}
}
