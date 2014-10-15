using System.Web.Mvc;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using FTV;
using FT.Web.Site.Controllers.Base;
using FT.Web.Business.Pagamento;
using System;
using FT.Web.Core.Util.SiteTracer;

namespace FT.Web.Site.Controllers.Pagamento
{
    [Tracer]
    public class PagamentoController : _BaseController
    {
        PagamentoBusiness pagamentoBussiness = new PagamentoBusiness();

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Modalidade(int? indice, int? id, string valorOper)
        {
            ConsultaFinanRetornoModel retorno = new ConsultaFinanRetornoModel();
            try
            {
                var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
                {
                    return new FlyTourModel();
                });

                if (indice != null)
                    ViewBag.cartaoIndice = indice;
                else
                    ViewBag.cartaoIndice = 1;
                if (string.IsNullOrEmpty(valorOper))
                    retorno = pagamentoBussiness.LoadPagamentos(addedFlytour, Conv.ToDecimal((addedFlytour.ValorTotal / ViewBag.cartaoIndice)));
                else
                    retorno = pagamentoBussiness.LoadPagamentos(addedFlytour, Conv.ToDecimal(valorOper));

                if (string.IsNullOrEmpty(valorOper))
                {
                    var valorTotal = retorno.Valor;
                    decimal valor = Conv.ToDecimal(Conv.ToDecimal(retorno.Valor / ViewBag.cartaoIndice).ToString("N")) * ViewBag.cartaoIndice;
                    retorno.Valor = retorno.Valor / ViewBag.cartaoIndice;
                    retorno.ValorDif = valor - valorTotal;

                }
                else
                {
                    retorno.Valor = Conv.ToDecimal(valorOper);
                    retorno.ValorDif = 0;
                }
                ViewBag.cartaoIndice = id;

            }
            catch (Exception ex)
            {
                retorno.Mensagem = Conv.GetErrorMessage(ex, true);
            }
            return PartialView(retorno);

        }

        public PartialViewResult LoadPagamentos(int? indice)
        {
            ConsultaFinanRetornoModel retorno = new ConsultaFinanRetornoModel();
            try
            {
                var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
                {
                    return new FlyTourModel();
                });

                if (indice != null)
                    ViewBag.cartaoIndice = indice;
                else
                    ViewBag.cartaoIndice = 1;

                retorno = pagamentoBussiness.LoadPagamentos(addedFlytour, Conv.ToDecimal((addedFlytour.ValorTotal / ViewBag.cartaoIndice)));



                var valorTotal = retorno.Valor;
                decimal valor = Conv.ToDecimal(Conv.ToDecimal(retorno.Valor / ViewBag.cartaoIndice).ToString("N")) * ViewBag.cartaoIndice;
                retorno.Valor = retorno.Valor / ViewBag.cartaoIndice;
                retorno.ValorDif = valor - valorTotal;

            }
            catch (Exception ex)
            {
                retorno.Mensagem = Conv.GetErrorMessage(ex, true);
            }
            return PartialView(retorno);

        }
    }
}
