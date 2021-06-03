using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using SharedCode;
using SharedCode.Resources;
using SharedLibrary;
using UtilityLibrary;


namespace SharedResources
{
	/// <summary>
	/// Interaction logic for ErrorReport.xaml
	/// </summary>
	///
	public partial class DialogTrace : Window, INotifyPropertyChanged
	{
		private string assemblyName;
		private string appName;
		private string folderPath;
		private string processStatus;
		private string startStopBtnText;

		private bool configured;

		private ShTraceLogging logger;

		public DialogTrace(Window w)
		{
			InitializeComponent();

			logger = ShTraceLogging.Instance;
			assemblyName = CsUtilities.AssemblyVersion;
			appName = LocalResMgr.AppName;

			// this.Owner = w;

			StartStopBtnText = AppStrings.R_Trace_Start;

			OnPropertyChanged("Logger");
		}

	#region + properties

		public string FolderPath => folderPath;

		public ShTraceLogging Logger => logger;

		public bool Configured => configured;

		public string StartStopBtnText
		{
			get => startStopBtnText;
			private set
			{
				startStopBtnText = value;
				OnPropertyChanged();
			}
		}

		public string ProcessStatus
		{
			get => processStatus;
			set
			{
				processStatus = value;

				this.Dispatcher.Invoke(
					() => { tblk_statusProcess.Text = value; });

				logger.TraceEvent(TraceEventType.Information, 
					1, "ProcessStatus", "Complete", value);
			}
		}

	#endregion

	#region + public methods

		public void Config(string folderPath)
		{
			this.folderPath = folderPath;
			configured = true;

			logger.TraceFolderPath = folderPath;
		}

		public void Stop()
		{
			if (!logger.TraceActivated) return;

			logger.TraceEvent(TraceEventType.Information, 1, GetType().Name,
				nameof(Stop), "trace stopping");

			logger.Stop();
			logger.Close();
		}

	#endregion

	#region + private methods

		private void start()
		{
			if (logger.TraceActivated) return;

			logger.Activate(appName);
			logger.Start(SourceLevels.Information);

			logger.TraceEvent(TraceEventType.Information, 1, GetType().Name,
				nameof(start), "trace started");
		}

		private void Btn_traceShow_OnClick(object sender, RoutedEventArgs ex)
		{
			if (logger.TraceActivated)
			{
				Stop();
			}

			string filePath = logger.TraceFilePath;

			if (File.Exists(filePath))
			{
				Process process = new System.Diagnostics.Process();

				try
				{
					process.StartInfo.FileName = "explorer";
					process.StartInfo.Arguments = filePath;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.UseShellExecute = false;
					process.Start();
				}
				catch { }
			}
		}
		
		private void Btn_traceFolder_OnClick(object sender, RoutedEventArgs ex)
		{
			if (logger.TraceActivated)
			{
				Stop();
			}

			string folderPath = logger.TraceFolderPath;

			if (Directory.Exists(folderPath))
			{
				Process process = new System.Diagnostics.Process();

				try
				{
					process.StartInfo.FileName = "explorer";
					process.StartInfo.Arguments = folderPath;
					process.StartInfo.CreateNoWindow = true;
					process.StartInfo.UseShellExecute = false;
					process.Start();
				}
				catch { }
			}
		}

		private void Btn_traceStartStop_OnClick(object sender, RoutedEventArgs e)
		{
			if (!logger.TraceActivated)
			{
				StartStopBtnText = AppStrings.R_Trace_Stop;
				start();
			}
			else
			{
				StartStopBtnText = AppStrings.R_Trace_Start;
				Stop();
			}
		}

		public void btnHelp_Click(object sender, RoutedEventArgs e)
		{
			e.Handled = true;

			string resource = (string) ((Button) sender).Tag;

			ShHelpDialog.ShowHelpMessage(resource);
		}

		private void Btn_done_OnClick(object sender, RoutedEventArgs e)
		{
			Stop();
			Close();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

	#endregion
	}

	[ValueConversion(typeof(bool), typeof(bool))]
	public class BoolInverterConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool)) return false;

			return ! (bool) value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}