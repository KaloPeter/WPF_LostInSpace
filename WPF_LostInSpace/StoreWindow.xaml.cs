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
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.Store;

namespace WPF_LostInSpace
{
    /// <summary>
    /// Interaction logic for StoreWindow.xaml
    /// </summary>
    public partial class StoreWindow : Window
    {
        private GO_Player goPlayer;
        private MainWindow mw;

        public StoreWindow(GO_Player goPlayer, MainWindow mw)
        {
            this.goPlayer = goPlayer;
            this.mw = mw;

            List<SpaceSuit> spaceSuits = new List<SpaceSuit>();
            spaceSuits.Add(new SpaceSuit(1, 2, 100, "astronaut_1", 0));
            spaceSuits.Add(new SpaceSuit(2, 3, 150, "astronaut_2", 50000));
            spaceSuits.Add(new SpaceSuit(3, 3, 500, "astronaut_3", 100000));
            spaceSuits.Add(new SpaceSuit(4, 5, 170, "astronaut_4", 300000));

            InitializeComponent();

            lbPurchaseableItems.ItemsSource = spaceSuits;

        }

        private SpaceSuit selectedSuit;

        private void lbPurchaseableItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //selectedItem
            selectedSuit = ((sender as ListBox).SelectedItem as SpaceSuit);



            //Image myImage = new Image();
            //BitmapImage myImageSource = new BitmapImage();
            //myImageSource.BeginInit();
            //myImageSource.UriSource = new Uri(selectedSuit.SpaceSuitResPath, UriKind.RelativeOrAbsolute);
            //myImageSource.EndInit();
            //myImage.Source = myImageSource;
            //lbActiveSpaceSuitImgRes.Content = myImage;


            //lbActiveSpaceSuitHealth.Content = "Health: " + selectedSuit.Health;
            //lbActiveSpaceSuitSpeed.Content = "Speed: " + selectedSuit.Speed;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Active space suit load
            lbMoney.Content = goPlayer.Money;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.EnableDisableMainMenuButtons(true);
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSuit != null)
            {
                QuestionWindow qw = new QuestionWindow("Are you sure you want to purchase the choosen suit?","Purchase confirm");

                if (qw.ShowDialog() == true)
                {
                    //minus player money
                    //Add suit suit to players inventory by ID, remove suit price from store
                    //make suit active?? 
                    //owned property for spacesuit
                    //activate button for owned suits
                }
                
            }
            else
            {
                MessageBox.Show("Choose spaceSuit!");
            }



            


        }
    }
}