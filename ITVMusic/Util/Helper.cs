using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ITVMusic.Util {
    public static class Helper {

        public static void AddRange<T>(this Collection<T> self, IEnumerable<T?>? collection) {

            if (collection is null) return;

            foreach (var it in collection) {

                if (it is not null) self.Add(it);
            }

        }

        public static Task AddRangeAsync<T>(this Collection<T> self, IEnumerable<T?>? collection) {
            return Task.Run(() => self.AddRange(collection));
        }

        public static async Task<byte[]?> ToByteArray(this ImageSource? image) {

            if (image is not BitmapImage bitmapImage) return null;

            return await File.ReadAllBytesAsync(bitmapImage.UriSource.LocalPath);
        }

        public static bool IsFuture(this DateOnly date) {
            return date > DateTime.Today.ToDateOnly();
        }

        public static bool IsValid(this DateOnly date) {

            bool result = !date.IsFuture();

            int year = date.Year;
            int currentYear = DateTime.Today.Year;

            int old = currentYear - year;

            result &= old > 16;

            return result;
        }

        public static bool IsDefault(this DateOnly date) {
            return date == DateOnly.MinValue;
        }

        public static DateOnly ToDateOnly(this DateTime dateTime) {
            return DateOnly.FromDateTime(dateTime);
        }

        public static Duration ToDuration(this DateTime dateTime) {
            var timeOnly = TimeOnly.FromDateTime(dateTime);

            return new(timeOnly.ToTimeSpan());
        }

        public static TimeOnly ToTimeOnly(this Duration duration) {

            if (!duration.HasTimeSpan) return TimeOnly.MinValue;

            return TimeOnly.FromTimeSpan(duration.TimeSpan);
        }

        public static ImageSource? ToImage(this object? obj) {

            if (obj is null || obj is DBNull) return null;

            if (obj is not byte[] bytes) return null;

            return new ImageSourceConverter().ConvertFrom(bytes) as ImageSource;
        }

        public static uint ToUInt32(this object? obj) {

            if (obj is null || obj is DBNull) return 0;

            return Convert.ToUInt32(obj);
        }

        public static Task Save(this byte[] bytes, string path) {
            return File.WriteAllBytesAsync(path, bytes);
        }

        public static string RemoveDiacritics(this string text) {
            return string.Concat(
                text.Normalize(NormalizationForm.FormD)
                .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                .Normalize(NormalizationForm.FormC);
        }

        public static bool ContainsWithoutDiacritics(this string text, string value, StringComparison comparisonType) {

            var _text = text.RemoveDiacritics();
            var _value = value.RemoveDiacritics();

            return _text.Contains(_value, comparisonType);
        }

        public static MediaState? GetMediaState(this MediaElement media) {
            var hlp = typeof(MediaElement).GetField("_helper", BindingFlags.NonPublic | BindingFlags.Instance);

            var helperObject = hlp?.GetValue(media);

            var stateField = helperObject?.GetType().GetField("_currentState", BindingFlags.NonPublic | BindingFlags.Instance);

            var state = (MediaState?)stateField?.GetValue(helperObject);

            return state;
        }

    }
}
