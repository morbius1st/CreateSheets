using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using UtilityLibrary;
using RevitLibrary;
using static SharedCode.ShNamePartItemCode;


namespace SharedCode
{
	// class for items that go into a combo box
	// items are suffix and divider codes and
	// their titles NamePartItem
	public class ShNamePartItem 
	{
		public string Title { get; set; }
		public ShNamePartItemCode Code { get; set; }

		public ShNamePartItem(string title, ShNamePartItemCode code)
		{
			Title = title;
			Code  = code;
		}

		public override string ToString()
		{
			return Title;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ShNamePartItem b)) return false;

			return b.Code.Equals(Code) &&
				b.Title.Equals(Title);
		}

		public override int GetHashCode()
		{
			return Title.GetHashCode();
		}

		public bool Equals(ShNamePartItemCode c)
		{
			return (int) c == (int) Code;
		}
	}

	public class ShNamePartItems : INotifyPropertyChanged
	{
		public ObservableCollection<ShNamePartItem> Collection { get; set; }

		public ShNamePartItem Selected { get; set; } = new ShNamePartItem("", UNDEFINED);

		public ShNamePartType Type { get; private set; }

		public string Custom { get; private set; } = "";

		public int DefaultItemIdx { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public ShNamePartItems()
		{
		}

		public ShNamePartItems(ObservableCollection<ShNamePartItem> boxItems, 
			int defSelectItemIdx, ShNamePartType type)
		{
			if (boxItems == null) throw new ArgumentNullException(nameof(boxItems), "combobox items is null");

			Collection = new ObservableCollection<ShNamePartItem>(boxItems);

			DefaultItemIdx = defSelectItemIdx;

			SetToDefault();

			Type = type;


		}

		public bool FindTitle(string search)
		{
			foreach (ShNamePartItem bi in Collection)
			{
				if (bi.Title.Equals(search))
				{
					return true;
				}
			}

			return false;
		}

		public int FindCode(int search)
		{
			int idx = -1;

			for (int i = 0; i < Collection.Count; i++)
			{
				if ((int) Collection[i].Code == search)
				{
					idx = i;
					break;
				}
			}

			return idx;
		}

		public void SetToDefault()
		{
			Selected = Collection[DefaultItemIdx];
		}

		public int UpdateCustom(string custom)
		{
			// validate the string provided
			if (!CsUtilities.ValidateStringChars(custom,
				RvtIdentifiers.INVALID_NAME_CHARS_ARRAY))
			{
				// invalid - return -1
				return -1;
			}

			// get the index for the current custom item
			// get -1 if not found
			int idx = FindCode((int) CustomType.ITEM);

			if (!string.IsNullOrWhiteSpace(custom))
			{
				// got a valid custom string

				// save custom
				this.Custom = custom;

				// create the box item
				ShNamePartItem bi = new ShNamePartItem("[" + custom + "]", 
					(int) Type * 100 + CUSTOMSET);

				// update the list of box items based on 
				// wheather the current custom value is found
				if (idx > -1)
				{
					// current custom item found -
					// remove the current and replace
					// with the updated valie
					Collection.RemoveAt(idx);
					Collection.Insert(idx, bi);
				}
				else
				{
					// custom item not found
					// add a new
					Collection.Add(bi);
					idx = Collection.Count - 1;
				}
			}
			else
			{
				// custom string is blank
				// if current exists - remove
				// else do nothing
				if (idx > -1)
				{
					Collection.RemoveAt(idx);
				}

				idx = -2;
			}

			OnPropertyChanged("Title");

			return idx;
		}

		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;

			handler?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	public static class Extension 
	{
		public static bool Equals(this ShNamePartItemCode c, ShNamePartItemCode test)
		{
			return (int) c == (int) test;
		}
	}
}