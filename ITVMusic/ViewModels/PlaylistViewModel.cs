using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.ViewModels {
    public class PlaylistViewModel : ViewModelBase {

        private SongModel? m_SelectedSong;

        public PlaylistModel? Playlist => App.SelectedMusicModel as PlaylistModel;

        public SongModel? SelectedSong {
            get => m_SelectedSong;
            set {
                m_SelectedSong = value;
                App.SelectedMusicModel = value;
                OnPropertyChanged();
            }
        }

        public PlaylistViewModel() {

        }

    }
}
