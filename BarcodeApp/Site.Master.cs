using System;
using System.Web.UI;

namespace BarcodeApp
{
    public partial class SiteMaster : MasterPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			hlAppName.Text = Properties.Settings.Default.AppName;
		}
	}
}