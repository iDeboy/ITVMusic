using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ITVMusic.Util {
    public class VolumeToIconConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is not double volume) return IconChar.Node;

            if (volume == 0) return IconChar.VolumeMute;

            if(volume < (1d/3d)) return IconChar.VolumeOff;

            if (volume < (2d / 3d)) return IconChar.VolumeLow;

            return IconChar.VolumeHigh;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
