using App1.Clases;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolCom.Clases
{
    public static class cSessionActiva
    {
        /// Encripta una cadena
        public static bool IsActiva(string sUsuario, string sToken)
        {
            bool result = false;

            string sUser = Seguridad.Encriptar(sUsuario.ToString());
            string sLoginApi = ApiRoutes.UrlSessionValida();
            WSClass client = new WSClass();
            result = client.AutenticarSession(sLoginApi, sUser, sToken);

            return result;
        }
    }
}
