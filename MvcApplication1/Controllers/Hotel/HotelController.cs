using FT.Web.Model.Models;
using FTV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Site.Controllers.Base;
using FT.Web.Business.Hotel;
using FTV.Shared.Business;
using FT.Web.Core.Util.SiteTracer;
using System.Diagnostics;
using FT.Web.Core.Util.Common;
using FT.Web.Business.Pagamento;
using System.Text;
using PagedList;
using FT.Web.Business.HotelService;
using System.Configuration;

namespace FT.Web.Site.Controllers.Hotel
{
    public class HotelController : _BaseController
    {
        //
        // GET: /Hotel/
        //

        readonly HotelBusiness _hotelBussiness = new HotelBusiness();
        readonly PagamentoBusiness _pagamentoBussiness = new PagamentoBusiness();
        readonly Common _common = new Common();

        public virtual ActionResult Index()
        {
            var x = _hotelBussiness.ConsultarLocalizacaoHotel();
            return View();
        }

        public virtual ActionResult HotelForm(HotelModel hotel, string origem, string numerodeadultos)
        {
            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () => new FlyTourModel());

            if (hotel.Checkin == null && Session["dataCheckinGlobal"] != null)
                hotel.Checkin = Session["dataCheckinGlobal"].ToString();
            if (hotel.Checkout == null && Session["dataCheckOutGlobal"] != null)
                hotel.Checkout = Session["dataCheckOutGlobal"].ToString();
            if (hotel.CodigoCidade == null && Session["codigoCidadeHotalGlobal"] != null)
                hotel.CodigoCidade = Session["codigoCidadeHotalGlobal"].ToString();
            if (hotel.DestinoHotel == null && Session["DestinoHotelGlobal"] != null)
                hotel.DestinoHotel = Session["DestinoHotelGlobal"].ToString();

            if (BloquearApartamentizacao(addedFlytour))
            {
                hotel.Apartamentos = addedFlytour.Apartamentos;

                Session["UtilizaApartamentos"] = "S";
            }
            else
            {
                Session["UtilizaApartamentos"] = "N";
            }

            CarregarCombos();
            return PartialView("HotelForm", hotel);
        }

        public ActionResult BuscarHotel(string codigocidade, string checkinGlobal, string checkoutGlobal)
        {
            var Adultos = 0;
            var Total = 0;
            try
            {
                ViewBag.CodigoCidade = codigocidade;
                var hotel = new HotelModel();

                hotel.CodigoCidade = codigocidade;
                hotel.Checkin = checkinGlobal.Replace("-", "/");
                Session["dataCheckinGlobal"] = checkinGlobal.Replace("-", "/");
                hotel.Checkout = checkoutGlobal.Replace("-", "/");
                Session["dataCheckOutGlobal"] = checkoutGlobal.Replace("-", "/");
                Session["codigoCidadeHotalGlobal"] = codigocidade;

                var apartamentoList = new List<ApartamentoModel>();
                for (int i = 1; i <= 1; i++)
                {
                    var apartamentoIn = new ApartamentoModel();
                    apartamentoIn.Passageiros = new List<PassageiroModel>();

                    for (int k = 0; k < 2; k++)
                    {
                        var passageiroIn = new PassageiroModel();
                        passageiroIn.IdadePassageiro = 30;
                        apartamentoIn.Passageiros.Add(passageiroIn);
                        Total++;
                        Adultos++;
                    }
                    apartamentoList.Add(apartamentoIn);
                }

                hotel.Apartamentos = apartamentoList;
                var hotelsd = _hotelBussiness.ConsultarLocalizacaoHotel();
                if (hotelsd != null && hotelsd.Count > 0)
                {
                    var hotelComplete = hotelsd.FirstOrDefault(p => p.CodigoCidade.ToString() == codigocidade);

                    if (hotelComplete != null && hotelComplete.CidadeEstadoPais != null)
                    {
                        Session["DestinoHotelGlobal"] = hotelComplete.CidadeEstadoPais;
                    }
                }

                var hotelmodel = BuscarHoteisAssincrono(hotel);

                if (hotelmodel.Any())
                {
                    Decimal? menos = hotelmodel.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderBy(p => p.ValorTotal).First().ValorTotal;
                    Decimal? mais = hotelmodel.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderByDescending(p => p.ValorTotal).First().ValorTotal;
                    ViewBag.MenorPreco = Math.Floor(Conv.ToDecimal(menos));
                    ViewBag.MaiorPreco = Math.Floor(Conv.ToDecimal(mais));

                    ViewBag.AdultosNum = hotelmodel.FirstOrDefault().QuantAdultos != null && hotelmodel.FirstOrDefault().QuantAdultos > 0 ? hotelmodel.FirstOrDefault().QuantAdultos : Adultos;

                    hotelmodel.FirstOrDefault().QuantAdultos = hotelmodel.FirstOrDefault().QuantAdultos ?? Adultos;
                    ViewBag.Dados = hotelmodel.FirstOrDefault();


                    foreach (var item in hotelmodel)
                    {
                        item.Checkin = checkinGlobal.Replace("-", "/");
                        item.Checkout = checkoutGlobal.Replace("-", "/");
                    }

                    AppCache.Put(EnumProcesso.HotelVenda, String.Format("{0}{1}", SessionKey, "Hotel"), hotelmodel, TimeSpan.FromMinutes(90));


                    Session["TotalHoteis"] = hotelmodel.Count();

                    ViewBag.CodigoHotelCidade = codigocidade;
                    return View("Resultado", hotelmodel);
                }
                else { return RedirectToAction("Index", "Home"); }
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult BuscarHotelDetalhe(Int64 codigohotel, string checkin, string checkout, string codigocidade, int? quantcrianca, int? quantadulto)
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

            return PartialView("HotelDetalhes", HotelFlytour.Where(p => p.CodigoHotel == Conv.ToInt32(codigohotel)).FirstOrDefault());
        }

        public ActionResult Resultado(HotelModel hotelResultado, string origem, string hdnDestinoHotelCodigo, string HDCidadeHotel)
        {
            if (String.IsNullOrEmpty(hdnDestinoHotelCodigo))
            { hdnDestinoHotelCodigo = HDCidadeHotel; }

            if (!String.IsNullOrEmpty(hdnDestinoHotelCodigo))
            {
                ViewBag.CodigoCidade = hdnDestinoHotelCodigo;
            }
            else
            {
                ViewBag.CodigoCidade = hotelResultado.CodigoCidade;
            }

            int Adultos = 0;
            int Criancas = 0;
            int Bebes = 0;
            int Total = 0;
            var ApartamentoList = new List<ApartamentoModel>();
            for (int i = 1; i <= Conv.ToInt32(Request.Form["qtdQuartos"]); i++)
            {
                var ApartamentoIN = new ApartamentoModel();
                ApartamentoIN.Passageiros = new List<PassageiroModel>();

                if (Conv.ToInt32(Request.Form["ddlQ" + i + "Adulto"]) > 0) //  
                {
                    for (int k = 0; k < Conv.ToInt32(Request.Form["ddlQ" + i + "Adulto"]); k++)
                    {
                        // PARA ENVIO DA RESERVA E USO DO FILTRO
                        var PassageiroIN = new PassageiroModel();
                        PassageiroIN.IdadePassageiro = 30;
                        ApartamentoIN.Passageiros.Add(PassageiroIN);
                        Total++;
                        Adultos++;
                    }
                }

                if (Conv.ToInt32(Request.Form["ddlQ" + i + "Crianca"]) > 0) //  
                {
                    for (int j = 0; j < Conv.ToInt32(Request.Form["ddlQ" + i + "Crianca"]); j++)
                    {
                        // PARA ENVIO DA RESERVA E USO DO FILTRO
                        var PassageiroIN = new PassageiroModel();
                        PassageiroIN.IdadePassageiro = Conv.ToDecimal(Request.Form["ddlQ" + i + "Crianca" + j]);
                        if (PassageiroIN.IdadePassageiro < 2)
                            Bebes++;
                        else
                            Criancas++;

                        Total++;
                        ApartamentoIN.Passageiros.Add(PassageiroIN);
                    }
                }
                ApartamentoList.Add(ApartamentoIN);
            }

            hotelResultado.Apartamentos = ApartamentoList;
            Session["Apartamentos"] = ApartamentoList;
            hotelResultado.CodigoCidade = ViewBag.CodigoCidade;
            HotelModel hotel = hotelResultado;
            hotel.QuantAdultos = Adultos;
            hotel.QuantCriancas = Criancas + Bebes;

            var hotelmodel = BuscarHoteisAssincrono(hotel);

            Session["HoteilSessionPaginacaoFiltrado"] = null;

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", new List<HotelModel>(), TimeSpan.FromMinutes(20));

            if (!String.IsNullOrEmpty(hotelResultado.NomeHotel))
            {
                if (hotelResultado.NomeHotel != "Informe o nome do hotel")
                {
                    hotelmodel = _hotelBussiness.FiltrarPorNome(hotelmodel, hotelResultado.NomeHotel);
                }
            }

            if (hotelResultado.Estrelas != 1)
            {
                List<int> estrelasFiltro = new List<int>();
                if (hotelResultado.Estrelas == 3)
                {
                    estrelasFiltro.Add(4);
                    estrelasFiltro.Add(5);
                }
                if (hotelResultado.Estrelas == 4)
                {
                    estrelasFiltro.Add(5);
                    estrelasFiltro.Add(6);
                }

                hotelmodel = _hotelBussiness.FiltrarPorEstrelas(hotelmodel, estrelasFiltro);
            }

            Decimal? mais = hotelmodel.Max(p => p.ValorFinalMenorPreco);
            Decimal? menos = hotelmodel.Min(p => p.ValorFinalMenorPreco);

            ViewBag.Dados = hotelResultado;

            ViewBag.AdultosNum = hotelResultado.QuantAdultos;
            ViewBag.TextJson = hotelmodel;

            hotelmodel = hotelmodel.OrderBy(p => p.ValorFinalMenorPreco).ToList();

            string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

            if (isAssincrono.ToUpper() == "S")
            {
                ViewBag.ContadorAssincrono = "setTimeout(VerificarNovosHoteis, totalMilisegundosBusca);";
            }
            else
            {
                ViewBag.ContadorAssincrono = "";
            }

            ViewBag.DestinoHotel = _hotelBussiness.ConsultarLocalizacaoHotel(hdnDestinoHotelCodigo);

            return View("Resultado", hotelmodel);

        }

        public ActionResult ResultadoMontePacote(string hdnDestinoHotelCodigo, DateTime? dataSaida, DateTime? dataChegada)
        {
            HotelModel hotelResultado = MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])].Hotel;

            hotelResultado.Apartamentos = MPSession.Apartamentos;
            hotelResultado.CodigoCidade = hdnDestinoHotelCodigo;
            hotelResultado.QuantAdultos = MPSession.QuantAdultos;
            hotelResultado.QuantCriancas = MPSession.QuantCriancas;
            hotelResultado.QuantQuartos = hotelResultado.Apartamentos.Count;
            hotelResultado.Checkin = (dataSaida.HasValue) ? dataSaida.Value.ToString("dd/MM/yyyy") : MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])].Saida;
            hotelResultado.Checkout = (dataChegada.HasValue) ? dataChegada.Value.ToString("dd/MM/yyyy") : MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])].Retorno;
            hotelResultado.Estrelas = 1;

            var hotelmodel = BuscarHoteisAssincrono(hotelResultado);

            Session["HoteilSessionPaginacaoFiltradao"] = null;

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", new List<HotelModel>(), TimeSpan.FromMinutes(20));

            if (hotelResultado.Estrelas != 1)
            {
                List<int> estrelasFiltro = new List<int>();
                if (hotelResultado.Estrelas == 3)
                {
                    estrelasFiltro.Add(4);
                    estrelasFiltro.Add(5);
                }
                if (hotelResultado.Estrelas == 4)
                {
                    estrelasFiltro.Add(5);
                    estrelasFiltro.Add(6);
                }

                hotelmodel = _hotelBussiness.FiltrarPorEstrelas(hotelmodel, estrelasFiltro);
            }

            ViewBag.Dados = hotelResultado;

            if (!String.IsNullOrEmpty(hdnDestinoHotelCodigo))
            {
                ViewBag.CodigoCidade = hdnDestinoHotelCodigo;
            }
            else
            {
                ViewBag.CodigoCidade = hotelResultado.CodigoCidade;
            }

            ViewBag.AdultosNum = hotelResultado.QuantAdultos;

            ViewBag.TextJson = hotelmodel;

            string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

            if (isAssincrono.ToUpper() == "S")
            {
                ViewBag.ContadorAssincrono = "setTimeout(VerificarNovosHoteis, totalMilisegundosBusca);";
            }
            else
            {
                ViewBag.ContadorAssincrono = "";
            }

            return RedirectToAction("ResultadoGridMontePacote", "Hotel", hotelmodel);
        }

        public List<HotelModel> BuscarHoteis(HotelModel hotelBusca)
        {
            if (SessionUsuario != null && SessionUsuario.ID_PESSOA_AGENCIA.HasValue)
                hotelBusca.CodigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();

            List<HotelModel> hoteis = _hotelBussiness.ResultHoteis(hotelBusca, SessionUsuario);

            var i = 1;

            List<HoteisMapaModel> HoteisMapa = new List<HoteisMapaModel>();

            foreach (var item in hoteis)
            {
                if (item != null && !String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                {
                    var hotelmapa = new HoteisMapaModel();

                    hotelmapa.Latidude = item.Latitude.Replace(",", ".");
                    hotelmapa.Longitude = item.Longitude.Replace(",", ".");
                    hotelmapa.Order = i;
                    hotelmapa.Titulo = item.NomeHotel;
                    hotelmapa.ValorDiaria = item.ValorFinalMenorPreco;
                    hotelmapa.Endereco = String.Format("{0} - {1}{2} - {3}", item.Endereco, item.CodigoCidade, item.NomeEstado, item.Cep);
                    hotelmapa.NumeroEstrelas = item.NumeroEstrelas;
                    hotelmapa.CodigoHotel = item.CodigoHotel;
                    hotelmapa.Checkin = item.Checkin;
                    hotelmapa.Checkout = item.Checkout;
                    hotelmapa.QuantAdultos = hotelBusca.QuantAdultos;
                    hotelmapa.QuantCriancas = hotelBusca.QuantCriancas;
                    hotelmapa.CodigoCidade = hotelBusca.CodigoCidade;
                    if (item.HotelFotos.Count() > 0)
                    { hotelmapa.Foto = item.HotelFotos.FirstOrDefault().HotelFotoUrl; }
                    HoteisMapa.Add(hotelmapa);
                    i++;
                }
            }

            ViewBag.HotelMapa = HoteisMapa;

            hoteis = hoteis.Where(x => x != null).ToList();
            return hoteis.OrderBy(x => x.ValorFinalMenorPreco).ToList();
        }

        public List<HotelModel> BuscarHoteisAssincrono(HotelModel hotelBusca)
        {
            if (SessionUsuario != null && SessionUsuario.ID_PESSOA_AGENCIA.HasValue)
            {
                hotelBusca.CodigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();
            }

            var i = 1;

            List<HoteisMapaModel> HoteisMapa = new List<HoteisMapaModel>();

            Session["HotelBusca"] = hotelBusca;

            string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

            bool bIsAssincrono = isAssincrono == "S" ? true : false;

            List<HotelModel> hoteis = _hotelBussiness.ConsultarHoteisAssincrono(hotelBusca, SessionUsuario, SessionKey, isConsultaAssincrona: bIsAssincrono, login: LoginService());

            foreach (var item in hoteis)
            {
                if (item != null && !String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                {
                    var hotelmapa = new HoteisMapaModel();

                    hotelmapa.Latidude = item.Latitude.Replace(",", ".");
                    hotelmapa.Longitude = item.Longitude.Replace(",", ".");
                    hotelmapa.Order = i;
                    hotelmapa.Titulo = item.NomeHotel;
                    hotelmapa.ValorDiaria = item.ValorFinalMenorPreco;
                    hotelmapa.Endereco = String.Format("{0} - {1}{2} - {3}", item.Endereco, item.CodigoCidade, item.NomeEstado, item.Cep);
                    hotelmapa.NumeroEstrelas = item.NumeroEstrelas;
                    hotelmapa.CodigoHotel = item.CodigoHotel;
                    hotelmapa.Checkin = item.Checkin;
                    hotelmapa.Checkout = item.Checkout;
                    hotelmapa.QuantAdultos = hotelBusca.QuantAdultos;
                    hotelmapa.QuantCriancas = hotelBusca.QuantCriancas;
                    hotelmapa.CodigoCidade = hotelBusca.CodigoCidade;
                    if (item.HotelFotos.Count() > 0)
                    { hotelmapa.Foto = item.HotelFotos.FirstOrDefault().HotelFotoUrl; }
                    HoteisMapa.Add(hotelmapa);
                    i++;
                }
            }

            ViewBag.HotelMapa = HoteisMapa;

            hoteis = hoteis.Where(x => x != null).ToList();

            hoteis = hoteis.OrderBy(x => x.ValorFinalMenorPreco).ToList();

            if (hoteis.Count > 0)
            {
                Decimal? menos = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderBy(p => p.ValorTotal).First().ValorTotal;
                Decimal? mais = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderByDescending(p => p.ValorDiariaMedia).First().ValorTotal;
                ViewBag.MenorPreco = Math.Floor(Conv.ToDecimal(menos));
                ViewBag.MaiorPreco = Math.Floor(Conv.ToDecimal(mais));

                Session["MenorPreco"] = Math.Floor(Conv.ToDecimal(menos));
                Session["MaiorPreco"] = Math.Floor(Conv.ToDecimal(mais));
            }

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey, hoteis, TimeSpan.FromMinutes(20));

            Session["TotalHoteis"] = hoteis.Count();
            Session["CodigoConsulta"] = hoteis.Count() == 0 ? "0" : hoteis.First().CodigoConsulta;

            Session["HoteilSession"] = hoteis;

            var paginado = hoteis.ToPagedList(1, 10);

            List<HotelModel> lstHoteis = new List<HotelModel>();

            foreach (var item in paginado)
            {
                lstHoteis.Add(item);
            }

            return lstHoteis;
        }

        /// <summary>
        /// Método utilizado para buscar os novos registros no serviço
        /// </summary>
        /// <param name="isMontePacote"></param>
        /// <returns></returns>
        public string AtualizarNovosRegistros(bool isMontePacote = false)
        {
            string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

            bool bIsAssincrono = isAssincrono == "S" ? true : false;

            List<HotelModel> hoteis = _hotelBussiness.ConsultarHoteisAssincrono(Session["HotelBusca"] as HotelModel, SessionUsuario, SessionKey, Session["CodigoConsulta"].ToString(), isConsultaAssincrona: bIsAssincrono, login: LoginService(), isSession: true);

            if (Session["HoteilSession"] == null)
            {
                Session["HoteilSession"] = hoteis;
            }
            else
            {
                List<HotelModel> _hoteis = Session["HoteilSession"] as List<HotelModel>;

                _hoteis.AddRange(hoteis);

                Session["HoteilSession"] = _hoteis;

                hoteis = _hoteis;
            }

            Session["TotalHoteis"] = hoteis.Count();

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;
            }

            return Session["TotalHoteis"].ToString();
        }

        /// <summary>
        /// Verifica a quantidade de registros já processados
        /// </summary>
        /// <param name="isMontePacote"></param>
        /// <returns></returns>
        public string QuantidadeNovosRegistros(bool isMontePacote = false)
        {
            int totalHoteisProcessados = _hotelBussiness.RetornarTotalHoteisProcessados(Session["HotelBusca"] as HotelModel, SessionUsuario, SessionKey, Session["CodigoConsulta"].ToString());

            return totalHoteisProcessados.ToString();
        }

        /// <summary>
        /// Alterar a página na consulta de hotéis
        /// </summary>
        /// <param name="totalItensPagina"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        public ActionResult MudarPagina(int totalItensPagina, int pagina, int ordenacao, bool isMontePacote = false)
        {
            List<HotelModel> hoteis = null;

            if (Session["HoteilSessionPaginacaoFiltrado"] != null)
            {
                hoteis = Session["HoteilSessionPaginacaoFiltrado"] as List<HotelModel>;
            }

            if (hoteis == null)
            {
                if (Session["HoteilSession"] != null)
                {
                    hoteis = Session["HoteilSession"] as List<HotelModel>;
                }
            }

            this.CarregarHoteisMapa(hoteis);

            if (ordenacao == 3)
            {
                hoteis = hoteis.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteis = hoteis.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            if (hoteis.Count > 0)
            {
                Decimal? menos = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderBy(p => p.ValorTotal).First().ValorTotal;
                Decimal? mais = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderByDescending(p => p.ValorDiariaMedia).First().ValorTotal;
                ViewBag.MenorPreco = Math.Floor(Conv.ToDecimal(menos));
                ViewBag.MaiorPreco = Math.Floor(Conv.ToDecimal(mais));
            }

            var paginado = hoteis.ToPagedList(pagina, totalItensPagina);

            List<HotelModel> lstHoteis = new List<HotelModel>();

            foreach (var item in paginado)
            {
                lstHoteis.Add(item);
            }

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;
                ViewBag.PaginaSelecionada = pagina;
                ViewBag.TotalItensPagina = totalItensPagina;
                ViewBag.Ordenacao = ordenacao;
                ViewBag.PaginaAtual = pagina;
                ViewBag.Carregado = true;
                Session["TotalHoteis"] = hoteis.Count();

                string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

                if (isAssincrono.ToUpper() == "S")
                {
                    ViewBag.ContadorAssincrono = "setTimeout(VerificarNovosHoteis, totalMilisegundosBusca);";
                }
                else
                {
                    ViewBag.ContadorAssincrono = "";
                }
            }

            return PartialView("ResultadoGrid", lstHoteis);
        }

        public string CarregarHoteisMapa(List<HotelModel> hoteis = null)
        {

            List<HoteisMapaModel> HoteisMapa = new List<HoteisMapaModel>();
            HotelModel hotelBusca = (HotelModel)Session["HotelBusca"];
            int i = 1;

            if (hoteis == null)
            {
                hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
                {
                    return new List<HotelModel>();
                });
            }

            foreach (var item in hoteis)
            {
                if (item != null && !String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                {
                    var hotelmapa = new HoteisMapaModel();

                    hotelmapa.Latidude = item.Latitude.Replace(",", ".");
                    hotelmapa.Longitude = item.Longitude.Replace(",", ".");
                    hotelmapa.Order = i;
                    hotelmapa.Titulo = item.NomeHotel;
                    hotelmapa.ValorDiaria = item.ValorFinalMenorPreco;
                    hotelmapa.Endereco = String.Format("{0} - {1}{2} - {3}", item.Endereco, item.CodigoCidade, item.NomeEstado, item.Cep);
                    hotelmapa.NumeroEstrelas = item.NumeroEstrelas;
                    hotelmapa.CodigoHotel = item.CodigoHotel;
                    hotelmapa.Checkin = item.Checkin;
                    hotelmapa.Checkout = item.Checkout;
                    hotelmapa.QuantAdultos = hotelBusca.QuantAdultos;
                    hotelmapa.QuantCriancas = hotelBusca.QuantCriancas;
                    hotelmapa.CodigoCidade = hotelBusca.CodigoCidade;
                    if (item.HotelFotos.Count() > 0)
                    { hotelmapa.Foto = item.HotelFotos.FirstOrDefault().HotelFotoUrl; }
                    HoteisMapa.Add(hotelmapa);
                    i++;
                }
            }

            ViewBag.HotelMapa = HoteisMapa;

            return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(HoteisMapa);
        }

        /// <summary>
        /// Método utilizado no botão para carregar novos registros na tela
        /// inseridos pela consulta assincrona
        /// </summary>
        /// <param name="totalItensPagina"></param>
        /// <returns></returns>
        public ActionResult AtualizarNovosHoteis(int totalItensPagina = 10, int ordenacao = 3, bool isMontePacote = false)
        {
            List<HotelModel> hoteis = _hotelBussiness.RetornarHoteis(Session["HotelBusca"] as HotelModel, SessionUsuario, SessionKey, Session["CodigoConsulta"].ToString(), LoginService(), isSession: true);

            if (Session["HoteilSession"] == null)
            {
                Session["HoteilSession"] = hoteis;

                var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey, hoteis, TimeSpan.FromMinutes(20));
            }
            else
            {
                List<HotelModel> _hoteis = Session["HoteilSession"] as List<HotelModel>;

                _hoteis.AddRange(hoteis);

                Session["HoteilSession"] = _hoteis;

                hoteis = _hoteis;

                var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey, hoteis, TimeSpan.FromMinutes(20));
            }

            Session["TotalHoteis"] = hoteis.Count();

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;
                ViewBag.Carregado = true;
            }

            this.CarregarHoteisMapa(hoteis);

            //valor 3 = menor -> maior
            if (ordenacao == 3)
            {
                hoteis = hoteis.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else //maior -> menor
            {
                hoteis = hoteis.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            if (hoteis.Count > 0)
            {
                Decimal? menos = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderBy(p => p.ValorTotal).First().ValorTotal;
                Decimal? mais = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderByDescending(p => p.ValorDiariaMedia).First().ValorTotal;
                ViewBag.MenorPreco = Math.Floor(Conv.ToDecimal(menos));
                ViewBag.MaiorPreco = Math.Floor(Conv.ToDecimal(mais));
            }

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;
                ViewBag.TotalItensPagina = totalItensPagina;
                ViewBag.Ordenacao = ordenacao;

                string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

                if (isAssincrono.ToUpper() == "S")
                {
                    ViewBag.ContadorAssincrono = "setTimeout(VerificarNovosHoteis, totalMilisegundosBusca);";
                }
                else
                {
                    ViewBag.ContadorAssincrono = "";
                }

                var l = 1;

                List<HoteisMapaModel> HoteisMapa = new List<HoteisMapaModel>();

                foreach (var item in hoteis)
                {
                    item.NomeCidade = MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])].NomeCidadeDestino;

                    if (!String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                    {
                        var hotelmapa = new HoteisMapaModel();

                        hotelmapa.Latidude = item.Latitude.Replace(",", ".");
                        hotelmapa.Longitude = item.Longitude.Replace(",", ".");
                        hotelmapa.Order = l;
                        hotelmapa.Titulo = item.NomeHotel;
                        hotelmapa.ValorDiaria = item.ValorFinalMenorPreco;
                        hotelmapa.Endereco = String.Format("{0} - {1}{2} - {3}", item.Endereco, item.CodigoCidade, item.NomeEstado, item.Cep);
                        hotelmapa.NumeroEstrelas = item.NumeroEstrelas;
                        hotelmapa.CodigoHotel = item.CodigoHotel;
                        hotelmapa.Checkin = item.Checkin;
                        hotelmapa.Checkout = item.Checkout;
                        hotelmapa.QuantAdultos = item.QuantAdultos;
                        hotelmapa.QuantCriancas = item.QuantCriancas;
                        hotelmapa.CodigoCidade = item.CodigoCidade;

                        if (item.HotelFotos.Count() > 0)
                        {
                            hotelmapa.Foto = item.HotelFotos.FirstOrDefault().HotelFotoUrl;
                        }

                        HoteisMapa.Add(hotelmapa);
                        l++;
                    }
                }

                ViewBag.HotelMapa = HoteisMapa;
            }

            var paginado = hoteis.ToPagedList(1, totalItensPagina);

            List<HotelModel> lstHoteis = new List<HotelModel>();

            foreach (var item in paginado)
            {
                lstHoteis.Add(item);
            }



            Session["TotalHoteis"] = hoteis.Count();

            return PartialView("ResultadoGrid", lstHoteis);

        }

        /// <summary>
        /// Método que retorna qual o valor mínimo que o slider utilizará
        /// </summary>
        /// <returns></returns>
        public decimal RetornarValorMinimo()
        {
            List<HotelModel> hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
            {
                return new List<HotelModel>();
            });

            decimal valorMinimoAtual = 0;

            if (hoteis.Count > 0)
            {
                Decimal? menos = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderBy(p => p.ValorTotal).First().ValorTotal;
                ViewBag.MenorPreco = Math.Floor(Conv.ToDecimal(menos));

                valorMinimoAtual = Math.Floor(Conv.ToDecimal(menos));
            }

            return valorMinimoAtual;
        }

        /// <summary>
        /// Método que retorna qual o valor máximo que o slider utilizará
        /// </summary>
        /// <returns></returns>
        public decimal RetornarValorMaximo()
        {
            List<HotelModel> hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
            {
                return new List<HotelModel>();
            });

            decimal valorMaximoAtual = 0;

            if (hoteis.Count > 0)
            {
                Decimal? mais = hoteis.SelectMany(p => p.Acomodacoes).SelectMany(p => p.Tarifas).OrderByDescending(p => p.ValorDiariaMedia).First().ValorTotal;

                ViewBag.MaiorPreco = Math.Floor(Conv.ToDecimal(mais));

                valorMaximoAtual = Math.Floor(Conv.ToDecimal(mais));
            }

            return valorMaximoAtual;
        }

        public ActionResult FiltroHotel(string nome, string menorPreco, string maiorPreco, string estrela, string lstFacilidades, string hotelBuscacheckin, string hotelBuscaCheckout, string order, string quantAdultos, string quantCriancas, string codigocidade)
        {
            try
            {
                HotelModel hotelBusca = new HotelModel
                {
                    QuantAdultos = Conv.ToInt32(quantAdultos),
                    QuantCriancas = Conv.ToInt32(quantCriancas),
                    Checkin = hotelBuscacheckin,
                    Checkout = hotelBuscaCheckout,
                    CodigoCidade = codigocidade
                };

                List<int> estrelasFiltro = new List<int>();
                List<string> facilidadesFiltro = new List<string>();

                if (!String.IsNullOrEmpty(estrela))
                {
                    string[] estrelas = estrela.Remove(estrela.Length - 1, 1).ToString().Split(',');
                    for (int i = 0; i < estrelas.Length; i++)
                    {
                        estrelasFiltro.Add(Conv.ToInt32(estrelas[i]));
                    }
                }
                else { estrelasFiltro = null; }

                if (!String.IsNullOrEmpty(lstFacilidades))
                {
                    string[] facilidades = lstFacilidades.Remove(lstFacilidades.Length - 1, 1).ToString().Split(',');
                    for (int i = 0; i < facilidades.Length; i++)
                    {
                        facilidadesFiltro.Add(facilidades[i]);
                    }
                }
                else { facilidadesFiltro = null; }

                List<HotelModel> HotelSession = new List<HotelModel>();

                if (SessionUsuario != null && SessionUsuario.ID_PESSOA_AGENCIA.HasValue)
                    hotelBusca.CodigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();

                if (Session["HoteilSession"] != null)
                    HotelSession = (List<HotelModel>)Session["HoteilSession"];
                else
                    HotelSession = _hotelBussiness.ResultHoteis(hotelBusca, SessionUsuario);

                if (Session["MPFiltro"] != null)
                {
                    if ((bool)Session["MPFiltro"] == true)
                    {
                        ViewBag.IsMontePacote = true;
                        Session["MPFiltro"] = false;
                    }
                }

                var hoteisEstrelas = _hotelBussiness.FiltroHotel(HotelSession, hotelBusca, Conv.ToDecimal(menorPreco), Conv.ToDecimal(maiorPreco), nome, estrelasFiltro, facilidadesFiltro, Conv.ToInt32(order));

                List<HoteisMapaModel> HoteisMapa = new List<HoteisMapaModel>();
                int l = 0;

                foreach (var item in hoteisEstrelas)
                {
                    if (!String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                    {
                        var hotelmapa = new HoteisMapaModel();

                        hotelmapa.Latidude = item.Latitude.Replace(",", ".");
                        hotelmapa.Longitude = item.Longitude.Replace(",", ".");
                        hotelmapa.Order = l;
                        hotelmapa.Titulo = item.NomeHotel;
                        hotelmapa.ValorDiaria = item.ValorFinalMenorPreco;
                        hotelmapa.Endereco = String.Format("{0} - {1}{2} - {3}", item.Endereco, item.CodigoCidade, item.NomeEstado, item.Cep);
                        hotelmapa.NumeroEstrelas = item.NumeroEstrelas;
                        hotelmapa.CodigoHotel = item.CodigoHotel;
                        hotelmapa.Checkin = item.Checkin;
                        hotelmapa.Checkout = item.Checkout;
                        hotelmapa.QuantAdultos = item.QuantAdultos;
                        hotelmapa.QuantCriancas = item.QuantCriancas;
                        hotelmapa.CodigoCidade = item.CodigoCidade;
                        if (item.HotelFotos.Count() > 0)
                        { hotelmapa.Foto = item.HotelFotos.FirstOrDefault().HotelFotoUrl; }
                        HoteisMapa.Add(hotelmapa);
                        l++;
                    }
                }

                ViewBag.HotelMapa = HoteisMapa;

                hoteisEstrelas = hoteisEstrelas.OrderBy(p => p.ValorFinalMenorPreco).ToList();

                return PartialView("ResultadoGrid", hoteisEstrelas);
            }
            catch (Exception)
            {
                return PartialView("ResultadoGrid");
            }
        }

        [HttpPost]
        public ActionResult FiltroHotelAtualizado(string nome, string menorPreco, string maiorPreco, string estrela, string lstFacilidades, int qtdeItensPorPagina = 10, int ordenacao = 3)
        {
            var hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
            {
                return new List<HotelModel>();
            });

            List<int> estrelasFiltro = new List<int>();
            List<string> facilidadesFiltro = new List<string>();

            if (!String.IsNullOrEmpty(estrela))
            {
                string[] estrelas = estrela.Remove(estrela.Length - 1, 1).ToString().Split(',');

                for (int i = 0; i < estrelas.Length; i++)
                {
                    estrelasFiltro.Add(Conv.ToInt32(estrelas[i]));
                }
            }
            else
            {
                estrelasFiltro = null;
            }

            if (!String.IsNullOrEmpty(lstFacilidades))
            {
                string[] facilidades = lstFacilidades.Remove(lstFacilidades.Length - 1, 1).ToString().Split(',');

                for (int i = 0; i < facilidades.Length; i++)
                {
                    facilidadesFiltro.Add(facilidades[i]);
                }
            }
            else
            {
                facilidadesFiltro = null;
            }

            if (Session["MPFiltro"] != null)
            {
                if ((bool)Session["MPFiltro"] == true)
                {
                    ViewBag.IsMontePacote = true;
                    Session["MPFiltro"] = false;
                }
            }

            var hoteisEstrelas = _hotelBussiness.FiltroHotelAtualizado(hoteis, Conv.ToDecimal(menorPreco), Conv.ToDecimal(maiorPreco), nome, estrelasFiltro, facilidadesFiltro);

            if (ordenacao == 3)
            {
                hoteisEstrelas = hoteisEstrelas.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteisEstrelas = hoteisEstrelas.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            //Carrega os hotéis que foram filtrados de acordo com os parâmetros enviados pelo usuário
            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", hoteisEstrelas, TimeSpan.FromMinutes(20));

            var paginado = hoteisEstrelas.ToPagedList(1, qtdeItensPorPagina).ToList();

            return PartialView("ResultadoGrid", paginado);
        }

        [HttpPost]
        public ActionResult FiltroHotelPorNome(string nome, int qtdeItensPorPagina = 10, int ordenacao = 3, bool isMontePacote = false)
        {
            List<HotelModel> hoteis = new List<HotelModel>();

            if (Session["HoteilSessionPaginacaoFiltrado"] != null)
            {
                hoteis = Session["HoteilSessionPaginacaoFiltrado"] as List<HotelModel>;
            }
            else
            {
                hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", 20, () =>
                {
                    return null;
                });
            }

            if (String.IsNullOrEmpty(nome) || hoteis == null || hoteis.Count == 0)
            {
                if (Session["HoteilSession"] != null)
                {
                    hoteis = Session["HoteilSession"] as List<HotelModel>;
                }
                else
                {
                    hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
                    {
                        return new List<HotelModel>();
                    });
                }
            }

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;

                if (Session["MPFiltro"] != null)
                {
                    if ((bool)Session["MPFiltro"] == true)
                    {
                        ViewBag.IsMontePacote = true;
                        Session["MPFiltro"] = false;
                    }
                }
            }

            var hoteisFiltrados = _hotelBussiness.FiltroHotelPorNome(hoteis, nome);

            SessionFiltroHotelPorNome = nome;

            hoteisFiltrados = AplicarFiltrosAnteriores(hoteisFiltrados, EnumFiltros.PorNome);

            if (ordenacao == 3)
            {
                hoteisFiltrados = hoteisFiltrados.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteisFiltrados = hoteisFiltrados.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            Session["TotalHoteisFiltrados"] = hoteisFiltrados;

            Session["HoteilSessionPaginacaoFiltrado"] = hoteisFiltrados;

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", hoteisFiltrados, TimeSpan.FromMinutes(20));

            hoteisFiltrados = hoteisFiltrados.ToPagedList(1, qtdeItensPorPagina).ToList();

            ViewBag.Carregado = true;

            if (hoteisFiltrados.Count > 0)
            {
                return PartialView("ResultadoGrid", hoteisFiltrados);
            }
            else
            {
                return PartialView("NoDataFound");
            }
        }

        [HttpPost]
        public ActionResult FiltroHotelPorFacilidades(string lstFacilidades, int qtdeItensPorPagina = 10, int ordenacao = 3, bool isMontePacote = false)
        {
            List<HotelModel> hoteis = new List<HotelModel>();

            if (Session["HoteilSessionPaginacaoFiltrado"] != null)
            {
                hoteis = Session["HoteilSessionPaginacaoFiltrado"] as List<HotelModel>;
            }
            else
            {
                hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", 20, () =>
                {
                    return null;
                });
            }

            if (hoteis == null || hoteis.Count == 0)
            {
                if (Session["HoteilSession"] != null)
                {
                    hoteis = Session["HoteilSession"] as List<HotelModel>;
                }
                else
                {
                    hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
                    {
                        return new List<HotelModel>();
                    });
                }
            }

            List<string> facilidadesFiltro = new List<string>();

            if (!String.IsNullOrEmpty(lstFacilidades))
            {
                string[] facilidades = lstFacilidades.Remove(lstFacilidades.Length - 1, 1).ToString().Split(',');

                for (int i = 0; i < facilidades.Length; i++)
                {
                    facilidadesFiltro.Add(facilidades[i]);
                }
            }
            else
            {
                facilidadesFiltro = null;
            }

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;

                if (Session["MPFiltro"] != null)
                {
                    if ((bool)Session["MPFiltro"] == true)
                    {
                        ViewBag.IsMontePacote = true;
                        Session["MPFiltro"] = false;
                    }
                } 
            }

            var hoteisFiltrados = _hotelBussiness.FiltroHotelPorFacilidades(hoteis, facilidadesFiltro);

            SessionFiltroFacilidadesHotel = facilidadesFiltro;

            hoteisFiltrados = AplicarFiltrosAnteriores(hoteisFiltrados, EnumFiltros.PorFacilidades);

            if (ordenacao == 3)
            {
                hoteisFiltrados = hoteisFiltrados.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteisFiltrados = hoteisFiltrados.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            Session["HoteilSessionPaginacaoFiltrado"] = hoteisFiltrados;

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", hoteisFiltrados, TimeSpan.FromMinutes(20));

            hoteisFiltrados = hoteisFiltrados.ToPagedList(1, qtdeItensPorPagina).ToList();

            ViewBag.Carregado = true;

            return PartialView("ResultadoGrid", hoteisFiltrados);
        }

        [HttpPost]
        public ActionResult FiltroHotelPorEstrelas(string estrela, int qtdeItensPorPagina = 10, int ordenacao = 3, bool isMontePacote = false)
        {
            List<HotelModel> hoteis = new List<HotelModel>();

            if (Session["HoteilSession"] != null)
            {
                hoteis = Session["HoteilSession"] as List<HotelModel>;
            }
            else
            {
                hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
                {
                    return new List<HotelModel>();
                });
            }
            
            List<int> estrelasFiltro = new List<int>();

            if (!String.IsNullOrEmpty(estrela))
            {
                string[] estrelas = estrela.Remove(estrela.Length - 1, 1).ToString().Split(',');

                for (int i = 0; i < estrelas.Length; i++)
                {
                    estrelasFiltro.Add(Conv.ToInt32(estrelas[i]));
                }
            }

            ///Verifica se não tiveram hotéis filtrados ou se foram reiniciadas as estrelas
            if (hoteis == null || hoteis.Count == 0 || (estrelasFiltro != null && estrelasFiltro.Count == 3))
            {
                if (Session["HoteilSession"] != null)
                {
                    hoteis = Session["HoteilSession"] as List<HotelModel>;
                }
                else
                {
                    hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
                    {
                        return new List<HotelModel>();
                    });
                }
            }

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;

                if (Session["MPFiltro"] != null)
                {
                    if ((bool)Session["MPFiltro"] == true)
                    {
                        ViewBag.IsMontePacote = true;
                        Session["MPFiltro"] = false;
                    }
                } 
            }

            List<HotelModel> hoteisFiltrados = new List<HotelModel>();

            ///Se não tiver todas as estrelas checadas aplicará o filtro por estrelas
            if (estrelasFiltro != null && estrelasFiltro.Count() != 3)
            {
                hoteisFiltrados = _hotelBussiness.FiltroHotelPorEstrelas(hoteis, estrelasFiltro); 
            }
            else
            {
                hoteisFiltrados = hoteis;
            }

            SessionFiltroHotelPorEstrelas = estrelasFiltro;

            hoteisFiltrados = AplicarFiltrosAnteriores(hoteisFiltrados, EnumFiltros.PorEstrelas);

            if (ordenacao == 3)
            {
                hoteisFiltrados = hoteisFiltrados.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteisFiltrados = hoteisFiltrados.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            Session["HoteilSessionPaginacaoFiltrado"] = hoteisFiltrados;

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", hoteisFiltrados, TimeSpan.FromMinutes(20));

            hoteisFiltrados = hoteisFiltrados.ToPagedList(1, qtdeItensPorPagina).ToList();

            ViewBag.Carregado = true;

            return PartialView("ResultadoGrid", hoteisFiltrados);
        }

        /// <summary>
        /// Método responsável por verificar quais os filtros que foram aplicados anteriormente e atualizar os registros
        /// </summary>
        /// <param name="hoteisFiltrados"></param>
        /// <param name="filtroOrigem"></param>
        /// <returns></returns>
        private List<HotelModel> AplicarFiltrosAnteriores(List<HotelModel> hoteisFiltrados, EnumFiltros filtroOrigem)
        {

            switch (filtroOrigem)
            {
                case EnumFiltros.PorEstrelas:

                    if (SessionFiltroFacilidadesHotel.Count() > 0)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorFacilidades(hoteisFiltrados, SessionFiltroFacilidadesHotel);
                    }

                    if (SessionFiltroHotelPorNome != "")
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorNome(hoteisFiltrados, SessionFiltroHotelPorNome);
                    }

                    if (SessionFiltroMaiorValorHotel != null && SessionFiltroMenorValorHotel != null)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorValores(hoteisFiltrados, SessionFiltroMenorValorHotel.Value, SessionFiltroMaiorValorHotel.Value);
                    }

                    break;

                case EnumFiltros.PorNome:

                    if (SessionFiltroFacilidadesHotel.Count() > 0)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorFacilidades(hoteisFiltrados, SessionFiltroFacilidadesHotel);
                    }

                    if (SessionFiltroHotelPorEstrelas.Count > 0 && SessionFiltroHotelPorEstrelas.Count < 3)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorEstrelas(hoteisFiltrados, SessionFiltroHotelPorEstrelas);
                    }

                    if (SessionFiltroMaiorValorHotel != null && SessionFiltroMenorValorHotel != null)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorValores(hoteisFiltrados, SessionFiltroMenorValorHotel.Value, SessionFiltroMaiorValorHotel.Value);
                    }

                    break;

                case EnumFiltros.PorValores:

                    if (SessionFiltroFacilidadesHotel.Count() > 0)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorFacilidades(hoteisFiltrados, SessionFiltroFacilidadesHotel);
                    }

                    if (SessionFiltroHotelPorEstrelas.Count > 0)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorEstrelas(hoteisFiltrados, SessionFiltroHotelPorEstrelas);
                    }

                    if (SessionFiltroHotelPorNome != "")
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorNome(hoteisFiltrados, SessionFiltroHotelPorNome);
                    }


                    break;

                case EnumFiltros.PorFacilidades:

                    if (SessionFiltroHotelPorNome != "")
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorNome(hoteisFiltrados, SessionFiltroHotelPorNome);
                    }

                    if (SessionFiltroHotelPorEstrelas.Count > 0)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorEstrelas(hoteisFiltrados, SessionFiltroHotelPorEstrelas);
                    }

                    if (SessionFiltroMaiorValorHotel != null && SessionFiltroMenorValorHotel != null)
                    {
                        hoteisFiltrados = _hotelBussiness.FiltroHotelPorValores(hoteisFiltrados, SessionFiltroMenorValorHotel.Value, SessionFiltroMaiorValorHotel.Value);
                    }

                    break;
            }

            return hoteisFiltrados;
        }

        [HttpPost]
        public ActionResult FiltroHotelPorRangeValor(string menorPreco, string maiorPreco, int qtdeItensPorPagina = 10, int ordenacao = 3, string precoMaximo = "", string precoMinimo = "", bool isMontePacote = false)
        {
            List<HotelModel> hoteis = new List<HotelModel>();

            //if (Session["HoteilSessionPaginacaoFiltrado"] != null)
            //{
            //    hoteis = Session["HoteilSessionPaginacaoFiltrado"] as List<HotelModel>;
            //}
            //else
            //{
            //    hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", 20, () =>
            //    {
            //        return null;
            //    });
            //}

            //if (hoteis == null || hoteis.Count == 0 || (Conv.ToDecimal(maiorPreco) == Conv.ToDecimal(precoMaximo) || (Conv.ToDecimal(menorPreco) == 0 || Conv.ToDecimal(menorPreco) <= Conv.ToDecimal(precoMinimo))))
            //{
                //if (Session["HoteilSession"] != null)
                //{
                //    hoteis = Session["HoteilSession"] as List<HotelModel>;
                //}
                //else
                //{
                    hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
                    {
                        return new List<HotelModel>();
                    });
                //}
            //}

                    if (isMontePacote)
                    {
                        ViewBag.IsMontePacote = true;

                        if (Session["MPFiltro"] != null)
                        {
                            if ((bool)Session["MPFiltro"] == true)
                            {
                                ViewBag.IsMontePacote = true;
                                Session["MPFiltro"] = false;
                            }
                        } 
                    }

            HotelModel[] lstHoteis = new HotelModel[hoteis.Count];

            hoteis.CopyTo(lstHoteis);

            var hoteisFiltrados = _hotelBussiness.FiltroHotelPorValores(lstHoteis.ToList(), Conv.ToDecimal(menorPreco), Conv.ToDecimal(maiorPreco));

            SessionFiltroMenorValorHotel = Conv.ToDecimal(menorPreco);
            SessionFiltroMaiorValorHotel = Conv.ToDecimal(maiorPreco);

            hoteisFiltrados = AplicarFiltrosAnteriores(hoteisFiltrados, EnumFiltros.PorValores);

            if (ordenacao == 3)
            {
                hoteisFiltrados = hoteisFiltrados.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteisFiltrados = hoteisFiltrados.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            Session["HoteilSessionPaginacaoFiltrado"] = hoteisFiltrados;

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", hoteisFiltrados, TimeSpan.FromMinutes(20));

            hoteisFiltrados = hoteisFiltrados.ToPagedList(1, qtdeItensPorPagina).ToList();

            ViewBag.Carregado = true;

            return PartialView("ResultadoGrid", hoteisFiltrados);
        }

        public string TotalHoteisFiltrados()
        {
            var hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey + "filtrado", 20, () =>
            {
                return new List<HotelModel>();
            });

            string retorno = "0";

            if (hoteis != null)
            {
                retorno = hoteis.Count.ToString();
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se a pesquisa assíncrona retornou que já está finalizad
        /// </summary>
        /// <returns></returns>
        public bool VerificarConsultaFinalizada()
        {
            bool retorno = false;

            _hotelBussiness.RetornarConsultaAssincronaFinalizada(Session["CodigoConsulta"].ToString(), SessionKey);

            var isFinalizado = AppCache.Get<string>(EnumProcesso.HotelVenda, "ConsultaFinalizada" + SessionKey, 20, () =>
            {
                return String.Empty;
            });

            if (!String.IsNullOrEmpty(isFinalizado) && isFinalizado.ToUpper() == "S")
            {
                retorno = true;
            }

            if (retorno)
            {
                Session["ConsultaFinalizadaMP"] = true;
            }
            else
            {
                Session["ConsultaFinalizadaMP"] = null;
            }

            return retorno;
        }

        public ActionResult FiltroHotelMP(string nome, string menorPreco, string maiorPreco, string estrela, string lstFacilidades, string hotelBuscacheckin, string hotelBuscaCheckout, string order, string quantAdultos, string quantCriancas, string codigocidade)
        {
            try
            {
                HotelModel hotelBusca = new HotelModel
                {
                    QuantAdultos = Conv.ToInt32(quantAdultos),
                    QuantCriancas = Conv.ToInt32(quantCriancas),
                    Checkin = hotelBuscacheckin,
                    Checkout = hotelBuscaCheckout,
                    CodigoCidade = codigocidade
                };

                List<int> estrelasFiltro = new List<int>();
                List<string> facilidadesFiltro = new List<string>();

                if (!String.IsNullOrEmpty(estrela))
                {
                    string[] estrelas = estrela.Remove(estrela.Length - 1, 1).ToString().Split(',');
                    for (int i = 0; i < estrelas.Length; i++)
                    {
                        estrelasFiltro.Add(Conv.ToInt32(estrelas[i]));
                    }
                }
                else { estrelasFiltro = null; }

                if (!String.IsNullOrEmpty(lstFacilidades))
                {
                    string[] facilidades = lstFacilidades.Remove(lstFacilidades.Length - 1, 1).ToString().Split(',');
                    for (int i = 0; i < facilidades.Length; i++)
                    {
                        facilidadesFiltro.Add(facilidades[i]);
                    }
                }
                else { facilidadesFiltro = null; }

                List<HotelModel> HotelSession = new List<HotelModel>();

                if (SessionUsuario != null && SessionUsuario.ID_PESSOA_AGENCIA.HasValue)
                    hotelBusca.CodigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();

                if (Session["HoteilSession"] != null)
                    HotelSession = (List<HotelModel>)Session["HoteilSession"];
                else
                    HotelSession = _hotelBussiness.ResultHoteis(hotelBusca, SessionUsuario);

                var hoteisEstrelas = _hotelBussiness.FiltroHotel(HotelSession, hotelBusca, Conv.ToDecimal(menorPreco), Conv.ToDecimal(maiorPreco), nome, estrelasFiltro, facilidadesFiltro, Conv.ToInt32(order));

                hoteisEstrelas = hoteisEstrelas.OrderBy(p => p.ValorFinalMenorPreco).ToList();

                return RedirectToAction("ResultadoGridMontePacote", "Hotel", hoteisEstrelas);
            }
            catch (Exception)
            {
                return PartialView("ResultadoGrid");
            }
        }

        public void CarregarCombos()
        {
            ViewBag.ComboQuartos = CombosHotelModel.Quartos;
            ViewBag.ComboAdultos = CombosHotelModel.Adultos;
            ViewBag.ComboClasses = CombosHotelModel.Classes;
            ViewBag.ComboCriancas = CombosHotelModel.Criancas;
            ViewBag.ComboConfirmacoes = CombosHotelModel.Confirmacoes;
        }

        public ActionResult ResultadoGrid(List<HotelModel> hotel)
        {
            var success = AppCache.Put(EnumProcesso.HotelVenda, String.Format("{0}{1}", SessionKey, "Hotel"), hotel, TimeSpan.FromMinutes(20));
            return PartialView("ResultadoGrid", hotel);
        }

        public ActionResult ResultadoGridMontePacote(List<HotelModel> hotel)
        {
            ViewBag.IsMontePacote = true;

            if (hotel == null && Session["HoteilSession"] != null)
            {
                hotel = (List<HotelModel>)Session["HoteilSession"];
            }

            var l = 1;

            List<HoteisMapaModel> HoteisMapa = new List<HoteisMapaModel>();

            foreach (var item in hotel)
            {
                item.NomeCidade = MPSession.Destinos[Convert.ToInt32(Session["IndiceAdicionarHotel"])].NomeCidadeDestino;

                if (!String.IsNullOrEmpty(item.Latitude) && !String.IsNullOrEmpty(item.Longitude))
                {
                    var hotelmapa = new HoteisMapaModel();

                    hotelmapa.Latidude = item.Latitude.Replace(",", ".");
                    hotelmapa.Longitude = item.Longitude.Replace(",", ".");
                    hotelmapa.Order = l;
                    hotelmapa.Titulo = item.NomeHotel;
                    hotelmapa.ValorDiaria = item.ValorFinalMenorPreco;
                    hotelmapa.Endereco = String.Format("{0} - {1}{2} - {3}", item.Endereco, item.CodigoCidade, item.NomeEstado, item.Cep);
                    hotelmapa.NumeroEstrelas = item.NumeroEstrelas;
                    hotelmapa.CodigoHotel = item.CodigoHotel;
                    hotelmapa.Checkin = item.Checkin;
                    hotelmapa.Checkout = item.Checkout;
                    hotelmapa.QuantAdultos = item.QuantAdultos;
                    hotelmapa.QuantCriancas = item.QuantCriancas;
                    hotelmapa.CodigoCidade = item.CodigoCidade;
                    if (item.HotelFotos.Count() > 0)
                    { hotelmapa.Foto = item.HotelFotos.FirstOrDefault().HotelFotoUrl; }
                    HoteisMapa.Add(hotelmapa);
                    l++;
                }
            }

            hotel = hotel.OrderBy(p => p.ValorFinalMenorPreco).ToList();

            var paginado = hotel.ToPagedList(1, 10);

            List<HotelModel> lstHoteis = new List<HotelModel>();

            foreach (var item in paginado)
            {
                lstHoteis.Add(item);
            }

            string isAssincrono = ConfigurationManager.AppSettings["ConsultaAssincrona"];

            if (isAssincrono.ToUpper() == "S")
            {
                ViewBag.ContadorAssincrono = "setTimeout(VerificarNovosHoteis, totalMilisegundosBusca);";
            }
            else
            {
                ViewBag.ContadorAssincrono = "";
            }

            ViewBag.HotelMapa = HoteisMapa;
            return PartialView("ResultadoGrid", lstHoteis);
        }

        public virtual PartialViewResult HotelDestaque()
        {
            var DestaqueFlytour = AppCache.Get<List<HotelDestaqueModel>>(EnumProcesso.PacoteMatriz, "HotelDestaque", 30, () =>
            {
                return new List<HotelDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var hoteisDestaque = _hotelBussiness.BuscarHotelDestaque();
                if (hoteisDestaque.Count > 1)
                {
                    hoteisDestaque = _common.AddSevenDays(hoteisDestaque);
                    var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaque", hoteisDestaque, TimeSpan.FromMinutes(30));
                    return PartialView(_common.AddSevenDays(hoteisDestaque));
                }
                else
                {
                    return PartialView();
                }
            }

        }

        public virtual PartialViewResult HotelDestaqueHotsite()
        {
            var DestaqueFlytour = AppCache.Get<List<HotelDestaqueModel>>(EnumProcesso.PacoteMatriz, "HotelDestaqueHotsite", 30, () =>
            {
                return new List<HotelDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var hoteisDestaque = _hotelBussiness.BuscarHotelDestaque();
                if (hoteisDestaque.Count > 1)
                {
                    hoteisDestaque = _common.AddSevenDays(hoteisDestaque);
                    var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueHotsite", hoteisDestaque, TimeSpan.FromMinutes(30));
                    return PartialView(_common.AddSevenDays(hoteisDestaque));
                }
                else
                {
                    return PartialView();
                }
            }
        }

        public virtual PartialViewResult HotelDestaqueInferior()
        {
            var DestaqueFlytour = AppCache.Get<List<HotelDestaqueModel>>(EnumProcesso.PacoteMatriz, "HotelDestaqueInferior", 30, () =>
            {
                return new List<HotelDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var hoteisDestaque = _hotelBussiness.BuscarHotelDestaqueInferior();
                if (hoteisDestaque.Count > 1)
                {
                    hoteisDestaque = _common.AddSevenDays(hoteisDestaque);
                    var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueInferior", hoteisDestaque, TimeSpan.FromMinutes(30));
                    return PartialView(_common.AddSevenDays(hoteisDestaque));
                }
                else
                {
                    return PartialView();
                }
            }
        }

        public virtual PartialViewResult HotelDestaqueInternacional()
        {
            var DestaqueFlytour = AppCache.Get<List<HotelDestaqueModel>>(EnumProcesso.PacoteMatriz, "HotelDestaqueInternacional", 30, () =>
            {
                return new List<HotelDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var hoteisDestaque = _hotelBussiness.BuscarHotelDestaqueInternacional();
                if (hoteisDestaque.Count > 1)
                {
                    hoteisDestaque = _common.AddSevenDays(hoteisDestaque);
                    var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueInternacional", hoteisDestaque, TimeSpan.FromMinutes(30));
                    return PartialView(_common.AddSevenDays(hoteisDestaque));
                }
                else
                {
                    return PartialView();
                }
            }
        }

        public virtual PartialViewResult HotelDestaqueInternacionalHotsite()
        {
            var DestaqueFlytour = AppCache.Get<List<HotelDestaqueModel>>(EnumProcesso.PacoteMatriz, "HotelDestaqueInternacionalHotsite", 30, () =>
            {
                return new List<HotelDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var hoteisDestaque = _hotelBussiness.BuscarHotelDestaqueInternacional();
                if (hoteisDestaque.Count > 1)
                {
                    hoteisDestaque = _common.AddSevenDays(hoteisDestaque);
                    var success = AppCache.Put(EnumProcesso.PacoteMatriz, "HotelDestaqueInternacionalHotsite", hoteisDestaque, TimeSpan.FromMinutes(30));
                    return PartialView(_common.AddSevenDays(hoteisDestaque));
                }
                else
                {
                    return PartialView();
                }
            }
        }

        public ActionResult HotelDetalhes(HotelModel hotel)
        {
            return PartialView(hotel);
        }

        public JsonResult HotelGallery(List<HotelFotoModel> fotos)
        {
            var fotosHotel = new List<FotosHotel>();

            foreach (var item in fotos)
            {
                var fotoresultado = new FotosHotel();
                fotoresultado.href = item.HotelFotoUrl;
                fotoresultado.title = item.HotelFotoTitulo;
                fotosHotel.Add(fotoresultado);
            }

            return Json(fotosHotel);
        }

        [HttpPost]
        public ActionResult ResultadoGridOrdenado(string idCity, string ckIn, string ckOut, string order, string qtAdult, string qtChild)
        {
            int qtdAdultoConvertido = Conv.ToInt32(qtAdult);
            int qtdCriancaConvertido = Conv.ToInt32(qtChild);

            var hotelBuscas = new HotelModel();
            hotelBuscas.CodigoCidade = idCity;
            hotelBuscas.Checkin = ckIn;
            hotelBuscas.Checkout = ckOut;
            hotelBuscas.QuantAdultos = qtdAdultoConvertido;
            hotelBuscas.QuantCriancas = qtdCriancaConvertido;


            int Adultos = 0;
            int Criancas = 0;
            int Bebes = 0;
            int Total = 0;
            var ApartamentoList = new List<ApartamentoModel>();
            for (int i = 1; i <= 1; i++)
            {
                var ApartamentoIN = new ApartamentoModel();


                ApartamentoIN.Passageiros = new List<PassageiroModel>();

                if (qtdAdultoConvertido > 0)
                {
                    for (int k = 0; k < qtdAdultoConvertido; k++)
                    {
                        // PARA ENVIO DA RESERVA E USO DO FILTRO
                        var PassageiroIN = new PassageiroModel();
                        PassageiroIN.IdadePassageiro = 30;
                        ApartamentoIN.Passageiros.Add(PassageiroIN);
                        Total++;
                        Adultos++;
                    }
                }

                if (qtdCriancaConvertido > 0)
                {
                    for (int j = 0; j < qtdCriancaConvertido; j++)
                    {
                        // PARA ENVIO DA RESERVA E USO DO FILTRO
                        var PassageiroIN = new PassageiroModel();
                        PassageiroIN.IdadePassageiro = qtdCriancaConvertido;
                        if (PassageiroIN.IdadePassageiro < 2)
                            Bebes++;
                        else
                            Criancas++;
                        Total++;
                        ApartamentoIN.Passageiros.Add(PassageiroIN);
                    }
                }
                ApartamentoList.Add(ApartamentoIN);
            }
            hotelBuscas.Apartamentos = ApartamentoList;



            List<HotelModel> hoteis = BuscarHoteis(hotelBuscas);
            hoteis = _hotelBussiness.OrdenarHoteis(hoteis, Conv.ToInt32(order));

            return PartialView("ResultadoGrid", hoteis);
        }

        [HttpPost]
        public ActionResult ResultadoGridNovaOrdenacao(int ordem, int qtdItensPorPagia = 10, bool isMontePacote = false)
        {
            List<HotelModel> hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
            {
                return new List<HotelModel>();
            });

            if (ordem == 1)
                hoteis = hoteis.OrderBy(x => x.NomeHotel).ToList();
            else if (ordem == 2)
                hoteis = hoteis.OrderByDescending(x => x.NomeHotel).ToList();
            else if (ordem == 3)
                hoteis = hoteis.OrderBy(x => x.ValorFinalMenorPreco).ToList();
            else if (ordem == 4)
                hoteis = hoteis.OrderByDescending(x => x.ValorFinalMenorPreco).ToList();

            var inserido = AppCache.Put(EnumProcesso.HotelVenda, "paginacao" + SessionKey, hoteis, TimeSpan.FromMinutes(20));

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;
                ViewBag.TotalItensPagina = qtdItensPorPagia;
                ViewBag.Ordenacao = ordem;
                ViewBag.Carregado = true;
                Session["TotalHoteis"] = hoteis.Count();
            }

            var paginado = hoteis.ToPagedList(1, qtdItensPorPagia);

            hoteis.Clear();

            foreach (var item in paginado)
            {
                hoteis.Add(item);
            }

            return PartialView("ResultadoGrid", hoteis);
        }

        [HttpPost]
        public ActionResult AlterarQuantidadeItens(int qtdItensPorPagina = 10, bool isMontePacote = false, int ordenacao = 3)
        {
            List<HotelModel> hoteis = AppCache.Get<List<HotelModel>>(EnumProcesso.HotelVenda, "paginacao" + SessionKey, 20, () =>
            {
                return new List<HotelModel>();
            });

            if (ordenacao == 3)
            {
                hoteis = hoteis.OrderBy(p => p.ValorFinalMenorPreco).ToList();
            }
            else
            {
                hoteis = hoteis.OrderByDescending(p => p.ValorFinalMenorPreco).ToList();
            }

            if (isMontePacote)
            {
                ViewBag.IsMontePacote = true;
                ViewBag.TotalItensPagina = qtdItensPorPagina;
                ViewBag.Ordenacao = ordenacao;
                ViewBag.Carregado = true;
                Session["TotalHoteis"] = hoteis.Count();
            }

            var paginado = hoteis.ToPagedList(1, qtdItensPorPagina);

            List<HotelModel> hoteisPaginados = new List<HotelModel>();

            foreach (var item in paginado)
            {
                hoteisPaginados.Add(item);
            }

            return PartialView("ResultadoGrid", hoteisPaginados);
        }

        [HttpPost]
        public ActionResult ResultadoGridOrdenadoMP(string idCity, string ckIn, string ckOut, string order, string qtAdult, string qtChild)
        {
            int qtdAdultoConvertido = Conv.ToInt32(qtAdult);
            int qtdCriancaConvertido = Conv.ToInt32(qtChild);


            var hotelBuscas = new HotelModel();
            hotelBuscas.CodigoCidade = idCity;
            hotelBuscas.Checkin = ckIn;
            hotelBuscas.Checkout = ckOut;
            hotelBuscas.QuantAdultos = qtdAdultoConvertido;
            hotelBuscas.QuantCriancas = Conv.ToInt32(qtChild);


            int Adultos = 0;
            int Criancas = 0;
            int Bebes = 0;
            int Total = 0;
            var ApartamentoList = new List<ApartamentoModel>();
            for (int i = 1; i <= 1; i++)
            {
                var ApartamentoIN = new ApartamentoModel();


                ApartamentoIN.Passageiros = new List<PassageiroModel>();

                if (qtdAdultoConvertido > 0)
                {
                    for (int k = 0; k < qtdAdultoConvertido; k++)
                    {
                        // PARA ENVIO DA RESERVA E USO DO FILTRO
                        var PassageiroIN = new PassageiroModel();
                        PassageiroIN.IdadePassageiro = 30;
                        ApartamentoIN.Passageiros.Add(PassageiroIN);
                        Total++;
                        Adultos++;
                    }
                }

                if (qtdCriancaConvertido > 0)
                {
                    for (int j = 0; j < qtdCriancaConvertido; j++)
                    {
                        // PARA ENVIO DA RESERVA E USO DO FILTRO
                        var PassageiroIN = new PassageiroModel();
                        PassageiroIN.IdadePassageiro = qtdCriancaConvertido;
                        if (PassageiroIN.IdadePassageiro < 2)
                            Bebes++;
                        else
                            Criancas++;
                        Total++;
                        ApartamentoIN.Passageiros.Add(PassageiroIN);
                    }
                }
                ApartamentoList.Add(ApartamentoIN);
            }
            hotelBuscas.Apartamentos = ApartamentoList;

            List<HotelModel> hoteis = BuscarHoteis(hotelBuscas);
            hoteis = _hotelBussiness.OrdenarHoteis(hoteis, Conv.ToInt32(order));
            ViewBag.IsMontePacote = true;
            return PartialView("ResultadoGrid", hoteis);
        }

        public virtual ActionResult HotelMenuForm()
        {
            HotelModel hotel = new HotelModel();
            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            if (addedFlytour.UtilizaApartamentos)
            {
                Session["UtilizaApartamentos"] = "S";
            }
            else
            {
                Session["UtilizaApartamentos"] = "N";
            }

            //if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0)
            if (BloquearApartamentizacao(addedFlytour))
            {
                hotel.Apartamentos = addedFlytour.Apartamentos;
            }
            else
            {
                var ApartamentosHotel = Session["Apartamentos"] as List<ApartamentoModel>;
                //if (ApartamentosHotel != null && ApartamentosHotel.Count > 0)
                if (BloquearApartamentizacao(addedFlytour))
                {
                    hotel.Apartamentos = ApartamentosHotel;
                    Session["Apartamentos"] = null;
                }
            }

            CarregarCombos();

            return View(hotel);
        }

        [HttpPost]
        public string FormasdePagamento(string CodigoTarifa, string NomeAcomodacao)
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

            var CodigoAcomocacao = PacoteHotel.Acomodacoes.Where(p => p.NomeAcomodacao == NomeAcomodacao).FirstOrDefault().CodigoAcomodacao;

            var flyTour = new FlyTourModel();

            flyTour.Hotel = new List<HotelModel>();
            PacoteHotel.Comprar = false;
            PacoteHotel.CodigoTarifa = CodigoTarifa;
            PacoteHotel.ValorTotal = PacoteHotel.Acomodacoes.SelectMany(p => p.Tarifas).First(v => v.CodigoTarifa == codigo).ValorTotal;
            PacoteHotel.ValorCusto = PacoteHotel.Acomodacoes.SelectMany(p => p.Tarifas).First(v => v.CodigoTarifa == codigo).ValorCusto;

            flyTour.Apartamentos = PacoteHotel.Apartamentos;

            flyTour.Hotel.Add(PacoteHotel);

            List<MaxParcelaRetorno> finan = new List<MaxParcelaRetorno>();

            finan = _pagamentoBussiness.RetornaFormasPagamento(flyTour, SessionUsuario);

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

    public class FotosHotel
    {
        public string href { get; set; }
        public string title { get; set; }
    }

    public enum EnumFiltros
    {
        PorEstrelas,
        PorNome,
        PorValores,
        PorFacilidades
    }
}