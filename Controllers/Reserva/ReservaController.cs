using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using FTV;
using FT.Web.Site.Controllers.Base;
using FT.Web.Business.Aereo;
using FT.Web.Business.Pagamento;
using FT.Web.Business.Atividade;
using FT.Web.Business.Pacote;
using FT.Web.Business.Hotel;
using FT.Web.Core.Util.Validation;
using FT.Web.Business.Carro;
using System.Configuration;
using FT.Web.Business.Reserva;
using FT.Web.Core.Util.SiteTracer;

namespace FT.Web.Site.Controllers.Reserva
{
    [Tracer]
    public class ReservaController : _BaseController
    {

        #region Instancia de WebServices

        FT.Web.Business.AtividadeService.ConsultaFiltro filtro = new FT.Web.Business.AtividadeService.ConsultaFiltro();
        FT.Web.Business.UsuarioService.UsuarioClient usuario = new FT.Web.Business.UsuarioService.UsuarioClient();

        //SERVICOS DE VENDA

        FT.Web.Business.HotelService.VendaClient vendaHotel = new FT.Web.Business.HotelService.VendaClient();

        //SERVICOS DE PACOTE

        //SERVICOS DE ATENDENTE
        FT.Web.Business.AtendenteService.AtendenteClient atendente = new FT.Web.Business.AtendenteService.AtendenteClient();
        FT.Web.Business.AtendenteService.AtendenteFiltro atendenteFiltro = new FT.Web.Business.AtendenteService.AtendenteFiltro();

        //SERVICOS DE DISTRIBUIDOR
        FT.Web.Business.DistribuidorService.DistribuidorClient distribuidorClient = new FT.Web.Business.DistribuidorService.DistribuidorClient();
        FT.Web.Business.DistribuidorService.DistribuidorFiltro distribuidorFiltro = new FT.Web.Business.DistribuidorService.DistribuidorFiltro();

        PagamentoBusiness pagamentoBussiness = new PagamentoBusiness();

        #endregion

        public List<AgenciaMapaModel> BuscarAgenciasMapa()
        {
            ViewBag.AgenciaMapa = AppCache.Get<List<AgenciaMapaModel>>(EnumProcesso.Global, "Agencias", 90, () =>
            {
                var login = LoginService();
                List<AgenciaMapaModel> agencias = new List<AgenciaMapaModel>();

                FT.Web.Business.UtilService.UtilClient client = new Business.UtilService.UtilClient();
                FT.Web.Business.UtilService.AgenciaFiltro agenciaFiltro = new Business.UtilService.AgenciaFiltro()
                {
                    CodigoEmpresa = "1",
                    Token = login.Token
                };

                var agenciasService = client.ConsultarAgencias(agenciaFiltro);

                foreach (var agencia in agenciasService)
                {
                    AgenciaMapaModel agenci = new AgenciaMapaModel();
                    ConvertMethod.EntityToEntity<FT.Web.Business.UtilService.AgenciaMapa, AgenciaMapaModel>(agencia, ref agenci);
                    agencias.Add(agenci);
                }

                return agencias;
            });
            return null;
        }

        public ActionResult Index()
        {
            var addedFlytour = Inicializar();

            return View(addedFlytour);
        }

        private FlyTourModel Inicializar()
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            addedFlytour.SomarValorTotal();
            //if (addedFlytour.ValorTotal == 0)
            //{
            //    return View("Home","");
            //}
            addedFlytour.Sexo = new List<SelectListItem>();
            addedFlytour.Sexo.Add(new SelectListItem() { Text = "Masculino", Value = "M" });
            addedFlytour.Sexo.Add(new SelectListItem() { Text = "Feminino", Value = "F" });
            ViewBag.DadoUsuario = SessionUsuario;

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).IN_PERFIL_MASTER == null))
                BuscarAgenciasMapa();


            addedFlytour.Documento = ReservaBusiness.DocumentosNecessarios(addedFlytour);
            return addedFlytour;
        }

        [HttpPost]
        public JsonResult ValidaDataInfantChd(string data, int idade)
        {
            var valido = false;

            if (idade == 30)
            {
                if (Convert.ToDateTime(data).AddYears(12) > DateTime.Now)
                {
                    return Json(valido);
                }
                valido = true;
                return Json(valido);
            }

            DateTime? dataNasc = Conv.ToDateTimeNullable(data);
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            if (addedFlytour.UtilizaIdades)
            {

                var dataMenorCheckin = MenorCheckin(addedFlytour);
                if (dataMenorCheckin != null && dataNasc != null)
                {
                    decimal idaded;
                    //idaded = (dataMenorCheckin.Value.Year - dataNasc.Value.Year);

                    idaded = Convert.ToDecimal(dataMenorCheckin.Value.Subtract(dataNasc.Value).TotalDays / 365);

                    idaded = Decimal.Truncate(idaded);

                    if (idaded == idade)
                        valido = true;
                }
            }
            else
            {
                var dataMenorCheckin = MenorCheckin(addedFlytour);
                if (dataMenorCheckin != null && dataNasc != null)
                {
                    int idaded;
                    idaded = dataMenorCheckin.Value.Year - dataNasc.Value.Year;
                    if (dataMenorCheckin < dataNasc.Value.AddYears(idaded))
                    {
                        idaded--;
                    }

                    if (idade == 6)
                    {
                        if (idaded >= 2 && idaded <= 11)
                            valido = true;
                    }
                    else if (idade == 1)
                    {
                        if (idaded < 2)
                            valido = true;
                    }
                }
            }
            return Json(valido);
        }

        private DateTime? MenorCheckin(FlyTourModel addedFlytour)
        {
            string dataMenor = null;
            if (addedFlytour.Atividade.Count > 0)
            {
                foreach (var atividade in addedFlytour.Atividade)
                {
                    if (atividade.Transfer != null && atividade.Transfer.Count > 0)
                        dataMenor = atividade.Transfer.Min(p => p.dtEmbarque);
                    if (atividade.PacoteAtividade != null && atividade.PacoteAtividade.Count > 0)
                        dataMenor = atividade.PacoteAtividade.Min(p => p.dtEmbarque);
                    if (atividade.Passeio != null && atividade.Passeio.Count > 0)
                        dataMenor = atividade.Passeio.Min(p => p.dtEmbarque);
                    if (atividade.Ingresso != null && atividade.Ingresso.Count > 0)
                        dataMenor = atividade.Ingresso.Min(p => p.dtEmbarque);
                    if (atividade.AssistenciaViagem != null && atividade.AssistenciaViagem.Count > 0)
                        dataMenor = atividade.AssistenciaViagem.Min(p => p.dtEmbarque);
                }
            }

            if (addedFlytour.Hotel.Count > 0)
            {
                dataMenor = addedFlytour.Hotel.Min(p => p.Checkin);
            }

            if (addedFlytour.Pacote.Count > 0)
            {
                dataMenor = addedFlytour.Pacote.SelectMany(p => p.Regime).Min(b => b.Checkin);
            }

            if (addedFlytour.Carro.Count > 0)
            {
                dataMenor = addedFlytour.Carro.Min(p => p.DataCheckIn);
            }

            if (addedFlytour.Aereo.Count > 0)
            {
                dataMenor = addedFlytour.Aereo.Min(p => p.DataEmbarque);
            }
            return Conv.ToDateTimeNullable(dataMenor);
        }

        [HttpPost]
        public JsonResult LoadDistribuidor()
        {
            var login = LoginService();
            distribuidorFiltro = new FT.Web.Business.DistribuidorService.DistribuidorFiltro();

            distribuidorFiltro.Token = login.Token;

            var usuario = SessionUsuario.ID_PESSOA_DISTRIBUIDOR;

            var lista = distribuidorClient.ConsultarDistribuidor(distribuidorFiltro).Distribuidores.Where(p => p.v_id_pessoa == usuario);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadAtendente(string idpessoa)
        {
            atendenteFiltro = new FT.Web.Business.AtendenteService.AtendenteFiltro();

            var login = LoginService();

            atendenteFiltro.Token = login.Token;

            atendenteFiltro.IdPessoa = idpessoa;

            var lista = atendente.ConsultarAtendente(atendenteFiltro);

            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        #region Registro de Reserva
        [HttpPost]
        public ViewResult RegistrarReserva(FlyTourModel FlyTour, string txtCodigoCep, string txtLogradouro, string txtTipo, string txtNumero, string txtComplemento, string txtBairro,
            string TxtNomeCartao, string txtNumeroCartao, string txtValidadeCartaoMes, string txtValidadeCartaoAno, string txtCodSeguranca, string textNome, string txtCPFCartao, string hdnAgencia, int? qtdPagamentos, string DLState, string DLAtendente)
        {
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            string codigoFile = string.Empty;
            try
            {
                var Btoc = false;
                var codigoTipoVenda = "5";
                if (Session["Usuario"] != null)
                    if ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).IN_PERFIL_MASTER == null)
                    {
                        Btoc = true;
                        codigoTipoVenda = "3";
                    }

                if (hdnAgencia == null && Session["Usuario"] != null)
                {
                    if (Btoc)
                    {
                        hdnAgencia = "7584";
                    }
                    else
                    {
                        hdnAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
                    }
                }

                if (Session["Usuario"] != null && (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA != null)
                    addedFlytour.CodigoVendedor = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA.ToString();

                addedFlytour.SomarValorTotal();
                addedFlytour.Apartamentos = PreencherApartamentos(addedFlytour);

                // =)
                decimal valorTotal = 0;
                List<DadosPagamentoModel> dadosPagamentoModels = new List<DadosPagamentoModel>();
                for (int i = 1; i < qtdPagamentos + 1; i++)
                {
                    DadosPagamentoModel dadosPagamentoModel = new DadosPagamentoModel();
                    dadosPagamentoModel.NumeroCartao = Request.Form["txtNumeroCartao-" + i];
                    dadosPagamentoModel.ValidadeCartaoMes = Request.Form["txtValidadeCartaoMes-" + i];
                    dadosPagamentoModel.ValidadeCartaoAno = Request.Form["txtValidadeCartaoAno-" + i];
                    dadosPagamentoModel.CodSeguranca = Request.Form["txtCodSeguranca-" + i];
                    dadosPagamentoModel.Nome = Request.Form["textNome-" + i];
                    dadosPagamentoModel.DataVencimento = Request.Form["hdnData-" + i];
                    dadosPagamentoModel.CodigoPlanoParcela = Request.Form["CodigoPlanoParcela-" + i];
                    dadosPagamentoModel.Valor = Request.Form["valor-" + i];
                    valorTotal = valorTotal + Conv.ToDecimal(dadosPagamentoModel.Valor);
                    dadosPagamentoModels.Add(dadosPagamentoModel);
                }


                PessoaModel Usuario = new PessoaModel();

                EnderecoModel enderecoModel = new EnderecoModel();
                enderecoModel.NR_CEP = txtCodigoCep;
                enderecoModel.DS_ENDERECO = txtLogradouro;
                enderecoModel.TP_ENDERECO = txtTipo;
                enderecoModel.NR_ENDERECO = Conv.ToDecimalNullable(txtNumero);
                enderecoModel.DS_COMPLEMENTO = txtComplemento;
                enderecoModel.DS_BAIRRO = txtBairro;

                Usuario.ENDERECO = enderecoModel;

                addedFlytour.ServicosVoo = PreencherServicos();

                codigoFile = Reservar(hdnAgencia, DLAtendente, addedFlytour, codigoFile, codigoTipoVenda);
                addedFlytour.Btoc = Btoc;
                if (String.IsNullOrEmpty(addedFlytour.MensagemErro))
                {
                    var login = LoginService();

                    if (Btoc)
                    {
                        addedFlytour.CodigoFile = Conv.ToInt64(codigoFile);
                        if (addedFlytour.DisponivelCompra)
                        {
                            var retorno = FT.Web.Business.Financeiro.Pagamento.EnviarPagamento(addedFlytour, codigoFile, Usuario, SessionUsuario, dadosPagamentoModels, hdnAgencia);
                            addedFlytour.CodigoFile = retorno.CodigoFile;
                            addedFlytour.MensagemErro += retorno.MensagemErro;
                        }
                        addedFlytour.Btoc = true;
                    }
                    else
                    {
                        addedFlytour.CodigoFile = Conv.ToInt64(codigoFile);
                        var url = "pkg_pagamento.prc_inicial?p_token={0}&p_id_pagamento={1}&p_login={2}&p_id_pessoa_agencia={3}";
                        url = string.Format(url, (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).Token, codigoFile, (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).USUARIO, (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA);
                        addedFlytour.BtoBPath = ConfigurationManager.AppSettings["UrlBackoffice"] + url;
                        addedFlytour.Btoc = false;
                    }
                }
                else
                {
                    addedFlytour.CodigoFile = Conv.ToInt64(codigoFile);
                    //Cancelar o File, pois ocorreu um erro ao gerar a Reserva
                }
            }
            catch (Exception ex)
            {
                addedFlytour.CodigoFile = Conv.ToInt64(codigoFile);
                addedFlytour.MensagemErro += "ReservaController - RegistrarReservar" + ex.Message;
            }

            var limpar = new FlyTourModel();
            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, limpar, TimeSpan.FromMinutes(90));

            return View(addedFlytour);
        }

        private List<ServicoVoo> PreencherServicos()
        {
            List<ServicoVoo> servicos = new List<ServicoVoo>();

            if (Request.Form["txtHoraVooChegada"] != null)
            {
                ServicoVoo servico = new ServicoVoo();

                #region Preencher dados do voo

                if (Request.Form["txtHoraVooChegada"] != null)
                {
                    servico.HoraFinalVoo = Request.Form["txtHoraVooChegada"];
                }
                if (Request.Form["txtCiaVooChegada"] != null)
                {
                    servico.NomeCia = Request.Form["txtCiaVooChegada"];
                }
                if (Request.Form["txtNumVooChegada"] != null)
                {
                    servico.NumeroVoo = Request.Form["txtNumVooChegada"];
                }
                if (Request.Form["txtLocalizadorChegada"] != null)
                {
                    servico.Localizador = Request.Form["txtLocalizadorChegada"];
                }

                servicos.Add(servico);
                ServicoVoo servico2 = new ServicoVoo();

                if (Request.Form["txtHoraVooPartida"] != null)
                {
                    servico2.HoraInicialVoo = Request.Form["txtHoraVooPartida"];
                }
                if (Request.Form["txtCiaVooPartida"] != null)
                {
                    servico2.NomeCia = Request.Form["txtCiaVooPartida"];
                }
                if (Request.Form["txtNumVooPartida"] != null)
                {
                    servico2.NumeroVoo = Request.Form["txtNumVooPartida"];
                }
                if (Request.Form["txtLocalizadorPartida"] != null)
                {
                    servico2.Localizador = Request.Form["txtLocalizadorPartida"];
                }
                servicos.Add(servico2);

                #endregion
            }

            return servicos;
        }

        private static string Reservar(string hdnAgencia, string DLAtendente, FlyTourModel addedFlytour, string codigoFile, string codigoTipoVenda)
        {
            try
            {
                if (addedFlytour.Atividade != null && addedFlytour.Atividade.Count > 0)
                {
                    foreach (var tarifaAtividade in addedFlytour.Atividade.Where(p => p.Comprar))
                    {
                        if (tarifaAtividade.Transfer != null && tarifaAtividade.Transfer.Count > 0)
                            foreach (var transfer in tarifaAtividade.Transfer)
                            {
                                var AtividadeRetorno = AtividadeBusiness.Reservar(transfer.dtEmbarque, transfer.dtRetorno, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda, transfer.TarifasItem.First().CodigoTarifa.ToString(), transfer.CodigoCidadeDestino);
                                if (AtividadeRetorno != null && AtividadeRetorno.CodigoFile != null && AtividadeRetorno.CodigoFile != 0)
                                    codigoFile = AtividadeRetorno.CodigoFile.ToString();
                                if (!string.IsNullOrEmpty(AtividadeRetorno.MensagemErro) || AtividadeRetorno.CodigoFile == 0 || AtividadeRetorno.CodigoFile == null)
                                {
                                    addedFlytour.MensagemErro += "Problemas na geração da reserva do Serviço: " + AtividadeRetorno.MensagemErro;
                                }
                            }

                        if (tarifaAtividade.Passeio != null && tarifaAtividade.Passeio.Count > 0)
                            foreach (var passeio in tarifaAtividade.Passeio)
                            {
                                var AtividadeRetorno = AtividadeBusiness.Reservar(passeio.dtEmbarque, passeio.dtRetorno, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda, passeio.TarifasItem.First().CodigoTarifa.ToString(), passeio.CodigoCidadeDestino);
                                if (AtividadeRetorno != null && AtividadeRetorno.CodigoFile != null && AtividadeRetorno.CodigoFile != 0)
                                    codigoFile = AtividadeRetorno.CodigoFile.ToString();
                                if (!string.IsNullOrEmpty(AtividadeRetorno.MensagemErro) || AtividadeRetorno.CodigoFile == 0 || AtividadeRetorno.CodigoFile == null)
                                {
                                    addedFlytour.MensagemErro += "Problemas na geração da reserva do Serviço: " + AtividadeRetorno.MensagemErro;
                                }
                            }

                        if (tarifaAtividade.Restaurante != null && tarifaAtividade.Restaurante.Count > 0)
                            foreach (var restaurante in tarifaAtividade.Restaurante)
                            {
                                var AtividadeRetorno = AtividadeBusiness.Reservar(restaurante.dtEmbarque, restaurante.dtRetorno, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda, restaurante.TarifasItem.First().CodigoTarifa.ToString(), restaurante.CodigoCidadeDestino);
                                if (AtividadeRetorno != null && AtividadeRetorno.CodigoFile != null && AtividadeRetorno.CodigoFile != 0)
                                    codigoFile = AtividadeRetorno.CodigoFile.ToString();
                                if (!string.IsNullOrEmpty(AtividadeRetorno.MensagemErro) || AtividadeRetorno.CodigoFile == 0 || AtividadeRetorno.CodigoFile == null)
                                {
                                    addedFlytour.MensagemErro += "Problemas na geração da reserva do Serviço: " + AtividadeRetorno.MensagemErro;
                                }
                            }

                        if (tarifaAtividade.PacoteAtividade != null && tarifaAtividade.PacoteAtividade.Count > 0)
                            foreach (var pacoteAtividade in tarifaAtividade.PacoteAtividade)
                            {
                                var AtividadeRetorno = AtividadeBusiness.Reservar(pacoteAtividade.dtEmbarque, pacoteAtividade.dtRetorno, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda, pacoteAtividade.TarifasItem.First().CodigoTarifa.ToString(), pacoteAtividade.CodigoCidadeDestino);
                                if (AtividadeRetorno != null && AtividadeRetorno.CodigoFile != null && AtividadeRetorno.CodigoFile != 0)
                                    codigoFile = AtividadeRetorno.CodigoFile.ToString();
                                if (!string.IsNullOrEmpty(AtividadeRetorno.MensagemErro) || AtividadeRetorno.CodigoFile == 0 || AtividadeRetorno.CodigoFile == null)
                                {
                                    addedFlytour.MensagemErro += "Problemas na geração da reserva do Serviço: " + AtividadeRetorno.MensagemErro;
                                }
                            }

                        if (tarifaAtividade.Ingresso != null && tarifaAtividade.Ingresso.Count > 0)
                            foreach (var ingresso in tarifaAtividade.Ingresso)
                            {
                                var AtividadeRetorno = AtividadeBusiness.Reservar(ingresso.dtEmbarque, ingresso.dtRetorno, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda, ingresso.TarifasItem.First().CodigoTarifa.ToString(), ingresso.CodigoCidadeDestino);
                                if (AtividadeRetorno != null && AtividadeRetorno.CodigoFile != null && AtividadeRetorno.CodigoFile != 0)
                                    codigoFile = AtividadeRetorno.CodigoFile.ToString();
                                if (!string.IsNullOrEmpty(AtividadeRetorno.MensagemErro) || AtividadeRetorno.CodigoFile == 0 || AtividadeRetorno.CodigoFile == null)
                                {
                                    addedFlytour.MensagemErro += "Problemas na geração da reserva do Serviço: " + AtividadeRetorno.MensagemErro;
                                }
                            }

                        if (tarifaAtividade.AssistenciaViagem != null && tarifaAtividade.AssistenciaViagem.Count > 0)
                            foreach (var assistenciaViagem in tarifaAtividade.AssistenciaViagem)
                            {
                                var AtividadeRetorno = AtividadeBusiness.Reservar(assistenciaViagem.dtEmbarque, assistenciaViagem.dtRetorno, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda, assistenciaViagem.TarifasItem.First().CodigoTarifa.ToString(), assistenciaViagem.CodigoCidadeDestino);
                                if (AtividadeRetorno != null && AtividadeRetorno.CodigoFile != null && AtividadeRetorno.CodigoFile != 0)
                                    codigoFile = AtividadeRetorno.CodigoFile.ToString();
                                if (!string.IsNullOrEmpty(AtividadeRetorno.MensagemErro) || AtividadeRetorno.CodigoFile == 0 || AtividadeRetorno.CodigoFile == null)
                                {
                                    addedFlytour.MensagemErro += "Problemas na geração da reserva do Serviço: " + AtividadeRetorno.MensagemErro;
                                }
                            }
                    }
                }


                if (addedFlytour.Carro != null && addedFlytour.Carro.Count > 0)
                {
                    foreach (var carro in addedFlytour.Carro.Where(p => p.Comprar))
                    {
                        var CarroRetorno = CarroBusiness.Reservar(carro, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda);
                        if (CarroRetorno != null && CarroRetorno.CodigoFile != 0)
                            codigoFile = CarroRetorno.CodigoFile.ToString();
                        if (!string.IsNullOrEmpty(CarroRetorno.MensagemErro) || CarroRetorno.CodigoFile == 0 || CarroRetorno.CodigoFile == null)
                        {
                            addedFlytour.MensagemErro += "Problemas na geração da reserva do Carro: " + CarroRetorno.MensagemErro;
                        }
                    }
                }

                if (addedFlytour.Pacote != null && addedFlytour.Pacote.Count > 0)
                {
                    foreach (var pacote in addedFlytour.Pacote.Where(p => p.Comprar))
                    {
                        var PacoteRetorno = PacoteBusiness.Reservar(pacote, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda);
                        if (PacoteRetorno != null && PacoteRetorno.CodigoFile != null && PacoteRetorno.CodigoFile != 0)
                            codigoFile = PacoteRetorno.CodigoFile.ToString();
                        if (!string.IsNullOrEmpty(PacoteRetorno.MensagemErro) || PacoteRetorno.CodigoFile == 0 || PacoteRetorno.CodigoFile == null)
                        {
                            addedFlytour.MensagemErro += "Problemas na geração da reserva do Pacote: " + PacoteRetorno.MensagemErro;
                        }
                    }
                }

                if (addedFlytour.Hotel != null && addedFlytour.Hotel.Count > 0)
                {
                    foreach (var hotel in addedFlytour.Hotel.Where(p => p.Comprar))
                    {
                        var HotelRetorno = HotelBusiness.Reservar(hotel, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda);
                        if (HotelRetorno != null && HotelRetorno.CodigoFile != null && HotelRetorno.CodigoFile != 0)
                            codigoFile = HotelRetorno.CodigoFile.ToString();
                        if ((!string.IsNullOrEmpty(HotelRetorno.MensagemErro) && HotelRetorno.MensagemErro != "null") || HotelRetorno.CodigoFile == 0 || HotelRetorno.CodigoFile == null)
                        {
                            addedFlytour.MensagemErro += "Problemas na geração da reserva do Hotel: " + HotelRetorno.MensagemErro;
                        }
                    }
                }

                if (addedFlytour.Aereo != null && addedFlytour.Aereo.Count > 0)
                {
                    foreach (var aereo in addedFlytour.Aereo.Where(p => p.Comprar))
                    {
                        var AereoRetorno = AereoBusiness.Reservar(aereo, codigoFile, hdnAgencia, DLAtendente, addedFlytour, codigoTipoVenda);

                        if (AereoRetorno != null && AereoRetorno.CodigoFile != 0)
                            codigoFile = AereoRetorno.CodigoFile.ToString();
                        if ((!string.IsNullOrEmpty(AereoRetorno.MensagemErro) && (AereoRetorno.MensagemErro != "null")) || AereoRetorno.CodigoFile == 0 || AereoRetorno.CodigoFile == null)
                        {
                            addedFlytour.MensagemErro += "Problemas na geração da reserva do Aereo: " + AereoRetorno.MensagemErro;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                addedFlytour.MensagemErro += "ReservaController - Reservar" + ex.Message;
                return codigoFile;
            }
            return codigoFile;
        }

        private List<ApartamentoModel> PreencherApartamentos(FlyTourModel addedFlytour)
        {
            List<ApartamentoModel> apartamentos = new List<ApartamentoModel>();


            for (int h = 1; h < Conv.ToInt32(Request.Form["qtdApto"]) + 1; h++)
            {
                ApartamentoModel apartamento = new ApartamentoModel();
                apartamento.Passageiros = new List<PassageiroModel>();
                for (int i = 0; i < Conv.ToInt32(Request.Form["qtdPassageiros"]); i++)
                {
                    if (Conv.ToInt32(Request.Form["hdnApartamentoIndex-" + i]) == h)
                    {
                        var PassageiroIN = new PassageiroModel();

                        PassageiroIN.NomePassageiro = Request.Form["txtNome" + i];
                        PassageiroIN.SobrenomePassageiro = Request.Form["txtSobreNome" + i];
                        PassageiroIN.DataNascimentoPassageiro = Convert.ToDateTime(Request.Form["txtDataNascimento" + i]);
                        var data = Convert.ToDateTime(Request.Form["txtDataNascimento" + i]);
                        var dataMenorCheckin = MenorCheckin(addedFlytour);
                        PassageiroIN.IdadePassageiro = Decimal.Truncate(Convert.ToDecimal(dataMenorCheckin.Value.Subtract(PassageiroIN.DataNascimentoPassageiro.Value).TotalDays / 365));
                        PassageiroIN.RG = Request.Form["txtRg" + i];
                        PassageiroIN.IndicadorSexo = Request.Form["ddlSexo" + i];
                        PassageiroIN.CpfCnpj = Request.Form["txtCPF" + i];
                        if (Request.Form["ckCondutor" + i] != null)
                            PassageiroIN.Condutor = Request.Form["ckCondutor" + i].Split(',').First() == "true" ? true : false;

                        //Endereco de Cobranca
                        PassageiroIN.CodigoCep = Request.Form["txtCodigoCep"];
                        PassageiroIN.Logradouro = Request.Form["txtLogradouro"];
                        PassageiroIN.Tipo = Request.Form["txtTipo"];
                        PassageiroIN.Numero = Request.Form["txtNumero"];
                        PassageiroIN.Complemento = Request.Form["txtComplemento"];
                        PassageiroIN.Bairro = Request.Form["txtBairro"];

                        apartamento.Passageiros.Add(PassageiroIN);
                    }
                }
                apartamentos.Add(apartamento);
            }
            return apartamentos;
        }


        #endregion
    }
}
