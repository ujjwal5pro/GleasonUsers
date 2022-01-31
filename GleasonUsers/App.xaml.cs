using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GleasonUsers.Views;
using System.IO;
using GleasonUsers.Data;

namespace GleasonUsers
{
    public partial class App : Application
    {
        static UserDatabase database;

        public App()
        {
            InitializeComponent();
            LoadData();

            MainPage = new LoginPage();
        }

        #region App State

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #endregion

        #region Database Connection

        // Create the database connection as a singleton.
        public static UserDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new UserDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Sample.db3"));
                }
                return database;
            }
        }

        async void LoadData()
        {
            var users = await App.Database.GetUsersAsync();
            if (users != null && users.Count > 0)
                Application.Current.Properties["Users"] = users.Count;
            else
                Application.Current.Properties["Users"] = "0";
        }
        #endregion
    }
}
