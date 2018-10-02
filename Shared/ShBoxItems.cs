using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UtilityLibrary;
using RevitLibrary;

using SharedCode.Resources;
using static SharedCode.CbxType;
using static SharedCode.CbxItemCode;


namespace SharedCode
{
	public enum CustomType
	{
		KEY  = -1,
		ITEM = -2
	}

	public class ShBoxItems : INotifyPropertyChanged
	{
		public ObservableCollection<ShBoxItem> Collection { get; set; }

		public ShBoxItem Selected { get; set; } = new ShBoxItem("", UNDEFINED);

		public CbxType Type { get; private set; }

		public string Custom { get; private set; } = "";

		public int DefaultItemIdx { get; }

		public event PropertyChangedEventHandler PropertyChanged;

		public ShBoxItems()
		{
		}

		public ShBoxItems(ObservableCollection<ShBoxItem> boxItems, 
			int defSelectItemIdx, CbxType type)
		{
			if (boxItems == null) throw new ArgumentNullException(nameof(boxItems), "combobox items is null");

			Collection = new ObservableCollection<ShBoxItem>(boxItems);

			DefaultItemIdx = defSelectItemIdx;

			SetToDefault();

			Type = type;


		}

		public bool FindTitle(string search)
		{
			foreach (ShBoxItem bi in Collection)
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
				RvtIdentifiers.INVALID_NAME_CHARACTERS))
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
				ShBoxItem bi = new ShBoxItem("[" + custom + "]", CUSTOMSET);

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

	public class ShBoxItem 
	{
		public string Title { get; set; }

		public CbxItemCode Code { get; set; }

		public ShBoxItem(string title, CbxItemCode code)
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
			if (!(obj is ShBoxItem b)) return false;

			return b.Code.Equals(Code) &&
				b.Title.Equals(Title);
		}

		public override int GetHashCode()
		{
			return Title.GetHashCode();
//			return base.GetHashCode();
		}

		public bool Equals(CbxItemCode c)
		{
			return (int) c == (int) Code;
		}
	}

	public static class Extension 
	{
		public static bool Equals(this CbxItemCode c, CbxItemCode test)
		{
			return (int) c == (int) test;
		}
	}

	public class CbxBoxItems
	{
		private const int CBX_CURR_QTY   = 4;
		public const  int CBX_CURR_START = 0;
		public const  int CBX_CURR_END   = CBX_CURR_START + CBX_CURR_QTY - 1;

		private const int CBX_SET_QTY   = 1;
		public const  int CBX_SET_START = CBX_CURR_QTY;
		public const  int CBX_SET_END   = CBX_SET_START + CBX_SET_QTY - 1;

		public const int CBX_QTY = CBX_CURR_QTY + CBX_SET_QTY;

		// the index of the default item
		public static int[] PreSelectList { get; private set; } 
			= new int[] { 2, 0, 2, 0, 0 };

		public ShBoxItems[] Tables { get; private set; } = new ShBoxItems[CBX_QTY];

		public static CbxBoxItems Instance { get; } = new CbxBoxItems();

		private CbxBoxItems()
		{
			AssignBoxItems();
		}

		public string FindTitleByCode(CbxItemCode code)
		{
			return FindByCode(code).Title;
		}

		public ShBoxItem FindByCode(CbxItemCode code)
		{
			int tableCode = ((int) code) / 100;
			int itemCode  = (int) code - tableCode;

			foreach (ShBoxItem bi in Tables[tableCode].Collection)
			{
				if (bi.Code.Equals(code)) return bi;
			}

			return null;
		}

		private void AssignBoxItems()
		{
			Tables[(int) CUR_NUMDIVCHARS] 
				= new ShBoxItems(new ObservableCollection<ShBoxItem>()
				{
					new ShBoxItem(AppStrings.R_DivNone  , C_DV_NONE),
					new ShBoxItem(AppStrings.R_DivSpace , C_DV_SPACE),
					new ShBoxItem(AppStrings.R_DivPeriod, C_DV_PERIOD),
					new ShBoxItem(AppStrings.R_DivDash  , C_DV_DASH),
					new ShBoxItem(AppStrings.R_DivCustom, CUSTOMSELECT)
				}, PreSelectList[(int)CUR_NUMDIVCHARS], CUR_NUMDIVCHARS);

			Tables[(int) CUR_NUMSUFFIX] 
				= new ShBoxItems(new ObservableCollection<ShBoxItem>()
				{
					new ShBoxItem(AppStrings.R_SuffixNumNumeric1, C_SX_NUMNUM1),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric2, C_SX_NUMNUM2),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric3, C_SX_NUMNUM3),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric4, C_SX_NUMNUM4),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric5, C_SX_NUMNUM5),
					new ShBoxItem(AppStrings.R_SuffixNumAlphaLc , C_SX_NUMALPLC),
					new ShBoxItem(AppStrings.R_SuffixNumAlphaUc , C_SX_NUMALPUC)
				}, PreSelectList[(int)CUR_NUMSUFFIX], CUR_NUMSUFFIX);

			Tables[(int) CUR_NAMEDIVCHARS] 
				= new ShBoxItems(new ObservableCollection<ShBoxItem>()
				{
					new ShBoxItem(AppStrings.R_DivNone  , C_DV_NONE),
					new ShBoxItem(AppStrings.R_DivSpace , C_DV_SPACE),
					new ShBoxItem(AppStrings.R_DivPeriod, C_DV_PERIOD),
					new ShBoxItem(AppStrings.R_DivDash  , C_DV_DASH),
					new ShBoxItem(AppStrings.R_DivCustom, CUSTOMSELECT)
				}, PreSelectList[(int)CUR_NAMEDIVCHARS], CUR_NAMEDIVCHARS);

			Tables[(int) CUR_NAMESUFFIX] 
				= new ShBoxItems(new ObservableCollection<ShBoxItem>()
				{
					new ShBoxItem(AppStrings.R_SuffixNameNone   , C_SX_NAMNONE),
					new ShBoxItem(AppStrings.R_SuffixNameCopy1  , C_SX_NAMCOPY1),
					new ShBoxItem(AppStrings.R_SuffixNameCopyA  , C_SX_NAMCOPYA),
					new ShBoxItem(AppStrings.R_SuffixNameNumeric, C_SX_NAMNUM1),
					new ShBoxItem(AppStrings.R_SuffixNameAlphaLc, C_SX_NAMALPLC),
					new ShBoxItem(AppStrings.R_SuffixNameAlphaUc, C_SX_NAMALPUC),
					new ShBoxItem(AppStrings.R_SuffixNameCustom , CUSTOMSELECT)
				}, PreSelectList[(int)CUR_NAMESUFFIX], CUR_NAMESUFFIX);

			Tables[(int) SET_NUMSUFFIX] 
				= new ShBoxItems(new ObservableCollection<ShBoxItem>()
				{
					new ShBoxItem(AppStrings.R_SuffixNumNumeric1, S_SX_NUMNUM1),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric2, S_SX_NUMNUM2),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric3, S_SX_NUMNUM3),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric4, S_SX_NUMNUM4),
					new ShBoxItem(AppStrings.R_SuffixNumNumeric5, S_SX_NUMNUM5)
				}, PreSelectList[(int)SET_NUMSUFFIX], SET_NUMSUFFIX);
		}
	}
}