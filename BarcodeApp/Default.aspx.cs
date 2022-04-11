using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Web.UI;

namespace BarcodeApp
{
	public partial class _Default : Page
	{
		//private static Random random = new Random();

		protected void Page_Load(object sender, EventArgs e)
		{
			Page.Title = $"Home - {Properties.Settings.Default.AppName}";

			// https://security-code-scan.github.io/#SCS0005
			var sb = new StringBuilder();
			for (int i = 1; i <= 10; i++)
			{
				Thread.Sleep(1);
				string weak = GenerateWeakRandomNumber(1234);
				string strong = GenerateStrongRandomNumber();
				sb.Append($"Weak: {weak}    Strong: {strong}    <br \\>");
			}

			lblContent.Text = sb.ToString();
			lblContent.Text += "<br />" + GetHtmlEncodedValue("<");
			lblContent.Text += "<br />" + GetHtmlEncodedValue("⇑");
			lblContent.Text += "<br />" + GetHtmlEncodedValue("❌");
		}

		private static string GenerateWeakRandomNumber(int? seed = null)
		{
			var rnd = seed == null ? new Random() : new Random((int)seed);
			byte[] buffer = new byte[16];
			rnd.NextBytes(buffer);
			return BitConverter.ToString(buffer, 0);
		}

		private static string GenerateStrongRandomNumber()
		{
			byte[] buffer = new byte[16];
			var rnd = RandomNumberGenerator.Create();
			rnd.GetBytes(buffer);
			return BitConverter.ToString(buffer, 0);
		}

		private static string GetHtmlEncodedValue(string value)
		{
			var encoder = HtmlEncoder.Create();
			string encoded = encoder.Encode(value);
			if (encoded.StartsWith("&#x"))
			{
				Console.WriteLine("HEX format detected, converting to DEC format...");
				// Ex: "❌" => "&#x274C;" => "247C" => 10060 => "&#10060;"
				////string hex = encoded.Remove(0, 3).TrimEnd(';');
				////int dec = int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
				////return string.Format("&#{0}", dec);

				string hex = encoded.Remove(0, 3).TrimEnd(';');
				var ca = encoded.ToCharArray().ToList().RemoveAll(x => !char.IsLetterOrDigit(x));

				if (int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out int dec))
					return string.Format("&#{0}", dec);
				else
					return encoded;
			}
			else
			{
				return encoded;
			}
			//string hexValue = intValue.ToString("X");
			//int.Parse(HtmlEncoder.Create().Encode("❌").Remove(0, 3).TrimEnd(';'), System.Globalization.NumberStyles.HexNumber)
		}
	}
}