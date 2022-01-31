using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GleasonUsers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GleasonUsers.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserManagePage : ContentPage
    {
        int UserID = 0;
        string UserRole = "";
        List<Users> UserList ;


        // Initialize
        public UserManagePage()
        {
            InitializeComponent();
        }


        // Search/View User
        private void SearchUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchItem = SearchUser.Text;
            if (searchItem == "" || searchItem == null)
            {
                UsersListView_Refreshing(new object(), new EventArgs());
            }
            else
            {
                UsersListView.ItemsSource = null;
                UsersListView.ItemsSource = UserList.Where(c => c.FirstName.ToLower().StartsWith(searchItem.ToLower()));
            }
        }
        async private void UsersListView_Refreshing(object sender, EventArgs e)
        {
            UserList = await App.Database.GetUsersAsync();
            UsersListView.EndRefresh();
            UsersListView.ItemsSource = null;
            UsersListView.ItemsSource = UserList;
            Application.Current.Properties["Users"] = UserList.Count();
        }
        private void UsersListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var user = e.Item as Users;
            UserID = user.Id;
            UsersListView.SelectedItem = null;
            OpenUserView(user);
        }
        private void UserViewCloseButton_Clicked(object sender, EventArgs e)
        {
            CloseUserView();
        }


        // Add New User
        private void AddToolbarItem_Clicked(object sender, EventArgs e)
        {
            var senderText = (sender as ToolbarItem).Text;
            if (senderText == "Update")
                OpenAddView(ViewUserStack.BindingContext as Users);
            else
                OpenAddView(null);
        }
        async private void AddUserButtonClicked(object sender, EventArgs e)
        {

            if (!ValidateAddView())
            {
                await DisplayAlert("Warning", "Please fill the form.", "Ok");
                return;
            }

            var tempUser = new Users()
            {
                Role = UserRole,
                FirstName = FirstNameEntry.Text,
                LastName = LastNameEntry.Text,
                Email = EmailEntry.Text,
            };

            if ((sender as Button).Text == "Update")
            {
                //UserList.Remove(UserList.Where(c => c.FirstName == FirstNameEntry.Text).First());
                tempUser.Id = UserID;
                await DisplayAlert("Message", "User Updated successfully", "Ok");
            }
            else
            {
                tempUser.Id = 0;
                await DisplayAlert("Message", "User added successfully", "Ok");
            }
           

            //UserList.Add(tempUser);

            var tempResult = await App.Database.SaveUserAsync(tempUser);

            UsersListView_Refreshing(new object(), new EventArgs());
            CloseAddView();
        }
        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            UserRole = (sender as RadioButton).Content.ToString();
        }
        private void AddCancelButton_Clicked(object sender, EventArgs e)
        {
            CloseAddView();
        }


        // Update User
        private void UpdateMenuItem_Clicked(object sender, EventArgs e)
        {
            var tempdata = ((MenuItem)sender).BindingContext as Users;

            OpenAddView(tempdata);
        }


        // Remove User
        async private void DeleteMenuItem_Clicked(object sender, EventArgs e)
        {
            var user = ((MenuItem)sender).BindingContext as Users;
            var result = await App.Database.DeleteUserAsync(user);
            //UserList.Remove(tempdata);

            UsersListView_Refreshing(new object(), new EventArgs());
            await DisplayAlert("Message", "User removed", "Ok");
        }


        #region Supporting methods

        void OpenAddView(Users user)
        {
            if(user != null)
            {
                ViewUserStack.IsVisible = false;
                AddUserButton.Text = "Update";
                AddUserStack.BindingContext = user;
                SetRadioButton(user.Role);
            }

            MainViewStack.IsVisible = false;
            AddUserStack.IsVisible = true;
            AddNewToolBarButton.IsEnabled = false;

        }

        void OpenUserView(Users user)
        {
            ViewUserStack.BindingContext = user;
            ViewUserStack.IsVisible = true;
            MainViewStack.IsVisible = false;
            this.Title = "User Details";
            AddNewToolBarButton.Text = "Update";

            //AddUserStack.IsVisible = true;
            //FirstNameEntry.Text = user.FirstName;
            //LastNameEntry.Text = user.LastName;
            //EmailEntry.Text = user.Email;
            //AddButtonMenuStack.IsVisible = false;
            //FirstNameEntry.IsEnabled = false;
            //LastNameEntry.IsEnabled = false;
            //EmailEntry.IsEnabled = false;

        }

        void CloseUserView()
        {
            this.Title = "User Management";
            ViewUserStack.IsVisible = false;
            MainViewStack.IsVisible = true;
            AddNewToolBarButton.Text = "Add";

        }

        void CloseAddView()
        {
            MainViewStack.IsVisible = true;
            AddUserStack.IsVisible = false;
            UsersListView.ItemsSource = UserList;
            AddNewToolBarButton.IsEnabled = true;
            AddUserButton.Text = "Add";
            //Dispose Add FOrm Values
            FirstNameEntry.Text = "";
            LastNameEntry.Text = "";
            EmailEntry.Text = "";
            RadioButton0.IsChecked = false;
            RadioButton1.IsChecked = false;
            RadioButton2.IsChecked = false;
        }

        bool ValidateAddView()
        {
            if (FirstNameEntry.Text != null && LastNameEntry.Text != null)
                return true;
            else
                return false;
        }

        void SetRadioButton(string value)
        {
            switch (value)
            {
                case "Gleasons Admin":
                    RadioButton0.IsChecked = true;
                    break;
                case "User":
                    RadioButton1.IsChecked = true;
                    break;
                case "Customer Admin":
                    RadioButton2.IsChecked = true;
                    break;
                default: return;
            }
        }

        #endregion

        #region App State
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Retrieve all the notes from the database, and set them as the
            // data source for the CollectionView.

            //var note = new Users()
            //{
            //    Id = 0,
            //    FirstName = "Hello"
            //};
            //var temp1 = await App.Database.SaveUserAsync(note);

            UserList = await App.Database.GetUsersAsync();
            UsersListView.ItemsSource = UserList;

        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();
            return true;
        }
        #endregion

    }
}


// await App.Database.GetUsersAsync();
// await App.Database.GetNoteAsync(id);
// await App.Database.SaveNoteAsync(note);
// await App.Database.DeleteNoteAsync(note);