#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

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
		private object _sheetView = null;

		public SheetData() {}

		public SheetData(string number, string name, object view)
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

		public object SheetView
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
			if (obj == null) return false;


			if (typeof(object) != typeof(SheetData))
			{
				return false;
			}

			return this.CompareTo((SheetData) obj) == 0;
		}


		public override string ToString()
		{
			return _sheetNumber + " :: " + _sheetName;
		}

		// ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
		public override int GetHashCode() => base.GetHashCode();

		public SheetData Clone()
		{
			return new SheetData(_sheetNumber, _sheetName, _sheetView);
		}


	}
}
