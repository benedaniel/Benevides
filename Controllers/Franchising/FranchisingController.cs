using FT.Web.Business.FaleConosco;
using FT.Web.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FT.Web.Site.Helpers.FaleConosco;

namespace FT.Web.Site.Controllers.Franchising
{
    public class FranchisingController : Controller
    {
        //
      
        // GET: /FaleConosco/
        private void CarregaViewBagEstadoECidade(int idEstado = 0, string nomeCidade = null)
        {
            Business.Shared.SharedDAO shared = new Business.Shared.SharedDAO();
            List<Business.UtilService.EstadoDados> estados = shared.GetEstadosDoBrasil();
            List<Business.UtilService.CidadeDados> cidades = idEstado > 0 ? shared.GetCidades(idEstado) : null;

            cidades = cidades ?? new List<Business.UtilService.CidadeDados>();

            string idEstadoStr = idEstado.ToString();
            estados = estados ?? new List<Business.UtilService.EstadoDados>();

            List<SelectListItem> sLIEstados = estados.Select(a => new SelectListItem()
            {
                Text = a.CD_ESTADO,
                Value = String.Format("{0}|{1}", a.ID_ESTADO, a.CD_ESTADO),
                Selected = idEstado > 0 && idEstadoStr.Equals(a.CD_ESTADO)
            }).ToList();

            sLIEstados.Insert(0, new SelectListItem()
            {
                Value = String.Empty,
                Text = "Sel.."
            });

            List<SelectListItem> sLICidades = cidades.Select(a => new SelectListItem()
            {
                Text = a.NM_CIDADE,
                Value = a.NM_CIDADE,
                Selected = !String.IsNullOrEmpty(nomeCidade) && nomeCidade.Equals(a.NM_CIDADE)
            }).ToList();

            sLICidades.Insert(0, new SelectListItem()
            {
                Value = String.Empty,
                Text = "Selecione..."
            });

            ViewBag.OpcoesParaEstados = sLIEstados;
            ViewBag.OpcoesParaCidades = sLICidades;
        }
        private string _titulo = "{0}Hotsites/Franchising/{1}.htm";
        public ActionResult Index( string hotsite)
        {

            //Cria o cookie de visita da página
            HttpCookie cookiePagina = new HttpCookie("UltimaPagina");
            cookiePagina.Value = "Franchising";
            cookiePagina.Expires = DateTime.Now.AddYears(1);

            if (!this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("UltimaPagina"))
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookiePagina);
            else
                this.ControllerContext.HttpContext.Response.Cookies.Set(cookiePagina);

            if (!String.IsNullOrEmpty(hotsite))
            {
                string htmlPath = String.Format(this._titulo.Replace('/', '\\'), AppDomain.CurrentDomain.BaseDirectory, hotsite);

                if (System.IO.File.Exists(htmlPath))
                {
                    ViewBag.CaminhoHotSite = String.Format(this._titulo, '/', hotsite);
                }
                else { return RedirectToAction("Index"); }
            }
            else
                ViewBag.CaminhoHotSite = String.Format(this._titulo, '/', "index");

            return View();
        }

        [HttpPost]
        public ActionResult Index(FaleConoscoModel model, string hotsite)
        {
            string[] codEstadoStrSplit = model != null && model.Estado != null ? model.Estado.Split('|') : new string[]{};
            int codEstado = 0;

            if(codEstadoStrSplit.Length > 0)
            {
                Int32.TryParse(codEstadoStrSplit[0], out codEstado);

                if(codEstadoStrSplit.Length > 1)
                    model.Estado = codEstadoStrSplit[1];
            }

            if (ModelState.IsValid && model != null)
            {
                FaleConoscoDAO fcDAO = new FaleConoscoDAO();
                string email = String.Empty;
                string mensagem = model.Mensagem;

                ViewBag.Title = "Fale conosco - Flytour Franchising";
                model.IdEmpresaDestinataria = 8;
                model.Mensagem = (this.RenderViewToString("EmailDefault", model) ?? String.Empty);

                if (model.Mensagem.Length > 4000)
                    model.Mensagem = model.Mensagem.Substring(0, 4000);

                bool enviou = fcDAO.EnviarEmailFaleConoscoEInserirDadosNaBase(Shared.SharedController.ToUtilService_FaleConoscoModel(model));

                if (!enviou)
                {
                    ViewBag.MensagemDeErro = "Ocorreu algum erro ao tentar enviar o email, por favor tente novamente mais tarde.";
                    ModelState.AddModelError("naoEnviou", "Ocorreu algum erro ao tentar enviar o email, por favor tente novamente mais tarde.");
                    model.Mensagem = mensagem;
                }
                else
                {
                    return RedirectToRoute("FaleConoscoPage", new { siteName = "franchising" , action = "enviado" });
                }
            }

            this.CarregaViewBagEstadoECidade(codEstado, model != null ? model.Cidade : null);

            return View("IndexComFaleConosco", model);
        }

        [HttpPost]
        public JsonResult GetCidades(int id)
        {
            Business.Shared.SharedDAO shared = new Business.Shared.SharedDAO();
            List<Business.UtilService.CidadeDados> cidades = shared.GetCidades(id);

            return Json(cidades);
        }


        public ActionResult testeForm(FaleConoscoModel model, string hotsite = "franchising")
        {
            string[] codEstadoStrSplit = model != null && model.Estado != null ? model.Estado.Split('|') : new string[] { };
            int codEstado = 0;

            if (codEstadoStrSplit.Length > 0)
            {
                Int32.TryParse(codEstadoStrSplit[0], out codEstado);

                if (codEstadoStrSplit.Length > 1)
                    model.Estado = codEstadoStrSplit[1];
            }

            if (ModelState.IsValid && model != null)
            {
                FaleConoscoDAO fcDAO = new FaleConoscoDAO();
                string email = String.Empty;
                string mensagem = model.Mensagem;

                ViewBag.Title = "Fale conosco - Flytour Franchising";
                model.IdEmpresaDestinataria = 8;
                model.Mensagem = (this.RenderViewToString("EmailDefault", model) ?? String.Empty);

                if (model.Mensagem.Length > 4000)
                    model.Mensagem = model.Mensagem.Substring(0, 4000);

                bool enviou = fcDAO.EnviarEmailFaleConoscoEInserirDadosNaBase(Shared.SharedController.ToUtilService_FaleConoscoModel(model));

                if (!enviou)
                {
                    ViewBag.MensagemDeErro = "Ocorreu algum erro ao tentar enviar o email, por favor tente novamente mais tarde.";
                    ModelState.AddModelError("naoEnviou", "Ocorreu algum erro ao tentar enviar o email, por favor tente novamente mais tarde.");
                    model.Mensagem = mensagem;
                }
                else
                {
                    return RedirectToRoute("FaleConoscoPage", new { siteName = "franchising", action = "enviado" });
                }
            }

            this.CarregaViewBagEstadoECidade(codEstado, model != null ? model.Cidade : null);
            return View();
        }


        public ActionResult IndexComFaleConosco()
        {
            return View();
        }

    }
}
