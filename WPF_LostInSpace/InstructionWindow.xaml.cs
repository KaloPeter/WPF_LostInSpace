using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_LostInSpace
{
    public partial class InstructionWindow : Window
    {
        private static ImageBrush instruction_background = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        private static BitmapFrame icon = BitmapFrame.Create(new BitmapImage(new Uri(Path.Combine("Images", "Icons", "MainWindowIcon.ico"), UriKind.RelativeOrAbsolute)));
        private static string instructions = File.ReadAllText(Path.Combine("Instruction", "Instruction.txt"));

        private MainWindow mainWindow;

        public InstructionWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            InitializeComponent();

            Icon = icon;
        }
        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.EnableDisableMainMenuButtons(true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tx_instructionsText.Text = instructions;
            GameInstructionsGrid.Background = instruction_background;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.EnableDisableMainMenuButtons(true);
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
