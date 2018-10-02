﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DuplicateSheets2017.Resources;
using UtilityLibrary;

using static DuplicateSheets2017.Command;

using SharedCode;
using static SharedCode.CbxItemCode;

namespace DuplicateSheets2017
{
	// the whole setting system auto initalzes - the first
	// call to one of the public elements, everything is 
	// processed.
	public static class SettingsUser
	{
		// this is the primary data structure - it holds the settings
		// configuration information as well as the setting data
		public static SettingsMgr<UserSettings> USettings { get; private set; }

		// this is just the setting data - this is a shortcut to
		// the setting data
		public static UserSettings USet { get; private set; }

		// initalize and create the setting objects
		static SettingsUser()
		{
			USettings = new SettingsMgr<UserSettings>(ResetData);
			USet = USettings.Settings;
		}

		// reset the settings data to their current value
		public static void ResetData()
		{
			USet = USettings.Settings;
		}

	}

	public static class SettingExtensions
	{
		public static NewSheetFormat Clone(this NewSheetFormat orig)
		{
			NewSheetFormat copy = new NewSheetFormat(orig.Defined);

			copy.OperationOption = orig.OperationOption;
			copy.NewSheetOption = orig.NewSheetOption;
//			copy.UseParameters = orig.UseParameters;

			copy.SheetFormatFrmCurrent = orig.SheetFormatFrmCurrent.Clone();
			copy.SheetFormatPerSetting = orig.SheetFormatPerSetting.Clone();

			return copy;
		}

		public static ShtFmtPerSetting Clone(this ShtFmtPerSetting orig)
		{
			ShtFmtPerSetting copy = new ShtFmtPerSetting();

			copy.IncSheetName = orig.IncSheetName;
			copy.NumberPrefix = orig.NumberPrefix;
			copy.SheetNamePrefix = orig.SheetNamePrefix;

			for (var i = 0; i < orig.CbxSelItem.Length; i++)
			{
				copy.CbxSelItem[i] = orig.CbxSelItem[i];
			}

			for (var i = 0; i < orig.CustomText.Length; i++)
			{
				copy.CustomText[i] = orig.CustomText[i];
			}

			return copy;
		}

		public static ShtFmtFrmCurrent Clone(this ShtFmtFrmCurrent orig)
		{
			ShtFmtFrmCurrent copy = new ShtFmtFrmCurrent();

			for (var i = 0; i < orig.CbxSelItem.Length; i++)
			{
				copy.CbxSelItem[i] = orig.CbxSelItem[i];
			}

			for (var i = 0; i < orig.CustomText.Length; i++)
			{
				copy.CustomText[i] = orig.CustomText[i];
			}

			return copy;
		}
	}

	// this is the actual data set saved to the user's configuration file
	// this is unique for each program
	[DataContract]
	public class UserSettings : SettingsPathFileUserBase
	{
		// this is just the version of this class
		public UserSettings()
		{
			SetDefaultValues();
		}

		public sealed override string FileVersion { get; set; }

		// this is for a first time / not been saved before location
		[DataMember(Order = 1)]
		public WinLocation WinLocMainWin { get; set; }

		[DataMember(Order = 2)]
		public WinLocation WinLocCustomTextDlg { get; set; }

		// sample info
		[DataMember(Order = 33)]
		public ShtFmtSample SheetFormatSample { get; set; }

		[DataMember(Order = 41)]
		public NewSheetFormat Basic { get; set; }

		[DataMember(Order = 42)]
		public NewSheetFormat OneClick { get; set; }


		// provide all of the default values here
		private void SetDefaultValues()
		{
			FileVersion = "2.0";

			WinLocMainWin =
				new WinLocation(_uiapp.MainWindowExtents.Top + 20,
					_uiapp.MainWindowExtents.Left            + 20);

			WinLocCustomTextDlg =
				new WinLocation(_uiapp.MainWindowExtents.Top + 40,
					_uiapp.MainWindowExtents.Left            + 40);

			SheetFormatSample = new ShtFmtSample();

			Basic    = new NewSheetFormat(true);
			OneClick = new NewSheetFormat(false);

		}

		[OnDeserializing]
		void OnDeserializing(StreamingContext context)
		{
			SetDefaultValues();
		}
	}

	[DataContract]
	public class WinLocation
	{
		[DataMember]
		public int    Top   { get; set; }

		[DataMember]
		public int    Left  { get; set; }
		
		[DataMember]
		public double Width { get; set; }

		//		// these are "default" values that can be
		//		// set by the app before the first usage of the settings system
		//		public static int top;
		//		public static int left;

		// parameterless constructor is required 
		// by datacontractserializer

		public WinLocation(int top, int left)
		{
			Top   = top;
			Left  = left;
			Width = 0;
		}

		public WinLocation(int top, int left, double width)
		{
			Top   = top;
			Left  = left;
			Width = width;
		}
	}

//	[DataContract(Name = "NewSheetFormatSettings")]
//	public class NewSheetFormat
//	{
//		[DataMember]
//		public bool Defined { get; set; } = false;
//
//		public int Copies;
//		public string TitleBlockName;
//
//		[DataMember]
//		public OperOpType OperationOption { get; set; } = OperOpType.DupSheetAndViews;
//
//		[DataMember]
//		public RbNewShtOptions NewSheetOption { get; set; } = RbNewShtOptions.FromCurrent;
//
//		[DataMember]
//		public ShtFmtFrmCurrent SheetFormatFrmCurrent { get; set; } = new ShtFmtFrmCurrent();
//		
//		[DataMember]
//		public ShtFmtPerSetting SheetFormatPerSetting { get; set; } = new ShtFmtPerSetting();
//
//		public NewSheetFormat(bool defined)
//		{
//			Defined = defined;
//		}
//	}
//
//	public class ShtFmtSample
//	{
//		public int Sequence             { get; set; } = 1;
//		public string SampleSheetNumber { get; set; } = "A01.01-00";
//		public string SampleSheetName   { get; set; } = WpfWindows.R_ShtOpCurSampleShtName;
//	}
//
//	public class ShtFmtFrmCurrent : CbxInfo
//	{
//		public CbxItemCode[] CbxSelItem { get; set; } = { C_DV_PERIOD, C_SX_NUMNUM2, C_DV_PERIOD, C_SX_NAMCOPY1 };
//		public string[] CustomText      { get; set; } = { "", "", "", "" };
//	}
//
//	public class ShtFmtPerSetting : CbxInfo
//	{
//		public string NumberPrefix      { get; set; } = ".X-";
//		public string SheetNamePrefix   { get; set; } = "Sheet Name";
//		public bool IncSheetName        { get; set; } = false;
//		public CbxItemCode[] CbxSelItem { get; set; } = {S_SX_NUMNUM1 };
//		public string[] CustomText      { get; set; } = {"" };
//	}
//
//	public interface CbxInfo
//	{
//		CbxItemCode[] CbxSelItem        { get; set; } 
//		string[] CustomText             { get; set; }
//	}

}

