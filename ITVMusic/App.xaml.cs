using ITVMusic.Models;
using ITVMusic.Repositories;
using ITVMusic.Repositories.Bases;
using ITVMusic.Views;
using System;
using System.IO;
using System.Windows;

namespace ITVMusic {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        // Repositories
        public static ISuscriptionRepository SuscriptionRepository => new SuscriptionRepository();
        public static IUserRepository UserRepository => new UserRepository();
        public static ISongRepository SongRepository => new SongRepository();
        public static IPlaylistRepository PlaylistRepository => new PlaylistRepository();
        public static IArtistRepository ArtistRepository => new ArtistRepository();
        public static IAlmacenRepository AlmacenRepository => new AlmacenRepository();
        public static IAlbumRepository AlbumRepository => new AlbumRepository();

        public static string UtilFolder => Path.Combine(Environment.CurrentDirectory, "util");

        public static UserModel? UserData { get; set; }

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
