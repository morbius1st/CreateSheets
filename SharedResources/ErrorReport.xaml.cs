using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SharedResources
{
	/// <summary>
	/// Interaction logic for ErrorReport.xaml
	/// </summary>
	public partial class ErrorReport : Window
	{
		public ErrorReport()
		{
			InitializeComponent();
		}

		public string WindowTitle
		{
			get => Title;
			set => Title = value;
		}

		public string StackTrace
		{
			set => tblkStackTrace.Text = value;
		}

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;

			Close();
		}
	}
}
