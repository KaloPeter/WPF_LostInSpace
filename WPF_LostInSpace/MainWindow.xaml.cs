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

        private List<DispatcherTimer> dispatcherTimers;

        private bool firstTimeStart = false;

        private bool isPaused = false;

        private Button[] bs_MainMenu;//Actual buttons
        private string[] bs_MainMenuText;//text of buttons

        //**************

        private DispatcherTimer timer_LOGGER;
        //**************

        public MainWindow()
        {

            InitializeComponent();

            Icon = BitmapFrame.Create(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Icons", "MainWindowIcon.ico"), UriKind.RelativeOrAbsolute)));

            logic = new Logic();
            controller = new Controller(logic);
            display.SetUpLogic(logic);

            dispatcherTimers = new List<DispatcherTimer>();

            int[] timerMilliseconds = new int[]
                {
                    1,//0 timer_backgroundMove
                    10,//1 timer_itemMove
                    500,//2 timer_generateAsteroid
                    6000,//3 timer_generateSatellite
                    3000,//4 timer_generateCrystal
                    4000,//5 timer_generateHealth
                    10,//6 timer_playerMovement
                    0,//7 timer_moveLaser
                    10,//8 timer_laserItemDetection
                    10,//9 timer_playerItemDetection
                    200,//10 timer_playerItemDetectionDelay
                    250,//11 timer_cooldownReduce
                };

            //We will have 3 buttons: play, instructions,exit
            const int NUMBER_OF_BUTTONS = 5;
            bs_MainMenu = new Button[NUMBER_OF_BUTTONS] { new Button(), new Button(), new Button(), new Button(), new Button() };
            bs_MainMenuText = new string[NUMBER_OF_BUTTONS] { "Start", "Instructions", "Store", "Users", "Exit" };


            for (int i = 0; i < timerMilliseconds.Length; i++)
            {
                dispatcherTimers.Add(new DispatcherTimer());
                dispatcherTimers[i].Interval = TimeSpan.FromMilliseconds(timerMilliseconds[i]);
            }


            dispatcherTimers[0].Tick += (sender, args) => { logic.BackgroundMove(); };
            dispatcherTimers[1].Tick += (sender, args) => { logic.ItemMove(); };
            dispatcherTimers[2].Tick += (sender, args) => { logic.GenerateAsteroid(); };
            dispatcherTimers[3].Tick += (sender, args) => { logic.GenerateSatellite(); };
            dispatcherTimers[4].Tick += (sender, args) => { logic.GenerateCrystal(); };
            dispatcherTimers[5].Tick += (sender, args) => { logic.GenerateHealth(); };
            dispatcherTimers[6].Tick += (sender, args) => { controller.DecideMoveDirection(); };
            dispatcherTimers[7].Tick += (sender, args) => { logic.MoveLaser(); };
            dispatcherTimers[8].Tick += (sender, args) => { logic.CheckLaserItemDetection(); };
            dispatcherTimers[9].Tick += (sender, args) => { logic.CheckPlayerItemDetection(); };
            dispatcherTimers[10].Tick += (sender, args) => { logic.PlayerItemDetectionDelay(); };
            dispatcherTimers[11].Tick += (sender, args) => { logic.ReduceLaserCooldown(); };


            //*******
            timer_LOGGER = new DispatcherTimer();
            timer_LOGGER.Interval = TimeSpan.FromSeconds(100);
            timer_LOGGER.Tick += (sender, args) => { logic.LOG_OBJ_PROP_VALS(); };
            //*******
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //When window loaded, we create a grid, and generate buttons on it. 
            CreateGrid();
            GenerateButtonsOnGrid();

            logic.SetUpPlayArea(new Size(grid.ActualWidth, grid.ActualHeight));
            logic.SetUpBackground();
            logic.SetUpPanels();

            logic.SetUpPlayer();

            //When play button pressed, we remove very event from second button(can be Instructions and MainMenu button)->bs_MainMenu[1],
            //then we remove the buttons from grid, we start the game(.start() for DisptachetTimers)
            //IPause, is false because we started game
            //firtTimeStarted is true, because we might want to pause the game, but when we want to continue, only resume button is allowed to start dispatcher timers.
            bs_MainMenu[0].Click += (sender, e) =>
            {
                bs_MainMenu[1].Click -= OpenInstructionWindow;

                bs_MainMenu[1].Click -= BackToMainMenu_ResetPropValues;//if player presses resume button, it is not gonna be removed, that's why we have to call this here too.

                bs_MainMenu[2].IsEnabled = false;
                bs_MainMenu[3].IsEnabled = false;


                RemoveButtonsFromGrid();

                StartStopDispatcherTimer(true);
                isPaused = false;
                firstTimeStart = true;

            };

            bs_MainMenu[1].Click += OpenInstructionWindow;

            bs_MainMenu[2].Click += (sender, e) => { logic.OpenStore(this); EnableDisableMainMenuButtons(false); };

            bs_MainMenu[3].Click += (sender, e) => { logic.OpenUsers(this); EnableDisableMainMenuButtons(false); };

            bs_MainMenu[4].Click += (sender, e) => { Application.Current.Shutdown(); };

            //******
            // timer_LOGGER.Start();
            //******

            //UserManagementWindow userManagementWindow = new UserManagementWindow();
            //userManagementWindow.Show();

        }

        private void OpenInstructionWindow(object sender, RoutedEventArgs e)
        {
            InstructionWindow instructionWindow = new InstructionWindow(this);
            instructionWindow.Show();
            EnableDisableMainMenuButtons(false);
        }

        public void EnableDisableMainMenuButtons(bool isEnable)//true:enable___false:disable
        {
            for (int i = 0; i < bs_MainMenu.Length; i++)
            {
                if (isEnable)
                {
                    bs_MainMenu[i].IsEnabled = isEnable;
                }
                else
                {
                    bs_MainMenu[i].IsEnabled = isEnable;
                }
            }
        }

        private void BackToMainMenu_ResetPropValues(object sender, RoutedEventArgs e)//!!!!!!!!!!!!!!!!!!
        {

            QuestionWindow qw = new QuestionWindow("Are you sure you want to go back to the MainMenu?", "Back to MainMenu");

            if (qw.ShowDialog() == true)
            {
                RemoveButtonsFromGrid();


                bs_MainMenuText[0] = "Start";
                bs_MainMenuText[1] = "Instructions";
                bs_MainMenu[1].Click -= BackToMainMenu_ResetPropValues;
                bs_MainMenu[1].Click += OpenInstructionWindow;
                bs_MainMenu[2].IsEnabled = true;
                bs_MainMenu[3].IsEnabled = true;
                GenerateButtonsOnGrid();
                //!!!!!!!!!!!!
                //when player dies, or goes back to main menu(if keeps money, then sync it with players money=wetie to jason, when manimenu button pressed, or player dies)
                logic.ResetGame();
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (firstTimeStart)//Game can be started only when Play/Resume button pressed___firstTimeStart name might be not the correct variable name
            {

                controller.KeyDown(e.Key);//Movement
                controller.SpaceDown(e.Key);//shoot

                if (e.Key == Key.P)
                {
                    isPaused = !isPaused;
                    firstTimeStart = !firstTimeStart;
                    controller.setAllKeyUp();
                }

                if (isPaused)
                {
                   

                    StartStopDispatcherTimer(false);

                    bs_MainMenuText[0] = "Resume";
                    bs_MainMenuText[1] = "MainMenu";

                    GenerateButtonsOnGrid();

                    bs_MainMenu[1].Click += BackToMainMenu_ResetPropValues;//The problem is here, when a I pause app, the instruction event added to Click

                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (!isPaused)
            {
                controller.KeyUp(e.Key);

                controller.SpaceUp(e.Key);
            }
        }

        public void StartStopDispatcherTimer(bool isStart)
        {
            for (int i = 0; i < dispatcherTimers.Count; i++)
            {
                if (isStart)
                {
                    dispatcherTimers[i].Start();
                }
                else
                {
                    dispatcherTimers[i].Stop();
                }
            }
        }//true->start___false:stop

        private void CreateGrid()
        {

            List<RowDefinition> rds = new List<RowDefinition>();
            for (int i = 0; i < 6; i++)
            {
                rds.Add(new RowDefinition());
                if (i == 0 || i == 5)
                {
                    rds[i].Height = new GridLength(3, GridUnitType.Star);//1*;
                }
                else
                {
                    rds[i].Height = new GridLength(1, GridUnitType.Star);//1*;
                }
                grid.RowDefinitions.Add(rds[i]);
            }

            List<ColumnDefinition> cls = new List<ColumnDefinition>();

            for (int i = 0; i < 3; i++)
            {
                cls.Add(new ColumnDefinition());
                cls[i].Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(cls[i]);
            }

        }//Creates the grids

        private void GenerateButtonsOnGrid()
        {
            for (int i = 0; i < bs_MainMenu.Length; i++)
            {
                bs_MainMenu[i].Content = bs_MainMenuText[i];
                bs_MainMenu[i].Width = 150;
                bs_MainMenu[i].Height = 50;
                //bs_MainMenu[i].Margin = new Thickness(10);
                bs_MainMenu[i].FontSize = 25;

                bs_MainMenu[i].Background = Brushes.LightBlue;
                bs_MainMenu[i].Opacity = 0.7;

                Grid.SetColumn(bs_MainMenu[i], 1);
                Grid.SetRow(bs_MainMenu[i], i + 1);
                grid.Children.Add(bs_MainMenu[i]);
            }
        }//Generates buttons on the grid

        private void RemoveButtonsFromGrid()
        {
            for (int i = grid.Children.Count - 1; i >= 0; i--)
            {
                if (grid.Children[i] is Button)
                {
                    grid.Children.Remove(grid.Children[i]);
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
