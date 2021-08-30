using Acr.UserDialogs;
using App1.Clases;
using SolCom;
using SolCom.Clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace App1.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        public string sUserName = "";
        public string sPassword = "";
        ILoginManager ILoginMngr = null;
		public Login (ILoginManager ILoginM)
		{
			InitializeComponent ();
            ILoginMngr = ILoginM;
		}

        private void btnLoginClick(object sender, EventArgs e)
        {
            try
            {
                var current = Connectivity.NetworkAccess;
                if (current != NetworkAccess.Internet)
                {
                    UserDialogs.Instance.Toast("No hay conexión a internet, favor de intentar mas tarde");
                    return;
                }

                if (Username.Text == string.Empty || Password.Text == string.Empty)
                {
                    return;
                }

                UserDialogs.Instance.ShowLoading("Autenticando...");
                Task.Run(async () =>
                {
                    await Task.Delay(100);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        sUserName = Seguridad.Encriptar(Username.Text);
                        sPassword = Seguridad.Encriptar(Password.Text);
                        string sLoginApi = ApiRoutes.UrlLogin();
                        WSClass client = new WSClass();
                        var result = client.AutenticarLogin(sLoginApi, sUserName, sPassword);
                        if (result != null)
                        {
                            string sJerarquiaApi = ApiRoutes.UrlJerarquiaUsuario(result.iIdUsuario.ToString());
                            cUsuarioJerarquia UsuarioJerarquia = client.ObtenerJerarquia(sJerarquiaApi);
                            Application.Current.Properties["IsLoggedIn"] = true;
                            Application.Current.Properties["IdUsuario"] = (int)result.iIdUsuario;
                            Application.Current.Properties["name"] = Username.Text;
                            Application.Current.Properties["cUsuarioJerarquia"] =  JsonConvert.SerializeObject(UsuarioJerarquia); 
                            Application.Current.SavePropertiesAsync();
                            App._JerarquiaUsuario = UsuarioJerarquia;

                            ProcGuardaJerarquia(UsuarioJerarquia);
                            ProcGuardaUsuario(result.iIdUsuario, Username.Text);

                            ILoginMngr.ShowMainPage();
                        }
                        else
                        {
                            UserDialogs.Instance.Toast("El Usuario y/o la Contraseña son incorrectos.");
                        }

                        UserDialogs.Instance.HideLoading();

                    });
                });

            }
            catch (Exception ex)
            {
                string sError = ex.Message;
                UserDialogs.Instance.Toast(sError);
                System.Diagnostics.Debug.WriteLine(sError);
            }
        }

        async void ProcGuardaUsuario(int iIdUser, string sUser)
        {
            Usuario vUsuario = new Usuario
            {
                iIdUsuario = iIdUser,
                sUsuario = sUser
            };
            //await App.Database.SaveUserAsync(vUsuario);
        }


        async void ProcGuardaJerarquia(cUsuarioJerarquia uJerarquia)
        {
            UsuarioJerarquia vUsuarioJerarquia = new UsuarioJerarquia();
                vUsuarioJerarquia.iIdUsuario = uJerarquia.iIdUsuario;
                vUsuarioJerarquia.iIdCentro = uJerarquia.iIdCentro;
                vUsuarioJerarquia.iIdGerencia = uJerarquia.iIdGerencia;
                vUsuarioJerarquia.iIdDireccion = uJerarquia.iIdDireccion;
                vUsuarioJerarquia.iIdEmpresa = uJerarquia.iIdEmpresa;
                vUsuarioJerarquia.bGerencia = uJerarquia.bGerencia;
                vUsuarioJerarquia.bDireccion = uJerarquia.bDireccion;
                vUsuarioJerarquia.bConsejo = uJerarquia.bConsejo;
            //await App.Database.SaveJerarquiaAsync(vUsuarioJerarquia);
        }
    }
}