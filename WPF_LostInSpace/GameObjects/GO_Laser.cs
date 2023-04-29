using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace WPF_LostInSpace.GameObjects
{
    public class GO_Laser
    {

        public Size LaserSize { get; set; }
        public Point LaserPoint { get; set; }

        public Brush LaserBrush { get; set; }

        public GO_Laser()
        {
            LaserBrush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", "laser_Red.png"), UriKind.RelativeOrAbsolute)));
        }

        public void MoveLaser()
        {
            Point npoint = new Point(LaserPoint.X, LaserPoint.Y + 1);
            LaserPoint = npoint;
        }



    }
}
