using ITVMusic.Models;
using ITVMusic.Repositories;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;

namespace ITVMusic.ViewModels {
    public class SearchViewModel : ViewModelBase {

        private readonly IAlmacenRepository almacenRepository;
        private readonly IPlaylistRepository playlistRepository;
        private readonly IAlbumRepository albumRepository;

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

            almacenRepository = new AlmacenRepository();
            playlistRepository = new PlaylistRepository();
            albumRepository = new AlbumRepository();

            // FillMusicItemsTest();
            FillMusicItems();

        }

        private async void FillMusicItems() {

            MusicItems.AddRange(await almacenRepository.GetByAll());
            MusicItems.AddRange(await playlistRepository.GetByAll());
            MusicItems.AddRange(await albumRepository.GetByAll());

            MessageBox.Show("Cargados");
        }

        private void FillMusicItemsTest() {

            //var songs = new ObservableCollection<SongModel>();

            /* Get Songs
            for (uint i = 1; i < 10; i++) {

                var song = new AlmacenModel() {
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
            */

            /* Get Playlists
            for (uint i = 1; i < 5; i++) {

                MusicItems.Add(new PlaylistModel() {
                    Id = i,
                    //Icon = File.ReadAllBytes(@"C:\Users\iDeboy\Pictures\fondo1.jpg").ToImage(),
                    Title = $"Playlist {i}",
                    Songs = songs
                });

            }
            */

            /* Get Albums
            for (uint i = 1; i < 10; i++) {

                MusicItems.Add(new AlbumModel() {
                    Id = i,
                    //Icon = File.ReadAllBytes(@"C:\Users\iDeboy\Pictures\foto_perfil.jpg").ToImage(),
                    Title = $"Álbum {i}",
                    Songs = songs
                });

            }
            */
        }

    }
}
