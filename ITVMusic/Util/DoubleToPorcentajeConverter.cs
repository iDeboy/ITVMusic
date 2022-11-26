using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ITVMusic.Util {
    public class DoubleToPorcentajeConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {

            if (value is not double porcent) return 0;

            if (porcent < 0) return 0;

            if (porcent > 1) return 100;

            return (int)Math.Round(porcent * 100, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
