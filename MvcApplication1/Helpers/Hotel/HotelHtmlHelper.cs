using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FT.Web.Site.Helpers.Hotel
{
    public class HotelHtmlHelper
    {
        public string GetEstrelas(Decimal? estrelas)
        {
            if (estrelas.HasValue)
            {
                bool isInt = estrelas % 1 == 0;

                if (isInt)
                {
                    return String.Format("<div class='hotel-resultados-estrelas estrelafil{0}'></div>", estrelas);
                }
                else {

                    var meiaEstrela = estrelas.ToString().Replace(",","");
                    return String.Format("<div class='hotel-resultados-estrelas estrelafil{0}' ></div>", meiaEstrela);
                }
            }
            else {
                return "";
            }
        }
    }
}