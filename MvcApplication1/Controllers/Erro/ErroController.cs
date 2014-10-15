using FT.Web.Core.Util.SiteTracer;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Erro
{
    [Tracer]
    public class ErroController : Controller
    {
        //
        // GET: /Erro/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Erro404()
        {
            return View();
        }

        public ActionResult Erro500()
        {
            return View();
        }
    }
}
