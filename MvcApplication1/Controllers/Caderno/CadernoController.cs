using FT.Web.Core.Util.SiteTracer;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Caderno
{
    [Tracer]
    public class CadernoController : Controller
    {
        //
        // GET: /Caderno/

        public ActionResult Index()
        {
            return View();
        }
    }
}
