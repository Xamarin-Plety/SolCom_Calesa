using App1.Models;
using SolCom;
using SolCom.Clases;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        ViewCell lastCell;

        public MenuPage()
        {
            InitializeComponent();

            menuItems = new List<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Soli, Title="Solicitudes por Autorizar" },
                new HomeMenuItem {Id = MenuItemType.ConsultaSolicitudes, Title="Consulta de Solicitudes" },
                new HomeMenuItem {Id = MenuItemType.AboutPage, Title="Acerca De" },
                new HomeMenuItem {Id = MenuItemType.Cerrar, Title="Cerrar Sesion" }
            };

            ListViewMenu.ItemsSource = menuItems;

            ListViewMenu.ItemSelected += async (sender, e) =>
            {

                if (e.SelectedItem == null)
                    return;

                int id = (int)((HomeMenuItem)e.SelectedItem).Id;
                await RootPage.NavigateFromMenu(id);
            };
        }

        private void ViewCell_Tapped(object sender, EventArgs e)
        {
            if (lastCell != null)
                lastCell.View.BackgroundColor = Color.Transparent;
            var viewCell = (ViewCell)sender;
            if (viewCell.View != null)
            {
                viewCell.View.BackgroundColor = Color.FromHex("5fbefa");
                lastCell = viewCell;
            }
        }
    }
}