using ITVMusic.Models;
using ITVMusic.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ITVMusic.ViewModels {
    public class SearchViewModel : ViewModelBase {

        private ObservableCollection<IMusicModelBase>? m_MusicItemsFound;
        private IMusicModelBase? m_SelectedMusicModel;

        public ObservableCollection<IMusicModelBase> MusicItems { get; }
        public ObservableCollection<IMusicModelBase>? MusicItemsFound {
            get => m_MusicItemsFound;
            set {
                m_MusicItemsFound = value;
                OnPropertyChanged();
            }
        }
        public IMusicModelBase? SelectedMusicModel {
            get => m_SelectedMusicModel;
            set {
                m_SelectedMusicModel = value;
                App.SelectedMusicModel = value;
                OnPropertyChanged();
            }
        }

        public SearchViewModel() {

            MusicItems = new();

            FillMusicItemsTest();

        }

        private void FillMusicItemsTest() {

            var songs = new ObservableCollection<SongModel>();

            for (uint i = 1; i < 10; i++) {

                var song = new SongModel() {
                    Id = i,
                    //Icon = File.ReadAllBytes(@"C:\Users\iDeboy\Pictures\fondo2.jpg").ToImage(),
                    Title = $"Canción {i}",
                    Duration = new Duration(new(0, new Random().Next(1, 4), new Random().Next(0, 59))),
                    Album = new() { Title = $"Álbum {i}" },
                    Reproductions = (uint)new Random().Next(0, 100)
                    //Bytes = File.ReadAllBytes(@"C:\Users\iDeboy\Downloads\Rolas\BoyWithUke Toxic (Live Performance) .m4a")
                };

                MusicItems.Add(song);
                songs.Add(song);
            }

            for (uint i = 1; i < 5; i++) {

                MusicItems.Add(new PlaylistModel() {
                    Id = i,
                    //Icon = File.ReadAllBytes(@"C:\Users\iDeboy\Pictures\fondo1.jpg").ToImage(),
                    Title = $"Playlist {i}",
                    Songs = songs
                });

            }

            for (uint i = 1; i < 10; i++) {

                MusicItems.Add(new AlbumModel() {
                    Id = i,
                    //Icon = File.ReadAllBytes(@"C:\Users\iDeboy\Pictures\foto_perfil.jpg").ToImage(),
                    Title = $"Álbum {i}",
                    Songs = songs
                });

            }

        }

    }
}
