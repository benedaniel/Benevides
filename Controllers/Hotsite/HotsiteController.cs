using FT.Web.Core.Util.SiteTracer;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using FT.Web.Site.Controllers.Base;
using FTV.Shared.Business;
using FT.Web.Model.Models;

namespace FT.Web.Site.Controllers
{
    [Tracer]
    public class HotsiteController : _BaseController
    {
        //
        // GET: /Hotsite/

        public ActionResult Index(string titulo)
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            CleanItensComprar(addedFlytour);

            return View();
        }

        public ActionResult Details(string titulo)
        {
            if (!String.IsNullOrEmpty(titulo))
            {
                string htmlCode = AppDomain.CurrentDomain.BaseDirectory + @"Hotsites\" + titulo + ".htm";

                if (System.IO.File.Exists(htmlCode))
                {
                    ViewBag.Teste = titulo + ".htm";
                }
                else { return RedirectToAction("Index", "Home"); }
            }
            else { return RedirectToAction("Index", "Home"); }

            return View("Index");
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
