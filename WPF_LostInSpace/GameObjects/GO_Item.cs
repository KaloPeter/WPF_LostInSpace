using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPF_LostInSpace.GameObjects
{
    public abstract class GO_Item
    {
        public Size ItemSize { get; set; }
        public Point ItemPoint { get; set; }

        public Brush ItemBrush { get; set; }

        public int Speed { get; set; } = 2;

        //protected static Random rnd = new Random();

        public abstract void SetBrushWithImage();

        public bool Move()//maybe it's the wrong place
        {
            Point nPoint = new Point(ItemPoint.X, ItemPoint.Y - Speed);

            if (nPoint.Y >= 0 - ItemSize.Height)//disappears when out of bounds "-" is good, "+" can be seen how they disappear
            {
                ItemPoint = nPoint;

                return true;
            }
            else
            {
                return false;
            }
        }

        public override string? ToString()
        {
            return $"Size:{ItemSize}-Point:{ItemPoint}-Brush:{ItemBrush}";
        }
    }
}
