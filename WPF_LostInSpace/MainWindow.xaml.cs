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
using WPF_LostInSpace.GameController;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.HelperClasses;

namespace WPF_LostInSpace
{
    //ctrl+k+d
    public partial class MainWindow : Window
    {
        private Logic logic;
        private Controller controller;

        private DispatcherTimer timer_backgroundMove;

        private DispatcherTimer timer_itemMove;

        private DispatcherTimer timer_generateAsteroid;
        private DispatcherTimer timer_generateSatellite;
        private DispatcherTimer timer_generateCrystal;
        private DispatcherTimer timer_generateHealth;

        private DispatcherTimer timer_playerMovement;

        public MainWindow()
        {
            InitializeComponent();

            logic = new Logic();
            controller = new Controller(logic);
            display.SetUpLogic(logic);

            timer_backgroundMove = new DispatcherTimer();
            timer_itemMove = new DispatcherTimer();
            timer_generateAsteroid = new DispatcherTimer();
            timer_generateSatellite = new DispatcherTimer();
            timer_generateCrystal = new DispatcherTimer();
            timer_generateHealth = new DispatcherTimer();
            timer_playerMovement = new DispatcherTimer();

            /////////////////////////////////////////////////////////
            //IDŐZÍTÉS

            timer_backgroundMove.Interval = TimeSpan.FromMilliseconds(1);
            timer_backgroundMove.Tick += (sender, eventArgs) =>
            {
                logic.BackgroundMove();
            };

            timer_itemMove.Interval = TimeSpan.FromMilliseconds(10);
            timer_itemMove.Tick += (sender, eventArgs) =>
            {
                logic.ItemMove();
            };

            timer_generateAsteroid.Interval = TimeSpan.FromMilliseconds(1000);
            timer_generateAsteroid.Tick += (sender, eventArgs) =>
            {
                logic.GenerateAsteroid();
            };

            timer_generateSatellite.Interval = TimeSpan.FromMilliseconds(3000);
            timer_generateSatellite.Tick += (sender, eventArgs) =>
            {
                logic.GenerateSatellite();
            };

            timer_generateCrystal.Interval = TimeSpan.FromMilliseconds(6000);
            timer_generateCrystal.Tick += (sender, eventArgs) =>
            {
                logic.GenerateCrystal();
            };

            timer_generateHealth.Interval = TimeSpan.FromMilliseconds(8000);
            timer_generateHealth.Tick += (sender, eventArgs) =>
            {
                logic.GenerateHealth();
            };
            /////////////////////////////////////////////////////////
            timer_playerMovement.Interval = TimeSpan.FromMilliseconds(10);
            timer_playerMovement.Tick += (sender, eventArgs) =>
            {
                controller.DecideMoveDirection();
            };

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            logic.SetUpPlayArea(new Size(grid.ActualWidth, grid.ActualHeight));
            logic.SetUpBackground();
            logic.SetUpPanels();
            logic.SetUpPlayer();

            timer_backgroundMove.Start();

            timer_generateAsteroid.Start();
            timer_generateCrystal.Start();
            timer_generateHealth.Start();
            timer_generateSatellite.Start();
            timer_itemMove.Start();

            timer_playerMovement.Start();

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            controller.KeyDown(e.Key);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            controller.KeyUp(e.Key);
        }
    }
}
