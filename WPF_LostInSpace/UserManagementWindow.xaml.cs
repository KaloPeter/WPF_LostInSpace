using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.Userdata;

namespace WPF_LostInSpace
{
    public partial class UserManagementWindow : Window
    {
        private static ImageBrush background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        private static BitmapFrame icon = BitmapFrame.Create(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Icons", "MainWindowIcon.ico"), UriKind.RelativeOrAbsolute)));

        private MainWindow mw;
        private Logic logic;
        public UserManagementWindow(MainWindow mw, Logic logic)
        {
            this.mw = mw;
            this.logic = logic;

            InitializeComponent();

            Icon = icon;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserManagementGrid.Background = background;
            LoadUsersFromUsersList();


        }

        private void LoadUsersFromUsersList()//Set usernames in combo box, and choose last logged User's name when this window opened
        {
            cb_userProfiles.Items.Clear();
            logic.Users.ForEach(u => cb_userProfiles.Items.Add(u.Username));
            string currentUserName = logic.Users.Select(u => u.Username).Where(u => u == logic.CurrentUser.Username).First();

            cb_userProfiles.SelectedItem = currentUserName;


        }

        private void bt_Create_Click(object sender, RoutedEventArgs e)
        {

            if (logic.Users.Any(u => u.Username == tb_username.Text))
            {
                WarningWindow ww = new WarningWindow("Username already exists!", "Username exists");
                ww.Show();
            }
            else
            {
                User user = new User();

                user.Username = tb_username.Text;
                user.LastLogin = DateTime.Now;
                user.MusicVolume = 0.2;//Default volume is 0.2
                user.EffectVolume = 0.2;
                user.PurchasedSpaceSuitIDX = new List<int>() { 1 };

                logic.Users.Add(user);
                logic.SelectLastLoggedUser();
                LoadUsersFromUsersList();
                logic.SaveUsersToJson();
            }

        }

        private void bt_EditName_Click(object sender, RoutedEventArgs e)
        {
            if (logic.Users.Any(u => u.Username == tb_username.Text))
            {
                WarningWindow ww = new WarningWindow("Username already exists, modifications cannot be done", "Username Modification problem!");
                ww.Show();
            }
            else
            {
                selectedUser.Username = tb_username.Text;
                LoadUsersFromUsersList();
                logic.SaveUsersToJson();
            }




        }

        private User selectedUser;
        private void bt_Delete_Click(object sender, RoutedEventArgs e)
        {
            QuestionWindow qw = new QuestionWindow("Are you sure you want to delete the user?", "Delete user");
            if (qw.ShowDialog() == true)
            {
                if (logic.Users.Count > 1)
                {
                    if (cb_userProfiles.SelectedIndex == 0)
                    {
                        logic.CurrentUser = logic.Users[1];

                    }
                    else if (cb_userProfiles.SelectedIndex == logic.Users.Count - 1)
                    {
                        logic.CurrentUser = logic.Users[0];
                    }
                    else
                    {
                        logic.CurrentUser = logic.Users[cb_userProfiles.SelectedIndex + 1];
                    }

                    logic.Users.Remove(selectedUser);
                    LoadUsersFromUsersList();
                    logic.SaveUsersToJson();
                }
                else
                {
                    WarningWindow ww = new WarningWindow("The last user cannot be removed!", "Remove process failed!");
                    ww.Show();
                }
            }


        }

        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.EnableDisableMainMenuButtons(true);
        }

        private void cb_userProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = logic.Users.Where(u => u.Username == (sender as ComboBox).SelectedItem).FirstOrDefault();

            if (selectedUser != null)
            {
                tb_username.Text = selectedUser.Username;
                logic.CurrentUser = selectedUser;
                logic.CurrentUser.LastLogin = DateTime.Now;

                lb_money.Content = selectedUser.Money;
                lb_bestDistance.Content = selectedUser.BestDistance;
                lb_totalDistance.Content = selectedUser.TotalDistance;
                logic.SpaceSuitsByUserInventory();
                logic.SetUpPlayer();
                logic.SetEffectVolume();
                logic.SetMusicVolume();

                logic.SaveUsersToJson();

            }
        }
    }
}
