using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Insurance_Website.Class_Files;

namespace Insurance_Website
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                if(Request.QueryString != null)
                {
                    Policy newPolicy = new Policy(Request.Form["firstName"], Request.Form["lastName"], Request.Form["country"],
                    Request.Form["numCars"], Request.Form["drivingRecord"].ToString());

                    Record.Create(newPolicy);
                }
            }
        }       
    }
}