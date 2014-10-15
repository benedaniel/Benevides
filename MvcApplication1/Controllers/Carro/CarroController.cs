using FT.Web.Business.Carro;
using FT.Web.Business.Pagamento;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models;
using FT.Web.Site.Controllers.Base;
using FTV.Shared.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace FT.Web.Site.Controllers.Carro
{
    [Tracer]
    public class CarroController : _BaseController
    {
        //
        // GET: /Carro/
        PagamentoBusiness pagamentoBussiness = new PagamentoBusiness();
        public ActionResult Index(CarroPesquisaModel carroModel, bool? hdnIsLocalDiferenteEntrega)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");

            CarroBusiness carroBusiness = new CarroBusiness();

            //CarroPesquisaModel carroModel = new CarroPesquisaModel();

            List<string> horascheckin = carroBusiness.PopularHoras();
            List<string> horasCheckOut = carroBusiness.PopularHoras();

            carroModel.HorasCheckIn = horascheckin.Select(c => new SelectListItem
                                                            {
                                                                Text = c,
                                                                Value = c,
                                                                Selected = c.TrimEnd().TrimStart() == carroModel.HoraCheckInSelecionada
                                                            }).ToList();

            carroModel.HorasCheckOut = horasCheckOut.Select(c => new SelectListItem
                                                            {
                                                                Text = c,
                                                                Value = c,
                                                                Selected = c.TrimEnd().TrimStart() == carroModel.HoraCheckOutSelecionada
                                                            }).ToList();

            carroModel.IsLocalDiferenteEntrega = hdnIsLocalDiferenteEntrega.HasValue && hdnIsLocalDiferenteEntrega.Value ? true : false;

            return View(carroModel);
        }

        public ActionResult IndexMontePacote(bool? hdnIsLocalDiferenteEntrega)
        {
            //CarroPesquisaModel carroModel = SessionMontePacote.Last().CarroPesquisa.FirstOrDefault();

            //Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            //Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("pt-BR");

            //CarroBusiness carroBusiness = new CarroBusiness();

            //List<string> horascheckin = carroBusiness.PopularHoras();
            //List<string> horasCheckOut = carroBusiness.PopularHoras();

            //carroModel.HorasCheckIn = horascheckin.Select(c => new SelectListItem
            //{
            //    Text = c,
            //    Value = c,
            //    Selected = c.TrimEnd().TrimStart() == carroModel.HoraCheckInSelecionada
            //}).ToList();

            //carroModel.HorasCheckOut = horasCheckOut.Select(c => new SelectListItem
            //{
            //    Text = c,
            //    Value = c,
            //    Selected = c.TrimEnd().TrimStart() == carroModel.HoraCheckOutSelecionada
            //}).ToList();

            //carroModel.IsLocalDiferenteEntrega = hdnIsLocalDiferenteEntrega.HasValue && hdnIsLocalDiferenteEntrega.Value ? true : false;

            //return View("Index", carroModel);
            return View("Index");
        }

        public PartialViewResult CarroForm()
        {
            CarroBusiness carroBusiness = new CarroBusiness();

            CarroPesquisaModel carroModel = new CarroPesquisaModel();

            List<string> horascheckin = carroBusiness.PopularHoras();
            List<string> horasCheckOut = carroBusiness.PopularHoras();

            carroModel.HorasCheckIn = horascheckin.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = c.TrimEnd().TrimStart() == "09:00"
            }).ToList();

            carroModel.HorasCheckOut = horasCheckOut.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = c.TrimEnd().TrimStart() == "09:00"
            }).ToList();

            return PartialView(carroModel);
        }

        public ActionResult CarroMenuForm()
        {
            CarroBusiness carroBusiness = new CarroBusiness();

            CarroPesquisaModel carroModel = new CarroPesquisaModel();

            List<string> horascheckin = carroBusiness.PopularHoras();
            List<string> horasCheckOut = carroBusiness.PopularHoras();

            carroModel.HorasCheckIn = horascheckin.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = c.TrimEnd().TrimStart() == "09:00"
            }).ToList();

            carroModel.HorasCheckOut = horasCheckOut.Select(c => new SelectListItem
            {
                Text = c,
                Value = c,
                Selected = c.TrimEnd().TrimStart() == "09:00"
            }).ToList();

            return View(carroModel);
        }

        //[HttpPost]
        public ActionResult Resultado(CarroPesquisaModel model, string hdnLocalRetiradaCodigo, string hdnLocalDevolucaoCodigo, bool? hdnIsLocalDiferenteEntrega)
        {
            List<IGrouping<string, ReservaCarroModel>> carrosAgrupados = new List<IGrouping<string, ReservaCarroModel>>();
            List<CarroResultadoTratadoModel> carroResultadoTratado = new List<CarroResultadoTratadoModel>();

            //if (hdnLocalDevolucaoCodigo == null)
            //{
            //    hdnLocalDevolucaoCodigo = SessionMontePacote.FirstOrDefault().CodigoCidadeDestino;
            //}

            //if (hdnLocalRetiradaCodigo == null)
            //{
            //    hdnLocalRetiradaCodigo = SessionMontePacote.FirstOrDefault().CodigoCidadeDestino;
            //}

            if (model != null)
            {
                CarroBusiness carroBusiness = new CarroBusiness();

                //CarroResultadoModel consultaRetorno = carroBusiness.ConsultaRetorno(model);

                //carrosAgrupados = consultaRetorno.Carros.OrderBy(x => x.DescricaoCategoria).GroupBy(c => c.DescricaoCategoria).ToList();

                model.IsLocalDiferenteEntrega = hdnIsLocalDiferenteEntrega.HasValue && hdnIsLocalDiferenteEntrega.Value == true ? true : false;

                carroResultadoTratado = carroBusiness.TrataResultadoCarro(model, hdnLocalRetiradaCodigo, hdnLocalDevolucaoCodigo, SessionKey);
            }

            if (String.IsNullOrEmpty(model.LocalDevolucao))
            {
                model.LocalDevolucao = model.LocalRetirada;
            }

            ViewBag.DadosPesquisaCarro = model;
            return PartialView("Resultado", carroResultadoTratado);
        }

        public ActionResult ResultadoMontePacote(string indiceDestino, string cidade, string codigoLocalDevolucao, string codigoLocalRetirada)
        {
            var CarroFlytour = AppCache.Get<List<CarroResultadoTratadoModel>>(EnumProcesso.CarroVenda, SessionKey + "CarroMP" + cidade, 2, () =>
            {
                return new List<CarroResultadoTratadoModel>();
            });

            if (CarroFlytour.Count > 0)
            {
                return PartialView("Resultado", CarroFlytour); 
            }
            List<IGrouping<string, ReservaCarroModel>> carrosAgrupados = new List<IGrouping<string, ReservaCarroModel>>();
            List<CarroResultadoTratadoModel> carroResultadoTratado = new List<CarroResultadoTratadoModel>();

            CarroPesquisaModel model = MPSession.Destinos[Convert.ToInt32(indiceDestino)].Carro;

            string hdnLocalDevolucaoCodigo = codigoLocalDevolucao;

            string hdnLocalRetiradaCodigo = codigoLocalRetirada;

            if (model != null)
            {
                CarroBusiness carroBusiness = new CarroBusiness();
                    
                model.IsLocalDiferenteEntrega = false;

                carroResultadoTratado = carroBusiness.TrataResultadoCarro(model, hdnLocalRetiradaCodigo, hdnLocalDevolucaoCodigo, SessionKey);

                ViewBag.IsMontePacote = true;
            }

            var success = AppCache.Put(EnumProcesso.CarroVenda, SessionKey + "CarroMP" + cidade, carroResultadoTratado, TimeSpan.FromMinutes(2));
            
            return PartialView("Resultado", carroResultadoTratado);
        }

        [HttpPost]
        public ActionResult ResultadoFiltroLateral(string hdnLocalRetiradaCodigo, string hdnLocalDevolucaoCodigo, bool isLocalDiferenteEntrega, string dataRetirada, string horaRetirada, string dataDevolucao, string horaDevolucao, string nomeCidadeRetirada, string nomeCidadeDevolucao)
        {
            CarroPesquisaModel model = new CarroPesquisaModel();

            model.LocalRetirada = nomeCidadeRetirada;
            model.DataCheckIn = dataRetirada;
            model.DataCheckOut = dataDevolucao;
            model.HoraCheckInSelecionada = horaRetirada;
            model.HoraCheckOutSelecionada = horaDevolucao;
            model.IsLocalDiferenteEntrega = isLocalDiferenteEntrega;
            model.LocalDevolucao = nomeCidadeDevolucao;

            List<IGrouping<string, ReservaCarroModel>> carrosAgrupados = new List<IGrouping<string, ReservaCarroModel>>();
            List<CarroResultadoTratadoModel> carroResultadoTratado = new List<CarroResultadoTratadoModel>();
            if (model != null)
            {
                CarroBusiness carroBusiness = new CarroBusiness();
                carroResultadoTratado = carroBusiness.TrataResultadoCarro(model, hdnLocalRetiradaCodigo, hdnLocalDevolucaoCodigo, SessionKey);
            }

            ViewBag.DadosPesquisaCarro = model;
            return PartialView("Resultado", carroResultadoTratado);
        }

        public ActionResult ReservaCarro(ReservaCarroModel carro)
        {
            ViewBag.CaracteCarro = ListaCaracteristicaCarro(carro);

            //CarroBusiness carroBusiness = new CarroBusiness();

            //carroBusiness.CarregarLocaisRetiradaDevolucao(carro);

            //Rotina que realiza a consulta de locais de retirada e devolução
            //disponíveis para o cliente
            
            //CarroBusiness carroBusiness = new CarroBusiness();

            //CarroPesquisaModel carroPesquisaModel = new CarroPesquisaModel();

            //carroPesquisaModel.LocalRetirada = carro.LocaisRetirada[0].NomeCidade;
            //carroPesquisaModel.DataCheckIn = carro.DataCheckIn;
            //carroPesquisaModel.DataCheckOut = carro.DataCheckOut;
            //carroPesquisaModel.HoraCheckInSelecionada = carro.HoraCheckInSelecionada;
            //carroPesquisaModel.HoraCheckOutSelecionada = carro.HoraCheckOutSelecionada;
            //carroPesquisaModel.LocalDevolucao = carro.LocaisDevolucao[0].NomeCidade;
            //carroPesquisaModel.IsLocalDiferenteEntrega = (carroPesquisaModel.LocalRetirada != carroPesquisaModel.LocalDevolucao ? true : false);            

            //var retorno = carroBusiness.ConsultaRetornoLocalidadesRetiradaDevolucao(carroPesquisaModel, carro.LocaisRetirada[0].CodigoCidade, carro.LocaisDevolucao[0].CodigoCidade, carro.CodigoLocadora, carro.CodigoProvedor);

            //var carroSelecionado = retorno.Carros.Where(c => c.SIPPCode == carro.SIPPCode && c.CodigoPlano == carro.CodigoPlano).FirstOrDefault();

            //carro.LocaisDevolucao = carroSelecionado.LocaisDevolucao;
            //carro.LocaisRetirada = carroSelecionado.LocaisRetirada;

            return PartialView(carro);
        }

        public ActionResult ReservaCarroMontePacote(ReservaCarroModel carro, bool? isMontePacote)
        {
            ViewBag.IsMontePacote = true;

            ViewBag.CaracteCarro = ListaCaracteristicaCarro(carro);

            return PartialView("ReservaCarro", carro);
        }

        private string ListaCaracteristicaCarro(ReservaCarroModel carro)
        {
            StringBuilder caracteristicas = new StringBuilder();

            if (!string.IsNullOrEmpty(carro.Combustivel))
                caracteristicas.Append(carro.Combustivel + ", ");

            if (!string.IsNullOrEmpty(carro.Tracao))
                caracteristicas.Append("Traçao: " + carro.Tracao + ", ");

            if (carro.QuantidadeBagagem != 0)
                caracteristicas.Append("Quantidade de Bagagem: " + carro.QuantidadeBagagem + ", ");

            if (carro.QuantidadePassageiro != 0)
                caracteristicas.Append("Quantidade de Passageiro: " + carro.QuantidadePassageiro + ", ");

            if (carro.QuantidadePorta != 0)
                caracteristicas.Append("Quantidade de Portas: " + carro.QuantidadePorta + ", ");

            if (!string.IsNullOrEmpty(carro.TipoTransmissao))
                caracteristicas.Append("Tipo de Transmissao: " + carro.TipoTransmissao + ", ");

            return caracteristicas.ToString().Length > 0 ? caracteristicas.ToString().Remove(caracteristicas.Length - 1, 1) : caracteristicas.ToString();
        }

        [HttpPost]
        public JsonResult EfetuarReserva(ReservaCarroModel carroModel)
        {
            //CarroBusiness.Reservar(carroModel, new UsuarioModel(), new PessoaModel());

            return Json(carroModel);
        }

        [HttpPost]
        public ActionResult AlterarLocalRetirada(string valor, string nomeLocadoraLoja, ReservaCarroModel carroModel)
        {
            decimal valorConvertido = Convert.ToDecimal(valor);

            List<SelectListItem> itensCombo = new List<SelectListItem>();

            foreach (var item in carroModel.LocaisRetiradaCombo)
            {
                if (item.Value == valor)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }

                itensCombo.Add(item);
            }

            carroModel.LocaisRetiradaCombo.Clear();
            carroModel.LocaisRetiradaCombo = itensCombo;

            carroModel.ValorLocalRetiradaSelecionado = valor.ToString();
            carroModel.NomeLocadoraLojaSelecionada = nomeLocadoraLoja;

            ViewBag.CaracteCarro = ListaCaracteristicaCarro(carroModel);

            return PartialView("ReservaCarro", carroModel);
        }

        [HttpPost]
        public ActionResult AlterarLocalDevolucao(string valor, string nomeLocadoraLoja, ReservaCarroModel carroModel)
        {
            decimal valorConvertido = Convert.ToDecimal(valor);

            List<SelectListItem> itensCombo = new List<SelectListItem>();

            foreach (var item in carroModel.LocaisDevolucaoCombo)
            {
                if (item.Value == valor)
                {
                    item.Selected = true;
                }
                else
                {
                    item.Selected = false;
                }

                itensCombo.Add(item);
            }

            carroModel.LocaisDevolucaoCombo.Clear();
            carroModel.LocaisDevolucaoCombo = itensCombo;

            carroModel.ValorLocalDevolucaoSelecionado = valor.ToString();
            carroModel.NomeLocadoraLoDevolucaojaSelecionada = nomeLocadoraLoja;

            ViewBag.CaracteCarro = ListaCaracteristicaCarro(carroModel);

            return PartialView("ReservaCarro", carroModel);
        }

        [HttpPost]
        public string FormasdePagamento(string CodigoTarifa)
        {
            var CarroFlytour = AppCache.Get<List<ReservaCarroModel>>(EnumProcesso.CarroVenda, SessionKey + "Carro", 90, () =>
            {
                return new List<ReservaCarroModel>();
            });

            ReservaCarroModel CarroReserva = new ReservaCarroModel();

            if (CarroFlytour != null && CarroFlytour.Count > 0)
            {
                CarroReserva = CarroFlytour.FirstOrDefault(p => p.CodigoTarifa == CodigoTarifa);
            }

            var flyTour = new FlyTourModel();
            var apto = new ApartamentoModel();
            apto.Passageiros = new List<PassageiroModel>();
            apto.Passageiros.Add(new PassageiroModel() { IdadePassageiro = 30 });
            flyTour.Apartamentos.Add(apto);
            CarroReserva.Comprar = true;
            flyTour.Carro.Add(CarroReserva);

            List<MaxParcelaRetorno> finan = new List<MaxParcelaRetorno>();

            finan = pagamentoBussiness.RetornaFormasPagamento(flyTour, SessionUsuario);

            StringBuilder retorno = new StringBuilder();
            retorno.Append("<table border='0' class='tbDetalhePreco'>");
            retorno.Append("<tbody>");

            foreach (var item in finan)
            {
                retorno.Append("<tr>");
                retorno.Append("<td>");
                retorno.Append(item.DescricaoPlano);
                retorno.Append("</td>");
                retorno.Append("</tr>");
            }

            retorno.Append("</tbody>");
            retorno.Append("</table>");

            return retorno.ToString();
        }
    }
}