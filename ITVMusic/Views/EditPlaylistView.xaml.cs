using ITVMusic.Models;
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

namespace ITVMusic.Views {
    /// <summary>
    /// Lógica de interacción para EditPlaylistView.xaml
    /// </summary>
    public partial class EditPlaylistView : UserControl {
        public EditPlaylistView() {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (sender is not ComboBox combo) return;

            model.Icon = model.Playlist?.Icon;
            model.Title = model.Playlist?.Title;
            model.SelectedSongs = model.Playlist?.Songs is not null ? new(model.Playlist.Songs) : null;

            model.Songs = new(from it in App.MusicItems
                              where it is AlmacenModel almacen && model.SelectedSongs is not null && !model.SelectedSongs.Any(a => a.Id == almacen.Id)
                              select (AlmacenModel)it);

        }

        private void IconedTextBox_KeyDown(object sender, KeyEventArgs e) {

            if (e.Key == Key.Enter) {
                helper.Focus();
            }

        }
    }
}
