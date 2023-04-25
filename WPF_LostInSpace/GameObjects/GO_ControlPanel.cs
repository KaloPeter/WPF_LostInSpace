using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPF_LostInSpace.GameObjects
{
    public class GO_ControlPanel
    {
        public Point ControlPanelPoint { get; set; }
        public Size ControlPanelSize { get; set; }
        public Brush ControlPanelBrush { get; }

        public GO_ControlPanel(string imageFileName)
        {
            Uri uri = new Uri(Path.Combine("Images", "Panels", imageFileName), UriKind.RelativeOrAbsolute);
            BitmapImage bitmImg = new BitmapImage(uri);

            ControlPanelBrush = new ImageBrush(bitmImg);
        }
    }
}
