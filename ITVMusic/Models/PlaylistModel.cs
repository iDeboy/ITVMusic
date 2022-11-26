using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class PlaylistModel : IMusicModelBase {
        public uint Id { get; set; }
        public ImageSource? Icon { get; set; }
        public string Type => "Playlist";
        public string? Title { get; set; }
        public string Description => "De @Creadores";
        public DateTime Creationdate { get; set; }
        public ObservableCollection<SongModel> Songs { get; set; } = new();
        private string SongCountString => Songs.Count == 1 ? "canción" : "canciones";
        public string Information => $"@Creadores - {Songs.Count} {SongCountString}, {SumSongsDuration()}";
        private string SumSongsDuration() {

            string result = "";
            Duration aux = new(TimeSpan.Zero);

            foreach (var song in Songs) {
                aux += song.Duration;
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
    }
}
