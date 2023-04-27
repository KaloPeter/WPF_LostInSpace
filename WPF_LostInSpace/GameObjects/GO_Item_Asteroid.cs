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
    public class GO_Item_Asteroid : GO_Item
    {
        private static List<Brush> asteroids = null;

        public static void LoadAsteroidImages()
        {
            asteroids = new List<Brush>();
            asteroids.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "ast_1_T.png"), UriKind.RelativeOrAbsolute))));//0
            asteroids.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "ast_2_T.png"), UriKind.RelativeOrAbsolute))));//1
            asteroids.Add(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Images", "Items", "ast_3_T.png"), UriKind.RelativeOrAbsolute))));//2
        }

        public override void SetBrushWithImage()
        {
            int randomAsteroidIndex = Utils.rnd.Next(0, asteroids.Count);

            int randomAsteroidSize = Utils.rnd.Next(20, 30);


            switch (randomAsteroidIndex)
            {
                case 0:
                    this.ItemSize = new Size(randomAsteroidSize, randomAsteroidSize);
                    break;
                case 1:
                    this.ItemSize = new Size(randomAsteroidSize, randomAsteroidSize);
                    break;
                case 2:
                    this.ItemSize = new Size(randomAsteroidSize, randomAsteroidSize);
                    break;
                default: break;
            }

            this.ItemBrush = asteroids[randomAsteroidIndex];

        }

    }
}
