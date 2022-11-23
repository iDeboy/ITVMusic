using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ITVMusic.Util {
    public static class Helper {

        public static byte[] ToByteArray(this BitmapImage image) {
            return File.ReadAllBytes(image.UriSource.LocalPath);
        }

        public static bool IsFuture(this DateOnly date) {
            return date > DateTime.Today.ToDateOnly();
        }

        public static bool IsDefault(this DateOnly date) { 
            return date == DateOnly.MinValue;
        }

        public static DateOnly ToDateOnly(this DateTime dateTime) {
            return DateOnly.FromDateTime(dateTime);
        }

    }
}
