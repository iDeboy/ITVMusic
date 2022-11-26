using ITVMusic.Models;
using ITVMusic.Util;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ITVMusic.Views {
    /// <summary>
    /// Lógica de interacción para SearchView.xaml
    /// </summary>
    public partial class SearchView : UserControl {
        public SearchView() {
            InitializeComponent();
        }

        private async void TextBox_TextChanged(object sender, TextChangedEventArgs e) {

            if (sender is not TextBox txtBox) return;

            await Task.Delay(1000);

            if (string.IsNullOrWhiteSpace(txtBox.Text)) {
                model.MusicItemsFound = null;
                return;
            }

            model.MusicItemsFound = new(from it in model.MusicItems
                                        where it.Title is not null && it.Title.ContainsWithoutDiacritics(txtBox.Text, StringComparison.InvariantCultureIgnoreCase)
                                        select it);
            
        }

    }
}
