using App1.Clases;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SolCom;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Acr.UserDialogs;
using SolCom.Views;
using SolCom.UserControl;
using System.Windows.Input;
using SolCom.Clases;
using System.IO;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SolicitudDetallePage : ContentPage
    {
        SolicitudViewModel viewModel;
        bool bNotificacion = false;
        public SolicitudDetallePage (SolicitudViewModel viewModel)
		{
            InitializeComponent ();

            BindingContext = this.viewModel = viewModel;
            bNotificacion = true;
            if (App._JerarquiaUsuario.bGerencia == true || App._JerarquiaUsuario.bDireccion == true || App._JerarquiaUsuario.bConsejo == true)
            {
                btnAcepta.Clicked += btnAceptar;
                btnRechaza.Clicked += btnRechazar;
                btnCancela.Clicked += btnCancelar;
            }

            PDFListView.IsVisible = true;
            ProcAjustaFilas();
        }

        public SolicitudDetallePage(SolicitudViewModel viewModel,bool bConsulta)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
            bNotificacion = false;
            if (bConsulta)
            {
                ToolbarItems.Remove(btnAcepta);
                ToolbarItems.Remove(btnRechaza);
                ToolbarItems.Remove(btnCancela);
            }
            else
            {
                if (App._JerarquiaUsuario.bGerencia == true || App._JerarquiaUsuario.bDireccion == true || App._JerarquiaUsuario.bConsejo == true)
                {
                    btnAcepta.Clicked += btnAceptar;
                    btnRechaza.Clicked += btnRechazar;
                    btnCancela.Clicked += btnCancelar;
                }
            }
            ProcAjustaFilas();
        }

        public SolicitudDetallePage()
        {
            InitializeComponent();

            var item = new RespuestaSolicitudes
            {

            };
            viewModel = new SolicitudViewModel(item);
            BindingContext = viewModel;
            bNotificacion = false;
            ProcAjustaFilas();
        }

        private void ProcAjustaFilas()
        {
            grdPrincipal.RowDefinitions[19].Height = ((viewModel.rSolicitud.lstDetalleSolicitud.Count() * 40) >= 300) ? 300 : viewModel.rSolicitud.lstDetalleSolicitud.Count() * 40;
            grdPrincipal.RowDefinitions[20].Height = ((viewModel.rSolicitud.lstDocumentosSolicitud.Count() * 80) >= 300) ? 300 : viewModel.rSolicitud.lstDocumentosSolicitud.Count() * 80;
            grdPrincipal.RowDefinitions[21].Height = ((viewModel.rSolicitud.lstComenatriosSolicitud.Count() * 30) >= 120) ? 120 : viewModel.rSolicitud.lstComenatriosSolicitud.Count() * 30;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        public bool bValidaEstatus()
        {
            bool bValida = false;
            viewModel.RefreshEncabezado();
            string sUsuario = Application.Current.Properties.ContainsKey("name") ? Application.Current.Properties["name"].ToString() : "";
            if (sUsuario.ToUpper() == viewModel.rSolicitud.Asignada.ToUpper())
            {
                if (viewModel.rSolicitud.iIdEstatus != 7 && viewModel.rSolicitud.iIdEstatus != 8 && viewModel.rSolicitud.iIdEstatus != 9)
                {
                    bValida = true;
                }
            }
            return bValida;
        }

        public Task<bool> bValidaTask()
        {
            bool bValida = false;
            viewModel.RefreshEncabezado();
            string sUsuario = Application.Current.Properties.ContainsKey("name") ? Application.Current.Properties["name"].ToString() : "";
            if (sUsuario.ToUpper() == viewModel.rSolicitud.Asignada.ToUpper())
            {
                if (viewModel.rSolicitud.iIdEstatus != 7 && viewModel.rSolicitud.iIdEstatus != 8 && viewModel.rSolicitud.iIdEstatus != 9)
                {
                    bValida = true;
                }
            }

            return Task.FromResult(bValida);
        }

        async void btnAceptar(object sender, EventArgs e)
        {
            bool bValidado = false;

            UserDialogs.Instance.ShowLoading("Validando...");
            await Task.Run(async () =>
            {
                await Task.Delay(600);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    bValidado = await bValidaTask();
                    UserDialogs.Instance.HideLoading();

                });
            });

            //if (!bValidaEstatus())
            if (!bValidado)
            {
                await DisplayAlert(" ", "No se puede aceptar esta solicitud", "ok");
                    return;
            }


            try
                {

                    bool answer = await DisplayAlert(" ", "Deseas Aceptar esta solicitud?", "Si", "No");
                    if (answer)
                    {

                    UserDialogs.Instance.ShowLoading("Aceptando...");

                    MainPage RootPage = Application.Current.MainPage as MainPage;
                        var current = Connectivity.NetworkAccess;
                        if (current == NetworkAccess.Internet)
                        {
                            if (viewModel.AceptaSolicitud(App._JerarquiaUsuario.iIdUsuario.Value, App._JerarquiaUsuario.iIdGerencia, App._JerarquiaUsuario.bGerencia, App._JerarquiaUsuario.bDireccion, App._JerarquiaUsuario.bConsejo))
                            {
                                if (bNotificacion)
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        RootPage.NavigateFromMenu(0);
                                    });
                                }
                                else
                                {
                                    Device.BeginInvokeOnMainThread(() =>
                                    {
                                        Navigation.PopAsync();
                                    });
                                }
                            }
                        }

                    UserDialogs.Instance.HideLoading();
                    }
                }
                catch (Exception ex)
                {
                    UserDialogs.Instance.Toast(ex.Message);
                }
        }

        async void btnCancelar(object sender, EventArgs e)
        {
            bool bValidado = false;

            UserDialogs.Instance.ShowLoading("Validando...");
            await Task.Run(async () =>
            {
                await Task.Delay(600);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    bValidado = await bValidaTask();
                    UserDialogs.Instance.HideLoading();

                });
            });

            //if (!bValidaEstatus())
            if (!bValidado)
                    {
                        await DisplayAlert(" ", "No se puede cancelar esta solicitud", "ok");
                        return;
                    }

            try
            {
                bool answer = false;
                string result = await DisplayPromptAsync("Motivo de Cancelación", " ", "OK", "Cancelar");
                answer = (result != null && result.Length >5);
                if (answer)
                {
                    MainPage RootPage = Application.Current.MainPage as MainPage;
                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        if (viewModel.CancelaSolicitud(App._JerarquiaUsuario.iIdUsuario.Value, result))
                        {
                            if (bNotificacion)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    RootPage.NavigateFromMenu(0);
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Navigation.PopAsync();
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast(ex.Message);
            }

        }

        async void btnRechazar(object sender, EventArgs e)
        {
            bool bValidado = false;

            UserDialogs.Instance.ShowLoading("Validando...");
            await Task.Run(async () =>
            {
                await Task.Delay(300);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    bValidado = await bValidaTask();
                    UserDialogs.Instance.HideLoading();

                });
            });

            //if (!bValidaEstatus())
            if (!bValidado)
            {
                await DisplayAlert(" ", "No se puede rechazar esta solicitud", "ok");
                return;
            }

            try
            {
                bool answer = false;
                string result = await DisplayPromptAsync("Motivo de Rechazo", " ","OK","Cancelar");
                answer = (result != null && result.Length > 5);
                if (answer)
                {
                    MainPage RootPage = Application.Current.MainPage as MainPage;
                    var current = Connectivity.NetworkAccess;
                    if (current == NetworkAccess.Internet)
                    {
                        if (viewModel.RechazaSolicitud(App._JerarquiaUsuario.iIdUsuario.Value, result))
                        {
                            if (bNotificacion)
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    RootPage.NavigateFromMenu(0);
                                });
                            }
                            else
                            {
                                Device.BeginInvokeOnMainThread(() =>
                                {
                                    Navigation.PopAsync();
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                UserDialogs.Instance.Toast(ex.Message);
            }

        }

        private async void btnConsultar(object sender, EventArgs e)
        {
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                return;
            }

            try
            {
                string data = ((Button)sender).BindingContext as string;

                var status = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (status == PermissionStatus.Granted)
                {
                    cArchivos cGuarda = new cArchivos();
                    string sResultado = cGuarda.ProcRecuperaPDF(viewModel.rSolicitud.iIdSolicitud.ToString(), viewModel.rSolicitud.iIdCentroAlta.ToString(), data);
                    ProcOpenFile(sResultado);
                }


            }
            catch (Exception ex) 
            {
                UserDialogs.Instance.Toast(ex.Message);
            }

        }

        async void SolicitudListView_ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                return;
            }

            DetalleSolicitud item = args.SelectedItem as DetalleSolicitud;
            if (item == null)
                return;

            await Navigation.PushAsync(new SolicitudDetalleDet(item));

            // Manually deselect item.
            SolicitudListView.SelectedItem = null;
        }

        public async void ProcOpenFile(string sRutaArchivo)
        {
            await Launcher.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(sRutaArchivo)
            });
        }


    }
}