using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services;
using FT.Web.Model.Models;
using FTV;
using System.Globalization;
using FT.Web.Site.Controllers.Base;
using FTV.Shared.Business;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Business.Pagamento;
using System.Text;

namespace FT.Web.Site.Controllers.Atividade
{
    [Tracer]
    public partial class AtividadeController : _BaseController
    {
        FT.Web.Business.AtividadeService.ConsultaFiltro filtro = new FT.Web.Business.AtividadeService.ConsultaFiltro();
        FT.Web.Business.AtividadeService.VendaClient client = new FT.Web.Business.AtividadeService.VendaClient();
        FT.Web.Business.UsuarioService.UsuarioClient usuario = new FT.Web.Business.UsuarioService.UsuarioClient();
        PagamentoBusiness pagamentoBussiness = new PagamentoBusiness();
        FT.Web.Business.DestinoService.DestinoClient destinoClient = new FT.Web.Business.DestinoService.DestinoClient();

        FT.Web.Business.Pacote.PacoteBusiness pacoteBis = new FT.Web.Business.Pacote.PacoteBusiness();

        public string Token
        {
            get { return Session["Token"] as string; }
            set { Session["Token"] = value; }
        }

        //
        // GET: /Atividades/

        public ActionResult Index()
        {

            var list = new SelectList(new[]
                                        {
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                        },
                            "ID", "Name", 1);
            ViewData["adultos"] = list;

            ViewData["criancas"] = list;
            return View();
        }

        public virtual PartialViewResult AtividadeForm()
        {

            AtividadeModel Atividade = new AtividadeModel();

            var list = new SelectList(new[]
                                        {
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                        },
                           "ID", "Name", 1);
            ViewData["adultos"] = list;

            ViewData["criancas"] = list;

            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            if (BloquearApartamentizacao(addedFlytour))
            {
                Atividade.Apartamento = addedFlytour.Apartamentos;
            }

            return PartialView(Atividade);
        }

        public ActionResult LoadDetalheAtividade()
        {
            try
            {
                return View();

            }
            catch (Exception)
            {
                return View();
            }

        }

        public ActionResult LoadDetalheAtividadeMontePacote()
        {
            return View("LoadDetalheAtividade");
        }

        public string LoadDestinoDados(string CodCidade)
        {
            var login = LoginService();
            var Token = login.Token;

            var destinoFiltro = new FT.Web.Business.DestinoService.DestinoFiltro();
            IList<DestinoModel> destinoModel = new List<DestinoModel>();
            List<DestinoModel> destino = new List<DestinoModel>();

            DestinoTopico destinoTopico = new DestinoTopico();
            DestinoTopicoConteudo topicoConteudo = new DestinoTopicoConteudo();
            DestinoFoto destinoFoto = new DestinoFoto();

            destinoFiltro.Token = Token;
            destinoFiltro.CodigoCidade = CodCidade;

            var listaDestino = destinoClient.Consultar(destinoFiltro);
            var destinoItem = new DestinoModel();
            var NomeCidade = "Cidade nao cadastrada;Pais nao cadastrado";

            foreach (var destinoIn in listaDestino)
            {
                if (!string.IsNullOrEmpty(destinoIn.NomeCidade))
                {
                    NomeCidade = destinoIn.NomeCidade;
                }
                else
                {
                    NomeCidade = "Cidade não cadastrada";
                }

                if (!string.IsNullOrEmpty(destinoIn.NomePais))
                {
                    NomeCidade += ";" + destinoIn.NomePais;
                }
                else
                {
                    NomeCidade = NomeCidade + ";Pais nao cadastrado";
                }
            }

            return NomeCidade;
        }

        [WebMethod]
        public PartialViewResult LoadGrid(string DataInicio, string DataFinal, string DestinoAtividade, string Crianca1, string Crianca2, string Crianca3, string Crianca4, string Adulto)
        {
            IList<AtividadeModel> models = new List<AtividadeModel>();

            int Adultos = 0;
            int Criancas = 0;
            int Bebes = 0;
            int Total = 0;
            var login = FT.Web.Business.Gerenciamento.LoginSistema.LoginService();
            var passageiros = new List<FT.Web.Business.AtividadeService.PassageiroFiltro>();
            var ApartamentoList = new List<ApartamentoModel>();

            //Preenche Apartamento com 2 passageiros iniciais DEFAULT
            var ApartamentoIN = new ApartamentoModel();
            ApartamentoIN.Passageiros = new List<PassageiroModel>();

            for (int i = 0; i < Convert.ToInt32(Adulto); i++)
            {
                var passageiro = new FT.Web.Business.AtividadeService.PassageiroFiltro();
                passageiro.Idade = "30";
                passageiros.Add(passageiro);

                // PARA ENVIO DA RESERVA
                var PassageiroIN = new PassageiroModel();
                PassageiroIN.IdadePassageiro = 30;
                ApartamentoIN.Passageiros.Add(PassageiroIN);

                Total++;
                Adultos++;

            }
            /*quantCriancas*/
            if (!string.IsNullOrEmpty(Crianca1))
            {
                var passageiro = new FT.Web.Business.AtividadeService.PassageiroFiltro();
                passageiro.Idade = Crianca1;
                passageiros.Add(passageiro);

                if (Convert.ToInt64(Crianca1) < 2)
                    Bebes++;
                else
                    Criancas++;

                Total++;

                // PARA ENVIO DA RESERVA
                var PassageiroIN = new PassageiroModel();
                PassageiroIN.IdadePassageiro = Convert.ToInt32(Crianca1);
                ApartamentoIN.Passageiros.Add(PassageiroIN);
            }

            if (!string.IsNullOrEmpty(Crianca2))
            {

                var passageiro = new FT.Web.Business.AtividadeService.PassageiroFiltro();
                passageiro.Idade = Crianca2;
                passageiros.Add(passageiro);

                if (Convert.ToInt64(Crianca2) < 2)
                    Bebes++;
                else
                    Criancas++;

                Total++;

                // PARA ENVIO DA RESERVA
                var PassageiroIN = new PassageiroModel();
                PassageiroIN.IdadePassageiro = Convert.ToInt32(Crianca2);
                ApartamentoIN.Passageiros.Add(PassageiroIN);
            }

            if (!string.IsNullOrEmpty(Crianca3))
            {
                var passageiro = new FT.Web.Business.AtividadeService.PassageiroFiltro();
                passageiro.Idade = Crianca3;
                passageiros.Add(passageiro);

                if (Convert.ToInt64(Crianca3) < 2)
                    Bebes++;
                else
                    Criancas++;

                Total++;

                // PARA ENVIO DA RESERVA
                var PassageiroIN = new PassageiroModel();
                PassageiroIN.IdadePassageiro = Convert.ToInt32(Crianca3);
                ApartamentoIN.Passageiros.Add(PassageiroIN);
            }

            if (!string.IsNullOrEmpty(Crianca4))
            {
                var passageiro = new FT.Web.Business.AtividadeService.PassageiroFiltro();
                passageiro.Idade = Crianca4;
                passageiros.Add(passageiro);


                if (Convert.ToInt64(Crianca4) < 2)
                    Bebes++;
                else
                    Criancas++;

                Total++;

                // PARA ENVIO DA RESERVA
                var PassageiroIN = new PassageiroModel();
                PassageiroIN.IdadePassageiro = Convert.ToInt32(Crianca4);
                ApartamentoIN.Passageiros.Add(PassageiroIN);
            }

            ApartamentoList.Add(ApartamentoIN);



            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });


            if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0)
            {
                if (addedFlytour.Carro.Where(c => !c.Comprar).Count() > 0
                    || addedFlytour.Pacote.Where(p => !p.Comprar).Count() > 0
                    || addedFlytour.Aereo.Where(a => !a.Comprar).Count() > 0
                    || addedFlytour.Hotel.Where(h => !h.Comprar).Count() > 0
                    || addedFlytour.Atividade.Where(a => !a.Comprar).Count() > 0)
                {
                    var quantidadeAdulto =
                        addedFlytour.Apartamentos.SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).
                            Count();

                    var quantidadeCrianca =
                        addedFlytour.Apartamentos.SelectMany(
                            p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();

                    var quantidadeBebe =
                        addedFlytour.Apartamentos.SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count
                            ();

                    if (!addedFlytour.SomenteCarro)
                    {
                        if ((quantidadeAdulto != Adultos) || (quantidadeCrianca != Criancas) ||
                            (quantidadeBebe != Bebes))
                        {
                            Response.Write(
                                "<div class='AlertaTotal'><span class='AlertaTotalSpan'>* Já existe produto selecionado em seu carrinho. Novos produtos devem possuir a mesma quantidade de passageiros.Favor selecionar a quantidade de passageiros conforme abaixo e refazer a busca.</span></div>");
                            Response.Write("<div class='AlertaTotal'><span class='AlertaTotalSpan'>Adultos:(" +
                                           quantidadeAdulto + ") Criancas:(" + quantidadeCrianca + ") Bebes:(" +
                                           quantidadeBebe + ")</span></div>");
                            return PartialView("LoadGrid");

                        }

                        /*if (Convert.ToInt32(Request.Form["hdnTotalPassageiro"]) != 2)
                        {
                            if (Convert.ToInt32(Request.Form["hdnTotalPassageiro"]) != Total)
                            {
                                Response.Write("<div class='AlertaTotal'><span class='AlertaTotalSpan'>* Já existe produto selecionado em seu carrinho. Novos produtos devem possuir a mesma quantidade de passageiros.Favor selecionar a quantidade de passageiros conforme abaixo e refazer a busca.</span></div>");
                                Response.Write("<div class='AlertaTotal'><span class='AlertaTotalSpan'>Adultos:(" + quantidadeAdulto + ") Criancas:(" + quantidadeCrianca + ") Bebes:(" + quantidadeBebe + ")</span></div>");

                                return PartialView("LoadGrid");
                            }
                        }*/
                    }
                }
            }

            var passageirosDivide = ApartamentoIN.Passageiros.Count();
            Token = login.Token;

            CultureInfo Culture = new CultureInfo("pt-BR");

            filtro.Passageiros = passageiros;
            filtro.CodigoCidade = DestinoAtividade;
            filtro.Token = login.Token;
            filtro.DataInicial = Conv.ToDateTime(DataInicio, Culture).ToString("dd/MM/yyyy");
            filtro.DataFinal = Conv.ToDateTime(DataFinal, Culture).ToString("dd/MM/yyyy");
            filtro.CodigoEmpresa = "1";

            var model = Session["Usuario"] as FT.Web.Model.Models.PessoaModel;
            if (model != null && (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && (model.ID_PESSOA_AGENCIA != null)))
            {
                var pessoaModel = Session["Usuario"] as FT.Web.Model.Models.PessoaModel;
                if (pessoaModel != null)
                    filtro.CodigoAgencia = pessoaModel.ID_PESSOA_AGENCIA.ToString();
            }

            filtro.CodigoIsoMoeda = "BRL";
            filtro.CodigoMoeda = "1";
            filtro.IndicadorInformacaoAtividade = "S";

            var listaAtividades = client.ConsultarAtividade(filtro);
            var Atividade = new AtividadeModel();
            string[] s = LoadDestinoDados(DestinoAtividade).Split(';');

            Atividade.NomeCidade = s[0];
            Atividade.NomePais = s[1];

            foreach (var atividadeRet in listaAtividades)
            {


                if (atividadeRet.CodigoTipoAtividade == 4)
                {

                    var Transfer = new TransferModel();
                    Transfer.NomeAtividade = atividadeRet.NomeAtividade;
                    Transfer.Apartamento = ApartamentoList;
                    Transfer.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Transfer.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Transfer.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Transfer.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Transfer.CodigoCidadeDestino = DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Transfer.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");
                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));
                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }

                            Transfer.Tarifas.Add(tarifaSL);
                        }


                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Transfer.ValorParcela = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(tarifaAtividade.DetalheTarifas[0].ValorParcela), passageirosDivide));
                            Transfer.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Transfer.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }


                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;
                        Transfer.ValorTotal = tarifaAtividade.ValorTotal.Value;



                        Transfer.TarifasItem.Add(tarifa);

                    }

                    Atividade.Transfer.Add(Transfer);

                }
                else if (atividadeRet.CodigoTipoAtividade == 5)
                {
                    var Ingresso = new IngressoModel();

                    Ingresso.NomeAtividade = atividadeRet.NomeAtividade;
                    Ingresso.Apartamento = ApartamentoList;
                    Ingresso.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Ingresso.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Ingresso.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Ingresso.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Ingresso.CodigoCidadeDestino = DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Ingresso.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;
                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");

                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));
                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }
                            Ingresso.Tarifas.Add(tarifaSL);
                        }


                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Ingresso.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            Ingresso.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Ingresso.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;


                        Ingresso.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Ingresso.TarifasItem.Add(tarifa);

                    }

                    Atividade.Ingresso.Add(Ingresso);
                }

                else if (atividadeRet.CodigoTipoAtividade == 6)
                {
                    var Passeio = new PasseioModel();

                    Passeio.NomeAtividade = atividadeRet.NomeAtividade;
                    Passeio.Apartamento = ApartamentoList;
                    Passeio.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Passeio.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Passeio.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Passeio.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Passeio.CodigoCidadeDestino = DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Passeio.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");
                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {

                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);

                            }
                            Passeio.Tarifas.Add(tarifaSL);
                        }

                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Passeio.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            Passeio.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Passeio.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;


                        Passeio.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Passeio.TarifasItem.Add(tarifa);

                    }

                    Atividade.Passeio.Add(Passeio);
                }
                else if (atividadeRet.CodigoTipoAtividade == 7)
                {
                    var Restaurante = new RestauranteModel();


                    Restaurante.NomeAtividade = atividadeRet.NomeAtividade;
                    Restaurante.Apartamento = ApartamentoList;
                    Restaurante.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Restaurante.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Restaurante.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Restaurante.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Restaurante.CodigoCidadeDestino = DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Restaurante.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");

                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {

                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);

                            }
                            Restaurante.Tarifas.Add(tarifaSL);
                        }

                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Restaurante.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            Restaurante.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Restaurante.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;


                        Restaurante.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Restaurante.TarifasItem.Add(tarifa);

                    }

                    Atividade.Restaurante.Add(Restaurante);
                }
                else if (atividadeRet.CodigoTipoAtividade == 9)
                {
                    var AssistenciaViagem = new AssistenciaViagemModel();

                    AssistenciaViagem.NomeAtividade = atividadeRet.NomeAtividade;
                    AssistenciaViagem.Apartamento = ApartamentoList;
                    AssistenciaViagem.CodigoAtividade = atividadeRet.CodigoAtividade;
                    AssistenciaViagem.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    AssistenciaViagem.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    AssistenciaViagem.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    AssistenciaViagem.CodigoCidadeDestino = DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        AssistenciaViagem.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");

                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                            }
                            AssistenciaViagem.Tarifas.Add(tarifaSL);
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorTotalExibe = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide));

                        if (tarifaAtividade.ValorParcela.HasValue)
                        {
                            AssistenciaViagem.ValorParcela = Decimal.Divide(tarifaAtividade.ValorParcela.Value, passageirosDivide);
                        }

                        if (tarifaAtividade.ValorEntrada.HasValue)
                        {
                            AssistenciaViagem.ValorEntrada = Decimal.Divide(tarifaAtividade.ValorEntrada.Value, passageirosDivide);
                        }

                        if (tarifaAtividade.QuantidadeParcela.HasValue)
                        {
                            AssistenciaViagem.QuantidadeParcela = tarifaAtividade.QuantidadeParcela;
                        }

                        AssistenciaViagem.ValorTotal = tarifaAtividade.ValorTotal.Value;
                        AssistenciaViagem.ValorTotalExibe = string.Format("{0:0.00}", tarifaAtividade.ValorTotal.Value);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;

                        AssistenciaViagem.TarifasItem.Add(tarifa);
                    }
                    Atividade.AssistenciaViagem.Add(AssistenciaViagem);
                }
                else if (atividadeRet.CodigoTipoAtividade == 11)
                {
                    var PacoteAtividade = new PacoteAtividadeModel();

                    PacoteAtividade.NomeAtividade = atividadeRet.NomeAtividade;
                    PacoteAtividade.Apartamento = ApartamentoList;
                    PacoteAtividade.CodigoAtividade = atividadeRet.CodigoAtividade;
                    PacoteAtividade.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    PacoteAtividade.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    PacoteAtividade.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    PacoteAtividade.CodigoCidadeDestino = DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        PacoteAtividade.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");
                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }

                            PacoteAtividade.Tarifas.Add(tarifaSL);
                        }

                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            PacoteAtividade.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            PacoteAtividade.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            PacoteAtividade.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;

                        tarifa.ValorTotalExibe = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide));

                        PacoteAtividade.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        PacoteAtividade.TarifasItem.Add(tarifa);

                    }


                    Atividade.PacoteAtividade.Add(PacoteAtividade);
                }
            }

            //removido a ordenação - solicitado pelo diogenes por que será ordenado no pl
            //Ordenar por preço 
            //Atividade.Transfer = Atividade.Transfer.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.Ingresso = Atividade.Ingresso.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.Restaurante = Atividade.Restaurante.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.Passeio = Atividade.Passeio.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.PacoteAtividade = Atividade.PacoteAtividade.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.AssistenciaViagem = Atividade.AssistenciaViagem.OrderBy(x => x.ValorTotal).ToList();


            models.Add(Atividade);

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", models, TimeSpan.FromMinutes(90));

            return PartialView(models);
        }

        public PartialViewResult LoadGridMontePacote(AtividadeModel AtividadeModel)
        {
            AtividadeModel.Apartamento = MPSession.Apartamentos;
            IList<AtividadeModel> models = new List<AtividadeModel>();

            var login = FT.Web.Business.Gerenciamento.LoginSistema.LoginService();
            var passageiros = new List<FT.Web.Business.AtividadeService.PassageiroFiltro>();

            //Preenche Apartamento com 2 passageiros iniciais DEFAULT
            var ApartamentoIN = new ApartamentoModel();
            ApartamentoIN.Passageiros = new List<PassageiroModel>();

            foreach (ApartamentoModel apto in AtividadeModel.Apartamento)
            {
                foreach (var pax in apto.Passageiros)
                {
                    var passageiro = new FT.Web.Business.AtividadeService.PassageiroFiltro();
                    passageiro.Idade = pax.IdadePassageiro.ToString();
                    passageiros.Add(passageiro);
                }

            }

            var passageirosDivide = passageiros.Count();
            Token = login.Token;

            CultureInfo Culture = new CultureInfo("pt-BR");

            filtro.Passageiros = passageiros;
            filtro.CodigoCidade = AtividadeModel.DestinoAtividade;
            filtro.Token = login.Token;
            filtro.DataInicial = Conv.ToDateTime(AtividadeModel.dtEmbarque, Culture).ToString("dd/MM/yyyy");
            filtro.DataFinal = Conv.ToDateTime(AtividadeModel.dtRetorno, Culture).ToString("dd/MM/yyyy");
            filtro.CodigoEmpresa = "1";

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                filtro.CodigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();

            filtro.CodigoIsoMoeda = "BRL";
            filtro.CodigoMoeda = "1";
            filtro.IndicadorInformacaoAtividade = "S";

            var listaAtividades = client.ConsultarAtividade(filtro);
            var Atividade = new AtividadeModel();

            string[] s = LoadDestinoDados(AtividadeModel.DestinoAtividade).Split(';');

            Atividade.NomeCidade = s[0];
            Atividade.NomePais = s[1];

            foreach (var atividadeRet in listaAtividades)
            {

                if (atividadeRet.CodigoTipoAtividade == 4)
                {

                    var Transfer = new TransferModel();
                    Transfer.NomeAtividade = atividadeRet.NomeAtividade;
                    Transfer.Apartamento = AtividadeModel.Apartamento;
                    Transfer.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Transfer.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Transfer.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Transfer.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Transfer.CodigoCidadeDestino = AtividadeModel.DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Transfer.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");
                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));
                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }

                            Transfer.Tarifas.Add(tarifaSL);
                        }


                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Transfer.ValorParcela = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(tarifaAtividade.DetalheTarifas[0].ValorParcela), passageirosDivide));
                            Transfer.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Transfer.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }


                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Transfer.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);

                        Transfer.TarifasItem.Add(tarifa);

                    }

                    Atividade.Transfer.Add(Transfer);

                }
                else if (atividadeRet.CodigoTipoAtividade == 5)
                {
                    var Ingresso = new IngressoModel();

                    Ingresso.NomeAtividade = atividadeRet.NomeAtividade;
                    Ingresso.Apartamento = AtividadeModel.Apartamento;
                    Ingresso.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Ingresso.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Ingresso.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Ingresso.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Ingresso.CodigoCidadeDestino = AtividadeModel.DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Ingresso.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;
                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");

                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));
                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }
                            Ingresso.Tarifas.Add(tarifaSL);
                        }


                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Ingresso.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            Ingresso.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Ingresso.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);


                        Ingresso.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Ingresso.TarifasItem.Add(tarifa);

                    }

                    Atividade.Ingresso.Add(Ingresso);
                }

                else if (atividadeRet.CodigoTipoAtividade == 6)
                {
                    var Passeio = new PasseioModel();

                    Passeio.NomeAtividade = atividadeRet.NomeAtividade;
                    Passeio.Apartamento = AtividadeModel.Apartamento;
                    Passeio.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Passeio.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Passeio.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Passeio.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Passeio.CodigoCidadeDestino = AtividadeModel.DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Passeio.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");
                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }
                            Passeio.Tarifas.Add(tarifaSL);
                        }

                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Passeio.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            Passeio.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Passeio.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);


                        Passeio.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Passeio.TarifasItem.Add(tarifa);

                    }

                    Atividade.Passeio.Add(Passeio);
                }
                else if (atividadeRet.CodigoTipoAtividade == 7)
                {
                    var Restaurante = new RestauranteModel();


                    Restaurante.NomeAtividade = atividadeRet.NomeAtividade;
                    Restaurante.Apartamento = AtividadeModel.Apartamento;
                    Restaurante.CodigoAtividade = atividadeRet.CodigoAtividade;
                    Restaurante.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    Restaurante.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    Restaurante.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    Restaurante.CodigoCidadeDestino = AtividadeModel.DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        Restaurante.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");

                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }
                            Restaurante.Tarifas.Add(tarifaSL);
                        }

                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            Restaurante.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            Restaurante.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            Restaurante.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);


                        Restaurante.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        Restaurante.TarifasItem.Add(tarifa);

                    }

                    Atividade.Restaurante.Add(Restaurante);
                }
                else if (atividadeRet.CodigoTipoAtividade == 9)
                {
                    var AssistenciaViagem = new AssistenciaViagemModel();

                    AssistenciaViagem.NomeAtividade = atividadeRet.NomeAtividade;
                    AssistenciaViagem.Apartamento = AtividadeModel.Apartamento;
                    AssistenciaViagem.CodigoAtividade = atividadeRet.CodigoAtividade;
                    AssistenciaViagem.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    AssistenciaViagem.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    AssistenciaViagem.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    AssistenciaViagem.CodigoCidadeDestino = AtividadeModel.DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        AssistenciaViagem.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");

                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                            }
                            AssistenciaViagem.Tarifas.Add(tarifaSL);
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(tarifaAtividade.ValorTotal.Value, passageirosDivide);
                        tarifa.ValorTotalExibe = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide));

                        if (tarifaAtividade.ValorParcela.HasValue)
                        {
                            AssistenciaViagem.ValorParcela = Decimal.Divide(tarifaAtividade.ValorParcela.Value, passageirosDivide);
                        }

                        if (tarifaAtividade.ValorEntrada.HasValue)
                        {
                            AssistenciaViagem.ValorEntrada = Decimal.Divide(tarifaAtividade.ValorEntrada.Value, passageirosDivide);
                        }

                        if (tarifaAtividade.QuantidadeParcela.HasValue)
                        {
                            AssistenciaViagem.QuantidadeParcela = tarifaAtividade.QuantidadeParcela;
                        }

                        AssistenciaViagem.ValorTotal = tarifaAtividade.ValorTotal.Value;
                        AssistenciaViagem.ValorTotalExibe = string.Format("{0:0.00}", tarifaAtividade.ValorTotal.Value);
                        tarifa.ValorCusto = tarifaAtividade.ValorCustoTotal.Value;

                        AssistenciaViagem.TarifasItem.Add(tarifa);

                    }
                    Atividade.AssistenciaViagem.Add(AssistenciaViagem);
                }
                else if (atividadeRet.CodigoTipoAtividade == 11)
                {
                    var PacoteAtividade = new PacoteAtividadeModel();

                    PacoteAtividade.NomeAtividade = atividadeRet.NomeAtividade;
                    PacoteAtividade.Apartamento = AtividadeModel.Apartamento;
                    PacoteAtividade.CodigoAtividade = atividadeRet.CodigoAtividade;
                    PacoteAtividade.DescricaoAtividade = atividadeRet.DescricaoAtividade;
                    PacoteAtividade.DescricaoTipoAtividade = atividadeRet.DescricaoTipoAtividade;
                    PacoteAtividade.CodigoTipoAtividade = atividadeRet.CodigoTipoAtividade;
                    PacoteAtividade.CodigoCidadeDestino = AtividadeModel.DestinoAtividade;

                    foreach (var tarifaAtividade in atividadeRet.Tarifas)
                    {

                        PacoteAtividade.CodigoStatusVenda = tarifaAtividade.CodigoStatusVenda;

                        foreach (var DetalhetarifaAtividade in tarifaAtividade.DetalheTarifas)
                        {
                            SelectListItem tarifaSL = new SelectListItem();
                            tarifaSL.Text = DetalhetarifaAtividade.DataTarifa.ToString("dd/MM/yyyy");
                            if (DetalhetarifaAtividade.ValorParcela > 0)
                            {
                                tarifaSL.Value = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorParcela), passageirosDivide));

                            }
                            else
                            {
                                var primeirovalor = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(DetalhetarifaAtividade.ValorDiaria), passageirosDivide));
                                var segundovalor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorDiaria);
                                var terceiroValor = string.Format("{0:0.00}", DetalhetarifaAtividade.ValorCustoDiaria);

                                tarifaSL.Value = String.Format("{0}|{1}|{2}", primeirovalor, segundovalor, terceiroValor);
                            }
                            PacoteAtividade.Tarifas.Add(tarifaSL);
                        }

                        if (tarifaAtividade.DetalheTarifas != null && tarifaAtividade.DetalheTarifas.Count > 0)
                        {
                            PacoteAtividade.ValorParcela = tarifaAtividade.DetalheTarifas[0].ValorParcela;
                            PacoteAtividade.ValorEntrada = tarifaAtividade.DetalheTarifas[0].ValorEntrada;
                            PacoteAtividade.QuantidadeParcela = tarifaAtividade.DetalheTarifas[0].QuantidadeParcela;
                        }

                        FT.Web.Model.Models.TarifaModel tarifa = new FT.Web.Model.Models.TarifaModel();
                        tarifa.CodigoTarifa = tarifaAtividade.CodigoTarifa;
                        tarifa.DataInicial = Conv.ToString(tarifaAtividade.DataInicial);
                        tarifa.DataFinal = Conv.ToString(tarifaAtividade.DataFinal);
                        tarifa.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);

                        tarifa.ValorTotalExibe = string.Format("{0:0.00}", Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide));

                        PacoteAtividade.ValorTotal = Decimal.Divide(Convert.ToDecimal(tarifaAtividade.ValorTotal), passageirosDivide);
                        PacoteAtividade.TarifasItem.Add(tarifa);



                    }


                    Atividade.PacoteAtividade.Add(PacoteAtividade);
                }
            }

            //removido a ordenação - solicitado pelo diogenes por que será ordenado no pl
            //Ordenar por preço 
            //Atividade.Transfer = Atividade.Transfer.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.Ingresso = Atividade.Ingresso.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.Restaurante = Atividade.Restaurante.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.Passeio = Atividade.Passeio.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.PacoteAtividade = Atividade.PacoteAtividade.OrderBy(x => x.ValorTotal).ToList();
            //Atividade.AssistenciaViagem = Atividade.AssistenciaViagem.OrderBy(x => x.ValorTotal).ToList();

            models.Add(Atividade);

            ViewBag.IsMontePacote = true;

            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            if (AtividadeFlytour.Count == 0)
            {
                var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", models.ToList(), TimeSpan.FromMinutes(90));
            }
            else
            {
                AtividadeFlytour.AddRange(models);
                var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", AtividadeFlytour, TimeSpan.FromMinutes(90));
            }

            return PartialView("LoadGrid", models);
        }

        public ActionResult AtividadeMenuForm()
        {
            var list = new SelectList(new[] 
                                        { 
                                            new { ID = "1", Name = "1" }, 
                                            new { ID = "2", Name = "2" }, 
                                            new { ID = "3", Name = "3" } 
                                        },
                            "ID", "Name", 1);

            ViewData["adultos"] = list;
            ViewData["criancas"] = list;

            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            AtividadeModel Atividade = new AtividadeModel();

            //if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0)
            if (BloquearApartamentizacao(addedFlytour))
            {
                Atividade.Apartamento = addedFlytour.Apartamentos;
            }

            return View(Atividade);
        }

        [HttpPost]
        public string FormasdePagamento(string CodigoAtividade, string TipoAtividade, string Valor, string Dia, string entrada, long? parcelas, string DiaRetorno, decimal custo)
        {
            var AssitenciaFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var atividade = new AtividadeModel();

            if (TipoAtividade == "Assistencia")
            {
                AssistenciaViagemModel Carrinhoitem = new AssistenciaViagemModel();
                Carrinhoitem = AssitenciaFlytour[0].AssistenciaViagem.FirstOrDefault(p => p.CodigoAtividade == Conv.ToLong(CodigoAtividade));
                Carrinhoitem.ValorTotal = Convert.ToDecimal(Valor.Replace('.', ','));
                Carrinhoitem.dtEmbarque = Dia;
                Carrinhoitem.dtRetorno = DiaRetorno;
                Carrinhoitem.Comprar = true;
                atividade.AssistenciaViagem.Add(Carrinhoitem);
                atividade.Comprar = true;
            }
            else if (TipoAtividade == "Passeio")
            {
                PasseioModel Carrinhoitem = new PasseioModel();
                Carrinhoitem = AssitenciaFlytour[0].Passeio.FirstOrDefault(p => p.CodigoAtividade == Conv.ToLong(CodigoAtividade));
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AssitenciaFlytour.FirstOrDefault().Passeio.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }
                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;
                Carrinhoitem.Comprar = true;
                atividade.Passeio.Add(Carrinhoitem);
                atividade.Comprar = true;
            }
            else if (TipoAtividade == "Transfer")
            {
                TransferModel Carrinhoitem = new TransferModel();
                Carrinhoitem = AssitenciaFlytour[0].Transfer.FirstOrDefault(p => p.CodigoAtividade == Conv.ToLong(CodigoAtividade));
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AssitenciaFlytour.FirstOrDefault().Transfer.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }
                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;
                atividade.Comprar = true;
                Carrinhoitem.Comprar = true;
                atividade.Transfer.Add(Carrinhoitem);
            }
            else if (TipoAtividade == "Restaurante")
            {
                RestauranteModel Carrinhoitem = new RestauranteModel();
                Carrinhoitem = AssitenciaFlytour[0].Restaurante.FirstOrDefault(p => p.CodigoAtividade == Conv.ToLong(CodigoAtividade));
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AssitenciaFlytour.FirstOrDefault().Restaurante.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }
                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;
                Carrinhoitem.Comprar = true;
                atividade.Comprar = true;
                atividade.Restaurante.Add(Carrinhoitem);
            }
            else if (TipoAtividade == "Ingresso")
            {
                IngressoModel Carrinhoitem = new IngressoModel();
                Carrinhoitem = AssitenciaFlytour[0].Ingresso.FirstOrDefault(p => p.CodigoAtividade == Conv.ToLong(CodigoAtividade));
                Carrinhoitem.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AssitenciaFlytour.FirstOrDefault().Ingresso.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                if (parcelas != null)
                {
                    Carrinhoitem.ValorTotal = Decimal.Multiply(Carrinhoitem.ValorTotal, Conv.ToDecimal(parcelas));
                }
                if (entrada != null && entrada != "")
                {
                    Carrinhoitem.ValorTotal = Carrinhoitem.ValorTotal + Convert.ToDecimal(entrada);
                }
                Carrinhoitem.dtEmbarque = Dia;
                atividade.Comprar = true;
                Carrinhoitem.Comprar = true;
                atividade.Ingresso.Add(Carrinhoitem);
            }

            var flyTour = AdicionarAtividadeCarrinho(atividade);

            List<MaxParcelaRetorno> finan = new List<MaxParcelaRetorno>();

            finan = pagamentoBussiness.RetornaFormasPagamento(flyTour, SessionUsuario, custo);

            StringBuilder retorno = new StringBuilder();
            retorno.Append("<table border='0' class='tbDetalhePreco'>");
            retorno.Append("<tbody>");

            foreach (var item in finan)
            {
                retorno.Append("<tr>");
                retorno.Append("<td>");

                if (!String.IsNullOrEmpty(item.DescricaoPlano))
                {
                    retorno.Append(item.DescricaoPlano);
                }
                else
                {
                    retorno.Append("Não existe parcelamento liberado.");
                }

                retorno.Append("</td>");
                retorno.Append("</tr>");
            }

            retorno.Append("</tbody>");
            retorno.Append("</table>");

            return retorno.ToString();
        }

        public FlyTourModel AdicionarAtividadeCarrinho(AtividadeModel Carrinhoitem)
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
            return flyTour;
        }
    }
}