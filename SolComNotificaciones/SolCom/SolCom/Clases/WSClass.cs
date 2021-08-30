using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Data;
using SolCom.Clases;
using System.Collections.ObjectModel;

namespace App1.Clases
{
    public class WSClass
    {
        public class cLogin
        {
            public string vUser { get; set; }
            public string vPassword { get; set; }
        }

        public class cUsuario
        {
            public int iIdUsuario { get; set; }
            public int iIdPersona { get; set; }
            public string sUsuario { get; set; }
            public string sNombre { get; set; }
        }

        public class cArchivos
        {
            public string sNombre { get; set; }
            public byte[] btFile { get; set; }
            public string sTipo { get; set; }
        }

        public DataTable UGetDataTable(string url)
        {
            HttpClient client = new HttpClient();

            var response =  client.GetAsync(url);
            var json = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        public DataSet UGetDataSet(string url)
        {
            HttpClient client = new HttpClient();

            var response = client.GetAsync(url);
            var json = response.Result.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<DataSet>(json);
        }

        public List<cArchivos> ObtenerArchivos(string url)
        {
            List<cArchivos> result = null;
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(url);

                result = JsonConvert.DeserializeObject<List<cArchivos>> (response);
            }

            return result;
        }


        public async Task<T> Obtener<T>(string url)
        {
            System.Net.HttpWebRequest.DefaultWebProxy = null;
            HttpClient client = new HttpClient();

            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        public async Task<string> ObtenerStringFiltro<T>(string url)
        {
            System.Net.HttpWebRequest.DefaultWebProxy = null;
            string result = "";
            var request = WebRequest.Create(url);
            request.Proxy = null;
            var response = await request.GetResponseAsync();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
        public async Task<string> ObtenerString<T>(string url)
        {
            System.Net.HttpWebRequest.DefaultWebProxy = null;
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return json;
        }

        public async Task<List<T>> Get<T>(string url)
        {
            System.Net.HttpWebRequest.DefaultWebProxy = null;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(200);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
            RespuestaSolicitudes rSolicitud = new RespuestaSolicitudes();
            List<RespuestaSolicitudes> lstR = new List<RespuestaSolicitudes>
            {
                rSolicitud
            };
            var njson = JsonConvert.SerializeObject(lstR);
            return JsonConvert.DeserializeObject<List<T>>(njson);
        }
        public async Task<List<T>> GetS<T>(string url)
        {
            System.Net.HttpWebRequest.DefaultWebProxy = null;
            List<T> oList = new List<T>();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromSeconds(200);
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(json);
                }
            }

            return oList;
        }

        public async Task<ObservableCollection<T>> GetObs<T>(string url)
        {
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(200);
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ObservableCollection<T>>(json);
            }
            RespuestaSolicitudes rSolicitud = new RespuestaSolicitudes();
            List<RespuestaSolicitudes> lstR = new List<RespuestaSolicitudes>
            {
                rSolicitud
            };
            var njson = JsonConvert.SerializeObject(lstR);
            return JsonConvert.DeserializeObject<ObservableCollection<T>>(njson);
        }

        public cUsuario AutenticarLogin(string url, string sUser, string sPass)
        {
            System.Net.HttpWebRequest.DefaultWebProxy = null;
            cUsuario result = null;
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(url, new NameValueCollection()
                {
                   { "vUser", sUser },
                   { "vPassword", sPass }
                });

                result = JsonConvert.DeserializeObject<cUsuario>(Encoding.UTF8.GetString(response));
            }

            return result;
        }

        public bool AutenticarSession(string url, string iUser, string sToken)
        {
            bool result = false;
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(url, new NameValueCollection()
                {
                   { "vUser", iUser },
                   { "vToken", sToken }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }


        public cUsuarioJerarquia ObtenerJerarquia(string url)
        {
            cUsuarioJerarquia result = null;
            using (WebClient client = new WebClient())
            {
                string response = client.DownloadString(url);

                result = JsonConvert.DeserializeObject<cUsuarioJerarquia>(response);
            }

            return result;
        }

        public bool GuardaToken(int iIdUsuario, string sUsuario, string sToken, string sDispositivo, string sPlataforma)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlGuardaToken();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdUsuario", iIdUsuario.ToString() },
                   { "sUsuario", sUsuario },
                   { "sToken", sToken },
                   { "sDispositivo", sDispositivo },
                   { "sPlataforma", sPlataforma }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }


        public bool BorraToken(int iIdUsuario, string sUsuario, string sToken, string sDispositivo, string sPlataforma)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlBorraToken();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdUsuario", iIdUsuario.ToString() },
                   { "sUsuario", sUsuario },
                   { "sToken", sToken },
                   { "sDispositivo", sDispositivo },
                   { "sPlataforma", sPlataforma }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }




        public bool IntentaRechazaSolicitud(int iIdSolicitud, int iIdCentroAlta, int iIdUsuario)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlApiSolicitudesRechaza();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdSolicitud", iIdSolicitud.ToString() },
                   { "iIdCentro", iIdCentroAlta.ToString() },
                   { "iIdUsuario", iIdUsuario.ToString() }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }

        public bool IntentaCancelaSolicitud(int iIdSolicitud, int iIdCentroAlta, int iIdUsuario)
        {
            bool result = false;
            string sUrlCancela = ApiRoutes.UrlApiSolicitudesCancela();
            using (WebClient client = new WebClient())
            {
                byte[] response =
                client.UploadValues(sUrlCancela, new NameValueCollection()
                {
                   { "iIdSolicitud", iIdSolicitud.ToString() },
                   { "iIdCentro", iIdCentroAlta.ToString() },
                   { "iIdUsuario", iIdUsuario.ToString() }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }



        public bool IntentaAceptaSolicitud(int iIdSolicitud, int iIdCentroAlta, int iIdUsuario, int? iIdGerencia, bool? bIndGerencia, bool? bIndDireccion, bool? bIndConcejo)
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
                   { "bIndGerencia", bIndGerencia.ToString() },
                   { "bIndDireccion", bIndDireccion.ToString() },
                   { "bIndConcejo", bIndConcejo.ToString() }
                });

                result = JsonConvert.DeserializeObject<bool>(Encoding.UTF8.GetString(response));
            }

            return result;
        }

    }

}
