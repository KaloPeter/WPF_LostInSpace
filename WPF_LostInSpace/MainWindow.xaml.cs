using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.HelperClasses;

namespace WPF_LostInSpace
{
    //ctrl+k+d
    public partial class MainWindow : Window
    {
        private Logic logic;

        public MainWindow()
        {
            InitializeComponent();

            logic = new Logic();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logic.SetUpPlayArea(new Size(grid.ActualWidth, grid.ActualHeight));
            MessageBox.Show(new Size(grid.ActualWidth, grid.ActualHeight)+"");


        }
    }
}
