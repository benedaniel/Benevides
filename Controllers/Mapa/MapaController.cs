using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FTV.Shared.Business;

namespace FT.Web.Site.Controllers
{
    public class MapaController : Controller
    {
        private int _iDEmpresa;
        private List<Model.Models.AgenciaMapaModel> _agenciasMapas;

        public List<Model.Models.AgenciaMapaModel> AgenciasMapa
        {
            get
            {
                if (this._agenciasMapas == null || this._agenciasMapas.Count == 0)
                {
                    int tempoEmMinutos = 1;/*10080/*1 semana = 7 dias*/
                    string agenciasKey = String.Format("TodasAsAgenciasPorIDEmpresa_{0}_", this._iDEmpresa);

                    this._agenciasMapas = AppCache.Get<List<Model.Models.AgenciaMapaModel>>(EnumProcesso.Global, agenciasKey, tempoEmMinutos, () =>
                    {
                        Business.Shared.SharedDAO sharedDAO = new Business.Shared.SharedDAO();
                        List<Model.Models.AgenciaMapaModel> agencias = sharedDAO.GetAgenciasPorEmpresa(this._iDEmpresa, String.Empty);

                        if (agencias != null && agencias.Count > 0)
                            agencias = agencias.OrderBy(ag => ag.Estado).ThenBy(ag => ag.Cidade).ToList();

                        agencias.RemoveAll(item => item.Cidade == null);

                        return agencias;
                    });
                }

                if (this._agenciasMapas == null)
                    this._agenciasMapas = new List<Model.Models.AgenciaMapaModel>();


                _agenciasMapas.RemoveAll(item => item.Cidade == null);
                return this._agenciasMapas;
            }
        }

        private void CarregaViewBag(string siteName)
        {
            switch (siteName)
            {
                case "viagens-a-negocios":
                    ViewBag.Layout = "ViagensANegocios";
                    ViewBag.Title = "Viagens a negócios";
                    ViewBag.SiteCSS = "negocios";
                    ViewBag.IDEmpresa = this._iDEmpresa = 2;
                    break;
                case "consolidadora":
                    ViewBag.Layout = "Consolidadora";
                    ViewBag.Title = "Consolidadora";
                    ViewBag.SiteCSS = "consolidadora";
                    ViewBag.IDEmpresa = this._iDEmpresa = 3;
                    break;
                case "eventos-e-incentivo":
                    ViewBag.Layout = "EventosEIncentivo";
                    ViewBag.Title = "Eventos e incentivo";
                    ViewBag.SiteCSS = "eventos";
                    ViewBag.IDEmpresa = this._iDEmpresa = 5;
                    break;
                case "franchising":
                    ViewBag.Layout = "Franchising";
                    ViewBag.Title = "Franchising";
                    ViewBag.SiteCSS = "franchising";
                    ViewBag.IDEmpresa = this._iDEmpresa = 8;
                    break;
                case "viagens-a-lazer":
                default:
                    ViewBag.Layout = String.Empty;
                    ViewBag.Title = "Viagens a Lazer";
                    ViewBag.SiteCSS = "franchising";
                    ViewBag.IDEmpresa = this._iDEmpresa = 1;
                    break;
            }

            ViewBag.Title = String.Format("Onde Estamos - Flytour {0}", ViewBag.Title);
            ViewBag.SiteName = siteName;
        }

        private List<SelectListItem> GetListaDeEstadosOndeTemAgencias()
        {
            List<SelectListItem> estados = new List<SelectListItem>();
            List<string> estadosList = this.AgenciasMapa.Select(ag => ag.Estado).Distinct().OrderBy(est => est).ToList();

            for (int i = 0; i < estadosList.Count; i++)
            {
                estados.Add(new SelectListItem()
                {
                    Text = estadosList[i],
                    Value = estadosList[i]
                });
            }

            estados.Insert(0, new SelectListItem() { Text = "Selecione uma cidade", Value = String.Empty });

            return estados;
        }

        private List<string> GetCidadesOndeTemAgencias(string estado)
        {
            if (String.IsNullOrEmpty(estado))
                return new List<string>();

            estado = estado.ToLower();

            List<string> cidadeList = this.AgenciasMapa
                                            .FindAll(ag => !String.IsNullOrEmpty(ag.Cidade) && ag.Estado.ToLower().Equals(estado))
                                            .Select(ag => ag.Cidade).Distinct().OrderBy(cid => cid).ToList();

            return cidadeList;
        }

        public ActionResult Index(string siteName)
        {
            this.CarregaViewBag(siteName);
            ViewBag.OpcoesParaEstados = this.GetListaDeEstadosOndeTemAgencias();
            ViewBag.OpcoesParaCidades = new List<SelectListItem>()
            {
                new SelectListItem(){ Text = "Selecione uma cidade", Value = String.Empty}
            };

            return View("Index");
        }

        public JsonResult GetCidadesPorEstado(string siteName, string estado)
        {
            this.CarregaViewBag(siteName);

            if (!String.IsNullOrEmpty(siteName))
                return Json(this.GetCidadesOndeTemAgencias(estado), JsonRequestBehavior.AllowGet);

            return Json(new object[] { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgenciasNoMapaPorEstadoECidade(string siteName, string estado, string cidade)
        {

            if (String.IsNullOrEmpty(estado))
                estado = Session["cEPOuEnderecoestado"].ToString();
            else
                Session["cEPOuEnderecoestado"] = estado;

            if (String.IsNullOrEmpty(cidade))
                cidade = Session["cEPOuEnderecocidade"].ToString();
            else
                Session["cEPOuEnderecocidade"] = cidade;

            this.CarregaViewBag(siteName);

            if (!String.IsNullOrEmpty(estado))
            {
                var agencias = this.AgenciasMapa.FindAll
                                (ag =>
                                    !String.IsNullOrEmpty(ag.Estado) &&
                                    ag.Estado.ToLower().Equals(estado.ToLower()) &&
                                    (String.IsNullOrEmpty(cidade) ||
                                        (!String.IsNullOrEmpty(ag.Cidade) &&
                                         ag.Cidade.ToLower().Equals(cidade.ToLower())
                                        )
                                    )
                                ).OrderBy(ag => ag.Cidade)
                                .Select(ag => new { ag.Latitude, ag.Longitude })
                                .ToList();

                return Json(agencias, JsonRequestBehavior.AllowGet);
            }

            return Json(new object[] { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgenciasNoMapaPorCEPOuEndereco(string siteName, string cEPOuEndereco)
        {
            if (String.IsNullOrEmpty(cEPOuEndereco))
                cEPOuEndereco = Session["cEPOuEndereco"].ToString();
            else
                Session["cEPOuEndereco"] = cEPOuEndereco;



            this.CarregaViewBag(siteName);
            cEPOuEndereco = (cEPOuEndereco ?? String.Empty).Trim().ToLower();
            if (string.IsNullOrEmpty(siteName))
                siteName = "viagens-a-lazer";
            if (!String.IsNullOrEmpty(cEPOuEndereco))
            {
                Business.Shared.SharedDAO sharedDAO = new Business.Shared.SharedDAO();
                var agencias = sharedDAO.GetAgenciasPorEmpresa(this._iDEmpresa, cEPOuEndereco)
                                .OrderBy(ag => ag.DistanciaMetros)
                                .ThenByDescending(ag => ag.CodigoPreferencial)
                                .Select(ag => new { ag.Latitude, ag.Longitude, ag.DistanciaMetros })
                                .ToList();

                if (agencias.Count == 0)
                { return Json(AgenciasMapa, JsonRequestBehavior.AllowGet); }
                else
                {
                    return Json(agencias, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new object[] { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgenciasNoMapaPeloNome(string siteName, string nomeAgencia)
        {
            this.CarregaViewBag(siteName);
            nomeAgencia = (nomeAgencia ?? String.Empty).Trim().ToLower();

            if (!String.IsNullOrEmpty(nomeAgencia))
            {
                var agencias = this.AgenciasMapa
                    .FindAll(ag => !String.IsNullOrEmpty(ag.Titulo) && ag.Titulo.ToLower().Contains(nomeAgencia))
                    .OrderBy(ag => ag.Cidade).Select(ag => new { ag.Latitude, ag.Longitude }).ToList();

                return Json(agencias, JsonRequestBehavior.AllowGet);
            }

            return Json(new object[] { }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTodasAgenciasNoMapa(string siteName)
        {
            this.CarregaViewBag(siteName);
            return Json(this.AgenciasMapa.OrderBy(ag => ag.Titulo), JsonRequestBehavior.AllowGet);
        }
    }
}
