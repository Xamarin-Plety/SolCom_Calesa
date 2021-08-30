using App1.Clases;
using App1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SolCom.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SolicitudDetalleDet : ContentPage
    {
        SolicitudViewModel viewModel;
        public SolicitudDetalleDet()
        {
            InitializeComponent();
            var item = new RespuestaSolicitudes
            {
            };

            viewModel = new SolicitudViewModel(item);
            BindingContext = viewModel;
        }

        public SolicitudDetalleDet(DetalleSolicitud item)
        {
            InitializeComponent();
            BindingContext = item;
        }
    }
}