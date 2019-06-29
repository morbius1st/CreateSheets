#region + Using Directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Autodesk.Revit.DB;

#endregion


// projname: CreateSheets2020
// itemname: SheetData
// username: jeffs
// created:  8/15/2019 10:15:06 PM


namespace CreateSheets2020
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
}
