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
    public class ConvertHealthToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            int actVal = (int)value;

            if (actVal == 100)
            {
                return Brushes.Red;
            }
            else if (actVal > 100 && actVal <= 250)
            {
                return Brushes.Green;
            }
            else
            {
                return Brushes.Purple;
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
