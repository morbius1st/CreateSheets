#region + Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using static UtilityLibrary.MessageUtilities2;
	
#endregion


// projname: DuplicateSheets2017
// itemname: SheetData
// username: jeffs
// created:  8/15/2018 10:15:06 PM


namespace SharedCode
{
	public class SheetData : INotifyPropertyChanged, 
		IComparable<SheetData>, IComparer<SheetData>
	{
		private string _sheetNumber = String.Empty;
		private string _sheetName = String.Empty;
		private ViewSheet _sheetView = null;

		public SheetData() {}

		public SheetData(string number, string name, ViewSheet view)
		{
			SheetNumber = number;
			SheetName = name;
			SheetView = view;
		}

		public string SheetNumber
		{
			get => _sheetNumber;
			set
			{
				if (value != _sheetNumber)
				{
					_sheetNumber = value;
					OnPropetryChange();
				}
			}
		}

		public string SheetName
		{
			get => _sheetName;
			set
			{
				if (value != _sheetName)
				{
					_sheetName = value;
					OnPropetryChange();
				}
			}
		}

		public ViewSheet SheetView
		{
			get => _sheetView;
			set
			{
				if (_sheetView != value)
				{
					_sheetView = value;
					OnPropetryChange();
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropetryChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		public int CompareTo(SheetData other)
		{
			int i = String.CompareOrdinal(_sheetNumber, other._sheetNumber);

			if (i == 0)
			{
				i = String.CompareOrdinal(_sheetName, other._sheetName);
			}

			return i;
		}

		public int Compare(SheetData x, SheetData y)
		{
			return x.CompareTo(y);
		}

		public override bool Equals(object obj)
		{
			if (typeof(object) != typeof(SheetData))
			{
				return false;
			}

			return this.CompareTo((SheetData) obj) == 0;
		}

		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
		public override int GetHashCode() => base.GetHashCode();
	}

	public class SheetDataList : ObservableCollection<SheetData>, INotifyPropertyChanged
	{
		public static SheetDataList SheetList { get; set; } = new SheetDataList();

		private SheetData _selectedSheet = new SheetData();

		private SheetDataList()
		{


			Add(new SheetData("A100", "Sheet A100", null));
			Add(new SheetData("A101", "Sheet A101", null));
			Add(new SheetData("A102", "Sheet A102", null));
			Add(new SheetData("A103", "Sheet A103", null));
			Add(new SheetData("A104", "Sheet A100", null));
			Add(new SheetData("A105", "Sheet A101", null));
			Add(new SheetData("A106", "Sheet A102", null));
			Add(new SheetData("A107", "Sheet A103", null));
			Add(new SheetData("A108", "Sheet A100", null));
			Add(new SheetData("A109", "Sheet A101", null));
			Add(new SheetData("A110", "Sheet A102", null));
			Add(new SheetData("A111", "Sheet A103", null));
			Add(new SheetData("A112", "Sheet A102", null));
			Add(new SheetData("A113", "Sheet A103", null));
			Add(new SheetData("A114", "Sheet A100", null));

		}

		public void AddSheet(string number, string name, ViewSheet view)
		{
			if (!ContainsSheetNumber(number))
			{
				Add(new SheetData(number, name, view));
			}
		}

		public SheetData SelectedSheet
		{
			get => _selectedSheet;
			set
			{
				if (value != null  && !value.Equals(_selectedSheet))
				{
					_selectedSheet = value;
	
					OnPropertyChange();
				}
			}
		}

		protected override event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChange([CallerMemberName] string memberName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
		}

		private bool ContainsSheetNumber(string testSheetNumber)
		{
			return IndexOfSheetNumber(testSheetNumber) >= 0;
		}

		private int IndexOfSheetNumber(string testSheetNumber)
		{
			int i = 0;

			foreach (SheetData sd in this)
			{
				if (sd.SheetNumber.Equals(testSheetNumber))
				{
					return i;
				}
				i++;
			}
			return -1;
		}

		public new bool Contains(SheetData test)
		{
			return IndexOf(test) >= 0;
		}

		public new int IndexOf(SheetData test)
		{
			int i = 0;

			foreach (SheetData sd in this)
			{
				if (sd.CompareTo(test) == 0)
				{
					return i;
				}

				i++;
			}

			return -1;
		}




	}
}
