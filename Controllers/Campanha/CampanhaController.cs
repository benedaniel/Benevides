using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Campanha
{
    public class CampanhaController : Controller
    {
        //
        // GET: /Campanha/

        public ActionResult Index(string hotsite)
        {
            ViewBag.Caminho = "http://campanha.flytour.com.br" + Request.RawUrl;
            return View();
        }

    }
}
