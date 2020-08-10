using System.Collections.ObjectModel;
using SharedCode.Resources;

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
		CUSTOMSET    = 98,
		CUSTOMSELECT = 99,

		// 0+
		C_DV_NUMNONE   = ShNamePartType.CUR_NUMDIVCHARS_TBL * 100 + 0,
		C_DV_NUMSPACE  = ShNamePartType.CUR_NUMDIVCHARS_TBL * 100 + 1,
		C_DV_NUMPERIOD = ShNamePartType.CUR_NUMDIVCHARS_TBL * 100 + 2,
		C_DV_NUMDASH   = ShNamePartType.CUR_NUMDIVCHARS_TBL * 100 + 3,
		C_DV_NUMCUST   = ShNamePartType.CUR_NUMDIVCHARS_TBL * 100 + CUSTOMSELECT,
		C_DV_NUMCUSTSET= ShNamePartType.CUR_NUMDIVCHARS_TBL * 100 + CUSTOMSET,
		
		// 100+
		C_SX_NUMNUM1  = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 1,
		C_SX_NUMNUM2  = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 2,
		C_SX_NUMNUM3  = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 3,
		C_SX_NUMNUM4  = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 4,
		C_SX_NUMNUM5  = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 5,
		C_SX_NUMALPLC = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 11,
		C_SX_NUMALPUC = ShNamePartType.CUR_NUMSUFFIX_TBL * 100 + 12,

		// 200+
		C_DV_NAMNONE   = ShNamePartType.CUR_NAMEDIVCHARS_TBL * 100 + 0,
		C_DV_NAMSPACE  = ShNamePartType.CUR_NAMEDIVCHARS_TBL * 100 + 1,
		C_DV_NAMPERIOD = ShNamePartType.CUR_NAMEDIVCHARS_TBL * 100 + 2,
		C_DV_NAMDASH   = ShNamePartType.CUR_NAMEDIVCHARS_TBL * 100 + 3,
		C_DV_NAMCUST   = ShNamePartType.CUR_NAMEDIVCHARS_TBL * 100 + CUSTOMSELECT,
		C_DV_NAMCUSTSET= ShNamePartType.CUR_NAMEDIVCHARS_TBL * 100 + CUSTOMSET,

		// 300+
		C_SX_NAMNONE  = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + 0,
		C_SX_NAMCOPY1 = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + 1,
		C_SX_NAMCOPYA = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + 2,
		C_SX_NAMALPLC = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + 11,
		C_SX_NAMALPUC = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + 12,
		C_SX_NAMNUM1  = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + 21,
		C_SX_CUST     = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + CUSTOMSELECT,
		C_SX_CUSTSET  = ShNamePartType.CUR_NAMESUFFIX_TBL * 100 + CUSTOMSET,
		
		// 400+
		S_SX_NUMNUM1 = ShNamePartType.SET_NUMSUFFIX_TBL * 100 + 1,
		S_SX_NUMNUM2 = ShNamePartType.SET_NUMSUFFIX_TBL * 100 + 2,
		S_SX_NUMNUM3 = ShNamePartType.SET_NUMSUFFIX_TBL * 100 + 3,
		S_SX_NUMNUM4 = ShNamePartType.SET_NUMSUFFIX_TBL * 100 + 4,
		S_SX_NUMNUM5 = ShNamePartType.SET_NUMSUFFIX_TBL * 100 + 5
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
			// 0+
			Tables[(int)ShNamePartType.CUR_NUMDIVCHARS_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_DivNone  , ShNamePartItemCode.C_DV_NUMNONE            ),
					new ShNamePartItem(AppStrings.R_DivSpace , ShNamePartItemCode.C_DV_NUMSPACE           ),
					new ShNamePartItem(AppStrings.R_DivPeriod, ShNamePartItemCode.C_DV_NUMPERIOD          ),
					new ShNamePartItem(AppStrings.R_DivDash  , ShNamePartItemCode.C_DV_NUMDASH            ),
					new ShNamePartItem(AppStrings.R_DivCustom, ShNamePartItemCode.C_DV_NUMCUST            )
				}, PreSelectList[(int)ShNamePartType.CUR_NUMDIVCHARS_TBL], ShNamePartType.CUR_NUMDIVCHARS_TBL);

			// 100+
			Tables[(int)ShNamePartType.CUR_NUMSUFFIX_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric1, ShNamePartItemCode.C_SX_NUMNUM1 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric2, ShNamePartItemCode.C_SX_NUMNUM2 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric3, ShNamePartItemCode.C_SX_NUMNUM3 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric4, ShNamePartItemCode.C_SX_NUMNUM4 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric5, ShNamePartItemCode.C_SX_NUMNUM5 ),
					new ShNamePartItem(AppStrings.R_SuffixNumAlphaLc , ShNamePartItemCode.C_SX_NUMALPLC),
					new ShNamePartItem(AppStrings.R_SuffixNumAlphaUc , ShNamePartItemCode.C_SX_NUMALPUC)
				}, PreSelectList[(int)ShNamePartType.CUR_NUMSUFFIX_TBL], ShNamePartType.CUR_NUMSUFFIX_TBL);

			// 200+
			Tables[(int)ShNamePartType.CUR_NAMEDIVCHARS_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_DivNone  , ShNamePartItemCode.C_DV_NAMNONE  ),
					new ShNamePartItem(AppStrings.R_DivSpace , ShNamePartItemCode.C_DV_NAMSPACE ),
					new ShNamePartItem(AppStrings.R_DivPeriod, ShNamePartItemCode.C_DV_NAMPERIOD),
					new ShNamePartItem(AppStrings.R_DivDash  , ShNamePartItemCode.C_DV_NAMDASH  ),
					new ShNamePartItem(AppStrings.R_DivCustom, ShNamePartItemCode.C_DV_NAMCUST  )
				}, PreSelectList[(int)ShNamePartType.CUR_NAMEDIVCHARS_TBL], ShNamePartType.CUR_NAMEDIVCHARS_TBL);

			// 300+
			Tables[(int)ShNamePartType.CUR_NAMESUFFIX_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_SuffixNameNone   , ShNamePartItemCode.C_SX_NAMNONE ),
					new ShNamePartItem(AppStrings.R_SuffixNameCopy1  , ShNamePartItemCode.C_SX_NAMCOPY1),
					new ShNamePartItem(AppStrings.R_SuffixNameCopyA  , ShNamePartItemCode.C_SX_NAMCOPYA),
					new ShNamePartItem(AppStrings.R_SuffixNameAlphaLc, ShNamePartItemCode.C_SX_NAMALPLC),
					new ShNamePartItem(AppStrings.R_SuffixNameAlphaUc, ShNamePartItemCode.C_SX_NAMALPUC),
					new ShNamePartItem(AppStrings.R_SuffixNameNumeric, ShNamePartItemCode.C_SX_NAMNUM1 ),
					new ShNamePartItem(AppStrings.R_SuffixNameCustom , ShNamePartItemCode.C_SX_CUST    )
				}, PreSelectList[(int)ShNamePartType.CUR_NAMESUFFIX_TBL], ShNamePartType.CUR_NAMESUFFIX_TBL);

			// 400+
			Tables[(int)ShNamePartType.SET_NUMSUFFIX_TBL]
				= new ShNamePartItems(new ObservableCollection<ShNamePartItem>()
				{
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric1, ShNamePartItemCode.S_SX_NUMNUM1 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric2, ShNamePartItemCode.S_SX_NUMNUM2 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric3, ShNamePartItemCode.S_SX_NUMNUM3 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric4, ShNamePartItemCode.S_SX_NUMNUM4 ),
					new ShNamePartItem(AppStrings.R_SuffixNumNumeric5, ShNamePartItemCode.S_SX_NUMNUM5 )
				}, PreSelectList[(int)ShNamePartType.SET_NUMSUFFIX_TBL], ShNamePartType.SET_NUMSUFFIX_TBL);
		}
	}
}