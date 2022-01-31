using GleasonUsers.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GleasonUsers
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        string userString = "Admin";
        string passwordString = "Admin";

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButtonClicked(object sender, EventArgs e)
        {
            if(Username.Text == userString && Password.Text == passwordString)
            {
                Application.Current.MainPage = new HomePage();
            }
            else
            {
                DisplayAlert("Alert", "Login Failed.", "ok");
            }
        }

        private void ResetButtonClicked(object sender, EventArgs e)
        {
            Username.Text = "";
            Password.Text = "";
        }
    }
}