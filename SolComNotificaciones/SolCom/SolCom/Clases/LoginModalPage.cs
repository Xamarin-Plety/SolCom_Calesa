using App1.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1.Clases
{
    class LoginModalPage : CarouselPage
    {
        ContentPage login;

        public LoginModalPage(ILoginManager ILoginMgr)
        {
            login = new Login(ILoginMgr);

            this.Children.Add(login);
            MessagingCenter.Subscribe<ContentPage>(this,"Login",(sender) =>
            {
                this.SelectedItem = login;
            });
        }
    }
}
