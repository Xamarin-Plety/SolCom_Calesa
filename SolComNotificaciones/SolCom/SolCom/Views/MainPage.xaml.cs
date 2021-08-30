using App1.Clases;
using App1.Models;
using App1.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Linq;
using SolCom.Models;
using SolCom;
using SolCom.Clases;
using Acr.UserDialogs;
using System;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            try
            {
                MasterBehavior = MasterBehavior.Popover;
                //MenuPages.Add((int)MenuItemType.Soli, new NavigationPage(new ListSoli()));
                //CargaJerarquia();
                Detail = new NavigationPage(new ListSoli());
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
                UserDialogs.Instance.Toast("Main1=" + sError);
            }
        }


        public async void CargaJerarquia()
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    WSClass client = new WSClass();
                    var iIdUsuario = Application.Current.Properties.ContainsKey("IdUsuario") ? (int)Application.Current.Properties["IdUsuario"] : 0;
                    string sJerarquiaApi = ApiRoutes.UrlJerarquiaUsuario(iIdUsuario.ToString());
                    cUsuarioJerarquia UsuarioJerarquia = client.ObtenerJerarquia(sJerarquiaApi);
                    App._JerarquiaUsuario = UsuarioJerarquia;
                }
                else
                {
                    //var vJerarquia = await App.Database.GetUserJerarquiaAsync();
                    //if (vJerarquia.Count == 0)
                    //{
                        return;
                    //}
                    //cUsuarioJerarquia nJerarquia = new cUsuarioJerarquia();
                    //nJerarquia.iIdUsuario = vJerarquia[0].iIdUsuario;
                    //nJerarquia.iIdCentro = vJerarquia[0].iIdCentro;
                    //nJerarquia.iIdGerencia = vJerarquia[0].iIdGerencia;
                    //nJerarquia.iIdDireccion = vJerarquia[0].iIdDireccion;
                    //nJerarquia.iIdEmpresa = vJerarquia[0].iIdEmpresa;
                    //nJerarquia.bGerencia = vJerarquia[0].bGerencia;
                    //nJerarquia.bDireccion = vJerarquia[0].bDireccion;
                    //nJerarquia.bConsejo = vJerarquia[0].bConsejo;
                    //App._JerarquiaUsuario = nJerarquia;
                }
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
                UserDialogs.Instance.Toast("CJ=" + sError);
            }
        }

        public MainPage(bool bPush, RespuestaSolicitudes svItem)
        {
            InitializeComponent();
            MasterBehavior = MasterBehavior.Popover;
            //MenuPages.Add((int)MenuItemType.Soli, new NavigationPage(new ListSoli()));
            //Detail = new NavigationPage(new ListSoli(svItem));
            Detail = new NavigationPage(new SolicitudDetallePage(new SolicitudViewModel(svItem)));
        }

        public async Task MainNotification(bool bPush, RespuestaSolicitudes svItem)
        {
            bool answer = await App.Current.MainPage.DisplayAlert("Notification", "Deseas abrir la solicitud " + svItem.iIdSolicitud.ToString() + "?", "Si", "No");
            if (answer)
            {
                InitializeComponent();
                MasterBehavior = MasterBehavior.Popover;
                //MenuPages.Add((int)MenuItemType.Soli, new NavigationPage(new ListSoli()));
                Detail = new NavigationPage(new SolicitudDetallePage(new SolicitudViewModel(svItem)));
            }

        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Soli:
                        MenuPages.Add(id, new NavigationPage(new ListSoli()));
                        break;
                    case (int)MenuItemType.ConsultaSolicitudes:
                        MenuPages.Add(id, new NavigationPage(new ConsultaSolicitudes()));
                        break;
                    case (int)MenuItemType.AboutPage:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.Cerrar:

                        bool answer = await DisplayAlert(" ", "Deseas cerrar tu sesión?", "Si", "No");
                        if (answer)
                        {
                            App.Current.Logout();
                        }
                        return;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }

    }
}