using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace CreateSheets
{
	class Util
	{
		private const string NAMESPACE_PREFIX = "CreateSheets.Resources";

		// load an image from embeded resource
		public static BitmapImage getBitmapImage(string imageName)
		{
			Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(NAMESPACE_PREFIX + "." + imageName);

			BitmapImage img = new BitmapImage();

			img.BeginInit();
			img.StreamSource = s;
			img.EndInit();

			return img;

		}

		public static Image getImage(string imageName)
		{
			Stream s = Assembly.GetExecutingAssembly().GetManifestResourceStream(NAMESPACE_PREFIX + "." + imageName);

			return new Bitmap(s);
		}


	}
}
