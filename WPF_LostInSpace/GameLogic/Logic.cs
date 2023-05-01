using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.HelperClasses;
using WPF_LostInSpace.Interfaces;

namespace WPF_LostInSpace.GameLogic
{
    public class Logic : IGameLogic, IGameController
    {

        private Size playArea;

        public event EventHandler EventUpdateRender;

        public List<GO_Background> GO_Backgrounds { get; set; }
        public List<GO_ControlPanel> GO_ControlPanels { get; set; }
        public List<GO_Item> GO_Items { get; set; }

        public List<GO_Laser> GO_Lasers { get; set; }

        public GO_Player GO_Player { get; set; }

        private List<GO_Item_Crystal> GO_Item_Crystals;


        private int sec = 0;

        private bool isHitHappend = false;


        private List<byte[]> colors;

        public List<byte[]> Cooldown_RGB { get; set; }

        private bool isCooldown = false;



        //***********************************************************************************
        //***********************************************************************************

        public Logic()
        {
            GO_Backgrounds = new List<GO_Background>();
            GO_Items = new List<GO_Item>();
            GO_Item_Crystals = new List<GO_Item_Crystal>();
            GO_Lasers = new List<GO_Laser>();

            colors = new List<byte[]>();
            Cooldown_RGB = new List<byte[]>();



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



            GO_ControlPanels = new List<GO_ControlPanel>();
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Health.png"));
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Distance.png"));
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Empty_L.png"));
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Empty_R.png"));

            SetUpColorsForLaser();
            Cooldown_RGB.Add(colors[0]);

            GO_Player = new GO_Player();
        }

        ///////////////////////////setUp methods

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
            //-1=> két background közöti fehér vonal

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
            GO_Player.PlayerSize = new Size((playArea.Width / 20), (playArea.Height / 8));//50-25 ___ 100-50__GO_Player.PlayerSize = new Size((PlayArea.Width / 8), (PlayArea.Height / 16));
            GO_Player.PlayerPoint = new Point((int)((playArea.Width / 2) - (GO_Player.PlayerSize.Width / 2)), 20);
        }
        ///////////////////////////setUp methods



        ///////////////////////////GENERATE GO_Items

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

        ///////////////////////////GENERATE GO_Items

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


        ///////////////////////////Methods for Player/Laser

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
                gol.LaserPoint = new Point(GO_Player.PlayerPoint.X + GO_Player.PlayerSize.Width / 2, GO_Player.PlayerPoint.Y + 50);//CHANGE IT
                gol.LaserSize = new Size(10, 50);
                GO_Lasers.Add(gol);
                EnlargeLaserCooldown();
                EventUpdateRender?.Invoke(this, null);//refresh display
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
                            GO_Items.RemoveAt(i);
                            GO_Lasers.RemoveAt(j);

                            //media_asteroidDest.IsMuted = false;
                            //media_asteroidDest.Stop();
                            //media_asteroidDest.Play();
                            //different sounds for asteroid, satellite, then check items list element here


                        }
                    }
                    else
                    {
                        ;//for debugging
                    }
                }
            }

            /*
            There is a bug which is now being avoided by using the: if(i < Items.Count){...}.
            The bug/problem: When we shoot a laser, and a collision happens, we remove the laser and the item as well.
            But when we remove an item in the deepest part of the loops, the outer loop(which iterates on items), is not updated, so
            because of the  "Rect itemRect = new Rect(Items[i]...." gonna try to refer to
            an item in the list, that does not exist. So the i can be equal to Items.Count, and if our collection's count is 10, it's gonna
            try to refer to the Items[10], but that does not exist(we can index it only from 0 to 9)
            There might be a connection, when an Item leaves the play area, and another one is destroyed by a laser.

            It mighr depend on timing(faster asteroids, slower laser vice versa....)

            */
        }


        const int HEALTH = 5;

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
                    if (sec >= 1)
                    {
                        switch (GO_Items[i])
                        {
                            case GO_Item_Asteroid:
                                reduceHealth((GO_Items[i] as GO_Item_Asteroid).ItemSize.Width);
                                if (GO_Player.Money > 0)
                                {
                                    GO_Player.Money -= (int)((GO_Items[i] as GO_Item_Asteroid).ItemSize.Width - 30);
                                }

                                break;
                            case GO_Item_Health:

                                if (GO_Player.Health < 100)
                                {

                                    if (GO_Player.Health + HEALTH < 100)
                                    {
                                        GO_Player.Health += HEALTH;
                                    }
                                    else
                                    {
                                        GO_Player.Health = 100;
                                    }
                                }
                                break;
                            case GO_Item_Satellite:
                                reduceHealth((GO_Items[i] as GO_Item_Satellite).ItemSize.Width - 19);
                                if (GO_Player.Money > 0)
                                {
                                    GO_Player.Money -= (int)((GO_Items[i] as GO_Item_Satellite).ItemSize.Width - 30);//legjobb esetben:20, legrosszabb esetben 35-öt veszit
                                }

                                break;
                            case GO_Item_Crystal:
                                GO_Player.Money += (GO_Items[i] as GO_Item_Crystal).Value;
                                break;
                            default:
                                break;
                        }
                        isHitHappend = false;
                        GO_Items.RemoveAt(i);
                        sec = 0;

                    }



                    //if (GO_Player.Health <= 0)
                    //{
                    //    stopDisp?.Invoke(null, null);
                    //}


                }
            }

            EventUpdateRender?.Invoke(this, null);

        }

        public void PlayerItemDetectionDelay()
        {
            if (isHitHappend)
            {
                sec++;
            }
        }

        private void reduceHealth(double value)//goi=game object item
        {
            int amount = (int)value;

            if (GO_Player.Health > 0)
            {
                if (GO_Player.Health - (amount - 19) <= 0)
                {
                    GO_Player.Health = 0;
                }
                else
                {
                    GO_Player.Health -= (amount - 19);
                }

                //if (GO_Player.Health <= 10 && GO_Player.Health > 0)
                //{
                //    media_lowHealth.IsMuted = false;
                //    media_lowHealth.Stop();
                //    media_lowHealth.Play();
                //}
            }
        }


        ///////////////////////////Methods for Player

        public void ReduceLaserCooldown()//public, because Dispatcher will call this.
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
                    //media_cooldown[0].IsMuted = false;
                    //media_cooldown[0].Stop();
                    //media_cooldown[0].Play();
                }
                isCooldown = false;

            }



        }

        private void EnlargeLaserCooldown()//private, because in logic every time we shoot, according to current alg. 3 squares being added to crgbs list.
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
                //media_cooldown[1].IsMuted = false;
                //media_cooldown[1].Stop();
                //media_cooldown[1].Play();
                isCooldown = true;
            }


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

    }
}
