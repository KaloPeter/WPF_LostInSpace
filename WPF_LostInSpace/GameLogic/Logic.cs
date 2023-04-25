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
        public List<GO_ControlPanel> GO_ControlPanels { get; set; }

        public Logic()
        {
            GO_Backgrounds = new List<GO_Background>();
            GO_Background.LoadBackgrounds();
            GO_Backgrounds.Add(new GO_Background());//empty space
            GO_Backgrounds.Add(new GO_Background());//empty space with earth and moon



            GO_ControlPanels = new List<GO_ControlPanel>();
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Health.png"));
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Distance.png"));
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Empty_L.png"));
            GO_ControlPanels.Add(new GO_ControlPanel("controlPanel_Empty_R.png"));
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
        public void SetUpPanels()
        {
            //Panel order int collection: health, distance, empty_L, empty_R

            //health panel starts from top left corner, and the width of it is the start of the background, which is the axis X
            GO_ControlPanels[1].ControlPanelPoint = new Point(0, 0);
            //first one starts from 0,0, so the width of this panel is equelas with the axis X of background(bg starts here->this is how wide panel we need)
            GO_ControlPanels[1].ControlPanelSize = new Size(GO_Backgrounds[0].BackgroundPoint.X, 200);


            //second panel starts at the end of the background of x-axis(top right corner)
            GO_ControlPanels[0].ControlPanelPoint = new Point(GO_Backgrounds[0].BackgroundPoint.X + GO_Backgrounds[0].BackgroundSize.Width, 0);
            //second panel starts from the end of the bg(top right corner), and goes to the end of the window->
            //windows width-from (0,0) to axis X - background size =>difference between right side of the windows and the right side of the background
            GO_ControlPanels[0].ControlPanelSize = new Size(playArea.Width - GO_Backgrounds[0].BackgroundPoint.X - GO_Backgrounds[0].BackgroundSize.Width, 200);


            GO_ControlPanels[2].ControlPanelPoint = new Point(0, GO_ControlPanels[0].ControlPanelSize.Height);
            GO_ControlPanels[2].ControlPanelSize = new Size(GO_Backgrounds[0].BackgroundPoint.X, playArea.Height - GO_ControlPanels[0].ControlPanelSize.Height);

            GO_ControlPanels[3].ControlPanelPoint = new Point(GO_ControlPanels[0].ControlPanelSize.Width + GO_Backgrounds[0].BackgroundSize.Width, GO_ControlPanels[1].ControlPanelSize.Height);
            GO_ControlPanels[3].ControlPanelSize = new Size(playArea.Width - GO_ControlPanels[0].ControlPanelSize.Width - GO_Backgrounds[0].BackgroundSize.Width, playArea.Height - GO_ControlPanels[1].ControlPanelSize.Height);







            EventUpdateRender?.Invoke(this, null);
        }
    }
}
