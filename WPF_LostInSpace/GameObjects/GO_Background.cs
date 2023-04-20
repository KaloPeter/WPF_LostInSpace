using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_LostInSpace.GameObjects
{
    public class GO_Background
    {
        public Point BackgroundPoint { get; set; }//x,y
      //  public int Speed { get; set; } = 1;
        public Size BackgroundSize { get; set; }




        public Brush BackgroundBrush
        {
            get
            {
                return new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images","Backgrounds", "space_1.png"), UriKind.RelativeOrAbsolute)));//800x1920
            }
        }

    }
}
