using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WPF_LostInSpace.Store
{
    public class ConvertSpeedToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int actVal = (int)value;

            if (actVal <= 2)
            {
                return Brushes.Red;
            }
            else if (actVal > 2 && actVal <= 4)
            {
                return Brushes.Yellow;
            }
            else
            {
                return Brushes.Green;
            }




        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
