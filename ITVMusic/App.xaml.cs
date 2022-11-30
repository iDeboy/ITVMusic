using ITVMusic.Models;
using ITVMusic.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ITVMusic {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        public static string UtilFolder => Path.Combine(Environment.CurrentDirectory, "util");

        public static UserModel? UserData { get; set; }

        private static IMusicModelBase? m_SelectedMusicModel;
        private static SongModel? m_CurrentSong;
        public static IMusicModelBase? SelectedMusicModel {
            get => m_SelectedMusicModel;
            set {
                m_SelectedMusicModel = value;
                OnSelectedMusicModelChanged();
            }
        }

        public static SongModel? CurrentSong {
            get => m_CurrentSong;
            set {
                m_CurrentSong = value;
                OnCurrentSongChanged();
            }
        }
        public static event EventHandler<IMusicModelBase?>? SelectedMusicModelChanged;
        public static event EventHandler<SongModel?>? CurrentSongChanged;

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
