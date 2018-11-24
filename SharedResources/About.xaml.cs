﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using SharedCode.Resources;

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
			InitializeComponent();
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
