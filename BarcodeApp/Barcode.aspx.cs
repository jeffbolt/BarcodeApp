using Aspose.BarCode.BarCodeRecognition;
using Aspose.BarCode.Generation;

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace BarcodeApp
{
    public partial class Barcode : Page
    {
        public string BaseDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
            }
        }

        public string BarcodeDirectory
        {
            get
            {
                return Path.Combine(Directory.GetParent(BaseDirectory).Parent.FullName, "Barcodes");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var dataSource = EncodeTypes.AllEncodeTypes.ToList().OrderBy(x => x.TypeName);
                ddlEncodeType.DataSource = dataSource;
                ddlEncodeType.DataTextField = "TypeName";
                ddlEncodeType.DataValueField = "TypeIndex";
                ddlEncodeType.DataBind();
                ddlEncodeType.SelectedIndex = EncodeTypes.Code128.TypeIndex;

                txtValue.MaxLength = 128;  // TODO: determine max len
                txtValue.Text = Guid.NewGuid().ToString();
                //string dataDir = Path.Combine(Directory.GetParent(BaseDirectory).Parent.FullName, "Barcodes");
                //if (!Directory.Exists(BarcodeDirectory)) Directory.CreateDirectory(BarcodeDirectory);
                //string codeText = "1234567";
                //string filePath = Path.Combine(BarcodeDirectory, "Output.jpg");
                //if (File.Exists(filePath)) File.Delete(filePath);
                //SaveAsImage(filePath, codeText, EncodeTypes.Code128, BarCodeImageFormat.Jpeg);

                //string demoFile = Path.Combine(BarcodeDirectory, "GetStarted.png"); //@"d:\template.jpg";
                //ReadFromImage(demoFile);
            }
        }

        private Bitmap GenerateBitmap(string codeText, BaseEncodeType encodeType)
        {
            // https://products.aspose.com/barcode/net/
            // Instantiate object and set differnt barcode properties
            BarcodeGenerator generator = new BarcodeGenerator(encodeType, codeText);
            generator.Parameters.Barcode.XDimension.Millimeters = 1f;
            
            // Save the image to your system and set its image format to Jpeg
            return generator.GenerateBarCodeImage();
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

        private void ReadFromImage(string filePath)
        {
            using (BarCodeReader reader = new BarCodeReader(filePath))
            {
                foreach (BarCodeResult result in reader.ReadBarCodes())
                {
                    Console.WriteLine("Type: " + result.CodeType);
                    Console.WriteLine("CodeText: " + result.CodeText);
                }
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            Bitmap bmp = GenerateBitmap(txtValue.Text, BaseEncodeType.Parse(ddlEncodeType.SelectedItem?.Text));
            if (bmp != null)
            {
                pnlResult.Visible = true;

                using (var ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    string base64 = Convert.ToBase64String(ms.GetBuffer());
                    string dataUri = $"data:image/png;base64,{base64}";

                    //imgResult.Height = bmp.Height;
                    //imgResult.Width = bmp.Width;
                    imgResult.ImageUrl = dataUri;
                }
            }
        }
    }
}