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


namespace WPF_LostInSpace
{
    /// <summary>
    /// Interaction logic for QuestionWindow.xaml
    /// </summary>
    public partial class QuestionWindow : Window
    {
        private static ImageBrush instruction_background = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "InstructionsWindowBackground.jpg"), UriKind.RelativeOrAbsolute)));
        private string question;
        private string title;

        public QuestionWindow(string question, string title)
        {
            this.question = question;
            this.title = title;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameQuestionsGrid.Background = instruction_background;
            txQuestion.Text = question;
            this.Title = title;
        }

        private void bt_Yes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void bt_No_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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
