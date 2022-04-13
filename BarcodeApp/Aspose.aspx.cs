using Aspose.BarCode.BarCodeRecognition;
using Aspose.BarCode.Generation;

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BarcodeApp
{
	public partial class BarcodeAspose : Page
	{
		private string BarcodeFolder { get; set; }

		protected void Page_Load(object sender, EventArgs e)
		{
			Title = "Aspose.Barcode";
			pnlTempImage.Visible = false;
			pnlNewImage.Visible = false;
			lblTempImageError.Text = "";
			lblNewImageError.Text = "";

			if (!Page.IsPostBack)
			{
				var dataSource = EncodeTypes.AllEncodeTypes.ToList().OrderBy(x => x.TypeName);
				ddlEncodeType.DataSource = dataSource;
				ddlEncodeType.DataTextField = "TypeName";
				ddlEncodeType.DataValueField = "TypeIndex";
				ddlEncodeType.DataBind();
				ddlEncodeType.SelectedIndex = EncodeTypes.Code128.TypeIndex;

				//txtCodeText.MaxLength = 128;  // TODO: determine max len
				txtCodeText.Text = Guid.NewGuid().ToString();  //"https://ironsoftware.com/csharp/barcode/"

				//barcodeDirectory = IOHelper.GetBarcodeDirectory();

				//if (!Directory.Exists(barcodeDirectory)) Directory.CreateDirectory(barcodeDirectory);
				//string codeText = "1234567";
				//string filePath = Path.Combine(barcodeDirectory, "Output.jpg");
				//if (File.Exists(filePath)) File.Delete(filePath);
				//SaveAsImage(filePath, codeText, EncodeTypes.Code128, BarCodeImageFormat.Jpeg);

				//string demoFile = Path.Combine(barcodeDirectory, "GetStarted.png"); //@"d:\template.jpg";
				//ReadFromImage(demoFile);

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

						// Save as physical file
						//string tempPath = UploadFile();
						//ReadFromFile(tempPath);
						//imgTempImage.ImageUrl = $"file:///{tempPath}"; //Server.MapPath(tempPath);

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

		private void DisplayError(Label label, Exception ex)
		{
			label.Text = ex.Message;
		}

		#region " Read Barcodes "
		private string UploadFile()
		{
			// Limit the file size to a "reasonable" amount?
			// if (FileUploadBarcode.FileBytes.LongLength) > MaxImageFileBytes ...

			// Get the file type
			string mimeType = IOHelper.GetMimeContentType(FileUploadBarcode.PostedFile.ContentType);
			string extension = IOHelper.GetFileExtension(FileUploadBarcode.FileName);

			// Use MIME type (IE returns "pjpeg" instead of "jpeg")
			switch (mimeType.ToLower())
			{
				// Acceptable browser MIME types
				case "gif":
				case "jpeg":
				case "pjpeg":
				case "png":
					break;
				default:
					// Use file extention
					switch (extension)
					{
						// Acceptable image file extentions
						case ".gif":
						case ".jpg":
						case ".jpeg":
						case ".png":
							break;
						default:
							throw new BadImageFormatException($"The file type '{mimeType}' is not a supported.");
					}
					break;
			}

			// Get/create barcode folder
			BarcodeFolder = IOHelper.GetBarcodeDirectory();
			if (!Directory.Exists(BarcodeFolder)) Directory.CreateDirectory(BarcodeFolder);

			// Get/create temp folder
			string tempFolder = Path.Combine(BarcodeFolder, "Temp");
			if (!Directory.Exists(tempFolder)) Directory.CreateDirectory(tempFolder);

			string tempFileName = Path.Combine(tempFolder, FileUploadBarcode.FileName);
			if (File.Exists(tempFileName)) File.Delete(tempFileName);

			FileUploadBarcode.SaveAs(tempFileName);
			
			return tempFileName;
		}

		private void SaveAsImage(string filePath, string codeText, BaseEncodeType encodeType, BarCodeImageFormat imageFormat)
		{
			// https://products.aspose.com/barcode/net/
			// Instantiate object and set differnt barcode properties
			BarcodeGenerator generator = new BarcodeGenerator(encodeType, codeText);

			generator.Parameters.Barcode.XDimension.Millimeters = 1f;

			// Save the image to your system and set its image format to Jpeg
			generator.Save(filePath, imageFormat);
		}

		private void ReadFromFile(string filePath)
		{
			using (BarCodeReader reader = new BarCodeReader(filePath))
			{
				DisplayAttributes(reader);
			}
		}
		private void ReadFromBitmap(Bitmap bitmap)
		{
			using (BarCodeReader reader = new BarCodeReader(bitmap))
			{
				DisplayAttributes(reader);
			}
		}

		private void DisplayAttributes(BarCodeReader reader)
		{
			foreach (BarCodeResult result in reader.ReadBarCodes())
			{
				lblEncodeType.Text = result.CodeType.ToString();
				lblCodeText.Text = result.CodeText;
			}
		}
		#endregion

		#region " Create Barcodes "
		private Bitmap GenerateBitmap(string codeText, BaseEncodeType encodeType)
		{
			// https://products.aspose.com/barcode/net/
			// Instantiate object and set differnt barcode properties
			BarcodeGenerator generator = new BarcodeGenerator(encodeType, codeText);
			generator.Parameters.Barcode.XDimension.Millimeters = 1f;

			// Save the image to your system and set its image format to Jpeg
			return generator.GenerateBarCodeImage();
		}

		private void CreateBarcode(string codeText, BaseEncodeType encodeType)
		{
			try
			{
				Bitmap bitmap = GenerateBitmap(codeText, encodeType);
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
			}
			catch (Exception ex)
			{
				DisplayError(lblNewImageError, ex);
			}
		}

		protected void btnCreate_Click(object sender, EventArgs e)
		{
			CreateBarcode(txtCodeText.Text, BaseEncodeType.Parse(ddlEncodeType.SelectedItem?.Text));
		}
		#endregion
	}
}