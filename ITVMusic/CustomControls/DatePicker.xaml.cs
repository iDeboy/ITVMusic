using ITVMusic.Util;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ITVMusic.CustomControls {
    /// <summary>
    /// Lógica de interacción para DatePicker.xaml
    /// </summary>
    public partial class DatePicker : UserControl {

        // Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(DatePicker));

        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register(nameof(Date), typeof(DateOnly), typeof(DatePicker));

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public DateOnly? Date {
            get => (DateOnly)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public DatePicker() {
            InitializeComponent();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e) {

            if (sender is not System.Windows.Controls.DatePicker picker) return;

            Date = picker.SelectedDate?.ToDateOnly();
        }
    }
}
