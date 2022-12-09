using ITVMusic.Models;
using ITVMusic.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ITVMusic.ViewModels {
    public class EditPlaylistViewModel : ViewModelBase {

        // Fields
        private ImageSource? m_Icon;
        private string? m_Title;
        private ObservableCollection<AlmacenModel>? m_SelectedSongs;
        private ObservableCollection<AlmacenModel>? m_Songs;
        private ObservableCollection<PlaylistModel>? m_Playlists;
        private AlmacenModel? m_Song;
        private AlmacenModel? m_SelectedSong;
        private PlaylistModel? m_Playlist;

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

        public AlmacenModel? SelectedSong {
            get => m_SelectedSong;
            set {
                m_SelectedSong = value;
                OnPropertyChanged();
            }

        }

        public ObservableCollection<PlaylistModel>? Playlists {
            get => m_Playlists;
            set {
                m_Playlists = value;
                OnPropertyChanged();
            }
        }
        public PlaylistModel? Playlist {
            get => m_Playlist;
            set {
                m_Playlist = value;
                OnPropertyChanged();
            }
        }

        private List<AlmacenModel> RemovedSongs { get; }

        // Commands
        public ICommand EditCommand { get; }
        public ICommand AddSongCommand { get; }
        public ICommand RemoveSongCommand { get; }

        public EditPlaylistViewModel() {

            RemovedSongs = new();

            Playlists = new(from it in App.MusicItems
                            where it is PlaylistModel model && model.Authors.Any(u => u.NoControl == App.UserData!.NoControl)
                            select (PlaylistModel)it);

            EditCommand = new ViewModelCommand(ExecuteEditCommand, CanExecuteEditCommand);
            AddSongCommand = new ViewModelCommand(ExecuteAddSongCommand, CanExecuteAddSongCommand);
            RemoveSongCommand = new ViewModelCommand(ExecuteRemoveSongCommand, CanExecuteRemoveSongCommand);

        }

        private bool CanExecuteRemoveSongCommand(object? obj) {

            if (SelectedSongs is null || !SelectedSongs.Any()) return false;

            if (SelectedSong is null) return false;

            return true;
        }

        private void ExecuteRemoveSongCommand(object? obj) {

            Songs ??= new();

            Songs.Add(SelectedSong!);

            Songs = new(Songs.OrderBy(x => x.Id));

            RemovedSongs.Add(SelectedSong!);

            SelectedSongs!.Remove(SelectedSong!);
        }

        private bool CanExecuteAddSongCommand(object? obj) {

            if (Songs is null || !Songs.Any()) return false;

            if (Song is null) return false;

            return true;

        }

        private void ExecuteAddSongCommand(object? obj) {

            SelectedSongs ??= new();

            SelectedSongs.Add(Song!);

            Songs!.Remove(Song!);

            Song = null;

        }

        private bool CanExecuteEditCommand(object? obj) {

            if (Playlist is null) return false;

            if (string.IsNullOrWhiteSpace(Title)) return false;

            if (SelectedSongs is null || !SelectedSongs.Any()) return false;

            if (Playlist.Songs.Count == SelectedSongs.Count) {

                for (int i = 0; i < Playlist.Songs.Count; i++) {

                    if (Playlist.Songs[i].Id != SelectedSongs[i].Id)
                        return true;

                }

                if (Playlist.Title == Title && Playlist.Icon == Icon)
                    return false;


            }


            return true;

        }

        private async void ExecuteEditCommand(object? obj) {

            if (Playlist is null) return;

            Playlist.Title = Title;
            Playlist.Icon = Icon;

            Playlist.Songs.Clear();
            Playlist.Songs.AddRange(SelectedSongs);

            App.PlaylistRepository.Edit(Playlist);

            List<Task> tasks = new();

            foreach (var removedSong in RemovedSongs) {
                tasks.Add(App.PlaylistRepository.UnattatchSongAsync(removedSong, Playlist));
            }

            await Task.WhenAll(tasks);
            tasks.Clear();

            foreach (var song in Playlist.Songs) {
                tasks.Add(App.PlaylistRepository.AttatchSongAsync(song, Playlist));
            }

            await Task.WhenAll(tasks);

            await ClearAsync();

            MessageBox.Show("Playlist editada.");
        }

        private void Clear() {
            Playlist = null;

            RemovedSongs.Clear();

            Playlists = new(from it in App.MusicItems
                            where it is PlaylistModel model && model.Authors.Any(u => u.NoControl == App.UserData!.NoControl)
                            select (PlaylistModel)it);
        }

        private Task ClearAsync() {
            return Task.Run(Clear);
        }
    }
}
