using EnvDTE80;


namespace SharedCode
{
	static class ShIdeMgr
	{
		/// <summary>
		/// Clears the debut window in the IDE
		/// </summary>
		public static void clear()
		{
#if DEBUG
			DTE2 ide = (DTE2) System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.12.0");
			ide.ToolWindows.OutputWindow.OutputWindowPanes.Item("Debug").Clear();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(ide);
#endif
		}

		
	}
}
