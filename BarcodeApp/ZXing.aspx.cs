using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;

using ZXing;
using ZXing.Common;
using ZXing.Rendering;

namespace BarcodeApp
{
	// https://github.com/micjahn/ZXing.Net
	// https://csharp.hotexamples.com/examples/ZXing/BarcodeReader/Decode/php-barcodereader-decode-method-examples.html
	// https://stackoverflow.com/questions/51362087/how-to-scan-the-barcode-image-using-zxing-in-mvc#51394480
	public partial class BarcodeZXing : Page
	{
		private string BarcodeFolder { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			Title = "ZXing.NET";
			pnlTempImage.Visible = false;
			pnlNewImage.Visible = false;
			lblTempImageError.Text = "";
			lblNewImageError.Text = "";

			if (!Page.IsPostBack)
			{
				var dataSource = GetEncodeTypes();
				ddlEncodeType.DataSource = dataSource;
				ddlEncodeType.DataValueField = "Key";
				ddlEncodeType.DataTextField = "Value";
				ddlEncodeType.DataBind();
				//ddlEncodeType.SelectedIndex = (int)BarcodeFormat.CODE_128;

				//txtCodeText.MaxLength = 128;  // TODO: determine max len
				txtCodeText.Text = Guid.NewGuid().ToString();  //"https://ironsoftware.com/csharp/barcode/"

				// JS event handlers to browse and upload with a single button
				btnUploadBarcode.Attributes.Add("onclick", "$('#FileUploadBarcode').click();");
				FileUploadBarcode.Attributes.Add("onchange", "return uploadBarcode();");
			}
			else
			{
				if (FileUploadBarcode.HasFile)
				{
					try
					{
						pnlTempBarcode.Visible = true;
						pnlTempImage.Visible = true;

						// Save as memory stream
						var imageBytes = FileUploadBarcode.FileBytes;
						using (var stream = new MemoryStream(imageBytes))
						{
							var bmp = new Bitmap(stream);
							ReadFromBitmap(bmp);
						}

						// Display as base-64 uri
						string base64 = Convert.ToBase64String(imageBytes);
						string dataUri = $"data:image/png;base64,{base64}";
						imgTempImage.ImageUrl = dataUri;
					}
					catch (Exception ex)
					{
						DisplayError(lblTempImageError, ex);
					}
				}
			}
		}

		private void DisplayError(System.Web.UI.WebControls.Label label, Exception ex)
		{
			label.Text = ex.Message;
		}

		#region " Read Barcodes "
		private void ReadFromFile(string filePath)
		{
			// create a barcode reader instance
			IBarcodeReader reader = new BarcodeReader();

			// load a bitmap
			var bitmap = (Bitmap)Image.FromFile(filePath);

			// detect and decode the barcode inside the bitmap
			var result = reader.Decode(bitmap);

			DisplayResult(result);
		}

		private void ReadFromBitmap(Bitmap bitmap)
		{
			// create a barcode reader instance
			IBarcodeReader reader = new BarcodeReader();

			// detect and decode the barcode inside the bitmap
			var result = reader.Decode(bitmap);

			DisplayResult(result);
		}

		private void DisplayResult(Result result)
		{
			if (result != null)
			{
				pnlTempImage.Visible = true;
				lblEncodeType.Text = result.BarcodeFormat.ToString();
				lblCodeText.Text = result.Text;
				//txtDecoderType.Text = result.BarcodeFormat.ToString();
				//txtDecoderContent.Text = result.Text;
			}
		}
		#endregion

		#region " Create Barcodes "
		private Dictionary<int, string> GetEncodeTypes()
		{
			var dict = EnumHelper.EnumToDictionary(typeof(BarcodeFormat));
			var ordered = dict.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
			return ordered;
		}

		private void CreateBarcode(string codeText, BarcodeFormat barcodeFormat)
		{
			IBarcodeWriter<Bitmap> writer = new BarcodeWriter<Bitmap>
			{
				Format = barcodeFormat,
				Renderer = new BitmapRenderer()
			};
			Bitmap bitmap = writer.Write(codeText);

			if (bitmap != null)
			{
				pnlNewImage.Visible = true;

				using (var ms = new MemoryStream())
				{
					bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
					string base64 = Convert.ToBase64String(ms.GetBuffer());
					string dataUri = $"data:image/png;base64,{base64}";

					imgNewImage.Height = bitmap.Height;
					imgNewImage.Width = bitmap.Width;
					imgNewImage.ImageUrl = dataUri;
				}
			}

			//var encoder = new Encoder();
			//BitMatrix bitMatrix = writer.Encode(codeText);
			//Bitmap bitmap = bitMatrix.ToBitmap();  // Obsolete - Use BarcodeWriter instead

			//writer.Options = new EncodingOptions
			//{
			//	GS1Format = true,       // Specifies whether the data should be encoded to the GS1 standard; FNC1 character is added in front of the data
			//	PureBarcode = false,    // Don't put the content string into the output image.
			//	Width = 300
			//};
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			//CreateBarcode(txtCodeText.Text, Enum.Parse(typeof(BarcodeFormat), ddlEncodeType.SelectedItem?.Value));
			BarcodeFormat format;
			Enum.TryParse<BarcodeFormat>(ddlEncodeType.SelectedItem?.Value, out format);
			CreateBarcode(txtCodeText.Text, format);
		}
		#endregion
	}
}