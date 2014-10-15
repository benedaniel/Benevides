using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using System.Web.Services;
using FTV;
using FT.Web.Site.Controllers.Base;
using FT.Web.Business.Pacote;
using FT.Web.Business.Destaque;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Business.Pagamento;
using System.Text;
using System.Web.Script.Serialization;

namespace FT.Web.Site.Controllers.Pacote
{
    [Tracer]
    public class PacoteController : _BaseController
    {
        FT.Web.Business.PacoteVendaService.PacoteVendaFiltro VendaFiltro = new FT.Web.Business.PacoteVendaService.PacoteVendaFiltro();
        PagamentoBusiness pagamentoBussiness = new PagamentoBusiness();
        PacoteBusiness pacoteBussiness = new PacoteBusiness();

        DestaqueBusiness destaqueBusiness = new DestaqueBusiness();

        public ActionResult Index()
        {
            return View();
        }

        public virtual PartialViewResult PacoteForm()
        {
            return PartialView();
        }

        public virtual ActionResult PacoteMenuForm()
        {
            return View();
        }

        [WebMethod]
        public PartialViewResult GridDetalhe()
        {

            string id = Request.Form["idGet"].ToString();
            string origem = Request.Form["origemGet"].ToString();
            string data = Request.Form["dataGet"].ToString();
            string nome = Request.Form["nomeGet"].ToString();

            int Adultos = 0;
            int Criancas = 0;
            int Bebes = 0;
            int Total = 0;

            FT.Web.Business.UsuarioService.Login login = LoginService();

            var Token = login.Token;
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

            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            IList<PacoteVendaModel> models = new List<PacoteVendaModel>();

            if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0)
            {
                if (addedFlytour.Carro.Where(c => !c.Comprar).Count() > 0
                    || addedFlytour.Pacote.Where(p => !p.Comprar).Count() > 0
                    || addedFlytour.Aereo.Where(a => !a.Comprar).Count() > 0
                    || addedFlytour.Hotel.Where(h => !h.Comprar).Count() > 0
                    || addedFlytour.Atividade.Where(a => !a.Comprar).Count() > 0)
                {
                    var quantidadeAdulto =
                        addedFlytour.Apartamentos.SelectMany(
                            p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).Count();

                    var quantidadeCrianca =
                        addedFlytour.Apartamentos.SelectMany(
                            p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();

                    var quantidadeBebe =
                        addedFlytour.Apartamentos.SelectMany(
                            p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count();

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

                            return PartialView("GridDetalhePacoteMatriz");

                        }

                        //if (Convert.ToInt32(Request.Form["hdnTotalPassageiro"]) != 2)
                        //{
                        //    if (Convert.ToInt32(Request.Form["hdnTotalPassageiro"]) != Total)
                        //    {
                        //        Response.Write(
                        //            "<div class='AlertaTotal'><span class='AlertaTotalSpan'>* Já existe produto selecionado em seu carrinho. Novos produtos devem possuir a mesma quantidade de passageiros.Favor selecionar a quantidade de passageiros conforme abaixo e refazer a busca.</span></div>");
                        //        Response.Write("<div class='AlertaTotal'><span class='AlertaTotalSpan'>Adultos:(" +
                        //                       quantidadeAdulto + ") Criancas:(" + quantidadeCrianca + ") Bebes:(" +
                        //                       quantidadeBebe + ")</span></div>");

                        //        return PartialView("GridDetalhePacoteMatriz");
                        //    }
                        //}
                    }
                }
            }

            string codigoAgencia = string.Empty;

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                codigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
            models = pacoteBussiness.LoadDetalhePacoteMatriz(nome, Convert.ToInt64(id), data, Convert.ToInt64(origem), ApartamentoList, Request.Form["ddlDataViagemDia"], Request.Form["ddlCidadesEmbarquePacDest"], codigoAgencia);

            var PacoteDescricaoServico = Session["PacoteDescricaoServico"] as List<PacoteDescricaoServicoModel>;

            var PacoteServico = Session["PacoteServico"] as List<PacoteServicoModel>;

            if (PacoteDescricaoServico != null)
            {
                for (int i = 0; i < models.Count; i++)
                {
                    models[i].PacoteDescricaoServico = PacoteDescricaoServico;
                }

            }

            if (PacoteServico != null)
            {

                for (int i = 0; i < models.Count; i++)
                {
                    models[i].PacoteServico = PacoteServico;
                }

            }

            var success = AppCache.Put(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", models, TimeSpan.FromMinutes(90));

            //Verifica se aplicará as regras de filtro de acordo com
            //as informações que o usuário aplicou
            if (Request.Form["hdnIsFiltro"] == "1")
            {
                ViewBag.IsFiltro = "1";

                if (!String.IsNullOrEmpty(Request.Form["txtNome"]))
                {
                    models = pacoteBussiness.FiltrarPorNomeHotel(models, Request.Form["txtNome"].ToLower());
                }

                if (!String.IsNullOrEmpty(Request.Form["stars"]))
                {
                    string estrela = Request.Form["stars"].ToString();

                    List<int> estrelasFiltro = new List<int>();

                    string[] estrelas = estrela.Split(',');
                    for (int i = 0; i < estrelas.Length; i++)
                    {
                        estrelasFiltro.Add(Conv.ToInt32(estrelas[i]));
                    }

                    if (estrelasFiltro.Count < 3)
                    {
                        models = pacoteBussiness.FiltrarPorEstrelas(models, estrelasFiltro);
                    }
                }

                if (Session["RefazerBusca"] != null)
                {
                    int contador = (int)Session["RefazerBusca"];

                    if (contador < 3)
                    {
                        contador = contador + 1;
                        Session["RefazerBusca"] = contador;
                    }
                    else
                        Session["RefazerBusca"] = null;
                }

                if (!String.IsNullOrEmpty(Request.Form["amount"]) && Session["RefazerBusca"] == null)
                {
                    string[] rangeValores = Request.Form["amount"].ToString().Split('-');

                    decimal valorMenor = 0, valorMaior = 0;

                    valorMenor = Convert.ToDecimal(rangeValores[0].TrimEnd().Replace("R$", ""));
                    valorMaior = Convert.ToDecimal(rangeValores[1].TrimStart().Replace("R$", ""));

                    models = pacoteBussiness.FiltrarPorValores(models, valorMenor, valorMaior);
                }
            }
            else
            {
                VerificarRangeValoresFiltro(models);

                ViewBag.IsFiltro = null;

                Session["RefazerBusca"] = 1;
            }

            AppCache.Put(EnumProcesso.PacoteMatriz, String.Format("{0}-Busca", SessionKey), models, TimeSpan.FromMinutes(20));

            return PartialView("GridDetalhePacoteMatriz", models);
        }

        // #region Registro de Reserva

        //[AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LoadDetalhePacoteMatriz(long id, string data, long? origem)
        {
            string codigoAgencia = string.Empty;
            IList<PacoteModel> PacoteMatriz = new List<PacoteModel>();
            if (SessionUsuario.ID_PESSOA_AGENCIA.HasValue)
                codigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();

            PacoteMatriz = pacoteBussiness.LoadPacoteMatriz(data, null, Convert.ToString(origem), Convert.ToString(id), codigoAgencia);

            //IList<PacoteVendaModel> models = new List<PacoteVendaModel>();

            if (PacoteMatriz != null && PacoteMatriz.Count > 0)
            {
                // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
                var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
                {
                    return new FlyTourModel();
                });

                //if (id != null)
                //{
                //    if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                //        codigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
                //    models = pacoteBussiness.LoadDetalhePacoteMatriz(null, id, data, origem, addedFlytour.Apartamentos, null, null, codigoAgencia);
                //}

                PacoteMatriz[0].Apartamentos = addedFlytour.Apartamentos;

                if (PacoteMatriz.Count != 0)
                {
                    Session["PacoteSaidaModel"] = PacoteMatriz[0].Datasaida;
                }
                else
                {
                    Session["PacoteSaidaModel"] = null;
                }

                if (PacoteMatriz[0].PacoteDescricaoServico != null && PacoteMatriz[0].PacoteDescricaoServico.Count > 0)
                {
                    Session["PacoteDescricaoServico"] = PacoteMatriz[0].PacoteDescricaoServico;
                }
                else
                {
                    Session["PacoteDescricaoServico"] = null;
                }
                if (PacoteMatriz[0].PacoteServico != null && PacoteMatriz[0].PacoteServico.Count > 0)
                {
                    Session["PacoteServico"] = PacoteMatriz[0].PacoteServico;
                }
                else
                {
                    Session["PacoteServico"] = null;
                }
            }

            //VerificarRangeValoresFiltro(models);

            AppCache.Put(EnumProcesso.PacoteMatriz, String.Format("{0}-Busca", SessionKey), PacoteMatriz, TimeSpan.FromMinutes(20));

            return View(PacoteMatriz);
        }

        /// <summary>
        /// Faz a busca dos valores mínimos e máximos do pacote para
        /// serem utilizados no filtro de pacotes
        /// </summary>
        /// <Author>Daniel Benevides</Author>
        /// <param name="models"></param>
        private void VerificarRangeValoresFiltro(IList<PacoteVendaModel> models)
        {
            if (models.Count > 0)
            {
                var modelsOrdenadorValor = models.OrderBy(m => m.ValorVenda).ToList();

                var valorMenor = modelsOrdenadorValor.Min(m => m.ValorVenda);
                var valorMaior = modelsOrdenadorValor.Max(m => m.ValorVenda);

                ViewBag.ValorMinimo = Decimal.Round(valorMenor);
                ViewBag.ValorMaximo = Decimal.Round(valorMaior);
            }
        }

        public ActionResult FiltroPacote()
        {
            var pacoteMatriz = AppCache.Get(EnumProcesso.PacoteMatriz, String.Format("{0}-Busca", SessionKey), 90, () =>
            {
                return new List<PacoteVendaModel>();
            });

            VerificarRangeValoresFiltro(pacoteMatriz);

            return View("LoadDetalhePacoteMatriz", pacoteMatriz);
        }

        [WebMethod]
        public PartialViewResult GridDetalhePacoteMatriz(string nome, long? id, string dataget, long? origem)
        {


            // CARREGAR PASSAGEIROS CASO JÁ PREENCHIDOS
            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            IList<PacoteVendaModel> models = new List<PacoteVendaModel>();

            if (id != null)
            {
                string codigoAgencia = string.Empty;

                if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                    codigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
                models = pacoteBussiness.LoadDetalhePacoteMatriz(nome, id, dataget, origem, addedFlytour.Apartamentos, null, null, codigoAgencia);
            }


            var PacoteDescricaoServico = Session["PacoteDescricaoServico"] as List<PacoteDescricaoServicoModel>;

            var PacoteServico = Session["PacoteServico"] as List<PacoteServicoModel>;

            if (PacoteDescricaoServico != null)
            {

                for (int i = 0; i < models.Count; i++)
                {
                    models[i].PacoteDescricaoServico = PacoteDescricaoServico;
                }
            }

            if (PacoteDescricaoServico != null)
            {

                for (int i = 0; i < models.Count; i++)
                {
                    models[i].PacoteServico = PacoteServico;
                }
            }

            var success = AppCache.Put(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", models, TimeSpan.FromMinutes(90));

            VerificarRangeValoresFiltro(models);

            //ViewBag.ValorMinimo = Decimal.Round(Convert.ToDecimal(models.OrderBy(m => Convert.ToDecimal(m.ValorVenda)).First().ValorVenda));
            //ViewBag.ValorMaximo = Decimal.Round(Convert.ToDecimal(models.OrderBy(m => Convert.ToDecimal(m.ValorVenda)).Last().ValorVenda));


            return PartialView(models);
        }

        #region PACOTE MATRIZ
        [HttpPost]
        public ActionResult LoadPacoteMatriz()
        {
            IList<PacoteModel> models = new List<PacoteModel>();


            string codigoAgencia = string.Empty;

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                codigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
            models = pacoteBussiness.LoadPacoteMatriz(Request.Form["ddlDataVIagem"], Request.Form["ddlCidadesDestino"], Request.Form["ddlCidadesEmbarque"].ToString(), null, codigoAgencia);


            if (Request.Form["ddlTipoPrd"] == "2")
            {
                models = models.Where(item => item.PacoteServico.Any(id => id.idServico == 2)).ToList();
            }
            else if (Request.Form["ddlTipoPrd"] == "1")
            {
                models = models.Where(item => !item.PacoteServico.Any(id => id.idServico == 2)).ToList();
            }

            models = models.OrderBy(x => Convert.ToDecimal(x.ValorPacote)).ToList();
            return View(models);
        }

        [HttpPost]
        public JsonResult LoadPacoteMatrizDestino(string ddlDataVIagem, string ddlCidadesDestino, string ddlCidadesEmbarque)
        {
            IList<PacoteModel> models = new List<PacoteModel>();
            string codigoAgencia = string.Empty;

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                codigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
            models = pacoteBussiness.LoadPacoteMatriz(ddlDataVIagem, ddlCidadesDestino, ddlCidadesEmbarque, null, codigoAgencia);
            models = models.OrderBy(x => Convert.ToDecimal(x.ValorPacote)).ToList();

            var jsonResult = Json(models, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [WebMethod]
        public PartialViewResult GridPacoteMatriz(string DataViagem, string CidadeDestino, string CidadeEmbarque, string OrdenarPor, string TipoProduto)
        {
            IList<PacoteModel> models = new List<PacoteModel>();
            string codigoAgencia = string.Empty;

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                codigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();
            models = pacoteBussiness.LoadPacoteMatriz(DataViagem, CidadeDestino, CidadeEmbarque, null, codigoAgencia);

            if (TipoProduto == "2")
            {
                models = models.Where(item => item.PacoteServico.Any(id => id.idServico == 2)).ToList();
            }
            else if (TipoProduto == "1")
            {
                models = models.Where(item => !item.PacoteServico.Any(id => id.idServico == 2)).ToList();
            }

            if (OrdenarPor == "1")
                models = models.OrderBy(x => x.NomePacote).ToList();

            if (OrdenarPor == "2")
                models = models.OrderByDescending(x => x.NomePacote).ToList();

            if (OrdenarPor == "3")
                models = models.OrderBy(x => Convert.ToDecimal(x.ValorPacote)).ToList();

            if (OrdenarPor == "4")
                models = models.OrderByDescending(x => Convert.ToDecimal(x.ValorPacote)).ToList();


            return PartialView(models);
        }


        #endregion

        #region Filtro Principal
        [WebMethod]
        public JsonResult LoadDataViagem(string codigoCidadeOrigem, string codigoCidadeDestino, string indicadorAgrupMes)
        {
            var pacote = new PacoteModel();
            pacote = pacoteBussiness.LoadDataViagem(codigoCidadeOrigem, codigoCidadeDestino, indicadorAgrupMes);
            return Json(pacote.PacoteSaida, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult LoadDataViagemDestino(string codigoCidadeOrigem, string codigoCidadeDestino, string indicadorAgrupMes)
        {
            var pacote = new PacoteModel();
            var pacoteSaida = new PacoteSaidaModel();
            var pacoteSaidaList = new List<PacoteSaidaModel>();


            var PacoteSaidaModel = Session["PacoteSaidaModel"] as List<PacoteDataSaidaModel>;

            if (PacoteSaidaModel != null)
            {
                if (codigoCidadeOrigem != null)
                {
                    foreach (var item in PacoteSaidaModel.Where(x => x.ID_cidade == Convert.ToInt32(codigoCidadeOrigem)))
                    {
                        foreach (var Mes in item.Messaida)
                        {
                            pacoteSaida = new PacoteSaidaModel();
                            pacoteSaida.AnoSaida = Mes.anoSaida;
                            pacoteSaida.Mes = Mes.Mes;
                            pacoteSaida.NomeMes = Mes.NomeMes;
                            pacoteSaidaList.Add(pacoteSaida);
                        }

                    }
                }
            }

            return Json(pacoteSaidaList, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult LoadDiaViagem(string codigoCidadeOrigem, string codigoCidadeDestino, string indicadorAgrupMes)
        {
            var pacote = new PacoteModel();
            var pacoteSaida = new PacoteSaidaModel();
            var pacoteSaidaList = new List<PacoteSaidaModel>();


            var PacoteSaidaModel = Session["PacoteSaidaModel"] as List<PacoteDataSaidaModel>;

            if (PacoteSaidaModel != null)
            {
                foreach (var item in PacoteSaidaModel.Where(x => x.ID_cidade == Convert.ToInt32(codigoCidadeOrigem)))
                {
                    foreach (var Dia in item.Datasaida.Where(x => x.MesSaida == indicadorAgrupMes.ToString()))
                    {
                        pacoteSaida = new PacoteSaidaModel();
                        pacoteSaida.AnoSaida = Dia.AnoSaida;
                        pacoteSaida.MesSaida = Dia.MesSaida;
                        pacoteSaida.DiaSaida = Dia.DiaSaida;
                        pacoteSaidaList.Add(pacoteSaida);
                    }

                }
            }

            return Json(pacoteSaidaList, JsonRequestBehavior.AllowGet);
        }

        [WebMethod]
        public JsonResult LoadDestino(string codigoCidadeOrigem, string embarqueFiltro)
        {
            var paisSL = new PaisModel();
            paisSL = pacoteBussiness.LoadDestino(codigoCidadeOrigem, embarqueFiltro);
            return Json(paisSL.Cidade, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult LoadEmbarque(string embarqueFiltro)
        {
            var paisSL = new PaisModel();

            paisSL = pacoteBussiness.LoadEmbarque(embarqueFiltro);

            return Json(paisSL.Cidade, JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region PACOTE DESTAQUES
        public virtual PartialViewResult PacoteDestaque()
        {
            var origem = "";
            if (Session["Regiao"] != null)
                origem = Session["Regiao"].ToString();
            else
                origem = "9668";

            var DestaqueFlytour = AppCache.Get<List<DestaqueModel>>(EnumProcesso.PacoteMatriz, "PacoteDestaque" + origem, 2, () =>
            {
                return new List<DestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var pacotes = destaqueBusiness.LoadDestaques(20, origem, 1, 1);
                var grandes = pacotes.Where(p => p.DestaqueFotos.Any(b => b.AlturaFoto == 300 && b.LarguraFoto == 480)).OrderBy(r => Guid.NewGuid()).Take(5);
                var pequenos = pacotes.Where(p => p.DestaqueFotos.Any(b => b.AlturaFoto == 160 && b.LarguraFoto == 235)).OrderBy(r => Guid.NewGuid()).Take(2);
                List<DestaqueModel> result = new List<DestaqueModel>();
                result.AddRange(grandes);
                result.AddRange(pequenos);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaque" + origem, result, TimeSpan.FromMinutes(2));
                return PartialView(result);
            }
        }

        public virtual PartialViewResult PacoteDestaquesNacionais()
        {
            System.Diagnostics.Stopwatch swViewsControllerPacoteDestNasc = new System.Diagnostics.Stopwatch();
            swViewsControllerPacoteDestNasc.Start();
            var origem = "";
            if (Session["Regiao"] != null)
                origem = Session["Regiao"].ToString();
            else
                origem = "9668";

            var DestaqueFlytour = AppCache.Get<List<PacoteDestaqueModel>>(EnumProcesso.PacoteMatriz, "PacoteDestaquesNacionais" + origem, 60, () =>
            {
                return new List<PacoteDestaqueModel>();
            });


            if (DestaqueFlytour.Count > 0)
            {
                swViewsControllerPacoteDestNasc.Stop();
                
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var pacotes = pacoteBussiness.BuscarDestaques(12, 2, origem);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesNacionais" + origem, pacotes, TimeSpan.FromMinutes(60));
                swViewsControllerPacoteDestNasc.Stop();
                
                return PartialView(pacotes);
            }

        }

        public virtual PartialViewResult PacoteDestaquesInternacionais()
        {
            var origem = "";
            if (Session["Regiao"] != null)
                origem = Session["Regiao"].ToString();
            else
                origem = "9668";

            var DestaqueFlytour = AppCache.Get<List<PacoteDestaqueModel>>(EnumProcesso.PacoteMatriz, "PacoteDestaquesInternacionais" + origem, 60, () =>
            {
                return new List<PacoteDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var pacotes = pacoteBussiness.BuscarDestaques(12, 3, origem);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesInternacionais" + origem, pacotes, TimeSpan.FromMinutes(60));
                return PartialView(pacotes);
            }
        }

        public virtual PartialViewResult PacoteDestaquesNacionaisIframe()
        {
            var origem = "";
            if (Session["Regiao"] != null)
                origem = Session["Regiao"].ToString();
            else
                origem = "9668";

            var DestaqueFlytour = AppCache.Get<List<PacoteDestaqueModel>>(EnumProcesso.PacoteMatriz, "PacoteDestaquesNacionaisIframe" + origem, 60, () =>
            {
                return new List<PacoteDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var pacotes = pacoteBussiness.BuscarDestaques(12, 2, origem);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesNacionaisIframe" + origem, pacotes, TimeSpan.FromMinutes(60));
                return PartialView(pacotes);
            }
        }

        public virtual PartialViewResult PacoteDestaquesInternacionaisIframe()
        {
            var origem = "";
            if (Session["Regiao"] != null)
                origem = Session["Regiao"].ToString();
            else
                origem = "9668";

            var DestaqueFlytour = AppCache.Get<List<PacoteDestaqueModel>>(EnumProcesso.PacoteMatriz, "PacoteDestaquesInternacionaisIframe" + origem, 60, () =>
            {
                return new List<PacoteDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var pacotes = pacoteBussiness.BuscarDestaques(12, 3, origem);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesInternacionaisIframe" + origem, pacotes, TimeSpan.FromMinutes(60));
                return PartialView(pacotes);
            }
        }

        public virtual PartialViewResult PacoteDestaquesInferior()
        {
            var origem = "";
            if (Session["Regiao"] != null)
                origem = Session["Regiao"].ToString();
            else
                origem = "9668";

            var DestaqueFlytour = AppCache.Get<List<PacoteDestaqueModel>>(EnumProcesso.PacoteMatriz, "PacoteDestaquesInferior" + origem, 60, () =>
            {
                return new List<PacoteDestaqueModel>();
            });

            if (DestaqueFlytour.Count > 0)
            {
                return PartialView(DestaqueFlytour);
            }
            else
            {
                var pacotes = pacoteBussiness.BuscarDestaques(7, 4, origem);
                var success = AppCache.Put(EnumProcesso.PacoteMatriz, "PacoteDestaquesInferior" + origem, pacotes, TimeSpan.FromMinutes(60));
                return PartialView(pacotes);
            }
        }

        #endregion

        [HttpPost]
        public string FormasdePagamento(string CodigoPacote, string apartamentos = null)
        {
            var flyTour = new FlyTourModel();
            var pacote = new PacoteVendaModel();
            var PacotesFlytour = AppCache.Get<List<PacoteVendaModel>>(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", 90, () =>
            {
                return new List<PacoteVendaModel>();
            });

            if (PacotesFlytour == null || PacotesFlytour.Count == 0)
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                var ApartamentoList = (List<ApartamentoModel>)json.Deserialize(apartamentos, typeof(List<ApartamentoModel>));

                PacotesFlytour = PacoteBusiness.ConsultarPacoteVenda(CodigoPacote, ApartamentoList);
            }

            var success = AppCache.Put(EnumProcesso.PacoteVenda, SessionKey + "PacoteVenda", null, TimeSpan.FromMinutes(90));

            var PacoteCarrinho = PacotesFlytour.FirstOrDefault(p => p.CodigoPacote == CodigoPacote);


            if (PacoteCarrinho == null)
            {
                PacoteCarrinho = new PacoteVendaModel();
                PacoteCarrinho.CodigoPacote = CodigoPacote;
                PacoteCarrinho.Apartamento = new List<ApartamentoModel>();
                PassageiroModel pax = new PassageiroModel();
                pax.IdadePassageiro = 30;
                PassageiroModel pax1 = new PassageiroModel();
                pax1.IdadePassageiro = 30;
                ApartamentoModel apto = new ApartamentoModel();
                apto.Passageiros.Add(pax);
                apto.Passageiros.Add(pax1);
                PacoteCarrinho.Apartamento.Add(apto);
            }

            flyTour.Apartamentos = PacoteCarrinho.Apartamento;


            PacoteCarrinho.Comprar = true;

            flyTour.Pacote = new List<PacoteVendaModel>();
            flyTour.Pacote.Add(PacoteCarrinho);

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

        public JsonResult CalcularPrecoPacote(string CodigoPacote, string apartamentos)
        {

            JavaScriptSerializer json = new JavaScriptSerializer();
            var ApartamentoList = (List<ApartamentoModel>)json.Deserialize(apartamentos, typeof(List<ApartamentoModel>));

            var pacote = PacoteBusiness.TarifarPacoteVenda(CodigoPacote, ApartamentoList);
            return Json(new { Valor = Conv.ToDecimal(pacote.ValorVenda).ToString("n2"), Taxa = Conv.ToDecimal(pacote.ValorTaxa).ToString("n2"), SiglaMoeda = pacote.SiglaMoeda, ValorTotal = Conv.ToDecimal(pacote.ValorTotal).ToString("n2") });
        }
    }
}