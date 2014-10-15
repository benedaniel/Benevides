using FT.Web.Business.Aereo;
using FT.Web.Business.Hotel;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models;
using FT.Web.Site.AutoComplete;
using FT.Web.Site.Controllers.Base;
using FTV;
using FTV.Shared.Business;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FT.Web.Site.Controllers.MontePacote
{
    [Tracer]
    public class MontePacoteController : _BaseController
    {
        //
        // GET: /MontePacote/

        public ActionResult Index()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            CleanItensComprar(addedFlytour);

            MPSession = new MontePacoteModel();
            Session["IndiceAdicionarCarro"] = null;
            Session["IndiceAdicionarAtividade"] = null;
            Session["IndiceAdicionarHotel"] = null;
            Session["IndiceAdicionarAereo"] = null;

            return View();
        }

        public PartialViewResult QuartosMP(int? indice)
        {
            System.Diagnostics.Stopwatch swViewsFormMPQuartosMP = new System.Diagnostics.Stopwatch();
            swViewsFormMPQuartosMP.Start();
            if (indice != null)
                ViewBag.Indice = indice;
            else
                ViewBag.Indice = 1;
            swViewsFormMPQuartosMP.Stop();

            return PartialView();
        }

        public PartialViewResult QuartosMPMulti(int? indice)
        {
            System.Diagnostics.Stopwatch swViewsFormMPQuartosMPMulti = new System.Diagnostics.Stopwatch();
            swViewsFormMPQuartosMPMulti.Start();

            if (indice != null)
                ViewBag.IndiceMulti = indice;
            else
                ViewBag.IndiceMulti = 1;
            swViewsFormMPQuartosMPMulti.Stop();

            return PartialView();
        }

        public ActionResult AdicionarDestinoMP(string ListaOrigem, string ListaDestino, string ListaDestinoCodigo, string ListaOrigemCodigo, string ListaSaida, string RetornoMultiMP, string ApartamentoMP)
        {
            if (Request.UrlReferrer.PathAndQuery != "/MontePacote/ReloadIndex")
            {
                MPSession = new MontePacoteModel();
                Session["IndiceAdicionarCarro"] = null;
                Session["IndiceAdicionarAtividade"] = null;
                Session["IndiceAdicionarHotel"] = null;
                Session["IndiceAdicionarAereo"] = null;
            }

            MontePacoteModel montePacoteModel = MPSession;
            var Itens = ListaOrigem.Split(',');

            for (int i = 0; i < Itens.Length; i++)
            {
                DestinoMontePacoteModel destinoMontePacoteModel = new DestinoMontePacoteModel();

                destinoMontePacoteModel.NomeCidadeOrigem = Itens[i];
                destinoMontePacoteModel.NomeCidadeDestino = ListaDestino.Split(',')[i];
                destinoMontePacoteModel.CodigoCidadeOrigem = ListaOrigemCodigo.Split(',')[i];
                destinoMontePacoteModel.CodigoCidadeDestino = ListaDestinoCodigo.Split(',')[i];
                destinoMontePacoteModel.Saida = ListaSaida.Split(',')[i];

                if (i < Itens.Length - 1)
                {
                    destinoMontePacoteModel.Retorno = ListaSaida.Split(',')[i + 1];
                }
                else
                {
                    destinoMontePacoteModel.Retorno = RetornoMultiMP;
                }

                montePacoteModel.Aereo = new List<AereoModel>();
                montePacoteModel.Destinos.Add(destinoMontePacoteModel);
                RecalcularOpcoesVoo(montePacoteModel);
            }

            var QuantCriancas = 0;
            var QuantAdultos = 0;

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            var routes_list = (object[])json_serializer.DeserializeObject(ApartamentoMP);

            if (routes_list.Length > 0)
            {
                montePacoteModel.Apartamentos = new List<ApartamentoModel>();
                for (int i = 0; i < routes_list.Length; i++)
                {
                    var s = routes_list[i];
                    var ApartamentoModel = new ApartamentoModel();
                    ApartamentoModel.Passageiros = new List<PassageiroModel>();
                    var quantAdt = Conv.ToInt32((s as Dictionary<string, object>)["quantadulto"].ToString());
                    QuantAdultos += quantAdt;
                    for (int k = 0; k < quantAdt; k++)
                    {
                        ApartamentoModel.Passageiros.Add(new PassageiroModel() { IdadePassageiro = 30 });
                    }

                    var criancasd = (object[])(s as Dictionary<string, object>)["criancas"];
                    QuantCriancas += criancasd.Length;
                    for (int j = 0; j < criancasd.Length; j++)
                    {
                        ApartamentoModel.Passageiros.Add(new PassageiroModel() { IdadePassageiro = Conv.ToDecimal(Conv.ToInt32(criancasd[j]).ToString()) });
                    }
                    montePacoteModel.Apartamentos.Add(ApartamentoModel);
                }

                montePacoteModel.QuantAdultos = QuantAdultos;
                montePacoteModel.QuantCriancas = QuantCriancas;
                montePacoteModel.QuantQuartos = routes_list.Length;
            }

            MPSession = montePacoteModel;
            return RedirectToAction("ReloadIndex");
        }

        public ActionResult AdicionarDestino(string Origem, string Destino, string Saida, string Retorno, string origemMP, string hdnOrigemCodigo, string hdnDestinoCodigo, string hdnQuantQuartos)
        {
            if (Request.UrlReferrer.PathAndQuery != "/MontePacote/ReloadIndex")
            {
                MPSession = new MontePacoteModel();
                Session["IndiceAdicionarCarro"] = null;
                Session["IndiceAdicionarAtividade"] = null;
                Session["IndiceAdicionarHotel"] = null;
                Session["IndiceAdicionarAereo"] = null;
            }

            MontePacoteModel montePacoteModel = MPSession;
            DestinoMontePacoteModel destinoMontePacoteModel = new DestinoMontePacoteModel();

            destinoMontePacoteModel.NomeCidadeOrigem = Origem;
            destinoMontePacoteModel.NomeCidadeDestino = Destino;
            destinoMontePacoteModel.CodigoCidadeOrigem = hdnOrigemCodigo;
            destinoMontePacoteModel.CodigoCidadeDestino = hdnDestinoCodigo;
            destinoMontePacoteModel.Saida = Saida;
            destinoMontePacoteModel.Retorno = Retorno;

            var QuantCriancas = 0;
            var QuantAdultos = 0;

            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            var routes_list = (object[])json_serializer.DeserializeObject(origemMP);

            if (routes_list.Length > 0)
            {
                montePacoteModel.Apartamentos = new List<ApartamentoModel>();
                for (int i = 0; i < routes_list.Length; i++)
                {
                    var s = routes_list[i];
                    var ApartamentoModel = new ApartamentoModel();
                    ApartamentoModel.Passageiros = new List<PassageiroModel>();
                    var quantAdt = Conv.ToInt32((s as Dictionary<string, object>)["quantadulto"].ToString());
                    QuantAdultos += quantAdt;
                    for (int k = 0; k < quantAdt; k++)
                    {
                        ApartamentoModel.Passageiros.Add(new PassageiroModel() { IdadePassageiro = 30 });
                    }

                    var criancasd = (object[])(s as Dictionary<string, object>)["criancas"];
                    QuantCriancas += criancasd.Length;
                    for (int j = 0; j < criancasd.Length; j++)
                    {
                        ApartamentoModel.Passageiros.Add(new PassageiroModel() { IdadePassageiro = Conv.ToDecimal(Conv.ToInt32(criancasd[j]).ToString()) });
                    }
                    montePacoteModel.Apartamentos.Add(ApartamentoModel);
                }

                montePacoteModel.QuantAdultos = QuantAdultos;
                montePacoteModel.QuantCriancas = QuantCriancas;
                montePacoteModel.QuantQuartos = routes_list.Length;
            }

            montePacoteModel.Aereo = new List<AereoModel>();

            montePacoteModel.Destinos.Add(destinoMontePacoteModel);

            RecalcularOpcoesVoo(montePacoteModel);

            MPSession = montePacoteModel;

            return RedirectToAction("ReloadIndex");
        }

        private static void RecalcularOpcoesVoo(MontePacoteModel montePacoteModel)
        {
            if (montePacoteModel.Destinos.Count > 1)
            {
                montePacoteModel.OpcoesAereo = new List<DestinoMontePacoteModel>();

                foreach (DestinoMontePacoteModel destino in montePacoteModel.Destinos)
                {
                    montePacoteModel.OpcoesAereo.Add(destino);
                }

                montePacoteModel.OpcoesAereo.Add(new DestinoMontePacoteModel()
                {
                    NomeCidadeOrigem = montePacoteModel.Destinos.Last().NomeCidadeDestino,
                    NomeCidadeDestino = montePacoteModel.Destinos.First().NomeCidadeOrigem,
                    CodigoCidadeOrigem = montePacoteModel.Destinos.Last().CodigoCidadeDestino,
                    CodigoCidadeDestino = montePacoteModel.Destinos.First().CodigoCidadeOrigem,
                    Saida = montePacoteModel.Destinos.Last().Retorno
                });
            }
            else if (montePacoteModel.Destinos.Count == 1)
            {
                montePacoteModel.OpcoesAereo = new List<DestinoMontePacoteModel>();

                montePacoteModel.OpcoesAereo.Add(montePacoteModel.Destinos.Last());
            }
        }

        public JsonResult RemoverDestino(string indice)
        {
            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            MPSession.Destinos.Remove(destinoMontePacote);

            RecalcularOpcoesVoo(MPSession);

            return Json(SessionKey);
        }

        public ActionResult MontarPacoteAereoOffline(string IdaeVolta, string CodigoTarifa, string CodigoCidadeOrigem, string CodigoCidadeDestino, string NomeCidadeOrigem, string NomeCidadeDestino, string DataOrigem, string DataDestino, string SiglaOrigem, string SiglaDestino, string apartamentos)
        {
            AereoModel AereoModel = new AereoModel();

            JavaScriptSerializer json = new JavaScriptSerializer();
            var ApartamentoList = (List<ApartamentoModel>)json.Deserialize(apartamentos, typeof(List<ApartamentoModel>));

            AereoModel.Origem = SiglaOrigem;
            AereoModel.QuantidadeAdulto = Conv.ToInt32(ApartamentoList.SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro >= 12)).Count());
            AereoModel.QuantidadeCrianca = Conv.ToInt32(ApartamentoList.SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count());
            AereoModel.QuantidadeBebe = Conv.ToInt32(ApartamentoList.SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count());
            AereoModel.DataEmbarque = DataOrigem;
            AereoModel.DataRetorno = DataDestino;
            AereoModel.Destino = SiglaDestino;
            AereoModel.Tipo = "IdaeVolta";

            var reco1 = IdaeVolta.Split(';');

            var intersec = reco1.First();
            AereoModel.Recomendation = Conv.ToInt32(intersec);
            AereoModel.CodigoTarifa = Conv.ToInt64(CodigoTarifa);

            AereoModel.TrechosSel.Add(intersec);

            var tarifa = AereoBusiness.Tarifar(AereoModel);

            var AddflyTour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            var trechosAer = AereoBusiness.MapearTrecho(tarifa.Trechos);
            AereoModel.ValorTaxa = tarifa.ValorTaxa;
            //AereoModel.Valor = Conv.ToDecimal((AereoModel.ValorTaxa + tarifa.ValorTotal).ToString("n2"));
            AereoModel.Valor = Conv.ToDecimal(tarifa.ValorVenda.ToString("n2"));
            AereoModel.Trechos.Clear();
            foreach (var item in trechosAer)
            {
                AereoModel.Trechos.Add(item);
            }

            MontePacoteModel model = new MontePacoteModel();

            var flyTour = MPSession;

            AereoModel.Apartamentos = ApartamentoList;
            model.Apartamentos = ApartamentoList;

            if (AereoModel.Trechos.Count > 0)
            {
                var remover = flyTour.Aereo.FirstOrDefault();

                flyTour.OpcoesAereo.Clear();

                MPSession.Aereo.Clear();

                model.Aereo.Add(AereoModel);
            }

            DestinoMontePacoteModel destinoMontePacoteModel = new DestinoMontePacoteModel();
            HotelBusiness hotelBusiness = new Business.Hotel.HotelBusiness();
            destinoMontePacoteModel.NomeCidadeOrigem = NomeCidadeOrigem;
            if (!string.IsNullOrEmpty(CodigoCidadeDestino))
            {
                destinoMontePacoteModel.NomeCidadeDestino = hotelBusiness.ConsultarLocalizacaoHotel(CodigoCidadeDestino).NomeCidade;
            }
            else
            {
                destinoMontePacoteModel.NomeCidadeDestino = NomeCidadeDestino;
            }
            destinoMontePacoteModel.CodigoCidadeOrigem = CodigoCidadeOrigem;
            destinoMontePacoteModel.CodigoCidadeDestino = CodigoCidadeDestino;
            destinoMontePacoteModel.Saida = DataOrigem;
            destinoMontePacoteModel.Retorno = DataDestino;

            model.Destinos.Add(destinoMontePacoteModel);
            MPSession = model;
            return View("Index", model);
        }

        public ActionResult ReloadIndex()
        {
            if (MPSession.Aereo.FirstOrDefault() != null && MPSession.Aereo.FirstOrDefault().Trechos.Count == 0)
            {
                MPSession.Aereo = new List<AereoModel>();

                RecalcularOpcoesVoo(MPSession);
            }

            return View("Index", MPSession);
        }

        public ActionResult MPAdicionarCarro(string indiceDestino, string horarioSaidaSelecionado, string horarioRetornoSelecionado, string localDevolucao, string codigoLocalDevolucao, string localRetirada, string codigoLocalRetirada, string dataSaida, string dataRetorno)
        {
            Session["IndiceAdicionarCarro"] = indiceDestino;

            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indiceDestino)];

            destinoMontePacote.Carro.DataCheckIn = dataSaida;
            destinoMontePacote.Carro.DataCheckOut = dataRetorno;
            destinoMontePacote.Carro.LocalRetirada = localRetirada;
            destinoMontePacote.Carro.LocalDevolucao = localDevolucao;
            destinoMontePacote.Carro.HoraCheckInSelecionada = horarioSaidaSelecionado;
            destinoMontePacote.Carro.HoraCheckOutSelecionada = horarioRetornoSelecionado;
            destinoMontePacote.Carro.IsLocalDiferenteEntrega = false;

            MPSession.Destinos[Convert.ToInt32(indiceDestino)] = destinoMontePacote;

            return RedirectToAction("ResultadoMontePacote", "Carro", new { indiceDestino, cidade = destinoMontePacote.Carro.LocalRetirada, codigoLocalDevolucao, codigoLocalRetirada });
        }

        public JsonResult AdicionarCarroMontePacote(string codigoTarifa)
        {
            var AddflyTour = AppCache.Get<List<ReservaCarroModel>>(EnumProcesso.CarroVenda, SessionKey + "Carro", 90, () =>
            {
                return new List<ReservaCarroModel>();
            });

            var reservaCarro = AddflyTour.Where(c => c.CodigoTarifa == codigoTarifa).FirstOrDefault();

            MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarCarro"])].ReservaCarro = reservaCarro;

            return Json("ok");
        }

        public JsonResult RemoverCarroMontePacote(string indice)
        {
            MPSession.Destinos[Convert.ToInt32(indice)].ReservaCarro = new ReservaCarroModel();

            return Json("ok");
        }

        public ActionResult MPAdicionarAtividade(string indice)
        {
            Session["IndiceAdicionarAtividade"] = indice;

            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            //string CodigoCidadeOrigem = destinoMontePacote.CodigoCidadeOrigem;
            string CodigoCidadeDestino = destinoMontePacote.CodigoCidadeDestino;
            //var hotelBussiness = new HotelBusiness();
            //List<HotelLocalidadeModel> lstCidadeDesembarque = hotelBussiness.ConsultarLocalizacaoHotel();

            //string codigoCidadeDesembarque = "";

            //if (lstCidadeDesembarque.Count > 0)
            //{
            //    foreach (var item in lstCidadeDesembarque)
            //    {
            //        if (item.label == destinoMontePacote.NomeCidadeDestino)
            //        {
            //            codigoCidadeDesembarque = item.codigo;
            //        }
            //    }
            //}

            //if (codigoCidadeDesembarque == "")
            //{
            //    codigoCidadeDesembarque = lstCidadeDesembarque[0].codigo;
            //}

            AtividadeModel atividadeModel = new AtividadeModel();

            atividadeModel.dtEmbarque = destinoMontePacote.Saida;
            atividadeModel.dtRetorno = destinoMontePacote.Retorno;
            atividadeModel.qtdAdultos = (MPSession.QuantAdultos.HasValue) ? MPSession.QuantAdultos.Value : 1;
            atividadeModel.qtdCriancas = (MPSession.QuantCriancas.HasValue) ? MPSession.QuantCriancas.Value : 0;
            atividadeModel.DestinoAtividade = CodigoCidadeDestino;
            atividadeModel.Apartamento = MPSession.Apartamentos;
            //destinoMontePacote.Opcionais.Add(atividadeModel);

            MPSession.Destinos[Convert.ToInt32(indice)] = destinoMontePacote;

            return RedirectToAction("LoadGridMontePacote", "Atividade", atividadeModel);
        }

        public JsonResult AdicionarAtividadeMontePacote(AtividadeModel atividadeItem)
        {
            MontePacoteModel montePacoteModel = MPSession;

            //montePacoteModel.Apartamentos = new List<ApartamentoModel>();
            //ApartamentoModel apartamentoModel = new ApartamentoModel();
            //apartamentoModel.Passageiros = new List<PassageiroModel>();
            //if (atividadeItem.Transfer != null)
            //{

            //    foreach (var transfer in atividadeItem.Transfer)
            //    {
            //        foreach (var apto in transfer.Apartamento)
            //        {
            //            foreach (var pax in apto.Passageiros)
            //            {
            //                PassageiroModel passageiroModel = new PassageiroModel();
            //                passageiroModel.IdadePassageiro = pax.IdadePassageiro;
            //                apartamentoModel.Passageiros.Add(passageiroModel);
            //            }
            //        }
            //    }

            //}

            DestinoMontePacoteModel destinoMontePacoteModel = montePacoteModel.Destinos[Convert.ToInt32(Session["IndiceAdicionarAtividade"])];

            destinoMontePacoteModel.Opcionais.Add(atividadeItem);
            //MPSession.Apartamentos.Add(apartamentoModel);

            return Json(SessionKey);
        }

        public JsonResult AdicionarAtividadeTransferMontePacote(string codigoAtividade, string Valor, string Dia, string DiaRetorno, string entrada, long? parcelas)
        {
            var addedFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));

            AtividadeModel transfers = new AtividadeModel();

            var transfer = new AtividadeModel();

            foreach (var item in addedFlytour)
            {
                var transferCache = item.Transfer.Where(c => c.CodigoAtividade == Convert.ToInt64(codigoAtividade)).FirstOrDefault();

                if (transferCache != null)
                {
                    //transferCache.ValorTotal = Conv.ToDecimal(Valor);
                    transferCache.dtEmbarque = Dia;
                    transferCache.dtRetorno = DiaRetorno;

                    transfer.Transfer.Add(transferCache);

                    break;
                }
            }

            transfers.Transfer = transfer.Transfer;

            var adaptandoStringValor = Valor.Split(',');
            var valorFormatado = String.Format("{0},{1}", adaptandoStringValor[0].Trim(), adaptandoStringValor[1].Trim());

            transfers.Transfer[0].ValorTotal = Conv.ToDecimal(valorFormatado);
            transfers.Transfer[0].ValorTotal = Decimal.Multiply(Conv.ToDecimal(valorFormatado), transfer.Transfer.FirstOrDefault().Apartamento.SelectMany(p => p.Passageiros).Count());

            if (parcelas != null)
            {
                transfers.Transfer[0].ValorTotal = Decimal.Multiply(transfers.Transfer[0].ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                transfers.Transfer[0].ValorTotal = transfers.Transfer[0].ValorTotal + Convert.ToDecimal(entrada);
            }

            transfers.dtEmbarque = Dia;
            transfers.Transfer[0].dtEmbarque = Dia;

            if (DiaRetorno != null)
            {
                transfers.dtRetorno = DiaRetorno;
            }

            AdicionarAtividadeMontePacote(transfer);

            return Json(SessionKey);
        }

        public JsonResult AdicionarAtividadeAssistenciaMontePacote(string codigoAtividade, string Valor, string Dia, string DiaRetorno, string entrada, long? parcelas)
        {
            var addedFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            AtividadeModel transfer = new AtividadeModel();

            var atividade = new AtividadeModel();

            foreach (var item in addedFlytour)
            {
                var assistenciaViagem = item.AssistenciaViagem.Where(c => c.CodigoAtividade == Convert.ToInt64(codigoAtividade)).FirstOrDefault();

                if (assistenciaViagem != null)
                {
                    //assistenciaViagem.ValorTotal = Conv.ToDecimal(Valor);
                    assistenciaViagem.dtEmbarque = Dia;
                    assistenciaViagem.dtRetorno = DiaRetorno;

                    atividade.AssistenciaViagem.Add(assistenciaViagem);

                    break;
                }
            }

            atividade.AssistenciaViagem = atividade.AssistenciaViagem;
            atividade.AssistenciaViagem[0].ValorTotal = Conv.ToDecimal(Valor);
            atividade.AssistenciaViagem[0].ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), atividade.AssistenciaViagem.FirstOrDefault().Apartamento.SelectMany(p => p.Passageiros).Count());

            if (parcelas != null)
            {
                atividade.AssistenciaViagem[0].ValorTotal = Decimal.Multiply(atividade.AssistenciaViagem[0].ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                atividade.AssistenciaViagem[0].ValorTotal = atividade.AssistenciaViagem[0].ValorTotal + Convert.ToDecimal(entrada);
            }



            if (DiaRetorno != null)
            {
                atividade.dtRetorno = DiaRetorno;
            }

            //var transfer = addedFlytour.Where(c => c.AssistenciaViagem.Where(a => a.CodigoAtividade == Convert.ToInt64(codigoAtividade)).ToList().Count > 0).First();

            AdicionarAtividadeMontePacote(atividade);

            return Json(SessionKey);
        }

        public JsonResult AdicionarAtividadeIngressoMontePacote(string codigoAtividade, string Valor, string Dia, string DiaRetorno, string entrada, long? parcelas)
        {
            var addedFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });



            var atividade = new AtividadeModel();

            foreach (var item in addedFlytour)
            {
                var ingresso = item.Ingresso.Where(c => c.CodigoAtividade == Convert.ToInt64(codigoAtividade)).FirstOrDefault();

                if (ingresso != null)
                {
                    //ingresso.ValorTotal = Conv.ToDecimal(Valor);
                    ingresso.dtEmbarque = Dia;
                    ingresso.dtRetorno = DiaRetorno;

                    atividade.Ingresso.Add(ingresso);

                    break;
                }
            }


            atividade.Ingresso[0].ValorTotal = Conv.ToDecimal(Valor);
            atividade.Ingresso[0].ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), atividade.Ingresso.FirstOrDefault().Apartamento.SelectMany(p => p.Passageiros).Count());

            if (parcelas != null)
            {
                atividade.Ingresso[0].ValorTotal = Decimal.Multiply(atividade.Ingresso[0].ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                atividade.Ingresso[0].ValorTotal = atividade.Ingresso[0].ValorTotal + Convert.ToDecimal(entrada);
            }

            atividade.dtEmbarque = Dia;
            atividade.Ingresso[0].dtEmbarque = Dia;

            if (DiaRetorno != null)
            {
                atividade.dtRetorno = DiaRetorno;
            }

            AdicionarAtividadeMontePacote(atividade);

            return Json(SessionKey);
        }

        public JsonResult AdicionarAtividadePasseioMontePacote(string codigoAtividade, string Valor, string Dia, string DiaRetorno, string entrada, long? parcelas)
        {
            var addedFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            AtividadeModel transfer = new AtividadeModel();

            var atividade = new AtividadeModel();

            foreach (var item in addedFlytour)
            {
                var passeio = item.Passeio.Where(c => c.CodigoAtividade == Convert.ToInt64(codigoAtividade)).FirstOrDefault();

                if (passeio != null)
                {
                    //passeio.ValorTotal = Conv.ToDecimal(Valor);
                    passeio.dtEmbarque = Dia;
                    passeio.dtRetorno = DiaRetorno;

                    atividade.Passeio.Add(passeio);

                    break;
                }
            }

            atividade.Passeio[0].ValorTotal = Conv.ToDecimal(Valor);
            atividade.Passeio[0].ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), atividade.Passeio.FirstOrDefault().Apartamento.SelectMany(p => p.Passageiros).Count());


            if (parcelas != null)
            {
                atividade.Passeio[0].ValorTotal = Decimal.Multiply(atividade.Passeio[0].ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                atividade.Passeio[0].ValorTotal = atividade.Passeio[0].ValorTotal + Convert.ToDecimal(entrada);
            }

            atividade.dtEmbarque = Dia;
            atividade.Passeio[0].dtEmbarque = Dia;

            if (DiaRetorno != null)
            {
                atividade.dtRetorno = DiaRetorno;
            }

            AdicionarAtividadeMontePacote(atividade);

            return Json(SessionKey);
        }

        public JsonResult AdicionarAtividadeRestauranteMontePacote(string codigoAtividade, string Valor, string Dia, string DiaRetorno, string entrada, long? parcelas)
        {
            var addedFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            AtividadeModel transfer = new AtividadeModel();

            var atividade = new AtividadeModel();

            foreach (var item in addedFlytour)
            {
                var restaurante = item.Restaurante.Where(c => c.CodigoAtividade == Convert.ToInt64(codigoAtividade)).FirstOrDefault();

                if (restaurante != null)
                {
                    //restaurante.ValorTotal = Conv.ToDecimal(Valor);
                    restaurante.dtEmbarque = Dia;
                    restaurante.dtRetorno = DiaRetorno;

                    atividade.Restaurante.Add(restaurante);

                    break;
                }
            }


            atividade.Restaurante[0].ValorTotal = Conv.ToDecimal(Valor);
            atividade.Restaurante[0].ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), atividade.Restaurante.FirstOrDefault().Apartamento.SelectMany(p => p.Passageiros).Count());

            if (parcelas != null)
            {
                atividade.Restaurante[0].ValorTotal = Decimal.Multiply(atividade.Restaurante[0].ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                atividade.Restaurante[0].ValorTotal = atividade.Restaurante[0].ValorTotal + Convert.ToDecimal(entrada);
            }

            atividade.dtEmbarque = Dia;
            atividade.Restaurante[0].dtEmbarque = Dia;

            if (DiaRetorno != null)
            {
                atividade.dtRetorno = DiaRetorno;
            }

            AdicionarAtividadeMontePacote(atividade);

            return Json(SessionKey);
        }

        public JsonResult AdicionarAtividadePacoteAtividadeMontePacote(string codigoAtividade, string Valor, string Dia, string DiaRetorno, string entrada, long? parcelas)
        {
            var addedFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            AtividadeModel transfer = new AtividadeModel();

            var atividade = new AtividadeModel();

            foreach (var item in addedFlytour)
            {
                var pacoteAtividade = item.PacoteAtividade.Where(c => c.CodigoAtividade == Convert.ToInt64(codigoAtividade)).FirstOrDefault();

                if (pacoteAtividade != null)
                {
                    //pacoteAtividade.ValorTotal = Conv.ToDecimal(Valor);
                    pacoteAtividade.dtEmbarque = Dia;
                    pacoteAtividade.dtRetorno = DiaRetorno;

                    atividade.PacoteAtividade.Add(pacoteAtividade);

                    break;
                }
            }

            atividade.PacoteAtividade[0].ValorTotal = Conv.ToDecimal(Valor);
            atividade.PacoteAtividade[0].ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), atividade.PacoteAtividade.FirstOrDefault().Apartamento.SelectMany(p => p.Passageiros).Count());

            if (parcelas != null)
            {
                atividade.PacoteAtividade[0].ValorTotal = Decimal.Multiply(atividade.PacoteAtividade[0].ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                atividade.PacoteAtividade[0].ValorTotal = atividade.PacoteAtividade[0].ValorTotal + Convert.ToDecimal(entrada);
            }


            atividade.dtEmbarque = Dia;
            atividade.PacoteAtividade[0].dtEmbarque = Dia;
            if (DiaRetorno != null)
            {
                atividade.dtRetorno = DiaRetorno;
            }

            AdicionarAtividadeMontePacote(atividade);

            return Json(SessionKey);
        }

        public JsonResult RemoverTraslado(string codigoAtividade, string indice, string indiceAtividades)
        {
            AtividadeModel atividadeModel = MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)];

            TransferModel transferModel = atividadeModel.Transfer.Where(t => t.CodigoAtividade.Value == Convert.ToInt64(codigoAtividade)).First();

            atividadeModel.Transfer.Remove(transferModel);

            MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)] = atividadeModel;

            if (VerificarRemoverOpcionaisDestino(indice, atividadeModel))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Opcionais.Remove(atividadeModel);
            }

            return Json(SessionKey);
        }

        public JsonResult RemoverAssistenciaViagem(string codigoAtividade, string indice, string indiceAtividades)
        {
            AtividadeModel atividadeModel = MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)];

            AssistenciaViagemModel assistenciaViagemMode = atividadeModel.AssistenciaViagem.Where(a => a.CodigoAtividade.Value == Convert.ToInt64(codigoAtividade)).First();

            atividadeModel.AssistenciaViagem.Remove(assistenciaViagemMode);

            MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)] = atividadeModel;

            if (VerificarRemoverOpcionaisDestino(indice, atividadeModel))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Opcionais.Remove(atividadeModel);
            }

            return Json(SessionKey);
        }

        public JsonResult RemoverPasseio(string codigoAtividade, string indice, string indiceAtividades)
        {
            AtividadeModel atividadeModel = MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)];

            PasseioModel passeioModel = atividadeModel.Passeio.Where(p => p.CodigoAtividade.Value == Convert.ToInt64(codigoAtividade)).First();

            atividadeModel.Passeio.Remove(passeioModel);

            MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)] = atividadeModel;

            if (VerificarRemoverOpcionaisDestino(indice, atividadeModel))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Opcionais.Remove(atividadeModel);
            }

            return Json(SessionKey);
        }

        public JsonResult RemoverIngresso(string codigoAtividade, string indice, string indiceAtividades)
        {
            AtividadeModel atividadeModel = MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)];

            IngressoModel ingressoModel = atividadeModel.Ingresso.Where(p => p.CodigoAtividade.Value == Convert.ToInt64(codigoAtividade)).First();

            atividadeModel.Ingresso.Remove(ingressoModel);

            MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)] = atividadeModel;

            if (VerificarRemoverOpcionaisDestino(indice, atividadeModel))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Opcionais.Remove(atividadeModel);
            }

            return Json(SessionKey);
        }

        public JsonResult RemoverPacoteAtividade(string codigoAtividade, string indice, string indiceAtividades)
        {
            AtividadeModel atividadeModel = MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)];

            PacoteAtividadeModel pacoteAtividadeModel = atividadeModel.PacoteAtividade.Where(p => p.CodigoAtividade.Value == Convert.ToInt64(codigoAtividade)).First();

            atividadeModel.PacoteAtividade.Remove(pacoteAtividadeModel);

            MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)] = atividadeModel;

            if (VerificarRemoverOpcionaisDestino(indice, atividadeModel))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Opcionais.Remove(atividadeModel);
            }

            return Json(SessionKey);
        }

        public JsonResult RemoverRestaurante(string codigoAtividade, string indice, string indiceAtividades)
        {
            AtividadeModel atividadeModel = MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)];

            RestauranteModel restauranteModel = atividadeModel.Restaurante.Where(p => p.CodigoAtividade.Value == Convert.ToInt64(codigoAtividade)).First();

            atividadeModel.Restaurante.Remove(restauranteModel);

            MPSession.Destinos[Convert.ToInt32(indice)].Opcionais[Convert.ToInt32(indiceAtividades)] = atividadeModel;

            if (VerificarRemoverOpcionaisDestino(indice, atividadeModel))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Opcionais.Remove(atividadeModel);
            }

            return Json(SessionKey);
        }

        private bool VerificarRemoverOpcionaisDestino(string indice, AtividadeModel atividadeModel)
        {

            return (atividadeModel.Transfer.Count == 0 && atividadeModel.AssistenciaViagem.Count == 0 && atividadeModel.Passeio.Count == 0
                && atividadeModel.Ingresso.Count == 0 && atividadeModel.PacoteAtividade.Count == 0 && atividadeModel.Restaurante.Count == 0);
        }

        public ActionResult AdicionarHotel(string indice, string origemMP, DateTime? dataSaida, DateTime? dataChegada)
        {
            Session["IndiceAdicionarHotel"] = indice;

            Session["HoteilSessionPaginacaoFiltrado"] = null;

            Session["ConsultaFinalizadaMP"] = null;

            return RedirectToAction("ResultadoMontePacote", "Hotel", new { origem = origemMP, hdnDestinoHotelCodigo = MPSession.Destinos[Convert.ToInt32(indice)].CodigoCidadeDestino, dataSaida = dataSaida, dataChegada = dataChegada });
        }

        public ActionResult AdicionarHotelDatasAlteradas(string indice, string origemMP)
        {
            Session["IndiceAdicionarHotel"] = indice;

            Session["HoteilSessionPaginacaoFiltrado"] = null;

            Session["HoteilSessionPaginacaoFiltrado"] = null;

            if (String.IsNullOrEmpty(MPSession.Destinos[Convert.ToInt32(indice)].Hotel.Checkin))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Hotel.Checkin = MPSession.Destinos[Convert.ToInt32(indice)].Saida;
            }

            if (String.IsNullOrEmpty(MPSession.Destinos[Convert.ToInt32(indice)].Hotel.Checkout))
            {
                MPSession.Destinos[Convert.ToInt32(indice)].Hotel.Checkout = MPSession.Destinos[Convert.ToInt32(indice)].Retorno;
            }

            Nullable<DateTime> dataSaida = Convert.ToDateTime(MPSession.Destinos[Convert.ToInt32(indice)].Hotel.Checkin);
            Nullable<DateTime> dataChegada = Convert.ToDateTime(MPSession.Destinos[Convert.ToInt32(indice)].Hotel.Checkout);

            return RedirectToAction("ResultadoMontePacote", "Hotel", new { hdnDestinohotelCodigo = MPSession.Destinos[Convert.ToInt32(indice)].CodigoCidadeDestino, dataSaida = dataSaida, dataChegada = dataChegada });
        }

        public JsonResult VerificarHorarioHotel(string indice)
        {
            bool sugerirNovaData = false;

            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            Trecho trechoDestino = MPSession.Aereo.First().Trechos.Where(t => destinoMontePacote.NomeCidadeDestino.Contains(t.NomeCidadeDestino)).FirstOrDefault();

            if (trechoDestino != null)
            {
                TimeSpan horaChegada = new TimeSpan(trechoDestino.DataDestino.Hour, trechoDestino.DataDestino.Minute, trechoDestino.DataDestino.Second);

                string[] horaMinimaCheckin = ConfigurationManager.AppSettings["HorarioMinimoCheckIn"].Split(':');

                TimeSpan timeSpanhoraMinimaCheckin = new TimeSpan(Convert.ToInt32(horaMinimaCheckin[0]), Convert.ToInt32(horaMinimaCheckin[1]), Convert.ToInt32(horaMinimaCheckin[2]));

                int retornoComparacao = TimeSpan.Compare(horaChegada, timeSpanhoraMinimaCheckin);

                if (retornoComparacao <= 0)
                {
                    sugerirNovaData = true;
                }
            }

            if (!sugerirNovaData)
            {
                if (MPSession.Aereo.First().Tipo.ToLower() == "idaevolta")
                {
                    Trecho trechoVolta = MPSession.Aereo.First().Trechos.Last();

                    TimeSpan horaSaida = new TimeSpan(trechoVolta.DataOrigem.Hour, trechoVolta.DataOrigem.Minute, trechoVolta.DataOrigem.Second);

                    string[] horaMaximaCheckOut = ConfigurationManager.AppSettings["HorarioMaximoCheckOut"].Split(':');

                    TimeSpan timeSpanHoraMaximaCheckout = new TimeSpan(Convert.ToInt32(horaMaximaCheckOut[0]), Convert.ToInt32(horaMaximaCheckOut[1]), Convert.ToInt32(horaMaximaCheckOut[2]));

                    int retornoComparacao = TimeSpan.Compare(horaSaida, timeSpanHoraMaximaCheckout);

                    if (retornoComparacao >= 0)
                    {
                        sugerirNovaData = true;
                    }
                }
                else if (MPSession.Aereo.First().Tipo.ToLower() == "multitrecho")
                {
                    Trecho proximoTrecho = MPSession.Aereo.First().Trechos.Where(t => destinoMontePacote.NomeCidadeDestino.Contains(t.NomeCidadeOrigem)).FirstOrDefault();

                    if (proximoTrecho != null)
                    {
                        TimeSpan horaSaida = new TimeSpan(proximoTrecho.DataOrigem.Hour, proximoTrecho.DataOrigem.Minute, proximoTrecho.DataOrigem.Second);

                        string[] horaMaximaCheckOut = ConfigurationManager.AppSettings["HorarioMaximoCheckOut"].Split(':');

                        TimeSpan timeSpanHoraMaximaCheckout = new TimeSpan(Convert.ToInt32(horaMaximaCheckOut[0]), Convert.ToInt32(horaMaximaCheckOut[1]), Convert.ToInt32(horaMaximaCheckOut[2]));

                        int retornoComparacao = TimeSpan.Compare(horaSaida, timeSpanHoraMaximaCheckout);

                        if (retornoComparacao >= 0)
                        {
                            sugerirNovaData = true;
                        }
                    }
                }
            }

            return Json(sugerirNovaData);
        }

        public JsonResult VerificarHorarioCheckinHotel(string indice)
        {
            bool sugerirNovaData = false;

            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            Trecho trechoDestino = MPSession.Aereo.First().Trechos.FirstOrDefault(t => destinoMontePacote.NomeCidadeDestino.Contains(t.NomeCidadeDestino));

            var dataSaida = Conv.ToDate(destinoMontePacote.Saida);
            if (trechoDestino != null && (dataSaida != null && trechoDestino.DataDestino.Date > dataSaida.Value.Date))
            {
                destinoMontePacote.Hotel.Checkin = trechoDestino.DataDestino.ToString("dd/MM/yyyy");
                MPSession.Destinos[Convert.ToInt32(indice)].Hotel = destinoMontePacote.Hotel;
            }
            else
            {
                if (dataSaida != null) destinoMontePacote.Hotel.Checkin = dataSaida.Value.ToShortDateString();
            }

            if (trechoDestino != null)
            {
                TimeSpan horaChegada = new TimeSpan(trechoDestino.DataDestino.Hour, trechoDestino.DataDestino.Minute, trechoDestino.DataDestino.Second);

                string[] horaMinimaCheckin = ConfigurationManager.AppSettings["HorarioMinimoCheckIn"].Split(':');

                TimeSpan timeSpanHoraMinimaCheckin = new TimeSpan(Convert.ToInt32(horaMinimaCheckin[0]), Convert.ToInt32(horaMinimaCheckin[1]), Convert.ToInt32(horaMinimaCheckin[2]));

                int retornoComparacao = TimeSpan.Compare(horaChegada, timeSpanHoraMinimaCheckin);

                if (retornoComparacao <= 0)
                {
                    sugerirNovaData = true;
                }
            }

            return Json(sugerirNovaData);
        }

        public JsonResult VerificarHorarioCheckoutHotel(string indice)
        {
            bool sugerirNovaData = false;

            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            if (MPSession.Aereo.First().Tipo.ToLower() == "idaevolta")
            {
                Trecho trechoVolta = MPSession.Aereo.First().Trechos.Last();

                TimeSpan horaSaida = new TimeSpan(trechoVolta.DataOrigem.Hour, trechoVolta.DataOrigem.Minute, trechoVolta.DataOrigem.Second);

                string[] horaMaximaCheckOut = ConfigurationManager.AppSettings["HorarioMaximoCheckOut"].Split(':');

                TimeSpan timeSpanHoraMaximaCheckout = new TimeSpan(Convert.ToInt32(horaMaximaCheckOut[0]), Convert.ToInt32(horaMaximaCheckOut[1]), Convert.ToInt32(horaMaximaCheckOut[2]));

                int retornoComparacao = TimeSpan.Compare(horaSaida, timeSpanHoraMaximaCheckout);

                if (retornoComparacao >= 0)
                {
                    sugerirNovaData = true;
                }
            }
            else if (MPSession.Aereo.First().Tipo.ToLower() == "multitrecho")
            {
                Trecho proximoTrecho = MPSession.Aereo.First().Trechos.Where(t => destinoMontePacote.NomeCidadeDestino.Contains(t.NomeCidadeOrigem)).FirstOrDefault();

                if (proximoTrecho != null)
                {
                    TimeSpan horaSaida = new TimeSpan(proximoTrecho.DataOrigem.Hour, proximoTrecho.DataOrigem.Minute, proximoTrecho.DataOrigem.Second);

                    string[] horaMaximaCheckOut = ConfigurationManager.AppSettings["HorarioMaximoCheckOut"].Split(':');

                    TimeSpan timeSpanHoraMaximaCheckout = new TimeSpan(Convert.ToInt32(horaMaximaCheckOut[0]), Convert.ToInt32(horaMaximaCheckOut[1]), Convert.ToInt32(horaMaximaCheckOut[2]));

                    int retornoComparacao = TimeSpan.Compare(horaSaida, timeSpanHoraMaximaCheckout);

                    if (retornoComparacao >= 0)
                    {
                        sugerirNovaData = true;
                    }
                }
            }

            return Json(sugerirNovaData);
        }

        public JsonResult AdicionarDiaCheckin(string indice)
        {
            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            // Trecho trechoDestino = MPSession.Aereo.First().Trechos.Where(t => destinoMontePacote.NomeCidadeDestino.Contains(t.NomeCidadeDestino)).FirstOrDefault();

            var dateTime = Conv.ToDate(destinoMontePacote.Hotel.Checkin);
            if (dateTime != null)
            {
                DateTime novaDataCheckin = dateTime.Value;
                destinoMontePacote.Hotel.Checkin = novaDataCheckin.AddDays(-1).ToString("dd/MM/yyyy");
            }

            MPSession.Destinos[Convert.ToInt32(indice)].Hotel = destinoMontePacote.Hotel;

            return Json(true);
        }

        public JsonResult AdicionarDiaCheckout(string indice)
        {
            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(indice)];

            if (MPSession.Aereo.First().Tipo.ToLower() == "idaevolta")
            {
                Trecho trechoVolta = MPSession.Aereo.First().Trechos.Last();

                DateTime novaDataCheckOut = trechoVolta.DataOrigem.AddDays(1);

                destinoMontePacote.Hotel.Checkout = novaDataCheckOut.ToString("dd/MM/yyyy");

                MPSession.Destinos[Convert.ToInt32(indice)].Hotel = destinoMontePacote.Hotel;
            }
            else if (MPSession.Aereo.First().Tipo.ToLower() == "multitrecho")
            {
                Trecho proximoTrecho = MPSession.Aereo.First().Trechos.Where(t => destinoMontePacote.NomeCidadeDestino.Contains(t.NomeCidadeOrigem)).FirstOrDefault();

                if (proximoTrecho != null)
                {
                    DateTime novaDataCheckout = proximoTrecho.DataOrigem.AddDays(1);

                    destinoMontePacote.Hotel.Checkout = novaDataCheckout.ToString("dd/MM/yyyy");

                    MPSession.Destinos[Convert.ToInt32(indice)].Hotel = destinoMontePacote.Hotel;
                }
            }

            return Json(true);
        }

        public JsonResult AdicionarHotelMontePacote(HotelModel montePacoteItem, string NomeAcomodacao, string CodigoTarifa)
        {
            var CodigoAcomocacao = montePacoteItem.Acomodacoes.Where(p => p.NomeAcomodacao == NomeAcomodacao).FirstOrDefault().CodigoAcomodacao;

            DestinoMontePacoteModel destinoMontePacote = MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])];

            var codigo = Conv.ToInt64(CodigoTarifa);
            montePacoteItem.CodigoTarifa = CodigoTarifa;
            montePacoteItem.ValorTotal = montePacoteItem.Acomodacoes.SelectMany(p => p.Tarifas).First(v => v.CodigoTarifa == codigo).ValorTotal;
            montePacoteItem.ValorCusto = montePacoteItem.Acomodacoes.SelectMany(p => p.Tarifas).First(v => v.CodigoTarifa == codigo).ValorCusto;

            MPSession.Apartamentos = montePacoteItem.Apartamentos;

            destinoMontePacote.Hotel = montePacoteItem;

            destinoMontePacote.NomeAcomodacaoHotel = NomeAcomodacao;
            destinoMontePacote.CodigoTarifaHotel = CodigoTarifa;

            MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])] = destinoMontePacote;

            return Json(SessionKey);
        }

        public JsonResult RemoverHotelMontePacote(string CodigoTarifa, string Indice)
        {
            MPSession.Destinos[Convert.ToInt32(Indice)].Hotel = new HotelModel();

            return Json(SessionKey);
        }

        public ActionResult AdicionarAereo(string indiceDestino, string isIdaVolta)
        {
            Session["IndiceAdicionarAereo"] = indiceDestino;

            MontePacoteModel montePacoteModel = MPSession;

            decimal codigoCidadeEmbarque = Conv.ToDecimal(montePacoteModel.Destinos.LastOrDefault().CodigoCidadeOrigem);
            decimal codigoCidadeDesembarque = Conv.ToDecimal(montePacoteModel.Destinos.LastOrDefault().CodigoCidadeDestino);
            List<LocaisRetorno> lstCidadeEmbarque;
            List<LocaisRetorno> lstCidadeDesembarque;

            //List<DataJson> lstCidadeEmbarque;
            //List<DataJson> lstCidadeDesembarque;

            if (indiceDestino == "0")
            {
                //lstCidadeEmbarque = new ListaAutoComplete(nomeCidadeEmbarque).Get(10, new List<string>());
                //lstCidadeDesembarque = new ListaAutoComplete(nomeCidadeDesembarque).Get(5, new List<string>());

                var listaAeroportosOrigem = AereoBusiness.ConsultaOrigem(true);
                var listaAeroportosDestino = AereoBusiness.ConsultaDestino();
                lstCidadeEmbarque = listaAeroportosOrigem.Where(p => p.CodigoCidade == (codigoCidadeEmbarque)).ToList();
                lstCidadeDesembarque = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeDesembarque)).ToList();

                var teste = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeDesembarque)).FirstOrDefault();

            }
            else
            {
                //lstCidadeEmbarque = new ListaAutoComplete(nomeCidadeEmbarque).Get(4, new List<string>());
                //lstCidadeDesembarque = new ListaAutoComplete(nomeCidadeDesembarque).Get(5, new List<string>());

                var listaAeroportosOrigem = AereoBusiness.ConsultaOrigem();
                var listaAeroportosDestino = AereoBusiness.ConsultaDestino();
                lstCidadeEmbarque = listaAeroportosOrigem.Where(p => p.CodigoCidade == (codigoCidadeEmbarque)).ToList();
                lstCidadeDesembarque = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeDesembarque)).ToList();

            }

            //DataJson ultimaCidadeEmbarque = lstCidadeEmbarque.LastOrDefault();
            //DataJson ultimaCidadeDesembarque = lstCidadeDesembarque.LastOrDefault();

            LocaisRetorno ultimaCidadeEmbarque = lstCidadeEmbarque.LastOrDefault();
            LocaisRetorno ultimaCidadeDesembarque = lstCidadeDesembarque.LastOrDefault();

            montePacoteModel.Aereo.Add(new AereoModel());

            montePacoteModel.Aereo.LastOrDefault().DataEmbarque = montePacoteModel.Destinos.LastOrDefault().Saida;
            montePacoteModel.Aereo.LastOrDefault().QuantidadeAdulto = montePacoteModel.QuantAdultos.Value;
            montePacoteModel.Aereo.LastOrDefault().QuantidadeCrianca = montePacoteModel.QuantCriancas.Value;

            if (Convert.ToBoolean(isIdaVolta))
            {
                montePacoteModel.Aereo.LastOrDefault().DataRetorno = montePacoteModel.Destinos.LastOrDefault().Retorno;
                montePacoteModel.Aereo.LastOrDefault().Tipo = "IdaeVolta";
            }
            else
            {
                montePacoteModel.Aereo.LastOrDefault().DataRetorno = montePacoteModel.Destinos.LastOrDefault().Retorno;
                montePacoteModel.Aereo.LastOrDefault().Tipo = "SomenteIda";
            }


            return RedirectToAction("LoadAereoMontePacote", "Aereo", new
            {
                horarioIdaSelect = "Qualquer",
                horarioVoltaSelect = "Qualquer",
                classeSelect = "Qualquer",
                somenteDireto = "",
                hdnDestinoCodigoOld = "",
                hdnOrigemCodigoOld = "",
                hdnDestinoNomeOld = "",
                hdnOrigemNomeOld = "",
                hdnDestinoCodigo = ultimaCidadeDesembarque.SiglaAeroporto,
                hdnOrigemCodigo = ultimaCidadeEmbarque.SiglaAeroporto,
                hdnDestinoNome = ultimaCidadeDesembarque.NomeAeroporto,
                hdnOrigemNome = ultimaCidadeEmbarque.NomeAeroporto,
                ddlCriancas = montePacoteModel.QuantCriancas,
                ddlAdultos = montePacoteModel.QuantAdultos,
                ddlBebes = ""
            });
        }

        public ActionResult AdicionarAereoMultiTrecho(string trechos)
        {
            Session["IndiceAdicionarAereo"] = "0";

            string[] arrayTrechos = trechos.Replace("[", "").Replace("]", "").Split(',');

            bool isIdaVolta = false;

            for (int i = 0; i < arrayTrechos.Length; i++)
            {
                string[] arrayIndexIdaVolta = arrayTrechos[i].Split('|');

                MPSession.OpcoesAereo[i].IsSelected = arrayIndexIdaVolta[0].Contains("true");

                if (arrayIndexIdaVolta[1].Contains("true"))
                {
                    isIdaVolta = true;
                }
            }

            MontePacoteModel montePacoteModel = MPSession;
            List<LocaisRetorno> lstCidadeEmbarque;
            List<LocaisRetorno> lstCidadeDesembarque;
            decimal codigoCidadeEmbarque = 0;
            decimal codigoCidadeDesembarque = 0;

            if (isIdaVolta)
            {
                codigoCidadeEmbarque = Conv.ToDecimal(montePacoteModel.OpcoesAereo.Where(c => c.IsSelected).ToList().LastOrDefault().CodigoCidadeOrigem.Split(',')[0]);
                codigoCidadeDesembarque = Conv.ToDecimal(montePacoteModel.OpcoesAereo.Where(c => c.IsSelected).ToList().LastOrDefault().CodigoCidadeDestino.Split(',')[0]);
            }
            else
            {
                codigoCidadeEmbarque = Conv.ToDecimal(montePacoteModel.OpcoesAereo.LastOrDefault().CodigoCidadeOrigem.Split(',')[0]);
                codigoCidadeDesembarque = Conv.ToDecimal(montePacoteModel.OpcoesAereo.LastOrDefault().CodigoCidadeDestino.Split(',')[0]);
            }

            //var listaAeroportosOrigem = AereoBusiness.ConsultaOrigem(true);
            var listaAeroportosDestino = AereoBusiness.ConsultaDestino();
            lstCidadeEmbarque = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeEmbarque)).ToList();
            lstCidadeDesembarque = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeDesembarque)).ToList();

            LocaisRetorno ultimaCidadeEmbarque = lstCidadeEmbarque.LastOrDefault();
            LocaisRetorno ultimaCidadeDesembarque = lstCidadeDesembarque.LastOrDefault();

            montePacoteModel.Aereo.Clear();

            montePacoteModel.Aereo.Add(new AereoModel());

            if (isIdaVolta)
            {
                montePacoteModel.Aereo.First().Tipo = "IdaeVolta";
            }
            else
            {
                montePacoteModel.Aereo.First().Tipo = "SomenteIda";
            }

            return RedirectToAction("LoadAereoMultiTrechoMontePacote", "Aereo", new
            {
                horarioIdaSelect = "Qualquer",
                horarioVoltaSelect = "Qualquer",
                classeSelect = "Qualquer",
                somenteDireto = "",
                hdnDestinoCodigoOld = "",
                hdnOrigemCodigoOld = "",
                hdnDestinoNomeOld = "",
                hdnOrigemNomeOld = "",
                hdnDestinoCodigo = ultimaCidadeDesembarque.SiglaAeroporto,
                hdnOrigemCodigo = ultimaCidadeEmbarque.SiglaAeroporto,
                hdnDestinoNome = ultimaCidadeDesembarque.NomeAeroporto,
                hdnOrigemNome = ultimaCidadeDesembarque.NomeAeroporto,
                ddlCriancas = montePacoteModel.QuantCriancas,
                ddlAdultos = montePacoteModel.QuantAdultos,
                ddlBebes = ""
            });
        }

        public JsonResult AdicionarAereoMontePacote(AereoModel AereoModel)
        {
            bool aereoTripEngine = false;
            var configAereoTripEngine = ConfigurationManager.AppSettings["aereoTripEngine"];
            if (configAereoTripEngine != null)
                aereoTripEngine = Convert.ToBoolean(configAereoTripEngine);

            if (aereoTripEngine)
            {
                for (int i = 0; i < AereoModel.TrechosSel.Count; i++)
                {
                    AereoModel.CodigoTarifas.Add(AereoModel.TrechosSel[i]);
                }
            }
            else
            {
                var reco1 = AereoModel.TrechosSel[0].Split(';');
                if (AereoModel.TrechosSel.Count > 1)
                {

                    var reco2 = AereoModel.TrechosSel[1].Split(';');
                    var codigoTarifa = AereoModel.CodigoTarifa;

                    if (AereoModel.TrechosSel.Count > 2)
                    {
                        var reco3 = AereoModel.TrechosSel[2].Split(';');
                        var inter = reco1.Intersect(reco2).Intersect(reco3);
                        var intersec = inter.First(p => p != String.Empty);
                        AereoModel.Recomendation = Conv.ToInt32(intersec);
                    }
                    else
                    {

                        var inter = reco1.Intersect(reco2);
                        var intersec = inter.First(p => p != String.Empty);
                        AereoModel.Recomendation = Conv.ToInt32(intersec);
                    }
                }
                else
                {
                    var intersec = reco1.First();
                    AereoModel.Recomendation = Conv.ToInt32(intersec);
                }
            }


            var tarifa = AereoBusiness.Tarifar(AereoModel);

            var AddflyTour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            var trechosAer = AereoBusiness.MapearTrecho(tarifa.Trechos);
            AereoModel.ValorTaxa = tarifa.ValorTaxa;
            //AereoModel.Valor = Conv.ToDecimal((AereoModel.ValorTaxa + tarifa.ValorTotal).ToString("n2"));
            AereoModel.Valor = Conv.ToDecimal(tarifa.ValorVenda.ToString("n2"));
            AereoModel.Trechos.Clear();
            foreach (var item in trechosAer)
            {
                AereoModel.Trechos.Add(item);
            }

            //foreach (var item in AereoModel.Trechos)
            //{
            //    if(string.IsNullOrEmpty(item.NomeCidadeOrigem))
            //    {
            //        item.NomeCidadeOrigem = AereoModel.OrigemNome.Split('(')[0];
            //        item.NomeCidadeDestino = AereoModel.DestinoNome.Split('(')[0];
            //    }
            //}

            var apartamentos = new List<ApartamentoModel>();
            var apto = new ApartamentoModel();

            var flyTour = MPSession;

            if (AddflyTour.Apartamentos != null && AddflyTour.Apartamentos.Count == 0)
            {
                apto.Passageiros = new List<PassageiroModel>();
                for (int i = 0; i < AereoModel.QuantidadeAdulto; i++)
                {
                    var pax = new PassageiroModel();
                    pax.IdadePassageiro = 30;
                    apto.Passageiros.Add(pax);
                }

                for (int i = 0; i < AereoModel.QuantidadeCrianca; i++)
                {
                    var pax = new PassageiroModel();
                    pax.IdadePassageiro = 6;
                    apto.Passageiros.Add(pax);
                }

                for (int i = 0; i < AereoModel.QuantidadeBebe; i++)
                {
                    var pax = new PassageiroModel();
                    pax.IdadePassageiro = 1;
                    apto.Passageiros.Add(pax);
                }

                //flyTour.Apartamentos.Add(apto);
            }

            if (AereoModel.Trechos.Count > 0)
            {
                var remover = flyTour.Aereo.FirstOrDefault();

                flyTour.OpcoesAereo.Clear();

                MPSession.Aereo.Clear();

                MPSession.Aereo.Add(AereoModel);
            }

            return Json(SessionKey);
        }

        public JsonResult RemoverAereoMontePacote()
        {
            MPSession.Aereo.Clear();
            MPSession.OpcoesAereo.Clear();
            RecalcularOpcoesVoo(MPSession);

            return Json(SessionKey);
        }

        public PartialViewResult Filtrohotel()
        {
            var ResultadoHotel = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, SessionKey + "Hotel", 90, () =>
            {
                return new List<HotelModel>();
            });

            if (ResultadoHotel != null && ResultadoHotel.Count > 0)
            {
                Decimal? menos = ResultadoHotel.OrderBy(p => p.ValorDiaria).FirstOrDefault().ValorDiaria;
                Decimal? mais = ResultadoHotel.OrderByDescending(p => p.ValorDiaria).FirstOrDefault().ValorDiaria;
                ViewBag.MenorPreco = Math.Floor(Conv.ToDecimal(menos));
                ViewBag.MaiorPreco = Math.Floor(Conv.ToDecimal(mais));

                ViewBag.Dados = ResultadoHotel.FirstOrDefault();
                ViewBag.CodigoCidade = ResultadoHotel.FirstOrDefault().CodigoCidade;
                ViewBag.AdultosNum = ResultadoHotel.FirstOrDefault().QuantAdultos;
            }
            return PartialView(ResultadoHotel);
        }

        public PartialViewResult FiltroAereo()
        {
            AereoModel AereoRetorno = Session["AereoLista"] as AereoModel;
            return PartialView(AereoRetorno);
        }

        public ActionResult FiltrarAereo(AereoModel modelParametro)
        {
            NameObjectCollectionBase.KeysCollection Keys = Request.Form.Keys;
            FiltroAereoModel filtro = new FiltroAereoModel();
            filtro.combo = Request.Form["hdnCombo"];
            filtro.valor = Request.Form["hdnValorSelecionado"];
            filtro.amount = Request.Form["amount"];
            filtro.tipoFiltro = Request.Form["hdnTipoFiltro"];
            filtro.HorarioIdaMadrugada = Request.Form["HorarioIdaMadrugada"];
            filtro.EscalaVooDireto = Request.Form["EscalaVooDireto"];
            filtro.EscalaDuasParadas = Request.Form["EscalaDuasParadas"];
            filtro.EscalaUmaParada = Request.Form["EscalaUmaParada"];
            filtro.HorarioIdaManha = Request.Form["HorarioIdaManha"];
            filtro.HorarioIdaTarde = Request.Form["HorarioIdaTarde"];
            filtro.HorarioIdaNoite = Request.Form["HorarioIdaNoite"];

            Session["FiltroMP"] = filtro;
            Session["KeysMP"] = Keys;
            Session["ModelParametro"] = modelParametro;

            return RedirectToAction("FiltrarAereoMP", "Aereo");
        }

        public ActionResult FiltrarHotel(string nome, string menorPreco, string maiorPreco, string estrela,
            string lstFacilidades, string hotelBuscacheckin, string hotelBuscaCheckout, string order, string quantAdultos,
            string quantCriancas, string codigocidade)
        {
            Session["MPFiltro"] = true;

            return RedirectToAction("FiltroHotel", "Hotel", new
            {
                nome = nome,
                menorPreco = menorPreco,
                maiorPreco = maiorPreco,
                estrela = estrela,
                lstFacilidades = lstFacilidades,
                hotelBuscacheckin = hotelBuscacheckin,
                hotelBuscaCheckout = hotelBuscaCheckout,
                order = order,
                quantAdultos = quantAdultos,
                quantCriancas = quantCriancas,
                codigocidade = codigocidade
            });
        }

        public ActionResult MPFormMulti()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            CleanItensComprar(addedFlytour);

            return PartialView();
        }

        public PartialViewResult TrechosMP(int? indice)
        {
            if (indice != null)
                ViewBag.IndiceTrechoMP = indice;
            else
                ViewBag.IndiceTrechoMP = 1;

            return PartialView();
        }

        public PartialViewResult TrechosRemoverMP()
        {
            if (ViewBag.IndiceTrecho != null)
                ViewBag.IndiceTrecho = ViewBag.IndiceTrecho - 1;

            return null;
        }

        #region Métodos não utilizados

        [Obsolete]
        public ActionResult MPHotelVooForm(MontePacoteModel MP, string origemMP, string hdnOrigemMPCodigo, string hdnDestinoMPCodigo)
        {
            return RedirectToAction("ResultadoMontePacote", "Hotel", new { origem = origemMP, hdnDestinoHotelCodigo = hdnDestinoMPCodigo });
        }

        [Obsolete]
        public ActionResult MPVoo()
        {
            if (SessionMontePacote != null)
            {
                //if (SessionMontePacote.FirstOrDefault().tipoPacotes == "1" || SessionMontePacote.FirstOrDefault().tipoPacotes == "3")
                //{
                //    string nomeCidadeEmbarque = SessionMontePacote.FirstOrDefault().NomeCidadeOrigem.Split(',')[0];
                //    string nomeCidadeDesembarque = SessionMontePacote.FirstOrDefault().NomeCidadeDestino.Split(',')[0];

                //    List<DataJson> lstCidadeEmbarque = new ListaAutoComplete(nomeCidadeEmbarque).get(4, new List<string>());
                //    List<DataJson> lstCidadeDesembarque = new ListaAutoComplete(nomeCidadeDesembarque).get(5, new List<string>());

                //    DataJson ultimaCidadeEmbarque = lstCidadeEmbarque.LastOrDefault();
                //    DataJson ultimaCidadeDesembarque = lstCidadeDesembarque.LastOrDefault();

                //    SessionMontePacote.FirstOrDefault().Aereo.FirstOrDefault().DataEmbarque = SessionMontePacote.FirstOrDefault().Saida;
                //    SessionMontePacote.FirstOrDefault().Aereo.FirstOrDefault().DataRetorno = SessionMontePacote.FirstOrDefault().Retorno;
                //    SessionMontePacote.FirstOrDefault().Aereo.FirstOrDefault().QuantidadeAdulto = SessionMontePacote.FirstOrDefault().QuantAdultos.Value;
                //    SessionMontePacote.FirstOrDefault().Aereo.FirstOrDefault().QuantidadeCrianca = SessionMontePacote.FirstOrDefault().QuantCriancas.Value;

                //    return RedirectToAction("LoadAereoMontePacote", "Aereo", new
                //    {
                //        horarioIdaSelect = "Qualquer",
                //        horarioVoltaSelect = "Qualquer",
                //        classeSelect = "Qualquer",
                //        somenteDireto = "",
                //        hdnDestinoCodigoOld = "",
                //        hdnOrigemCodigoOld = "",
                //        hdnDestinoNomeOld = "",
                //        hdnOrigemNomeOld = "",
                //        hdnDestinoCodigo = ultimaCidadeDesembarque.codigo,
                //        hdnOrigemCodigo = ultimaCidadeEmbarque.codigo,
                //        hdnDestinoNome = ultimaCidadeDesembarque.label,
                //        hdnOrigemNome = ultimaCidadeEmbarque.label,
                //        ddlCriancas = SessionMontePacote.FirstOrDefault().QuantCriancas,
                //        ddlAdultos = SessionMontePacote.FirstOrDefault().QuantAdultos,
                //        ddlBebes = ""
                //    });
                //}
            }

            return View();
        }

        [Obsolete]
        public ActionResult MPOpcionais()
        {
            //if (SessionMontePacote != null)
            //{
            //    string nomeCidadeEmbarque = SessionMontePacote.FirstOrDefault().NomeCidadeOrigem.Split(',')[0];
            //    string nomeCidadeDesembarque = SessionMontePacote.FirstOrDefault().NomeCidadeDestino.Split(',')[0];

            //    List<DataJson> lstCidadeDesembarque = new ListaAutoComplete(nomeCidadeDesembarque).get(6, new List<string>());

            //    AtividadeModel atividadeModel = new AtividadeModel();

            //    atividadeModel.dtEmbarque = SessionMontePacote.FirstOrDefault().Saida;
            //    atividadeModel.dtRetorno = SessionMontePacote.FirstOrDefault().Retorno;
            //    atividadeModel.qtdAdultos = (SessionMontePacote.FirstOrDefault().QuantAdultos.HasValue) ? SessionMontePacote.FirstOrDefault().QuantAdultos.Value : 0;
            //    atividadeModel.qtdCriancas = (SessionMontePacote.FirstOrDefault().QuantCriancas.HasValue) ? SessionMontePacote.FirstOrDefault().QuantCriancas.Value : 0;
            //    atividadeModel.DestinoAtividade = lstCidadeDesembarque[0].codigo;

            //    SessionMontePacote.FirstOrDefault().Opcionais.Add(atividadeModel);

            //    return RedirectToAction("LoadDetalheAtividade", "Atividade", new
            //    {
            //        dtEmbarque = SessionMontePacote.FirstOrDefault().Saida,
            //        hdnDtRetorno = SessionMontePacote.FirstOrDefault().Retorno,
            //        dtRetorno = lstCidadeDesembarque[0].codigo,
            //        ddlQ0Crianca0 = "",
            //        ddlQ0Crianca1 = "",
            //        ddlQ0Crianca2 = "",
            //        ddlQ0Crianca3 = "",
            //        ddlQ0Adulto = SessionMontePacote.FirstOrDefault().QuantAdultos
            //    });
            //}

            return View();
        }

        [Obsolete]
        public ActionResult MPHotelCarroForm(MontePacoteModel MP, string origemMP, string hdnOrigemMPCodigo, string hdnDestinoMPCodigo, string ddlHoraRetirada, string ddlHoraDevolucao)
        {
            //MP.Hotel.FirstOrDefault().Checkin = MP.Saida;
            //MP.Hotel.FirstOrDefault().Checkout = MP.Retorno;
            //MP.Hotel.FirstOrDefault().Estrelas = 1;
            //MP.Hotel.FirstOrDefault().DestinoHotel = MP.NomeCidadeDestino;
            //MP.Hotel.FirstOrDefault().NomeHotel = "";
            //MP.tipoPacotes = "2";
            //MP.CodigoCidadeOrigem = hdnOrigemMPCodigo;
            //MP.CodigoCidadeDestino = hdnDestinoMPCodigo;
            //MP.HorarioSaidaSelecionado = ddlHoraRetirada;
            //MP.HorarioRetornoSelecionado = ddlHoraDevolucao;

            //SessionMontePacote = MP;


            return RedirectToAction("ResultadoMontePacote", "Hotel", new { origem = origemMP, hdnDestinoHotelCodigo = hdnDestinoMPCodigo });
        }

        [Obsolete("Utilizado na versão do Monte seu pacote que foi alterada")]
        public ActionResult MPCarro()
        {
            //MontePacoteModel montePacoteModel = SessionMontePacote.FirstOrDefault();

            //montePacoteModel.CarroPesquisa.FirstOrDefault().DataCheckIn = montePacoteModel.Saida;
            //montePacoteModel.CarroPesquisa.FirstOrDefault().DataCheckOut = montePacoteModel.Retorno;
            //montePacoteModel.CarroPesquisa.FirstOrDefault().LocalRetirada = montePacoteModel.NomeCidadeDestino.Split(',')[0];
            //montePacoteModel.CarroPesquisa.FirstOrDefault().LocalDevolucao = montePacoteModel.NomeCidadeDestino.Split(',')[0];
            //montePacoteModel.CarroPesquisa.FirstOrDefault().HoraCheckInSelecionada = montePacoteModel.HorarioSaidaSelecionado;
            //montePacoteModel.CarroPesquisa.FirstOrDefault().HoraCheckOutSelecionada = montePacoteModel.HorarioRetornoSelecionado;
            //montePacoteModel.CarroPesquisa.FirstOrDefault().IsLocalDiferenteEntrega = false;

            //SessionMontePacote.FirstOrDefault().CarroPesquisa = montePacoteModel.CarroPesquisa;

            //return RedirectToAction("IndexMontePacote", "Carro", new { hdnIsLocalDiferenteEntrega = montePacoteModel.CarroPesquisa.FirstOrDefault().IsLocalDiferenteEntrega });

            return View();
        }

        public virtual PartialViewResult MontePacote()
        {
            SessionMontePacote = null;

            return PartialView();
        }

        public virtual PartialViewResult MPHotelVoo()
        {
            return PartialView();
        }

        public virtual PartialViewResult MPHotelVooCarro()
        {
            return PartialView();
        }

        public virtual PartialViewResult MPHotelCarro()
        {
            return PartialView();
        }

        public virtual PartialViewResult MPVooCarro()
        {
            return PartialView();
        }

        public ActionResult MPHotelVooCarroForm(MontePacoteModel MP, string origemMP, string hdnOrigemMPCodigo, string hdnDestinoMPCodigo, string ddlHoraRetirada, string ddlHoraDevolucao)
        {
            //MP.Hotel.FirstOrDefault().Checkin = MP.Saida;
            //MP.Hotel.FirstOrDefault().Checkout = MP.Retorno;
            //MP.Hotel.FirstOrDefault().Estrelas = 1;
            //MP.Hotel.FirstOrDefault().DestinoHotel = MP.NomeCidadeDestino;
            //MP.Hotel.FirstOrDefault().NomeHotel = "";
            //MP.tipoPacotes = "3";
            //MP.CodigoCidadeOrigem = hdnOrigemMPCodigo;
            //MP.CodigoCidadeDestino = hdnDestinoMPCodigo;
            //MP.HorarioSaidaSelecionado = ddlHoraRetirada;
            //MP.HorarioRetornoSelecionado = ddlHoraDevolucao;

            //SessionMontePacote = MP;

            return RedirectToAction("ResultadoMontePacote", "Hotel", new { origem = origemMP, hdnDestinoHotelCodigo = hdnDestinoMPCodigo });
        }

        public ActionResult MPVooCarroForm(MontePacoteModel MP, string ddlHoraRetirada, string ddlHoraDevolucao, string hdnOrigemMPCodigo, string hdnDestinoMPCodigo)
        {
            //MP.tipoPacotes = "4";
            //MP.CodigoCidadeOrigem = hdnOrigemMPCodigo;
            //MP.CodigoCidadeDestino = hdnDestinoMPCodigo;
            //MP.HorarioSaidaSelecionado = ddlHoraRetirada;
            //MP.HorarioRetornoSelecionado = ddlHoraDevolucao;
            //MP.Aereo.FirstOrDefault().DataEmbarque = MP.Saida;
            //MP.Aereo.FirstOrDefault().DataRetorno = MP.Retorno;

            ////SessionMontePacote = MP;

            //string nomeCidadeEmbarque = MP.NomeCidadeOrigem.Split(',')[0];
            //string nomeCidadeDesembarque = MP.NomeCidadeDestino.Split(',')[0];

            //List<DataJson> lstCidadeEmbarque = new ListaAutoComplete(nomeCidadeEmbarque).get(4, new List<string>());
            //List<DataJson> lstCidadeDesembarque = new ListaAutoComplete(nomeCidadeDesembarque).get(5, new List<string>());

            //DataJson ultimaCidadeEmbarque = lstCidadeEmbarque.LastOrDefault();
            //DataJson ultimaCidadeDesembarque = lstCidadeDesembarque.LastOrDefault();

            //return RedirectToAction("LoadAereoMontePacote", "Aereo", new
            //{
            //    horarioIdaSelect = "Qualquer",
            //    horarioVoltaSelect = "Qualquer",
            //    classeSelect = "Qualquer",
            //    somenteDireto = "",
            //    hdnDestinoCodigoOld = "",
            //    hdnOrigemCodigoOld = "",
            //    hdnDestinoNomeOld = "",
            //    hdnOrigemNomeOld = "",
            //    hdnDestinoCodigo = ultimaCidadeDesembarque.codigo,
            //    hdnOrigemCodigo = ultimaCidadeEmbarque.codigo,
            //    hdnDestinoNome = ultimaCidadeDesembarque.label,
            //    hdnOrigemNome = ultimaCidadeEmbarque.label,
            //    ddlCriancas = 0,
            //    ddlAdultos = 1,
            //    ddlBebes = ""
            //});

            return View();
        }



        #endregion
    }
}