using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CreateSheets2021
{
	public partial class ShSheetDataList : ObservableCollection<SheetData>, INotifyPropertyChanged
	{
		public static ShSheetDataList SheetList { get; set; } = new ShSheetDataList();

		private SheetData _selectedSheet = new SheetData();

		private ShSheetDataList()
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
//
//		public void AddSheet(string number, string name, ViewSheet view)
//		{
//			if (!ContainsSheetNumber(number))
//			{
//				Add(new SheetData(number, name, view));
//			}
//		}

		public void AddSheet(SheetData sd)
		{
			if (!ContainsSheetNumber(sd.SheetNumber))
			{
				Add(sd);
			}
		}

		public SheetData SelectedSheet
		{
			get => _selectedSheet;
			set
			{
				if (value != null && !value.Equals(_selectedSheet))
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