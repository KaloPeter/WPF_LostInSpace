using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.Userdata;

namespace WPF_LostInSpace
{
    /// <summary>
    /// Interaction logic for UserManagementWindow.xaml
    /// </summary>
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

        private void LoadUsersFromUsersList()
        {
            cb_userProfiles.Items.Clear();
            logic.Users.ForEach(u => cb_userProfiles.Items.Add(u.Username));
            string cun = logic.Users.Select(u => u.Username).Where(u => u == logic.CurrentUser.Username).FirstOrDefault();

            cb_userProfiles.SelectedItem = cun;


        }

        private void bt_Create_Click(object sender, RoutedEventArgs e)
        {

            if (logic.Users.Any(u => u.Username == tb_username.Text))
            {
                MessageBox.Show("existing user!!!");
            }
            else
            {
                User user = new User();

                user.Username = tb_username.Text;
                user.LastLogin = DateTime.Now;

                //logic.CreateUser(user);
                logic.Users.Add(user);
                logic.SelectLastLoggedUser();
                LoadUsersFromUsersList();
                logic.SaveUsersToJson();
            }

        }

        private void bt_EditName_Click(object sender, RoutedEventArgs e)
        {
            selectedUser.Username = tb_username.Text;
            LoadUsersFromUsersList();
            logic.SaveUsersToJson();

        }
        private User selectedUser;
        private void bt_Delete_Click(object sender, RoutedEventArgs e)
        {
            QuestionWindow qw = new QuestionWindow("Are you sure you want to delete the user?", "Delete user");
            if (qw.ShowDialog() == true)
            {
                if (logic.Users.Count > 1)
                {
                    logic.Users.Remove(selectedUser);
                    logic.CurrentUser = logic.Users[0];
                    LoadUsersFromUsersList();
                    logic.SaveUsersToJson();
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


        //TEST WHEN USERS LIST EMPTY
        private void cb_userProfiles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = logic.Users.Where(u => u.Username == (sender as ComboBox).SelectedItem).FirstOrDefault();//((sender as ComboBox).SelectedItem) as User;

            if (selectedUser != null)
            {
                tb_username.Text = selectedUser.Username;
                logic.CurrentUser = selectedUser;
                logic.CurrentUser.LastLogin = DateTime.Now;

                lb_money.Content = selectedUser.Money;
                lb_bestDistance.Content = selectedUser.BestDistance;
                lb_totalDistance.Content = selectedUser.TotalDistance;

            }
        }
    }
}
