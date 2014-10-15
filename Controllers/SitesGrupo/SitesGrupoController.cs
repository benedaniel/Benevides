using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using FT.Web.Business.Destaque;
using FT.Web.Business.FaleConosco;
using FT.Web.Site.Helpers.FaleConosco;

namespace FT.Web.Site.Controllers.SitesGrupo
{
    public class SitesGrupoController : Controller
    {
        //
        // GET: /SitesGrupo/
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
        DestaqueBusiness destaqueBusiness = new DestaqueBusiness();

        public ActionResult Index()
        {
            return View();
        }

        public List<DestaqueModel> BannerDestaque(int id_empresa, int id_destaque_tipo)
        {
            //var DestaqueFlytour = AppCache.Get<List<DestaqueModel>>(EnumProcesso.PacoteMatriz, "BannerDestaque" + id_empresa + id_destaque_tipo, 60, () =>
            //{
            //    return new List<DestaqueModel>();
            //});

            //if (DestaqueFlytour.Count > 0)
            //{
            //    return DestaqueFlytour;
            //}
            //else
            //{
            var pacotes = destaqueBusiness.LoadDestaques(20, "", id_empresa, id_destaque_tipo);

            List<DestaqueModel> result = new List<DestaqueModel>();
            if (id_empresa == 1)
            {
                var grandes = pacotes.Where(p => p.DestaqueFotos.Any(b => b.AlturaFoto == 300 && b.LarguraFoto == 480)).OrderBy(r => Guid.NewGuid()).Take(5);
                var pequenos = pacotes.Where(p => p.DestaqueFotos.Any(b => b.AlturaFoto == 160 && b.LarguraFoto == 235)).OrderBy(r => Guid.NewGuid()).Take(2);
                result.AddRange(grandes);
                result.AddRange(pequenos);
            }
            else
            {
                result = pacotes;
            }

            var success = AppCache.Put(EnumProcesso.PacoteMatriz, "BannerDestaque" + id_empresa + id_destaque_tipo, result, TimeSpan.FromMinutes(60));
            return result;
            //}
        }

        public ActionResult BannerDestaqueConsolidadora()
        {
            List<DestaqueModel> result = BannerDestaque(3, 1);
            return View(result);
        }

        public ActionResult BannerDestaqueViagensaNegocio()
        {
            List<DestaqueModel> result = BannerDestaque(2, 1);
            return View(result);
        }

        public ActionResult BannerDestaqueFranchising()
        {
            List<DestaqueModel> result = BannerDestaque(8, 1);
            return View(result);
        }

        public ActionResult BannerDestaqueEventos()
        {
            List<DestaqueModel> result = BannerDestaque(5, 1);
            return View(result);
        }

        public ActionResult NoticiasEventos()
        {
            List<DestaqueModel> result = BannerDestaque(5, 2);
            return View(result);
        }

        public ActionResult NoticiasViagensaNegocio()
        {
            List<DestaqueModel> result = BannerDestaque(2, 2);
            return View(result);
        }

        public ActionResult NoticiasFranchising()
        {
            List<DestaqueModel> result = BannerDestaque(8, 2);
            return View(result);
        }

        public ActionResult NoticiasConsolidadora()
        {
            List<DestaqueModel> result = BannerDestaque(3, 2);

            return View(result);
        }

        public ActionResult CampanhaEventos()
        {
            List<DestaqueModel> result = BannerDestaque(5, 3);
            return View(result);
        }

        public ActionResult CampanhaViagensaNegocio()
        {
            List<DestaqueModel> result = BannerDestaque(2, 3);
            return View(result);
        }

        public ActionResult CampanhaFranchising()
        {
            List<DestaqueModel> result = BannerDestaque(8, 3);
            return View(result);
        }

        public ActionResult CampanhaConsolidadora()
        {
            List<DestaqueModel> result = BannerDestaque(3, 3);
            return View(result);
        }

        public ActionResult FaleConoscoFranchising()
        {
            FaleConoscoModel model = new FaleConoscoModel();
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
            }

            this.CarregaViewBagEstadoECidade(codEstado, model != null ? model.Cidade : null);

            return View("FaleConoscoFranchising", model);
        }


        public ActionResult EnvioEmail(FaleConoscoModel model)
        {
            FaleConoscoDAO fcDAO = new FaleConoscoDAO();
            if (String.IsNullOrEmpty(model.Nome))
            {
                CarregaViewBagEstadoECidade();
                return View("FaleConoscoFranchising");
            }

            string email = String.Empty;
            string mensagem = model.Mensagem;

            ViewBag.Title = "Fale conosco - Flytour Franchising";
            model.IdEmpresaDestinataria = 8;
            model.Mensagem = (this.RenderViewToString("EmailDefault", model) ?? String.Empty);

            if (model.Mensagem.Length > 4000)
                model.Mensagem = model.Mensagem.Substring(0, 4000);

            model.Assunto = "Fale conosco - Flytour Franchising";
            model.Sexo = 'M';

            bool enviou = fcDAO.EnviarEmailFaleConoscoEInserirDadosNaBase(Shared.SharedController.ToUtilService_FaleConoscoModel(model));

            if (!enviou)
            {
                ViewBag.Mensagem = "Ocorreu algum erro ao tentar enviar o email, por favor tente novamente mais tarde.";
                ModelState.AddModelError("naoEnviou", "Ocorreu algum erro ao tentar enviar o email, por favor tente novamente mais tarde.");
                model.Mensagem = mensagem;
            }
            else
            {
                ViewBag.Mensagem = "E-mail enviado com sucesso.";
            }

            return View("FaleConoscoFranchising");
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
                    return RedirectToRoute("FaleConoscoFranchising", new { siteName = "franchising", action = "enviado" });
                }
            }

            this.CarregaViewBagEstadoECidade(codEstado, model != null ? model.Cidade : null);
            return View();
        }
    }
}