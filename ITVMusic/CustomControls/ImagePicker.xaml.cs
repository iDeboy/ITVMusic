using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ITVMusic.CustomControls {
    /// <summary>
    /// Lógica de interacción para ImagePicker.xaml
    /// </summary>
    public partial class ImagePicker : UserControl {

        // Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(ImagePicker));

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(BitmapImage), typeof(ImagePicker));

        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.Register(nameof(UriSource), typeof(Uri), typeof(ImagePicker));

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public BitmapImage? Source {
            get => (BitmapImage)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Uri? UriSource {
            get => (Uri)GetValue(UriSourceProperty);
            set => SetValue(UriSourceProperty, value);
        }

        public ImagePicker() {
            InitializeComponent();
        }

        private void RoundedButton_Click(object sender, RoutedEventArgs e) {
            OpenFileDialog dialog = new() {
                Filter = "Todos los archivos de imagenes (*.png;*.jpeg;*.jpg;*.bmp;)|*.png;*.jpeg;*.jpg;*.bmp" +
                         "|Archivos PNG (*.png)|*.png" +
                         "|Archivos JPEG (*.jpg *.jpeg)|*.jpg;*.jpeg" +
                         "|Archivos BMP (*.bmp)|*.bmp",
                DefaultExt = "*.png",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (dialog.ShowDialog() is not bool result) return;

            if (!result) return;

            Source = new(new(dialog.FileName));

        }
    }
}
