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

namespace WPF_LostInSpace
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private MainWindow mw = null;
        private Logic logic = null;

        public SettingsWindow(MainWindow mw, Logic logic)
        {
            this.mw = mw;
            this.logic = logic;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            slEffects.Value = (int)(logic.CurrentUser.EffectVolume * 100);
            slMusic.Value = (int)(logic.CurrentUser.MusicVolume * 100);
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



        private void btWindowMode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btFullScreenMode_Click(object sender, RoutedEventArgs e)
        {

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
