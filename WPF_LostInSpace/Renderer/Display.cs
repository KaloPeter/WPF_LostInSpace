using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPF_LostInSpace.GameObjects;
using WPF_LostInSpace.Interfaces;

namespace WPF_LostInSpace.Renderer
{
    public class Display : FrameworkElement
    {
        private IGameLogic logic;

        public void SetUpLogic(IGameLogic logic)
        {
            this.logic = logic;
            logic.EventUpdateRender += (sender, eventArgs) => { this.InvalidateVisual(); };
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //Background
            for (int i = 0; i < logic.GO_Backgrounds.Count; i++)
            {
                drawingContext.DrawRectangle(
                    logic.GO_Backgrounds[i].BackgroundBrush,
                    new Pen(Brushes.Red, 2),
                    new Rect(logic.GO_Backgrounds[i].BackgroundPoint.X, logic.GO_Backgrounds[i].BackgroundPoint.Y,
                    logic.GO_Backgrounds[i].BackgroundSize.Width, logic.GO_Backgrounds[i].BackgroundSize.Height)
                    );
            }


            //Player
            drawingContext.DrawRectangle(
                logic.GO_Player.PlayerBrush,
                new Pen(Brushes.Red, 2),
                new Rect(logic.GO_Player.PlayerPoint.X, logic.GO_Player.PlayerPoint.Y, logic.GO_Player.PlayerSize.Width, logic.GO_Player.PlayerSize.Height)
                );


            //GO_Items
            for (int i = 0; i < logic.GO_Items.Count; i++)
            {
                drawingContext.DrawRectangle(
                logic.GO_Items[i].ItemBrush,
                new Pen(Brushes.Red, 2),
                new Rect(logic.GO_Items[i].ItemPoint.X, logic.GO_Items[i].ItemPoint.Y,
                logic.GO_Items[i].ItemSize.Width, logic.GO_Items[i].ItemSize.Height)
                );
            }

            //Laser
            for (int i = 0; i < logic.GO_Lasers.Count; i++)
            {
                drawingContext.DrawRectangle(
                    logic.GO_Lasers[i].LaserBrush,
                    new Pen(Brushes.Yellow, 2),
                    new Rect(logic.GO_Lasers[i].LaserPoint.X, logic.GO_Lasers[i].LaserPoint.Y,
                    logic.GO_Lasers[i].LaserSize.Width, logic.GO_Lasers[i].LaserSize.Height)
                    );
            }





            //GO_Panels
            for (int i = 0; i < logic.GO_ControlPanels.Count; i++)
            {
                drawingContext.DrawRectangle(
                    logic.GO_ControlPanels[i].ControlPanelBrush,
                    new Pen(Brushes.Red, 2),
                    new Rect(logic.GO_ControlPanels[i].ControlPanelPoint.X, logic.GO_ControlPanels[i].ControlPanelPoint.Y,
                    logic.GO_ControlPanels[i].ControlPanelSize.Width, logic.GO_ControlPanels[i].ControlPanelSize.Height)
                    );
            }

            FormattedText ft_distance = new FormattedText($"Distance: {logic.GO_Player.Distance} km", System.Globalization.CultureInfo.CurrentCulture,
              FlowDirection.LeftToRight, new Typeface(new FontFamily("Arial"), FontStyles.Normal,
              FontWeights.Normal, FontStretches.Normal
              ), 20, Brushes.Yellow, 10);


            drawingContext.DrawText(ft_distance, new Point(logic.GO_ControlPanels[1].ControlPanelPoint.X+25, 120));




            FormattedText ft_health = new FormattedText(logic.GO_Player.Health + "+", System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, new Typeface(new FontFamily("Arial"), FontStyles.Normal,
                FontWeights.Normal, FontStretches.Normal
                ), 30, Brushes.Red, 10);


            drawingContext.DrawText(ft_health, new Point(logic.GO_ControlPanels[0].ControlPanelPoint.X + 35, 30));



            FormattedText ft_money = new FormattedText($"Money: {logic.GO_Player.Money}$", System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight, new Typeface(new FontFamily("Arial"), FontStyles.Normal,
                FontWeights.Normal, FontStretches.Normal
                ), 30, Brushes.LightGreen, 10);


            drawingContext.DrawText(ft_money, new Point(logic.GO_ControlPanels[3].ControlPanelPoint.X + 20, logic.GO_ControlPanels[3].ControlPanelPoint.Y + 25));




            //drawingContext.DrawRectangle(
            //    Brushes.Red,
            //    new Pen(Brushes.Yellow, 2),
            //    new Rect(logic.GO_ControlPanels[0].ControlPanelPoint.X, logic.GO_ControlPanels[0].ControlPanelPoint.Y,
            //    50,50)
            //    );


        }
    }
}
