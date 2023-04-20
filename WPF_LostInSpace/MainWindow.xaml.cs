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
using System.Windows.Threading;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.HelperClasses;

namespace WPF_LostInSpace
{
    //ctrl+k+d
    public partial class MainWindow : Window
    {
        private Logic logic;

        private DispatcherTimer timer_backgroundMove;


        public MainWindow()
        {
           
            InitializeComponent();

            logic = new Logic();
            display.SetUpLogic(logic);
            timer_backgroundMove = new DispatcherTimer();

            timer_backgroundMove.Interval = TimeSpan.FromMilliseconds(0);
            timer_backgroundMove.Tick += (sender, eventArgs) =>
            {
                logic.BackgroundMove();
            };


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
         
            logic.SetUpPlayArea(new Size(grid.ActualWidth, grid.ActualHeight));
            logic.SetUpBackground();
            
            

            timer_backgroundMove.Start();

        }
    }
}
