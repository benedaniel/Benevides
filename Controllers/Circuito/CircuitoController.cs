using FT.Web.Core.Util.SiteTracer;
using FT.Web.Site.Controllers.Base;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Circuito
{
    [Tracer]
    public class CircuitoController : _BaseController
    {
        //
        // GET: /Circuito/

        public ActionResult Index()
        {
            if (!EstaLogado)
                return RedirectToAction("Index", "Autenticar", new { returnUrl = CurrentUrl });

            return View();
        }

    }
}
