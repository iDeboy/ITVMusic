using FontAwesome.Sharp;
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
using static System.Net.Mime.MediaTypeNames;

namespace ITVMusic.CustomControls {
    /// <summary>
    /// Lógica de interacción para IconTextBox.xaml
    /// </summary>
    public partial class IconTextBox : UserControl {

        // Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(IconTextBox));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(IconChar), typeof(IconTextBox));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconTextBox));

        public static readonly DependencyProperty TitleColorProperty =
            DependencyProperty.Register(nameof(TitleColor), typeof(Brush), typeof(IconTextBox));

        public static readonly DependencyProperty IconColorProperty =
            DependencyProperty.Register(nameof(IconColor), typeof(Brush), typeof(IconTextBox));

        public static readonly DependencyProperty TextColorProperty =
            DependencyProperty.Register(nameof(TextColor), typeof(Brush), typeof(IconTextBox));

        public string Title {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public IconChar Icon {
            get => (IconChar)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string Text {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush TitleColor {
            get => (Brush)GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public Brush IconColor {
            get => (Brush)GetValue(IconColorProperty);
            set => SetValue(IconColorProperty, value);
        }

        // Events
        public event TextChangedEventHandler? TextChanged;

        protected virtual void OnTextChanged(TextChangedEventArgs e) {
            TextChanged?.Invoke(this, e);
        }

        public Brush TextColor {
            get => (Brush)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public IconTextBox() {
            InitializeComponent();
            SetResourceReference(TitleColorProperty, "TextBoxTitleColor");
            SetResourceReference(IconColorProperty, "TextBoxIconColor");
            SetResourceReference(TextColorProperty, "TextBoxContentColor");
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {

            if (sender is not TextBox txtBox) return;

            Text = txtBox.Text;
            
            OnTextChanged(e);
        }

        public void Clear() => m_TextBox.Clear();
    }
}
