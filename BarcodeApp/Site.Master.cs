using System;
using System.Web.UI;

namespace BarcodeApp
{
    public partial class SiteMaster : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			hlAppName.Text = Properties.Settings.Default.AppName;
			lblFooter.Text = $"&copy; {DateTime.Now.Year} - {Properties.Settings.Default.AppName}";
		}
	}
}