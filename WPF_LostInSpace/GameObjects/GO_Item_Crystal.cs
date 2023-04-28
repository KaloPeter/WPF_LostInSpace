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
    public class GO_Item_Crystal : GO_Item
    {
        public int Value { get; set; }

        private static List<Brush> crystals = null;

        public static void LoadCrystalImages()
        {
            crystals = new List<Brush>();
            //crystals.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "crystal_0.png"), UriKind.RelativeOrAbsolute))));//0
            for (int i = 1; i <= 6; i++)
            {
                crystals.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", $"crystal_{i}.png"), UriKind.RelativeOrAbsolute))));
            }
        }

        public GO_Item_Crystal(int value)
        {
            Value = value;
        }

        public override void SetBrushWithImage()
        {
            ItemSize = new Size(20, 20);

            switch (Value)
            {
                case 10:
                    ItemBrush = crystals[0];
                    break;
                case 20:
                    ItemBrush = crystals[1];
                    break;
                case 30:
                    ItemBrush = crystals[2];
                    break;
                case 40:
                    ItemBrush = crystals[3];
                    break;
                case 50:
                    ItemBrush = crystals[4];
                    break;
                case 100:
                    ItemBrush = crystals[5];
                    break;
                default: break;
            }

        }
    }
}
