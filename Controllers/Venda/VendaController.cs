using FT.Web.Business.Venda;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models.Venda;
using FT.Web.Site.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Venda
{
    [Tracer]
    public class VendaController : _BaseController
    {
        //
        // GET: /Venda/

        public ActionResult Index()
        {
            SetUrlVoucher();
            var filtro = new FiltroVendaModel();
                        
            filtro.ID_PESSOA = SessionUsuario.ID_PESSOA;
            List<VendaModel> lstVenda = VendaBusiness.Consulta(filtro);
            ViewBag.lstVenda = lstVenda;
            return View(filtro);
        }

        private void SetUrlVoucher()
        {
            

            string IsUrlVoucherDev = ConfigurationManager.AppSettings["IsUrlVoucherDev"];
            string url = "";
            if (!string.IsNullOrEmpty(IsUrlVoucherDev) && (Convert.ToBoolean(IsUrlVoucherDev)))
            {
                url = ConfigurationManager.AppSettings["UrlBackoffice"] 
                    + "pkg_voucher.prc_gera_voucher?p_token=" 
                    + FT.Web.Business.Service.LoginService.GetLoginService().Token;
            }
            else
            {
                url = ConfigurationManager.AppSettings["UrlBackoffice"]
                    + "pkg_voucher.prc_gera_voucher?p_token="
                    + FT.Web.Business.Service.LoginService.GetLoginService().Token;
            }
            @ViewBag.urlVoucher = url;
        }

        [HttpPost]
        public ActionResult Consulta(FiltroVendaModel filtro)
        {
            filtro.ID_PESSOA = SessionUsuario.ID_PESSOA;

            List<VendaModel> lstVenda = VendaBusiness.Consulta(filtro);

            return PartialView(lstVenda);
        }

        [HttpPost]
        public ActionResult Detalhe(string id_file)
        {
            var filtro = new FiltroVendaModel();
            filtro.CodigoFile = id_file;
            filtro.ID_PESSOA = SessionUsuario.ID_PESSOA;
            List<VendaModel> lstVenda = VendaBusiness.Consulta(filtro);
            return PartialView(lstVenda[0]);
        }
    }
}
