using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_LostInSpace.Renderer
{
    public class Display : FrameworkElement
    {
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);


            drawingContext.DrawRectangle(
                Brushes.Red,
                new Pen(Brushes.Black,2),
                new Rect(100,100,20,20)
                );

        }
    }
}
