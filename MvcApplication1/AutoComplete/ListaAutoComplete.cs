using FT.Web.Business.Aereo;
using FT.Web.Business.Carro;
using FT.Web.Business.CarroService;
using FT.Web.Business.Hotel;
using FT.Web.Business.Localizacao;
using FT.Web.Core.Util.Validation;
using FT.Web.Model.Models;
using FTV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FT.Web.Site.AutoComplete
{
    public class ListaAutoComplete
    {

        /// <summary>
        /// Enum com os tipos de listas usadas no AutoComplete. Informação utilizada na função autoComplete do javascript
        /// </summary>
        public enum TipoLista
        {
            Municipios = 1,
            Nomes = 2,
            Estado = 3,
            AereoOrigem = 4,
            AereoOrigemNacional = 10,
            AereoDestino = 5,
            HotelDestino = 6,
            Pais = 7,
            Cidade = 8,
            Carro = 9
        }

        string _searchText;

        public ListaAutoComplete(string searchtext)
        {
            _searchText = searchtext;
        }

        /// <summary>
        /// Retorna uma lista de DataJason no padrão utilizado pelo autocomplete implementado
        /// </summary>
        /// <param name="tipolista">id da lista. Set/Get Ids no enum na classe ListaAutoComplete</param>
        /// <param name="parameters"> </param>
        /// <returns></returns>
        public List<DataJson> Get(int tipolista, List<string> parameters)
        {
            var dj = new List<DataJson>();
            switch ((TipoLista)tipolista)
            {
                case TipoLista.Municipios:
                    {
                        var lstMuni = new MunicipiosModel().MunicipiosEstados();
                        dj = ConvertToDataJson(lstMuni, "nome", "id", false);
                        break;
                    }
                case TipoLista.Nomes:
                    {
                        break;
                    }
                case TipoLista.Estado:
                    {
                        break;
                    }
                case TipoLista.AereoOrigem:
                    {
                        var lstMuni = AereoBusiness.ConsultaOrigem();
                        dj = ConvertToDataJson(lstMuni, "NomeAeroporto", "SiglaAeroporto", false);
                        break;
                    }
                case TipoLista.AereoOrigemNacional:
                    {
                        var lstMuni = AereoBusiness.ConsultaOrigem(true);
                        dj = ConvertToDataJson(lstMuni, "NomeAeroporto", "SiglaAeroporto", false);
                        break;
                    }
                case TipoLista.AereoDestino:
                    {
                        var lstMuni = AereoBusiness.ConsultaDestino();
                        dj = ConvertToDataJson(lstMuni, "NomeAeroporto", "SiglaAeroporto", false, true);
                        break;
                    }
                case TipoLista.HotelDestino:
                    {
                        var hotelBussiness = new HotelBusiness();
                        var lstHotelLocalidade = hotelBussiness.ConsultarLocalizacaoHotel();
                        //var lista = new  List<HotelLocalidadeModel>();
                        //foreach (var item in lstHotelLocalidade)
                        //{
                        //    var item2 = item.CidadeEstadoPais.Split(',')[0];
                        //    item.CidadeEstadoPais = item2;
                        //    lista.Add(item);
                        //}
                        //_searchText = _searchText.Split(',')[0].Trim();
                        dj = ConvertToDataJson(lstHotelLocalidade, "CidadeEstadoPais", "CodigoCidade", false);
                        break;
                    }
                case TipoLista.Pais:
                    {
                        var lstPais = GetLocal.GetPais(new PaisModel());
                        dj = ConvertToDataJson(lstPais, "NM_PAIS", "ID_PAIS", false);
                        break;
                    }
                case TipoLista.Cidade:
                    {
                        if (parameters.Count > 0)
                        {
                            var lstCidade = GetLocal.GetCidade(new CidadeModel { ID_PAIS = (parameters[0] == "" ? "0" : parameters[0]) });
                            dj = ConvertToDataJson(lstCidade, "NM_CIDADE", "ID_CIDADE", false);
                        }
                        else
                        {
                            var lstCidade = GetLocal.GetCidade(new CidadeModel());
                            dj = ConvertToDataJson(lstCidade, "NM_CIDADE", "ID_CIDADE", false);
                        }
                        break;
                    }
                case TipoLista.Carro:
                    {
                        var carroLocalizacaoFiltro = new CarroLocalizacaoFiltro();

                        var carroBusiness = new CarroBusiness();

                        var lstLocalizacoes = carroBusiness.ConsultarLocalidades(carroLocalizacaoFiltro);
                        dj = ConvertToDataJson(lstLocalizacoes, "NomeCidade", "CodigoCidade", false);

                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            return dj;
        }

        /// <summary>
        /// Método Generico que realiza a conversão do tipo da lista para o tipo DataJason, utilizado pelo AutoComplete
        /// </summary>
        /// <typeparam name="T">Tipo da lista (Entidade/Model que a lista esta usando)</typeparam>
        /// <param name="lista">Lista da Entidade/Model</param>
        /// <param name="name"></param>
        /// <param name="id"></param>
        /// <param name="startsWith"> </param>
        /// <returns></returns>
        private List<DataJson> ConvertToDataJson<T>(IEnumerable<T> lista, string name, string id, bool startsWith, bool getCodigoCidade = false)
        {
            var lstDj = new List<DataJson>();
            var propriedadesEntidade = new List<PropertyInfo>(typeof(T).GetProperties());
            Dictionary<string, PropertyInfo> propriedade;
            if (getCodigoCidade)
            {
                var result = (from x in propriedadesEntidade
                              where x.Name.ToLower().Equals(name.ToLower()) || x.Name.ToLower().Equals(id.ToLower()) || x.Name.ToLower().Equals("codigocidade")
                              select x);
                propriedade = result.ToDictionary(p => p.Name);
            }
            else
            {
                var result = (from x in propriedadesEntidade
                              where x.Name.ToLower().Equals(name.ToLower()) || x.Name.ToLower().Equals(id.ToLower())
                              select x);
                propriedade = result.ToDictionary(p => p.Name);
            }

            _searchText = _searchText.RemoverAcento().ToLower();

            foreach (T item in lista.Where(i => i != null))
            {
                string text = Conv.ToString(propriedade[name].GetValue(item)).Split(',')[0];

                if (!startsWith)
                {
                    if (!text.RemoverAcento().ToLower().Contains(_searchText))
                        continue;
                }
                else
                {
                    if (!text.RemoverAcento().ToLower().StartsWith(_searchText))
                        continue;
                }
                var cod = Conv.ToString(propriedade[id].GetValue(item));

                if (getCodigoCidade)
                {
                    var codigoCidade = propriedade["CodigoCidade"].GetValue(item).ToString();
                    lstDj.Add(new DataJson { codigo = cod, label = propriedade[name].GetValue(item).ToString(), codigoCidade = codigoCidade });
                }
                else
                {
                    lstDj.Add(new DataJson { codigo = cod, label = propriedade[name].GetValue(item).ToString() });
                }
            }

            if (lstDj.Count == 0)
                lstDj.Add(new DataJson { label = "Não encontrado", codigo = "" });

            return lstDj;

            //return (from x in lstDJ where x.label.ToLower().RemoverAcentos().Contains(searchText.ToLower().RemoverAcentos()) select x).ToList();
        }
    }
}