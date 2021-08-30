using App1.Clases;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SolCom.Clases
{
    public class Centro
    {
        public int iIdCentro { get; set; }
        public string sNombre { get; set; }
    }

    public class Centros
    {
        public List<Centro> lstCentro { get; set; }

        public void CargaCentros()
        {
            List<Centro> nlstCentros = new List<Centro>();

            string sUrlCentros = ApiRoutes.UrlCentros();
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(sUrlCentros + "?iIdCentro=0&iIdGerencia=0&iIdDireccion=0");

                nlstCentros = JsonConvert.DeserializeObject<List<Centro>>(response);
            }

            this.lstCentro = nlstCentros;
        }


        public void CargaCentros(int? iCentro, int? iGerencia, int? iDireccion)
        {
            List<Centro> nlstCentros = new List<Centro>();
            if (iCentro == null) iCentro = 0;
            if (iGerencia == null) iGerencia = 0;
            if (iDireccion == null) iDireccion = 0;
            string sUrlCentros = ApiRoutes.UrlCentros();
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(sUrlCentros + "?iIdCentro="+iCentro.ToString()+"&iIdGerencia="+iGerencia.ToString()+"&iIdDireccion="+ iDireccion.ToString());

                nlstCentros = JsonConvert.DeserializeObject<List<Centro>>(response);
            }

            this.lstCentro = nlstCentros;
        }

    }
}
