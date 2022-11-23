using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ITVMusic.Views {
    /// <summary>
    /// Lógica de interacción para RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window {
        public RegisterView() {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {

            if (IsVisible || !IsLoaded) return;

            model.NextWindow?.Show();
            Close();

            Application.Current.MainWindow = model.NextWindow;
        }

        private void DontAllowSpaces(object sender, KeyEventArgs e) {

            if (e.Key is Key.Space) e.Handled = true;
        }

        private void Phone_PreviewTextInput(object sender, TextCompositionEventArgs e) {
            char ch = e.Text.First();

            if (!char.IsDigit(ch)) e.Handled = true;
        }

    }
}
