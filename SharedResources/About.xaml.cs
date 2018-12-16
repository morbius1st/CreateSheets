using System;
using System.Diagnostics;
using System.Resources;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharedCode;
using SharedCode.Resources;
using UtilityLibrary;




namespace SharedResources
{
	/// <summary>
	/// Interaction logic for ErrorReport.xaml
	/// </summary>
	///
	
	public partial class About : Window
	{
		public About()
		{
			this.Icon = ShUtil.GetBitmapImage("CyberStudio Icon.png", "Images");

			this.ibxAbout = new Image();
			this.ibxLogo = new Image();

			this.ibxAbout.Source = ShUtil.GetBitmapImage("CreateSheetsAbout v2.png", "Images");
			this.ibxLogo.Source = ShUtil.GetBitmapImage("CyberStudio Logo - Narrow.png", "Images");

			InitializeComponent();

			tbkVersion.Text = CsUtilities.AssemblyVersion;
			tbkProgramName.Text = SharedCode.LocalResMgr.AppName;
		}

		#region + properties

		public double ParentLeft
		{
			set
			{
				if (value >= 0 )
					this.Left = value + 50.0;
			}
		}

		public double ParentTop
		{
			set
			{
				if (value >= 0)
					this.Top = value + 50.0; 
			}
		}

		#endregion


		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;

			Close();
		}

		private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		public Uri WebSite
		{
			get
			{
				return new Uri(AppStrings.R_CyberStudioAddr);
			}
		}
	}
}
