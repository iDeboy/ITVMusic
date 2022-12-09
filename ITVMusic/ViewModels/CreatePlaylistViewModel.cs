using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ITVMusic.ViewModels {
    public class CreatePlaylistViewModel : ViewModelBase {

        // Fields
        private ImageSource? m_Icon;
        private string? m_Title;
        private ObservableCollection<AlmacenModel>? m_SelectedSongs;
        private ObservableCollection<AlmacenModel>? m_Songs;
        private AlmacenModel? m_Song;

        public ImageSource? Icon {
            get => m_Icon;
            set {
                m_Icon = value;
                OnPropertyChanged();
            }
        }

        public string? Title {
            get => m_Title;
            set {
                m_Title = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AlmacenModel>? SelectedSongs {
            get => m_SelectedSongs;
            set {
                m_SelectedSongs = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AlmacenModel>? Songs {
            get => m_Songs;
            set {
                m_Songs = value;
                OnPropertyChanged();
            }
        }

        public AlmacenModel? Song {
            get => m_Song;
            set {
                m_Song = value;
                OnPropertyChanged();
            }

        }

        // Commands
        public ICommand CreateCommand { get; }
        public ICommand AddSongCommand { get; }

        public CreatePlaylistViewModel() {

            Songs = new(from it in App.MusicItems
                        where it is AlmacenModel
                        select (AlmacenModel)it);

            SelectedSongs = new();

            CreateCommand = new ViewModelCommand(ExecuteCreateCommand, CanExecuteCreateCommand);
            AddSongCommand = new ViewModelCommand(ExecuteAddSongCommand, CanExecuteAddSongCommand);
        }

        private bool CanExecuteAddSongCommand(object? obj) {

            if (Songs is null || !Songs.Any()) return false;

            if (Song is null) return false;

            return true;
        }

        private void ExecuteAddSongCommand(object? obj) {

            if (Song is null || Songs is null) return;

            SelectedSongs ??= new();

            SelectedSongs.Add(Song);

            Songs.Remove(Song);

            Song = null;
        }

        private bool CanExecuteCreateCommand(object? obj) {

            if (string.IsNullOrWhiteSpace(Title)) return false;

            if (SelectedSongs is null || !SelectedSongs.Any()) return false;

            return true;
        }

        private async void ExecuteCreateCommand(object? obj) {

            if (SelectedSongs is null) return;

            PlaylistModel playlist = new() {
                Icon = Icon,
                Title = Title,
            };

            App.PlaylistRepository.Add(playlist);

            playlist = App.PlaylistRepository.GetByAll().Last();

            App.PlaylistRepository.AttatchAuthor(App.UserData, playlist);
            playlist.Authors.Add(App.UserData!);

            List<Task> tasks = new();

            foreach (var song in SelectedSongs) {
                tasks.Add(App.PlaylistRepository.AttatchSongAsync(song, playlist));
                playlist.Songs.Add(song);
            }

            await Task.WhenAll(tasks);

            await ClearAsync();

            App.MusicItems.Add(playlist);

            MessageBox.Show("Playlist creada.");
        }

        private void Clear() {
            Icon = null;
            Title = null;
            SelectedSongs = null;

            Songs = new(from it in App.MusicItems
                        where it is AlmacenModel
                        select (AlmacenModel)it);

            SelectedSongs = new();
        }

        private Task ClearAsync() {
            return Task.Run(Clear);

        }
    }
}
