﻿using System;
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
    /// Interaction logic for WarningWindow.xaml
    /// </summary>
    public partial class WarningWindow : Window
    {
        private static ImageBrush instruction_background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        private string warning;
        private string title;
        public WarningWindow(string warning, string title)
        {
            this.warning = warning;
            this.title = title;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameWarningsGrid.Background = instruction_background;
            txWarning.Text = warning;
            this.Title = title;
        }
        private void bt_OK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //We handle the right Mouse button, so user cant use it on the window
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonDown(e);
            e.Handled = true;
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseRightButtonUp(e);
            e.Handled = true;
        }
    }
}
