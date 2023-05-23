using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_LostInSpace.GameLogic;
using WPF_LostInSpace.Store;


namespace WPF_LostInSpace
{
    public partial class StoreWindow : Window
    {
        private Logic logic;
        private MainWindow mw;
        private static ImageBrush storeBackground = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "store_BG.jpg"), UriKind.RelativeOrAbsolute)));
        private static BitmapFrame icon = BitmapFrame.Create(new BitmapImage(new Uri(Path.Combine("Images", "Icons", "MainWindowIcon.ico"), UriKind.RelativeOrAbsolute)));

        public StoreWindow(Logic logic, MainWindow mw)
        {
            this.logic = logic;
            this.mw = mw;
            Icon = icon;

            InitializeComponent();



            lbPurchaseableItems.ItemsSource = logic.SpaceSuits;
        }

        private SpaceSuit selectedSuit;

        private void lbPurchaseableItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //selectedItem
            selectedSuit = ((sender as ListBox).SelectedItem as SpaceSuit);

            if (selectedSuit.Price == 0)
            {
                btPurchChoose.Content = "Choose";
            }
            else
            {
                btPurchChoose.Content = "Purchase";
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Background = storeBackground;
            this.Background.Opacity = 0.5;


            lbActiveSpaceSuitTitle.Foreground = Brushes.White;
            lbActiveSpaceSuitHealth.Foreground = Brushes.Red;
            lbActiveSpaceSuitSpeed.Foreground = Brushes.White;

            lbMoneyTitle.Foreground = Brushes.Green;
            lbMoney.Foreground = Brushes.Green;


            selectedSuit = logic.SpaceSuits.Where(ss => ss.ID == logic.CurrentUser.LastSuitID).First();

            lbMoney.Content = logic.CurrentUser.Money + " Ł";

            SetSelectedSuitImage();
        }

        private void SetSelectedSuitImage()
        {
            Image myImage = new Image();
            BitmapImage myImageSource = new BitmapImage();
            myImageSource.BeginInit();
            myImageSource.UriSource = new Uri(selectedSuit.SpaceSuitResPath, UriKind.RelativeOrAbsolute);
            myImageSource.EndInit();

            myImage.Source = myImageSource;

            lbActiveSpaceSuitImgRes.Content = myImage;
            lbActiveSpaceSuitHealth.Content = "Health: " + selectedSuit.Health + "+";
            lbActiveSpaceSuitSpeed.Content = "Speed: " + selectedSuit.Speed + " m/s";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.EnableDisableMainMenuButtons(true);
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_Purchase_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSuit != null)
            {
                QuestionWindow qw;
                if (selectedSuit.Price == 0)
                {

                    qw = new QuestionWindow("Are you sure you want to wear the choosen suit?", "Wear confirm");

                    if (qw.ShowDialog() == true)
                    {
                        SetSelectedSuitImage();

                        logic.CurrentUser.LastSuitID = selectedSuit.ID;//we first set lastUitId->here user alredy confirmed it
                        logic.SetUpPlayer();//we call setup player in logic which uses current user, and current user's object's lastsuitid prop

                        logic.PlayPickUpSuit();

                        logic.SaveUsersToJson();
                    }

                }
                else
                {
                    qw = new QuestionWindow("Are you sure you want to purchase the choosen suit?", "Purchase confirm");

                    if (qw.ShowDialog() == true)
                    {
                        //GO_PLayer__Current user money
                        if (logic.CurrentUser.Money >= selectedSuit.Price)
                        {

                            logic.PlayPurchaseSound();

                            //User can buy space suits, not GO_Player, that is why we do modifications User's data, then we modify GO_Player data by the User's data(Sync)
                            logic.CurrentUser.Money -= selectedSuit.Price;
                            logic.CurrentUser.PurchasedSpaceSuitIDX.Add(selectedSuit.ID);
                            var selectedSuitFromSpaceSuitsList = logic.SpaceSuits.Where(ss => ss.ID == selectedSuit.ID).First();
                            selectedSuitFromSpaceSuitsList.Price = 0;

                            logic.GO_Player.Money = logic.CurrentUser.Money;
                            logic.GO_Player.Health = selectedSuit.Health;
                            logic.GO_Player.Speed = selectedSuit.Speed;
                            logic.CurrentUser.LastSuitID = selectedSuit.ID;

                            lbMoney.Content = logic.CurrentUser.Money;
                            SetSelectedSuitImage();

                            logic.SetUpPlayer();

                            logic.PlayPickUpSuit();

                            lbPurchaseableItems.Items.Refresh();

                            logic.SaveUsersToJson();


                        }
                        else
                        {
                            WarningWindow ww = new WarningWindow("You don't have enough money to purchase this suit!", "Not enough money!");
                            ww.Show();
                        }
                    }
                }
            }
        }

    }
}