using System;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Base
{
    public abstract class Multibase : Controller
    {
        protected override void ExecuteCore()
        {
            string cultureName = null;
            HttpCookie cultureCookie = Request.Cookies["Language"];
            if (cultureCookie != null)
            {
                cultureName = cultureCookie.Value;
            }
            else
            {
                cultureName = "pt-BR";
            }
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            try
            {
                base.ExecuteCore();
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                throw ex;
            }
        }

        protected override bool DisableAsyncSupport
        {
            get { return true; }
        }
    }
}