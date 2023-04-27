using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using WPF_LostInSpace.HelperClasses;
using System.Windows;

namespace WPF_LostInSpace.GameObjects
{
    public class GO_Item_Satellite : GO_Item
    {
        private static List<Brush> satelites = null;
        public static void LoadSateliteImages()
        {
            satelites = new List<Brush>();
            satelites.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "sat_1_T.png"), UriKind.RelativeOrAbsolute))));//0
            satelites.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "sat_2_T.png"), UriKind.RelativeOrAbsolute))));//1
            satelites.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "sat_3_T.png"), UriKind.RelativeOrAbsolute))));//2
        }

        public override void SetBrushWithImage()
        {
            int randomSatelliteIndex = Utils.rnd.Next(0, satelites.Count);

            int randomSatelliteSize = Utils.rnd.Next(50, 65);

            switch (randomSatelliteIndex)
            {
                case 0:
                    this.ItemSize = new Size(randomSatelliteSize, randomSatelliteSize);
                    break;
                case 1:
                    this.ItemSize = new Size(randomSatelliteSize, randomSatelliteSize);
                    break;
                case 2:
                    this.ItemSize = new Size(randomSatelliteSize, randomSatelliteSize);
                    break;
                default: break;
            }

            this.ItemBrush = satelites[randomSatelliteIndex];
        }
    }
}
