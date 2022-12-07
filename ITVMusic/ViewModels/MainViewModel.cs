using FontAwesome.Sharp;
using ITVMusic.Models;
using ITVMusic.Util;
using ITVMusic.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ITVMusic.ViewModels {
    public class MainViewModel : ViewModelBase {

        // Timer
        private DispatcherTimer timer;

        // Fields
        private UserControl? m_CurrentChildView;
        private string? m_Caption;
        private IconChar m_Icon;
        private UserAccountModel? m_UserAccount;
        private bool m_IsCurrentSong;
        private double m_Volume;
        private AlmacenModel? m_CurrentSong;
        private Duration m_CurrentSongPosition;
        private IconChar m_IconPlayer;

        public UserControl? CurrentChildView {
            get => m_CurrentChildView;
            set {
                m_CurrentChildView = value;
                OnPropertyChanged();
            }
        }
        public string? Caption {
            get => m_Caption;
            set {
                m_Caption = value;
                OnPropertyChanged();
            }
        }
        public IconChar Icon {
            get => m_Icon;
            set {
                m_Icon = value;
                OnPropertyChanged();
            }
        }
        public UserAccountModel? UserAccount {
            get => m_UserAccount;
            set {
                m_UserAccount = value;
                OnPropertyChanged();
            }
        }
        public bool IsCurrentSong {
            get => m_IsCurrentSong;
            set {
                m_IsCurrentSong = value;
                OnPropertyChanged();
            }
        }
        public double Volume {
            get => m_Volume;
            set {
                m_Volume = value;
                OnPropertyChanged();
            }
        }
        public AlmacenModel? CurrentSong {
            get => m_CurrentSong;
            set {
                m_CurrentSong = value;

                IsCurrentSong = value is not null;

                CurrentSongPosition = new Duration(TimeSpan.Zero);

                App.CurrentSong = value;

                OnPropertyChanged();
            }
        }
        public Duration CurrentSongPosition {
            get => m_CurrentSongPosition;
            set {
                m_CurrentSongPosition = value;
                OnPropertyChanged();
            }
        }
        public IconChar IconPlayer {

            get => m_IconPlayer;
            set {
                m_IconPlayer = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand CloseCommand { get; }
        public ICommand HomeCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand MyLibraryCommand { get; }
        public ICommand CreatePlaylistCommand { get; }
        public ICommand ShowSongsCommand { get; }
        public ICommand ShowPlaylistsCommand { get; }
        public ICommand PausePlayCommand { get; }

        public MainViewModel() {

            timer = new() {
                Interval = TimeSpan.FromSeconds(1),
            };

            timer.Tick += Timer_Tick;

            App.SelectedMusicModelChanged += App_SelectedMusicModelChanged;

            Volume = 0.1;
            IconPlayer = IconChar.PlayCircle;

            CloseCommand = new ViewModelCommand(ExecuteCloseCommand);
            HomeCommand = new ViewModelCommand(ExecuteHomeCommand);
            SearchCommand = new ViewModelCommand(ExecuteSearchCommand);
            MyLibraryCommand = new ViewModelCommand(ExecuteMyLibraryCommand);
            CreatePlaylistCommand = new ViewModelCommand(ExecuteCreatePlaylistCommand);
            ShowSongsCommand = new ViewModelCommand(ExecuteShowSongsCommand);
            ShowPlaylistsCommand = new ViewModelCommand(ExecuteShowPlaylistsCommand);
            PausePlayCommand = new ViewModelCommand(ExecutePausePlayCommand);

            ExecuteHomeCommand(null);

            //UserAccountTest();
            InitUserAccount();
        }

        private void InitUserAccount() {
            UserAccount = new(App.UserData);
        }
        private void ExecutePausePlayCommand(object? obj) {

            if (CurrentSong is null) {
                IconPlayer = IconChar.PlayCircle;
                return;
            }

            if (timer.IsEnabled) {
                IconPlayer = IconChar.PlayCircle;
                timer.Stop();
            } else {
                IconPlayer = IconChar.PauseCircle;
                timer.Start();
            }

        }

        private void Timer_Tick(object? sender, EventArgs e) {

            if (CurrentSong is null) return;

            if (CurrentSong.Song is null) return;

            if (CurrentSongPosition >= CurrentSong.Song.Duration) {
                CurrentSongPosition = CurrentSong.Song.Duration;
                IconPlayer = IconChar.PlayCircle;
                return;
            }

            IconPlayer = IconChar.PauseCircle;
            CurrentSongPosition = CurrentSongPosition.Add(new Duration(TimeSpan.FromSeconds(1)));
        }

        private async void App_SelectedMusicModelChanged(object? sender, IMusicModelBase? e) {

            if (e is null) return;

            // Si es una canción no mostrar ninguna vista y reproducir la canción y 
            // mostrarla en el panel inferior

            if (e is AlmacenModel song) {

                // Hacer un insert a Escucha
                var escucha = App.UserRepository.ListenToSongAsync(App.UserData, song);

                IconPlayer = IconChar.PlayCircle;

                timer.Stop();

                CurrentSong = song;

                await Task.Delay(500);

                IconPlayer = IconChar.PauseCircle;

                timer.Start();

                await escucha;

                return;
            }

            // Si es una playlist generar una vista que muestre las canciones que tiene
            // esa playlist y si se da clic en alguna canción reproducirla

            if (e is PlaylistModel playlist) {
                CurrentChildView = new PlaylistView(); // <- View para las playlist
                Caption = $"{playlist.Title}";
                Icon = IconChar.Headphones;
                return;
            }

            // Si es un album generar una vista que muestre las canciones que tiene
            // esa playlist y si se da clic en alguna canción reproducirla

            if (e is AlbumModel album) {
                CurrentChildView = new AlbumView(); // <- View para los albums
                Caption = $"{album.Title}";
                Icon = IconChar.CompactDisc;
                return;
            }

        }

        private void UserAccountTest() {

            UserAccount = new(userData: null) {
                Icon = new BitmapImage(new Uri(@"C:\Users\iDeboy\Pictures\Suprime.png")),
                Nickname = "iDeboy"
            };

        }

        private void ExecuteShowPlaylistsCommand(object? obj) {

            CurrentChildView = null;
            Caption = "Tus playlists";
            Icon = IconChar.Music;

        }

        private void ExecuteShowSongsCommand(object? obj) {

            CurrentChildView = null;
            Caption = "Tus me gusta";
            Icon = IconChar.Heart;

        }

        private void ExecuteCreatePlaylistCommand(object? obj) {

            CurrentChildView = new CreatePlaylistView();
            Caption = "Crear playlist";
            Icon = IconChar.PlusSquare;
        }

        private void ExecuteMyLibraryCommand(object? obj) {

            CurrentChildView = null;
            Caption = "Tu biblioteca";
            Icon = IconChar.Book;

        }

        private void ExecuteSearchCommand(object? obj) {

            CurrentChildView = new SearchView();
            Caption = "Buscar";
            Icon = IconChar.Search;

        }

        private void ExecuteHomeCommand(object? obj) {

            CurrentChildView = null;
            Caption = "Inicio";
            Icon = IconChar.Home;

        }

        private void ExecuteCloseCommand(object? obj) {
            //NextWindow = null;
            IsViewVisible = false;
        }
    }
}
