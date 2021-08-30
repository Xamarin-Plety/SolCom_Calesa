using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Net;
using System.Text;
using App1.Clases;
using Newtonsoft.Json;
using SolCom.Clases;

namespace App1.ViewModels
{
    public class SolicitudViewModel: BaseViewModel
    {
         public RespuestaSolicitudes rSolicitud { get; set; }
        private DataSet dsSolicitudCompra { get; set; }

        public SolicitudViewModel(RespuestaSolicitudes solicitud = null)
        {
            dsSolicitudCompra = new DataSet("dsSolicitudCompra");
            rSolicitud = solicitud;
            /*
            RefreshEncabezado();
            CargaDetalleSolicitud();
            CargaDocumentosSolicitud();
            CargaComentariosSolicitud();
            */
            //CargaDatosSolicitud();
            CargaDatosSolicitudPrueba();
        }

        public void CargaDetalleSolicitud()
        {
            string sUrlDetalle = ApiRoutes.UrlApiSolicitudesDetalle(rSolicitud.iIdSolicitud.ToString(), rSolicitud.iIdCentroAlta.ToString());
            WSClass client = new WSClass();
            CreaEstructura();
            DataTable odtR = client.UGetDataTable(sUrlDetalle);
            if (odtR != null)
            {
                dsSolicitudCompra.Tables["tbl_SolicitudD"].Clear();
                dsSolicitudCompra.Tables["tbl_SolicitudD"].Merge(odtR);
                dsSolicitudCompra.Tables["tbl_SolicitudD"].AcceptChanges();
                List<DetalleSolicitud> nlstDetalle = new List<DetalleSolicitud>();
                foreach (DataRow dr in dsSolicitudCompra.Tables["tbl_SolicitudD"].Rows)
                {
                    DetalleSolicitud nDetalle = new DetalleSolicitud();
                    nDetalle.iPartida = Convert.ToInt32(dr["iPartida"]);
                    nDetalle.iIdMaterial = Convert.ToInt32(dr["iIdMaterial"]);
                    nDetalle.iIdUnidadMedida = Convert.ToInt32(dr["iIdUnidadMedida"]);
                    nDetalle.sDescripcion = dr["sDescripcion"].ToString();
                    nDetalle.sDescCorta = dr["sDesCorta"].ToString();
                    nDetalle.dCantidad = Convert.ToDecimal(dr["dCantidad"]);
                    nDetalle.dPrecioUnitario = Convert.ToDecimal(dr["dPrecioUnitario"]);
                    nDetalle.dTotal = Convert.ToDecimal(dr["dTotal"]);
                    nDetalle.sCeCo = dr["sCeCoDescripcion"].ToString();
                    nDetalle.sCuentaContable = dr["sCuentaContableDescripcion"].ToString();
                    nDetalle.sActivoFijo = dr["sActivoFijo"].ToString();
                    nDetalle.sRetencion = dr["sRetencionDescripcion"].ToString();
                    nDetalle.sUnidadMedida = dr["sUnidadMedida"].ToString();
                    nDetalle.sTipoMoneda = rSolicitud.sTipoMoneda;

                    nlstDetalle.Add(nDetalle);
                }
                rSolicitud.lstDetalleSolicitud = nlstDetalle;
            }
        }

        public void CargaDocumentosSolicitud()
        {
            string sUrlDetalle = ApiRoutes.UrlApiSolicitudesDocumentos(rSolicitud.iIdSolicitud.ToString(), rSolicitud.iIdCentroAlta.ToString());
            WSClass client = new WSClass();
            CreaEstructuraDocumento();
            DataTable odtR = client.UGetDataTable(sUrlDetalle);
            if (odtR != null)
            {
                dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].Clear();
                dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].Merge(odtR);
                dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].AcceptChanges();
                List<DocumentosSolicitud> nlstDocumentos = new List<DocumentosSolicitud>();
                foreach (DataRow dr in dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].Rows)
                {
                    DocumentosSolicitud nDocumento = new DocumentosSolicitud();
                    nDocumento.iIdCentroAlta = Convert.ToInt32(dr["iIdCentroAlta"]);
                    nDocumento.iIdSolicitud = Convert.ToInt32(dr["iIdSolicitud"]);
                    nDocumento.sNombreArchivo = dr["sNombreArchivo"].ToString();
                    nDocumento.sRuta = dr["sRuta"].ToString();
                    nDocumento.dtFechaAlta = DateTime.Now;

                    nlstDocumentos.Add(nDocumento);
                }
                rSolicitud.lstDocumentosSolicitud = nlstDocumentos;
            }
        }

        public void CargaComentariosSolicitud()
        {
            string sUrlDetalle = ApiRoutes.UrlApiSolicitudesComentarios(rSolicitud.iIdSolicitud.ToString(), rSolicitud.iIdCentroAlta.ToString());
            WSClass client = new WSClass();
            CreaEstructuraComentarios();
            DataTable odtR = client.UGetDataTable(sUrlDetalle);
            if (odtR != null)
            {
                dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].Clear();
                dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].Merge(odtR);
                dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].AcceptChanges();
                List<ComentariosSolicitud> nlstComentarios = new List<ComentariosSolicitud>();
                foreach (DataRow dr in dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].Rows)
                {
                    ComentariosSolicitud nComentario = new ComentariosSolicitud();
                    nComentario.iIdCentroAlta = Convert.ToInt32(dr["iIdCentroAlta"]);
                    nComentario.iIdSolicitud = Convert.ToInt32(dr["iIdSolicitud"]);
                    nComentario.iPosicion = Convert.ToInt32(dr["iPosicion"]);
                    nComentario.sComentario = dr["sComentario"].ToString();
                    nComentario.dtFecha = Convert.ToDateTime(dr["dtFecha"]);
                    nComentario.sUsuario = dr["sUsuario"].ToString();
                    nlstComentarios.Add(nComentario);
                }
                rSolicitud.lstComenatriosSolicitud = nlstComentarios;
            }
        }

        public void RefreshEncabezado()
        {
            string sUrlEncabezado = ApiRoutes.UrlApiSolicitudesEncabezado(rSolicitud.iIdSolicitud.ToString(), rSolicitud.iIdCentroAlta.ToString());
            WSClass client = new WSClass();
            DataTable odtR = client.UGetDataTable(sUrlEncabezado);
            if (odtR != null)
            {
                foreach (DataRow dr in odtR.Rows)
                {
                    rSolicitud.iIdEstatus = Convert.ToInt32(dr["iIdEstatus"]);
                    rSolicitud.sEstatus = dr["sEstatus"].ToString();
                    rSolicitud.Asignada = dr["Asignada"].ToString();
                    rSolicitud.iIdOC = Convert.ToInt32(dr["iIdOC"]);
                    rSolicitud.iIdEntrada = Convert.ToInt32(dr["iIdEntrada"]);
                    rSolicitud.iIdFactura = Convert.ToInt32(dr["iIdFactura"]);
                    rSolicitud.sCeCo = dr["sCeCoDescripcion"].ToString();
                    rSolicitud.sCuentaContable = dr["sCuentaContableDescripcion"].ToString();
                    rSolicitud.sEmpresa = dr["sEmpresa"].ToString();
                    rSolicitud.sPersonaProvSug = dr["sPersonaProvSug"].ToString();
                    rSolicitud.sTipoMoneda = dr["sMoneda"].ToString();
                }
            }
        }

        public void CargaDatosSolicitud()
        {
            string sUrlDatos = ApiRoutes.UrlApiSolicitudesDatos(rSolicitud.iIdSolicitud.ToString(), rSolicitud.iIdCentroAlta.ToString());
            WSClass client = new WSClass();
            DataSet dsDatos = client.UGetDataSet(sUrlDatos);

            DataTable dtEncabezado = dsDatos.Tables[0];
            DataTable dtDetalle = dsDatos.Tables[1];
            DataTable dtDocumentos = dsDatos.Tables[2];
            DataTable dtComentarios = dsDatos.Tables[3];

            if (dtEncabezado != null)
            {
                foreach (DataRow dr in dtEncabezado.Rows)
                {
                    rSolicitud.iIdEstatus = Convert.ToInt32(dr["iIdEstatus"]);
                    rSolicitud.sEstatus = dr["sEstatus"].ToString();
                    rSolicitud.Asignada = dr["Asignada"].ToString();
                    rSolicitud.iIdOC = Convert.ToInt32(dr["iIdOC"]);
                    rSolicitud.iIdEntrada = Convert.ToInt32(dr["iIdEntrada"]);
                    rSolicitud.iIdFactura = Convert.ToInt32(dr["iIdFactura"]);
                    rSolicitud.sCeCo = dr["sCeCoDescripcion"].ToString();
                    rSolicitud.sCuentaContable = dr["sCuentaContableDescripcion"].ToString();
                    rSolicitud.sEmpresa = dr["sEmpresa"].ToString();
                    rSolicitud.sPersonaProvSug = dr["sPersonaProvSug"].ToString();
                    rSolicitud.sTipoMoneda = dr["sMoneda"].ToString();
                }
            }

            if (dtDetalle != null)
            {
                CreaEstructura();
                dsSolicitudCompra.Tables["tbl_SolicitudD"].Clear();
                dsSolicitudCompra.Tables["tbl_SolicitudD"].Merge(dtDetalle);
                dsSolicitudCompra.Tables["tbl_SolicitudD"].AcceptChanges();
                List<DetalleSolicitud> nlstDetalle = new List<DetalleSolicitud>();
                foreach (DataRow dr in dsSolicitudCompra.Tables["tbl_SolicitudD"].Rows)
                {
                    DetalleSolicitud nDetalle = new DetalleSolicitud();
                    nDetalle.iPartida = Convert.ToInt32(dr["iPartida"]);
                    nDetalle.iIdMaterial = Convert.ToInt32(dr["iIdMaterial"]);
                    nDetalle.iIdUnidadMedida = Convert.ToInt32(dr["iIdUnidadMedida"]);
                    nDetalle.sDescripcion = dr["sDescripcion"].ToString();
                    nDetalle.sDescCorta = dr["sDesCorta"].ToString();
                    nDetalle.dCantidad = Convert.ToDecimal(dr["dCantidad"]);
                    nDetalle.dPrecioUnitario = Convert.ToDecimal(dr["dPrecioUnitario"]);
                    nDetalle.dTotal = Convert.ToDecimal(dr["dTotal"]);
                    nDetalle.sCeCo = dr["sCeCoDescripcion"].ToString();
                    nDetalle.sCuentaContable = dr["sCuentaContableDescripcion"].ToString();
                    nDetalle.sActivoFijo = dr["sActivoFijo"].ToString();
                    nDetalle.sRetencion = dr["sRetencionDescripcion"].ToString();
                    nDetalle.sUnidadMedida = dr["sUnidadMedida"].ToString();
                    nDetalle.sTipoMoneda = rSolicitud.sTipoMoneda;

                    nlstDetalle.Add(nDetalle);
                }
                rSolicitud.lstDetalleSolicitud = nlstDetalle;
            }

            if (dtDocumentos != null)
            {
                CreaEstructuraDocumento();
                dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].Clear();
                dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].Merge(dtDocumentos);
                dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].AcceptChanges();
                List<DocumentosSolicitud> nlstDocumentos = new List<DocumentosSolicitud>();
                foreach (DataRow dr in dsSolicitudCompra.Tables["tbl_SolicitudDocumento"].Rows)
                {
                    DocumentosSolicitud nDocumento = new DocumentosSolicitud();
                    nDocumento.iIdCentroAlta = Convert.ToInt32(dr["iIdCentroAlta"]);
                    nDocumento.iIdSolicitud = Convert.ToInt32(dr["iIdSolicitud"]);
                    nDocumento.sNombreArchivo = dr["sNombreArchivo"].ToString();
                    nDocumento.sRuta = dr["sRuta"].ToString();
                    nDocumento.dtFechaAlta = DateTime.Now;

                    nlstDocumentos.Add(nDocumento);
                }
                rSolicitud.lstDocumentosSolicitud = nlstDocumentos;
            }

            if (dtComentarios != null)
            {
                CreaEstructuraComentarios();
                dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].Clear();
                dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].Merge(dtComentarios);
                dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].AcceptChanges();
                List<ComentariosSolicitud> nlstComentarios = new List<ComentariosSolicitud>();
                foreach (DataRow dr in dsSolicitudCompra.Tables["tbl_SolicitudComentarios"].Rows)
                {
                    ComentariosSolicitud nComentario = new ComentariosSolicitud();
                    nComentario.iIdCentroAlta = Convert.ToInt32(dr["iIdCentroAlta"]);
                    nComentario.iIdSolicitud = Convert.ToInt32(dr["iIdSolicitud"]);
                    nComentario.iPosicion = Convert.ToInt32(dr["iPosicion"]);
                    nComentario.sComentario = dr["sComentario"].ToString();
                    nComentario.dtFecha = Convert.ToDateTime(dr["dtFecha"]);
                    nComentario.sUsuario = dr["sUsuario"].ToString();
                    nlstComentarios.Add(nComentario);
                }
                rSolicitud.lstComenatriosSolicitud = nlstComentarios;
            }
        }


        public void CargaDatosSolicitudPrueba()
        {
            string sUrlDatos = ApiRoutes.UrlApiSolicitudesDatos(rSolicitud.iIdSolicitud.ToString(), rSolicitud.iIdCentroAlta.ToString());
            WSClass client = new WSClass();
            DataSet dsDatos = client.UGetDataSet(sUrlDatos);

            DataTable dtEncabezado = dsDatos.Tables[0];
            DataTable dtDetalle = dsDatos.Tables[1];
            DataTable dtDocumentos = dsDatos.Tables[2];
            DataTable dtComentarios = dsDatos.Tables[3];

            if (dtEncabezado != null)
            {
                foreach (DataRow dr in dtEncabezado.Rows)
                {
                    rSolicitud.dtFechaAlta = (DateTime)dr["dtFechaAlta"];
                    rSolicitud.dtFechaEntrega = (DateTime)dr["dtFechaEntrega"];
                    rSolicitud.iIdEstatus = Convert.ToInt32(dr["iIdEstatus"]);
                    rSolicitud.sEstatus = dr["sEstatus"].ToString();
                    rSolicitud.Asignada = dr["Asignada"].ToString();
                    rSolicitud.iIdOC = Convert.ToInt32(dr["iIdOC"]);
                    rSolicitud.iIdEntrada = Convert.ToInt32(dr["iIdEntrada"]);
                    rSolicitud.iIdFactura = Convert.ToInt32(dr["iIdFactura"]);
                    rSolicitud.sCeCo = dr["sCeCoDescripcion"].ToString();
                    rSolicitud.sCuentaContable = dr["sCuentaContableDescripcion"].ToString();
                    rSolicitud.sEmpresa = dr["sEmpresa"].ToString();
                    rSolicitud.sPersonaProvSug = dr["sPersonaProvSug"].ToString();
                    rSolicitud.sTipoMoneda = dr["sMoneda"].ToString();
                }
            }

            if (dtDetalle != null)
            {

                List<DetalleSolicitud> nlstDetalle = new List<DetalleSolicitud>();
                foreach (DataRow dr in dtDetalle.Rows)
                {
                    DetalleSolicitud nDetalle = new DetalleSolicitud();
                    nDetalle.iPartida = Convert.ToInt32(dr["iPartida"]);
                    nDetalle.iIdMaterial = Convert.ToInt32(dr["iIdMaterial"]);
                    nDetalle.iIdUnidadMedida = Convert.ToInt32(dr["iIdUnidadMedida"]);
                    nDetalle.sDescripcion = dr["sDescripcion"].ToString();
                    nDetalle.sDescCorta = dr["sDesCorta"].ToString();
                    nDetalle.dCantidad = Convert.ToDecimal(dr["dCantidad"]);
                    nDetalle.dPrecioUnitario = Convert.ToDecimal(dr["dPrecioUnitario"]);
                    nDetalle.dTotal = Convert.ToDecimal(dr["dTotal"]);
                    nDetalle.sCeCo = dr["sCeCoDescripcion"].ToString();
                    nDetalle.sCuentaContable = dr["sCuentaContableDescripcion"].ToString();
                    nDetalle.sActivoFijo = dr["sActivoFijo"].ToString();
                    nDetalle.sRetencion = dr["sRetencionDescripcion"].ToString();
                    nDetalle.sUnidadMedida = dr["sUnidadMedida"].ToString();
                    nDetalle.sTipoMoneda = rSolicitud.sTipoMoneda;

                    nlstDetalle.Add(nDetalle);
                }
                rSolicitud.lstDetalleSolicitud = nlstDetalle;
            }

            if (dtDocumentos != null)
            {

                List<DocumentosSolicitud> nlstDocumentos = new List<DocumentosSolicitud>();
                foreach (DataRow dr in dtDocumentos.Rows)
                {
                    DocumentosSolicitud nDocumento = new DocumentosSolicitud();
                    nDocumento.iIdCentroAlta = Convert.ToInt32(dr["iIdCentroAlta"]);
                    nDocumento.iIdSolicitud = Convert.ToInt32(dr["iIdSolicitud"]);
                    nDocumento.sNombreArchivo = dr["sNombreArchivo"].ToString();
                    nDocumento.sRuta = dr["sRuta"].ToString();
                    nDocumento.dtFechaAlta = DateTime.Now;

                    nlstDocumentos.Add(nDocumento);
                }
                rSolicitud.lstDocumentosSolicitud = nlstDocumentos;
            }

            if (dtComentarios != null)
            {

                List<ComentariosSolicitud> nlstComentarios = new List<ComentariosSolicitud>();
                foreach (DataRow dr in dtComentarios.Rows)
                {
                    ComentariosSolicitud nComentario = new ComentariosSolicitud();
                    nComentario.iIdCentroAlta = Convert.ToInt32(dr["iIdCentroAlta"]);
                    nComentario.iIdSolicitud = Convert.ToInt32(dr["iIdSolicitud"]);
                    nComentario.iPosicion = Convert.ToInt32(dr["iPosicion"]);
                    nComentario.sComentario = dr["sComentario"].ToString();
                    nComentario.dtFecha = Convert.ToDateTime(dr["dtFecha"]);
                    nComentario.sUsuario = dr["sUsuario"].ToString();
                    nlstComentarios.Add(nComentario);
                }
                rSolicitud.lstComenatriosSolicitud = nlstComentarios;
            }
        }
        public void CreaEstructura()
        {
            if (!dsSolicitudCompra.Tables.Contains("tbl_SolicitudD"))
            {
                DataTable dtResult = new DataTable("tbl_SolicitudD");
                dtResult.Columns.Add("iIdCentroAlta", typeof(System.Int64));
                dtResult.Columns.Add("iIdSolicitud", typeof(System.Int64));
                dtResult.Columns.Add("iPartida", typeof(System.Int64));
                dtResult.Columns.Add("iIdMaterial", typeof(System.Int64));
                dtResult.Columns.Add("iIdUnidadMedida", typeof(System.Int64));
                dtResult.Columns.Add("sDescripcion", typeof(System.String));
                dtResult.Columns.Add("sDesCorta", typeof(System.String));
                dtResult.Columns.Add("dCantidad", typeof(System.Double));
                dtResult.Columns.Add("dPrecioUnitario", typeof(System.Double));
                dtResult.Columns.Add("dTotal", typeof(System.Double));
                dtResult.Columns.Add("sCeCo", typeof(System.String));
                dtResult.Columns.Add("sCuentaContable", typeof(System.String));
                dtResult.Columns.Add("dtFechaAlta", typeof(System.DateTime));
                dtResult.Columns.Add("iIdUsuarioAlta", typeof(System.Int64));
                dtResult.Columns.Add("dtFechaModifica", typeof(System.DateTime));
                dtResult.Columns.Add("iIdUsuarioModifica", typeof(System.Int64));
                dsSolicitudCompra.Tables.Add(dtResult);
            }
        }
        public void CreaEstructuraDocumento()
        {
            if (!dsSolicitudCompra.Tables.Contains("tbl_SolicitudDocumento"))
            {
                DataTable dt = new DataTable("tbl_SolicitudDocumento");
                dt.Columns.Add("iIdCentroAlta", typeof(System.Int64));
                dt.Columns.Add("iIdSolicitud", typeof(System.Int64));
                dt.Columns.Add("iIdDocumento", typeof(System.Int64));
                dt.Columns.Add("bIndCompras", typeof(System.Boolean));
                dt.Columns.Add("sNombreArchivo", typeof(System.String));
                dt.Columns.Add("sRuta", typeof(System.String));
                dt.Columns.Add("sTipoDocumento", typeof(System.String));
                dt.Columns.Add("dtFechaAlta", typeof(System.DateTime));
                dt.Columns.Add("iIdUsuarioAlta", typeof(System.Int64));
                dsSolicitudCompra.Tables.Add(dt);
            }
        }
        public void CreaEstructuraComentarios()
        {
            if (!dsSolicitudCompra.Tables.Contains("tbl_SolicitudComentarios"))
            {
                DataTable dt = new DataTable("tbl_SolicitudComentarios");
                dt.Columns.Add("iIdCentroAlta", typeof(System.Int64));
                dt.Columns.Add("iIdSolicitud", typeof(System.Int64));
                dt.Columns.Add("iPosicion", typeof(System.Int64));
                dt.Columns.Add("iIdUsuario", typeof(System.Int64));
                dt.Columns.Add("sComentario", typeof(System.String));
                dt.Columns.Add("dtFecha", typeof(System.DateTime));
                dt.Columns.Add("sUsuario", typeof(System.String));
                dsSolicitudCompra.Tables.Add(dt);
            }
        }

        public bool AceptaSolicitud(int iIdUsuario, int? iIdGerencia, bool? bIndGerencia, bool? bIndDireccion, bool? bIndConcejo)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlApiSolicitudesAceptar();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdSolicitud", rSolicitud.iIdSolicitud.ToString()},
                   { "iIdCentro", rSolicitud.iIdCentroAlta.ToString()},
                   { "iIdUsuario", iIdUsuario.ToString() },
                   { "iIdGerencia", iIdGerencia.ToString() },
                   { "bIndGerencia", bIndGerencia.Value.ToString() },
                   { "bIndDireccion", bIndDireccion.Value.ToString() },
                   { "bIndConcejo", bIndConcejo.Value.ToString() }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }

        public bool RechazaSolicitud(int iIdUsuario, string sComentario)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlApiSolicitudesRechaza();
            using (WebClient client = new WebClient())
            { 
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdSolicitud", rSolicitud.iIdSolicitud.ToString() },
                   { "iIdCentro", rSolicitud.iIdCentroAlta.ToString() },
                   { "iIdUsuario", iIdUsuario.ToString() },
                   { "sComentario", sComentario }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }

        public bool CancelaSolicitud( int iIdUsuario, string sComentario)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlApiSolicitudesCancela();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdSolicitud", rSolicitud.iIdSolicitud.ToString() },
                   { "iIdCentro", rSolicitud.iIdCentroAlta.ToString() },
                   { "iIdUsuario", iIdUsuario.ToString() },
                   { "sComentario", sComentario }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }


    }

    public class cAccionesSolicitud
    {
        public bool AceptaSolicitud(int iIdSolicitud, int iIdCentroAlta,int iIdUsuario, int? iIdGerencia, bool? bIndGerencia, bool? bIndDireccion, bool? bIndConcejo)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlApiSolicitudesAceptar();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdSolicitud", iIdSolicitud.ToString()},
                   { "iIdCentro", iIdCentroAlta.ToString()},
                   { "iIdUsuario", iIdUsuario.ToString() },
                   { "iIdGerencia", iIdGerencia.ToString() },
                   { "bIndGerencia", bIndGerencia.Value.ToString() },
                   { "bIndDireccion", bIndDireccion.Value.ToString() },
                   { "bIndConcejo", bIndConcejo.Value.ToString() }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }

    }
}
