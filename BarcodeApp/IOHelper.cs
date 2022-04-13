using System.IO;
using System.Reflection;

namespace BarcodeApp
{
	public static class IOHelper
	{
		public static string GetBaseDirectory()
		{
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
		}

		public static string GetBarcodeDirectory()
		{
			return Path.Combine(Directory.GetParent(GetBaseDirectory()).Parent.FullName, "Barcodes");
		}

		public static string GetFileExtension(string fileName)
		{
			var objFileInfo = new FileInfo(fileName);
			return objFileInfo.Extension;
		}

		public static string GetMimeContentType(string mimeType)
		{
			// Return the file extension part of the MIME string (i.e. "image/pjpeg")
			var arrKeys = mimeType.Split('/');
			if (arrKeys.Length == 2)
				return arrKeys[1].Trim();
			else
				return "";
		}
	}
}