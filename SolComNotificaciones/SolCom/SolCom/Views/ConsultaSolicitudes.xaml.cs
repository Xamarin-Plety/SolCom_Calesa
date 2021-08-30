using Acr.UserDialogs;
using App1.Clases;
using App1.ViewModels;
using SolCom;
using SolCom.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConsultaSolicitudes : ContentPage
    {
        ObservableCollection<cProveedor> data = new ObservableCollection<cProveedor>();
        cProveedor pSelectedProveedor = new cProveedor();

        public ConsultaSolicitudes ()
		{
			InitializeComponent ();
            //TraeDatosFiltros();
            string sFiltros = " Inicia Filtros : " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
            
            dtFechaInicial.Date = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM") + "/01");

            UserDialogs.Instance.ShowLoading("Cargando Filtros...");
            Task.Run(() =>
            {
                // Run code here
                //await Task.Delay(200);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    string sCarga = " FF Inicia " + DateTime.Now.ToString("yyyyMMdd HH:mm:ss");
                    //cFiltrosConsulta cFiltro = new cFiltrosConsulta();
                    //cFiltro.CargaFiltros(App._JerarquiaUsuario.iIdCentro, App._JerarquiaUsuario.iIdGerencia, App._JerarquiaUsuario.iIdDireccion);

                    #region WS
                    if (App.Current.dsFiltrosV == null)
                    {
                        DataSet dsFiltros2 = null;
                        Stopwatch sw = new Stopwatch();
                        int? iCentro = App._JerarquiaUsuario.iIdCentro;
                        int? iGerencia = App._JerarquiaUsuario.iIdGerencia;
                        int? iDireccion = App._JerarquiaUsuario.iIdDireccion;
                        if (iCentro == null) iCentro = 0;
                        if (iGerencia == null) iGerencia = 0;
                        if (iDireccion == null) iDireccion = 0;
                        string sUrlFiltros = ApiRoutes.UrlConsultaFiltros();
                        sw.Restart();
                        System.Net.HttpWebRequest.DefaultWebProxy = null;
                        System.Net.WebRequest.DefaultWebProxy = null;
                        System.Net.ServicePointManager.Expect100Continue = false;
                        System.Net.ServicePointManager.UseNagleAlgorithm = false;
                        System.Net.ServicePointManager.DefaultConnectionLimit = 10;


                        sw.Restart();
                        WSClass client = new WSClass();
                        string result = await client.ObtenerStringFiltro<string>(sUrlFiltros + "?iIdCentro=" + iCentro.ToString() + "&iIdGerencia=" + iGerencia.ToString() + "&iIdDireccion=" + iDireccion.ToString());
                        sw.Stop();
                        App.Current.dsFiltrosV = JsonConvert.DeserializeObject<DataSet>(result);
                        //cLog oLog2 = new cLog("Respuesta: " + sw.ElapsedMilliseconds.ToString());

                    }

                    #endregion

                    //DataTable dtCentros = cFiltro.dsFiltrosLocal.Tables[0];
                    //DataTable dtComprador = cFiltro.dsFiltrosLocal.Tables[1];
                    //DataTable dtProveedor = cFiltro.dsFiltrosLocal.Tables[2];
                    //DataTable dtSolicitante = cFiltro.dsFiltrosLocal.Tables[3];


                    DataTable dtCentros = App.Current.dsFiltrosV.Tables[0];
                    DataTable dtComprador = App.Current.dsFiltrosV.Tables[1];
                    DataTable dtProveedor = App.Current.dsFiltrosV.Tables[2];
                    DataTable dtSolicitante = App.Current.dsFiltrosV.Tables[3];


                    List<Centro> nlstCentros = new List<Centro>();
                        foreach (DataRow dr in dtCentros.Rows)
                        {
                            nlstCentros.Add(new Centro { iIdCentro = Int32.Parse(dr["iIdCentro"].ToString()), sNombre = dr["sNombre"].ToString() });
                        }
                        pIdCentro.ItemsSource = nlstCentros;
                        pIdCentro.ItemDisplayBinding = new Binding("sNombre");

                        List<cComprador> nlstComprador = new List<cComprador>();
                        foreach (DataRow dr in dtComprador.Rows)
                        {
                            nlstComprador.Add(new cComprador { iIdFolioPersona = Int32.Parse(dr["iIdFolioPersona"].ToString()), sNombreCompleto = dr["sNombreCompleto"].ToString() });
                        }
                        pIdComprador.ItemsSource = nlstComprador;
                        pIdComprador.ItemDisplayBinding = new Binding("sNombreCompleto");

                        List<cSolicitante> nlstSolicitante = new List<cSolicitante>();
                        foreach (DataRow dr in dtSolicitante.Rows)
                        {
                            nlstSolicitante.Add(new cSolicitante { iIdUsuario = Int32.Parse(dr["iIdUsuario"].ToString()), sNombreCompleto = dr["sNombreCompleto"].ToString() });
                        }
                        pIdSolicitante.ItemsSource = nlstSolicitante;
                        pIdSolicitante.ItemDisplayBinding = new Binding("sNombreCompleto");
                        ListOfProveedores(dtProveedor);

                    UserDialogs.Instance.HideLoading();
                });
            });



            //cLog oLog = new cLog(sFiltros);

        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

        }

        private void btnConsultar(object sender, EventArgs e)
        {
            UserDialogs.Instance.ShowLoading("Consultando...");
            Task.Run(async () =>
            {

                cProveedor scProveedor = ProveedorListView.SelectedItem as cProveedor;
                Centro pSelectCentro = pIdCentro.SelectedItem as Centro;
                cComprador pSelectComprador = pIdComprador.SelectedItem as cComprador;
                cSolicitante pSelectSolicitante = pIdSolicitante.SelectedItem as cSolicitante;
                var pIdSolicitud = Convert.ToInt32(iIdSolicitud.Text);

                int piCentro = (pSelectCentro == null) ? 0 : pSelectCentro.iIdCentro;
                int? piComprador = (pSelectComprador == null) ? 0 : pSelectComprador.iIdFolioPersona;
                int? piProveedor = (scProveedor == null) ? 0 : scProveedor.iIdFolioPersona;
                int? piSolicitante = (pSelectSolicitante == null) ? 0 : pSelectSolicitante.iIdUsuario;

                await Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        Navigation.PushAsync(new ListSoli(pIdSolicitud, piCentro, dtFechaInicial.Date, dtFechaFinal.Date, piComprador, piSolicitante, piProveedor));
                    }
                    catch (Exception ex)
                    {
                        string sError = ex.Message;
                        UserDialogs.Instance.Toast(sError);
                        UserDialogs.Instance.HideLoading();
                    }

                });
                //UserDialogs.Instance.HideLoading();
            });


            
        }

        public async void ListOfProveedores(DataTable dtProveedor) //List of Proveedores  
        {
            try
            {
                foreach (DataRow dr in dtProveedor.Rows)
                {
                    data.Add(new cProveedor { iIdFolioPersona = Int32.Parse(dr["iIdFolioPersona"].ToString()), sNombreCompleto = dr["sNombreCompleto"].ToString() });
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex, "Ok");
            }
        }

        private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            scrollV.ScrollToAsync(searchBar, ScrollToPosition.Start, true);
            pSelectedProveedor = null;
            btnConsulta.IsEnabled = false;
            ProveedorListView.IsVisible = true;
            ProveedorListView.BeginRefresh();

            try
            {
                var dataEmpty = data.Where(i => i.sNombreCompleto.ToLower().Contains(e.NewTextValue.ToLower()));

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                {
                    ProveedorListView.SelectedItem = null;
                    ProveedorListView.IsVisible = false;
                    btnConsulta.IsEnabled = true;
                }
                else
                    ProveedorListView.ItemsSource = data.Where(i => i.sNombreCompleto.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            catch (Exception ex)
            {
                ProveedorListView.IsVisible = false;

            }
            ProveedorListView.EndRefresh();

        }


        private void ListView_OnItemTapped(Object sender, ItemTappedEventArgs e)
        {
            cProveedor listsd = e.Item as cProveedor;
            searchBar.Text = listsd.sNombreCompleto;
            ProveedorListView.IsVisible = false;

            btnConsulta.IsEnabled = true;
        }

        private void searchBar_Focused(object sender, FocusEventArgs e)
        {
            scrollV.ScrollToAsync(searchBar, ScrollToPosition.Start, true);
        }

        private async void TraeDatosFiltros()
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                int? iCentro = App._JerarquiaUsuario.iIdCentro;
                int? iGerencia = App._JerarquiaUsuario.iIdGerencia;
                int? iDireccion = App._JerarquiaUsuario.iIdDireccion;
                if (iCentro == null) iCentro = 0;
                if (iGerencia == null) iGerencia = 0;
                if (iDireccion == null) iDireccion = 0;
                string sUrlFiltros = ApiRoutes.UrlConsultaFiltros();
                sUrlFiltros += "?iIdCentro=" + iCentro.ToString() + "&iIdGerencia=" + iGerencia.ToString() + "&iIdDireccion=" + iDireccion.ToString();
                sw.Restart();

                //WSClass client = new WSClass();
                //string result = await client.ObtenerString<string>(sUrlFiltros);
                System.Net.HttpWebRequest.DefaultWebProxy = null;
                string result = "";
                var request = WebRequest.Create("http://acceso.pastelerialety.com/SolComApi/api/solicitudes/NewM");
                request.Proxy = null;
                var response = request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }


                sw.Stop();
                //DataSet dsFiltros2 = JsonConvert.DeserializeObject<DataSet>(result);
                cLog oLog = new cLog("Respuesta: " + sw.ElapsedMilliseconds.ToString());

            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }
        }
    }
}