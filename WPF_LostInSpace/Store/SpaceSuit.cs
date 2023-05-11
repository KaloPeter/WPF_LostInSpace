using Newtonsoft.Json;
using System;
using System.IO;


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

        public string SpaceSuitBrushPath_R { get; set; }
        public string SpaceSuitBrushPath_L { get; set; }

        [JsonIgnore]
        public Brush SpaceSuitBrush_R { get; set; }

        [JsonIgnore]
        public Brush SpaceSuitBrush_L { get; set; }

        public string SuitImgName { get; set; }

        public SpaceSuit(int id, int speed, int health, string suitImgName, int price)
        {
            ID = id;
            Speed = speed;
            Health = health;
            SuitImgName = suitImgName;
            SpaceSuitResPath = Path.Combine("Images", "Astronaut_Res", $"{SuitImgName}_R.png");

            SpaceSuitBrushPath_R = Path.Combine("Images", "Astronaut", $"{SuitImgName}_R.png");
            SpaceSuitBrushPath_L = Path.Combine("Images", "Astronaut", $"{SuitImgName}_L.png");

            SpaceSuitBrush_R = new ImageBrush(new BitmapImage(new Uri(SpaceSuitBrushPath_R, UriKind.RelativeOrAbsolute)));
            SpaceSuitBrush_L = new ImageBrush(new BitmapImage(new Uri(SpaceSuitBrushPath_L, UriKind.RelativeOrAbsolute)));
            Price = price;
        }
        

        public SpaceSuit DeppCopy()
        {
            return new SpaceSuit(ID,Speed,Health, SuitImgName,Price);
        }


        public override string? ToString()
        {
            return $"Health:{Health}-Speed:{Speed}-ImagePath:{SpaceSuitResPath}";
        }

    }
}
