using FT.Web.Business.Pacote;
using FT.Web.Business.Aereo;
using FT.Web.Business.Gerenciamento;
using FT.Web.Business.Orcamento;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models;
using FT.Web.Site.Controllers.Base;
using FTV;
using FTV.Shared.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace FT.Web.Site.Controllers.Orcamento
{
    [Tracer]
    public class OrcamentoController : _BaseController
    {
        OrcamentoBusiness orcamentoBusiness = new OrcamentoBusiness();
        //
        // GET: /Orcamento/

        public ActionResult Index()
        {
            var orcamentoModel = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () =>
            {
                return new OrcamentoModel();
            });

            return View(orcamentoModel);
        }

        public ActionResult ListarOrcamento()
        {
            return View();
        }

        public string SessaoDestino(OrcamentoModel orcamentos)
        {
            List<string> DestinosListados = new List<string>();

            Session["Destino"] = null;

            if (orcamentos.Hotel.Count() > 0)
            {
                foreach (var item in orcamentos.Hotel)
                {
                    DestinosListados.Add(item.NomeCidade + " - " + item.NomePais);
                }
            }

            if (orcamentos.Aereo.Count() > 0)
            {
                foreach (var item in orcamentos.Aereo)
                {
                    DestinosListados.Add(item.DestinoNome + "- Brasil");
                }
            }
            if (orcamentos.Carro.Count() > 0)
            {
                foreach (var item in orcamentos.Carro)
                {
                    foreach (var item2 in item.LocaisRetirada)
                    {
                        DestinosListados.Add(item2.NomeCidade + " - " + item2.NomePais);
                    }
                }
            }
            if (orcamentos.Atividade.Count() > 0)
            {
                foreach (var item in orcamentos.Atividade)
                {
                    DestinosListados.Add(item.NomeCidade + " - " + item.NomePais);
                }
            }
            if (orcamentos.Pacote.Count() > 0)
            {
                foreach (var item in orcamentos.Pacote)
                {
                    DestinosListados.Add(item.NomeCidade + " - " + item.NomePais);
                }
            }
            
            var Destinos = DestinosListados.Distinct().ToList();
            
            foreach (var item in Destinos)
            {
                if (Destinos.IndexOf(item) < Destinos.Count - 1)
                {
                    Session["Destino"] += item + " / ";
                }
                else {
                    Session["Destino"] += item;
                }
            }

            return "";
        }

        public ActionResult EditarOrcamento()
        {
            try
            {
                var Codigo = Request.QueryString["code"];
                string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
                var caminhoArquivo = diretorioArquivoXML + Codigo + ".xml";

                var orcamentos = orcamentoBusiness.CarregarDetalhesOrcamento(caminhoArquivo);
                if (orcamentos != null)
                {
                    var retorno = SessaoDestino(orcamentos);
                }
                return View(orcamentos);
            }
            catch (Exception)
            {
                return View("GetOrcamentos");
                throw;
            }
        }

        public ActionResult TestarXml()
        {
            //var orcamentoModel = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () => 
            //{
            //    return new OrcamentoModel();
            //});

            //OrcamentoBusiness orcamentoBusiness = new OrcamentoBusiness();

            //orcamentoBusiness.GravarArquivoXml(orcamentoModel);

            return View("Index");
        }

        public ActionResult LerXml()
        {
            //OrcamentoBusiness orcamentoBusiness = new OrcamentoBusiness();

            //orcamentoBusiness.CarregarArquivoXml();

            return View("Index");
        }

        public PartialViewResult ShowOrcamentoHotel(OrcamentoModel orcamento)
        {
            return PartialView(orcamento);
        }

        public PartialViewResult ShowOrcamentoAereo(OrcamentoModel orcamento)
        {
            return PartialView(orcamento);
        }

        public PartialViewResult ShowOrcamentoCarro(OrcamentoModel orcamento)
        {
            return PartialView(orcamento);
        }

        public PartialViewResult ShowOrcamentoPacote(OrcamentoModel orcamento)
        {
            return PartialView(orcamento);
        }

        public PartialViewResult ShowOrcamentoAtividade(OrcamentoModel orcamento)
        {
            return PartialView(orcamento);
        }
        [HttpPost]
        public PartialViewResult OrcamentoEmail(string destino, string codigo, string assunto, string mensagem, string[] acomodacoesSelecionadas)
        {
            var Codigo = codigo;
            string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
            var caminhoArquivo = diretorioArquivoXML + Codigo + ".xml";

            var orcamentoModel = orcamentoBusiness.CarregarDetalhesOrcamento(caminhoArquivo);

            for (int i = 0; i <= orcamentoModel.Hotel.Count - 1; i++)
            {
                string[] acomodacoes = acomodacoesSelecionadas.Where(a => a.Contains(String.Format("_{0}", i.ToString()))).ToArray();

                List<TarifaModel> tarifasSelecionadas = new List<TarifaModel>();

                List<AcomodacaoModel> lstAcomodacoesParaNaoRemover = new List<AcomodacaoModel>();

                foreach (string acomodacao in acomodacoes)
                {
                    string[] detalhesAcomodacaoSelecionada = acomodacao.Split('_');

                    foreach (var item in orcamentoModel.Hotel[i].Acomodacoes)
                    {
                        var paraNaoRemover = item.Tarifas.Where(t => t.NomeAcomodacao == detalhesAcomodacaoSelecionada[0] && t.NomeRegimeAlimentacao == detalhesAcomodacaoSelecionada[1]).ToList();

                        if (paraNaoRemover.Count > 0)
                        {
                            lstAcomodacoesParaNaoRemover.Add(item);

                            break;
                        }
                    }
                }

                var paraRemover = orcamentoModel.Hotel[i].Acomodacoes.Except(lstAcomodacoesParaNaoRemover).ToList();

                foreach (var item in paraRemover)
                {
                    orcamentoModel.Hotel[i].Acomodacoes.Remove(item);
                }
            }

            OrcamentoEmail orcamento = new OrcamentoEmail();

            orcamento.DataEnvio = DateTime.Now.ToShortDateString();
            orcamento.NumOrcamento = orcamentoModel.CodigoOrcamento.ToString();
            orcamento.Assunto = assunto;
            orcamento.Destino = destino;
            orcamento.Mensagem = mensagem;

            var dadosAgencia = PainelControle.GetPessoa(SessionUsuario.ID_PESSOA_AGENCIA.Value, SessionUsuario.ID_EMPRESA.Value, SessionUsuario.ID_PERFIL.Value);

            orcamento.Endereco = dadosAgencia.ENDERECO.DS_ENDERECO;
            orcamento.NumeroEndereco = dadosAgencia.ENDERECO.NR_ENDERECO.ToString();
            orcamento.Complemento = dadosAgencia.ENDERECO.DS_COMPLEMENTO;
            orcamento.Cidade = dadosAgencia.ENDERECO.DS_CIDADE;
            orcamento.Estado = dadosAgencia.ENDERECO.SG_ESTADO;
            orcamento.NomeAgencia = SessionUsuario.NM_PESSOA_AGENCIA;
            orcamento.DDD = dadosAgencia.NR_DDD;
            orcamento.Telefone = dadosAgencia.NR_TELEFONE;

            foreach (var Aereo in orcamentoModel.Aereo)
            {

                foreach (var Trechos in Aereo.Trechos)
                {
                    AereoOrcamento aereoOrca = new AereoOrcamento();

                    aereoOrca.QuantidadeAdulto = Aereo.QuantidadeAdulto.ToString();
                    aereoOrca.QuantidadeCrianca = Aereo.QuantidadeCrianca.ToString();
                    aereoOrca.QuantidadeBebe = Aereo.QuantidadeBebe.ToString();
                    aereoOrca.ImagemCia = Aereo.Trechos.FirstOrDefault().UrlLogoCia;
                    aereoOrca.TotalPassagens = Aereo.Valor.ToString("n2");
                    aereoOrca.Taxa = Aereo.ValorTaxa.ToString("n2");
                    aereoOrca.Total = (Aereo.Valor + Aereo.ValorTaxa).ToString("n2");

                    aereoOrca.LocalSaida = Trechos.NomeCidadeOrigem + Trechos.SiglaAeroportoOrigem;
                    aereoOrca.CiaSaida = Trechos.NomeCia;
                    aereoOrca.HorarioSaida = Trechos.DataOrigem.ToString();
                    aereoOrca.ParadaSaida = Trechos.QuantidadeConexao.ToString();
                    aereoOrca.NumVooSaida = Trechos.Seguimentos[0].NumeroVoo;
                    aereoOrca.ClasseSaida = Trechos.Seguimentos[0].Cabine;

                    aereoOrca.LocalChegada = Trechos.NomeCidadeDestino + Trechos.NomeAeroportoDestino;
                    aereoOrca.CiaChegada = Trechos.NomeCia;
                    aereoOrca.HorarioChegada = Trechos.DataDestino.ToString();
                    aereoOrca.ParadaChegada = Trechos.QuantidadeConexao.ToString();
                    aereoOrca.NumVooChegada = Trechos.Seguimentos[0].NumeroVoo;
                    aereoOrca.ClasseChegada = Trechos.Seguimentos[0].Cabine;

                    orcamento.Aereo.Add(aereoOrca);
                }

            }


            foreach (var item in orcamentoModel.Pacote)
            {
                PacoteOrcamento PacoteOrca = new PacoteOrcamento();

                var totaldias = 0;
                var Periodo = "";

                PacoteOrca.Titulo = item.NomePacote;

                if (item.Aereo.Count > 0)
                {
                    DateTime data = Convert.ToDateTime(item.Aereo.FirstOrDefault().DataRetorno);
                    DateTime datainicial = Convert.ToDateTime(item.Aereo[1].DataEmbarque);
                    totaldias = ((datainicial - data).Days);
                }
                else if (item.Hotel.Count > 0)
                {
                    DateTime data = Convert.ToDateTime(item.Hotel.FirstOrDefault().Checkout);
                    DateTime datainicial = Convert.ToDateTime(item.Hotel.FirstOrDefault().Checkin);
                    totaldias = ((datainicial - data).Days);
                }

                PacoteOrca.Dias = totaldias.ToString();

                if (item.Aereo.Count > 0)
                {
                    int dataMonth = Convert.ToDateTime(item.Aereo.FirstOrDefault().DataEmbarque).Month;
                    int dataYear = Convert.ToDateTime(item.Aereo.FirstOrDefault().DataEmbarque).Year;
                    Periodo = dataMonth.ToString() + "/" + dataYear.ToString();
                }
                else if (item.Hotel.Count > 0)
                {
                    int dataMonth = Convert.ToDateTime(item.Hotel.FirstOrDefault().Checkin).Month;
                    int dataYear = Convert.ToDateTime(item.Hotel.FirstOrDefault().Checkin).Year;
                    Periodo = dataMonth.ToString() + "/" + dataYear.ToString();
                }

                PacoteOrca.Periodo = Periodo;

                if (item.Aereo.Count > 0)
                    PacoteOrca.Saida = item.Aereo.FirstOrDefault().DataEmbarque.ToString();
                else if (item.Hotel.Count > 0)
                    PacoteOrca.Saida = item.Hotel.FirstOrDefault().Checkout.ToString();

                foreach (var itens in item.PacoteDescricaoServico)
                {
                    PacoteOrca.ItensInclusos += String.Format("{0} <br />", itens.Descricao);
                }

                foreach (var itemRegime in item.Regime)
                {
                    PacoteOrca.Valorpacote = itemRegime.ValorTotalSemTaxas.ToString("n2");
                    PacoteOrca.Taxas = itemRegime.Taxas.ToString("n2");
                    PacoteOrca.Total = itemRegime.ValorTotal.ToString("n2");
                }



                foreach (var itens in item.Aereo)
                {
                    AereoPacoteOrcamento AereoOrcamento = new AereoPacoteOrcamento();

                    AereoOrcamento.Imagem = itens.FotoUrl;
                    AereoOrcamento.LocalSaida = item.Aereo[0].Origem;
                    AereoOrcamento.InfoSaida = item.Aereo[0].DescricaoVoo;
                    AereoOrcamento.HorarioSaida = item.Aereo[0].DataEmbarque;
                    AereoOrcamento.HorarioSaidaChegada = item.Aereo[0].DataRetorno;

                    AereoOrcamento.LocalChagada = item.Aereo[1].Origem;
                    AereoOrcamento.InfoChegada = item.Aereo[1].DescricaoVoo;
                    AereoOrcamento.HorarioChegada = item.Aereo[1].DataEmbarque;
                    AereoOrcamento.HorarioChegadaChegada = item.Aereo[1].DataRetorno;

                    PacoteOrca.Aereo.Add(AereoOrcamento);
                }




                foreach (var itemRegime in item.Hotel)
                {
                    HotelPacoteOrcamento HotelOrcamento = new HotelPacoteOrcamento();

                    HotelOrcamento.NomeHotel = itemRegime.NomeHotel;
                    HotelOrcamento.Estrelas = itemRegime.Estrelas.ToString();
                    HotelOrcamento.Checkin = itemRegime.Checkin;
                    HotelOrcamento.Checkout = itemRegime.Checkout;
                    foreach (var itemRegimes in item.Regime)
                    {
                        HotelOrcamento.Quarto = itemRegimes.Quarto;
                        HotelOrcamento.Regime = itemRegimes.TipoRegime;
                    }


                    if (itemRegime.HotelFotos != null && itemRegime.HotelFotos.Count >= 1)
                    {
                        HotelOrcamento.Imagem = itemRegime.HotelFotos[0].HotelFotoUrl;
                    }
                    else
                    {
                        HotelOrcamento.Imagem = "http://www.flytour.com/images/no-image.png";
                    }
                

                    PacoteOrca.Hotel.Add(HotelOrcamento);
                }


                orcamento.Pacote.Add(PacoteOrca);
            }

            foreach (var Hotel in orcamentoModel.Hotel)
            {
                HotelOrcamento HotelOrca = new HotelOrcamento();

                HotelOrca.NomeHotel = Hotel.NomeHotel;
                HotelOrca.Estrelas = Hotel.Estrelas.ToString();
                HotelOrca.Periodo = Hotel.Checkin + " a " + Hotel.Checkout;

                if (Hotel.HotelFotos != null && Hotel.HotelFotos.Count >= 1)
                {
                    HotelOrca.Imagem = Hotel.HotelFotos[0].HotelFotoUrl;
                }
                else
                {
                    HotelOrca.Imagem = "http://www.flytour.com/images/no-image.png";
                }
                


                HotelOrca.Endereco = Hotel.Endereco + " - " + Hotel.NomeCidade + " - " + Hotel.NomeEstado + " - " + Hotel.Cep;
                HotelOrca.Apartamentos = Hotel.Apartamentos;


                foreach (var tarifa in Hotel.Acomodacoes.SelectMany(p => p.Tarifas).OrderBy(p => p.ValorTotal).ToList())
                {
                    AcomodacaoOrcamento acomodacao = new AcomodacaoOrcamento();

                    acomodacao.Quarto = tarifa.NomeAcomodacao;
                    acomodacao.Regime = tarifa.NomeRegimeAlimentacao;
                    acomodacao.DiariaMedia = tarifa.ValorDiariaMedia.ToString("n2");
                    acomodacao.TotalPeriodo = tarifa.ValorTotal.ToString("n2");
                    HotelOrca.Acomodacao.Add(acomodacao);
                }

                orcamento.Hoteis.Add(HotelOrca);
            }

            foreach (var Atividade in orcamentoModel.Atividade)
            {
                AtividadeOrcamento atividadeOrca = new AtividadeOrcamento();

                atividadeOrca.AssistenciaViagem = Atividade.AssistenciaViagem;
                atividadeOrca.Transfer = Atividade.Transfer;
                atividadeOrca.Passeio = Atividade.Passeio;
                atividadeOrca.Ingresso = Atividade.Ingresso;
                atividadeOrca.PacoteAtividade = Atividade.PacoteAtividade;
                atividadeOrca.Restaurante = Atividade.Restaurante;
                atividadeOrca.dtEmbarque = Atividade.dtEmbarque;
                atividadeOrca.ValorTotalExibe = Atividade.ValorTotalExibe;

                orcamento.Atividade.Add(atividadeOrca);
            }


            foreach (var Carro in orcamentoModel.Carro)
            {
                CarroOrcamento carroOrca = new CarroOrcamento();

                carroOrca.TipoCarro = Carro.ModeloReferencia;
                carroOrca.ImagemCarro = Carro.CarrosFoto.FirstOrDefault().CarroFotoLink;
                carroOrca.ImagemLocadora = Carro.UrlLogoLocadora;
                carroOrca.OBS = Carro.Plano;
                carroOrca.Total = Carro.ValorTotal.ToString();
                carroOrca.LocalRetirada = Carro.LocaisRetirada.FirstOrDefault().NomeCidade + " - "
                                          + Carro.LocaisRetirada.FirstOrDefault().NomeEstado + " - "
                                          + Carro.LocaisRetirada.FirstOrDefault().NomePais + " - "
                                          + Carro.LocaisRetirada.FirstOrDefault().NomeLocadoraLoja;
                carroOrca.DataDRetirada = Carro.DataCheckIn + " + " + Carro.HoraCheckInSelecionada;
                carroOrca.LocalDevolucao = Carro.LocaisDevolucao.FirstOrDefault().NomeCidade + " - "
                                          + Carro.LocaisDevolucao.FirstOrDefault().NomeEstado + " - "
                                          + Carro.LocaisDevolucao.FirstOrDefault().NomePais + " - "
                                          + Carro.LocaisDevolucao.FirstOrDefault().NomeLocadoraLoja;
                carroOrca.DataDevolucao = Carro.DataCheckOut + " + " + Carro.HoraCheckOutSelecionada;

                orcamento.Carro.Add(carroOrca);
            }

            return PartialView(orcamento);

        }

        [HttpPost]
        public bool arquivarOrcamento(string CodigoOrcamento)
        {
            try
            {
                string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
                var caminhoArquivo = diretorioArquivoXML + CodigoOrcamento + ".xml";

                //FT.Web.Core.Util.SerializacaoXml<OrcamentoModel> serializacaoXML = new Core.Util.SerializacaoXml<OrcamentoModel>();

                OrcamentoModel orcamentoXML = orcamentoBusiness.CarregarDetalhesOrcamento(CodigoOrcamento);  //serializacaoXML.LerXml(caminhoArquivo);

                orcamentoBusiness.ArquivarOrcamento(orcamentoXML);
                

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost]
        public string atualizaDadosOrcamento(long CodigoOrcamento, string Nome, string EmailEnvio, string Assunto, string Mensagem)
        {
            try
            {
                var orcamentoModel = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () =>
                {
                    return new OrcamentoModel();
                });

                string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
                var caminhoArquivo = diretorioArquivoXML + CodigoOrcamento + ".xml";

                //FT.Web.Core.Util.SerializacaoXml<OrcamentoModel> serializacaoXML = new Core.Util.SerializacaoXml<OrcamentoModel>();

                OrcamentoModel orcamentoXML = orcamentoBusiness.CarregarDetalhesOrcamento(caminhoArquivo); //serializacaoXML.LerXml(caminhoArquivo);

                if (orcamentoModel.Hotel.Count > 0)
                    orcamentoXML.Hotel.AddRange(orcamentoModel.Hotel);

                if (orcamentoModel.Carro.Count > 0)
                    orcamentoXML.Carro.AddRange(orcamentoModel.Carro);

                if (orcamentoModel.Pacote.Count > 0)
                    orcamentoXML.Pacote.AddRange(orcamentoModel.Pacote);

                if (orcamentoModel.Aereo.Count > 0)
                    orcamentoXML.Aereo.AddRange(orcamentoModel.Aereo);

                if (orcamentoModel.Atividade.Count > 0)
                    orcamentoXML.Atividade.AddRange(orcamentoModel.Atividade);


                orcamentoXML.NomeDestinatario = Nome;
                orcamentoXML.EmailDestinatario = EmailEnvio;
                orcamentoXML.Assunto = Assunto;
                orcamentoXML.Mensagem = Mensagem;

                orcamentoXML.CaminhoArquivoXml = caminhoArquivo;

                orcamentoBusiness.AtualizarOrcamento(orcamentoXML);
                //orcamentoBusiness.AtualizarOrcamentoSincronismo(orcamentoModel.CodigoOrcamento, caminhoArquivo);

                var success = AppCache.Put(EnumProcesso.Orcamento, SessionKey, new OrcamentoModel(), TimeSpan.FromMinutes(90));

                return Convert.ToString(CodigoOrcamento);
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpPost]
        public string atualizarOrcamento(long CodigoOrcamento)
        {
            try
            {
                var orcamentoModel = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () =>
                {
                    return new OrcamentoModel();
                });

                string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
                var caminhoArquivo = diretorioArquivoXML + CodigoOrcamento + ".xml";

                //FT.Web.Core.Util.SerializacaoXml<OrcamentoModel> serializacaoXML = new Core.Util.SerializacaoXml<OrcamentoModel>();

                OrcamentoModel orcamentoXML = orcamentoBusiness.CarregarDetalhesOrcamento(caminhoArquivo); //serializacaoXML.LerXml(caminhoArquivo);

                if (orcamentoModel.Hotel.Count > 0)
                    orcamentoXML.Hotel.AddRange(orcamentoModel.Hotel);

                if (orcamentoModel.Carro.Count > 0)
                    orcamentoXML.Carro.AddRange(orcamentoModel.Carro);

                if (orcamentoModel.Pacote.Count > 0)
                    orcamentoXML.Pacote.AddRange(orcamentoModel.Pacote);

                if (orcamentoModel.Aereo.Count > 0)
                    orcamentoXML.Aereo.AddRange(orcamentoModel.Aereo);

                if (orcamentoModel.Atividade.Count > 0)
                    orcamentoXML.Atividade.AddRange(orcamentoModel.Atividade);

                orcamentoXML.CaminhoArquivoXml = caminhoArquivo;

                orcamentoBusiness.AtualizarOrcamento(orcamentoXML);
                //orcamentoBusiness.AtualizarOrcamentoSincronismo(orcamentoModel.CodigoOrcamento, caminhoArquivo);

                var success = AppCache.Put(EnumProcesso.Orcamento, SessionKey, new OrcamentoModel(), TimeSpan.FromMinutes(90));

                return Convert.ToString(CodigoOrcamento);
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpPost]
        public string SalvarOrcamento(string NomeOrcamento)
        {
            try
            {
                var orcamentoModel = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () =>
                {
                    return new OrcamentoModel();
                });

                orcamentoModel.DataOrcamento = DateTime.Now;
                orcamentoModel.CodigoUsuario = Conv.ToLong(SessionUsuario.ID_PESSOA);
                orcamentoModel.CodigoAgencia = Conv.ToLong(SessionUsuario.ID_PESSOA_AGENCIA);
                orcamentoModel.IndicadorArquivado = false;
                orcamentoModel.NomeOrcamento = NomeOrcamento;
                orcamentoModel.Usuario.ID_PESSOA_AGENCIA = Conv.ToLong(SessionUsuario.ID_PESSOA_AGENCIA);
                orcamentoModel.Usuario.ID_PESSOA = Conv.ToLong(SessionUsuario.ID_PESSOA);
                ViewBag.ListaOrcamento = orcamentoBusiness.CadastrarOrcamento(orcamentoModel);

                var success = AppCache.Put(EnumProcesso.Orcamento, SessionKey, new OrcamentoModel(), TimeSpan.FromMinutes(90));

                return Convert.ToString(ViewBag.ListaOrcamento);
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpPost]
        public PartialViewResult GetOrcamentos(string datainicial, string datafinal, string codigo, int status)
        {
            if (EstaLogado)
            {
                OrcamentoPesquisaModel orcamento = new OrcamentoPesquisaModel();
                orcamento.DataInicial = datainicial;
                orcamento.DataFinal = datafinal;

                if (!String.IsNullOrEmpty(codigo))
                    orcamento.CodigoOrcamento = Conv.ToLong(codigo);

                orcamento.IsArquivado = false;
                orcamento.Usuario = SessionUsuario;

                var orcamentos = orcamentoBusiness.ConsultarOrcamentos(orcamento);

                return PartialView(orcamentos);
            }
            else
            {
                return PartialView();
            }
        }

        [HttpPost]
        public PartialViewResult ShowOrcamento(string SessionKey)
        {
            var Orcamento = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () =>
            {
                return new OrcamentoModel();
            });

            try
            {
                OrcamentoCadastradoModel orcamentoCadastradoModel = new OrcamentoCadastradoModel();
                orcamentoCadastradoModel.CodigoPessoaAgencia = Conv.ToLong(SessionUsuario.ID_PESSOA_AGENCIA);
                orcamentoCadastradoModel.CodigoPessoaUsuario = Conv.ToLong(SessionUsuario.ID_PESSOA);
                orcamentoCadastradoModel.IsMaster = false;

                List<OrcamentoCadastradoModel> orcamentoCadastrados = orcamentoBusiness.RetornarOrcamentosJaExistentes(orcamentoCadastradoModel, false);

                ViewBag.ListaOrcamentos = "<option value='0' class='dropdown-cotizacao-itens'>Selecione</option>";
                foreach (var item in orcamentoCadastrados)
                {
                    ViewBag.ListaOrcamentos += "<option value='" + item.CodigoOrcamento + "' class='dropdown-cotizacao-itens'>" + item.CodigoOrcamento + " - " + item.NomeOrcamento + " - " + item.NomeAgente + " - " + item.Conteudo + "</option>";
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return PartialView(Orcamento);
        }

        [HttpPost]
        public PartialViewResult GetOrcamentosInativos(string datainicial, string datafinal, string codigo, int status)
        {
            OrcamentoPesquisaModel orcamento = new OrcamentoPesquisaModel();
            orcamento.DataInicial = datainicial;
            orcamento.DataFinal = datafinal;
            orcamento.CodigoOrcamento = Conv.ToLong(codigo);
            orcamento.IsArquivado = true;
            orcamento.Usuario = SessionUsuario;

            var orcamentos = orcamentoBusiness.ConsultarOrcamentos(orcamento);

            return PartialView("GetOrcamentos", orcamentos);
        }

        [HttpPost]
        public JsonResult AdicionarPacoteOrcamento(string CodigoPacote)
        {
            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var PacotesFlytour = AppCache.Get<List<PacoteVendaModel>>(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", 90, () =>
            {
                return new List<PacoteVendaModel>();
            });

            var orcamentoModel = new OrcamentoModel();
            var pacote = new PacoteVendaModel();

            if (PacotesFlytour != null && PacotesFlytour.Count > 0)
            {
                var PacoteCarrinho = PacotesFlytour.FirstOrDefault(p => p.CodigoPacote == CodigoPacote);

                if (PacoteCarrinho.Regime.Any(p => p.ValorTotal == 0))
                {
                    var pacotesDisponiveis = PacoteBusiness.ConsultarPacoteVenda(PacoteCarrinho.CodigoPacote, PacoteCarrinho.Apartamento);
                    if (pacotesDisponiveis != null && pacotesDisponiveis.Count > 0)
                        PacoteCarrinho = pacotesDisponiveis.First();
                }

                orcamentoModel.Apartamentos = PacoteCarrinho.Apartamento;
                orcamentoModel.Pacote = new List<PacoteVendaModel>();
                orcamentoModel.Pacote.Add(PacoteCarrinho);
            }

            AdicionarOrcamento(orcamentoModel);

            return Json(SessionKey);
        }

        #region Adicionar Atividades no Carrinho
        /*Objetos das atividades*/
        [HttpPost]
        public JsonResult AdicionarAtividadeAssistenciaOrcamento(long orcamentoItem, string Valor, string Dia)
        {
            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));
            var atividade = new AtividadeModel();
            AssistenciaViagemModel AssistenciaViagemFlytour = new AssistenciaViagemModel();

            if (AtividadeFlytour != null && AtividadeFlytour.Count > 0)
            {
                AssistenciaViagemFlytour = AtividadeFlytour[0].AssistenciaViagem.FirstOrDefault(p => p.CodigoAtividade == orcamentoItem);
                //AssistenciaViagemFlytour.ValorTotal = Convert.ToDecimal(Valor);
                atividade.NomeCidade = AtividadeFlytour[0].NomeCidade;


            }


            atividade.AssistenciaViagem.Add(AssistenciaViagemFlytour);
            atividade.CodigoAtividade = AssistenciaViagemFlytour.CodigoAtividade;
            AdicionarAtividadeOrcamento(atividade);

            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeTransferOrcamento(long orcamentoItem, string Valor, string Dia, string entrada, long? parcelas)
        {
            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));
            var atividade = new AtividadeModel();
            TransferModel TrasnferFlytour = new TransferModel();

            if (AtividadeFlytour != null && AtividadeFlytour.Count > 0)
            {

                TrasnferFlytour = AtividadeFlytour[0].Transfer.FirstOrDefault(p => p.CodigoAtividade == orcamentoItem);
                atividade.NomeCidade = AtividadeFlytour[0].NomeCidade;
                atividade.dtEmbarque = Dia;
                atividade.ValorTotalExibe = Valor;
            }


            TrasnferFlytour.ValorTotal = Conv.ToDecimal(Valor);
            TrasnferFlytour.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), TrasnferFlytour.Apartamento.FirstOrDefault().Passageiros.Count());

            if (parcelas != null)
            {
                TrasnferFlytour.ValorTotal = Decimal.Multiply(TrasnferFlytour.ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                TrasnferFlytour.ValorTotal = TrasnferFlytour.ValorTotal + Convert.ToDecimal(entrada);
            }

            TrasnferFlytour.dtEmbarque = Dia;
            TrasnferFlytour.dtEmbarque = Dia;

            atividade.CodigoAtividade = TrasnferFlytour.CodigoAtividade;

            atividade.Transfer.Add(TrasnferFlytour);
            AdicionarAtividadeOrcamento(atividade);

            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeIngressoOrcamento(long orcamentoItem, string Valor, string Dia, string entrada, long? parcelas)
        {
            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));
            var atividade = new AtividadeModel();
            IngressoModel IngressosFlytour = new IngressoModel();

            if (AtividadeFlytour != null && AtividadeFlytour.Count > 0)
            {

                IngressosFlytour = AtividadeFlytour[0].Ingresso.FirstOrDefault(p => p.CodigoAtividade == orcamentoItem);
                //IngressosFlytour.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AtividadeFlytour.FirstOrDefault().Ingresso.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());
                atividade.NomeCidade = AtividadeFlytour[0].NomeCidade;
                atividade.dtEmbarque = Dia;
                atividade.ValorTotalExibe = Valor;
            }

            IngressosFlytour.ValorTotal = Conv.ToDecimal(Valor);
            IngressosFlytour.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), IngressosFlytour.Apartamento.FirstOrDefault().Passageiros.Count());

            if (parcelas != null)
            {
                IngressosFlytour.ValorTotal = Decimal.Multiply(IngressosFlytour.ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                IngressosFlytour.ValorTotal = IngressosFlytour.ValorTotal + Convert.ToDecimal(entrada);
            }

            IngressosFlytour.dtEmbarque = Dia;

            atividade.CodigoAtividade = IngressosFlytour.CodigoAtividade;


            atividade.Ingresso.Add(IngressosFlytour);
            AdicionarAtividadeOrcamento(atividade);

            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadePasseioOrcamento(long orcamentoItem, string Valor, string Dia, string entrada, long? parcelas)
        {
            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));

            PasseioModel PasseioFlytour = new PasseioModel();
            var atividade = new AtividadeModel();

            if (AtividadeFlytour != null && AtividadeFlytour.Count > 0)
            {

                PasseioFlytour = AtividadeFlytour[0].Passeio.FirstOrDefault(p => p.CodigoAtividade == orcamentoItem);
                atividade.NomeCidade = AtividadeFlytour[0].NomeCidade;

                atividade.dtEmbarque = Dia;
                atividade.ValorTotalExibe = Valor;
            }


            PasseioFlytour.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), AtividadeFlytour.FirstOrDefault().Passeio.FirstOrDefault().Apartamento.FirstOrDefault().Passageiros.Count());

            if (parcelas != null)
            {
                PasseioFlytour.ValorTotal = Decimal.Multiply(PasseioFlytour.ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                PasseioFlytour.ValorTotal = PasseioFlytour.ValorTotal + Convert.ToDecimal(entrada);
            }

            PasseioFlytour.dtEmbarque = Dia;

            atividade.CodigoAtividade = PasseioFlytour.CodigoAtividade;

            atividade.Passeio.Add(PasseioFlytour);
            AdicionarAtividadeOrcamento(atividade);

            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadeRestauranteOrcamento(long orcamentoItem, string Valor, string Dia, string entrada, long? parcelas)
        {
            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));

            var atividade = new AtividadeModel();
            RestauranteModel RestauranteFlytour = new RestauranteModel();

            if (AtividadeFlytour != null && AtividadeFlytour.Count > 0)
            {

                RestauranteFlytour = AtividadeFlytour[0].Restaurante.FirstOrDefault(p => p.CodigoAtividade == orcamentoItem);
                atividade.NomeCidade = AtividadeFlytour[0].NomeCidade;

                atividade.dtEmbarque = Dia;
                atividade.ValorTotalExibe = Valor;
            }

            RestauranteFlytour.ValorTotal = Conv.ToDecimal(Valor);
            RestauranteFlytour.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), RestauranteFlytour.Apartamento.FirstOrDefault().Passageiros.Count());

            if (parcelas != null)
            {
                RestauranteFlytour.ValorTotal = Decimal.Multiply(RestauranteFlytour.ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                RestauranteFlytour.ValorTotal = RestauranteFlytour.ValorTotal + Convert.ToDecimal(entrada);
            }

            RestauranteFlytour.dtEmbarque = Dia;

            atividade.CodigoAtividade = RestauranteFlytour.CodigoAtividade;

            atividade.Restaurante.Add(RestauranteFlytour);
            AdicionarAtividadeOrcamento(atividade);

            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAtividadePacoteAtividadeOrcamento(long orcamentoItem, string Valor, string Dia, string entrada, long? parcelas)
        {
            var AtividadeFlytour = AppCache.Get<List<AtividadeModel>>(EnumProcesso.ServicoVenda, SessionKey + "Servicos", 90, () =>
            {
                return new List<AtividadeModel>();
            });

            var success = AppCache.Put(EnumProcesso.ServicoVenda, SessionKey + "Servicos", null, TimeSpan.FromMinutes(90));
            var atividade = new AtividadeModel();
            PacoteAtividadeModel PacoteAtividadeFlytour = new PacoteAtividadeModel();

            if (AtividadeFlytour != null && AtividadeFlytour.Count > 0)
            {

                PacoteAtividadeFlytour = AtividadeFlytour[0].PacoteAtividade.FirstOrDefault(p => p.CodigoAtividade == orcamentoItem);
                atividade.NomeCidade = AtividadeFlytour[0].NomeCidade;
                atividade.dtEmbarque = Dia;
                atividade.ValorTotalExibe = Valor;
            }


            PacoteAtividadeFlytour.ValorTotal = Conv.ToDecimal(Valor);
            PacoteAtividadeFlytour.ValorTotal = Decimal.Multiply(Conv.ToDecimal(Valor), PacoteAtividadeFlytour.Apartamento.FirstOrDefault().Passageiros.Count());

            if (parcelas != null)
            {
                PacoteAtividadeFlytour.ValorTotal = Decimal.Multiply(PacoteAtividadeFlytour.ValorTotal, Conv.ToDecimal(parcelas));
            }

            if (entrada != null && entrada != "")
            {
                PacoteAtividadeFlytour.ValorTotal = PacoteAtividadeFlytour.ValorTotal + Convert.ToDecimal(entrada);
            }

            PacoteAtividadeFlytour.dtEmbarque = Dia;

            atividade.CodigoAtividade = PacoteAtividadeFlytour.CodigoAtividade;

            atividade.PacoteAtividade.Add(PacoteAtividadeFlytour);
            AdicionarAtividadeOrcamento(atividade);

            return Json(SessionKey);
        }

        /*Fim dos objetos das atividades*/

        #endregion

        [HttpPost]
        public JsonResult AdicionarAtividadeOrcamento(AtividadeModel orcamentoItem)
        {
            var orcamentoModel = new OrcamentoModel();

            orcamentoModel.Atividade = new List<AtividadeModel>();
            orcamentoModel.Atividade.Add(orcamentoItem);

            AdicionarOrcamento(orcamentoModel);

            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarHotelOrcamento(long orcamentoItem, string NomeAcomodacao, string CodigoTarifa)
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

            var success = AppCache.Put(EnumProcesso.HotelVenda, SessionKey + "Hotel", null, TimeSpan.FromMinutes(90));

            HotelModel PacoteHotel = new HotelModel();

            if (HotelFlytour != null && HotelFlytour.Count > 0)
            {
                PacoteHotel = HotelFlytour.FirstOrDefault(p => p.CodigoHotel == orcamentoItem);
            }

            var CodigoAcomodacao = PacoteHotel.Acomodacoes.Where(p => p.NomeAcomodacao == NomeAcomodacao).FirstOrDefault().CodigoAcomodacao;

            var orcamentoModel = new OrcamentoModel();

            orcamentoModel.Hotel = new List<HotelModel>();

            orcamentoModel.Hotel.Add(PacoteHotel);
            AdicionarOrcamento(orcamentoModel);
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarAereoOrcamento(AereoModel orcamentoItem)
        {
            try
            {
                bool aereoTripEngine = false;
            var configAereoTripEngine = ConfigurationManager.AppSettings["aereoTripEngine"];
            if (configAereoTripEngine != null)
                aereoTripEngine = Convert.ToBoolean(configAereoTripEngine);

            if (aereoTripEngine)
            {
                for (int i = 0; i < orcamentoItem.TrechosSel.Count; i++)
                {
                    orcamentoItem.CodigoTarifas.Add(orcamentoItem.TrechosSel[i]);
                }
            }
            else
            {
                var reco1 = orcamentoItem.TrechosSel[0].Split(';');
                if (orcamentoItem.TrechosSel.Count > 1)
                {

                    var reco2 = orcamentoItem.TrechosSel[1].Split(';');
                    var codigoTarifa = orcamentoItem.CodigoTarifa;

                    if (orcamentoItem.TrechosSel.Count > 2)
                    {
                        var reco3 = orcamentoItem.TrechosSel[2].Split(';');
                        var inter = reco1.Intersect(reco2).Intersect(reco3);
                        var intersec = inter.First(p => p != String.Empty);
                        orcamentoItem.Recomendation = Conv.ToInt32(intersec);
                    }
                    else
                    {

                        var inter = reco1.Intersect(reco2);
                        var intersec = inter.First(p => p != String.Empty);
                        orcamentoItem.Recomendation = Conv.ToInt32(intersec);
                    }
                }
                else
                {
                    var intersec = reco1.First();
                    orcamentoItem.Recomendation = Conv.ToInt32(intersec);
                }
            }

                var tarifa = AereoBusiness.Tarifar(orcamentoItem);

                var orcamentoModel = new OrcamentoModel();

                var trechosAer = AereoBusiness.MapearTrecho(tarifa.Trechos);
                orcamentoItem.ValorTaxa = tarifa.ValorTaxa;
                orcamentoItem.Valor = Conv.ToDecimal(tarifa.ValorVenda.ToString("n2"));
                orcamentoItem.Trechos.Clear();
                foreach (var item in trechosAer)
                {
                    orcamentoItem.Trechos.Add(item);
                }
                orcamentoModel.Aereo.Add(orcamentoItem);
                AdicionarOrcamento(orcamentoModel);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(SessionKey);
        }

        [HttpPost]
        public JsonResult AdicionarCarroOrcamento(ReservaCarroModel orcamentoItem)
        {
            var orcamentoModel = new OrcamentoModel();

            orcamentoModel.Carro.Add(orcamentoItem);
            AdicionarOrcamento(orcamentoModel);
            return Json(SessionKey);
        }

        private JsonResult AdicionarOrcamento(OrcamentoModel orcamentoModel)
        {
            var addedFlyTour = AppCache.Get<OrcamentoModel>(EnumProcesso.Orcamento, SessionKey, 90, () =>
            {
                return new OrcamentoModel();
            });
            
            if (orcamentoModel.Atividade.Count > 0)
            {
                addedFlyTour.Atividade.AddRange(orcamentoModel.Atividade);
            }

            if (orcamentoModel.Hotel.Count > 0)
            {
                addedFlyTour.Hotel.AddRange(orcamentoModel.Hotel);
            }

            if (orcamentoModel.Pacote.Count > 0)
            {
                addedFlyTour.Pacote.AddRange(orcamentoModel.Pacote);
            }

            if (orcamentoModel.Carro.Count > 0)
            {
                addedFlyTour.Carro.AddRange(orcamentoModel.Carro);
            }

            if (orcamentoModel.Aereo.Count > 0)
            {
                addedFlyTour.Aereo.AddRange(orcamentoModel.Aereo);
            }

            var success = AppCache.Put(EnumProcesso.Orcamento, SessionKey, addedFlyTour, TimeSpan.FromMinutes(90));

            return Json(SessionKey);
        }

        public PartialViewResult Teste()
        {
            return PartialView();
        }

        public PartialViewResult ShowOrcamentoHotelEmail()
        {
            return PartialView();
        }

        public PartialViewResult ShowOrcamentoAereoEmail()
        {
            return PartialView();
        }

        [HttpPost]
        public void noisqueta(string html, string assunto, string email, string nome, string mensagem, string codigo)
        {
            
            var x = orcamentoBusiness.EnvairEmailFinal(HttpUtility.HtmlDecode(html), assunto, email, nome, mensagem, codigo, SessionUsuario);
        }

        [HttpPost]
        public void RemoverHotel(string codigoHotel, string codigoOrcamento)
        {
            try
            {
                var Codigo = codigoOrcamento;
                string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
                var caminhoArquivo = diretorioArquivoXML + Codigo + ".xml";

                var orcamento = orcamentoBusiness.CarregarDetalhesOrcamento(caminhoArquivo);
                long? codigoHotelConv = Conv.ToLong(codigoHotel);

                var hotelRemovido = orcamento.Hotel.Where(p => p.CodigoHotel == codigoHotelConv).FirstOrDefault();

                if (orcamento.Hotel.Remove(hotelRemovido))
                {
                    orcamentoBusiness.AtualizarOrcamento(orcamento);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public void RemoverItemOrcamento(string codigoItem, string codigoOrcamento, EnumItemOrcamento itemOrcamento)
        {
            try
            {
                var Codigo = codigoOrcamento;
                string diretorioArquivoXML = ConfigurationManager.AppSettings["CaminhoXmlOrcamento"];
                var caminhoArquivo = diretorioArquivoXML + Codigo + ".xml";
                var orcamento = orcamentoBusiness.CarregarDetalhesOrcamento(caminhoArquivo);

                switch (itemOrcamento)
                {
                    case EnumItemOrcamento.Hotel:
                        long? codigoHotelConv = Conv.ToLong(codigoItem);
                        if (codigoHotelConv.HasValue)
                        {
                            var hotelRemovido = orcamento.Hotel.Where(p => p.CodigoHotel == codigoHotelConv).FirstOrDefault();
                            if (orcamento.Hotel.Remove(hotelRemovido))
                                orcamentoBusiness.AtualizarOrcamento(orcamento); 
                        }
                        break;

                    case EnumItemOrcamento.Aereo:
                        long? codigoAereoConv = Conv.ToLong(codigoItem);
                        if (codigoAereoConv.HasValue)
                        {
                            var AereoRemovido = orcamento.Aereo.Where(p => p.CodigoTarifa == codigoAereoConv).FirstOrDefault();
                            if (orcamento.Aereo.Remove(AereoRemovido))
                                orcamentoBusiness.AtualizarOrcamento(orcamento); 
                        }
                        break;

                    case EnumItemOrcamento.Pacote:
                        if (!String.IsNullOrEmpty(codigoItem))
                        {
                            var PacoteRemovido = orcamento.Pacote.Where(p => p.CodigoPacote == codigoItem).FirstOrDefault();
                            if (orcamento.Pacote.Remove(PacoteRemovido))
                                orcamentoBusiness.AtualizarOrcamento(orcamento); 
                        }
                        break;

                    case EnumItemOrcamento.Atividade:
                        long? codigoAtividadeConv = Conv.ToLong(codigoItem);
                        if (codigoAtividadeConv.HasValue)
                        {
                            var AtividadeRemovido = orcamento.Atividade.Where(p => p.CodigoAtividade == codigoAtividadeConv).FirstOrDefault();
                            if (orcamento.Atividade.Remove(AtividadeRemovido))
                                orcamentoBusiness.AtualizarOrcamento(orcamento); 
                        }
                        break;

                    case EnumItemOrcamento.Carro:
                        if (!String.IsNullOrEmpty(codigoItem))
                        {
                            var CarroRemovido = orcamento.Carro.Where(p => p.CodigoTarifa == codigoItem).FirstOrDefault();
                            if (orcamento.Carro.Remove(CarroRemovido))
                                orcamentoBusiness.AtualizarOrcamento(orcamento); 
                        }
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public JsonResult AdicionarMontePacoteOrcamento()
        {
            var orcamento = new OrcamentoModel();

            foreach (var destino in MPSession.Destinos)
            {
                if (destino.ReservaCarro != null && !String.IsNullOrEmpty(destino.ReservaCarro.CodigoTarifa))
                {
                    orcamento.Carro.Add(destino.ReservaCarro);
                }

                foreach (var opcional in destino.Opcionais)
                {
                    orcamento.Atividade.Add(opcional); 
                }

                if (destino.Hotel != null && !String.IsNullOrEmpty(destino.Hotel.CodigoTarifa))
                {
                    orcamento.Hotel.Add(destino.Hotel);
                }
            }

            if (MPSession.Aereo.Count > 0 && MPSession.Aereo.First().CodigoTarifa > 0)
            {
                orcamento.Aereo.Add( MPSession.Aereo.First());
            }

            var success = AppCache.Put(EnumProcesso.Orcamento, SessionKey, orcamento, TimeSpan.FromMinutes(90));

            //Limpando as variáveis utilizadas pelo monte seu pacote
            MPSession = new MontePacoteModel();
            Session["IndiceAdicionarCarro"] = null;
            Session["IndiceAdicionarAtividade"] = null;
            Session["IndiceAdicionarHotel"] = null;
            Session["IndiceAdicionarAereo"] = null;

            return Json(SessionKey);
        }

        public enum EnumItemOrcamento
        {
            Hotel = 1,
            Aereo = 2,
            Pacote = 3,
            Atividade = 4,
            Carro = 5
        }
    }
}
