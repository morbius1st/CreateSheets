using System.Windows;
using System.Windows.Media;
using CreateSheets2018.Resources;
using RevitLibrary;
using static UtilityLibrary.MessageUtilities;
using static UtilityLibrary.CsUtilities;
using static CreateSheets2018.SettingsUser;

using SharedCode.Resources;

namespace CreateSheets2018
{
    /// <summary>
    /// Interaction logic for WpfCustomText.xaml
    /// </summary>
    public partial class WpfCustomText : Window
	{
		public static double h { get; set; } = 1.0;
	
		public string CustomText { get; private set; } = null;

		private string InvalidChars;

//		private static readonly string InvalidChars = AppStrings.R_CharactersInvalid
//				+ nl + RvtIdentifiers.INVALID_NAME_CHARACTERS;

		public WpfCustomText(string currCustDivChars, 
			string titleText, string labelText, 
			string invalidCharactersTitle, 
			string invalidCharacters)
		{
			SetWinLocation();

			InitializeComponent();

			this.Title = titleText;

			lblCustom.Content = labelText;
			tbCustomText.Focus();

			if (currCustDivChars != null)
			{
				tbCustomText.Text = currCustDivChars;
				tbCustomText.CaretIndex = currCustDivChars.Length;

			}

			InvalidChars = invalidCharactersTitle + nl + invalidCharacters;

			tbInvalidChar.Text = InvalidChars;
		}

		private void SetWinLocation()
		{
			this.Top = USet.WinLocCustomTextDlg.Top;
			this.Left = USet.WinLocCustomTextDlg.Left;
		}

		private void btnOk_Click(object sender, RoutedEventArgs e)
		{
			CustomText = tbCustomText.Text;
			this.DialogResult = true;
			Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e)
		{
			CustomText = null;
			this.DialogResult = false;
			Close();
		}

		private void tbCustomText_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			// is the character just entered bad
			if (!ValidateChar(e.Text[0], 
				RvtIdentifiers.INVALID_NAME_CHARS_ARRAY))
			{
				// bad
				e.Handled = true;
				tbInvalidChar.Foreground = Brushes.Red;
				tbInvalidChar.Text = AppStrings.R_CharacterInvalid;
			}
			else
			{
				tbInvalidChar.Foreground = Brushes.Black;
				tbInvalidChar.Text = InvalidChars;
			}
		}

		private void winCustomText_Loaded(object sender, RoutedEventArgs e)
		{
			SetWinLocation();
		}

		private void winCustomText_LocationChanged(object sender, System.EventArgs e)
		{
			USet.WinLocCustomTextDlg.Top = (int) this.Top;
			USet.WinLocCustomTextDlg.Left = (int) this.Left;
//			USettings.Save();
		}
	}
}
