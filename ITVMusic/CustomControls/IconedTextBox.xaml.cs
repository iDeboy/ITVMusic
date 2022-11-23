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
    /// Lógica de interacción para IconedTextBox.xaml
    /// </summary>
    public partial class IconedTextBox : UserControl {

        // Properties
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(IconedTextBox));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(IconChar), typeof(IconedTextBox));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(IconedTextBox));

        public static readonly DependencyProperty MaxLenghtProperty =
            DependencyProperty.Register(nameof(MaxLenght), typeof(int), typeof(IconedTextBox));

        public static readonly DependencyProperty CharacterCasingProperty =
            DependencyProperty.Register(nameof(CharacterCasing), typeof(CharacterCasing), typeof(IconedTextBox));

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

        public int MaxLenght {
            get => (int)GetValue(MaxLenghtProperty);
            set => SetValue(MaxLenghtProperty, value);
        }

        public CharacterCasing CharacterCasing {
            get => (CharacterCasing)GetValue(CharacterCasingProperty);
            set => SetValue(CharacterCasingProperty, value);
        }

        // Events
        public event TextChangedEventHandler? TextChanged;

        protected virtual void OnTextChanged(TextChangedEventArgs e) {
            TextChanged?.Invoke(this, e);
        }

        public IconedTextBox() {
            InitializeComponent();
            MaxLenght = int.MaxValue;
            CharacterCasing = CharacterCasing.Normal;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) {

            if (sender is not TextBox txtBox) return;

            Text = txtBox.Text;
            
            OnTextChanged(e);
        }

        public void Clear() => m_TextBox.Clear();
    }
}
