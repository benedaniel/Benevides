﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Consolidadora
{
    public class ConsolidadoraController : Controller
    {
        //
        // GET: /SiteConsolidadora/
        private string _titulo = "{0}Hotsites/Consolidadora/{1}.htm";

        public ActionResult Index(string hotsite)
        {
            ////cria o cookie de visita da página
            HttpCookie cookiePagina = new HttpCookie("UltimaPagina");
            cookiePagina.Value = "Consolidadora";
            cookiePagina.Expires = DateTime.Now.AddYears(1);
            cookiePagina.Shareable = true;

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
