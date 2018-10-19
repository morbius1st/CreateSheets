using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using UtilityLibrary;
using RevitLibrary;

using SharedCode.Resources;
using static SharedCode.ShNamePartType;
using static SharedCode.ShNamePartItemCode;


namespace SharedCode
{

	public enum CustomType
	{
		KEY  = -1,
		ITEM = -2
	}

	// these are the table numbers in the master table array
	// and they are the index in the newsheetformat namepartIdemcode array
	// except for number set number suffix
	public enum ShNamePartType
	{
		CUR_NUMDIVCHARS_TBL  = 0, // table number in the tables array
		CUR_NUMSUFFIX_TBL    = 1, // table number in the tables array
		CUR_NAMEDIVCHARS_TBL = 2, // table number in the tables array
		CUR_NAMESUFFIX_TBL   = 3, // table number in the tables array
		SET_NUMSUFFIX_TBL    = 4  // table number in the tables array
	}

	public enum ShNamePartItemCode
	{
		// < 0
		UNDEFINED    = -3,
		CUSTOMSET    = -2,
		CUSTOMSELECT = -1,
		
		// 0+
		C_DV_NONE   = CUR_NUMDIVCHARS_TBL * 100 + 0,
		C_DV_SPACE  = CUR_NUMDIVCHARS_TBL * 100 + 1,
		C_DV_PERIOD = CUR_NUMDIVCHARS_TBL * 100 + 2,
		C_DV_DASH   = CUR_NUMDIVCHARS_TBL * 100 + 3,
		
		// 100+
		C_SX_NUMNUM1  = CUR_NUMSUFFIX_TBL * 100 + 1,
		C_SX_NUMNUM2  = CUR_NUMSUFFIX_TBL * 100 + 2,
		C_SX_NUMNUM3  = CUR_NUMSUFFIX_TBL * 100 + 3,
		C_SX_NUMNUM4  = CUR_NUMSUFFIX_TBL * 100 + 4,
		C_SX_NUMNUM5  = CUR_NUMSUFFIX_TBL * 100 + 5,
		C_SX_NUMALPLC = CUR_NUMSUFFIX_TBL * 100 + 11,
		C_SX_NUMALPUC = CUR_NUMSUFFIX_TBL * 100 + 12,
		
		// 300+
		C_SX_NAMNONE  = CUR_NAMESUFFIX_TBL * 100 + 0,
		C_SX_NAMCOPY1 = CUR_NAMESUFFIX_TBL * 100 + 1,
		C_SX_NAMCOPYA = CUR_NAMESUFFIX_TBL * 100 + 2,
		C_SX_NAMALPLC = CUR_NAMESUFFIX_TBL * 100 + 11,
		C_SX_NAMALPUC = CUR_NAMESUFFIX_TBL * 100 + 12,
		C_SX_NAMNUM1  = CUR_NAMESUFFIX_TBL * 100 + 21,
		
		// 400+
		S_SX_NUMNUM1 = SET_NUMSUFFIX_TBL * 100 + 1,
		S_SX_NUMNUM2 = SET_NUMSUFFIX_TBL * 100 + 2,
		S_SX_NUMNUM3 = SET_NUMSUFFIX_TBL * 100 + 3,
		S_SX_NUMNUM4 = SET_NUMSUFFIX_TBL * 100 + 4,
		S_SX_NUMNUM5 = SET_NUMSUFFIX_TBL * 100 + 5
	}

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
				ShNamePartItem bi = new ShNamePartItem("[" + custom + "]", CUSTOMSET);

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

	public class ShNamePartItemsTables
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

		public ShNamePartItems[] Tables { get; private set; } = new ShNamePartItems[CBX_QTY];

		public static ShNamePartItemsTables Instance { get; } = new ShNamePartItemsTables();

		private ShNamePartItemsTables()
		{
			AssignBoxItems();
		}

		public string FindTitleByCode(ShNamePartItemCode code)
		{
			return FindByCode(code).Title;
		}

		public ShNamePartItem FindByCode(ShNamePartItemCode code)
		{
			int tableCode = ((int) code) / 100;
			int itemCode  = (int) code - tableCode;

			foreach (ShNamePartItem bi in Tables[tableCode].Collection)
			{
				if (bi.Code.Equals(code)) return bi;
			}

			return null;
		}

		private void AssignBoxItems()
		{
			Tables[(int)CUR_NUMDIVCHARS_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_DivNone  , C_DV_NONE            ),
					new ShNamePartItem(AppStrings.R_DivSpace , C_DV_SPACE           ),
					new ShNamePartItem(AppStrings.R_DivPeriod, C_DV_PERIOD          ),
					new ShNamePartItem(AppStrings.R_DivDash  , C_DV_DASH            ),
					new ShNamePartItem(AppStrings.R_DivCustom, CUSTOMSELECT         )
				}, PreSelectList[(int)CUR_NUMDIVCHARS_TBL], CUR_NUMDIVCHARS_TBL);

			Tables[(int)CUR_NUMSUFFIX_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric1, C_SX_NUMNUM1 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric2, C_SX_NUMNUM2 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric3, C_SX_NUMNUM3 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric4, C_SX_NUMNUM4 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric5, C_SX_NUMNUM5 ),
					new ShNamePartItem(AppStrings.R_SuffixNumAlphaLc , C_SX_NUMALPLC),
					new ShNamePartItem(AppStrings.R_SuffixNumAlphaUc , C_SX_NUMALPUC)
				}, PreSelectList[(int)CUR_NUMSUFFIX_TBL], CUR_NUMSUFFIX_TBL);

			Tables[(int)CUR_NAMEDIVCHARS_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_DivNone  , C_DV_NONE            ),
					new ShNamePartItem(AppStrings.R_DivSpace , C_DV_SPACE           ),
					new ShNamePartItem(AppStrings.R_DivPeriod, C_DV_PERIOD          ),
					new ShNamePartItem(AppStrings.R_DivDash  , C_DV_DASH            ),
					new ShNamePartItem(AppStrings.R_DivCustom, CUSTOMSELECT         )
				}, PreSelectList[(int)CUR_NAMEDIVCHARS_TBL], CUR_NAMEDIVCHARS_TBL);

			Tables[(int)CUR_NAMESUFFIX_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_SuffixNameNone   , C_SX_NAMNONE ),
					new ShNamePartItem(AppStrings.R_SuffixNameCopy1  , C_SX_NAMCOPY1),
					new ShNamePartItem(AppStrings.R_SuffixNameCopyA  , C_SX_NAMCOPYA),
					new ShNamePartItem(AppStrings.R_SuffixNameNumeric, C_SX_NAMNUM1 ),
					new ShNamePartItem(AppStrings.R_SuffixNameAlphaLc, C_SX_NAMALPLC),
					new ShNamePartItem(AppStrings.R_SuffixNameAlphaUc, C_SX_NAMALPUC),
					new ShNamePartItem(AppStrings.R_SuffixNameCustom , CUSTOMSELECT )
				}, PreSelectList[(int)CUR_NAMESUFFIX_TBL], CUR_NAMESUFFIX_TBL);

			Tables[(int)SET_NUMSUFFIX_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric1, S_SX_NUMNUM1 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric2, S_SX_NUMNUM2 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric3, S_SX_NUMNUM3 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric4, S_SX_NUMNUM4 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric5, S_SX_NUMNUM5 )
				}, PreSelectList[(int)SET_NUMSUFFIX_TBL], SET_NUMSUFFIX_TBL);
		}
	}
}