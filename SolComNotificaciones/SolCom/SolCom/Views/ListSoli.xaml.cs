using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using App1.Clases;
using App1.ViewModels;
using SolCom.Clases;
using Xamarin.Essentials;
using SolCom;
using Acr.UserDialogs;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Data;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListSoli : ContentPage
    {
        public bool bConsulta = false;
        public bool bDetalle = false;
        BaseViewModel viewModel;
        public int iCantPendiente = 0;

        public ListSoli()
        {
            string sListado = " Inicia Listado1 : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            try
            {
                        InitializeComponent();

                        try
                        {
                            bConsulta = false;
                            Title = "Solicitudes por Autorizar";

                            if (!Application.Current.Properties.ContainsKey("cUsuarioJerarquia"))
                            {
                                App.Current.Logout();
                                return;
                            }

                            App._JerarquiaUsuario = JsonConvert.DeserializeObject<cUsuarioJerarquia>((string)Application.Current.Properties["cUsuarioJerarquia"]); 
                            if (App._JerarquiaUsuario == null)
                            {
                                App.Current.Logout();
                                return;
                            }

                            //WSClass wsClass2 = new WSClass();
                            //wsClass2.GuardaToken((int)App._JerarquiaUsuario.iIdUsuario, (string)Application.Current.Properties["name"], (string)Application.Current.Properties["Fcmtocken"],
                            //    (string)Application.Current.Properties["device"], (string)Application.Current.Properties["platform"]);


                        }
                        catch (Exception ex)
                        {
                            string sError = ex.Message;

                            UserDialogs.Instance.Toast("LS-" + sError);
                        }
            }
            catch (Exception ex)
            {
                string strEx = "LSI-|" + ex.Message;
                UserDialogs.Instance.Toast(strEx);
            }
            sListado += " Termina Listado1 : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            cLog cLog = new cLog(sListado);
        }

        public ListSoli(RespuestaSolicitudes vitem)
        {
            if (SolicitudListView == null)
            {
                InitializeComponent();
            }
            bConsulta = false;
            Title = "Solicitudes por Autorizar";
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (current == NetworkAccess.Internet)
            {
                TraeDatos();
            }

            PushSolicitud(vitem);
            
        }

        public ListSoli(int pIdSolicitud, int pIdCentro, DateTime dtFechaInicial, DateTime dtFechaFinal)
        {
            UserDialogs.Instance.ShowLoading("Cargando Datos...");

            if (SolicitudListView == null)
            {
                InitializeComponent();
            }
            SolicitudListView.ItemsSource = null;

            Task.Run(async () =>
            {
                await Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    bConsulta = true;
                    Title = "Consulta de Solicitudes";

                    var current = Connectivity.NetworkAccess;
                    var profiles = Connectivity.ConnectionProfiles;
                    if (current == NetworkAccess.Internet)
                    {
                        TraeDatos(pIdSolicitud, pIdCentro, dtFechaInicial, dtFechaFinal);
                    }

                    UserDialogs.Instance.HideLoading();

                });
            });


        }

        public ListSoli(int pIdSolicitud, int pIdCentro, DateTime dtFechaInicial, DateTime dtFechaFinal, int? pIdComprador, int? pIdSolicitante, int? pIdProveedor)
        {


            if (SolicitudListView == null)
            {
                InitializeComponent();
            }
            SolicitudListView.ItemsSource = null;

                    bConsulta = true;
                    Title = "Consulta de Solicitudes";

                    var current = Connectivity.NetworkAccess;
                    var profiles = Connectivity.ConnectionProfiles;
                    if (current == NetworkAccess.Internet)
                    {
                        TraeDatos(pIdSolicitud, pIdCentro, dtFechaInicial, dtFechaFinal, pIdComprador, pIdSolicitante, pIdProveedor);
                    }

        }


        void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            bDetalle = true;
            UserDialogs.Instance.ShowLoading("Procesando...");
            Task.Run(async () =>
            {
                await Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    var current = Connectivity.NetworkAccess;
                    if (current != NetworkAccess.Internet)
                    {
                        UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                        return;
                    }

                    RespuestaSolicitudes item = args.SelectedItem as RespuestaSolicitudes;
                    if (item == null)
                        return;

                    Navigation.PushAsync(new SolicitudDetallePage(new SolicitudViewModel(item), bConsulta));

                    SolicitudListView.SelectedItem = null;

                    UserDialogs.Instance.HideLoading();

                });
            });

        }

        async void PushSolicitud(RespuestaSolicitudes rSolicitud)
        {
            await Navigation.PushAsync(new NavigationPage(new SolicitudDetallePage(new SolicitudViewModel(rSolicitud))));
        }

        private async void TraeFiltros()
        {
            try
            {
                if (App.Current.dsFiltrosV == null)
                {
                    Stopwatch sw = new Stopwatch();
                    string sNDatos = " Inicia TraeDatosWS : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                    string sFiltroApi = ApiRoutes.UrlConsultaFiltros();

                    int? iCentro = App._JerarquiaUsuario.iIdCentro;
                    int? iGerencia = App._JerarquiaUsuario.iIdGerencia;
                    int? iDireccion = App._JerarquiaUsuario.iIdDireccion;
                    if (iCentro == null) iCentro = 0;
                    if (iGerencia == null) iGerencia = 0;
                    if (iDireccion == null) iDireccion = 0;

                    sw.Restart();
                    WSClass client = new WSClass();
                    string result = await client.ObtenerStringFiltro<string>(sFiltroApi + "?iIdCentro=" + iCentro.ToString() + "&iIdGerencia=" + iGerencia.ToString() + "&iIdDireccion=" + iDireccion.ToString());
                    sw.Stop();
                    App.Current.dsFiltrosV = JsonConvert.DeserializeObject<DataSet>(result);
                    //cLog oLog2 = new cLog("Respuesta: " + sw.ElapsedMilliseconds.ToString());
                }
            }
            catch (OperationCanceledException oex)
            {
                string strEx = "oex|" + oex.Message;
                UserDialogs.Instance.Toast(strEx);
            }
            catch (Exception ex)
            {
                string strEx = "|flag|" + ex.Message;
                UserDialogs.Instance.Toast(strEx);

            }

        }
        private async void TraeDatos()
        {
            string sTraeDatos = " Inicia TraeDatos : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            string xFlag = "0";
            try
            {

                if (SolicitudListView == null)
                {
                    InitializeComponent();
                }

                //App.Current.ProcValidaSession();
                xFlag = "1";
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                    return;
                }

                xFlag = "2";
                if (!Application.Current.Properties.ContainsKey("cUsuarioJerarquia"))
                {
                    App.Current.Logout();
                    return;
                }

                App._JerarquiaUsuario = JsonConvert.DeserializeObject<cUsuarioJerarquia>((string)Application.Current.Properties["cUsuarioJerarquia"]);

                if ((App._JerarquiaUsuario == null) || App._JerarquiaUsuario.iIdUsuario == 0)
                {
                    //var vJerarquia = await App.Database.GetUserJerarquiaAsync();
                    //if (vJerarquia.Count == 0)
                    //{
                        return;
                    //}
                    //App._JerarquiaUsuario.iIdUsuario = vJerarquia[0].iIdUsuario;
                    //App._JerarquiaUsuario.iIdCentro = vJerarquia[0].iIdCentro;
                    //App._JerarquiaUsuario.iIdGerencia = vJerarquia[0].iIdGerencia;
                    //App._JerarquiaUsuario.iIdDireccion = vJerarquia[0].iIdDireccion;
                    //App._JerarquiaUsuario.iIdEmpresa = vJerarquia[0].iIdEmpresa;
                    //App._JerarquiaUsuario.bGerencia = vJerarquia[0].bGerencia;
                    //App._JerarquiaUsuario.bDireccion = vJerarquia[0].bDireccion;
                    //App._JerarquiaUsuario.bConsejo = vJerarquia[0].bConsejo;
                }


                xFlag = "3";
                string sNDatos = " Inicia TraeDatosWS : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                string sSolicitudesApi = ApiRoutes.UrlApiGetSolicitudesPendientes(App._JerarquiaUsuario.iIdEmpresa, App._JerarquiaUsuario.iIdDireccion, App._JerarquiaUsuario.iIdGerencia,
                                                                                  App._JerarquiaUsuario.bGerencia, App._JerarquiaUsuario.bDireccion, App._JerarquiaUsuario.bConsejo,
                                                                                  DateTime.Now.Date.AddMonths(-1).ToString("yyyyMMdd"), DateTime.Now.Date.ToString("yyyyMMdd"),
                                                                                  App._JerarquiaUsuario.iIdUsuario);
                xFlag = "3.2|";
                WSClass client = new WSClass();
                List<RespuestaSolicitudes> result = await client.GetS<RespuestaSolicitudes>(sSolicitudesApi);

                if (result != null)
                {
                    xFlag = "|4";

                    if (result.Count > 0)
                    {
                        xFlag = "|4.2|";
                            SolicitudListView.ItemsSource = result.OrderByDescending(r => r.iIdSolicitud);
                        CambiaColor(result);
                    }
                    else
                    {
                        xFlag = "4.3";
                        SolicitudListView.ItemsSource = null;
                    }
                    sNDatos += " termina TraeDatosWs : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                    //cLog oLogDatos = new cLog(sNDatos);
                }
                else
                {
                    SolicitudListView.ItemsSource = null;
                }
                xFlag = "5";
            }
            catch (OperationCanceledException oex)
            {
                string strEx = "oex|" + xFlag + oex.Message;
                UserDialogs.Instance.Toast(strEx);
            }
            catch (Exception ex)
            {
                string strEx = "|flag|" + xFlag + "|" + ex.Message;
                UserDialogs.Instance.Toast(strEx);

            }

            sTraeDatos += " Inicia TraeDatos : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            //cLog oLog = new cLog(sTraeDatos);


        }

        private async void TraeDatos(int pIdSolicitud, int pIdCentro,DateTime dtFechaInicial,DateTime dtFechaFinal)
        {

            try
            {
                if (SolicitudListView == null)
                {
                    InitializeComponent();
                }



                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                    return;
                }

                string sSolicitudesApi = ApiRoutes.UrlApiSolicitudesPendientes(pIdSolicitud.ToString(), pIdCentro.ToString(), dtFechaInicial, dtFechaFinal
                                                                                , (int)App._JerarquiaUsuario.iIdEmpresa, (int)App._JerarquiaUsuario.iIdDireccion, (int)App._JerarquiaUsuario.iIdGerencia);


                WSClass client = new WSClass();
                List<RespuestaSolicitudes> result = await client.Get<RespuestaSolicitudes>(sSolicitudesApi);
                if (result != null)
                {
                    CambiaColor(result);
                    SolicitudListView.ItemsSource = result.OrderByDescending(r => r.iIdSolicitud);
                }
                else
                {
                    SolicitudListView.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                string strEx = ex.Message;
                UserDialogs.Instance.Toast("TD-" +strEx);

            }

        }


        private async void TraeDatos(int pIdSolicitud, int pIdCentro, DateTime dtFechaInicial, DateTime dtFechaFinal,int? pIdComprador, int? pIdSolicitante, int? pIdProveedor)
        {
            try
            {

                if (SolicitudListView == null)
                {
                    InitializeComponent();
                }

                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                    return;
                }

                string sSolicitudesApi = ApiRoutes.UrlApiSolicitudesPendientes(pIdSolicitud.ToString(), pIdCentro.ToString(), dtFechaInicial.ToString("yyyyMMdd"), dtFechaFinal.ToString("yyyyMMdd")
                                                                                , pIdComprador.ToString(), pIdProveedor.ToString(), pIdSolicitante.ToString()
                                                                                , (int)App._JerarquiaUsuario.iIdEmpresa, (int)App._JerarquiaUsuario.iIdDireccion, (int)App._JerarquiaUsuario.iIdGerencia);


                WSClass client = new WSClass();
                List<RespuestaSolicitudes> result = await client.Get<RespuestaSolicitudes>(sSolicitudesApi);
                if (result != null)
                {
                    CambiaColor(result);
                    SolicitudListView.ItemsSource = result.OrderByDescending(r => r.iIdSolicitud);
                }
                else
                {
                    SolicitudListView.ItemsSource = null;
                }
            }
            catch (Exception ex)
            {
                string strEx = ex.Message;
                UserDialogs.Instance.Toast(strEx);
            }
            UserDialogs.Instance.HideLoading();
        }

        void Handle_OnResumeHandler(object sender, EventArgs e)
        {
            Console.WriteLine("OnPauseResumeWithPage");

            DependencyService.Get<IPushCancel>().CancelPush(Convert.ToInt32(0), iCantPendiente);

            if (!bConsulta)
            {
                TraeDatos();
            }
        }

        protected override void OnAppearing()
        {
            (App.Current as App).OnResumeHandler += Handle_OnResumeHandler;
            base.OnAppearing();


            DependencyService.Get<IPushCancel>().CancelPush(Convert.ToInt32(0), iCantPendiente);

            if (!bConsulta)
                {
                    TraeDatos();
                    TraeFiltros();
                }

        }

        protected override void OnDisappearing()
        {
            (App.Current as App).OnResumeHandler -= Handle_OnResumeHandler;
            base.OnDisappearing();
        }

        private void SolicitudListView_Refreshing(object sender, EventArgs e)
        {
            SolicitudListView.IsRefreshing = false;
            if (!bConsulta)
            {
                TraeDatos();
            }
        }


        private void CambiaColor(List<RespuestaSolicitudes> lstSolicitudes)
        {
            try
            {
                iCantPendiente = lstSolicitudes.Count();

                Color vColor = Color.LightGray;
                foreach (RespuestaSolicitudes rSolicitud in lstSolicitudes.OrderByDescending(r => r.iIdSolicitud))
                {
                    if (vColor == Color.LightGray)
                    {
                        vColor = Color.White;
                    }
                    else
                    {
                        vColor = Color.LightGray;
                    }

                    rSolicitud.bgColor = vColor;
                    rSolicitud.fntColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                string strEx = ex.Message;
                UserDialogs.Instance.Toast("Color-" + strEx);
            }
        }
    }
}
