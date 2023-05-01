﻿using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for InstructionWindow.xaml
    /// </summary>
    public partial class InstructionWindow : Window
    {
        MainWindow mainWindow;
        public InstructionWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            mainWindow.isInstructionWindowOpen = true;
            mainWindow.isPaused = true;
            mainWindow.StopDispatcherTimers();

            GameInstructionsGrid.Background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        }
        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {
            Window_Closed(sender, e);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadInstructions();
        }

        private void LoadInstructions()
        {
            //lb_instructionsText.Content = File.ReadAllText(System.IO.Path.Combine("Others", "Instruction.txt"));
            string instructions = "Player movement:left and right arrow keys.\r\nAvoid asteroids and satellites.\r\nOxigen gives you health.\r\nTraver as far as you can.\r\nHave fun.";
            lb_instructionsText.Content = instructions;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mainWindow.isInstructionWindowOpen = false;
            mainWindow.isPaused = false;
            mainWindow.StartDispatcherTimers();
            this.Close();
        }
    }
}