using ITVMusic.Models;
using ITVMusic.Repositories;
using ITVMusic.Repositories.Bases;
using ITVMusic.Util;
using ITVMusic.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ITVMusic {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        // Repositories
        private static ISuscriptionRepository? suscriptionRepository;
        private static IUserRepository? userRepository;
        private static ISongRepository? songRepository;
        private static IPlaylistRepository? playlistRepository;
        private static IArtistRepository? artistRepository;
        private static IAlmacenRepository? almacenRepository;
        private static IAlbumRepository? albumRepository;

        public static ISuscriptionRepository SuscriptionRepository => suscriptionRepository ??= new SuscriptionRepository();
        public static IUserRepository UserRepository => userRepository ??= new UserRepository();
        public static ISongRepository SongRepository => songRepository ??= new SongRepository();
        public static IPlaylistRepository PlaylistRepository => playlistRepository ??= new PlaylistRepository();
        public static IArtistRepository ArtistRepository => artistRepository ??= new ArtistRepository();
        public static IAlmacenRepository AlmacenRepository => almacenRepository ??= new AlmacenRepository();
        public static IAlbumRepository AlbumRepository => albumRepository ??= new AlbumRepository();

        public static string UtilFolder => Path.Combine(Environment.CurrentDirectory, "util");

        public static UserModel? UserData { get; set; }

        private static ObservableCollection<IMusicModelBase>? musicItems;
        public static ObservableCollection<IMusicModelBase> MusicItems => musicItems ??= new();

        private static IMusicModelBase? m_SelectedMusicModel;
        private static AlmacenModel? m_CurrentSong;
        public static IMusicModelBase? SelectedMusicModel {
            get => m_SelectedMusicModel;
            set {
                m_SelectedMusicModel = value;
                OnSelectedMusicModelChanged();
            }
        }

        public static AlmacenModel? CurrentSong {
            get => m_CurrentSong;
            set {
                m_CurrentSong = value;
                OnCurrentSongChanged();
            }
        }
        public static event EventHandler<IMusicModelBase?>? SelectedMusicModelChanged;
        public static event EventHandler<AlmacenModel?>? CurrentSongChanged;

        private static void OnSelectedMusicModelChanged() {
            SelectedMusicModelChanged?.Invoke(Current, SelectedMusicModel);
        }
        private static void OnCurrentSongChanged() {
            CurrentSongChanged?.Invoke(Current, CurrentSong);
        }

        public static async Task InitMusicElements() {

            var almacenes = LoadAlmacenes();
            var playlists = LoadPlaylists();
            var albums = LoadAlbums();

            await Task.WhenAll(almacenes, playlists, albums);

            MusicItems.AddRange(almacenes.Result);
            MusicItems.AddRange(playlists.Result);
            MusicItems.AddRange(albums.Result);

            await Task.CompletedTask;
        }

        public static async Task<IEnumerable<AlmacenModel>> LoadAlmacenes() {

            var almacenes = (await AlmacenRepository.GetByAllAsync()).ToList();

            var songs = (from almacen in almacenes
                         select SongRepository.GetFromAsync(almacen)).ToList();

            var albums = (from almacen in almacenes
                          select AlbumRepository.GetFromAsync(almacen)).ToList();

            await Task.WhenAll(songs);

            await Task.WhenAll(albums);

            for (int i = 0; i < almacenes.Count; i++) {
                almacenes[i].Song = songs[i].Result;
                almacenes[i].Album = albums[i].Result;
            }

            return almacenes;
        }

        public static async Task<IEnumerable<PlaylistModel>> LoadPlaylists() {

            var playlists = (await PlaylistRepository.GetByAllAsync()).ToList();

            var items = (from playlist in playlists
                         select AlmacenRepository.GetFromAsync(playlist)).ToList();

            var authors = (from playlist in playlists
                           select UserRepository.GetFromAsync(playlist)).ToList();

            await Task.WhenAll(items);

            await Task.WhenAll(authors);

            for (int i = 0; i < playlists.Count; i++) {
                playlists[i].Songs.AddRange(items[i].Result);
                playlists[i].Authors.AddRange(authors[i].Result);
            }

            return playlists;
        }

        public static async Task<IEnumerable<AlbumModel>> LoadAlbums() {

            var albums = (await AlbumRepository.GetByAllAsync()).ToList();

            var items = (from album in albums
                         select AlmacenRepository.GetFromAsync(album)).ToList();

            await Task.WhenAll(items);

            for (int i = 0; i < albums.Count; i++) {
                albums[i].Songs.AddRange(items[i].Result);
            }

            return albums;
        }

        private void Application_Startup(object sender, StartupEventArgs e) {

            var login = new LoginView();
            login.Show();

            Current.MainWindow = login;

            //var main = new MainView();
            //main.Show();

            //Current.MainWindow = main;
        }

        private void Application_Exit(object sender, ExitEventArgs e) {

            //Directory.Delete(UtilFolder, true);

        }
    }
}
