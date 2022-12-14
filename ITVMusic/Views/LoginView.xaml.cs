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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ITVMusic.Views {
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginView : Window {
        public LoginView() {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {

            if (!IsVisible && IsLoaded) {
                
                model.NextWindow?.Show();
                Close();

                Application.Current.MainWindow = model.NextWindow;
                
            }

        }

        private void IconedTextBox_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key is Key.Space) e.Handled = true;
        }
    }
}
