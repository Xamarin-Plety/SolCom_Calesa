using System;
using System.Collections.Generic;
using System.Text;

namespace SolCom.Clases
{
    public static class ApiRoutes
    {
        //public static string sUrl = "http://192.168.104.78:49804";
        //public static string sUrl = "http://192.168.104.78/ApiSolCom";
        //public static string sUrl = "http://207.248.62.76/SolComAPI";
        //public static string sUrl = "http://acceso.pastelerialety.com/SolComApi";
        public static string sUrl = "http://acceso.pastelerialety.com/SolComApiDES";

        public static string UrlLogin()
        {
            string sApiBase = sUrl + "/api/Login/Autentificar";
            return sApiBase;
        }
        public static string UrlSessionValida()
        {
            string sApiBase = sUrl + "/api/Login/ValidarSession";
            return sApiBase;
        }

        public static string UrlCentros()
        {
            string sApiBase = sUrl + "/api/Estructuras/GetCentros";
            return sApiBase;
        }

        public static string UrlCompradores()
        {
            string sApiBase = sUrl + "/api/Estructuras/GetCompradores";
            return sApiBase;
        }

        public static string UrlProveedores()
        {
            string sApiBase = sUrl + "/api/Estructuras/GetProveedores";
            return sApiBase;
        }

        public static string UrlSolicitantes()
        {
            string sApiBase = sUrl + "/api/Estructuras/GetSolicitantes";
            return sApiBase;
        }

        public static string UrlConsultaFiltros()
        {
            string sApiBase = sUrl + "/api/Estructuras/GetFiltros";
            return sApiBase;
        }

        public static string UrlJerarquiaUsuario(string siIdUsuario)
        {
            string sApiBase = sUrl + "/api/JerarquiaUsuario/GetJerarquia";
            string sParametros = string.Empty;
            if (siIdUsuario != string.Empty) sParametros += "idUsuario=" + siIdUsuario ;
            return sApiBase + "?" + sParametros;
        }
        
        public static string UrlGuardaToken()
        {
            string sApiBase = sUrl + "/api/Notificaciones/GuardarToken";
            return sApiBase;
        }

        public static string UrlBorraToken()
        {
            string sApiBase = sUrl + "/api/Notificaciones/BorrarToken";
            return sApiBase;
        }

        public static string UrlApiSolicitudesPendientes()
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudesPendientes";
            return sApiBase;
        }

        public static string UrlApiGetSolicitudesPendientes(int? iIdEmpresa, int? iIdDireccion, int? iIdGerencia, bool? bGerencia, bool? bDireccion, bool? bConsejo, DateTime dtFechaAltaIni, DateTime dtFechaAltaFin, int? iIdUsr)
        {
            string sApiBase = sUrl + "/api/Solicitudes/GetSolicitudesPendientes";
            string sParametros = string.Empty;
            sParametros += "iIdEmpresa=" + iIdEmpresa.ToString() + "&";
            sParametros += "iIdDireccion=" + iIdDireccion.ToString() + "&";
            sParametros += "iIdGerencia=" + iIdGerencia.ToString() + "&";
            sParametros += "bGerencia=" + bGerencia.ToString() + "&";
            sParametros += "bDireccion=" + bDireccion.ToString() + "&";
            sParametros += "bConsejo=" + bConsejo.ToString() + "&";
            sParametros += "dtFechaAltaIni=" + dtFechaAltaIni.ToString("MM/dd/yyyy") + "&";
            sParametros += "dtFechaAltaFin=" + dtFechaAltaFin.ToString("MM/dd/yyyy") + "&";
            sParametros += "iIdUsr=" + iIdUsr.ToString() ;
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);
            
            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiGetSolicitudesPendientes(int? iIdEmpresa, int? iIdDireccion, int? iIdGerencia, bool? bGerencia, bool? bDireccion, bool? bConsejo, string dtFechaAltaIni, string dtFechaAltaFin, int? iIdUsr)
        {
            string sApiBase = sUrl + "/api/Solicitudes/GetSolPendientes";
            string sParametros = string.Empty;
            sParametros += "iIdEmpresa=" + iIdEmpresa.ToString() + "&";
            sParametros += "iIdDireccion=" + iIdDireccion.ToString() + "&";
            sParametros += "iIdGerencia=" + iIdGerencia.ToString() + "&";
            sParametros += "bGerencia=" + bGerencia.ToString() + "&";
            sParametros += "bDireccion=" + bDireccion.ToString() + "&";
            sParametros += "bConsejo=" + bConsejo.ToString() + "&";
            sParametros += "dtFechaAltaIni=" + dtFechaAltaIni + "&";
            sParametros += "dtFechaAltaFin=" + dtFechaAltaFin + "&";
            sParametros += "iIdUsr=" + iIdUsr.ToString();
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesPendientes(string sIdSolicitud, string sIdCentro, DateTime dtFechaIni, DateTime dtFechaFin, int iEmpresa, int iDireccion, int iGerencia)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudesPendientes";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (dtFechaIni != null) sParametros += "dtFechaIni=" + dtFechaIni.ToShortDateString() + "&";
            if (dtFechaFin != null) sParametros += "dtFechaFin=" + dtFechaFin.ToShortDateString() + "&";

            if (iEmpresa > 0) sParametros += "iEmpresa=" + iEmpresa + "&";
            if (iDireccion > 0) sParametros += "iDireccion=" + iDireccion + "&";
            if (iGerencia > 0) sParametros += "iGerencia=" + iGerencia + "&";

            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);
            
            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesPendientes(string sIdSolicitud, string sIdCentro, DateTime dtFechaIni, DateTime dtFechaFin,string sIdComprador,string sIdSolicitante, int iEmpresa, int iDireccion, int iGerencia)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudesPendientes";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (dtFechaIni != null) sParametros += "dtFechaIni=" + dtFechaIni.ToShortDateString() + "&";
            if (dtFechaFin != null) sParametros += "dtFechaFin=" + dtFechaFin.ToShortDateString() + "&";

            if (sIdComprador != string.Empty) sParametros += "iIdComprador=" + sIdComprador + "&";
            if (sIdSolicitante != string.Empty) sParametros += "iIdSolicitante=" + sIdSolicitante + "&";
            if (iEmpresa > 0) sParametros += "iEmpresa=" + iEmpresa + "&";
            if (iDireccion > 0) sParametros += "iDireccion=" + iDireccion + "&";
            if (iGerencia > 0) sParametros += "iGerencia=" + iGerencia + "&";

            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesPendientes(string sIdSolicitud, string sIdCentro, DateTime dtFechaIni, DateTime dtFechaFin, string sIdComprador, string sIdProveedor, string sIdSolicitante, int iEmpresa, int iDireccion, int iGerencia)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudesPendientes";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (dtFechaIni != null) sParametros += "dtFechaIni=" + dtFechaIni.ToShortDateString() + "&";
            if (dtFechaFin != null) sParametros += "dtFechaFin=" + dtFechaFin.ToShortDateString() + "&";

            if (sIdComprador != string.Empty) sParametros += "iIdComprador=" + sIdComprador + "&";
            if (sIdProveedor != string.Empty) sParametros += "iIdProveedor=" + sIdProveedor + "&";
            if (sIdSolicitante != string.Empty) sParametros += "iIdSolicitante=" + sIdSolicitante + "&";
            if (iEmpresa > 0) sParametros += "iEmpresa=" + iEmpresa + "&";
            if (iDireccion > 0) sParametros += "iDireccion=" + iDireccion + "&";
            if (iGerencia > 0) sParametros += "iGerencia=" + iGerencia + "&";

            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesPendientes(string sIdSolicitud, string sIdCentro, string dtFechaIni, string dtFechaFin, string sIdComprador, string sIdProveedor, string sIdSolicitante, int iEmpresa, int iDireccion, int iGerencia)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolPendientes";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (dtFechaIni != null) sParametros += "dtFechaIni=" + dtFechaIni + "&";
            if (dtFechaFin != null) sParametros += "dtFechaFin=" + dtFechaFin + "&";

            if (sIdComprador != string.Empty) sParametros += "iIdComprador=" + sIdComprador + "&";
            if (sIdProveedor != string.Empty) sParametros += "iIdProveedor=" + sIdProveedor + "&";
            if (sIdSolicitante != string.Empty) sParametros += "iIdSolicitante=" + sIdSolicitante + "&";
            if (iEmpresa > 0) sParametros += "iEmpresa=" + iEmpresa + "&";
            if (iDireccion > 0) sParametros += "iDireccion=" + iDireccion + "&";
            if (iGerencia > 0) sParametros += "iGerencia=" + iGerencia + "&";

            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesEncabezado(string sIdSolicitud, string sIdCentro)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudEncabezado";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesDetalle(string sIdSolicitud, string sIdCentro)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudDetalle";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);
            
            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesDocumentos(string sIdSolicitud, string sIdCentro)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudDocumentos";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesComentarios(string sIdSolicitud, string sIdCentro)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudComentarios";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesDatos(string sIdSolicitud, string sIdCentro)
        {
            string sApiBase = sUrl + "/api/Solicitudes/SolicitudDatos";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentro=" + sIdCentro + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudesAceptar()
        {
            string sApiBase = sUrl + "/api/Solicitudes/AceptarSolicitud";
            return sApiBase;
        }


        public static string UrlApiSolicitudesRechaza()
        {
            string sApiBase = sUrl + "/api/Solicitudes/RechazarSolicitud";
            return sApiBase;
        }

        public static string UrlApiSolicitudesCancela()
        {
            string sApiBase = sUrl + "/api/Solicitudes/CancelarSolicitud";
            return sApiBase;
        }

        public static string UrlApiSolicitudesArchivos(string sIdSolicitud, string sIdCentro)
        {
            string sApiBase = sUrl + "/api/Archivo/PDFSolicitud";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentroAlta=" + sIdCentro + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }

        public static string UrlApiSolicitudArchivo(string sIdSolicitud, string sIdCentro, string sDocumento)
        {
            string sApiBase = sUrl + "/api/Archivo/PDFSolicitudDocumento";
            string sParametros = string.Empty;
            if (sIdSolicitud != string.Empty) sParametros += "iIdSolicitud=" + sIdSolicitud + "&";
            if (sIdCentro != string.Empty) sParametros += "iIdCentroAlta=" + sIdCentro + "&";
            if (sIdCentro != string.Empty) sParametros += "sDocumento=" + sDocumento + "&";
            if (sParametros.Substring(sParametros.Length - 1, 1) == "&") sParametros = sParametros.Substring(0, sParametros.Length - 1);

            return sApiBase + "?" + sParametros;
        }



    }
}
