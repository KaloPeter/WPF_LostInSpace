using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
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

            for (int i = 0; i < logic.GO_Backgrounds.Count; i++)
            {
                drawingContext.DrawRectangle(
                    logic.GO_Backgrounds[i].BackgroundBrush,
                    null,
                    new Rect(logic.GO_Backgrounds[i].BackgroundPoint.X, logic.GO_Backgrounds[i].BackgroundPoint.Y,
                    logic.GO_Backgrounds[i].BackgroundSize.Width, logic.GO_Backgrounds[i].BackgroundSize.Height)
                    );
            }


            for (int i = 0; i < logic.GO_Items.Count; i++)
            {
                drawingContext.DrawRectangle(
                logic.GO_Items[i].ItemBrush,
                null,
                new Rect(logic.GO_Items[i].ItemPoint.X, logic.GO_Items[i].ItemPoint.Y,
                logic.GO_Items[i].ItemSize.Width, logic.GO_Items[i].ItemSize.Height)
                );
            }








            for (int i = 0; i < logic.GO_ControlPanels.Count; i++)
            {
                drawingContext.DrawRectangle(
                    logic.GO_ControlPanels[i].ControlPanelBrush,
                    null,
                    new Rect(logic.GO_ControlPanels[i].ControlPanelPoint.X, logic.GO_ControlPanels[i].ControlPanelPoint.Y,
                    logic.GO_ControlPanels[i].ControlPanelSize.Width, logic.GO_ControlPanels[i].ControlPanelSize.Height)
                    );
            }


        }
    }
}
