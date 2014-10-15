using FTV;
using FTV.Shared.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using FT.Web.Model.Models;
using FT.Web.Business.Gerenciamento;
using System.Linq;
using FT.Web.Business.Service;

namespace FT.Web.Site.Controllers.Base
{
    public class _BaseController : Multibase
    {
        static FT.Web.Business.UsuarioService.UsuarioClient usuarioService = new FT.Web.Business.UsuarioService.UsuarioClient();

        public PessoaModel SessionUsuario 
        {
            get { return Session["Usuario"] == null ? new PessoaModel() : (PessoaModel)Session["Usuario"]; }
            set { Session["Usuario"] = value; }
        }

        [Obsolete]
        public List<MontePacoteModel> SessionMontePacote
        {
            get { return Session["MontePacoteAntigo"] == null ? new List<MontePacoteModel>() : (List<MontePacoteModel>)Session["MontePacoteAntigo"]; }
            set { Session["MontePacoteAntigo"] = value; }
        }

        public MontePacoteModel MPSession
        {
            get { return Session["MontePacote"] == null ? new MontePacoteModel() : (MontePacoteModel)Session["MontePacote"]; }
            set { Session["MontePacote"] = value; }
        }

        public String SessionKey
        {
            get
            {
                if (Session["SessionKey"] == null)
                {
                    Session["SessionKey"] = Guid.NewGuid().ToString();
                }
                return Session["SessionKey"] as String;
            }
        }

        /// <summary>
        /// Propriedade Get que retorna se o usuário esta logado
        /// </summary>
        public bool EstaLogado
        {
            get {
                if (SessionUsuario.ID_PESSOA != null
                    || !string.IsNullOrEmpty(SessionUsuario.NM_PESSOA) 
                    )
                    return true;
                else 
                    return false;
            }
        }

        public List<int> SessionFiltroHotelPorEstrelas
        {
            get { return Session["FiltroPorEstrelasHotel"] == null ? new List<int>() : (List<int>)Session["FiltroPorEstrelasHotel"]; }
            set { Session["FiltroPorEstrelasHotel"] = value; }
        }

        public string SessionFiltroHotelPorNome
        {
            get { return Session["FiltroPorNomeHotel"] == null ? "" : (string)Session["FiltroPorNomeHotel"]; }
            set { Session["FiltroPorNomeHotel"] = value; }
        }

        public decimal? SessionFiltroMenorValorHotel
        {
            get { return Session["FiltroMenorValorHotel"] == null ? null : (decimal?)Session["FiltroMenorValorHotel"]; }
            set { Session["FiltroMenorValorHotel"] = value; }
        }

        public decimal? SessionFiltroMaiorValorHotel
        {
            get { return Session["FiltroMaiorValorHotel"] == null ? null : (decimal?)Session["FiltroMaiorValorHotel"]; }
            set { Session["FiltroMaiorValorHotel"] = value; }
        }

        public List<string> SessionFiltroFacilidadesHotel
        {
            get { return Session["FiltroFacilidadesHotel"] == null ? new List<string>() : (List<string>)Session["FiltroFacilidadesHotel"]; }
            set { Session["FiltroFacilidadesHotel"] = value; }
        }



        public string HomeUrl { get { return Request.Url.Scheme +"://" +Request.Url.Authority; } }
        public string CurrentUrl { get { return Request.Url.AbsolutePath; } }
        public string CurrentUrlFull { get { return Request.Url.AbsoluteUri; } }        

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            
            
            base.OnAuthorization(filterContext);
            if (PainelControle.ValidaRestricaoPagina(CurrentUrl))
                if (EstaLogado)
                {
                    SessionUsuario = SessionUsuario;
                    
                    bool acesso = PainelControle.GetPermissaoPagina(SessionUsuario, CurrentUrl);
                    if (!acesso)
                    {
                        if (filterContext.HttpContext.Response.RedirectLocation == null)
                            filterContext.HttpContext.Response.Redirect("/Autenticar/AcessoNegado", true);
                    }
                }
                else
                {
                    if (filterContext.HttpContext.Response.RedirectLocation == null)
                        filterContext.HttpContext.Response.Redirect(FormsAuthentication.LoginUrl + "?returnUrl=" + CurrentUrl, true);
                }
        }

        public FT.Web.Business.UsuarioService.Login LoginService()
        {
            FT.Web.Business.UsuarioService.Login login = new Business.UsuarioService.Login();

            if (Session["LoginService"] == null)
            {
                string usuario = ConfigurationManager.AppSettings["loginService"];
                string senha = ConfigurationManager.AppSettings["senhaService"];

                login = usuarioService.Login(new FT.Web.Business.UsuarioService.Pessoa() { Usuario = usuario, Senha = senha });

                Session["LoginService"] = login;

                //return AppCache.Get<FT.Web.Business.UsuarioService.Login>(EnumProcesso.PacoteMatriz, usuario + senha, Conv.ToInt32(TimeSpan.FromHours(11).TotalMinutes), () =>
                //{
                //    return usuarioService.Login(new FT.Web.Business.UsuarioService.Pessoa() { Usuario = usuario, Senha = senha });
                //});
            }
            else
            {
                login = Session["LoginService"] as FT.Web.Business.UsuarioService.Login;
            }

            return login;
        }

        public string retornaNomeMes(long Numeromes)
        {
            switch (Numeromes)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";

            }
            return "";
        }

        public static string RetornaNomeMesView(int numeromes)
        {
            switch (numeromes)
            {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";

            }
            return "";
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            RetornarRegiao();
            base.OnActionExecuting(filterContext);
        }

        private void RetornarRegiao()
        {
        }

        protected bool BloquearApartamentizacao(FlyTourModel model)
        {
            bool isApartamentizado = false;

            //Verifica se todos os produtos que estão no cache para compra estão com status Comprar = false
            //esse status indica que o produto está no carrinho, se tiver itens no carrinho a apartamentização
            //deve ficar bloqueada de acordo com os itens já inseridos no carrinho

            if (model.Pacote.Count(p => !p.Comprar) > 0)
            {
                isApartamentizado = true;
            }

            if (!isApartamentizado && model.Aereo.Count(p => !p.Comprar) > 0)
            {
                isApartamentizado = true;
            }

            if (!isApartamentizado && model.Hotel.Count(p => !p.Comprar) > 0)
            {
                isApartamentizado = true;
            }

            if (!isApartamentizado && model.Atividade.Count(p => !p.Comprar) > 0)
            {
                isApartamentizado = true;
            }
            
            return isApartamentizado;
        }

        protected void CleanItensComprar(FlyTourModel model)
        {
            if (model.Pacote.Count(p => p.Comprar) > 0)
                model.Pacote.RemoveAll(p => p.Comprar);

            if (model.Aereo.Count(p => p.Comprar) > 0)
                model.Aereo.RemoveAll(p => p.Comprar);

            if (model.Hotel.Count(p => p.Comprar) > 0)
                model.Hotel.RemoveAll(p => p.Comprar);

            if (model.Carro.Count(p => p.Comprar) > 0)
                model.Carro.RemoveAll(p => p.Comprar);

            if (model.Atividade.Count(p => p.Comprar) > 0)
                model.Atividade.RemoveAll(p => p.Comprar);

            AppCache.Put(EnumProcesso.PacoteMatriz, SessionKey, model, TimeSpan.FromMinutes(90));

        }
    }
}