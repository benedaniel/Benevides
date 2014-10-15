using System;
using System.Web.Mvc;
using FT.Web.Site.Controllers.Base;
using FT.Web.Business.Gerenciamento;
using FT.Web.Model.Models;
using FT.Web.Core.Util.Validation;
using System.Linq;
using FT.Web.Core.Util.SiteTracer;
using SVC = FT.Web.Business.GerenciamentoSistemaService;

namespace FT.Web.Site.Controllers.Gerenciamento
{
    [Tracer]
    public class AutenticarController : _BaseController
    {

        //
        // GET: /Autenticar/

        public ActionResult Index(string returnUrl, PessoaModel usuarioModel)
        {

            ModelState.Clear();

            if (EstaLogado)
            { return Redirect(HomeUrl); }
            else
            {
                ViewBag.Error = "";
                return View(usuarioModel);
            }

        }

        //public void ValidaPerfilUsuario(LoginDadosModel perfilAgencia)
        //{
        //    if (string.IsNullOrEmpty(SessionUsuario.DS_PERFIL))
        //    {
        //        if (perfilAgencia.lstPerfil.Count > 0)
        //            ViewBag.lstPerfil = perfilAgencia.lstPerfil;
        //    }

        //}

        //public void ValidaAgenciaUsuario(LoginDadosModel perfilAgencia)
        //{
        //    if (string.IsNullOrEmpty(SessionUsuario.NM_PESSOA_AGENCIA))
        //    {
        //        if (perfilAgencia.lstAgencia.Count > 0)
        //            ViewBag.lstAgencia = perfilAgencia.lstAgencia;
        //    }
        //}

        //[HttpPost]
        //public ActionResult SetaPerfil(decimal? CodigoPerfil, string NomePerfil)
        //{
        //    SessionUsusario.ID_PERFIL = CodigoPerfil; SessionUsusario.DS_PERFIL = NomePerfil;
        //    return View("index");
        //}

        [HttpPost]
        //public JsonResult SetaPerfilAgenciaUsuario(PessoaModel usuarioModel, decimal? CodigoPerfil, decimal? CodigoAgencia)
        public JsonResult SetaPerfilAgenciaUsuario(decimal? CodigoPerfil, decimal? CodigoAgencia, decimal? ID_PESSOA, decimal? ID_EMPRESA, string id_token)
        {
            var usuarioModel = PainelControle.GetPessoa(ID_PESSOA.ToDecimal(), ID_EMPRESA.ToDecimal(), CodigoPerfil.ToDecimal());

            usuarioModel.ID_PERFIL = CodigoPerfil;
            usuarioModel.Token = id_token;
            if (usuarioModel.ID_PERFIL != 159)
            {
                usuarioModel.ID_PESSOA_AGENCIA = CodigoAgencia;
                var ID_PESSOA_DISTRIBUIDOR = DadosUsuariosID_Distribuidor(usuarioModel, usuarioModel.Token);
                usuarioModel.ID_PESSOA_DISTRIBUIDOR = ID_PESSOA_DISTRIBUIDOR;
            }
            else
                usuarioModel.ID_PESSOA_AGENCIA = null;           

            SessionUsuario = usuarioModel;
            return Json("");
        }

        public static decimal DadosUsuariosID_Distribuidor(PessoaModel usuarioModel, string token)
        {
            SVC.PessoaDados usuarioLogado = new SVC.PessoaDados();
            SVC.GerenciamentoSistemaClient service = new SVC.GerenciamentoSistemaClient();
            ConvertMethod.EntityToEntity<PessoaModel, SVC.PessoaDados>(usuarioModel, ref usuarioLogado);
            usuarioLogado.Token = token;
            SVC.LoginDados loginDados = service.ListaDadosUsuario(usuarioLogado);

            var retorno = loginDados.lstAgencia.Where(p => p.ID_PESSOA_AGENCIA == usuarioModel.ID_PESSOA_AGENCIA).FirstOrDefault().ID_PESSOA_DISTRIBUIDOR.Value;
            
            return retorno;
        }

        string token;
        [HttpPost]
        public ActionResult Logar(PessoaModel usuarioModel, string paramUrl)
        {
            string url = string.IsNullOrEmpty(paramUrl) ? HomeUrl : paramUrl;
            try
            {

                ViewBag.Error = "";

                usuarioModel.ID_SISTEMA = "1";
                usuarioModel.ID_EMPRESA = 1;

                usuarioModel = LoginSistema.Logar(usuarioModel);
                token = usuarioModel.Token;
                if (usuarioModel.ID_PESSOA != null)
                {
                    usuarioModel = PainelControle.GetPessoa(usuarioModel.ID_PESSOA.ToDecimal(), usuarioModel.ID_EMPRESA.ToDecimal(), usuarioModel.ID_PERFIL.ToDecimal());
                    usuarioModel.Token = token;

                    LoginDadosModel perfilAgencia = null;
                    if (usuarioModel.ID_PERFIL == 159)
                    {
                        SessionUsuario = usuarioModel;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(usuarioModel.DS_PERFIL) || string.IsNullOrEmpty(usuarioModel.NM_PESSOA_AGENCIA))
                        {
                            var login = LoginSistema.LoginService();
                            perfilAgencia = PainelControle.DadosUsuarios(usuarioModel, login.Token);

                            ViewBag.lstPerfil = perfilAgencia.lstPerfil;
                            ViewBag.lstAgencia = perfilAgencia.lstAgencia;
                            ViewBag.url = url;
                            return View("Index", usuarioModel);
                        }
                        else
                        {
                            SessionUsuario = usuarioModel;
                        }
                    }
                }
                else
                {
                    ViewBag.Error = usuarioModel.DS_ERRO;
                    return View("Index", usuarioModel);
                }

            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
            }
            return Redirect(url);

        }

        #region Recupera senha

        public ActionResult RecuperarSenha()
        {
            ModelState.Clear();

            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        public JsonResult ExecutaRecuperarSenha(string usuario)
        {
            var usuarioModel = new UsuarioModel { Usuario = usuario };
            if (string.IsNullOrEmpty(usuario))
            {
                string erro = "Campo Usuário";
                ModelState.AddModelError("Usuario", erro);
                ViewBag.Error = erro;
            }
            else
            {
                ViewBag.Error = LoginSistema.RecuperaSenha(usuarioModel);
                if (string.IsNullOrEmpty(ViewBag.Error))
                    ViewBag.Error = @"Dentro de alguns minutos você receberá um e-mail com as instruções para a troca da senha";
            }
            return Json(ViewBag.Error);
        }
        #endregion

        #region Troca Senha

        public ActionResult TrocaSenha(AlteraSenhaModel alterasenhamodel)
        {
            ModelState.Clear();
            ViewBag.Error = "";
            return View(alterasenhamodel);
        }

        [HttpPost]
        public JsonResult ExecutaTrocaSenha(AlteraSenhaModel alterasenhamodel)
        {
            ViewBag.Error = LoginSistema.TrocaSenha(alterasenhamodel);

            if (Convert.ToString(ViewBag.Error) == "")
                ViewBag.Error = "Senha Alterada com Sucesso.";

            return Json(ViewBag.Error);
        }
        #endregion

        public ActionResult HomeLogin()
        {

            ViewBag.Usuario = EstaLogado;

            return PartialView(SessionUsuario);
        }

        public ActionResult LogOff()
        {
            SessionUsuario = null;
            return Redirect(HomeUrl);
        }

        public ActionResult _Logar()
        {
            return PartialView();
        }

        public ActionResult AcessoNegado()
        {
            return View();
        }
    }
}
