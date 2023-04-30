using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_LostInSpace.GameObjects
{
    public class GO_Player
    {
        private Brush playerBrush;
        private  Brush playerBrushLeft;
        private  Brush playerBrushRight;

        public string Name { get; set; }

        public Point PlayerPoint { get; set; }
        public Size PlayerSize { get; set; }

        public int Money { get; set; }
        public int Speed { get; set; } = 2;
        public int Health { get; set; } = 100;
        public double Distance { get; set; }

        public Brush PlayerBrush { get { return playerBrush; } }
        public Brush PlayerBrushLeft { get { return playerBrushLeft; } }
        public Brush PlayerBrushRight { get { return playerBrushRight; } }

        public GO_Player()
        {
            //playerBrush = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", "astronaut_L.png"), UriKind.RelativeOrAbsolute)));
            playerBrushLeft = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", "astronaut_L.png"), UriKind.RelativeOrAbsolute)));
            playerBrushRight = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", "astronaut_R.png"), UriKind.RelativeOrAbsolute)));
            playerBrush = playerBrushLeft;
        }

        public void Move(PlayerController pc)
        {
            Point newPoint;
            switch (pc)
            {
                case PlayerController.Left:
                    newPoint = new Point(PlayerPoint.X - Speed, PlayerPoint.Y);
                    playerBrush = PlayerBrushLeft;
                    break;
                case PlayerController.Right:
                    newPoint = new Point(PlayerPoint.X + Speed, PlayerPoint.Y);
                    playerBrush = PlayerBrushRight;
                    break;
                default:
                    break;
            }

            this.PlayerPoint = newPoint;
        }

        //public Brush PlayerBrushLeft
        //{
        //    get
        //    {
        //        Uri uri = new Uri(Path.Combine("Images", "Astronaut", "astronaut_L.png"), UriKind.RelativeOrAbsolute);
        //        BitmapImage bitmImg = new BitmapImage(uri);

        //        return new ImageBrush(bitmImg);
        //    }
        //}

        //public Brush PlayerBrushRight
        //{
        //    get
        //    {
        //        Uri uri = new Uri(Path.Combine("Images", "Astronaut", "astronaut_R.png"), UriKind.RelativeOrAbsolute);
        //        BitmapImage bitmImg = new BitmapImage(uri);

        //        return new ImageBrush(bitmImg);
        //    }
        //}

    }
}
