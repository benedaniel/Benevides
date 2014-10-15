using FT.Web.Business.FaleConosco;
using FT.Web.Model.Models;
using FT.Web.Site.Helpers.FaleConosco;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.FaleConosco
{
    public class FaleConoscoController : Controller
    {
        private bool _redirecionaPraHome;
        // GET: /FaleConosco/
        private void CarregaViewBag(string siteName)
        {
            this._redirecionaPraHome = false;

            switch (siteName)
            {
                case "viagens-a-negocios":
                    ViewBag.Layout = "ViagensANegocios";
                    ViewBag.Title = "Viagens a negócios";
                    ViewBag.SiteCSS = "negocios";
                    ViewBag.IDEmpresa = 2;
                    break;
                case "consolidadora":
                    ViewBag.Layout = "Consolidadora";
                    ViewBag.Title = "Consolidadora";
                    ViewBag.SiteCSS = "consolidadora";
                    ViewBag.IDEmpresa = 3;
                    break;
                case "eventos-e-incentivo":
                    ViewBag.Layout = "EventosEIncentivo";
                    ViewBag.Title = "Eventos e incentivo";
                    ViewBag.SiteCSS = "eventos";
                    ViewBag.IDEmpresa = 5;
                    break;
                case "franchising":
                    ViewBag.Layout = "Franchising";
                    ViewBag.Title = "Franchising";
                    ViewBag.SiteCSS = "franchising";
                    ViewBag.IDEmpresa = 8;
                    break;
                default:
                    this._redirecionaPraHome = true;
                    return;
            }

            ViewBag.Title = String.Format("Fale conosco - Flytour {0}", ViewBag.Title);

            string[] opcoesAssuntoString = { "Dúvidas", "Sugestões", "Elogios", "Reclamações" },
                     opcoesSexoString = { "Masculino", "Feminino" }, opcoesSexoStringValor = { "M", "F" };

            List<SelectListItem> opcoesFaleConosco = Shared.SharedController.OpcoesSelectList(opcoesAssuntoString),
                                 opcoesSexo = Shared.SharedController.OpcoesSelectList(opcoesSexoString, opcoesSexoStringValor);

            ViewBag.OpcoesParaAssunto = opcoesFaleConosco;
            ViewBag.OpcoesParaSexo = opcoesSexo;
            ViewBag.SiteName = siteName;
        }

        public ActionResult Index(string siteName)
        {
            this.CarregaViewBag(siteName);

            if (this._redirecionaPraHome)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }

        [HttpPost]
        public ActionResult Index(FaleConoscoModel model, string siteName)
        {
            this.CarregaViewBag(siteName);

            if (this._redirecionaPraHome)
                return RedirectToAction("Index", "Home");
            else
            {
                if (ModelState.IsValid && model != null)
                {
                    FaleConoscoDAO fcDAO = new FaleConoscoDAO();
                    string email = String.Empty;
                    string mensagem = model.Mensagem;

                    if (model != null && model.IdEmpresaDestinataria <= 0)
                        model.IdEmpresaDestinataria = ViewBag.IDEmpresa;

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
                        return RedirectToRoute("FaleConoscoPage", new { siteName = siteName, action = "enviado" });
                    }
                }

                return View(model);
            }
        }

        public ActionResult Enviado(string siteName)
        {
            this.CarregaViewBag(siteName);

            if (this._redirecionaPraHome)
                return RedirectToAction("Index", "Home");
            else
                return View();
        }
    }
}
