using ITVMusic.Models;
using System.Collections.ObjectModel;

namespace ITVMusic.ViewModels {
    public class SearchViewModel : ViewModelBase {

        private ObservableCollection<IMusicModelBase>? m_MusicItemsFound;
        private IMusicModelBase? m_SelectedMusicModel;

        //public ObservableCollection<IMusicModelBase> MusicItems { get; }
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

            //MusicItems = new();

            // FillMusicItemsTest();
            // FillMusicItems();

        }
        /*
        private async void FillMusicItems() {

            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();

            var almacenes = LoadAlmacenes();
            var playlists = LoadPlaylists();
            var albums = LoadAlbums();

            await Task.WhenAll(almacenes, playlists, albums);

            sw.Stop();
            MessageBox.Show($"{sw.ElapsedMilliseconds} ms");
            MusicItems.AddRange(almacenes.Result);
            MusicItems.AddRange(playlists.Result);
            MusicItems.AddRange(albums.Result);

            MessageBox.Show("Cargados");
        }

        private static async Task<IEnumerable<AlmacenModel>> LoadAlmacenes() {

            var almacenes = (await App.AlmacenRepository.GetByAllAsync()).ToList();

            var songs = (from almacen in almacenes
                         select App.SongRepository.GetFromAsync(almacen)).ToList();

            var albums = (from almacen in almacenes
                          select App.AlbumRepository.GetFromAsync(almacen)).ToList();

            await Task.WhenAll(songs);

            await Task.WhenAll(albums);

            for (int i = 0; i < almacenes.Count; i++) {
                almacenes[i].Song = songs[i].Result;
                almacenes[i].Album = albums[i].Result;
            }

            return almacenes;
        }

        private static async Task<IEnumerable<PlaylistModel>> LoadPlaylists() {

            var playlists = (await App.PlaylistRepository.GetByAllAsync()).ToList();

            var items = (from playlist in playlists
                         select App.AlmacenRepository.GetFromAsync(playlist)).ToList();

            await Task.WhenAll(items);

            for (int i = 0; i < playlists.Count; i++) {
                playlists[i].Songs.AddRange(items[i].Result);
            }

            return playlists;
        }

        private static async Task<IEnumerable<AlbumModel>> LoadAlbums() {

            var albums = (await App.AlbumRepository.GetByAllAsync()).ToList();

            var items = (from album in albums
                         select App.AlmacenRepository.GetFromAsync(album)).ToList();

            await Task.WhenAll(items);

            for (int i = 0; i < albums.Count; i++) {
                albums[i].Songs.AddRange(items[i].Result);
            }

            return albums;
        }
        */
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
