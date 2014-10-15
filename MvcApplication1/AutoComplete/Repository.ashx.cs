using FT.Web.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FT.Web.Site.AutoComplete
{
    /// <summary>
    /// Summary description for Repository
    /// </summary>
    public class Repository : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string searchText = context.Request.QueryString["term"].ToLower();
            searchText = searchText.Split(',')[0].Trim();
            string lista = context.Request.QueryString["lista"];
            List<string> parameters = new List<string>();
            
            foreach(string param in context.Request.QueryString.AllKeys.Where(p => p.StartsWith("id")).ToList())
                parameters.Add(context.Request.QueryString[param]);

            List<DataJson> data = new ListaAutoComplete(searchText).Get(Convert.ToInt32(lista), parameters);


            JavaScriptSerializer serializer = new JavaScriptSerializer();

            context.Response.Write(serializer.Serialize(data));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}