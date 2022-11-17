using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json;
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
    /// Lógica de interacción para BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl {
        // Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(BindablePasswordBox));

        public static readonly DependencyProperty TitleColorProperty =
            DependencyProperty.Register(nameof(TitleColor), typeof(Brush), typeof(BindablePasswordBox));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(BindablePasswordBox));

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(SecureString), typeof(BindablePasswordBox));

        public SecureString Password {
            get => (SecureString)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public Brush TitleColor {
            get => (Brush)GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public Brush IconColor {
            get => (Brush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        public BindablePasswordBox() {
            InitializeComponent();
            SetResourceReference(TitleColorProperty, "TextBoxTitleColor");
            SetResourceReference(IconColorProperty, "TextBoxIconColor");
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) {
            Password = m_PasswordBox.SecurePassword;
        }

        public void Clear() {
            m_PasswordBox.Clear();
            m_TextBox.Clear();
        }

        private void ShowPassword(object sender, MouseButtonEventArgs e) {
            m_ShowPasswordIcon.Icon = IconChar.Eye;

            m_TextBox.Text = m_PasswordBox.Password;
            m_TextBox.Visibility = Visibility.Visible;
            m_PasswordBox.Visibility = Visibility.Collapsed;
        }

        private void HidePassword(object sender, MouseButtonEventArgs e) {
            m_ShowPasswordIcon.Icon = IconChar.EyeSlash;

            m_PasswordBox.Password = m_TextBox.Text;
            m_PasswordBox.Visibility = Visibility.Visible;
            m_TextBox.Visibility = Visibility.Collapsed;
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key is Key.Space) e.Handled = true;
        }

       
    }
}
