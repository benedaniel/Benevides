using FT.Web.Business.Destino;
using FT.Web.Business.Geomob;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models;
using FT.Web.Site.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Destino
{
    [Tracer]
    public partial class DestinoController : _BaseController
    {
        DestinoBusiness destinoBussiness = new DestinoBusiness();

        FT.Web.Business.DestinoService.DestinoClient destinoClient = new FT.Web.Business.DestinoService.DestinoClient();
        FT.Web.Business.DestinoService.DestinoFiltro destinoFiltro = new FT.Web.Business.DestinoService.DestinoFiltro();

        FT.Web.Business.UsuarioService.UsuarioClient usuario = new FT.Web.Business.UsuarioService.UsuarioClient();

        public string Token
        {
            get { return Session["Token"] as string; }
            set { Session["Token"] = value; }
        }

        public ActionResult Index()
        {

            var login = FT.Web.Business.Gerenciamento.LoginSistema.LoginService();

            Token = login.Token;

            destinoFiltro = new FT.Web.Business.DestinoService.DestinoFiltro();
            IList<DestinoModel> destinoModel = new List<DestinoModel>();

            DestinoModel destino = new DestinoModel();

            DestinoTopico destinoTopico = new DestinoTopico();
            DestinoTopicoConteudo topicoConteudo = new DestinoTopicoConteudo();
            DestinoFoto destinoFoto = new DestinoFoto();

            destinoFiltro.Token = Token;
            destinoFiltro.CodigoCidade = "988";

            var listaDestino = destinoClient.Consultar(destinoFiltro);

            foreach (var destinoIn in listaDestino)
            {
                destino = new DestinoModel();
                destino.CodigoCidade = (long)destinoIn.CodigoCidade;
                destino.CodigoDestino = destinoIn.CodigoDestino;
                destino.CodigoEstado = (long)destinoIn.CodigoEstado;
                destino.CodigoPais = (long)destinoIn.CodigoPais;
                destino.NomeCidade = destinoIn.NomeCidade;

                foreach (var topico in destinoIn.Topico)
                {
                    destinoTopico = new DestinoTopico();
                    destinoTopico.CodigoDestino = topico.CodigoDestino;
                    destinoTopico.CodigoDestinoTopico = topico.CodigoDestinoTopico;
                    destinoTopico.NomeDestinoTopico = topico.NomeDestinoTopico;

                    foreach (var conteudo in topico.Conteudo)
                    {
                        topicoConteudo = new DestinoTopicoConteudo();
                        topicoConteudo.CodigoConteudo = conteudo.CodigoConteudo;
                        topicoConteudo.ConteudoDescricao = conteudo.ConteudoDescricao;
                        topicoConteudo.ConteudoTitulo = conteudo.ConteudoDescricao;
                        destinoTopico.Conteudo.Add(topicoConteudo);
                    }
                    destino.Topico.Add(destinoTopico);
                }

                foreach (var foto in destinoIn.Foto)
                {
                    destinoFoto = new DestinoFoto();
                    destinoFoto.CodigoDestinoFoto = foto.CodigoDestinoFoto;
                    destinoFoto.DestinoFotoLink = foto.DestinoFotoLink;
                    destinoFoto.DestinoFotoTitulo = foto.DestinoFotoTitulo;
                    destinoFoto.DestinoFotoUrl = foto.DestinoFotoUrl;
                    destino.Foto.Add(destinoFoto);
                }
                destinoModel.Add(destino);
            }

            return View(destinoModel);
        }

        public ActionResult DestinoNacional()
        {
            return View();
        }

        public ActionResult DestinoInternacional()
        {
            return View();
        }

        public ActionResult DestinoNacionalDetalhes()
        {
            try
            {
                string cityCode = Request.QueryString["cityCode"];
                var destino = destinoBussiness.ConsultarDestinos(cityCode);

                var firstOrDefault = destino.FirstOrDefault();
                try
                {
                    var geomob = new GeomobBusiness();
                    if (firstOrDefault != null)
                        ViewBag.GeomobUrl = geomob.GetUrl(firstOrDefault.NomeCidade);
                }
                catch (Exception ex)
                {
                    ViewBag.GeomobUrl = string.Empty;
                }

                return View(destino);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DestinoInternacionalDetalhes()
        {
            try
            {
                string cityCode = Request.QueryString["cityCode"];
                var destino = destinoBussiness.ConsultarDestinos(cityCode);
                var firstOrDefault = destino.FirstOrDefault();
                try
                {
                    var geomob = new GeomobBusiness();
                    if (firstOrDefault != null)
                        ViewBag.GeomobUrl = geomob.GetUrl(firstOrDefault.NomeCidade);
                }
                catch (Exception ex)
                {
                    ViewBag.GeomobUrl = string.Empty;
                }

                return View(destino);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}