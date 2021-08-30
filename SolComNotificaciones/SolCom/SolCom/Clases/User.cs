using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xamarin.Forms;

namespace App1.Clases
{
    public class User
    {
        public static string sNombre = "";
        public static int iIdUsuario = 0;

        public User()
        {
            if (Application.Current.Properties.ContainsKey("name"))
            {
                var val = Application.Current.Properties["name"];
                sNombre = val.ToString();
            }

            if (Application.Current.Properties.ContainsKey("IdUsuario"))
            {
                var valUsuario = Application.Current.Properties["IdUsuario"];
                iIdUsuario = (int)valUsuario;
            }
        }

        public string sRegresaNombre()
        {
            return sNombre;
        }


        public int iRegresaUsuario()
        {
            return iIdUsuario;
        }

    }

    public class Usuario
    {
        [PrimaryKey]
        public int iIdUsuario { get; set; }
        public string sUsuario { get; set; }
    }

    public class UsuarioJerarquia
    {
        [PrimaryKey]
        public int? iIdUsuario { get; set; }
        public bool? bConsejo { get; set; }
        public bool? bDireccion { get; set; }
        public bool? bGerencia { get; set; }
        public int? iIdEmpresa { get; set; }
        public int? iIdDireccion { get; set; }
        public int? iIdGerencia { get; set; }
        public int? iIdCentro { get; set; }
    }

    public class cUsuarioJerarquia
    {
        public int? iIdUsuario { get; set; }
        public bool? bConsejo { get; set; }
        public bool? bDireccion { get; set; }
        public bool? bGerencia { get; set; }
        public int? iIdEmpresa { get; set; }
        public int? iIdDireccion { get; set; }
        public int? iIdGerencia { get; set; }
        public int? iIdCentro { get; set; }

        public cUsuarioJerarquia(int iIdUsr)
        {
            iIdUsuario = iIdUsr;
        }

        public cUsuarioJerarquia()
        {

        }

            public void CargaJerarquia(DataTable dtJerarquia)
        {
            bConsejo = dtJerarquia.Rows[0].IsNull("Concejo") ? null : (bool?)dtJerarquia.Rows[0]["Concejo"];
            bDireccion = dtJerarquia.Rows[0].IsNull("Direccion") ? null : (bool?)dtJerarquia.Rows[0]["Direccion"];
            bGerencia = dtJerarquia.Rows[0].IsNull("Gerencia") ? null : (bool?)dtJerarquia.Rows[0]["Gerencia"];
            iIdEmpresa = dtJerarquia.Rows[0].IsNull("iIdEmpresa") ? null : (int?)dtJerarquia.Rows[0]["iIdEmpresa"];
            iIdDireccion = dtJerarquia.Rows[0].IsNull("iIdDireccion") ? null : (int?)dtJerarquia.Rows[0]["iIdDireccion"];
            iIdGerencia = dtJerarquia.Rows[0].IsNull("iIdGerencia") ? null : (int?)dtJerarquia.Rows[0]["iIdGerencia"];
            iIdCentro = dtJerarquia.Rows[0].IsNull("iIdCentro") ? null : (int?)dtJerarquia.Rows[0]["iIdCentro"];

        }

    }
}
