using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ITVMusic.Util {
    public class DurationToDoubleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is not Duration duration) return 0;

            if (!duration.HasTimeSpan) return 0;

            return (double)duration.TimeSpan.Ticks;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is not double tickDuration) return new Duration(TimeSpan.Zero);

            return new Duration(new TimeSpan((long)tickDuration));
        }
    }
}
