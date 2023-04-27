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
    public class GO_Item_Health : GO_Item
    {
        private static List<Brush> healths = null;

        public static void LoadHealthImages()
        {
            healths = new List<Brush>();
            healths.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "oxygen_1_T.png"), UriKind.RelativeOrAbsolute))));//0
            healths.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "oxygen_2_T.png"), UriKind.RelativeOrAbsolute))));//1
        }

        public override void SetBrushWithImage()
        {
            int randomHealthIndex = Utils.rnd.Next(0, healths.Count);

            //int randomHealthSizeHeight = Utils.rnd.Next(55, 65);
            //switch (randomHealthIndex)
            //{
            //    case 0:
            //        this.ItemSize = new Size(randomHealthSizeHeight - 25, randomHealthSizeHeight);
            //        break;
            //    case 1:
            //        this.ItemSize = new Size(randomHealthSizeHeight - 25, randomHealthSizeHeight);
            //        break;
            //    default: break;
            //}

            switch (randomHealthIndex)
            {
                case 0:
                    this.ItemSize = new Size(15, 55);
                    break;
                case 1:
                    this.ItemSize = new Size(25, 55);
                    break;
                default: break;
            }

            this.ItemBrush = healths[randomHealthIndex];
        }
    }
}
