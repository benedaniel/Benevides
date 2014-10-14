using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using FT.Web.Model.Models;
using FT.Web.Site.Controllers.Base;
using FT.Web.Business.Home;
using System.Configuration;
using FT.Web.Core.Util.SiteTracer;
using System;
using System.Linq;
using FTV.Shared.Business;
using FT.Web.Business.Aereo;

namespace FT.Web.Site.Controllers.Home
{
    [Tracer]
    public class HomeController : _BaseController
    {
        //
        // GET: /Home/

        public virtual ActionResult Index()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            CleanItensComprar(addedFlytour);

            var httpCookieSessionId = ControllerContext.HttpContext.Request.Cookies["ASP.NET_SessionId"];

            if (Request.UrlReferrer == null || httpCookieSessionId == null)
            {
                var httpCookie = ControllerContext.HttpContext.Request.Cookies["UltimaPagina"];

                //verifica se o cookie de ulima pagina visitada que está armazenado no cliente
                //está utilizando o padrão antigo, utilizando o prefixo site
                if(httpCookie != null && httpCookie.Value.ToLower().Contains("site"))
                {
                    //retira o valor da controller que já estava no cliente
                    //e adapta para o novo padrão que não contém a palavra
                    //site no início
                    string valorAntigo = httpCookie.Value.Substring(4);

                    //atribui o novo valor no cookie
                    httpCookie.Value = valorAntigo;
                }

                if (httpCookie != null && httpCookie.Value.ToLower() != "home")
                    return RedirectToAction("Index",
                                            httpCookie.Value);
            }
            else
            {
                if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UltimaPagina"))
                {
                    //cria o cookie de visita da página
                    var cookiePagina = new HttpCookie("UltimaPagina") { Value = "Home", Expires = DateTime.Now.AddYears(1) };

                    ControllerContext.HttpContext.Response.Cookies.Set(cookiePagina);
                }
            }

            return View();
        }

        public JsonResult AlterarLoading(string produto, string tipo)
        {
            string path = ConfigurationManager.AppSettings["loading" + produto + tipo];
            if (!string.IsNullOrEmpty(path))
                if (Request.Url != null) path = "http://" + Request.Url.Authority + "/Images/" + path;
            return Json(path);
        }

        public virtual PartialViewResult Quartos(int? indice)
        {
            ViewBag.Indice = indice ?? 1;

            return PartialView();
        }

        public virtual PartialViewResult Trechos(int? indice)
        {
            ViewBag.IndiceTrecho = indice ?? 1;

            return PartialView();
        }

        public virtual PartialViewResult TrechosFiltroLateral(int? indice)
        {
            ViewBag.IndiceTrecho = indice ?? 1;

            var destinos = Session["DestinosFiltroLateralAereo"] as List<FT.Web.Model.Models.Destino>;

            var destino = destinos[indice.Value - 1];

            destino.NomeDestino = HttpUtility.HtmlDecode(destino.NomeDestino.Replace(",", ""));
            destino.NomeOrigem = HttpUtility.HtmlDecode(destino.NomeOrigem.Replace(",", ""));

            List<LocaisRetorno> lstCidadesOrigem;
            List<LocaisRetorno> lstCidadesDestino;
            

            var listaAeroportosDestino = AereoBusiness.ConsultaDestino();

            lstCidadesOrigem = listaAeroportosDestino.Where(p => p.NomeAeroporto.Contains(destino.NomeOrigem)).ToList();
            lstCidadesDestino = listaAeroportosDestino.Where(p => p.NomeAeroporto.Contains(destino.NomeDestino)).ToList();

            ViewBag.aeroportoDestino = lstCidadesDestino.First().NomeAeroporto;
            ViewBag.aeroportoOrigem = lstCidadesOrigem.First().NomeAeroporto;
            ViewBag.codigoCidadeOrigem = lstCidadesOrigem.First().SiglaAeroporto;
            ViewBag.codigoCidadeDestino = lstCidadesDestino.First().SiglaAeroporto;
            ViewBag.dataFinal = destino.DataFinal;
            ViewBag.dataInicial = destino.DataInicial;

            return PartialView("Trechos");
        }

        public virtual PartialViewResult TrechosRemover()
        {
            if (ViewBag.IndiceTrecho != null)
                ViewBag.IndiceTrecho = ViewBag.IndiceTrecho - 1;

            return null;
        }

        public virtual PartialViewResult Criancas()
        {
            return PartialView();
        }

        [HttpPost]
        public bool SendEmail(string nome, string email)
        {
            var homeBussiness = new HomeBusiness();
            return homeBussiness.SendNews(nome, email);
        }

        [HttpPost]
        public string GetCotacao()
        {
            var homeBussiness = new HomeBusiness();
            return homeBussiness.GetCotacao(SessionKey);
        }

        public virtual PartialViewResult NewsletterForm()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult FirstViewer()
        {
            if (ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("FirstViewer"))
            {
                return Json(new { returnvalue = false });
            }
            var cookiePagina = new HttpCookie("FirstViewer") { Value = "JaVi", Expires = DateTime.Now.AddYears(1) };

            ControllerContext.HttpContext.Response.Cookies.Add(cookiePagina);
            return Json(new { returnvalue = true });
        }
    }
}
