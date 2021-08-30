using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SolCom.Clases
{
    public class cComprador
    {
        public int? iIdFolioPersona { get; set; }
        public string sNombreCompleto { get; set; }
    }
    public class cProveedor
    {
        public int? iIdFolioPersona { get; set; }
        public string sNombreCompleto { get; set; }

    }
    public class cSolicitante
    {
        public int? iIdUsuario { get; set; }
        public string sNombreCompleto { get; set; }
    }

    public class cFiltrosConsulta
    {
        public DataSet dsFiltrosLocal { get; set; }
        public List<Centro> lstCentros { get; set; }
        public List<cComprador> lstCompradores { get; set; }
        public List<cSolicitante> lstSolicitantes { get; set; }
        public List<cProveedor> lstProveedores { get; set; }

        public void CargaFiltros(int? iCentro, int? iGerencia, int? iDireccion)
        {
            DataSet dsFiltros2 = null;
            Stopwatch sw = new Stopwatch();
            if (iCentro == null) iCentro = 0;
            if (iGerencia == null) iGerencia = 0;
            if (iDireccion == null) iDireccion = 0;
            string sUrlFiltros = ApiRoutes.UrlConsultaFiltros();
            //sw.Restart();
            //using (WebClient client = new WebClient())
            //{
            //    client.Proxy = null;
            //    var response = client.DownloadString(sUrlFiltros + "?iIdCentro=" + iCentro.ToString() + "&iIdGerencia=" + iGerencia.ToString() + "&iIdDireccion=" + iDireccion.ToString());
            //    dsFiltros2 = JsonConvert.DeserializeObject<DataSet>(response);
            //}
            //sw.Stop();
            //cLog oLog = new cLog("Respuesta: " + sw.ElapsedMilliseconds.ToString());


            sw.Restart();
            var request = (HttpWebRequest)WebRequest.Create(sUrlFiltros + "?iIdCentro=" + iCentro.ToString() + "&iIdGerencia=" + iGerencia.ToString() + "&iIdDireccion=" + iDireccion.ToString());
            request.Proxy = GlobalProxySelection.GetEmptyWebProxy();
            var response = (HttpWebResponse)request.GetResponse();
            string sRess = "";
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                sRess =  reader.ReadToEnd();
            }
            sw.Stop();
            dsFiltros2 = JsonConvert.DeserializeObject<DataSet>(sRess);
            cLog oLog = new cLog("Respuesta: " + sw.ElapsedMilliseconds.ToString());



            dsFiltrosLocal = dsFiltros2;

        }


    }
}
