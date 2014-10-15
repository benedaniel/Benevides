using FT.Web.Business.Aereo;
using FT.Web.Business.Carro;
using FT.Web.Business.CarroService;
using FT.Web.Business.Destaque;
using FT.Web.Business.Hotel;
using FT.Web.Business.Localizacao;
using FT.Web.Business.Pacote;
using FT.Web.Core.Util.Common;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using System.Linq;


namespace FT.Web.Site
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        HotelBusiness hotelBussiness = new HotelBusiness();
        PacoteBusiness pacoteBussiness = new PacoteBusiness();
        DestaqueBusiness destaqueBusiness = new DestaqueBusiness();
        Common common = new Common();

        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteTable.Routes.MapRoute(
                "Hotsite",
                "Hotsite/{titulo}",
                new { controller = "Hotsite", action = "Details" }
            );

            RouteTable.Routes.MapRoute(
                "Hotel",
                "Hotel/{codigocidade}/{checkinGlobal}/{checkoutGlobal}",
                new { controller = "Hotel", action = "BuscarHotel" }
            );

            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

#if !DEBUG
            try
            {
                CacheHotelDestaque();
                CacheHotelDestaqueHotsite();
                CacheHotelDestaqueInferior();
                CacheHotelDestaqueInternacional();
                CacheHotelDestaqueInternacionalHotsite();
                CachePacoteDestaque("9668");
                CachePacoteDestaquesNacionais("9668");
                CachePacoteDestaquesInternacionais("9668");
                CachePacoteDestaquesNacionaisIframe("9668");
                CachePacoteDestaquesInternacionaisIframe("9668");
                CachePacoteDestaquesInferior("9668");
                CacheAutoCompleteAereoOrigem();
                CacheAutoCompleteAereoOrigemNacional();
                CacheAutoCompleteAereoDestino();
                CacheAutoCompleteHotelDestino();
                CacheAutoCompleteCarro();
            }
            catch (Exception ex)
            {
                var x = ex;
                //throw;
            }
#endif
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            if (Context.Handler is IRequiresSessionState || Context.Handler is IReadOnlySessionState)
            {
                HttpContext context = HttpContext.Current;
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                //These headers are handling the "pre-flight" OPTIONS call sent by the browser
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            if (!Server.GetLastError().Message.ToLower().Contains(".jpg") 
                && !Server.GetLastError().Message.ToLower().Contains(".png")
                && !Server.GetLastError().Message.ToLower().Contains("the controller for path"))
            {
                Tracer.ArmazenarTracer(HttpContext.Current, Server.GetLastError().Message, Server.GetLastError().StackTrace);
            }

            Server.ClearError();

            Response.Redirect("~/Erro/Erro500");
        }

        private void Session_Start()
        {
            //if (HttpContext.Current.Request.Cookies.AllKeys.Contains("ASP.NET_SessionId"))
            //{
            //    var cookieSessionId = HttpContext.Current.Request.Cookies["ASP.NET_SessionId"];
            //    cookieSessionId.Value = "";

            //    HttpContext.Current.Request.Cookies.Set(cookieSessionId);
            //    HttpContext.Current.Response.Cookies.Set(cookieSessionId);
            //}
        }

        private void CacheHotelDestaque()
        {

            var hoteisDestaque = hotelBussiness.BuscarHotelDestaque();
            if (hoteisDestaque.Count > 1)
            {
                hoteisDestaque = common.AddSevenDays(hoteisDestaque);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaque", hoteisDestaque, TimeSpan.FromMinutes(30));
            }
        }

        private void CacheHotelDestaqueHotsite()
        {
            var hoteisDestaque = hotelBussiness.BuscarHotelDestaque();
            if (hoteisDestaque.Count > 1)
            {
                hoteisDestaque = common.AddSevenDays(hoteisDestaque);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueHotsite", hoteisDestaque, TimeSpan.FromMinutes(30));
            }

        }

        private void CacheHotelDestaqueInternacional()
        {
            var hoteisDestaque = hotelBussiness.BuscarHotelDestaqueInternacional();
            if (hoteisDestaque.Count > 1)
            {
                hoteisDestaque = common.AddSevenDays(hoteisDestaque);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueInternacional", hoteisDestaque, TimeSpan.FromMinutes(30));
            }
        }
        
        private void CacheHotelDestaqueInternacionalHotsite()
        {
            var hoteisDestaque = hotelBussiness.BuscarHotelDestaqueInternacional();
            if (hoteisDestaque.Count > 1)
            {
                hoteisDestaque = common.AddSevenDays(hoteisDestaque);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueInternacionalHotsite", hoteisDestaque, TimeSpan.FromMinutes(30));
            }
        }

        private void CacheHotelDestaqueInferior()
        {
            var hoteisDestaque = hotelBussiness.BuscarHotelDestaqueInferior();
            if (hoteisDestaque.Count > 1)
            {
                hoteisDestaque = common.AddSevenDays(hoteisDestaque);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueInferior", hoteisDestaque, TimeSpan.FromMinutes(30));
            }
        }

        private void CachePacoteDestaque(string origem)
        {
            var pacotes = destaqueBusiness.LoadDestaques(20, origem, 1, 1);
            if (pacotes.Count > 1)
            {
                var grandes = pacotes.Where(p => p.DestaqueFotos.Any(b => b.AlturaFoto == 300 && b.LarguraFoto == 480)).OrderBy(r => Guid.NewGuid()).Take(5);
                var pequenos = pacotes.Where(p => p.DestaqueFotos.Any(b => b.AlturaFoto == 160 && b.LarguraFoto == 235)).OrderBy(r => Guid.NewGuid()).Take(2);
                List<DestaqueModel> result = new List<DestaqueModel>();
                result.AddRange(grandes);
                result.AddRange(pequenos);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaque" + origem, result, TimeSpan.FromMinutes(2));
            }
        }

        private void CachePacoteDestaquesNacionais(string origem)
        {
            var pacotes = pacoteBussiness.BuscarDestaques(12, 2, origem);
            if (pacotes.Count > 1)
            {
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesNacionais" + origem, pacotes, TimeSpan.FromMinutes(2));
            }
        }

        private void CachePacoteDestaquesInternacionais(string origem)
        {
            var pacotes = pacoteBussiness.BuscarDestaques(12, 3, origem);
            if (pacotes.Count > 1)
            {
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesInternacionais" + origem, pacotes, TimeSpan.FromMinutes(2));
            }

        }

        private void CachePacoteDestaquesNacionaisIframe(string origem)
        {
            var pacotes = pacoteBussiness.BuscarDestaques(12, 2, origem);
            if (pacotes.Count > 1)
            {
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesNacionaisIframe" + origem, pacotes, TimeSpan.FromMinutes(2));

            }
        }

        private void CachePacoteDestaquesInternacionaisIframe(string origem)
        {
            var pacotes = pacoteBussiness.BuscarDestaques(12, 3, origem);
            if (pacotes.Count > 1)
            {
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesInternacionaisIframe" + origem, pacotes, TimeSpan.FromMinutes(2));
            }
        }

        private void CachePacoteDestaquesInferior(string origem)
        {
            var pacotes = pacoteBussiness.BuscarDestaques(7, 4, origem);
            if (pacotes.Count > 1)
            {
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesInferior" + origem, pacotes, TimeSpan.FromMinutes(2));
            }
        }

        private void CacheAutoCompleteAereoOrigem()
        {
            AereoBusiness.ConsultaOrigem();
        }

        private void CacheAutoCompleteAereoOrigemNacional()
        {
            AereoBusiness.ConsultaOrigem(true);
        }

        private void CacheAutoCompleteAereoDestino()
        {
            AereoBusiness.ConsultaDestino();
        }

        private void CacheAutoCompleteHotelDestino()
        {
            HotelBusiness hotelBusiness = new HotelBusiness();
            hotelBusiness.ConsultarLocalizacaoHotel();
        }

        private void CacheAutoCompleteCarro()
        {
            CarroLocalizacaoFiltro carroLocalizacaoFiltro = new CarroLocalizacaoFiltro();
            CarroBusiness carroBusiness = new CarroBusiness();
            carroBusiness.ConsultarLocalidades(carroLocalizacaoFiltro);
        }
    }
}