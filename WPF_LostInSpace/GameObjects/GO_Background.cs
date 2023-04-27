using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPF_LostInSpace.HelperClasses;

namespace WPF_LostInSpace.GameObjects
{
    public class GO_Background
    {
        public Point BackgroundPoint { get; set; }//x,y
        public int Speed { get; set; } = 1;
        public Size BackgroundSize { get; set; }

        private static List<ImageBrush> images = null;

        private Brush backgroundBrush;


        private static int indexForFirstTwoBackgrunds = 0;

        public GO_Background()
        {
            //First 2 bacgrounds are empty space(not the colorful space), after the 2nd, random space backgrounds can be set.
            if (indexForFirstTwoBackgrunds <= 1)
            {
                backgroundBrush = images[indexForFirstTwoBackgrunds];
                indexForFirstTwoBackgrunds++;
            }
            else
            {
                backgroundBrush = images[Utils.rnd.Next(0, images.Count)];
            }

        }

        public static void LoadBackgrounds()
        {
            images = new List<ImageBrush>();
            indexForFirstTwoBackgrunds = 0;
            images.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "space_1.png"), UriKind.RelativeOrAbsolute))));
            images.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "space_earth_1.png"), UriKind.RelativeOrAbsolute))));
            images.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "space_2.png"), UriKind.RelativeOrAbsolute))));
            images.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "space_3.png"), UriKind.RelativeOrAbsolute))));
            images.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "space_4.png"), UriKind.RelativeOrAbsolute))));
        }

        public bool Move(Size playArea)
        {
            Point newPoint = new Point(BackgroundPoint.X, BackgroundPoint.Y - Speed);
            Point bottomLeftOfBackground = new Point(BackgroundPoint.X, BackgroundPoint.Y + BackgroundSize.Height);//bottom left corner (+40 was here), but it works with it (while background can be seen)

            if (bottomLeftOfBackground.Y >= 0/*playArea.Height*/)//there is still uncovered image part
            {
                this.BackgroundPoint = newPoint;
                return true;
            }
            else
            {
                return false;//If background stops, then stop the ticker that is responsible for moving it
            }
        }

        public Brush BackgroundBrush
        {
            get
            {
                return backgroundBrush;//new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Backgrounds", "space_1.png"), UriKind.RelativeOrAbsolute))); //800x1920
            }
        }

    }
}
