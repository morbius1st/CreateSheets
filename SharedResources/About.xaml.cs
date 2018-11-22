using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using SharedCode.Resources;

namespace SharedResources
{
	/// <summary>
	/// Interaction logic for ErrorReport.xaml
	/// </summary>
	public partial class About : Window
	{
		public const string test = "about";

		public About()
		{
			InitializeComponent();
		}

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
