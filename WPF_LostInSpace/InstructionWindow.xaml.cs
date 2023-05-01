using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
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
        private static ImageBrush instruction_background = new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        private static string instructions = "Player movement:left and right arrow keys.\r\nAvoid asteroids and satellites.\r\nOxigen gives you health.\r\nTraver as far as you can.\r\nHave fun.";
        private MainWindow mainWindow;

        //********************************************************* Removes closing button and icon(left side)
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        //*********************************************************


        public InstructionWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();

        }
        private void bt_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.EnableDisableMainMenuButtons(true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //lb_instructionsText.Content = File.ReadAllText(System.IO.Path.Combine("Others", "Instruction.txt"));//If we read from file, static variable to read from it only once

            //********************************************************* Removes closing button and icon(left side)
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            //********************************************************* Removes closing button and icon(left side)


            lb_instructionsText.Content = instructions;
            GameInstructionsGrid.Background = instruction_background;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.EnableDisableMainMenuButtons(true);
        }
    }
}
