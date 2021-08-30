using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Views;
using App1.Clases;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using App1.ViewModels;
using SolCom.Clases;
using Xamarin.Essentials;
using Acr.UserDialogs;
using System.Data;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SolCom
{
    public partial class App : Application, ILoginManager
    {
        public EventHandler OnResumeHandler;

        static ILoginManager loginManager;
        public static App Current;
        public Int32 vDato = 0;
        public static cUsuarioJerarquia _JerarquiaUsuario;
        public NavigationPage navPage;
        public static SolComDatabase database;
        public DataSet dsFiltrosV;

        //public static SolComDatabase Database
        //{
        //    get
        //    {
        //        if (database == null)
        //        {
        //            database = new SolComDatabase();
        //        }
        //        return database;
        //    }
        //}

        public App()
        {
            InitializeComponent();

            try
            {
                string sTiempo = "Inicia : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                Current = this;
                BindingContext = new BaseViewModel();

                if (DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    bool bLogged = Application.Current.Properties.ContainsKey("IsLoggedIn");
                    bool bUsuario = Application.Current.Properties.ContainsKey("IdUsuario");

                    MainPage = new LoginModalPage(this);
                }

                bool bLogg = false;
                int iUsr = 0;
                if (Application.Current.Properties.ContainsKey("IsLoggedIn"))
                {
                    Boolean.TryParse(Application.Current.Properties["IsLoggedIn"].ToString(), out bLogg);
                }

                if (Application.Current.Properties.ContainsKey("IdUsuario"))
                {
                    Int32.TryParse(Application.Current.Properties["IdUsuario"].ToString(), out iUsr);
                }

                    if (bLogg && iUsr != 0)
                    {
                        MainPage = new MainPage();
                    }
                    else
                    {
                        MainPage = new LoginModalPage(this);
                    }

                 sTiempo += "  Termina : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");

                cLog oLog = new cLog(sTiempo);

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast("Excep|" + ex.Message);
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        protected override void OnStart()
        {
                Connectivity.ConnectivityChanged += (sender, args) =>
                    {
                        if (args.NetworkAccess != NetworkAccess.Internet)
                        {
                            UserDialogs.Instance.Toast("Oops, se ha perdido la conexión a internet");
                            Application.Current.Properties["IsNotConnected"] = true;
                            Application.Current.SavePropertiesAsync();
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("Conectado a internet nuevamente");
                            Application.Current.Properties["IsNotConnected"] = false;
                            Application.Current.SavePropertiesAsync();
                            //IntentaSolicitudes();
                        }

                    };

                    // This should come before AppCenter.Start() is called
                    // Avoid duplicate event registration:
                    if (!AppCenter.Configured)
                    {
                        Push.PushNotificationReceived += async (sender, e) =>
                        {
                            var summary = $"Push notification received:" +
                                                $"\n\tNotification title: {e.Title}" +
                                                $"\n\tMessage: {e.Message}";

                            // If there's custom data for the notification,
                            // print the entries
                            if ((e.CustomData != null) && e.CustomData.ContainsKey("iIdSolicitud"))
                            {

                                bool answer = await App.Current.MainPage.DisplayAlert(" ", "Deseas abrir la solicitud " + e.CustomData["iIdSolicitud"].ToString() + "?", "Si", "No");
                                if (answer)
                                {

                                    UserDialogs.Instance.ShowLoading("Cargando...");
                                    RespuestaSolicitudes itemSolicitud = CargaSolicitud(e.CustomData);

                                    Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
                                    {
                                        MainPage = new MainPage(true, itemSolicitud);

                                        UserDialogs.Instance.HideLoading();
                                    });
                                }
                            }

                            AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Verbose;
                            // Send the notification summary to debug output
                            System.Diagnostics.Debug.WriteLine(summary);
                        };
                    }

                    // Handle when your app starts
                    AppCenter.Start("android=ae241cb7-68cf-4479-902a-729dfea71bb2;" +
                          "uwp={Your UWP App secret here};" +
                          "ios=05431e2c-e3cc-4327-9dd8-b09486664068",
                          typeof(Analytics), typeof(Crashes), typeof(Push));

                    if ((App._JerarquiaUsuario != null) && (int)App._JerarquiaUsuario.iIdUsuario > 0)
                    {
                        WSClass wsClass = new WSClass();
                        wsClass.GuardaToken((int)App._JerarquiaUsuario.iIdUsuario, (string)Application.Current.Properties["name"], (string)Application.Current.Properties["Fcmtocken"], 
                            (string)Application.Current.Properties["device"], (string)Application.Current.Properties["platform"]);
                    }

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            //// Handle when your app resumes
            ///

            //if (DeviceInfo.Platform == DevicePlatform.iOS)
            //{
                OnResumeHandler?.Invoke(null, new EventArgs());
            //}
        }


        public void ShowMainPage()
        {
            try
            {
                MainPage = new MainPage();
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
                UserDialogs.Instance.Toast("sMP-" + sError);
            }
        }

        public void Logout()
        {
            Application.Current.Properties["IsLoggedIn"] = false;
            Application.Current.SavePropertiesAsync();

            //if ((App._JerarquiaUsuario != null) && (int)App._JerarquiaUsuario.iIdUsuario > 0)
            //{
            //    WSClass wsClass = new WSClass();
            //    wsClass.BorraToken((int)App._JerarquiaUsuario.iIdUsuario, (string)Application.Current.Properties["name"], (string)Application.Current.Properties["Fcmtocken"], 
            //                        (string)Application.Current.Properties["device"], (string)Application.Current.Properties["platform"]);
            //}

            MainPage = new LoginModalPage(this);
        }

        public RespuestaSolicitudes CargaSolicitud(System.Collections.Generic.IDictionary<string,string> DataSolicitud)
        {
            string psCentroAlta = "";
            string psCentroSolicita = "";
            int piIdSolicitud = 0;
            int piIdCentroAlta = 0;
            int piIdCentroSolicita = 0;
            string psIdJustificacion = "";
            string psJustificacion = "";
            int piIdEstatus = 0;
            string psEstatus = "";
            string psCeCo = "";
            string psCuentaContable = "";
            string psSolicitante = "";
            DateTime pdtFechaAlta = DateTime.Now;
            DateTime pdtFechaEntrega = DateTime.Now;
            decimal pdTotal = 0;
            
            foreach (var key in DataSolicitud.Keys)
            {
                switch (key)
                {
                    case "sCentroAlta":
                        psCentroAlta = DataSolicitud[key].ToString();
                        break;
                    case "sCentroSolicita":
                        psCentroSolicita = DataSolicitud[key].ToString();
                        break;
                    case "iIdSolicitud":
                        Int32.TryParse(DataSolicitud[key].ToString(), out piIdSolicitud);
                        break;
                    case "iIdCentroAlta":
                        Int32.TryParse(DataSolicitud[key].ToString(), out piIdCentroAlta);
                        break;
                    case "iIdCentroSolicita":
                        Int32.TryParse(DataSolicitud[key].ToString(), out piIdCentroSolicita);
                        break;
                    case "sIdJustificacion":
                        psIdJustificacion = DataSolicitud[key].ToString();
                        break;
                    case "sJustificacion":
                        psJustificacion = DataSolicitud[key].ToString();
                        break;
                    case "iIdEstatus":
                        Int32.TryParse(DataSolicitud[key].ToString(), out piIdEstatus);
                        break;
                    case "sEstatus":
                        psEstatus = DataSolicitud[key].ToString();
                        break;
                    case "sCeCo":
                        psCeCo = DataSolicitud[key].ToString();
                        break;
                    case "sCuentaContable":
                        psCuentaContable = DataSolicitud[key].ToString();
                        break;
                    //case "dtFechaAlta":
                    //    pdtFechaAlta = DateTime.ParseExact(DataSolicitud[key].ToString(), "dd/MM/yyyy", null);
                    //    break;
                    //case "dtFechaEntrega":
                    //    pdtFechaEntrega = DateTime.ParseExact(DataSolicitud[key].ToString(), "dd/MM/yyyy", null);
                    //    break;
                    case "dTotal":
                        Decimal.TryParse(DataSolicitud[key].ToString(), out pdTotal);
                        break;
                    case "sSolicitante":
                        psSolicitante = DataSolicitud[key].ToString();
                        break;
                    default:
                        break;
                }

            }

            RespuestaSolicitudes vRespuestaS = new RespuestaSolicitudes
            {
                sCentroAlta = psCentroAlta,
                sCentroSolicita = psCentroSolicita,
                iIdSolicitud = piIdSolicitud,
                iIdCentroAlta = piIdCentroAlta,
                iIdCentroSolicita = piIdCentroSolicita,
                sIdJustificacion = psIdJustificacion,
                sJustificacion = psJustificacion,
                iIdEstatus = piIdEstatus,
                sEstatus = psEstatus,
                sCeCo = psCeCo,
                sCuentaContable = psCuentaContable,
                dtFechaAlta = pdtFechaAlta,
                dtFechaEntrega = pdtFechaEntrega,
                dTotal = pdTotal,
                sSolicitante  = psSolicitante
            };
            return vRespuestaS;
        }


        public void IntentaSolicitudes()
        {
            try
            {
                //var dtSolicitudes = Database.GetSolicitudesAsync();

                //foreach (AccionSolicitud aSolicitud in dtSolicitudes.Result)
                //{
                //    WSClass wsClass = new WSClass();
                //    if (aSolicitud.bRechaza)
                //    {
                //        wsClass.IntentaRechazaSolicitud(aSolicitud.iIdSolicitud, aSolicitud.iIdCentro, aSolicitud.iIdUsuario);
                //    }
                //    if (aSolicitud.bCancela)
                //    {
                //        wsClass.IntentaCancelaSolicitud(aSolicitud.iIdSolicitud, aSolicitud.iIdCentro, aSolicitud.iIdUsuario);
                //    }
                //    if (aSolicitud.bAcepta)
                //    {
                //        wsClass.IntentaAceptaSolicitud(aSolicitud.iIdSolicitud, aSolicitud.iIdCentro, aSolicitud.iIdUsuario, aSolicitud.iIdGerencia, aSolicitud.bIndGerencia, aSolicitud.bIndDireccion, aSolicitud.bIndConcejo);
                //    }

                //    Database.DeleteRow(aSolicitud);
                //}


            }
            catch (Exception ex)
            {
                string sError = ex.Message;
                System.Diagnostics.Debug.WriteLine(sError);
            }
        }


    }
}
