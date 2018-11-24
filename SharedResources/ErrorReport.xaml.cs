using System.Windows;

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
		#region + properties

		public string WindowTitle
		{
			get => Title;
			set => Title = value;
		}

		public string ErrReport
		{
			set => tblkStackTrace.Text = value;
		}

		public double ParentLeft
		{
			set
			{
				if (value >= 0)
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

		private void btnClose_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;

			Close();
		}
	}
}
