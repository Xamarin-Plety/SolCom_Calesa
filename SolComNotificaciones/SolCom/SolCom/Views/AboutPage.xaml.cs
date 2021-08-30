using SolCom.Clases;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            lblApi.Text = "Api url : " + ApiRoutes.sUrl;
            string sUsuario = string.Empty;
            if (Application.Current.Properties.ContainsKey("name"))
            {
                sUsuario =  Application.Current.Properties["name"].ToString();
            }

            lblUsuario.Text = "Usuario : " + sUsuario;
        }
    }
}