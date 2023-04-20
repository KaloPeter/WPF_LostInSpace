using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.Interfaces;

namespace WPF_LostInSpace.GameLogic
{
    public class Logic : IGameLogic, IGameController
    {

        private Size playArea;

        public event EventHandler EventUpdateRender;

        public List<GO_Background> GO_Backgrounds { get; set; }

        public Logic()
        {
            GO_Backgrounds = new List<GO_Background>();
            GO_Background.LoadBackgrounds();




            GO_Backgrounds.Add(new GO_Background());//empty space
            GO_Backgrounds.Add(new GO_Background());//empty space with earth and moon


        }

        public void SetUpPlayArea(Size playArea)
        {
            this.playArea = playArea;
        }


        public void SetUpBackground()//Put background in the middle
        {
            GO_Backgrounds[0].BackgroundSize = new Size(playArea.Width - 500, playArea.Height * 2);
            GO_Backgrounds[0].BackgroundPoint = new Point((int)(playArea.Width / 2) - (int)(GO_Backgrounds[0].BackgroundSize.Width / 2), 0);

            GO_Backgrounds[1].BackgroundSize = GO_Backgrounds[0].BackgroundSize;
            GO_Backgrounds[1].BackgroundPoint = new Point(GO_Backgrounds[0].BackgroundPoint.X, GO_Backgrounds[0].BackgroundSize.Height);

            /*(int)((PlayArea.Width / 2) - (GO_Background.BackgroundSize.Width / 2)),/*(int)(playArea.Height / 2)*/
            EventUpdateRender?.Invoke(this, null);
        }


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
                    GO_Backgrounds[1].BackgroundPoint = new Point((int)(playArea.Width / 2) - (int)(GO_Backgrounds[0].BackgroundSize.Width / 2), GO_Backgrounds[0].BackgroundSize.Height);
                }
            }

            //GO_Player.Distance += 0.001;
            //GO_Player.Distance = Math.Round(GO_Player.Distance, 3);
            EventUpdateRender?.Invoke(this, null);
        }
    }
}
