using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GleasonUsers.Model;

namespace GleasonUsers.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : FlyoutPage
    {
        public HomePage()
        {
            InitializeComponent();

            Detail = new NavigationPage(new HomePageDetail(Application.Current.Properties["Users"].ToString()));
            FlyoutPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutMenuItem;
            if (item == null)
                return;

            //var page = (MainPage)Activator.CreateInstance(item.TargetType);
            //page.Title = item.Title;

            switch (item.Title)
            {
                case "Dashboard":
                    Detail = new NavigationPage(new HomePageDetail(Application.Current.Properties["Users"].ToString()));
                    break;
                case "User Management":
                    Detail = new NavigationPage(new GleasonUsers.Pages.UserManagePage());
                    break;
                case "Logout":
                    Application.Current.MainPage = new LoginPage();
                    break;
                default: DisplayAlert("Alert", "Invalid selection.", "Ok");
                    break;
            }

            IsPresented = false;
            FlyoutPage.ListView.SelectedItem = null;
        }
    }
}