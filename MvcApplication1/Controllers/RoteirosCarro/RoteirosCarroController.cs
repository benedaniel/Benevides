using FT.Web.Core.Util.SiteTracer;
using FT.Web.Site.Controllers.Base;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.RoteirosCarro
{
    [Tracer]
    public class RoteirosCarroController : _BaseController
    {
        public ActionResult Index()
        {
            if (!EstaLogado)
                return RedirectToAction("Index", "Autenticar", new { returnUrl = CurrentUrl });

            return View();
        }
    }
}
