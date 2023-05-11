using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.Store;


namespace WPF_LostInSpace
{
    /// <summary>
    /// Interaction logic for StoreWindow.xaml
    /// </summary>
    public partial class StoreWindow : Window
    {
        private Logic logic;
        private MainWindow mw;

        public StoreWindow(Logic logic, MainWindow mw)
        {
            this.logic = logic;
            this.mw = mw;
            //List<SpaceSuit> SpaceSuits = new List<SpaceSuit>();
            //SpaceSuits.Add(new SpaceSuit(1, 2, 100, "astronaut_1", 0));
            //SpaceSuits.Add(new SpaceSuit(2, 3, 150, "astronaut_2", 50000));
            //SpaceSuits.Add(new SpaceSuit(3, 3, 500, "astronaut_3", 100000));
            //SpaceSuits.Add(new SpaceSuit(4, 5, 170, "astronaut_4", 300000));
            //File.WriteAllText(new Uri(Path.Combine("Store", "SpaceSuits.json"), UriKind.RelativeOrAbsolute).ToString(), JsonConvert.SerializeObject(SpaceSuits, Formatting.Indented));
            //SpaceSuits = JsonConvert.DeserializeObject<List<SpaceSuit>>(File.ReadAllText(new Uri(Path.Combine("Store", "SpaceSuits.json"), UriKind.RelativeOrAbsolute).ToString()));

            InitializeComponent();



            lbPurchaseableItems.ItemsSource = logic.SpaceSuits;
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
            lbMoney.Content = logic.GO_Player.Money;

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
                QuestionWindow qw = new QuestionWindow("Are you sure you want to purchase the choosen suit?", "Purchase confirm");

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