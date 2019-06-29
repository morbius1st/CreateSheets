using System.Runtime.Serialization;
using UtilityLibrary;

// there are no app specific saved settings

namespace CreateSheets2020
{
//	public static class SettingsApp
//	{
//		// this is the primary data structure - it holds the settings
//		// configuration information as well as the setting data
//		public static SettingsMgr<AppSettings> ASettings { get; private set; }
//
//		// this is just the setting data - this is a shortcut to
//		// the setting data
//		public static AppSettings ASet { get; private set; }
//
//		// initalize and create the setting objects
//		static SettingsApp()
//		{
//			ASettings = new SettingsMgr<AppSettings>();
//			ASet = ASettings.Settings;
//			ASettings.ResetData = ResetData;
//		}
//
//		// reset the settings data to their current value
//		public static void ResetData()
//		{
//			ASet = ASettings.Settings;
//		}
//	}
//
//
//	[DataContract(Name = "AppSettings")]
//	public class AppSettings : SettingsPathFileAppBase
//	{
//		// this is the version of this class
//		public override string FileVersion { get; } = "1.0";
//
//		[DataMember(Order = 1)]
//		public int AppI { get; set; } = 0;
//
//		[DataMember(Order = 2)]
//		public bool AppB { get; set; } = false;
//
//		[DataMember(Order = 3)]
//		public double AppD { get; set; } = 0.0;
//
//		[DataMember(Order = 4)]
//		public string AppS { get; set; } = "this is an App";
//
//		[DataMember(Order = 5)]
//		public int[] AppIs { get; set; } = new[] {20, 30};
//
//		
//	}
}

