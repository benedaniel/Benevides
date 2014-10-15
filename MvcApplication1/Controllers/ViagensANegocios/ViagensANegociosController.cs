using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.SiteViagensANegocios
{
    public class ViagensANegociosController : Controller
    {
        //
        // GET: /SiteViagensANegocios/
        private string _titulo = "{0}Hotsites/ViagensANegocios/{1}.htm";
        public ActionResult Index(string hotsite)
        {
            //cria o cookie de visita da página
            HttpCookie cookiePagina = new HttpCookie("UltimaPagina");
            cookiePagina.Value = "ViagensANegocios";
            cookiePagina.Expires = DateTime.Now.AddYears(1);

            if (!this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UltimaPagina"))
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookiePagina);
            else
                this.ControllerContext.HttpContext.Response.Cookies.Set(cookiePagina);

            if (!String.IsNullOrEmpty(hotsite))
            {
                string htmlPath = String.Format(this._titulo.Replace('/', '\\'), AppDomain.CurrentDomain.BaseDirectory, hotsite);

                if (System.IO.File.Exists(htmlPath))
                {
                    ViewBag.CaminhoHotSite = String.Format(this._titulo, '/', hotsite);
                }
                else { return RedirectToAction("Index"); }
            }
            else
                ViewBag.CaminhoHotSite = String.Format(this._titulo, '/', "index");

            return View();
        }
    }
}
