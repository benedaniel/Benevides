using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Business.Aereo;
using FT.Web.Model.Models;
using FTV;
using FT.Web.Site.Controllers.Base;
using FT.Web.Core.Util.Validation;
using FTV.Shared.Business;
using FT.Web.Site.AutoComplete;
using System.Collections.Specialized;
using FT.Web.Core.Util.SiteTracer;
using FT.Web.Business.Pagamento;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Configuration;

namespace FT.Web.Site.Controllers.Aereo
{
    [Tracer]
    public class AereoController : _BaseController
    {
        //
        // GET: /Aereo/

        PagamentoBusiness pagamentoBussiness = new PagamentoBusiness();
        public ActionResult Index()
        {
            #region List

            var list = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                            "ID", "Name", 1);
            ViewData["Adultos"] = list;

            var list1 = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                "ID", "Name", 0);
            ViewData["Criancas"] = list1;

            ViewData["Bebes"] = list1;

            #endregion
            return View();
        }

        public virtual PartialViewResult AereoForm()
        {
            #region List

            var list = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                            "ID", "Name", 1);
            ViewData["Adultos"] = list;

            var list1 = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                "ID", "Name", 0);
            ViewData["Criancas"] = list1;

            ViewData["Bebes"] = list1;

            #endregion

            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            AereoModel model = new AereoModel();

            //if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0 && addedFlytour.Apartamentos.SelectMany(p => p.Passageiros).Count() > 0)
            if (BloquearApartamentizacao(addedFlytour))
            {
                model.QuantidadeAdulto = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).Count();
                model.QuantidadeCrianca = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();
                model.QuantidadeBebe = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count();
                model.BloquearPassageiros = true;
            }
            else
            {
                model.QuantidadeAdulto = 1;
                model.QuantidadeCrianca = 0;
                model.QuantidadeBebe = 0;
                model.BloquearPassageiros = false;
            }
            return PartialView(model);
        }

        public virtual PartialViewResult AereoPacotesForm()
        {
            #region List

            var list = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                            "ID", "Name", 1);
            ViewData["Adultos"] = list;

            var list1 = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                "ID", "Name", 0);
            ViewData["Criancas"] = list1;

            ViewData["Bebes"] = list1;

            #endregion

            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            AereoModel model = new AereoModel();

            //if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0 && addedFlytour.Apartamentos.SelectMany(p => p.Passageiros).Count() > 0)
            if (BloquearApartamentizacao(addedFlytour))
            {
                model.QuantidadeAdulto = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).Count();
                model.QuantidadeCrianca = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();
                model.QuantidadeBebe = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count();
                model.BloquearPassageiros = true;
            }
            else
            {
                model.QuantidadeAdulto = 1;
                model.QuantidadeCrianca = 0;
                model.QuantidadeBebe = 0;
                model.BloquearPassageiros = false;
            }
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult AereoResultado(AereoModel modelParametro, string hdnDestinoCodigoOld, string hdnOrigemCodigoOld, string hdnDestinoNomeOld, string hdnOrigemNomeOld, string hdnDestinoCodigo, string hdnOrigemCodigo, string hdnDestinoNome, string hdnOrigemNome, string ddlAdultos, string ddlCriancas, string ddlBebes)
        {
            if (String.IsNullOrEmpty(hdnOrigemCodigo))
                modelParametro.Origem = hdnOrigemCodigoOld;
            else
                modelParametro.Origem = hdnOrigemCodigo;

            if (String.IsNullOrEmpty(hdnDestinoCodigo))
                modelParametro.Destino = hdnDestinoCodigoOld;
            else
                modelParametro.Destino = hdnDestinoCodigo;

            if (String.IsNullOrEmpty(hdnOrigemNome))
                modelParametro.OrigemNome = hdnOrigemNomeOld;
            else
                modelParametro.OrigemNome = hdnOrigemNome;

            if (String.IsNullOrEmpty(hdnDestinoNome))
                modelParametro.DestinoNome = hdnDestinoNomeOld;
            else
                modelParametro.DestinoNome = hdnDestinoNome;

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");

            if (SessionUsuario != null && SessionUsuario.ID_PESSOA != null && ((Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA != null))
                modelParametro.CodigoAgencia = (Session["Usuario"] as FT.Web.Model.Models.PessoaModel).ID_PESSOA_AGENCIA.ToString();

            modelParametro.SomenteOffline = false;

            AereoModel AereoRetorno = AereoBusiness.Consultar(modelParametro);
            Session["AereoLista"] = AereoRetorno;

            if (Session["CriarViewBag"] != null)
            {
                ViewBag.IsMontePacote = true;

                Session["CriarViewBag"] = null;
            }

            return PartialView(AereoRetorno);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult FiltrarAereo(AereoModel modelParametro)
        {
            var newAereoRetorno = new AereoModel();
            var aereoRetorno = Session["AereoLista"] as AereoModel;
            ConvertMethod.EntityToEntity<AereoModel, AereoModel>(aereoRetorno, ref newAereoRetorno);

            if (aereoRetorno != null) newAereoRetorno.GrupoTarifas = aereoRetorno.GrupoTarifas;

            var combo = Request.Form["hdnComboPrice"];
            if (combo != "4")
            {
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.OrderBy(p => p.Valor).ToList();
                newAereoRetorno.Ordenar = "Menor";
            }
            else
            {
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.OrderByDescending(p => p.Valor).ToList();
                newAereoRetorno.Ordenar = "Maior";
            }

            var valor = Request.Form["hdnValorSelecionado"];
            if (!string.IsNullOrEmpty(valor))
            {
                var valorConv = Conv.ToDecimal(valor);
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.AsParallel().Where(p => p.Valor - 1 <= valorConv && p.Valor + 1 >= valorConv).ToList();
            }
            else
            {
                var amount = Request.Form["amount"];
                if (amount != null)
                {
                    var valorMenor = Conv.ToDecimal(amount.Split('-')[0].Replace("R$", "").Replace(" ", ""));
                    var valorMaior = Conv.ToDecimal(amount.Split('-')[1].Replace("R$", "").Replace(" ", ""));
                    newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.AsParallel().Where(p => p.Valor >= valorMenor - 1 && p.Valor <= valorMaior + 1).ToList();
                }
            }

            foreach (var grupo in newAereoRetorno.GrupoTarifas)
            {
                var i = 1;
                foreach (var tarifa in grupo.GrupoTarifaTrechos)
                {
                    foreach (var codigo in tarifa.TrechosTarifaCodigo)
                    {
                        codigo.Visible = true;

                        var tipoFiltro = Request.Form["hdnTipoFiltro"];

                        if ((i == 1) || (tipoFiltro == "MultiTrecho"))
                        {
                            var listaKeys = (from object item in Request.Form.Keys select item.ToString()).AsParallel().ToList();

                            if (listaKeys.All(p => p != ("Companhias-" + codigo.Trecho.NomeCia.Trim().Replace(" ",""))))
                                codigo.Visible = false;

                            if (listaKeys.All(p => p != ("AeroportoPartida-" + codigo.Trecho.SiglaAeroportoOrigem.Trim())))
                                codigo.Visible = false;

                            var horarioIdaMadrugada = Request.Form["HorarioIdaMadrugada"];

                            if (horarioIdaMadrugada == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 0 && codigo.Trecho.DataOrigem.Hour < 6))
                                    codigo.Visible = false;
                            }

                            var escalaVooDireto = Request.Form["EscalaVooDireto"];

                            if (escalaVooDireto == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count == 1)
                                    codigo.Visible = false;
                            }
                            var escalaUmaParada = Request.Form["EscalaUmaParada"];

                            if (escalaUmaParada == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 1)
                                    codigo.Visible = false;
                            }
                            var escalaDuasParadas = Request.Form["EscalaDuasParadas"];

                            if (escalaDuasParadas == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 2)
                                    codigo.Visible = false;
                            }

                            var horarioIdaManha = Request.Form["HorarioIdaManha"];

                            if (horarioIdaManha == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour > 5 && codigo.Trecho.DataOrigem.Hour <= 11))
                                    codigo.Visible = false;
                            }

                            var horarioIdaTarde = Request.Form["HorarioIdaTarde"];

                            if (horarioIdaTarde == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 12 && codigo.Trecho.DataOrigem.Hour < 18))
                                    codigo.Visible = false;
                            }

                            var horarioIdaNoite = Request.Form["HorarioIdaNoite"];

                            if (horarioIdaNoite == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 18 && codigo.Trecho.DataOrigem.Hour <= 23))
                                    codigo.Visible = false;
                            }
                        }
                        else if (grupo.GrupoTarifaTrechos.Count > 1)
                        {
                            var listaKeys = (from object item in Request.Form.Keys select item.ToString()).AsParallel().ToList();

                            if (listaKeys.All(p => p != ("Companhias-" + codigo.Trecho.NomeCia.Trim().Replace(" ", ""))))
                                codigo.Visible = false;

                            if (listaKeys.All(p => p != ("AeroportoDestino-" + codigo.Trecho.SiglaAeroportoOrigem.Trim())))
                                codigo.Visible = false;

                            var horarioIdaMadrugada = Request.Form["HorarioVoltaMadrugada"];
                            if (horarioIdaMadrugada == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 0 && codigo.Trecho.DataOrigem.Hour < 6))
                                    codigo.Visible = false;
                            }

                            var horarioIdaManha = Request.Form["HorarioVoltaManha"];
                            if (horarioIdaManha == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour > 5 && codigo.Trecho.DataOrigem.Hour <= 11))
                                    codigo.Visible = false;
                            }

                            var horarioIdaTarde = Request.Form["HorarioVoltaTarde"];
                            if (horarioIdaTarde == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 12 && codigo.Trecho.DataOrigem.Hour < 18))
                                    codigo.Visible = false;
                            }

                            var horarioIdaNoite = Request.Form["HorarioVoltaNoite"];
                            if (horarioIdaNoite == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 18 && codigo.Trecho.DataOrigem.Hour <= 23))
                                    codigo.Visible = false;
                            }

                            var escalaVooDireto = Request.Form["EscalaVooDireto"];
                            if (escalaVooDireto == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count == 1)
                                    codigo.Visible = false;
                            }
                            var escalaUmaParada = Request.Form["EscalaUmaParada"];
                            if (escalaUmaParada == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 1)
                                    codigo.Visible = false;
                            }
                            var escalaDuasParadas = Request.Form["EscalaDuasParadas"];
                            if (escalaDuasParadas == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 2)
                                    codigo.Visible = false;
                            }
                        }

                    }
                    i++;
                }
            }

            newAereoRetorno.CompanhiaMatrix = aereoRetorno.CompanhiaMatrix;

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");

            if (Request.Form["hdnIsMontePacote"] != null && Request.Form["hdnIsMontePacote"] == "1")
            {
                ViewBag.IsMontePacote = true;
            }

            newAereoRetorno.Tarifas = aereoRetorno.Tarifas;

            return PartialView("AereoResultado", newAereoRetorno);
        }

        public ActionResult FiltrarAereoPacote(AereoModel modelParametro)
        {
            var newAereoRetorno = new AereoModel();
            var aereoRetorno = Session["AereoLista"] as AereoModel;
            ConvertMethod.EntityToEntity<AereoModel, AereoModel>(aereoRetorno, ref newAereoRetorno);

            if (aereoRetorno != null) newAereoRetorno.GrupoTarifas = aereoRetorno.GrupoTarifas;

            var combo = Request.Form["hdnCombo"];
            if (combo != "4")
            {
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.OrderBy(p => p.Valor).ToList();
                newAereoRetorno.Ordenar = "Menor";
            }
            else
            {
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.OrderByDescending(p => p.Valor).ToList();
                newAereoRetorno.Ordenar = "Maior";
            }

            var valor = Request.Form["hdnValorSelecionado"];
            if (!string.IsNullOrEmpty(valor))
            {
                var valorConv = Conv.ToDecimal(valor);
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.AsParallel().Where(p => p.Valor - 1 <= valorConv && p.Valor + 1 >= valorConv).ToList();
            }
            var amount = Request.Form["amount"];
            if (amount != null)
            {
                var valorMenor = Conv.ToDecimal(amount.Split('-')[0].Replace("R$", "").Replace(" ", ""));
                var valorMaior = Conv.ToDecimal(amount.Split('-')[1].Replace("R$", "").Replace(" ", ""));
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.AsParallel().Where(p => p.Valor >= valorMenor - 1 && p.Valor <= valorMaior + 1).ToList();
            }

            foreach (var grupo in newAereoRetorno.GrupoTarifas)
            {
                var i = 1;
                foreach (var tarifa in grupo.GrupoTarifaTrechos)
                {
                    foreach (var codigo in tarifa.TrechosTarifaCodigo)
                    {
                        codigo.Visible = true;

                        var tipoFiltro = Request.Form["hdnTipoFiltro"];

                        if ((i == 1) || (tipoFiltro == "MultiTrecho"))
                        {
                            var listaKeys = (from object item in Request.Form.Keys select item.ToString()).AsParallel().ToList();

                            if (listaKeys.All(p => p != ("AeroportoPartida-" + codigo.Trecho.SiglaAeroportoOrigem.Trim())))
                                codigo.Visible = false;

                            var horarioIdaMadrugada = Request.Form["HorarioIdaMadrugada"];

                            if (horarioIdaMadrugada == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 0 && codigo.Trecho.DataOrigem.Hour < 6))
                                    codigo.Visible = false;
                            }

                            var horarioIdaManha = Request.Form["HorarioIdaManha"];

                            if (horarioIdaManha == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour > 5 && codigo.Trecho.DataOrigem.Hour <= 11))
                                    codigo.Visible = false;
                            }

                            var horarioIdaTarde = Request.Form["HorarioIdaTarde"];

                            if (horarioIdaTarde == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 12 && codigo.Trecho.DataOrigem.Hour < 18))
                                    codigo.Visible = false;
                            }

                            var horarioIdaNoite = Request.Form["HorarioIdaNoite"];

                            if (horarioIdaNoite == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 18 && codigo.Trecho.DataOrigem.Hour <= 23))
                                    codigo.Visible = false;
                            }
                        }
                        else if (grupo.GrupoTarifaTrechos.Count > 1)
                        {
                            var listaKeys = (from object item in Request.Form.Keys select item.ToString()).AsParallel().ToList();


                            if (listaKeys.All(p => p != ("AeroportoDestino-" + codigo.Trecho.SiglaAeroportoOrigem.Trim())))
                                codigo.Visible = false;

                            var horarioIdaMadrugada = Request.Form["HorarioVoltaMadrugada"];
                            if (horarioIdaMadrugada == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 0 && codigo.Trecho.DataOrigem.Hour < 6))
                                    codigo.Visible = false;
                            }

                            var horarioIdaManha = Request.Form["HorarioVoltaManha"];
                            if (horarioIdaManha == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour > 5 && codigo.Trecho.DataOrigem.Hour <= 11))
                                    codigo.Visible = false;
                            }

                            var horarioIdaTarde = Request.Form["HorarioVoltaTarde"];
                            if (horarioIdaTarde == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 12 && codigo.Trecho.DataOrigem.Hour < 18))
                                    codigo.Visible = false;
                            }

                            var horarioIdaNoite = Request.Form["HorarioVoltaNoite"];
                            if (horarioIdaNoite == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 18 && codigo.Trecho.DataOrigem.Hour <= 23))
                                    codigo.Visible = false;
                            }
                        }

                    }
                    i++;
                }
            }

            newAereoRetorno.CompanhiaMatrix = aereoRetorno.CompanhiaMatrix;

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");

            if (Request.Form["hdnIsMontePacote"] != null && Request.Form["hdnIsMontePacote"] == "1")
            {
                ViewBag.IsMontePacote = true;
            }

            newAereoRetorno.Tarifas = aereoRetorno.Tarifas;

            return PartialView("AereoPacoteResultado", newAereoRetorno);
        }

        public ActionResult FiltrarAereoMP()
        {
            NameObjectCollectionBase.KeysCollection Keys = (NameObjectCollectionBase.KeysCollection)Session["KeysMP"];
            FiltroAereoModel filtro = new FiltroAereoModel();
            AereoModel modelParametro = new AereoModel();

            modelParametro = (AereoModel)Session["ModelParametro"];
            filtro = (FiltroAereoModel)Session["FiltroMP"];

            AereoModel newAereoRetorno = new AereoModel();
            AereoModel AereoRetorno = Session["AereoLista"] as AereoModel;
            ConvertMethod.EntityToEntity<AereoModel, AereoModel>(AereoRetorno, ref newAereoRetorno);
            newAereoRetorno.GrupoTarifas = AereoRetorno.GrupoTarifas;
            var combo = filtro.combo;
            if (combo != "4")
            {
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.OrderBy(p => p.Valor).ToList();
                newAereoRetorno.Ordenar = "Menor";
            }
            else
            {
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.OrderByDescending(p => p.Valor).ToList();
                newAereoRetorno.Ordenar = "Maior";
            }

            var valor = filtro.valor;
            if (valor != null && valor != "")
            {
                var valorConv = Conv.ToDecimal(valor);
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.AsParallel().Where(p => p.Valor - 1 <= valorConv && p.Valor + 1 >= valorConv).ToList();
            }
            var amount = filtro.amount;
            if (amount != null)
            {
                var valorMenor = Conv.ToDecimal(amount.Split('-')[0].Replace("R$", "").Replace(" ", ""));
                var valorMaior = Conv.ToDecimal(amount.Split('-')[1].Replace("R$", "").Replace(" ", ""));
                newAereoRetorno.GrupoTarifas = newAereoRetorno.GrupoTarifas.AsParallel().Where(p => p.Valor >= valorMenor - 1 && p.Valor <= valorMaior + 1).ToList();
            }

            foreach (var grupo in newAereoRetorno.GrupoTarifas)
            {
                var i = 1;
                foreach (var tarifa in grupo.GrupoTarifaTrechos)
                {
                    foreach (var codigo in tarifa.TrechosTarifaCodigo)
                    {
                        codigo.Visible = true;

                        var tipoFiltro = filtro.tipoFiltro;

                        if ((i == 1) || (tipoFiltro == "MultiTrecho"))
                        {
                            List<String> listaKeys = new List<string>();
                            foreach (var item in Keys)
                            {
                                listaKeys.Add(item.ToString());
                            }
                            if (!listaKeys.Any(p => p == ("Companhias-" + codigo.Trecho.NomeCia.Trim())))
                                codigo.Visible = false;

                            if (!listaKeys.Any(p => p == ("AeroportoPartida-" + codigo.Trecho.SiglaAeroportoOrigem.Trim())))
                                codigo.Visible = false;

                            var HorarioIdaMadrugada = filtro.HorarioIdaMadrugada;
                            if (HorarioIdaMadrugada == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 0 && codigo.Trecho.DataOrigem.Hour < 6))
                                    codigo.Visible = false;
                            }

                            var EscalaVooDireto = filtro.EscalaVooDireto;
                            if (EscalaVooDireto == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count == 1)
                                    codigo.Visible = false;
                            }
                            var EscalaUmaParada = filtro.EscalaUmaParada;
                            if (EscalaUmaParada == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 1)
                                    codigo.Visible = false;
                            }
                            var EscalaDuasParadas = filtro.EscalaDuasParadas;
                            if (EscalaDuasParadas == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 2)
                                    codigo.Visible = false;
                            }

                            var HorarioIdaManha = filtro.HorarioIdaManha;
                            if (HorarioIdaManha == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour > 5 && codigo.Trecho.DataOrigem.Hour <= 11))
                                    codigo.Visible = false;
                            }

                            var HorarioIdaTarde = filtro.HorarioIdaTarde;
                            if (HorarioIdaTarde == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 12 && codigo.Trecho.DataOrigem.Hour < 18))
                                    codigo.Visible = false;
                            }

                            var HorarioIdaNoite = filtro.HorarioIdaNoite;
                            if (HorarioIdaNoite == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 18 && codigo.Trecho.DataOrigem.Hour <= 23))
                                    codigo.Visible = false;
                            }
                        }
                        else if (grupo.GrupoTarifaTrechos.Count > 1)
                        {
                            List<String> listaKeys = new List<string>();
                            foreach (var item in Keys)
                            {
                                listaKeys.Add(item.ToString());
                            }
                            if (!listaKeys.Any(p => p == ("Companhias-" + codigo.Trecho.NomeCia.Trim())))
                                codigo.Visible = false;

                            if (!listaKeys.Any(p => p == ("AeroportoDestino-" + codigo.Trecho.SiglaAeroportoOrigem.Trim())))
                                codigo.Visible = false;

                            var HorarioIdaMadrugada = filtro.HorarioIdaMadrugada;
                            if (HorarioIdaMadrugada == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 0 && codigo.Trecho.DataOrigem.Hour < 6))
                                    codigo.Visible = false;
                            }

                            var HorarioIdaManha = filtro.HorarioIdaManha;
                            if (HorarioIdaManha == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour > 5 && codigo.Trecho.DataOrigem.Hour <= 11))
                                    codigo.Visible = false;
                            }

                            var HorarioIdaTarde = filtro.HorarioIdaTarde;
                            if (HorarioIdaTarde == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 12 && codigo.Trecho.DataOrigem.Hour < 18))
                                    codigo.Visible = false;
                            }

                            var HorarioIdaNoite = filtro.HorarioIdaNoite;
                            if (HorarioIdaNoite == null)
                            {
                                if ((codigo.Trecho.DataOrigem.Hour >= 18 && codigo.Trecho.DataOrigem.Hour <= 23))
                                    codigo.Visible = false;
                            }

                            var EscalaVooDireto = filtro.EscalaVooDireto;
                            if (EscalaVooDireto == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count == 1)
                                    codigo.Visible = false;
                            }
                            var EscalaUmaParada = filtro.EscalaUmaParada;
                            if (EscalaUmaParada == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 1)
                                    codigo.Visible = false;
                            }
                            var EscalaDuasParadas = filtro.EscalaDuasParadas;
                            if (EscalaDuasParadas == null)
                            {
                                if (codigo.Trecho.Seguimentos.Count > 2)
                                    codigo.Visible = false;
                            }
                        }

                    }
                    i++;
                }
            }

            newAereoRetorno.CompanhiaMatrix = AereoRetorno.CompanhiaMatrix;

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            newAereoRetorno.Tarifas = AereoRetorno.Tarifas;

            ViewBag.IsMontePacote = true;


            Session["FiltroMP"] = null;
            Session["KeysMP"] = null;
            Session["ModelParametro"] = null;
            return PartialView("AereoResultado", newAereoRetorno);
        }

        [HttpPost]
        public ActionResult LoadAereo(AereoModel modelParametro, string horarioIdaSelect, string horarioVoltaSelect, string classeSelect, string somenteDireto, string hdnDestinoCodigoOld, string hdnOrigemCodigoOld, string hdnDestinoNomeOld, string hdnOrigemNomeOld, string hdnDestinoAereoCodigo, string hdnOrigemAereoCodigo, string hdnDestinoAereoNome, string hdnOrigemAereoNome, string ddlAdultos, string ddlCriancas, string ddlBebes)
        {
            List<FT.Web.Model.Models.Destino> ParOrigemDestinoDatas = new List<FT.Web.Model.Models.Destino>();

            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });

            if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0
                && addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros).Count() > 0
                && (addedFlytour.Aereo.Where(p => !p.Comprar).Count() > 0
                    || addedFlytour.Pacote.Where(p => !p.Comprar).Count() > 0
                    || addedFlytour.Hotel.Where(p => !p.Comprar).Count() > 0
                    || addedFlytour.Carro.Where(p => !p.Comprar).Count() > 0
                    || addedFlytour.Atividade.Where(p => !p.Comprar).Count() > 0))
            {
                modelParametro.QuantidadeAdulto = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).Count();
                modelParametro.QuantidadeCrianca = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();
                modelParametro.QuantidadeBebe = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count();
                modelParametro.BloquearPassageiros = true;
            }

            if (!string.IsNullOrEmpty(horarioIdaSelect))
            {
                modelParametro.HorarioIda = horarioIdaSelect;
            }

            if (!string.IsNullOrEmpty(horarioVoltaSelect))
            {
                modelParametro.HorarioVolta = horarioVoltaSelect;
            }

            if (!string.IsNullOrEmpty(classeSelect))
            {
                if (classeSelect == "Econômica")
                    modelParametro.Classe = 2;
                if (classeSelect == "Executiva")
                    modelParametro.Classe = 1;
                if (classeSelect == "Primeira classe")
                    modelParametro.Classe = 0;
            }
            else
                modelParametro.Classe = null;

            if (!string.IsNullOrEmpty(somenteDireto))
            {
                modelParametro.SomenteDireto = true;
            }


            if (String.IsNullOrEmpty(hdnOrigemAereoCodigo))
                modelParametro.Origem = hdnOrigemCodigoOld;
            else
                modelParametro.Origem = hdnOrigemAereoCodigo;

            if (String.IsNullOrEmpty(hdnDestinoAereoCodigo))
                modelParametro.Destino = hdnDestinoCodigoOld;
            else
                modelParametro.Destino = hdnDestinoAereoCodigo;

            if (String.IsNullOrEmpty(hdnOrigemAereoNome))
                modelParametro.OrigemNome = hdnOrigemNomeOld;
            else
                modelParametro.OrigemNome = hdnOrigemAereoNome;

            if (String.IsNullOrEmpty(hdnDestinoAereoNome))
                modelParametro.DestinoNome = hdnDestinoNomeOld;
            else
                modelParametro.DestinoNome = hdnDestinoAereoNome;

            FT.Web.Model.Models.Destino destination = new FT.Web.Model.Models.Destino();
            destination.AeroportoOrigem = modelParametro.Origem;
            destination.AeroportoDestino = modelParametro.Destino;
            destination.DataInicial = modelParametro.DataEmbarque;
            destination.DataFinal = modelParametro.DataRetorno;

            modelParametro.Destinos.Add(destination);

            if (modelParametro.Tipo == "MultiTrecho")
            {
                List<string> keys = new List<string>();
                foreach (var item in Request.Form.Keys)
                {
                    if (item.ToString().Contains("Cmp_"))
                        if (item.ToString().Contains("Codigo"))
                            keys.Add(item.ToString());
                }

                foreach (var item in keys)
                {
                    if (item.Contains("hdnOrigemCmp_"))
                    {
                        var destino = keys.FirstOrDefault(p => p == "hdnDestinoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_')));

                        var propriedadeNomeOrigem = "hdnOrigemCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_') - 6) + "Nome";
                        var propriedadeNomeDestino = "hdnDestinoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_') - 6) + "Nome";

                        if (destino != null)
                        {
                            var dataEmbarque = keys.FirstOrDefault(p => p == "DataEmbarqueCodigoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_') - 6));
                            if (dataEmbarque != null)
                            {
                                FT.Web.Model.Models.Destino parOrigemDestinoData = new FT.Web.Model.Models.Destino();
                                var orig = Request.Form[item];
                                var dest = Request.Form[destino];
                                var dat = Request.Form[dataEmbarque];
                                var nomeOrig = HttpUtility.HtmlDecode(Request.Form[propriedadeNomeOrigem]);
                                var nomeDest = HttpUtility.HtmlDecode(Request.Form[propriedadeNomeDestino]);


                                parOrigemDestinoData.AeroportoOrigem = orig;
                                parOrigemDestinoData.AeroportoDestino = dest;
                                parOrigemDestinoData.DataInicial = dat;
                                parOrigemDestinoData.NomeOrigem = nomeOrig;
                                parOrigemDestinoData.NomeDestino = nomeDest;
                                if (!string.IsNullOrEmpty(orig) && !string.IsNullOrEmpty(dest) && !string.IsNullOrEmpty(dat))
                                    ParOrigemDestinoDatas.Add(parOrigemDestinoData);
                            }
                        }
                    }
                }

                modelParametro.Destinos.AddRange(ParOrigemDestinoDatas);
            }

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            modelParametro.SomenteOffline = false;
            AereoModel AereoRetorno = AereoBusiness.Consultar(modelParametro);
            Session["AereoLista"] = AereoRetorno;
            return View(AereoRetorno);
        }

        public ActionResult LoadAereoMontePacote(string horarioIdaSelect, string horarioVoltaSelect, string classeSelect,
            string somenteDireto = null, string hdnDestinoCodigoOld = null, string hdnOrigemCodigoOld = null, string hdnDestinoNomeOld = null,
            string hdnOrigemNomeOld = null, string hdnDestinoCodigo = null, string hdnOrigemCodigo = null, string hdnDestinoNome = null,
            string hdnOrigemNome = null, string ddlAdultos = null, string ddlCriancas = null, string ddlBebes = null)
        {
            AereoModel modelParametro = new AereoModel();

            modelParametro = MPSession.Aereo.FirstOrDefault();

            List<FT.Web.Model.Models.Destino> ParOrigemDestinoDatas = new List<FT.Web.Model.Models.Destino>();

            if (!string.IsNullOrEmpty(horarioIdaSelect))
            {
                modelParametro.HorarioIda = horarioIdaSelect;
            }

            if (!string.IsNullOrEmpty(horarioVoltaSelect))
            {
                modelParametro.HorarioVolta = horarioVoltaSelect;
            }

            if (!string.IsNullOrEmpty(classeSelect))
            {
                if (classeSelect == "Econômica")
                    modelParametro.Classe = 2;
                if (classeSelect == "Executiva")
                    modelParametro.Classe = 1;
                if (classeSelect == "Primeira classe")
                    modelParametro.Classe = 0;
            }
            else
                modelParametro.Classe = null;

            if (!string.IsNullOrEmpty(somenteDireto))
            {
                modelParametro.SomenteDireto = true;
            }

            if (String.IsNullOrEmpty(hdnOrigemCodigo))
                modelParametro.Origem = hdnOrigemCodigoOld;
            else
                modelParametro.Origem = hdnOrigemCodigo;

            if (String.IsNullOrEmpty(hdnDestinoCodigo))
                modelParametro.Destino = hdnDestinoCodigoOld;
            else
                modelParametro.Destino = hdnDestinoCodigo;

            if (String.IsNullOrEmpty(hdnOrigemNome))
                modelParametro.OrigemNome = hdnOrigemNomeOld;
            else
                modelParametro.OrigemNome = hdnOrigemNome;

            if (String.IsNullOrEmpty(hdnDestinoNome))
                modelParametro.DestinoNome = hdnDestinoNomeOld;
            else
                modelParametro.DestinoNome = hdnDestinoNome;

            FT.Web.Model.Models.Destino destination = new FT.Web.Model.Models.Destino();
            destination.AeroportoOrigem = modelParametro.Origem;
            destination.AeroportoDestino = modelParametro.Destino;
            destination.DataInicial = modelParametro.DataEmbarque;
            destination.DataFinal = modelParametro.DataRetorno;

            var destinoJaAdicionado = modelParametro.Destinos.AsParallel().Where(d => d.AeroportoOrigem == modelParametro.Origem
                && d.AeroportoDestino == modelParametro.Destino && d.DataInicial == modelParametro.DataEmbarque
                && d.DataFinal == modelParametro.DataRetorno).FirstOrDefault();

            if (destinoJaAdicionado == null)
            {
                modelParametro.Destinos.Add(destination);
            }
            else
            {
                modelParametro.Destinos.Remove(destinoJaAdicionado);
                modelParametro.Destinos.Add(destination);
            }


            if (modelParametro.Tipo == "MultiTrecho")
            {
                List<string> keys = new List<string>();
                foreach (var item in Request.Form.Keys)
                {
                    if (item.ToString().Contains("Cmp_"))
                        if (item.ToString().Contains("Codigo"))
                            keys.Add(item.ToString());
                }

                foreach (var item in keys)
                {
                    if (item.Contains("hdnOrigemCmp_"))
                    {
                        var destino = keys.FirstOrDefault(p => p == "hdnDestinoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_')));
                        if (destino != null)
                        {
                            var dataEmbarque = keys.FirstOrDefault(p => p == "DataEmbarqueCodigoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_') - 6));
                            if (dataEmbarque != null)
                            {
                                FT.Web.Model.Models.Destino parOrigemDestinoData = new FT.Web.Model.Models.Destino();
                                var orig = Request.Form[item];
                                var dest = Request.Form[destino];
                                var dat = Request.Form[dataEmbarque];
                                parOrigemDestinoData.AeroportoOrigem = orig;
                                parOrigemDestinoData.AeroportoDestino = dest;
                                parOrigemDestinoData.DataInicial = dat;
                                if (!string.IsNullOrEmpty(orig) && !string.IsNullOrEmpty(dest) && !string.IsNullOrEmpty(dat))
                                    ParOrigemDestinoDatas.Add(parOrigemDestinoData);
                            }
                        }
                    }
                }

                modelParametro.Destinos.AddRange(ParOrigemDestinoDatas);
            }

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            modelParametro.QuantidadeAdulto = String.IsNullOrEmpty(ddlAdultos) ? 0 : Convert.ToInt32(ddlAdultos);
            modelParametro.QuantidadeCrianca = String.IsNullOrEmpty(ddlCriancas) ? 0 : Convert.ToInt32(ddlCriancas);
            modelParametro.SomenteOffline = false;
            AereoModel AereoRetorno = AereoBusiness.Consultar(modelParametro);
            Session["AereoLista"] = AereoRetorno;

            ViewBag.IsMontePacote = true;

            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey + "Aereo", AereoRetorno, TimeSpan.FromMinutes(90));

            return PartialView("LoadAereo", AereoRetorno);
        }

        public ActionResult LoadAereoMultiTrechoMontePacote(string horarioIdaSelect, string horarioVoltaSelect, string classeSelect,
            string somenteDireto = null, string hdnDestinoCodigoOld = null, string hdnOrigemCodigoOld = null, string hdnDestinoNomeOld = null,
            string hdnOrigemNomeOld = null, string hdnDestinoCodigo = null, string hdnOrigemCodigo = null, string hdnDestinoNome = null,
            string hdnOrigemNome = null, string ddlAdultos = null, string ddlCriancas = null, string ddlBebes = null)
        {
            AereoModel modelParametro = new AereoModel();

            modelParametro = MPSession.Aereo.LastOrDefault();

            modelParametro.DataEmbarque = MPSession.OpcoesAereo.First().Saida;

            if (modelParametro.Tipo != "IdaeVolta")
            {
                modelParametro.Tipo = "MultiTrecho";
            }

            if (!string.IsNullOrEmpty(horarioIdaSelect))
            {
                modelParametro.HorarioIda = horarioIdaSelect;
            }

            if (!string.IsNullOrEmpty(horarioVoltaSelect))
            {
                modelParametro.HorarioVolta = horarioVoltaSelect;
            }

            if (!string.IsNullOrEmpty(classeSelect))
            {
                if (classeSelect == "Econômica")
                    modelParametro.Classe = 2;
                if (classeSelect == "Executiva")
                    modelParametro.Classe = 1;
                if (classeSelect == "Primeira classe")
                    modelParametro.Classe = 0;
            }
            else
                modelParametro.Classe = null;

            if (!string.IsNullOrEmpty(somenteDireto))
                modelParametro.SomenteDireto = true;

            if (string.IsNullOrEmpty(hdnOrigemCodigo))
                modelParametro.Origem = hdnOrigemCodigoOld;
            else
                modelParametro.Origem = hdnOrigemCodigo;

            if (string.IsNullOrEmpty(hdnDestinoCodigo))
                modelParametro.Destino = hdnDestinoCodigoOld;
            else
                modelParametro.Destino = hdnDestinoCodigo;

            if (string.IsNullOrEmpty(hdnOrigemNome))
                modelParametro.OrigemNome = hdnOrigemNomeOld;
            else
                modelParametro.OrigemNome = hdnOrigemNome;

            if (string.IsNullOrEmpty(hdnDestinoNome))
                modelParametro.DestinoNome = hdnDestinoNomeOld;
            else
                modelParametro.DestinoNome = hdnDestinoNome;

            foreach (var item in MPSession.OpcoesAereo.AsParallel().Where(o => o.IsSelected).ToList())
            {
                decimal codigoCidadeEmbarque = 0;
                decimal codigoCidadeDesembarque = 0;
                List<LocaisRetorno> lstCidadeEmbarque;
                List<LocaisRetorno> lstCidadeDesembarque;
                codigoCidadeEmbarque = Conv.ToDecimal(item.CodigoCidadeOrigem);
                codigoCidadeDesembarque = Conv.ToDecimal(item.CodigoCidadeDestino);

                var listaAeroportosOrigem = AereoBusiness.ConsultaOrigem(true);
                var listaAeroportosDestino = AereoBusiness.ConsultaDestino();
                lstCidadeEmbarque = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeEmbarque)).ToList();
                lstCidadeDesembarque = listaAeroportosDestino.Where(p => p.CodigoCidade == (codigoCidadeDesembarque)).ToList();

                LocaisRetorno ultimaCidadeEmbarque = lstCidadeEmbarque.LastOrDefault();
                LocaisRetorno ultimaCidadeDesembarque = lstCidadeDesembarque.LastOrDefault();

                FT.Web.Model.Models.Destino destination = new Model.Models.Destino();
                destination.AeroportoOrigem = ultimaCidadeEmbarque.SiglaAeroporto;
                destination.AeroportoDestino = ultimaCidadeDesembarque.SiglaAeroporto;
                destination.NomeOrigem = ultimaCidadeEmbarque.NomeAeroporto;
                destination.NomeDestino = ultimaCidadeDesembarque.NomeAeroporto;
                destination.DataInicial = item.Saida;

                if (modelParametro.Tipo == "IdaeVolta")
                {
                    destination.DataFinal = item.Retorno;
                }

                var destinoJaAdicionado = modelParametro.Destinos.AsParallel().Where(d => d.AeroportoOrigem == modelParametro.Origem
                        && d.AeroportoDestino == modelParametro.Destino && d.DataInicial == modelParametro.DataEmbarque
                        && d.DataFinal == modelParametro.DataRetorno).FirstOrDefault();

                if (destinoJaAdicionado == null)
                {
                    modelParametro.Destinos.Add(destination);
                }
                else
                {
                    modelParametro.Destinos.Remove(destinoJaAdicionado);
                    modelParametro.Destinos.Add(destination);
                }
            }

            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            modelParametro.QuantidadeAdulto = String.IsNullOrEmpty(ddlAdultos) ? 0 : Convert.ToInt32(ddlAdultos);
            modelParametro.QuantidadeCrianca = String.IsNullOrEmpty(ddlCriancas) ? 0 : Convert.ToInt32(ddlCriancas);
            modelParametro.SomenteOffline = false;
            AereoModel aereoRetorno = AereoBusiness.Consultar(modelParametro);
            Session["AereoLista"] = aereoRetorno;

            ViewBag.IsMontePacote = true;

            var success = AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey + "Aereo", aereoRetorno, TimeSpan.FromMinutes(90));

            return PartialView("LoadAereo", aereoRetorno);
        }

        public virtual PartialViewResult PassagemDestaque()
        {
            var Passagens = AereoBusiness.BuscarPassagemDestaque();
            foreach (var item in Passagens)
            {
                if ((DateTime)item.DataInicial < DateTime.Now)
                {
                    item.DataInicial = DateTime.Now;
                }
                item.DataFinal = (DateTime)item.DataInicial.Value.AddDays(7);
            }
            return PartialView(Passagens);
        }

        public virtual PartialViewResult Tarifar()
        {
            return PartialView(null);
        }

        public virtual PartialViewResult TesteGrid()
        {
            return PartialView(null);
        }

        public virtual PartialViewResult AereoMenuForm()
        {
            #region List

            var list = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                            "ID", "Name", 1);
            ViewData["Adultos"] = list;

            var list1 = new SelectList(new[]
                                        {
                                            new{ID="0",Name="0"},
                                            new{ID="1",Name="1"},
                                            new{ID="2",Name="2"},
                                            new{ID="3",Name="3"},
                                            new{ID="4",Name="4"},
                                            new{ID="5",Name="5"},
                                            new{ID="6",Name="6"},

                                        },
                "ID", "Name", 0);
            ViewData["Criancas"] = list1;

            ViewData["Bebes"] = list1;

            #endregion

            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });
            AereoModel model = new AereoModel();

            //if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0 && addedFlytour.Apartamentos.SelectMany(p => p.Passageiros).Count() > 0)
            if (BloquearApartamentizacao(addedFlytour))
            {
                model.QuantidadeAdulto = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).Count();
                model.QuantidadeCrianca = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();
                model.QuantidadeBebe = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count();
                model.BloquearPassageiros = true;
            }
            else
            {
                model.QuantidadeAdulto = 1;
                model.QuantidadeCrianca = 0;
                model.QuantidadeBebe = 0;
                model.BloquearPassageiros = false;
            }
            return PartialView(model);
        }

        [HttpPost]
        public string FormasdePagamento(AereoModel AereoModel, bool MontePacote = false, decimal listaTarifas = 0)
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
            AereoModel.ValorCusto = listaTarifas;

            AereoModel.Trechos.Clear();
            foreach (var item in trechosAer)
            {
                AereoModel.Trechos.Add(item);
            }

            var apartamentos = new List<ApartamentoModel>();
            var apto = new ApartamentoModel();

            var flyTour = new FlyTourModel();

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

                flyTour.Apartamentos.Add(apto);
            }
            else
            {
                flyTour.Apartamentos = AddflyTour.Apartamentos;
            }
            flyTour.Aereo.Add(AereoModel);

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




        [HttpPost, ValidateInput(false)]
        public ActionResult LoadAereoPacotes(AereoModel modelParametro, string horarioIdaSelect, string horarioVoltaSelect, string classeSelect, string somenteDireto, string hdnDestinoCodigoOld, string hdnOrigemCodigoOld, string hdnDestinoNomeOld, string hdnOrigemNomeOld, string hdnDestinoAereoCodigo, string hdnOrigemAereoCodigo, string hdnDestinoAereoNome, string hdnOrigemAereoNome, string ddlAdultos, string ddlCriancas, string ddlBebes, string hdnDestinoAereoCodigoCidade)
        {
            List<FT.Web.Model.Models.Destino> ParOrigemDestinoDatas = new List<FT.Web.Model.Models.Destino>();
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

            var addedFlytour = AppCache.Get<FlyTourModel>(EnumProcesso.PacoteMatriz, SessionKey, 90, () =>
            {
                return new FlyTourModel();
            });


            //if (addedFlytour.Apartamentos != null && addedFlytour.Apartamentos.Count > 0 && addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros).Count() > 0)
            //{
            //    modelParametro.QuantidadeAdulto = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 11)).Count();
            //    modelParametro.QuantidadeCrianca = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro > 1 && v.IdadePassageiro < 12)).Count();
            //    modelParametro.QuantidadeBebe = addedFlytour.Apartamentos.AsParallel().SelectMany(p => p.Passageiros.Where(v => v.IdadePassageiro < 2)).Count();
            //    modelParametro.BloquearPassageiros = true;
            //}

            if (!string.IsNullOrEmpty(horarioIdaSelect))
            {
                modelParametro.HorarioIda = horarioIdaSelect;
            }

            if (!string.IsNullOrEmpty(horarioVoltaSelect))
            {
                modelParametro.HorarioVolta = horarioVoltaSelect;
            }

            if (!string.IsNullOrEmpty(classeSelect))
            {
                if (classeSelect == "Econômica")
                    modelParametro.Classe = 2;
                if (classeSelect == "Executiva")
                    modelParametro.Classe = 1;
                if (classeSelect == "Primeira classe")
                    modelParametro.Classe = 0;
            }
            else
                modelParametro.Classe = null;

            if (!string.IsNullOrEmpty(somenteDireto))
            {
                modelParametro.SomenteDireto = true;
            }


            //if (String.IsNullOrEmpty(hdnOrigemAereoCodigo))
            //    modelParametro.Origem = hdnOrigemCodigoOld;
            //else
            modelParametro.Origem = hdnOrigemAereoCodigo;

            //if (String.IsNullOrEmpty(hdnDestinoAereoCodigo))
            //    modelParametro.Destino = hdnDestinoCodigoOld;
            //else
            modelParametro.Destino = hdnDestinoAereoCodigo;

            //if (String.IsNullOrEmpty(hdnOrigemAereoNome))
            //    modelParametro.OrigemNome = hdnOrigemNomeOld;
            //else
            modelParametro.OrigemNome = hdnOrigemAereoNome;

            //if (String.IsNullOrEmpty(hdnDestinoAereoNome))
            //    modelParametro.DestinoNome = hdnDestinoNomeOld;
            //else
            modelParametro.DestinoNome = hdnDestinoAereoNome;

            FT.Web.Model.Models.Destino destination = new FT.Web.Model.Models.Destino();
            destination.AeroportoOrigem = modelParametro.Origem;
            destination.AeroportoDestino = modelParametro.Destino;
            destination.DataInicial = modelParametro.DataEmbarque;
            destination.DataFinal = modelParametro.DataRetorno;

            modelParametro.Destinos.Add(destination);

            if (modelParametro.Tipo == "MultiTrecho")
            {
                List<string> keys = new List<string>();
                foreach (var item in Request.Form.Keys)
                {
                    if (item.ToString().Contains("Cmp_"))
                        if (item.ToString().Contains("Codigo"))
                            keys.Add(item.ToString());
                }

                foreach (var item in keys)
                {
                    if (item.Contains("hdnOrigemCmp_"))
                    {
                        var destino = keys.FirstOrDefault(p => p == "hdnDestinoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_')));
                        if (destino != null)
                        {
                            var dataEmbarque = keys.FirstOrDefault(p => p == "DataEmbarqueCodigoCmp" + item.Substring(item.IndexOf('_'), item.Length - item.IndexOf('_') - 6));
                            if (dataEmbarque != null)
                            {
                                FT.Web.Model.Models.Destino parOrigemDestinoData = new FT.Web.Model.Models.Destino();
                                var orig = Request.Form[item];
                                var dest = Request.Form[destino];
                                var dat = Request.Form[dataEmbarque];
                                parOrigemDestinoData.AeroportoOrigem = orig;
                                parOrigemDestinoData.AeroportoDestino = dest;
                                parOrigemDestinoData.DataInicial = dat;
                                if (!string.IsNullOrEmpty(orig) && !string.IsNullOrEmpty(dest) && !string.IsNullOrEmpty(dat))
                                    ParOrigemDestinoDatas.Add(parOrigemDestinoData);
                            }
                        }
                    }
                }

                modelParametro.Destinos.AddRange(ParOrigemDestinoDatas);
            }


            var data = Conv.ToDateTime(modelParametro.DataEmbarque);

            modelParametro.DataEmbarqueExtenso = data.Day + " de " + retornaNomeMes(data.Month) + " de " + data.Year;
            modelParametro.DataEmbarqueExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");
            data = Conv.ToDateTime(modelParametro.DataRetorno);
            modelParametro.DataRetornoExtensoAbrev = data.ToString("dddd").Substring(0, 3).ToUpper() + ", " + data.ToString("dd/MM/yyyy");

            modelParametro.SomenteOffline = true;
            modelParametro.QuantidadeAdulto = Adultos;
            modelParametro.QuantidadeCrianca = Criancas;
            modelParametro.QuantidadeBebe = Bebes;

            AereoModel AereoRetorno = AereoBusiness.Consultar(modelParametro);
            Session["AereoLista"] = AereoRetorno;

            AereoRetorno.Origem = hdnOrigemAereoCodigo;
            AereoRetorno.Destino = hdnDestinoAereoCodigo;
            if (String.IsNullOrEmpty(hdnOrigemAereoNome))
                AereoRetorno.OrigemNome = hdnOrigemNomeOld;
            else
                AereoRetorno.OrigemNome = hdnOrigemAereoNome;

            AereoRetorno.DestinoNome = hdnDestinoAereoNome;
            AereoRetorno.CodigoCidadeDestino = hdnDestinoAereoCodigoCidade;
            AereoRetorno.Apartamentos = ApartamentoList;

            return View(AereoRetorno);
        }

        [HttpGet]
        public ActionResult LoadAereoPacotes(AereoModel modelParametro, string horarioIdaSelect, string horarioVoltaSelect, string classeSelect, string somenteDireto, string hdnDestinoCodigoOld, string hdnOrigemCodigoOld, string hdnDestinoNomeOld, string hdnOrigemNomeOld, string hdnDestinoAereoCodigo, string hdnOrigemAereoCodigo, string hdnDestinoAereoNome, string hdnOrigemAereoNome, string ddlAdultos, string ddlCriancas, string ddlBebes, string a = null, string b = null)
        {
            AereoModel AereoRetorno = new AereoModel();
            AereoRetorno.QuantidadeAdulto = 2;
            return View(AereoRetorno);
        }


        public ActionResult ConsultarAereoPacoteMatriz(string trechos)
        {
            var trecho1 = trechos.Split(';')[0];
            var trecho2 = trechos.Split(';')[1];
            var parametro = new ConsultaPacoteMatrizParametro();
            parametro.CodigoEmpresa = "1";
            parametro.CodigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();
            parametro.CodigoTarifaVigenciaCustoIda = trecho1;
            parametro.CodigoTarifaVigenciaCustoVolta = trecho2;
            var retorno = AereoBusiness.ConsultarAereoPacoteMatriz(parametro);
            retorno.trecho1 = trecho1;
            retorno.trecho2 = trecho2;

            if (retorno != null && retorno.ListaAereoPacoteMatriz != null && retorno.ListaAereoPacoteMatriz.Count > 0)
                retorno.ListaAereoPacoteMatriz = retorno.ListaAereoPacoteMatriz.OrderBy(p => p.ValorReferencia).ToList();

            if (retorno != null && retorno.ListaAereoPacoteMatriz != null && retorno.ListaAereoPacoteMatriz.Count > 0)
            {
                foreach (var pacote in retorno.ListaAereoPacoteMatriz)
                {
                    if (pacote.PacoteDescricao != null && pacote.PacoteDescricao != null && pacote.PacoteDescricao.DescricaoConteudo.Count > 0)
                    {
                        Session["PacoteDescricaoServico" + pacote.CodigoPacoteMatriz] = pacote.PacoteDescricao;
                    }
                    else
                    {
                        Session["PacoteDescricaoServico" + pacote.CodigoPacoteMatriz] = null;
                    }
                }

            }

            return PartialView("AereoPacoteMatriz", retorno);
        }

        public ActionResult ConsultarAereoPacoteVenda(string trechos, string codigoMatriz)
        {
            //JavaScriptSerializer json = new JavaScriptSerializer();
            //var produtos1 = (List<ApartamentoModel>)json.Deserialize(apartamentos, typeof(List<ApartamentoModel>));

            var trecho1 = trechos.Split(';')[0];
            var trecho2 = trechos.Split(';')[1];
            var parametro = new ConsultaPacoteParametro();
            parametro.CodigoEmpresa = "1";
            parametro.CodigoAgencia = SessionUsuario.ID_PESSOA_AGENCIA.ToString();
            parametro.CodigoMatriz = codigoMatriz;
            parametro.CodigoTarifaVigenciaCustoIda = trecho1;
            parametro.CodigoTarifaVigenciaCustoVolta = trecho2;
            //parametro.QuantidadeAdulto = qtdAdulto;
            //parametro.QuantidadeCrianca = qtdCrianca;
            //parametro.QuantidadeBebe = qtdBebe;
            var retorno = AereoBusiness.ConsultarAereoPacote(parametro);

            if (retorno != null && retorno.ListaAereoPacote != null && retorno.ListaAereoPacote.Count > 0)
            {
                retorno.ListaAereoPacote = retorno.ListaAereoPacote.OrderBy(p => p.ValorReferencia).ToList();

                if (retorno != null && retorno.ListaAereoPacote != null && retorno.ListaAereoPacote.Count > 0)
                {
                    var conteudo = Session["PacoteDescricaoServico" + codigoMatriz] as PacoteDescricao;

                    foreach (var pacote in retorno.ListaAereoPacote)
                    {
                        if (conteudo != null)
                        {
                            Session["PacoteDescricaoServico" + pacote.CodigoPacote] = conteudo;
                        }
                        else
                        {
                            Session["PacoteDescricaoServico" + pacote.CodigoPacote] = null;
                        }
                    }
                }
            }

            return PartialView("AereoPacoteVenda", retorno);
        }
    }
}

