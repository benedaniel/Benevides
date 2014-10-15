using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using FT.Web.Site.Controllers.Base;
using FTV;
using FT.Web.Business.Aereo;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Business.Pacote;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;

namespace FT.Web.Site.Controllers.Carrinho
{
    [Tracer]
    public class CarrinhoController : _BaseController
    {
        FlyTourModel storeDB = new FlyTourModel();

        public ActionResult Index()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            return View(addedFlytour);
        }

        public PartialViewResult MinhaViagem()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            return PartialView(addedFlytour);
        }

        public ActionResult LoadCarrinho()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            return PartialView(addedFlytour);
        }

        [HttpPost]
        public JsonResult AdicionarPacoteCarrinho(string CodigoPacote, bool Comprar, string apartamentos = null)
        {

            var flyTour = new FlyTourModel();
            var pacote = new PacoteVendaModel();

            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var PacotesFlytour = AppCache.Get<List<PacoteVendaModel>>(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", 90, () =>
            {
                return new List<PacoteVendaModel>();
            });

            var success = AppCache.Put(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", null, TimeSpan.FromMinutes(90));

            if (PacotesFlytour == null || PacotesFlytour.Count == 0)
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                var ApartamentoList = (List<ApartamentoModel>)json.Deserialize(apartamentos, typeof(List<ApartamentoModel>));

                PacotesFlytour = PacoteBusiness.ConsultarPacoteVenda(CodigoPacote, ApartamentoList);
                if (PacotesFlytour != null && PacotesFlytour.Count > 0)
                {
                    foreach (var pecote in PacotesFlytour)
                    {
                        var PacoteDescricaoServico = Session["PacoteDescricaoServico" + CodigoPacote] as PacoteDescricao;

                        if (PacoteDescricaoServico != null)
                            foreach (var descricao in PacoteDescricaoServico.DescricaoConteudo.Where(p => p.TituloConteudo == "No pacote estão incluídos"))
                            {
                                PacoteDescricaoServicoModel servico = new PacoteDescricaoServicoModel();
                                servico.Descricao = descricao.DescricaoConteudo;
                                pecote.PacoteDescricaoServico.Add(servico);

                            }

                    }
                }
            }

            if (PacotesFlytour != null && PacotesFlytour.Count > 0)
            {

                var PacoteCarrinho = PacotesFlytour.FirstOrDefault(p => p.CodigoPacote == CodigoPacote);
                /*pacote.Itens = new List<PacoteItensModel>();
                pacote.Itens.AddRange(PacoteCarrinho.Itens);*/
                if (PacoteCarrinho == null)
                {
                    JavaScriptSerializer json = new JavaScriptSerializer();
                    var ApartamentoList = (List<ApartamentoModel>)json.Deserialize(apartamentos, typeof(List<ApartamentoModel>));

                    PacoteCarrinho = PacoteBusiness.ConsultarPacoteVenda(CodigoPacote, ApartamentoList).First();
                }

                flyTour.Apartamentos = PacoteCarrinho.Apartamento;

                if (PacoteCarrinho.Regime.Any(p => p.ValorTotal == 0))
                {
                    var pacotesDisponiveis = PacoteBusiness.ConsultarPacoteVenda(PacoteCarrinho.CodigoPacote, PacoteCarrinho.Apartamento);
                    if (pacotesDisponiveis != null && pacotesDisponiveis.Count > 0)
                        PacoteCarrinho = pacotesDisponiveis.First();
                }

                PacoteCarrinho.Comprar = Comprar;

                flyTour.Pacote = new List<PacoteVendaModel>();
                flyTour.Pacote.Add(PacoteCarrinho);

                AdicionarCarrinho(flyTour, Comprar);

            }

            return Json(SessionKey);
        }

        #region Adicionar Atividades no Carrinho

        [HttpPost]
        public JsonResult AdicionarAtividadeAssistencia(long Atividade, string Valor, string Dia, string DiaRetorno, bool Comprar, string entrada, long? parcelas)
        {
            Valor = Valor.Replace(".", ",");

            var atividade = new AtividadeModel();

            AssistenciaViagemModel Carrinhoitem = new AssistenciaViagemModel();

            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var AssitenciaFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));

            if (AssitenciaFlytour[0].AssistenciaViagem != null && AssitenciaFlytour[0].AssistenciaViagem.Count > 0)
            {
                Carrinhoitem = AssitenciaFlytour[0].AssistenciaViagem.FirstOrDefault(p => p.CodigoAtividade == Atividade);

                //Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AssitenciaFlytour.FirstOrDefault().AssistenciaViagem.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                //if (parcelas != null)
                //{
                //    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                //}

                //if (entrada != null && entrada!= "")
                //{
                //    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                //}                

                Carrinhoitem.ValorTotal = Convert.ToDecimal(Valor);

                Carrinhoitem.dtEmbarque = Dia;
                Carrinhoitem.dtRetorno = DiaRetorno;
            }
            Carrinhoitem.Comprar = Comprar;

            atividade.AssistenciaViagem.Add(Carrinhoitem);
            atividade.Comprar = Comprar;
            AdicionarAtividadeCarrinho(atividade);
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult FinalizarCompra(bool SomenteCarrinho)
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            if (SomenteCarrinho)
            {
                if (addedFlytour.Atividade.Count > 0)
                {
                    addedFlytour.Atividade = addedFlytour.Atividade.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Hotel.Count > 0)
                {
                    addedFlytour.Hotel = addedFlytour.Hotel.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Pacote.Count > 0)
                {
                    addedFlytour.Pacote = addedFlytour.Pacote.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Carro.Count > 0)
                {
                    addedFlytour.Carro = addedFlytour.Carro.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Aereo.Count > 0)
                {
                    addedFlytour.Aereo = addedFlytour.Aereo.Where(p => !p.Comprar).ToList();
                }
            }

            if (addedFlytour.Aereo.Count > 0)
                foreach (var aereo in addedFlytour.Aereo)
                {
                    aereo.Comprar = true;
                }

            if (addedFlytour.Pacote.Count > 0)
                foreach (var pacote in addedFlytour.Pacote)
                {
                    pacote.Comprar = true;
                }

            if (addedFlytour.Hotel.Count > 0)
                foreach (var hotel in addedFlytour.Hotel)
                {
                    hotel.Comprar = true;
                }

            if (addedFlytour.Carro.Count > 0)
                foreach (var carro in addedFlytour.Carro)
                {
                    carro.Comprar = true;
                }

            if (addedFlytour.Atividade.Count > 0)
                foreach (var atividade in addedFlytour.Atividade)
                {
                    atividade.Comprar = true;
                    foreach (var item in atividade.Transfer)
                    {
                        item.Comprar = true;
                    }
                    foreach (var item in atividade.AssistenciaViagem)
                    {
                        item.Comprar = true;
                    }
                    foreach (var item in atividade.Passeio)
                    {
                        item.Comprar = true;
                    }
                    foreach (var item in atividade.Restaurante)
                    {
                        item.Comprar = true;
                    }
                    foreach (var item in atividade.PacoteAtividade)
                    {
                        item.Comprar = true;
                    }
                    foreach (var item in atividade.Ingresso)
                    {
                        item.Comprar = true;
                    }
                }

            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, addedFlytour, TimeSpan.FromMinutes(90));
            return null;
        }

        [HttpPost]
        public JsonResult AdicionarAtividadePasseio(long Atividade, string Valor, string Dia, bool Comprar, string entrada, long? parcelas)
        {
            var atividade = new AtividadeModel();

            PasseioModel Carrinhoitem = new PasseioModel();


            var PasseioFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));


            if (PasseioFlytour[0].Passeio != null && PasseioFlytour[0].Passeio.Count > 0)
            {

                Carrinhoitem = PasseioFlytour[0].Passeio.FirstOrDefault(p => p.CodigoAtividade == Atividade);
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), PasseioFlytour.FirstOrDefault().Passeio.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }

                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;

            }

            Carrinhoitem.Comprar = Comprar;
            atividade.Comprar = Comprar;
            atividade.Passeio.Add(Carrinhoitem);

            AdicionarAtividadeCarrinho(atividade);
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeTransfer(long Atividade, string Valor, string Dia, bool Comprar, string entrada, long? parcelas)
        {
            var atividade = new AtividadeModel();


            TransferModel Carrinhoitem = new TransferModel();


            var TransferFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));


            if (TransferFlytour[0].Transfer != null && TransferFlytour[0].Transfer.Count > 0)
            {
                Carrinhoitem = TransferFlytour[0].Transfer.FirstOrDefault(p => p.CodigoAtividade == Atividade);
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), TransferFlytour.FirstOrDefault().Transfer.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }

                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;

            }
            atividade.Comprar = Comprar;
            Carrinhoitem.Comprar = Comprar;
            atividade.Transfer.Add(Carrinhoitem);

            AdicionarAtividadeCarrinho(atividade);
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeRestaurante(long Atividade, string Valor, string Dia, bool Comprar, string entrada, long? parcelas)
        {
            RestauranteModel Carrinhoitem = new RestauranteModel();
            var atividade = new AtividadeModel();

            var RestauranteFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));


            if (RestauranteFlytour[0].Restaurante != null && RestauranteFlytour[0].Restaurante.Count > 0)
            {

                Carrinhoitem = RestauranteFlytour[0].Restaurante.FirstOrDefault(p => p.CodigoAtividade == Atividade);
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), RestauranteFlytour.FirstOrDefault().Restaurante.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }

                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;

            }



            Carrinhoitem.Comprar = Comprar;
            atividade.Comprar = Comprar;
            atividade.Restaurante.Add(Carrinhoitem);

            AdicionarAtividadeCarrinho(atividade);
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeIngresso(long Atividade, string Valor, string Dia, bool Comprar, string entrada, long? parcelas)
        {
            var atividade = new AtividadeModel();

            IngressoModel Carrinhoitem = new IngressoModel();


            var IngressoFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));


            if (IngressoFlytour[0].Ingresso != null && IngressoFlytour[0].Ingresso.Count > 0)
            {

                Carrinhoitem = IngressoFlytour[0].Ingresso.FirstOrDefault(p => p.CodigoAtividade == Atividade);
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), IngressoFlytour.FirstOrDefault().Ingresso.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }

                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;

            }
            atividade.Comprar = Comprar;
            Carrinhoitem.Comprar = Comprar;
            atividade.Ingresso.Add(Carrinhoitem);

            AdicionarAtividadeCarrinho(atividade);
            return Json(SessionKey);
        }

        public PartialViewResult ContadorCarrinho()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            ViewBag.QuantidadeItens = addedFlytour.QuantidadeItens;

            return PartialView();
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeCarrinho(AtividadeModel Carrinhoitem, bool MontePacote = false)
        {
            var flyTour = new FlyTourModel();

            flyTour.Apartamentos = new List<ApartamentoModel>();
            ApartamentoModel apartamentoModel = new ApartamentoModel();
            apartamentoModel.Passageiros = new List<PassageiroModel>();

            if (Carrinhoitem.Transfer != null)
            {

                foreach (var transfer in Carrinhoitem.Transfer)
                {
                    transfer.Comprar = Carrinhoitem.Comprar;
                    foreach (var apto in transfer.Apartamento)
                    {
                        foreach (var pax in apto.Passageiros)
                        {
                            PassageiroModel passageiroModel = new PassageiroModel();
                            passageiroModel.IdadePassageiro = pax.IdadePassageiro;
                            apartamentoModel.Passageiros.Add(passageiroModel);
                        }
                    }
                }
            }

            if (Carrinhoitem.Passeio != null)
            {

                foreach (var passeio in Carrinhoitem.Passeio)
                {
                    passeio.Comprar = Carrinhoitem.Comprar;
                    foreach (var apto in passeio.Apartamento)
                    {
                        foreach (var pax in apto.Passageiros)
                        {
                            PassageiroModel passageiroModel = new PassageiroModel();
                            passageiroModel.IdadePassageiro = pax.IdadePassageiro;
                            apartamentoModel.Passageiros.Add(passageiroModel);
                        }
                    }
                }
            }


            if (Carrinhoitem.Ingresso != null)
            {

                foreach (var ingresso in Carrinhoitem.Ingresso)
                {
                    ingresso.Comprar = Carrinhoitem.Comprar;
                    foreach (var apto in ingresso.Apartamento)
                    {
                        foreach (var pax in apto.Passageiros)
                        {
                            PassageiroModel passageiroModel = new PassageiroModel();
                            passageiroModel.IdadePassageiro = pax.IdadePassageiro;
                            apartamentoModel.Passageiros.Add(passageiroModel);
                        }
                    }
                }
            }


            if (Carrinhoitem.AssistenciaViagem != null)
            {

                foreach (var assitenciaViagem in Carrinhoitem.AssistenciaViagem)
                {
                    assitenciaViagem.Comprar = Carrinhoitem.Comprar;
                    foreach (var apto in assitenciaViagem.Apartamento)
                    {
                        foreach (var pax in apto.Passageiros)
                        {
                            PassageiroModel passageiroModel = new PassageiroModel();
                            passageiroModel.IdadePassageiro = pax.IdadePassageiro;
                            apartamentoModel.Passageiros.Add(passageiroModel);
                        }
                    }
                }
            }

            if (Carrinhoitem.Restaurante != null)
            {
                foreach (var Restaurante in Carrinhoitem.Restaurante)
                {
                    Restaurante.Comprar = Carrinhoitem.Comprar;
                    foreach (var apto in Restaurante.Apartamento)
                    {
                        foreach (var pax in apto.Passageiros)
                        {
                            PassageiroModel passageiroModel = new PassageiroModel();
                            passageiroModel.IdadePassageiro = pax.IdadePassageiro;
                            apartamentoModel.Passageiros.Add(passageiroModel);
                        }
                    }
                }
            }


            flyTour.Atividade = new List<AtividadeModel>();
            flyTour.Atividade.Add(Carrinhoitem);
            flyTour.Apartamentos.Add(apartamentoModel);
            AdicionarCarrinho(flyTour, Carrinhoitem.Comprar, MontePacote);

            return Json(SessionKey);
        }

        #endregion

        [HttpPost]
        public JsonResult AdicionarHotelCarrinho(long Carrinhoitem, string NomeAcomodacao, string CodigoTarifa, bool Comprar, bool MontePacote = false)
        {
            List<HotelModel> HotelFlytour;

            if (Session["HoteilSession"] != null)
            {
                HotelFlytour = Session["HoteilSession"] as List<HotelModel>;
            }
            else
            {
                HotelFlytour = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, SessionKey + "Hotel", 90, () =>
                {
                    return new List<HotelModel>();
                }); 
            }

            HotelModel PacoteHotel = new HotelModel();

            var codigo = Conv.ToInt64(CodigoTarifa);

            if (HotelFlytour != null && HotelFlytour.Count > 0)
            {
                PacoteHotel = HotelFlytour.FirstOrDefault(p => p.Acomodacoes.Any(b => b.Tarifas.Any(g => g.CodigoTarifa == codigo)));
            }

            var acomodacaoSelecionada = PacoteHotel.Acomodacoes.Where(p => p.NomeAcomodacao.Contains(NomeAcomodacao)).ToList();

            var tarifaSelecionada = new TarifaModel();

            foreach (var acomodacaoModel in acomodacaoSelecionada)
            {
                tarifaSelecionada = acomodacaoModel.Tarifas.FirstOrDefault(p => p.CodigoTarifa == Convert.ToInt64(CodigoTarifa));
                if (tarifaSelecionada != null)
                    break;
            }

            var acomodacoesComATarifa = PacoteHotel.Acomodacoes.FirstOrDefault(p => p.Tarifas.Any(t => t.CodigoTarifa == Convert.ToInt64(CodigoTarifa)));

            for (int i = 0; i < PacoteHotel.Acomodacoes.Count; i++)
            {
                if (PacoteHotel.Acomodacoes[i] == acomodacoesComATarifa)
                {
                    PacoteHotel.Acomodacoes.Remove(PacoteHotel.Acomodacoes[i]);
                }
            }

            acomodacoesComATarifa.Tarifas.Clear();

            acomodacoesComATarifa.Tarifas.Add(tarifaSelecionada);
            PacoteHotel.Acomodacoes.Insert(0, acomodacoesComATarifa);


            var flyTour = new FlyTourModel();

            flyTour.Hotel = new List<HotelModel>();
            PacoteHotel.Comprar = Comprar;
            PacoteHotel.CodigoTarifa = CodigoTarifa;

            var listaParalela = PacoteHotel.Acomodacoes.SelectMany(p => p.Tarifas);

            foreach (var tarifaModel in listaParalela)
            {
                if (tarifaModel != null && tarifaModel.CodigoTarifa == codigo)
                {
                    PacoteHotel.ValorTotal = tarifaModel.ValorTotal;
                    PacoteHotel.ValorCusto = tarifaModel.ValorCusto;
                }
            }

            //PacoteHotel.ValorTotal = PacoteHotel.Acomodacoes.SelectMany(p => p.Tarifas).First(v => v.CodigoTarifa == codigo).ValorTotal;
            //PacoteHotel.ValorCusto = PacoteHotel.Acomodacoes.SelectMany(p => p.Tarifas).First(v => v.CodigoTarifa == codigo).ValorCusto;

            flyTour.Apartamentos = PacoteHotel.Apartamentos;

            flyTour.Hotel.Add(PacoteHotel);

            AdicionarCarrinho(flyTour, Comprar, MontePacote);
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAereoCarrinho(AereoModel AereoModel, bool MontePacote = false)
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
            AereoModel.Valor = Conv.ToDecimal((AereoModel.ValorTaxa + tarifa.ValorVenda).ToString("n2"));

            if (tarifa.ValorVenda == 0)
            {
                return Json("Não foi possível tarifar o voo escolhido, favor escolha outro voo!");
            }
            else
            {
                FT.Web.Business.AereoService.AereoClient client = new FT.Web.Business.AereoService.AereoClient();


                AereoModel.Trechos.Clear();
                foreach (var item in trechosAer)
                {
                    Business.AereoService.CidadeAeroportoParametro siglaOrigem = new Business.AereoService.CidadeAeroportoParametro();
                    Business.AereoService.CidadeAeroportoParametro siglaDestino = new Business.AereoService.CidadeAeroportoParametro();

                    siglaOrigem.SiglaAeroporto = item.SiglaAeroportoOrigem;
                    siglaDestino.SiglaAeroporto = item.SiglaAeroportoDestino;

                    item.NomeCidadeOrigem = AereoBusiness.RetornaCidadeAeroporto(siglaOrigem).NomeCidadeAeroporto;
                    item.NomeCidadeDestino = AereoBusiness.RetornaCidadeAeroporto(siglaDestino).NomeCidadeAeroporto;

                    AereoModel.Trechos.Add(item);
                }

                var apartamentos = new List<ApartamentoModel>();
                var apto = new ApartamentoModel();

                var flyTour = new FlyTourModel();

                if ((AddflyTour.Apartamentos != null && AddflyTour.Apartamentos.Count == 0) || AereoModel.Comprar)
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

                    flyTour.Apartamentos.Add(apto);
                }
                flyTour.Aereo.Add(AereoModel);
                AdicionarCarrinho(flyTour, AereoModel.Comprar, MontePacote);

                return Json("");
            }
        }

        [HttpPost]
        public JsonResult AdicionarCarroCarrinho(string reservaCarroModel, bool Comprar, bool MontePacote = false)
        {
            var CarroFlytour = AppCache.Get<List<ReservaCarroModel>>(EnumProcesso.CarroVenda, SessionKey + "Carro", 90, () =>
            {
                return new List<ReservaCarroModel>();
            });

            //put de nulo não faz nada
            //var success = AppCache.Put(EnumProcesso.CarroVenda, SessionKey + "Carro", null, TimeSpan.FromMinutes(90));

            ReservaCarroModel CarroReserva = new ReservaCarroModel();

            if (CarroFlytour != null && CarroFlytour.Count > 0)
            {

                CarroReserva = CarroFlytour.FirstOrDefault(p => p.CodigoTarifa == reservaCarroModel);
            }

            var flyTour = new FlyTourModel();
            var apto = new ApartamentoModel();
            apto.Passageiros = new List<PassageiroModel>();
            apto.Passageiros.Add(new PassageiroModel() { IdadePassageiro = 30 });
            flyTour.Apartamentos.Add(apto);
            CarroReserva.Comprar = Comprar;
            flyTour.Carro.Add(CarroReserva);
            AdicionarCarrinho(flyTour, Comprar, MontePacote);
            return Json(SessionKey);
        }

        [HttpPost]
        public PartialViewResult LimparCarrinho()
        {
            var limpar = new FlyTourModel();
            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, limpar, TimeSpan.FromMinutes(90));
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            return PartialView("MinhaViagem", addedFlytour);
        }
        [HttpPost]
        public PartialViewResult AtualizarCarrinho(string tipoItem, string codigoItem)
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            return PartialView("MinhaViagem", addedFlytour);
        }
        [HttpPost]
        public PartialViewResult removerItemCarrinho(string tipoItem, string codigoItem)
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            if (tipoItem == "Pacote")
            {
                addedFlytour.Pacote.Remove(addedFlytour.Pacote.First(p => p.CodigoPacote == codigoItem));
            }
            else if (tipoItem == "Aereo")
            {
                addedFlytour.Aereo.Remove(addedFlytour.Aereo.First(p => p.CodigoTarifa.ToString() == codigoItem));
            }
            else if (tipoItem == "Carro")
            {
                var item = addedFlytour.Carro;
                addedFlytour.Carro.Remove(item.First(p => p.CodigoTarifa.ToString() == codigoItem));
            }
            else if (tipoItem == "Translados")
            {
                var item = addedFlytour.Atividade.First(p => p.Transfer.Any(b => b.CodigoAtividade.ToString() == codigoItem));
                addedFlytour.Atividade.Remove(item);
            }
            else if (tipoItem == "AssistenciaViagem")
            {
                var item = addedFlytour.Atividade.First(p => p.AssistenciaViagem.Any(b => b.CodigoAtividade.ToString() == codigoItem));
                addedFlytour.Atividade.Remove(item);
            }
            else if (tipoItem == "Passeio")
            {
                var item = addedFlytour.Atividade.First(p => p.Passeio.Any(b => b.CodigoAtividade.ToString() == codigoItem));
                addedFlytour.Atividade.Remove(item);
            }
            else if (tipoItem == "Ingresso")
            {
                var item = addedFlytour.Atividade.First(p => p.Ingresso.Any(b => b.CodigoAtividade.ToString() == codigoItem));
                addedFlytour.Atividade.Remove(item);
            }
            else if (tipoItem == "Restaurante")
            {
                var item = addedFlytour.Atividade.First(p => p.Restaurante.Any(b => b.CodigoAtividade.ToString() == codigoItem));
                addedFlytour.Atividade.Remove(item);
            }
            else if (tipoItem == "PacoteAtividade")
            {
                var item = addedFlytour.Atividade.First(p => p.PacoteAtividade.Any(b => b.CodigoAtividade.ToString() == codigoItem));
                addedFlytour.Atividade.Remove(item);

            }
            else if (tipoItem == "Hotel")
            {
                addedFlytour.Hotel.Remove(addedFlytour.Hotel.First(p => p.CodigoHotel.ToString() == codigoItem));
            }

            if ((addedFlytour.Hotel != null && addedFlytour.Hotel.Count > 0)
                || (addedFlytour.Aereo != null && addedFlytour.Aereo.Count > 0)
                || (addedFlytour.Pacote != null && addedFlytour.Pacote.Count > 0)
                || (addedFlytour.Atividade != null && addedFlytour.Atividade.Count > 0))
            {
                //Mantem a apartamentalização
            }
            else
            {
                addedFlytour.Apartamentos = new List<ApartamentoModel>();
            }

            addedFlytour.SomarValorTotal();
            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, addedFlytour, TimeSpan.FromMinutes(90));

            return PartialView("MinhaViagem", addedFlytour);
        }
        [HttpPost]
        public JsonResult LimparCompra()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            if (addedFlytour != null)
            {
                if (addedFlytour.Atividade.Count > 0)
                {
                    addedFlytour.Atividade = addedFlytour.Atividade.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Hotel.Count > 0)
                {
                    addedFlytour.Hotel = addedFlytour.Hotel.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Pacote.Count > 0)
                {
                    addedFlytour.Pacote = addedFlytour.Pacote.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Carro.Count > 0)
                {
                    addedFlytour.Carro = addedFlytour.Carro.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Aereo.Count > 0)
                {
                    addedFlytour.Aereo = addedFlytour.Aereo.Where(p => !p.Comprar).ToList();
                }
            }

            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, addedFlytour, TimeSpan.FromMinutes(90));

            return null;
        }

        private JsonResult AdicionarCarrinho(FlyTourModel FlyTouritem, bool Comprar, bool MontePacote = false)
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            if (Comprar && !MontePacote)
            {
                if (addedFlytour.Atividade.Count > 0)
                {
                    addedFlytour.Atividade = addedFlytour.Atividade.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Hotel.Count > 0)
                {
                    addedFlytour.Hotel = addedFlytour.Hotel.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Pacote.Count > 0)
                {
                    addedFlytour.Pacote = addedFlytour.Pacote.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Carro.Count > 0)
                {
                    addedFlytour.Carro = addedFlytour.Carro.Where(p => !p.Comprar).ToList();
                }

                if (addedFlytour.Aereo.Count > 0)
                {
                    addedFlytour.Aereo = addedFlytour.Aereo.Where(p => !p.Comprar).ToList();
                }
            }

            if ((addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count == 0) || (!addedFlytour.UtilizaApartamentos))
            {
                if (FlyTouritem.Apartamentos.Count > 0)
                {
                    addedFlytour.Apartamentos = FlyTouritem.Apartamentos;
                }
            }

            if (FlyTouritem.Atividade.Count > 0)
            {
                addedFlytour.Atividade.AddRange(FlyTouritem.Atividade);
            }

            if (FlyTouritem.Hotel.Count > 0)
            {
                addedFlytour.Hotel.AddRange(FlyTouritem.Hotel);
            }

            if (FlyTouritem.Pacote.Count > 0)
            {
                addedFlytour.Pacote.AddRange(FlyTouritem.Pacote);
            }

            if (FlyTouritem.Carro.Count > 0)
            {
                addedFlytour.Carro.AddRange(FlyTouritem.Carro);
            }

            if (FlyTouritem.Aereo.Count > 0)
            {
                addedFlytour.Aereo.AddRange(FlyTouritem.Aereo);
            }
            addedFlytour.SomarValorTotal();
            addedFlytour.ValorTaxas = SomarValorTotalTaxas(addedFlytour);

            //Código inserido, pois está ocorrendo de quando é somente carro não está adicionando os passageiros
            //caso seja adicionado item de carro e o apartamento não contém registro, será inserido o registro default
            //criado no método AdicionarCarroCarrinho
            if ((addedFlytour.Apartamentos == null || addedFlytour.Apartamentos.Count == 0) && FlyTouritem.Carro.Count > 0)
            {
                addedFlytour.Apartamentos = new List<ApartamentoModel>();
                addedFlytour.Apartamentos = FlyTouritem.Apartamentos;
            }

            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, addedFlytour, TimeSpan.FromMinutes(90));

            return Json(SessionKey);
        }

        private decimal SomarValorTotalTaxas(FlyTourModel addedFlytour)
        {
            Decimal taxas = 0;
            if (addedFlytour.Aereo != null)
                taxas += Convert.ToDecimal(addedFlytour.Aereo.Where(b => !b.Comprar).Sum(p => p.ValorTaxa));
            if (addedFlytour.Carro != null)
                taxas += Convert.ToDecimal(addedFlytour.Carro.Where(b => !b.Comprar).Sum(p => p.ValorTaxa));
            if (addedFlytour.Pacote != null)
                taxas += Convert.ToDecimal(addedFlytour.Pacote.Where(b => !b.Comprar).Sum(p => p.Regime.First().Taxas));
            return taxas;
        }

        [ChildActionOnly]
        public ActionResult CarrinhoSummary()
        {
            return PartialView("CarrinhoSummary");
        }

        public JsonResult AdicionarMontePacoteCarrinho()
        {
            foreach (var destino in MPSession.Destinos)
            {
                if (destino.ReservaCarro != null && !String.IsNullOrEmpty(destino.ReservaCarro.CodigoTarifa))
                {
                    AdicionarCarroCarrinho(destino.ReservaCarro.CodigoTarifa, true, true);
                }

                foreach (var opcional in destino.Opcionais)
                {
                    opcional.Comprar = true;
                    AdicionarAtividadeCarrinho(opcional, true);
                }

                if (destino.Hotel != null && !String.IsNullOrEmpty(destino.Hotel.CodigoTarifa))
                {
                    var acomodacoesComATarifa = destino.Hotel.Acomodacoes.FirstOrDefault(p => p.Tarifas.Any(t => t.CodigoTarifa == Convert.ToInt64(destino.Hotel.CodigoTarifa)));

                    AdicionarHotelCarrinho(destino.Hotel.CodigoHotel.Value, acomodacoesComATarifa.NomeAcomodacao, destino.Hotel.CodigoTarifa, true, true);
                }
            }

            if (MPSession.Aereo.Count > 0 && MPSession.Aereo.First().CodigoTarifa > 0)
            {
                MPSession.Aereo.First().Comprar = true;
                AdicionarAereoCarrinho(MPSession.Aereo.First(), true);

                var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey + "Aereo", null, TimeSpan.FromMinutes(90));
            }

            //Limpando as variáveis utilizadas pelo monte seu pacote
            MPSession = new MontePacoteModel();
            Session["IndiceAdicionarCarro"] = null;
            Session["IndiceAdicionarAtividade"] = null;
            Session["IndiceAdicionarHotel"] = null;
            Session["IndiceAdicionarAereo"] = null;

            return Json(SessionKey);
        }
    }
}
