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

namespace WPF_LostInSpace
{
    /// <summary>
    /// Interaction logic for UserManagementWindow.xaml
    /// </summary>
    public partial class UserManagementWindow : Window
    {
        private static ImageBrush background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        private static BitmapFrame icon = BitmapFrame.Create(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Icons", "MainWindowIcon.ico"), UriKind.RelativeOrAbsolute)));

        public UserManagementWindow()
        {
            InitializeComponent();

            Icon = icon;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserManagementGrid.Background = background;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
