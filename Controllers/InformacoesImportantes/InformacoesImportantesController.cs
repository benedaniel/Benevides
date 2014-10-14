using FT.Web.Core.Util.SiteTracer;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers
{
    [Tracer]
    public class InformacoesImportantesController : Controller
    {
        //
        // GET: /InformacoesImportantes/

        public ActionResult Index()
        {
            return View();
        }
    }
}
