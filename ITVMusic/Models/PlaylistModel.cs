using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class PlaylistModel : ModelBase, IMusicModelBase {

        public PlaylistModel() { }
        public PlaylistModel(MySqlDataReader reader) {
            Id = Convert.ToUInt32(reader["Playlist_Codigo"]);
            Creationdate = Convert.ToDateTime(reader["Playlist_FechaCreacion"]);
            Title = Convert.ToString(reader["Playlist_Titulo"]);
            Icon = reader["Playlist_Icono"].ToImage();
        }

        public uint Id { get; set; }
        public DateTime Creationdate { get; set; }
        public string? Title { get; set; }
        public ImageSource? Icon { get; set; }

        public ObservableCollection<AlmacenModel> Songs { get; } = new();
        public ObservableCollection<UserModel> Authors { get; } = new();

        public string Type => "Playlist";
        public string Description => $"De {string.Join(", ", AuthorsName)}";
        private string SongCountString => Songs.Count == 1 ? "canción" : "canciones";
        public string Information => $"De {string.Join(", ", AuthorsName)} - {Songs.Count} {SongCountString}, {SumSongsDuration()}";
        public string[] AuthorsName => GetAuthorsName();
        public string Presentation => $"#{Id} {Title}";
        private string SumSongsDuration() {

            string result = "";
            Duration aux = new(TimeSpan.Zero);

            foreach (var song in Songs) {
                if (song.Song is not null) aux += song.Song.Duration;
            }

            if (aux.TimeSpan.Hours > 0) {
                result += $"{aux.TimeSpan.Hours} h ";

                if (aux.TimeSpan.Minutes > 0) {
                    result += $"{aux.TimeSpan.Minutes} min";
                }

                return result;
            }

            if (aux.TimeSpan.Minutes > 0) {
                result += $"{aux.TimeSpan.Minutes} min ";
            }

            result += $"{aux.TimeSpan.Seconds} s";

            return result;
        }
        private string[] GetAuthorsName() {

            List<string> names = new();

            if (Authors is null) return names.ToArray();

            names = new(from author in Authors
                        select author.Nickname);

            return names.ToArray();
        }
    }
}
