using FT.Web.Core.Util.SiteTracer;
using FT.Web.Model.Models;
using FTV.Shared.Business;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace FT.Web.Site.Controllers.Shared
{
    [Tracer]
    public class SharedController : Controller
    {
        //
        // GET: /Shared/
        //
        static FT.Web.Business.UsuarioService.UsuarioClient usuarioService;
        private static Controller _currentController;
        private static string _key;
        private static bool _showMenu;

        public static bool ShowMenu
        {
            get
            {
                return bool.TryParse(ConfigurationManager.AppSettings["ExibirMenuSuperiorNoFTV"], out _showMenu) && _showMenu;
            }
        }

        public SharedController()
        {
            _currentController = this;
            _key = "ShowMenu";
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Regionalizacao()
        {
            List<RegionalizacaoModel> regioes = new List<RegionalizacaoModel>();
            try
            {
                XDocument doc = XDocument.Load(ConfigurationManager.AppSettings["CaminhoXmlRegiao"]);

                var query = from d in doc.Root.Descendants("Regionalizacao") select d;
                foreach (var q in query)
                {
                    regioes.Add(new RegionalizacaoModel()
                    {
                        CodigoCidade = q.Element("CodigoCidade").Value,
                        NomeCidade = q.Element("NomeCidade").Value
                    });
                }

                if (Session["Regiao"] == null)
                    Session["Regiao"] = regioes[0].CodigoCidade;

            }
            catch (Exception)
            {
                RegionalizacaoModel regiao = new RegionalizacaoModel();
                regiao.NomeCidade = "São Paulo";
                regiao.CodigoCidade = "9668";
                regioes.Add(regiao);
            }

            return PartialView(regioes);
        }

        [HttpPost]
        public JsonResult RegionalizarSite(string codigoCidade)
        {
            try
            {
                bool result = false;

                if (!String.IsNullOrEmpty(codigoCidade))
                {
                    Session["Regiao"] = codigoCidade;
                    result = true;
                }

                return Json(result);
            }
            catch (Exception)
            {
                return Json(null);
                throw;
            }
        }

        [HttpPost]
        public JsonResult TodasAsAgenciasMapas(int IDEmpresa)
        {
            Business.Shared.SharedDAO sharedDAO = new Business.Shared.SharedDAO();
            List<AgenciaMapaModel> agenciasList = sharedDAO.GetAgenciasPorEmpresa(IDEmpresa, String.Empty);

            return Json(agenciasList);//, JsonRequestBehavior.AllowGet);
        }

        public FT.Web.Business.UsuarioService.Login LoginService()
        {
            usuarioService = new FT.Web.Business.UsuarioService.UsuarioClient();

            string usuario = ConfigurationManager.AppSettings["loginService"];
            string senha = ConfigurationManager.AppSettings["senhaService"];

            return AppCache.Get<FT.Web.Business.UsuarioService.Login>(EnumProcesso.PacoteMatriz, usuario + senha, FTV.Conv.ToInt32(TimeSpan.FromHours(11).TotalMinutes), () =>
            {
                return usuarioService.Login(new FT.Web.Business.UsuarioService.Pessoa() { Usuario = usuario, Senha = senha });
            });
        }

        public static List<SelectListItem> OpcoesSelectList(string[] opcoesText, string[] opcoesValor  = null, bool comSelect = true)
        {
            List<SelectListItem> opcoesAssunto = new List<SelectListItem>();
            if (comSelect)
                opcoesAssunto.Add(new SelectListItem()
                {
                    Text = "Selecione...",
                    Value = String.Empty
                });

            for (int i = 0; opcoesText != null && i < opcoesText.Length; i++)
            {
                opcoesAssunto.Add(new SelectListItem() { Text = opcoesText[i], Value = opcoesValor != null && i < opcoesValor.Length ? opcoesValor[i]: opcoesText[i] });
            }
            
            return opcoesAssunto;
        }

        public static FT.Web.Business.UtilService.FaleConoscoModel ToUtilService_FaleConoscoModel(FaleConoscoModel faleConosco)
        {
            return faleConosco == null ? null : new Business.UtilService.FaleConoscoModel()
            {
                Assunto = faleConosco.Assunto,
                Celular = faleConosco.Celular,
                Cidade = faleConosco.Cidade,
                DataDeNascimento = faleConosco.DataDeNascimento,
                Email = faleConosco.Email,
                Estado = faleConosco.Estado,
                IdEmpresaDestinataria = faleConosco.IdEmpresaDestinataria,
                Mensagem = faleConosco.Mensagem,
                Nome = faleConosco.Nome,
                Sexo = faleConosco.Sexo,
                Telefone = faleConosco.Telefone
            };
        }
    }
}
