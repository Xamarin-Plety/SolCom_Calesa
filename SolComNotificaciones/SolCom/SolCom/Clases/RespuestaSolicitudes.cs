using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace App1.Clases
{
    public class RespuestaSolicitudes
    {
        public int iIdCentroAlta { get; set; }
        public string sCentroAlta { get; set; }
        public int iIdSolicitud { get; set; }
        public string sRefExt { get; set; }
        public int iIdCentroSolicita { get; set; }
        public string sCentroSolicita { get; set; }
        public bool bIndUrgente { get; set; }
        public int? iIdFolioPersonaProvSug { get; set; }
        public int iIdUsuarioEjecutivo { get; set; }
        public string sCeCo { get; set; }
        public string sCuentaContable { get; set; }
        public int iIdJustificacion { get; set; }
        public string sIdJustificacion { get; set; }
        public string sJustificacion { get; set; }
        public DateTime dtFechaEntrega { get; set; }
        public int iIdTipoMoneda { get; set; }
        public int iIdFolioPersonaSolicita { get; set; }
        public int iIdEstatus { get; set; }
        public string sEstatus { get; set; }
        public DateTime dtFechaAlta { get; set; }
        public DateTime? dtFechaModifica { get; set; }
        public int? iIdUsuarioModifica { get; set; }
        public decimal dTotal { get; set; }
        public List<DetalleSolicitud> lstDetalleSolicitud { get; set; }
        public List<DocumentosSolicitud> lstDocumentosSolicitud { get; set; }
        public List<ComentariosSolicitud> lstComenatriosSolicitud { get; set; }
        public Color bgColor { get; set; }
        public Color fntColor { get; set; }
        public string sSolicitante { get; set; }
        public string sEmpresa { get; set; }
        public string Asignada { get; set; }
        public int iIdOC { get; set; }
        public int iIdEntrada { get; set; }
        public int iIdFactura { get; set; }
        public string sPersonaProvSug { get; set; }
        public string sTipoMoneda { get; set; }
    }

    public class DetalleSolicitud
    {
        public int iPartida { get; set; }
        public int iIdMaterial { get; set; }
        public int iIdUnidadMedida { get; set; }
        public string sDescripcion { get; set; }
        public string sDescCorta { get; set; }
        public decimal dCantidad { get; set; }
        public decimal dPrecioUnitario { get; set; }
        public decimal dTotal { get; set; }
        public string sCeCo { get; set; }
        public string sCuentaContable { get; set; }
        public DateTime dtFechaAlta { get; set; }
        public string sActivoFijo { get; set; }
        public string sRetencion { get; set; }
        public string sUnidadMedida { get; set; }
        public string sTipoMoneda { get; set; }
    }

    public class DocumentosSolicitud
    {
        public int iIdCentroAlta { get; set; }
        public int iIdSolicitud { get; set; }
        public int iIdDocumento { get; set; }
        public string sNombreArchivo { get; set; }
        public string sRuta { get; set; }
        public DateTime dtFechaAlta { get; set; }
    }

    public class ComentariosSolicitud
    {
        public int iIdCentroAlta { get; set; }
        public int iIdSolicitud { get; set; }
        public int iPosicion { get; set; }
        public int iIdUsuario { get; set; }
        public string sComentario { get; set; }
        public DateTime dtFecha { get; set; }
        public string sUsuario { get; set; }
    }

    public class RespuestaSolicitud
    {
        public List<RespuestaSolicitudes> lstRespuesta { get; set; }
    }


    public class AccionSolicitud
    {
        [PrimaryKey, AutoIncrement]
        public int iId { get; set; }
        public int iIdSolicitud { get; set; }
        public int iIdCentro { get; set; }
        public int iIdUsuario { get; set; }
        public int iIdGerencia { get; set; }
        public bool? bIndGerencia { get; set; }
        public bool? bIndDireccion { get; set; }
        public bool? bIndConcejo { get; set; }
        public bool bAcepta { get; set; }
        public bool bRechaza { get; set; }
        public bool bCancela { get; set; }
    }
}
