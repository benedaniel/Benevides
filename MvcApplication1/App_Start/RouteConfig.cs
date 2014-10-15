using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FT.Web.Site
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Fale conosco
            routes.MapRoute(
                name: "FaleConoscoPage",
                url: "{siteName}/institucional-fale-conosco/{action}",
                defaults: new
                {
                    siteName = "viagens-a-lazer",
                    controller = "FaleConosco",
                    action = "Index"
                }
            );

            // Onde Estamos
            routes.MapRoute(
                name: "OndeEstamos",
                url: "{siteName}/institucional-onde-estamos/{action}",
                defaults: new
                {
                    siteName = "viagens-a-lazer",
                    controller = "OndeEstamos",
                    action = "Index"
                }
            );


            #region Hotsites

            // Consolidadora
            routes.MapRoute(
                name: "Consolidadora",
                url: "consolidadora/{hotsite}/",
                defaults: new
                {
                    controller = "Consolidadora",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );

            // Eventos e Incentivo
            routes.MapRoute(
                name: "EventosEIncentivo",
                url: "eventos-e-incentivo/{hotsite}/",
                defaults: new
                {
                    controller = "EventosEIncentivo",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );

            // Franchising
            routes.MapRoute(
                name: "Franchising",
                url: "franchising/{hotsite}/",
                defaults: new
                {
                    controller = "Franchising",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );

            // Viagens a Negócios
            routes.MapRoute(
                name: "ViagensANegocios",
                url: "viagens-a-negocios/{hotsite}/",
                defaults: new
                {
                    controller = "ViagensANegocios",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );

            // Viagens a Lazer
            routes.MapRoute(
                name: "SiteViagensALazer",
                url: "viagens-a-lazer/{hotsite}/",
                defaults: new
                {
                    controller = "Home",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );

            #region Companha



            // Franquia marilia
            routes.MapRoute(
                name: "marilia",
                url: "marilia/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "marilia"
                }
            );





            // Franquia maringa
            routes.MapRoute(
                name: "maringa",
                url: "maringa/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "maringa"
                }
            );



            // Franquia mogi
            routes.MapRoute(
                name: "mogi",
                url: "mogi/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "mogi"
                }
            );


            // Franquia cg2015
            routes.MapRoute(
                name: "cg2015",
                url: "cg2015/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "cg2015"
                }
            );



            // Franquia montenegro
            routes.MapRoute(
                name: "montenegro",
                url: "montenegro/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "montenegro"
                }
            );





            // Franquia natal
            routes.MapRoute(
                name: "natal",
                url: "natal/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "natal"
                }
            );


            // Franquia dwl
            routes.MapRoute(
                name: "dwl",
                url: "dwl/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "dwl"
                }
            );


            // Franquia osasco
            routes.MapRoute(
                name: "osasco",
                url: "osasco/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "osasco"
                }
            );


            // Franquia passofundo
            routes.MapRoute(
                name: "passofundo",
                url: "passofundo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "passofundo"
                }
            );

            // Franquia piracicaba
            routes.MapRoute(
                name: "piracicaba",
                url: "piracicaba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "piracicaba"
                }
            );





            // Franquia pocosdecaldas
            routes.MapRoute(
                name: "pocosdecaldas",
                url: "pocosdecaldas/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "pocosdecaldas"
                }
            );





            // Franquia portoalegre
            routes.MapRoute(
                name: "portoalegre",
                url: "portoalegre/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "portoalegre"
                }
            );





            // Franquia poacarlosgomes
            routes.MapRoute(
                name: "poacarlosgomes",
                url: "poacarlosgomes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "poacarlosgomes"
                }
            );










            // Franquia portoalegrecentro
            routes.MapRoute(
                name: "portoalegrecentro",
                url: "portoalegrecentro/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "portoalegrecentro"
                }
            );










            // Franquia recife
            routes.MapRoute(
                name: "recife",
                url: "recife/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "recife"
                }
            );









            // Franquia ribeiraopreto
            routes.MapRoute(
                name: "ribeiraopreto",
                url: "ribeiraopreto/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "ribeiraopreto"
                }
            );









            // Franquia riodejaneiro
            routes.MapRoute(
                name: "riodejaneiro",
                url: "riodejaneiro/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riodejaneiro"
                }
            );









            // Franquia riobarradatijuca
            routes.MapRoute(
                name: "riobarradatijuca",
                url: "riobarradatijuca/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riobarradatijuca"
                }
            );









            // Franquia riocentro
            routes.MapRoute(
                name: "riocentro",
                url: "riocentro/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riocentro"
                }
            );









            // Franquia rioevaristo
            routes.MapRoute(
                name: "rioevaristo",
                url: "rioevaristo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "rioevaristo"
                }
            );









            // Franquia riogrande
            routes.MapRoute(
                name: "riogrande",
                url: "riogrande/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riogrande"
                }
            );









            // Franquia rondonopolis
            routes.MapRoute(
                name: "rondonopolis",
                url: "rondonopolis/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "rondonopolis"
                }
            );









            // Franquia salvador
            routes.MapRoute(
                name: "salvador",
                url: "salvador/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "salvador"
                }
            );









            // Franquia salvadorpituba
            routes.MapRoute(
                name: "salvadorpituba",
                url: "salvadorpituba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "salvadorpituba"
                }
            );









            // Franquia salvadorbarra
            routes.MapRoute(
                name: "salvadorbarra",
                url: "salvadorbarra/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "salvadorbarra"
                }
            );









            // Franquia santacruz
            routes.MapRoute(
                name: "santacruz",
                url: "santacruz/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santacruz"
                }
            );









            // Franquia santoandre
            routes.MapRoute(
                name: "santoandre",
                url: "santoandre/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santoandre"
                }
            );









            // Franquia santoangelo
            routes.MapRoute(
                name: "santoangelo",
                url: "santoangelo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santoangelo"
                }
            );







            // Franquia saocarlos
            routes.MapRoute(
                name: "saocarlos",
                url: "saocarlos/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saocarlos"
                }
            );








            // Franquia saojoseriopreto
            routes.MapRoute(
                name: "saojoseriopreto",
                url: "saojoseriopreto/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saojoseriopreto"
                }
            );




            // Franquia saojosedoscampos
            routes.MapRoute(
                name: "saojosedoscampos",
                url: "saojosedoscampos/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saojosedoscampos"
                }
            );


            // Franquia saoleopoldo
            routes.MapRoute(
                name: "saoleopoldo",
                url: "saoleopoldo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saoleopoldo"
                }
            );


            // Franquia saoluis
            routes.MapRoute(
                name: "saoluis",
                url: "saoluis/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saoluis"
                }
            );


            // Franquia saosebastiao
            routes.MapRoute(
                name: "saosebastiao",
                url: "saosebastiao/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saosebastiao"
                }
            );


            // Franquia sorocaba
            routes.MapRoute(
                name: "sorocaba",
                url: "sorocaba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "sorocaba"
                }
            );


            // Franquia berrini
            routes.MapRoute(
                name: "berrini",
                url: "berrini/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "berrini"
                }
            );





            // Franquia spcentro
            routes.MapRoute(
                name: "spcentro",
                url: "spcentro/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcentro"
                }
            );


            // Franquia higienopolis
            routes.MapRoute(
                name: "higienopolis",
                url: "higienopolis/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "higienopolis"
                }
            );


            // Franquia paulista
            routes.MapRoute(
                name: "paulista",
                url: "paulista/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista"
                }
            );


            // Franquia tatuape
            routes.MapRoute(
                name: "tatuape",
                url: "tatuape/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "tatuape"
                }
            );


            // Franquia uberaba
            routes.MapRoute(
                name: "uberaba",
                url: "uberaba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "uberaba"
                }
            );

            // Franquia uberlandia
            routes.MapRoute(
                name: "uberlandia",
                url: "uberlandia/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "uberlandia"
                }
            );


            // Franquia vitoria
            routes.MapRoute(
                name: "vitoria",
                url: "vitoria/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "vitoria"
                }
            );

            // Franquia rioclaro
            routes.MapRoute(
                name: "rioclaro",
                url: "rioclaro/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "rioclaro"
                }
            );


            // Franquia itushopping
            routes.MapRoute(
                name: "itushopping",
                url: "itushopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "itushopping"
                }
            );


            // Franquia braganca
            routes.MapRoute(
                name: "braganca",
                url: "braganca/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "braganca"
                }
            );


            // Franquia lem
            routes.MapRoute(
                name: "lem",
                url: "lem/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "lem"
                }
            );


            // Franquia santanadeparnaiba
            routes.MapRoute(
                name: "santanadeparnaiba",
                url: "santanadeparnaiba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santanadeparnaiba"
                }
            );


            // Franquia niteroi
            routes.MapRoute(
                name: "niteroi",
                url: "niteroi/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "niteroi"
                }
            );


            // Franquia bhminasshopping
            routes.MapRoute(
                name: "bhminasshopping",
                url: "bhminasshopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bhminasshopping"
                }
            );


            // Franquia moema
            routes.MapRoute(
                name: "moema",
                url: "moema/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "moema"
                }
            );


            // Franquia copacabana
            routes.MapRoute(
                name: "copacabana",
                url: "copacabana/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "copacabana"
                }
            );


            // Franquia jardimpaulista
            routes.MapRoute(
                name: "jardimpaulista",
                url: "jardimpaulista/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "jardimpaulista"
                }
            );


            // Franquia pelotas
            routes.MapRoute(
                name: "pelotas",
                url: "pelotas/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "pelotas"
                }
            );


            // Franquia spcampobelo
            routes.MapRoute(
                name: "spcampobelo",
                url: "spcampobelo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcampobelo"
                }
            );

            // Franquia boulevard
            routes.MapRoute(
                name: "boulevard",
                url: "boulevard/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "boulevard"
                }
            );



            // Franquia barrashopping
            routes.MapRoute(
                name: "barrashopping",
                url: "barrashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "barrashopping"
                }
            );


            // Franquia casashopping
            routes.MapRoute(
                name: "casashopping",
                url: "casashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "casashopping"
                }
            );


            // Franquia tijucashopping
            routes.MapRoute(
                name: "tijucashopping",
                url: "tijucashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "tijucashopping"
                }
            );


            // Franquia shoppingparquedasbandeiras
            routes.MapRoute(
                name: "shoppingparquedasbandeiras",
                url: "shoppingparquedasbandeiras/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "shoppingparquedasbandeiras"
                }
            );


            // Franquia riobarrashopping
            routes.MapRoute(
                name: "riobarrashopping",
                url: "riobarrashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riobarrashopping"
                }
            );


            // Franquia riocasashopping
            routes.MapRoute(
                name: "riocasashopping",
                url: "riocasashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riocasashopping"
                }
            );


            // Franquia rioshoppingtijuca
            routes.MapRoute(
                name: "rioshoppingtijuca",
                url: "rioshoppingtijuca/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "rioshoppingtijuca"
                }
            );


            // Franquia macae
            routes.MapRoute(
                name: "macae",
                url: "macae/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "macae"
                }
            );


            // Franquia postos
            routes.MapRoute(
                name: "postos",
                url: "postos/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "postos"
                }
            );


            // Franquia spbelavista
            routes.MapRoute(
                name: "spbelavista",
                url: "spbelavista/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spbelavista"
                }
            );


            // Franquia spmoocaplazashopping
            routes.MapRoute(
                name: "spmoocaplazashopping",
                url: "spmoocaplazashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spmoocaplazashopping"
                }
            );


            // Franquia saobernardoplazashopping
            routes.MapRoute(
                name: "saobernardoplazashopping",
                url: "saobernardoplazashopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saobernardoplazashopping"
                }
            );


            // Franquia shoppingtambore
            routes.MapRoute(
                name: "shoppingtambore",
                url: "shoppingtambore/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "shoppingtambore"
                }
            );


            // Franquia chacarasantoantonio
            routes.MapRoute(
                name: "chacarasantoantonio",
                url: "chacarasantoantonio/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "chacarasantoantonio"
                }
            );



            // Franquia alphavillecentrocorporativo
            routes.MapRoute(
                name: "alphavillecentrocorporativo",
                url: "alphavillecentrocorporativo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "alphavillecentrocorporativo"
                }
            );


            // Franquia bhfuncionarios
            routes.MapRoute(
                name: "bhfuncionarios",
                url: "bhfuncionarios/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bhfuncionarios"
                }
            );


            // Franquia riopresvargas
            routes.MapRoute(
                name: "riopresvargas",
                url: "riopresvargas/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riopresvargas"
                }
            );


            // Franquia brasiliaparkshopping
            routes.MapRoute(
                name: "brasiliaparkshopping",
                url: "brasiliaparkshopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "brasiliaparkshopping"
                }
            );


            // Franquia shoppingpatiobrasil
            routes.MapRoute(
                name: "shoppingpatiobrasil",
                url: "shoppingpatiobrasil/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "shoppingpatiobrasil"
                }
            );



            // Franquia curitiba
            routes.MapRoute(
                name: "curitiba",
                url: "curitiba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "curitiba"
                }
            );





            // Franquia curitibaaguaverde
            routes.MapRoute(
                name: "curitibaaguaverde",
                url: "curitibaaguaverde/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "curitibaaguaverde"
                }
            );







            // Franquia erechim
            routes.MapRoute(
                name: "erechim",
                url: "erechim/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "erechim"
                }
            );






            // Franquia florianopolis
            routes.MapRoute(
                name: "florianopolis",
                url: "florianopolis/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "florianopolis"
                }
            );





            // Franquia fortaleza
            routes.MapRoute(
                name: "fortaleza",
                url: "fortaleza/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "fortaleza"
                }
            );


            // Franquia fozdoiguacu
            routes.MapRoute(
                name: "fozdoiguacu",
                url: "fozdoiguacu/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "fozdoiguacu"
                }
            );





            // Franquia franca
            routes.MapRoute(
                name: "franca",
                url: "franca/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "franca"
                }
            );





            // Franquia goiania
            routes.MapRoute(
                name: "goiania",
                url: "goiania/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "goiania"
                }
            );





            // Franquia guaruja
            routes.MapRoute(
                name: "guaruja",
                url: "guaruja/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "guaruja"
                }
            );





            // Franquia guarulhos
            routes.MapRoute(
                name: "guarulhos",
                url: "guarulhos/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "guarulhos"
                }
            );





            // Franquia indaiatuba
            routes.MapRoute(
                name: "indaiatuba",
                url: "indaiatuba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "indaiatuba"
                }
            );










            // Franquia ipatinga
            routes.MapRoute(
                name: "ipatinga",
                url: "ipatinga/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "ipatinga"
                }
            );










            // Franquia itajai
            routes.MapRoute(
                name: "itajai",
                url: "itajai/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "itajai"
                }
            );









            // Franquia itu
            routes.MapRoute(
                name: "itu",
                url: "itu/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "itu"
                }
            );









            // Franquia jaboatao
            routes.MapRoute(
                name: "jaboatao",
                url: "jaboatao/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "jaboatao"
                }
            );









            // Franquia saobernardo
            routes.MapRoute(
                name: "saobernardo",
                url: "saobernardo/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saobernardo"
                }
            );









            // Franquia jaragua
            routes.MapRoute(
                name: "jaragua",
                url: "jaragua/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "jaragua"
                }
            );









            // Franquia spsantana
            routes.MapRoute(
                name: "spsantana",
                url: "spsantana/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spsantana"
                }
            );









            // Franquia joinville
            routes.MapRoute(
                name: "joinville",
                url: "joinville/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "joinville"
                }
            );









            // Franquia juizdefora
            routes.MapRoute(
                name: "juizdefora",
                url: "juizdefora/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "juizdefora"
                }
            );









            // Franquia jundiai
            routes.MapRoute(
                name: "jundiai",
                url: "jundiai/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "jundiai"
                }
            );









            // Franquia lages
            routes.MapRoute(
                name: "lages",
                url: "lages/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "lages"
                }
            );









            // Franquia lajeado
            routes.MapRoute(
                name: "lajeado",
                url: "lajeado/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "lajeado"
                }
            );









            // Franquia laurodefreitas
            routes.MapRoute(
                name: "laurodefreitas",
                url: "laurodefreitas/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "laurodefreitas"
                }
            );









            // Franquia londrina
            routes.MapRoute(
                name: "londrina",
                url: "londrina/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "londrina"
                }
            );


            // Franquia manaus
            routes.MapRoute(
                name: "manaus",
                url: "manaus/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "manaus"
                }
            );



            // Franquia bauru
            routes.MapRoute(
                name: "bauru",
                url: "bauru/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru"
                }
            );


            // Franquia americana
            routes.MapRoute(
                name: "americana",
                url: "americana/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "americana"
                }
            );



            // Franquia aracaju
            routes.MapRoute(
                name: "aracaju",
                url: "aracaju/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "aracaju"
                }
            );



            // Franquia araraquara
            routes.MapRoute(
                name: "araraquara",
                url: "araraquara/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "araraquara"
                }
            );



            // Franquia belem
            routes.MapRoute(
                name: "belem",
                url: "belem/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "belem"
                }
            );


            // Franquia belohorizonte
            routes.MapRoute(
                name: "belohorizonte",
                url: "belohorizonte/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "belohorizonte"
                }
            );



            // Franquia bhsavassi
            routes.MapRoute(
                name: "bhsavassi",
                url: "bhsavassi/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bhsavassi"
                }
            );



            // Franquia bhsantaefigenia
            routes.MapRoute(
                name: "bhsantaefigenia",
                url: "bhsantaefigenia/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bhsantaefigenia"
                }
            );



            // Franquia blumenau
            routes.MapRoute(
                name: "blumenau",
                url: "blumenau/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "blumenau"
                }
            );



            // Franquia botucatu
            routes.MapRoute(
                name: "botucatu",
                url: "botucatu/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "botucatu"
                }
            );



            // Franquia brasiliagilbertosalomao
            routes.MapRoute(
                name: "brasiliagilbertosalomao",
                url: "brasiliagilbertosalomao/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "brasiliagilbertosalomao"
                }
            );



            // Franquia aracajushopping
            routes.MapRoute(
                name: "aracajushopping",
                url: "aracajushopping/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "aracajushopping"


                }
            );



            // Franquia barreira
            routes.MapRoute(
                name: "barreira",
                url: "barreira/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "barreira"


                }
            );




            // Franquia sprepublica
            routes.MapRoute(
                name: "sprepublica",
                url: "sprepublica/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "sprepublica"


                }
            );




            // Franquia brasilia
            routes.MapRoute(
                name: "brasilia",
                url: "brasilia/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "brasilia"


                }
            );




            // Franquia brasiliaII
            routes.MapRoute(
                name: "brasiliaII",
                url: "brasiliaII/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "brasiliaII"


                }
            );




            // Franquia campinas
            routes.MapRoute(
                name: "campinas",
                url: "campinas/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "campinas"


                }
            );



            // Franquia campogrande
            routes.MapRoute(
                name: "campogrande",
                url: "campogrande/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "campogrande"


                }
            );




            // Franquia caxiasdosul
            routes.MapRoute(
                name: "caxiasdosul",
                url: "caxiasdosul/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "caxiasdosul"


                }
            );



            // Franquia chapeco
            routes.MapRoute(
                name: "chapeco",
                url: "chapeco/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "chapeco"


                }
            );





            // Franquia granjaviana
            routes.MapRoute(
                name: "granjaviana",
                url: "granjaviana/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "granjaviana"


                }
            );




            // Franquia criciuma
            routes.MapRoute(
                name: "criciuma",
                url: "criciuma/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "criciuma"


                }
            );




            // Franquia incentivos
            routes.MapRoute(
                name: "incentivos",
                url: "incentivos/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "incentivos"
                }
            );


            // Franquia hsm
            routes.MapRoute(
                name: "hsm",
                url: "hsm/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "hsm"
                }
            );


            // Franquia marketing
            routes.MapRoute(
                name: "marketing",
                url: "marketing/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "marketing"
                }
            );



            // Franquia academia
            routes.MapRoute(
                name: "academia",
                url: "academia/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "academia"
                }
            );



            // Franquia gestores
            routes.MapRoute(
                name: "gestores",
                url: "gestores/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "gestores"
                }
            );




            // Franquia confly2013
            routes.MapRoute(
                name: "confly2013",
                url: "confly2013/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "confly2013"
                }
            );



            // Franquia openews
            routes.MapRoute(
                name: "openews",
                url: "openews/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "openews"
                }
            );



            // Franquia cna2014
            routes.MapRoute(
                name: "cna2014",
                url: "cna2014/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "cna2014"
                }
            );



            // Franquia fe2014
            routes.MapRoute(
                name: "fe2014",
                url: "fe2014/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "fe2014"
                }
            );


            // Franquia wef
            routes.MapRoute(
                name: "wef",
                url: "wef/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "wef"
                }
            );



            //lista-de-presentes/
            routes.MapRoute(
                name: "dwl/lista-de-presentes/",
                url: "dwl/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            //lista-de-presentes/
            routes.MapRoute(
                name: "lista-de-presentes/",
                url: "lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );









            //------------------------------------Novos---------------------//


            // Franquia aracaju/cursosnoexterior
            routes.MapRoute(
                name: "aracaju/cursosnoexterior",
                url: "aracaju/cursosnoexterior/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "aracaju/cursosnoexterior"
                }
            );


            // Franquia aracaju/lista-de-presentes/
            routes.MapRoute(
                name: "aracaju/lista-de-presentes/",
                url: "aracaju/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia aracaju/campanha/
            routes.MapRoute(
                name: "aracaju/campanha/",
                url: "aracaju/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );

            // Franquia bauru/teste
            routes.MapRoute(
                name: "bauru/teste",
                url: "bauru/teste/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru/teste"
                }
            );

            // Franquia bauru/campanha/
            routes.MapRoute(
                name: "bauru/campanha/",
                url: "bauru/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia bauru/lista-de-presentes/
            routes.MapRoute(
                name: "bauru/lista-de-presentes/",
                url: "bauru/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            // Franquia bauru/lasvegas2013
            routes.MapRoute(
                name: "bauru/lasvegas2013",
                url: "bauru/lasvegas2013/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru/lasvegas2013"
                }
            );




            // Franquia bauru/lasvegas
            routes.MapRoute(
                name: "bauru/lasvegas",
                url: "bauru/lasvegas/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru/lasvegas"
                }
            );



            // Franquia bauru/grupoliterario
            routes.MapRoute(
                name: "bauru/grupoliterario",
                url: "bauru/grupoliterario/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru/grupoliterario"
                }
            );






            // Franquia bauru/englishcamp
            routes.MapRoute(
                name: "bauru/englishcamp",
                url: "bauru/englishcamp/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru/englishcamp"
                }
            );







            // Franquia bauru/anfarmag2013
            routes.MapRoute(
                name: "bauru/anfarmag2013",
                url: "bauru/anfarmag2013/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "bauru/anfarmag2013"
                }
            );






            // Franquia berrini/gradual
            routes.MapRoute(
                name: "berrini/gradual",
                url: "berrini/gradual/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "berrini/gradual"
                }
            );


            // Franquia berrini/campanha/
            routes.MapRoute(
                name: "berrini/campanha/",
                url: "berrini/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia berrini/lista-de-presentes/
            routes.MapRoute(
                name: "berrini/lista-de-presentes/",
                url: "berrini/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );





            // Franquia brasiliaii/iiicgti
            routes.MapRoute(
                name: "brasiliaii/iiicgti",
                url: "brasiliaii/iiicgti/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "brasiliaii/iiicgti"
                }
            );


            // Franquia brasiliaii/campanha/
            routes.MapRoute(
                name: "brasiliaii/campanha/",
                url: "brasiliaii/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia brasiliaii/lista-de-presentes/
            routes.MapRoute(
                name: "brasiliaii/lista-de-presentes/",
                url: "brasiliaii/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia campogrande/OABMS
            routes.MapRoute(
                name: "campogrande/OABMS",
                url: "campogrande/OABMS/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "campogrande/OABMS"
                }
            );


            // Franquia campogrande/campanha/
            routes.MapRoute(
                name: "campogrande/campanha/",
                url: "campogrande/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia campogrande/lista-de-presentes/
            routes.MapRoute(
                name: "campogrande/lista-de-presentes/",
                url: "campogrande/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            // Franquia dwl/grupodwl
            routes.MapRoute(
                name: "dwl/grupodwl",
                url: "dwl/grupodwl/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "dwl/grupodwl"
                }
            );


            // Franquia dwl/campanha/
            routes.MapRoute(
                name: "dwl/campanha/",
                url: "dwl/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia dwl/gestao
            routes.MapRoute(
                name: "dwl/gestao",
                url: "dwl/gestao/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "dwl/gestao"
                }
            );





            // Franquia dwl/famtouralphaville
            routes.MapRoute(
                name: "dwl/famtouralphaville",
                url: "dwl/famtouralphaville/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "dwl/famtouralphaville"
                }
            );





            // Franquia dwl/winners
            routes.MapRoute(
                name: "dwl/winners",
                url: "dwl/winners/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "dwl/winners"
                }
            );





            // Franquia dwl/riohappyhour
            routes.MapRoute(
                name: "dwl/riohappyhour",
                url: "dwl/riohappyhour/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "dwl/riohappyhour"
                }
            );





            // Franquia florianopolis/acate
            routes.MapRoute(
                name: "florianopolis/acate",
                url: "florianopolis/acate/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "florianopolis/acate"
                }
            );


            // Franquia florianopolis/campanha/
            routes.MapRoute(
                name: "florianopolis/campanha/",
                url: "florianopolis/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia florianopolis/lista-de-presentes/
            routes.MapRoute(
                name: "florianopolis/lista-de-presentes/",
                url: "florianopolis/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia itu/grupopetropolis
            routes.MapRoute(
                name: "itu/grupopetropolis",
                url: "itu/grupopetropolis/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "itu/grupopetropolis"
                }
            );


            // Franquia itu/campanha/
            routes.MapRoute(
                name: "itu/campanha/",
                url: "itu/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia itu/lista-de-presentes/
            routes.MapRoute(
                name: "itu/lista-de-presentes/",
                url: "itu/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            // Franquia joinville/sociesc
            routes.MapRoute(
                name: "joinville/sociesc",
                url: "joinville/sociesc/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "joinville/sociesc"
                }
            );


            // Franquia joinville/campanha/
            routes.MapRoute(
                name: "joinville/campanha/",
                url: "joinville/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia joinville/lista-de-presentes/
            routes.MapRoute(
                name: "joinville/lista-de-presentes/",
                url: "joinville/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia joinville/labcat
            routes.MapRoute(
                name: "joinville/labcat",
                url: "joinville/labcat/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "joinville/labcat"
                }
            );





            // Franquia joinville/danica
            routes.MapRoute(
                name: "joinville/danica",
                url: "joinville/danica/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "joinville/danica"
                }
            );





            // Franquia marilia/orthometric
            routes.MapRoute(
                name: "marilia/orthometric",
                url: "marilia/orthometric/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "marilia/orthometric"
                }
            );


            // Franquia marilia/campanha/
            routes.MapRoute(
                name: "marilia/campanha/",
                url: "marilia/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia marilia/lista-de-presentes/
            routes.MapRoute(
                name: "marilia/lista-de-presentes/",
                url: "marilia/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia marilia/abcflytour
            routes.MapRoute(
                name: "marilia/abcflytour",
                url: "marilia/abcflytour/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "marilia/abcflytour"
                }
            );





            // Franquia mogi/lhh
            routes.MapRoute(
                name: "mogi/lhh",
                url: "mogi/lhh/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "mogi/lhh"
                }
            );

            // Franquia mogi/campanha/
            routes.MapRoute(
                name: "mogi/campanha/",
                url: "mogi/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia mogi/lista-de-presentes/
            routes.MapRoute(
                name: "mogi/lista-de-presentes/",
                url: "mogi/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );





            // Franquia natal/congressonacional
            routes.MapRoute(
                name: "natal/congressonacional",
                url: "natal/congressonacional/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "natal/congressonacional"
                }
            );

            // Franquia natal/campanha/
            routes.MapRoute(
                name: "natal/campanha/",
                url: "natal/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia natal/lista-de-presentes/
            routes.MapRoute(
                name: "natal/lista-de-presentes/",
                url: "natal/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia niteroi/pcsf
            routes.MapRoute(
                name: "niteroi/pcsf",
                url: "niteroi/pcsf/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "niteroi/pcsf"
                }
            );


            // Franquia niteroi/campanha/
            routes.MapRoute(
                name: "niteroi/campanha/",
                url: "niteroi/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia niteroi/lista-de-presentes/
            routes.MapRoute(
                name: "niteroi/lista-de-presentes/",
                url: "niteroi/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia niteroi/orgasystems
            routes.MapRoute(
                name: "niteroi/orgasystems",
                url: "niteroi/orgasystems/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "niteroi/orgasystems"
                }
            );





            // Franquia niteroi/n1marketing
            routes.MapRoute(
                name: "niteroi/n1marketing",
                url: "niteroi/n1marketing/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "niteroi/n1marketing"
                }
            );





            // Franquia paulista/condicoesgeraissiamfesp
            routes.MapRoute(
                name: "paulista/condicoesgeraissiamfesp",
                url: "paulista/condicoesgeraissiamfesp/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista/condicoesgeraissiamfesp"
                }
            );


            // Franquia paulista/campanha/
            routes.MapRoute(
                name: "paulista/campanha/",
                url: "paulista/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia paulista/lista-de-presentes/
            routes.MapRoute(
                name: "paulista/lista-de-presentes/",
                url: "paulista/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia paulista/condicoesgeraisgilbarco
            routes.MapRoute(
                name: "paulista/condicoesgeraisgilbarco",
                url: "paulista/condicoesgeraisgilbarco/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista/condicoesgeraisgilbarco"
                }
            );





            // Franquia paulista/condicoesgeraisconveniopuma
            routes.MapRoute(
                name: "paulista/condicoesgeraisconveniopuma",
                url: "bauru/anfarmag2013/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista/condicoesgeraisconveniopuma"
                }
            );





            // Franquia paulista/siamfesp
            routes.MapRoute(
                name: "paulista/siamfesp",
                url: "paulista/siamfesp/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista/siamfesp"
                }
            );





            // Franquia paulista/puma
            routes.MapRoute(
                name: "paulista/puma",
                url: "paulista/puma/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista/puma"
                }
            );





            // Franquia paulista/gilbarco
            routes.MapRoute(
                name: "paulista/gilbarco",
                url: "paulista/gilbarco/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "paulista/gilbarco"
                }
            );





            // Franquia portoalegrecentro/rbs
            routes.MapRoute(
                name: "portoalegrecentro/rbs",
                url: "portoalegrecentro/rbs/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "portoalegrecentro/rbs"
                }
            );


            // Franquia portoalegrecentro/campanha/
            routes.MapRoute(
                name: "portoalegrecentro/campanha/",
                url: "portoalegrecentro/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia portoalegrecentro/lista-de-presentes/
            routes.MapRoute(
                name: "portoalegrecentro/lista-de-presentes/",
                url: "portoalegrecentro/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            // Franquia riobarradatijuca/xp
            routes.MapRoute(
                name: "riobarradatijuca/xp",
                url: "riobarradatijuca/xp/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "riobarradatijuca/xp"
                }
            );


            // Franquia riobarradatijuca/campanha/
            routes.MapRoute(
                name: "riobarradatijuca/campanha/",
                url: "riobarradatijuca/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia riobarradatijuca/lista-de-presentes/
            routes.MapRoute(
                name: "riobarradatijuca/lista-de-presentes/",
                url: "riobarradatijuca/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            // Franquia salvadorbarra/oboticario
            routes.MapRoute(
                name: "salvadorbarra/oboticario",
                url: "salvadorbarra/oboticario/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "salvadorbarra/oboticario"
                }
            );


            // Franquia salvadorbarra/campanha/
            routes.MapRoute(
                name: "salvadorbarra/campanha/",
                url: "salvadorbarra/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia salvadorbarra/lista-de-presentes/
            routes.MapRoute(
                name: "salvadorbarra/lista-de-presentes/",
                url: "salvadorbarra/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            // Franquia portoalegrecentro/arbs
            routes.MapRoute(
                name: "portoalegrecentro/arbs",
                url: "portoalegrecentro/arbs/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "portoalegrecentro/arbs"
                }
            );


            // Franquia salvadorbarra/gdksa
            routes.MapRoute(
                name: "salvadorbarra/gdksa",
                url: "salvadorbarra/gdksa/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "salvadorbarra/gdksa"
                }
            );


            // Franquia santanadeparnaiba/mlgomes
            routes.MapRoute(
                name: "santanadeparnaiba/mlgomes",
                url: "santanadeparnaiba/mlgomes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santanadeparnaiba/mlgomes"
                }
            );


            // Franquia santanadeparnaiba/campanha/
            routes.MapRoute(
                name: "santanadeparnaiba/campanha/",
                url: "santanadeparnaiba/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia santanadeparnaiba/lista-de-presentes/
            routes.MapRoute(
                name: "santanadeparnaiba/lista-de-presentes/",
                url: "santanadeparnaiba/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia santanadeparnaiba/guieluisa
            routes.MapRoute(
                name: "santanadeparnaiba/guieluisa",
                url: "santanadeparnaiba/guieluisa/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santanadeparnaiba/guieluisa"
                }
            );



            // Franquia santanadeparnaiba/flytourcomsantanadeparnaiba
            routes.MapRoute(
                name: "santanadeparnaiba/flytourcomsantanadeparnaiba",
                url: "santanadeparnaiba/flytourcomsantanadeparnaiba/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santanadeparnaiba/flytourcomsantanadeparnaiba"
                }
            );







            // Franquia santoandre/panama
            routes.MapRoute(
                name: "santoandre/panama",
                url: "santoandre/panama/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santoandre/panama"
                }
            );


            // Franquia santoandre/campanha/
            routes.MapRoute(
                name: "santoandre/campanha/",
                url: "santoandre/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia santoandre/lista-de-presentes/
            routes.MapRoute(
                name: "santoandre/lista-de-presentes/",
                url: "santoandre/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );





            // Franquia santoandre/luademel
            routes.MapRoute(
                name: "santoandre/luademel",
                url: "santoandre/luademel/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santoandre/luademel"
                }
            );








            // Franquia santoandre/giorgi
            routes.MapRoute(
                name: "santoandre/giorgi",
                url: "santoandre/giorgi/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santoandre/giorgi"
                }
            );








            // Franquia santoandre/cpfl
            routes.MapRoute(
                name: "santoandre/cpfl",
                url: "santoandre/cpfl/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "santoandre/cpfl"
                }
            );








            // Franquia saojosedoscampos/GM
            routes.MapRoute(
                name: "saojosedoscampos/GM",
                url: "saojosedoscampos/GM/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "saojosedoscampos/GM"
                }
            );


            // Franquia saojosedoscampos/campanha/
            routes.MapRoute(
                name: "saojosedoscampos/campanha/",
                url: "saojosedoscampos/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia saojosedoscampos/lista-de-presentes/
            routes.MapRoute(
                name: "saojosedoscampos/lista-de-presentes/",
                url: "saojosedoscampos/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );





            // Franquia spcampobelo/Sanofi
            routes.MapRoute(
                name: "spcampobelo/Sanofi",
                url: "spcampobelo/Sanofi/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcampobelo/Sanofi"
                }
            );


            // Franquia spcampobelo/campanha/
            routes.MapRoute(
                name: "spcampobelo/campanha/",
                url: "spcampobelo/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia spcampobelo/lista-de-presentes/
            routes.MapRoute(
                name: "spcampobelo/lista-de-presentes/",
                url: "spcampobelo/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );





            // Franquia spcampobelo/dynamus
            routes.MapRoute(
                name: "spcampobelo/dynamus",
                url: "spcampobelo/dynamus/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcampobelo/dynamus"
                }
            );








            // Franquia spcampobelo/cooperfemsa
            routes.MapRoute(
                name: "spcampobelo/cooperfemsa",
                url: "spcampobelo/cooperfemsa/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcampobelo/cooperfemsa"
                }
            );







            // Franquia spcampobelo/ClubeMais
            routes.MapRoute(
                name: "spcampobelo/ClubeMais",
                url: "spcampobelo/ClubeMais/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcampobelo/ClubeMais"
                }
            );








            // Franquia spcampobelo/CECREB
            routes.MapRoute(
                name: "spcampobelo/CECREB",
                url: "spcampobelo/CECREB/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "spcampobelo/CECREB"
                }
            );








            // Franquia sprepublica/astcom
            routes.MapRoute(
                name: "sprepublica/astcom",
                url: "sprepublica/astcom/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "sprepublica/astcom"
                }
            );


            // Franquia spcampobelo/campanha/
            routes.MapRoute(
                name: "sprepublica/campanha/",
                url: "sprepublica/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia sprepublica/lista-de-presentes/
            routes.MapRoute(
                name: "sprepublica/lista-de-presentes/",
                url: "sprepublica/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );





            // Franquia tatuape/gradual
            routes.MapRoute(
                name: "tatuape/gradual",
                url: "tatuape/gradual/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = "tatuape/gradual"
                }
            );


            // Franquia tatuape/campanha/
            routes.MapRoute(
                name: "tatuape/campanha/",
                url: "tatuape/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia tatuape/lista-de-presentes/
            routes.MapRoute(
                name: "tatuape/lista-de-presentes/",
                url: "tatuape/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );



            // Franquia copacabana/lista-de-presentes/
            routes.MapRoute(
                name: "copacabana/lista-de-presentes/",
                url: "copacabana/lista-de-presentes/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );


            // Franquia copacabana/campanha/
            routes.MapRoute(
                name: "copacabana/campanha/",
                url: "copacabana/campanha/{hotsite}/",
                defaults: new
                {
                    controller = "Campanha",
                    action = "Index",
                    hotsite = UrlParameter.Optional
                }
            );




            #endregion


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            #endregion

            routes.MapRoute(
              name: "LoadDetalhePacoteMatriz",
              url: "{controller}/{action}/{id}/{data}/{origem}",
              defaults: new
              {
                  controller = "Pacote",
                  action = "LoadDetalhePacoteMatriz",
                  id = UrlParameter.Optional,
                  data = UrlParameter.Optional,
                  origem = UrlParameter.Optional
              }
            );

            //   routes.MapRoute(
            //    name: "Hotel",
            //    url: "{controller}/{action}/{codigocidade}/{checkinGlobal}/{checkoutGlobal}",
            //    defaults: new
            //    {
            //        controller = "Hotel",
            //        action = "BuscarHotel",
            //        codigocidade = UrlParameter.Optional,
            //        checkinGlobal = UrlParameter.Optional,
            //        checkoutGlobal = UrlParameter.Optional
            //    }
            //);
        }
    }
}