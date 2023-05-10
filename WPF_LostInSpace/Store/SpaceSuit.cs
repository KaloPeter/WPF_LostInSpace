using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_LostInSpace.Store
{
    public class SpaceSuit
    {
        public int ID { get; set; }
        public int Speed { get; set; }
        public int Health { get; set; }

        public string SpaceSuitResPath { get; set; }

        public int Price { get; set; }

        public Brush SpaceSuitBrush_R { get; set; }
        public Brush SpaceSuitBrush_L { get; set; }

        public SpaceSuit(int id, int speed, int health, string suitImgName, int price)
        {
            ID = id;
            Speed = speed;
            Health = health;
            SpaceSuitResPath = Path.Combine("Images", "Astronaut_Res", $"{suitImgName}_R.png");
            SpaceSuitBrush_R = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", $"{suitImgName}_R.png"), UriKind.RelativeOrAbsolute)));
            SpaceSuitBrush_L = new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Astronaut", $"{suitImgName}_L.png"), UriKind.RelativeOrAbsolute)));
            Price = price;
        }

        public override string? ToString()
        {
            return $"Health:{Health}-Speed:{Speed}-ImagePath:{SpaceSuitResPath}";
        }

    }
}
