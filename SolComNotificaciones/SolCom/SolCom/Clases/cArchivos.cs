using Acr.UserDialogs;
using App1.Clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace SolCom.Clases
{
    public class cArchivos
    {


        public string ProcRecuperaPDF(string iSolicitud, string iCentroAlta, string sDocumento)
        {
            string sRespuesta = string.Empty;
            try
            {
                string sSolicitudDocumento = ApiRoutes.UrlApiSolicitudArchivo(iSolicitud, iCentroAlta, sDocumento);
                WSClass client = new WSClass();
                List<WSClass.cArchivos> result = client.ObtenerArchivos(sSolicitudDocumento);
                if (result != null)
                {
                    foreach (WSClass.cArchivos bitem in result)
                    {
                        MemoryStream stream = new MemoryStream(bitem.btFile);

                        Xamarin.Forms.DependencyService.Get<ISave>().SaveTextAsync(bitem.sNombre + bitem.sTipo, "application/pdf", stream);

                        string srutadoc = Xamarin.Forms.DependencyService.Get<ISave>().sRuta().ToString();
                        sRespuesta = srutadoc + "/" + bitem.sNombre + bitem.sTipo;

                        UserDialogs.Instance.Toast("Archivo descargado correctamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }


            return sRespuesta;
        }

        public string ProcRecuperaRuta()
        {
            string srutadoc = Xamarin.Forms.DependencyService.Get<ISave>().sRuta().ToString();
            return srutadoc;
        }

    }
}
