using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_LostInSpace.GameLogic;

namespace WPF_LostInSpace
{
    public partial class SettingsWindow : Window
    {
        private MainWindow mw = null;
        private Logic logic = null;

        private static bool isFullscreen = false;

        private static ImageBrush settingsBackground = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "settings_BG.jpg"), UriKind.RelativeOrAbsolute)));

        private static BitmapFrame icon = BitmapFrame.Create(new BitmapImage(new Uri(Path.Combine("Images", "Icons", "MainWindowIcon.ico"), UriKind.RelativeOrAbsolute)));

        public SettingsWindow(MainWindow mw, Logic logic)
        {
            this.mw = mw;
            this.logic = logic;
            Icon = icon;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            slEffects.Value = (int)(logic.CurrentUser.EffectVolume * 100);
            slMusic.Value = (int)(logic.CurrentUser.MusicVolume * 100);
            this.Background = settingsBackground;
            this.Background.Opacity = 0.3;

            if (isFullscreen)
            {
                btFullScreenMode.Opacity = 0.2;
                btFullScreenMode.IsEnabled = false;
            }
            else
            {
                btWindowMode.Opacity = 0.2;
                btWindowMode.IsEnabled = false;
            }


        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.EnableDisableMainMenuButtons(true);
        }

        private void Button_MainMenu_Click(object sender, RoutedEventArgs e)
        {

            if (logic.CurrentUser.MusicVolume != slMusic.Value / 100 || logic.CurrentUser.EffectVolume != slEffects.Value / 100)
            {
                QuestionWindow qw = new QuestionWindow("You have unsaved modifications! Would you like to save them?", "Unsaved modifications");

                if (qw.ShowDialog() == true)
                {
                    SetNewVolumeValues();
                    this.Close();
                }
                else
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }

        }

        private void Button_Click_Apply(object sender, RoutedEventArgs e)
        {
            SetNewVolumeValues();
        }


        private static WindowState oldState;
        private static WindowStyle oldStyle;

        private void btWindowMode_Click(object sender, RoutedEventArgs e)
        {
            isFullscreen = false;
            //1200x720
            mw.WindowState = oldState;
            mw.WindowStyle = oldStyle;

            ReArrangeView();

            btWindowMode.IsEnabled = false;
            btFullScreenMode.IsEnabled = true;
            btWindowMode.Opacity = 0.2;
            btFullScreenMode.Opacity = 0.75;
        }

        private void ReArrangeView()
        {

            logic.SetUpPlayArea(new Size(mw.grid.ActualWidth, mw.grid.ActualHeight));
            logic.SetUpBackground();
            logic.SetUpPanels();

            logic.SetUpPlayer();
            this.Focus();
        }

        private void btFullScreenMode_Click(object sender, RoutedEventArgs e)
        {

            isFullscreen = true;

            oldState = mw.WindowState;
            oldStyle = mw.WindowStyle;

            mw.WindowState = WindowState.Maximized;
            mw.WindowStyle = WindowStyle.None;

            ReArrangeView();
            btWindowMode.IsEnabled = true;
            btFullScreenMode.IsEnabled = false;
            btWindowMode.Opacity = 0.75;
            btFullScreenMode.Opacity = 0.2;
        }

        private void SetNewVolumeValues()
        {
            logic.CurrentUser.EffectVolume = slEffects.Value / 100;
            logic.CurrentUser.MusicVolume = slMusic.Value / 100;

            logic.SetEffectVolume();
            logic.SetMusicVolume();

            logic.SaveUsersToJson();
        }
    }
}
