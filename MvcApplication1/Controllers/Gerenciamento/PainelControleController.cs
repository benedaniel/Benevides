using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FT.Web.Business.Gerenciamento;
using FT.Web.Site.Controllers.Base;
using FT.Web.Model.Models;
using FT.Web.Core.Util.Validation;
using FT.Web.Business.Localizacao;
using System.Configuration;
using FT.Web.Core.Util.SiteTracer;

namespace FT.Web.Site.Controllers.Gerenciamento
{
    [Tracer]
    public class PainelControleController : _BaseController
    {
        //
        // GET: /Controle/

        public ActionResult Index()
        {
            SetUrlVoucher();
            return View(SessionUsuario);
        }

        /// <summary>
        /// Retorna @ViewBag.urlVoucher com a url do vaoucher usando o token do usuario logado.
        /// </summary>
        private void SetUrlVoucher()
        {

            string IsUrlVoucherDev = ConfigurationManager.AppSettings["IsUrlVoucherDev"];
            string urlVoucher = "";
            string urlExtrato = "";
            if (!string.IsNullOrEmpty(IsUrlVoucherDev) && (Convert.ToBoolean(IsUrlVoucherDev)))
            {
                urlVoucher = ConfigurationManager.AppSettings["UrlBackoffice"]
                    + "pkg_portal.prc_inicial?p_id_pagina=11&token="
                    + SessionUsuario.Token;

                urlExtrato = ConfigurationManager.AppSettings["UrlBackoffice"]
                    + "pkg_portal.prc_inicial?p_id_pagina=12&token="
                    + SessionUsuario.Token;
            }
            else
            {
                urlVoucher = ConfigurationManager.AppSettings["UrlBackoffice"]
                    + "pkg_portal.prc_inicial?p_id_pagina=11&token="
                    + SessionUsuario.Token;

                urlExtrato = ConfigurationManager.AppSettings["UrlBackoffice"]
                    + "pkg_portal.prc_inicial?p_id_pagina=11&token="
                    + SessionUsuario.Token;
            }
            @ViewBag.urlVoucher = urlVoucher;
            @ViewBag.urlExtrato = urlExtrato;
        }

        [HttpPost]
        public JsonResult AlteraStatusUsuario(PessoaModel usuario)
        {
            string Erro = PainelControle.AlteraStatusUsuario(usuario);

            var lstPessoa = GetUsuariosAgencia();
            if (string.IsNullOrEmpty(Erro))
                Erro = "Status do usuário '" + usuario.NM_PESSOA + "' alterado com sucesso!";

            return Json(Erro);
        }

        public JsonResult GetPermissaoPagina(string url)
        {
            bool acesso = PainelControle.GetPermissaoPagina(SessionUsuario, url);
                        
            return Json(acesso);
        }

        public ActionResult ListaUsuario()
        {
            var lstPessoa = GetUsuariosAgencia();

            return View(lstPessoa);
        }

        public List<PessoaModel> GetUsuariosAgencia()
        {
            List<PessoaModel> lstPessoa = new List<PessoaModel>();
            if (EstaLogado)
                lstPessoa = PainelControle.ListaUsuarioAgencia(SessionUsuario).OrderBy(p => p.NM_PESSOA).ToList();
            return lstPessoa;
        }

        private void VisaoComboPerfil(PessoaModel pessoa)
        {
            ViewBag.AlterarPerfil = pessoa.IN_PERFIL_MASTER == "S" && pessoa.ID_PESSOA != SessionUsuario.ID_PESSOA ? true : false;
        }

        [HttpPost]
        public JsonResult ConsultaCep(string cep)
        {
            var logradouro = GetLocal.ConsultaCEP(cep);

            return Json(logradouro);
        }

        private PessoaModel CarregaPessoa(PessoaModel pessoa)
        {
            ModelState.Clear();

            if (!EstaLogado)
                ViewBag.tipo = "b2c";

            if (pessoa.ID_PESSOA != null && pessoa.ID_EMPRESA != null && pessoa.ID_PERFIL != null)
                pessoa = PainelControle.GetPessoa(pessoa.ID_PESSOA.ToDecimal(), pessoa.ID_EMPRESA.ToDecimal(), pessoa.ID_PERFIL.ToDecimal());

            ViwBagsTelaUsuario(pessoa);
            return pessoa;
        }

        [HttpPost]
        public ActionResult DadosUsuario(PessoaModel pessoa, string pag)
        {
            pessoa = CarregaPessoa(pessoa);
            return View("Usuario", pessoa);
        }

        [HttpPost]
        public ActionResult MinhaAgencia(PessoaModel pessoa, string pag)
        {
            pessoa = CarregaPessoa(pessoa);

            return View("Usuario", pessoa);
        }

        [HttpPost]
        public ActionResult UsuarioNovo(PessoaModel pessoa, string pag)
        {
            pessoa = CarregaPessoa(pessoa);
            return View("Usuario", pessoa);
        }

        [HttpPost]
        public ActionResult Usuario(PessoaModel pessoa, string pag)
        {
            pessoa = CarregaPessoa(pessoa);
            return View("Usuario", pessoa);
        }

        private void ViwBagsTelaUsuario(PessoaModel pessoa)
        {
            ViewBag.novo = (pessoa.ID_PESSOA == null ? "S" : "N");

            ViewBagNacional(pessoa.IN_NACIONAL);
            ViewBagPerfil(pessoa.IN_PERFIL_MASTER);
            VisaoComboPerfil(pessoa);
        }

        /// <summary>
        /// Defini qual item deve ser carregado na lista de nascionalidade através da ViewBag.lstNacional
        /// </summary>
        /// <param name="IN_NACIONAL">vazio ('') para selecione 'S' para Brasileiro (Nascional) e 'N' para Outros (Internacional)</param>
        private void ViewBagNacional(string IN_NACIONAL)
        {
            List<SelectListItem> lstNacional = new List<SelectListItem>();
            if (string.IsNullOrEmpty(IN_NACIONAL) && SessionUsuario.B2B)
            {
                lstNacional.Add(new SelectListItem { Text = "Selecione", Value = "", Selected = true });
                lstNacional.Add(new SelectListItem { Text = "Outros", Value = "N" });
                lstNacional.Add(new SelectListItem { Text = "Brasileiro", Value = "S" });
            }
            else if ((string.IsNullOrEmpty(IN_NACIONAL) && !SessionUsuario.B2B) || IN_NACIONAL.ToLower().Equals("s"))
                lstNacional.Add(new SelectListItem { Text = "Brasileiro", Value = "S", Selected = true });
            else
                lstNacional.Add(new SelectListItem { Text = "Outros", Value = "N", Selected = true });

            ViewBag.lstNacional = lstNacional;

        }

        /// <summary>
        /// Defini qual item deve ser carregado na lista de perfil através da ViewBag.lstPerfil
        /// </summary>
        /// <param name="IN_PERFIL">vazio ('') para selecione/Cliente 'S' para Master e 'N' para Atendente</param>
        private void ViewBagPerfil(string IN_PERFIL)
        {
            List<SelectListItem> lstPerfil = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(IN_PERFIL))
            {
                if (IN_PERFIL.ToLower().Equals("s"))
                    lstPerfil.Add(new SelectListItem { Text = "Master", Value = "S", Selected = true });
                else
                    lstPerfil.Add(new SelectListItem { Text = "Atendente", Value = "N", Selected = true });
            }
            else
            {
                if (SessionUsuario.B2B)
                {
                    lstPerfil.Add(new SelectListItem { Text = "Atendente", Value = "N", Selected = true });
                    lstPerfil.Add(new SelectListItem { Text = "Master", Value = "S" });
                }
                else
                    lstPerfil.Add(new SelectListItem { Text = "Cliente", Value = "", Selected = true });
            }

            ViewBag.lstPerfil = lstPerfil;
        }

        [HttpPost]
        public ActionResult SalvarUsuario(PessoaModel pessoa, decimal? ID_PESSOA, decimal? hdnENDERECO_DS_PAISCodigo, decimal? hdnENDERECO_DS_CIDADECodigo)
        {
            //verifica se o usuário esta logado ou se o id em questão é para um perfil de cliente.
            //neste caso não a necessidade de se estar logado.
            if (EstaLogado || string.IsNullOrEmpty(pessoa.IN_PERFIL_MASTER))
            {
                var getPessoa = PreencheModelPessoa(pessoa, ID_PESSOA, hdnENDERECO_DS_PAISCodigo, hdnENDERECO_DS_CIDADECodigo);
                var salvaPessoa = PainelControle.SalvaUsuario(getPessoa);

                if (string.IsNullOrEmpty(salvaPessoa.DS_ERRO))
                    getPessoa.DS_ERRO = "Usuário salvo com sucesso!";
                else
                    getPessoa.DS_ERRO = salvaPessoa.DS_ERRO.Replace("\"","\'").Replace("\r","").Replace("\n","");

                ViwBagsTelaUsuario(getPessoa);
                return View("Usuario", getPessoa);
            }
            else
                return Redirect(System.Web.Security.FormsAuthentication.LoginUrl);
        }

        private PessoaModel PreencheModelPessoa(PessoaModel pessoa, decimal? ID_PESSOA, decimal? hdnENDERECO_DS_PAISCodigo, decimal? hdnENDERECO_DS_CIDADECodigo)
        {

            pessoa.ID_PESSOA = ID_PESSOA;
            pessoa.ENDERECO.ID_PAIS = hdnENDERECO_DS_PAISCodigo;
            pessoa.ENDERECO.ID_CIDADE = hdnENDERECO_DS_CIDADECodigo;

            PessoaModel getPessoa = new PessoaModel();
            if (pessoa.ID_PESSOA != null)
                getPessoa = PainelControle.GetPessoa(pessoa.ID_PESSOA.ToDecimal(), pessoa.ID_EMPRESA.ToDecimal(), pessoa.ID_PERFIL.ToDecimal());

            #region campos tela
            //DADOS PESSOAIS
            getPessoa.NM_PESSOA = pessoa.NM_PESSOA;
            getPessoa.IN_NACIONAL = pessoa.IN_NACIONAL;
            getPessoa.NR_CPF_CNPJ = pessoa.NR_CPF_CNPJ;
            getPessoa.DT_NASCIMENTO = pessoa.DT_NASCIMENTO;
            getPessoa.IN_SEXO = pessoa.IN_SEXO;

            //DADOS ENDEREÇO
            getPessoa.ENDERECO.NR_CEP = pessoa.ENDERECO.NR_CEP;
            getPessoa.ENDERECO.DS_ENDERECO = pessoa.ENDERECO.DS_ENDERECO;
            getPessoa.ENDERECO.NR_ENDERECO = pessoa.ENDERECO.NR_ENDERECO;
            getPessoa.ENDERECO.DS_COMPLEMENTO = pessoa.ENDERECO.DS_COMPLEMENTO;
            getPessoa.ENDERECO.ID_PAIS = pessoa.ENDERECO.ID_PAIS;
            getPessoa.ENDERECO.DS_PAIS = pessoa.ENDERECO.DS_PAIS;
            getPessoa.ENDERECO.ID_CIDADE = pessoa.ENDERECO.ID_CIDADE;
            getPessoa.ENDERECO.DS_BAIRRO = pessoa.ENDERECO.DS_BAIRRO;

            //DADOS CONTATO
            getPessoa.NR_TELEFONE = pessoa.NR_TELEFONE;
            getPessoa.NR_TELEFONE_CEL = pessoa.NR_TELEFONE_CEL;

            //DADOS ACESSO
            getPessoa.USUARIO = string.IsNullOrEmpty(pessoa.USUARIO) ? Request.Params["USUARIO"] : pessoa.USUARIO;
            getPessoa.SENHA = pessoa.SENHA;
            getPessoa.REDEFINIR_SENHA = pessoa.REDEFINIR_SENHA;
            getPessoa.ID_PERFIL = pessoa.ID_PERFIL;
            getPessoa.DS_PERFIL = pessoa.DS_PERFIL;

            if (pessoa.ID_PESSOA != null)
            {
                if (pessoa.ID_PERFIL == 17)
                    getPessoa.IN_PERFIL_MASTER = "S";
                else if (pessoa.ID_PERFIL == 159)
                    getPessoa.IN_PERFIL_MASTER = null;
                else
                    getPessoa.IN_PERFIL_MASTER = "N";
            }
            #endregion

            if (EstaLogado)
                getPessoa.ID_PESSOA_AGENCIA = SessionUsuario.ID_PESSOA_AGENCIA;

            if (string.IsNullOrEmpty(pessoa.IN_PERFIL_MASTER))
            {
                getPessoa.IN_PERFIL_CLIENTE = "S";
                getPessoa.IN_PERFIL_MASTER = null;
            }
            else
            {
                getPessoa.IN_PERFIL_CLIENTE = null;
                getPessoa.IN_PERFIL_MASTER = pessoa.IN_PERFIL_MASTER;
            }

            return getPessoa;
        }

        public ActionResult Menu(string titulo)
        {
            @ViewBag.titulo = titulo;
            return View(SessionUsuario);
        }

        public ActionResult Extrato()
        {
            return View();
        }

        public ActionResult Reserva()
        {
            return View();
        }
    }
}
