﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.HelperClasses;
using WPF_LostInSpace.Interfaces;
using WPF_LostInSpace.Store;
using WPF_LostInSpace.Userdata;

namespace WPF_LostInSpace.GameLogic
{
    public class Logic : IGameLogic, IGameController
    {

        //GameLogic Interface
        public List<GO_Background> GO_Backgrounds { get; set; }
        public List<GO_ControlPanel> GO_ControlPanels { get; set; }
        public event EventHandler EventUpdateRender;
        public List<GO_Item> GO_Items { get; set; }
        public List<GO_Laser> GO_Lasers { get; set; }
        public GO_Player GO_Player { get; set; }
        public List<byte[]> Cooldown_RGB { get; set; }
        //GameLogic Interface

        //Collections
        private List<byte[]> colors;
        private List<GO_Item_Crystal> GO_Item_Crystals;
        public List<User> Users;
        public List<SpaceSuit> SpaceSuitsSource { get; set; }
        public List<SpaceSuit> SpaceSuits { get; set; }
        private List<MediaPlayer> effect_soundtracks = null;
        //Collections


        private bool isHitHappend = false;
        private bool isCooldown = false;
        private Size playArea;
        private int playerItemDecetionDelaySec = 0;
        public event EventHandler EventStopApplication;

        public User CurrentUser { get; set; }
        private MediaPlayer music_soundtrack = null;

        public ImageBrush LaserPistol { get; set; }

        public Logic()
        {
            GO_Backgrounds = new List<GO_Background>();
            GO_Items = new List<GO_Item>();
            GO_Item_Crystals = new List<GO_Item_Crystal>();
            GO_Lasers = new List<GO_Laser>();

            colors = new List<byte[]>();
            Cooldown_RGB = new List<byte[]>();

            Users = new List<User>();
            SpaceSuitsSource = new List<SpaceSuit>();
            SpaceSuits = new List<SpaceSuit>();

            GO_Item_Crystals.Add(new GO_Item_Crystal(10));
            GO_Item_Crystals.Add(new GO_Item_Crystal(20));
            GO_Item_Crystals.Add(new GO_Item_Crystal(30));
            GO_Item_Crystals.Add(new GO_Item_Crystal(40));
            GO_Item_Crystals.Add(new GO_Item_Crystal(50));
            GO_Item_Crystals.Add(new GO_Item_Crystal(100));

            GO_Background.LoadBackgrounds();

            GO_Item_Asteroid.LoadAsteroidImages();
            GO_Item_Health.LoadHealthImages();
            GO_Item_Satellite.LoadSateliteImages();

            GO_Item_Crystal.LoadCrystalImages();
            GO_Backgrounds.Add(new GO_Background());//empty space
            GO_Backgrounds.Add(new GO_Background());//empty space with earth and moon



            GO_ControlPanels = new List<GO_ControlPanel>()
            {
            new GO_ControlPanel("controlPanel_Health.png"),
            new GO_ControlPanel("controlPanel_Distance.png"),
            new GO_ControlPanel("controlPanel_Empty_L.png"),
            new GO_ControlPanel("controlPanel_Empty_R.png")
            };

            LaserPistol = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", "laser_pistol.png"), UriKind.RelativeOrAbsolute)));


            effect_soundtracks = new List<MediaPlayer>();
            music_soundtrack = new MediaPlayer();

            var soundtrackNames = Directory.GetFiles("Soundtracks", "*.mp3").Select(Path.GetFileName).ToArray();

            ;
            for (int i = 0; i < soundtrackNames.Length; i++)
            {
                effect_soundtracks.Add(new MediaPlayer());
                effect_soundtracks[i].Open(new Uri(Path.Combine("Soundtracks", soundtrackNames[i]), UriKind.RelativeOrAbsolute));//Cracks 
            }

            music_soundtrack.Open(new Uri(Path.Combine("Soundtracks", soundtrackNames[12]), UriKind.RelativeOrAbsolute));

            SetUpColorsForLaser();
            Cooldown_RGB.Add(colors[0]);

            LoadUsersFromJson();
            LoadSpaceSuitsFromJson();
            SelectLastLoggedUser();
            SpaceSuitsByUserInventory();//SpaceSuits list is made by knowing CurrentUser(Calling order important)


            GO_Player = new GO_Player();
            SetEffectVolume();//Only when we have current user
            SetMusicVolume();//Only when we have current user

            music_soundtrack.MediaEnded += (sender, eventArgs) => { music_soundtrack.Position = TimeSpan.Zero; music_soundtrack.Play(); };
            //When background music ended, this event called which sets the duration of music zero, and starts play it again.

            music_soundtrack.Play();
        }

        ///********************************************************************Soundtracks methods
        public void SetEffectVolume()
        {
            effect_soundtracks.ForEach(s => s.Volume = CurrentUser.EffectVolume);
        }

        public void SetMusicVolume()
        {
            music_soundtrack.Volume = CurrentUser.MusicVolume;
        }
        public void PlayPurchaseSound()
        {
            PlaySoundtrackById(13);
        }

        private void PlaySoundtrackById(int id)
        {
            effect_soundtracks[id].Play();
            effect_soundtracks[id].Position = TimeSpan.Zero;
        }
        public void PlayPickUpSuit()
        {
            PlaySoundtrackById(14);
        }
        ///********************************************************************Soundtracks methods

        ///********************************************************************Current User related methods
        public void SpaceSuitsByUserInventory()
        {
            SpaceSuits.Clear();
            var owned = SpaceSuitsSource.Where(t => CurrentUser.PurchasedSpaceSuitIDX.Contains(t.ID)).ToList();
            var notOwned = SpaceSuitsSource.Where(t => !CurrentUser.PurchasedSpaceSuitIDX.Contains(t.ID)).ToList();

            owned.ForEach(t =>
            {
                var copySpaceSuit = t.DeppCopy();
                SpaceSuits.Add(copySpaceSuit);
                copySpaceSuit.Price = 0;
            });

            notOwned.ForEach(t => SpaceSuits.Add(t.DeppCopy()));

            SpaceSuits = SpaceSuits.OrderBy(ss => ss.ID).ToList();

        }

        public void SelectLastLoggedUser()
        {
            CurrentUser = Users.OrderByDescending(u => u.LastLogin).First();//theres always be at least one user in json
        }
        ///********************************************************************Current User related methods

        ///********************************************************************SetUpMethods
        public void SetUpPlayArea(Size playArea)
        {
            this.playArea = playArea;
        }

        public void SetUpBackground()//Put background in the middle
        {
            GO_Backgrounds[0].BackgroundSize = new Size(playArea.Width - 500, (playArea.Height * 2));
            GO_Backgrounds[0].BackgroundPoint = new Point((int)(playArea.Width / 2) - (int)(GO_Backgrounds[0].BackgroundSize.Width / 2), 0);

            GO_Backgrounds[1].BackgroundSize = GO_Backgrounds[0].BackgroundSize;
            GO_Backgrounds[1].BackgroundPoint = new Point(GO_Backgrounds[0].BackgroundPoint.X, GO_Backgrounds[0].BackgroundSize.Height - 1);
            //-1=> white line between two backgrounds

            /*(int)((PlayArea.Width / 2) - (GO_Background.BackgroundSize.Width / 2)),/*(int)(playArea.Height / 2)*/
            EventUpdateRender?.Invoke(this, null);
        }

        public void SetUpPanels()
        {
            //Panel order int collection: health, distance, empty_L, empty_R


            //distance panel starts at the end of the background of x-axis(top right corner)
            GO_ControlPanels[0].ControlPanelPoint = new Point(GO_Backgrounds[0].BackgroundPoint.X + GO_Backgrounds[0].BackgroundSize.Width, 0);
            //distance panel starts from the end of the bg(top right corner), and goes to the end of the window->
            //windows width-from (0,0) to axis X - background size =>difference between right side of the windows and the right side of the background
            GO_ControlPanels[0].ControlPanelSize = new Size(playArea.Width - GO_Backgrounds[0].BackgroundPoint.X - GO_Backgrounds[0].BackgroundSize.Width, 200);





            //health panel starts from top left corner, and the width of it is the start of the background, which is the axis X
            GO_ControlPanels[1].ControlPanelPoint = new Point(0, 0);
            //first one starts from 0,0, so the width of this panel is equals with the axis X of background(bg starts here->this is how wide panel we need)
            GO_ControlPanels[1].ControlPanelSize = new Size(GO_Backgrounds[0].BackgroundPoint.X, 200);



            GO_ControlPanels[2].ControlPanelPoint = new Point(0, GO_ControlPanels[0].ControlPanelSize.Height);
            GO_ControlPanels[2].ControlPanelSize = new Size(GO_Backgrounds[0].BackgroundPoint.X, playArea.Height - GO_ControlPanels[0].ControlPanelSize.Height);

            GO_ControlPanels[3].ControlPanelPoint = new Point(GO_ControlPanels[0].ControlPanelSize.Width + GO_Backgrounds[0].BackgroundSize.Width, GO_ControlPanels[1].ControlPanelSize.Height);
            GO_ControlPanels[3].ControlPanelSize = new Size(playArea.Width - GO_ControlPanels[0].ControlPanelSize.Width - GO_Backgrounds[0].BackgroundSize.Width, playArea.Height - GO_ControlPanels[1].ControlPanelSize.Height);

            EventUpdateRender?.Invoke(this, null);
        }

        public void SetUpPlayer()
        {
            GO_Player.PlayerBrush = SpaceSuits.Where(ss => ss.ID == CurrentUser.LastSuitID).First().SpaceSuitBrush_R;
            GO_Player.PlayerBrushLeft = SpaceSuits.Where(ss => ss.ID == CurrentUser.LastSuitID).First().SpaceSuitBrush_L;
            GO_Player.PlayerBrushRight = SpaceSuits.Where(ss => ss.ID == CurrentUser.LastSuitID).First().SpaceSuitBrush_R;

            GO_Player.Name = CurrentUser.Username;
            GO_Player.BestDistance = CurrentUser.BestDistance;
            GO_Player.TotalDistance = CurrentUser.TotalDistance;

            GO_Player.Health = SpaceSuits.Where(ss => ss.ID == CurrentUser.LastSuitID).First().Health;//Health we change
            GO_Player.HealthConstant = SpaceSuits.Where(ss => ss.ID == CurrentUser.LastSuitID).First().Health;//Health we use for logic
            GO_Player.Speed = SpaceSuits.Where(ss => ss.ID == CurrentUser.LastSuitID).First().Speed;

            GO_Player.Money = CurrentUser.Money;
           

            GO_Player.PlayerSize = new Size((playArea.Width / 20), (playArea.Height / 8));//50-25 ___ 100-50__GO_Player.PlayerSize = new Size((PlayArea.Width / 8), (PlayArea.Height / 16));
            GO_Player.PlayerPoint = new Point((int)((playArea.Width / 2) - (GO_Player.PlayerSize.Width / 2)), 20);
            EventUpdateRender?.Invoke(this, null);

        }

        private void SetUpColorsForLaser()
        {
            byte r = 0;
            byte g = 255;
            byte b = 0;

            for (int i = 0; i < 20; i++)
            {
                colors.Add(new byte[] { r, g, b });

                if (r < 255)
                {
                    r += 51;
                }
                else if (r == 255 && g > 0)
                {
                    g -= 15;
                }
            }

        }

        ///********************************************************************SetUpMethods


        ///********************************************************************Generate/Move GameObject Items, Move background

        public void BackgroundMove()
        {

            for (int i = 0; i < GO_Backgrounds.Count; i++)
            {
                bool isBackgroundIn = GO_Backgrounds[i].Move(playArea);

                if (!isBackgroundIn)
                {
                    GO_Backgrounds.RemoveAt(i);
                    GO_Backgrounds.Add(new GO_Background());

                    GO_Backgrounds[1].BackgroundSize = GO_Backgrounds[0].BackgroundSize;
                    GO_Backgrounds[1].BackgroundPoint = new Point((int)(playArea.Width / 2) - (int)(GO_Backgrounds[0].BackgroundSize.Width / 2), GO_Backgrounds[0].BackgroundSize.Height - 1);
                }
            }
            //    Trace.WriteLine(GO_Backgrounds[1]  != null? GO_Backgrounds[1].BackgroundPoint:"GONE");

            GO_Player.Distance += 0.001;
            GO_Player.Distance = Math.Round(GO_Player.Distance, 3);
            EventUpdateRender?.Invoke(this, null);
        }

        public void GenerateAsteroid()
        {
            GO_Item_Asteroid goia = new GO_Item_Asteroid();//goia=game object item asteroid
            CreateNewGO_Item_ByParameter(goia);
        }

        public void GenerateHealth()
        {
            GO_Item_Health goih = new GO_Item_Health();//goih=game object item health
            CreateNewGO_Item_ByParameter(goih);
        }

        public void GenerateSatellite()
        {
            GO_Item_Satellite gois = new GO_Item_Satellite();//gois=game object item satellite
            CreateNewGO_Item_ByParameter(gois);
        }

        public void GenerateCrystal()
        {
            GO_Item_Crystal choosenCrystal = GO_Item_Crystals[Utils.rnd.Next(0, GO_Item_Crystals.Count)];//goic=game object item crystals
            GO_Item_Crystal goic = new GO_Item_Crystal(choosenCrystal.Value);
            CreateNewGO_Item_ByParameter(goic);
        }

        private void CreateNewGO_Item_ByParameter(GO_Item goItem)
        {
            goItem.SetBrushWithImage();//choose random image, set it's object brush to it

            int from = (int)GO_Backgrounds[0].BackgroundPoint.X;
            int to = (int)(GO_Backgrounds[0].BackgroundPoint.X + GO_Backgrounds[0].BackgroundSize.Width - goItem.ItemSize.Width);

            goItem.ItemPoint = new Point(Utils.rnd.Next(from, to), playArea.Height);
            //put down this object from bg left side to bg right side on random pos(within background=bg) 
            //PlayArea.Height->the object gonna be outside of layer view, to see how generating works, say: PlayArea.Height-100
            GO_Items.Add(goItem);

            EventUpdateRender?.Invoke(this, null);//refresh display
        }

        public void ItemMove()
        {
            //we are calling it every 1 millisec, and if the object out of map, we remove it->Move() can return true if moveable, false if out of bounds
            for (int i = 0; i < GO_Items.Count; i++)
            {
                bool withinBoundary = GO_Items[i].Move();

                if (!withinBoundary)
                {
                    GO_Items.RemoveAt(i);
                }
            }

            EventUpdateRender?.Invoke(this, null);
        }
        ///********************************************************************Generate/Move GameObject Items, Move background

        ///********************************************************************Methods for Player
        public void MovePlayer(PlayerController pc)
        {
            ////Player cant leave background
            ////if boundary was crossed we throw the player back with 1 pixel

            ////Idea is if player leaves background(it can), then we +-5 to it's position X, and ends up at the last position where it can move freely->happens fast,cannot be seen. 

            if (GO_Player.PlayerPoint.X < GO_Backgrounds[0].BackgroundPoint.X)
            {
                GO_Player.PlayerPoint = new Point(GO_Player.PlayerPoint.X + 5, GO_Player.PlayerPoint.Y);                                                                                                     //eg: if I write 1, then the player could go to the left with 4.
            }

            ////We are comparing the right top corner of player to the right top corner of background/gamearea

            if (GO_Player.PlayerPoint.X + GO_Player.PlayerSize.Width > GO_Backgrounds[0].BackgroundPoint.X + GO_Backgrounds[0].BackgroundSize.Width)
            {
                GO_Player.PlayerPoint = new Point(GO_Player.PlayerPoint.X - 5, GO_Player.PlayerPoint.Y);
            }

            ////Trace.WriteLine("Move: " + GO_Player.PlayerPoint.X);

            GO_Player.Move(pc);
            EventUpdateRender?.Invoke(this, null);

        }

        public void ShootLaser()//Create Laser object__Cooldown/Music might come here later
        {
            if (!isCooldown)
            {
                GO_Laser gol = new GO_Laser();
                gol.LaserPoint = new Point(GO_Player.PlayerPoint.X + GO_Player.PlayerSize.Width / 2, GO_Player.PlayerPoint.Y + 50);
                gol.LaserSize = new Size(10, 50);
                GO_Lasers.Add(gol);

                PlaySoundtrackById(Utils.rnd.Next(8, 11));

                EnlargeLaserCooldown();
                EventUpdateRender?.Invoke(this, null);//refresh display
            }
            else
            {
                PlaySoundtrackById(0);
            }
        }

        public void MoveLaser()
        {
            //if laser hits asteroid or satellite

            for (int i = 0; i < GO_Lasers.Count; i++)
            {
                if (GO_Lasers[i].LaserPoint.Y >= playArea.Height - 100)
                {
                    GO_Lasers.Remove(GO_Lasers[i]);
                }
                else
                {
                    GO_Lasers[i].MoveLaser();
                }
            }

            EventUpdateRender?.Invoke(this, null);

        }

        public void CheckLaserItemDetection()
        {

            for (int i = 0; i < GO_Items.Count; i++)
            {
                for (int j = 0; j < GO_Lasers.Count; j++)
                {

                    if (i < GO_Items.Count)
                    {
                        Rect laserRect = new Rect(GO_Lasers[j].LaserPoint.X, GO_Lasers[j].LaserPoint.Y, GO_Lasers[j].LaserSize.Width, GO_Lasers[j].LaserSize.Height);
                        Rect itemRect = new Rect(GO_Items[i].ItemPoint.X, GO_Items[i].ItemPoint.Y, GO_Items[i].ItemSize.Width, GO_Items[i].ItemSize.Height);

                        bool isLaserItemDet = laserRect.IntersectsWith(itemRect);

                        if (isLaserItemDet)
                        {
                            GO_Lasers.RemoveAt(j);
                            GO_Items[i].SetExplosionBrush();
                            GO_Items[i].IsExplode = true;
                        }
                    }
                }
            }
        }

        public void RemoveExplodedItem()
        {
            for (int i = 0; i < GO_Items.Count; i++)
            {
                if (GO_Items[i].IsExplode)
                {
                    GO_Items.RemoveAt(i);

                    PlaySoundtrackById(Utils.rnd.Next(4, 6));
                }
            }
        }

        const int HEALTH_FOR_PLAYER = 5;

        public void CheckPlayerItemDetection()
        {
            Rect pRect = new Rect(GO_Player.PlayerPoint.X, GO_Player.PlayerPoint.Y, GO_Player.PlayerSize.Width, GO_Player.PlayerSize.Height);

            for (int i = 0; i < GO_Items.Count; i++)
            {
                Rect iRect = new Rect(GO_Items[i].ItemPoint.X, GO_Items[i].ItemPoint.Y, GO_Items[i].ItemSize.Width, GO_Items[i].ItemSize.Height);

                bool hit = pRect.IntersectsWith(iRect);

                if (hit)
                {
                    isHitHappend = true;
                    if (playerItemDecetionDelaySec >= 1)
                    {
                        switch (GO_Items[i])
                        {
                            case GO_Item_Asteroid:
                                if (GO_Player.Money - (int)((GO_Items[i] as GO_Item_Asteroid).ItemSize.Width - 30) > 0)
                                {
                                    GO_Player.Money -= (int)((GO_Items[i] as GO_Item_Asteroid).ItemSize.Width - 30);
                                }
                                else
                                {
                                    GO_Player.Money = 0;
                                }
                                ReduceHealth((GO_Items[i] as GO_Item_Asteroid).ItemSize.Width);
                                GO_Items.RemoveAt(i);
                                break;
                            case GO_Item_Health:

                                if (GO_Player.Health < GO_Player.HealthConstant)
                                {
                                    GO_Items.RemoveAt(i);
                                    if (GO_Player.Health + HEALTH_FOR_PLAYER < GO_Player.HealthConstant)
                                    {
                                        GO_Player.Health += HEALTH_FOR_PLAYER;
                                    }
                                    else
                                    {
                                        GO_Player.Health = GO_Player.HealthConstant;
                                    }
                                    PlaySoundtrackById(6);
                                }
                                break;
                            case GO_Item_Satellite:

                                if (GO_Player.Money - (int)((GO_Items[i] as GO_Item_Satellite).ItemSize.Width - 30) > 0)
                                {
                                    GO_Player.Money -= (int)((GO_Items[i] as GO_Item_Satellite).ItemSize.Width - 30);//in best case:20, in worst case 35
                                }
                                else
                                {
                                    GO_Player.Money = 0;
                                }
                                ReduceHealth((GO_Items[i] as GO_Item_Satellite).ItemSize.Width - 19);
                                GO_Items.RemoveAt(i);
                                break;
                            case GO_Item_Crystal:
                                GO_Player.Money += (GO_Items[i] as GO_Item_Crystal).Value;
                                PlaySoundtrackById(15);
                                GO_Items.RemoveAt(i);
                                break;
                            default:
                                break;
                        }
                        isHitHappend = false;

                        playerItemDecetionDelaySec = 0;

                    }
                }
            }

            EventUpdateRender?.Invoke(this, null);

        }

        public void PlayerItemDetectionDelay()
        {
            if (isHitHappend)
            {
                playerItemDecetionDelaySec++;
            }
        }

        private void ReduceHealth(double value)
        {
            int amount = (int)value;

            if (GO_Player.Health > 0)
            {
                if (GO_Player.Health - (amount - 19) <= 0)
                {
                    GO_Player.Health = 0;
                    PlaySoundtrackById(7);
                    EventStopApplication.Invoke(this, null);
                }
                else
                {
                    GO_Player.Health -= (amount - 19);
                    PlaySoundtrackById(3);
                }

                if (GO_Player.Health <= 10 && GO_Player.Health > 0)
                {
                    PlaySoundtrackById(11);
                }
            }
        }
        ///********************************************************************Methods for Player


        ///********************************************************************Methods for Cooldown
        public void ReduceLaserCooldown()
        {
            if (Cooldown_RGB.Count > 1)
            {
                int a = Cooldown_RGB.Count - 1;

                Cooldown_RGB.RemoveAt(a);


                EventUpdateRender?.Invoke(this, null);
            }


            if (Cooldown_RGB.Count == 1)
            {
                if (isCooldown == true)
                {
                    PlaySoundtrackById(1);
                }
                isCooldown = false;

            }



        }
        private void EnlargeLaserCooldown()//every time we shoot, 3 squares added to crgbs list.
        {
            for (int i = 1; i <= 3; i++)
            {
                if (Cooldown_RGB.Count - 1 + i < colors.Count)
                {
                    Cooldown_RGB.Add(colors[Cooldown_RGB.Count - 1 + i]);
                }
            }

            if (Cooldown_RGB.Count >= 18)
            {
                isCooldown = true;
                PlaySoundtrackById(2);
            }


        }
        ///********************************************************************Methods for Cooldown


        ///********************************************************************Reset game
        public void ResetGame()//GoingBack Main menu when Player dies/ Main Menu button pressed
        {
            //Lasers/Items
            GO_Lasers.Clear();
            GO_Items.Clear();
            //Lasers/Items


            //Cooldown
            Cooldown_RGB.Clear();
            Cooldown_RGB.Add(colors[0]);
            //Cooldown

            //Sace distance/Player Money
            CurrentUser.Money = GO_Player.Money;
            CurrentUser.TotalDistance += GO_Player.Distance;
            CurrentUser.TotalDistance = Math.Round(CurrentUser.TotalDistance, 3);
            CurrentUser.BestDistance = GO_Player.Distance > GO_Player.BestDistance ? GO_Player.Distance : GO_Player.BestDistance;
            CurrentUser.BestDistance = Math.Round(CurrentUser.BestDistance, 3);
            //if current ditance better than bestdistance, override bestdistance with distance,
            //on the contrary override it with the same bestdistance cause it hasn't been changed

            //Player
            SetUpPlayer();
            GO_Player.Distance = 0;
            //Player

            //Background
            GO_Backgrounds.Clear();
            GO_Background.IndexForFirstTwoBackgrunds = 0;
            GO_Backgrounds.Add(new GO_Background());//empty space
            GO_Backgrounds.Add(new GO_Background());//empty space with earth and moon
            SetUpBackground();
            //Background

            EventUpdateRender?.Invoke(this, null);

            SaveUsersToJson();

        }
        ///********************************************************************Reset game

        ///********************************************************************Load from/Saveto Json 
        public void SaveUsersToJson()
        {
            File.WriteAllText(new Uri(Path.Combine("Userdata", "Userdata.json"), UriKind.RelativeOrAbsolute).ToString(), JsonConvert.SerializeObject(Users, Formatting.Indented));
        }
        public void LoadUsersFromJson()
        {

            if (!File.Exists(Path.Combine("Userdata", "Userdata.json")))
            {
                User u = new User();
                u.Username = "FirstUser";
                u.Money = 5000000;
                u.BestDistance = 0;
                u.TotalDistance = 0;
                u.LastLogin = DateTime.Now;
                u.LastSuitID = 1;
                u.PurchasedSpaceSuitIDX = new List<int> { 1 };
                u.MusicVolume = 0.2;
                u.EffectVolume = 0.2;
                Users.Add(u);
                if (!Directory.Exists("Userdata"))
                {
                    Directory.CreateDirectory("Userdata");
                }
                SaveUsersToJson();
            }
            else
            {
                Users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(new Uri(Path.Combine("Userdata", "Userdata.json"), UriKind.RelativeOrAbsolute).ToString()));
            }
        }
        public void LoadSpaceSuitsFromJson()
        {
            SpaceSuitsSource = JsonConvert.DeserializeObject<List<SpaceSuit>>(File.ReadAllText(new Uri(Path.Combine("Store", "SpaceSuits.json"), UriKind.RelativeOrAbsolute).ToString()));
        }
        ///********************************************************************Load from/Saveto Json 


        ///********************************************************************Open Settings/UserManagement/Store
        public void OpenSettings(MainWindow mainWindow)
        {
            SettingsWindow sw = new SettingsWindow(mainWindow, this);
            sw.Show();

        }
        public void OpenStore(MainWindow mw)
        {
            StoreWindow sw = new StoreWindow(this, mw);
            sw.Show();
        }
        public void OpenUsers(MainWindow mainWindow)
        {
            UserManagementWindow umw = new UserManagementWindow(mainWindow, this);
            umw.Show();
        }
        ///********************************************************************Open Settings/UserManagement/Store
    }
}
