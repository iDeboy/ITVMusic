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
    public class AlbumModel : ModelBase, IMusicModelBase {

        public AlbumModel() { }
        public AlbumModel(MySqlDataReader reader)
            : base(reader) {

            Id = Convert.ToUInt32(reader["Album_Codigo"]);
            Title = Convert.ToString(reader["Album_Titulo"]);
            RealeseDate = Convert.ToDateTime(reader["Album_Lanzamiento"]).ToDateOnly();
            Icon = reader["Album_Icono"].ToImage();
        }

        public uint Id { get; set; }
        public string? Title { get; set; }
        public DateOnly RealeseDate { get; set; }
        public ImageSource? Icon { get; set; }

        public ObservableCollection<AlmacenModel> Songs { get; } = new();

        public string Type => "Álbum";
        public string Description => $"{RealeseDate:dd MMM yyyy}";
        private string SongCountString => Songs.Count == 1 ? "canción" : "canciones";
        public string Information => $"{RealeseDate:dd MMM yyyy} - {Songs.Count} {SongCountString}, {SumSongsDuration()}";
        public string[] ArtistsNames => GetArtistsNames();
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
        private string[] GetArtistsNames() {

            var artists = (from song in Songs
                           where song.Song is not null
                           select song.Song!.Artists).FirstOrDefault();


            List<string> names = new();

            if (artists is null) return names.ToArray();

            names.AddRange(from artist in artists
                           where artist.Name is not null
                           select artist.Name);

            return names.ToArray();
        }
    }
}
