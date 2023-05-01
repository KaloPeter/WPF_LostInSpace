using System;
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

        private static ImageBrush instruction_background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));


        public InstructionWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }
        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.Show();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //lb_instructionsText.Content = File.ReadAllText(System.IO.Path.Combine("Others", "Instruction.txt"));//If we read from file, static variable to read from it only once
            string instructions = "Player movement:left and right arrow keys.\r\nAvoid asteroids and satellites.\r\nOxigen gives you health.\r\nTraver as far as you can.\r\nHave fun.";
            lb_instructionsText.Content = instructions;
            GameInstructionsGrid.Background = instruction_background;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.I)
            {
                Window_Closed(sender, e);
            }
        }
    }
}
