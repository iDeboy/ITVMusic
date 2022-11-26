using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class SongModel : IMusicModelBase {
        public uint Id { get; set; }
        public ImageSource? Icon { get; set; }
        public string Type => "Canción";
        public string? Title { get; set; }
        public string Description => "De: @Artistas";
        public string[]? Genders { get; set; }
        public byte[]? Bytes { get; set; }
        public Duration Duration { get; set; }
        public AlbumModel? Album { get; set; }
        public uint Reproductions { get; set; }

        public string Information => Description;

        public Stream GetStream() {

            if (Bytes is null) return Stream.Null;

            return new MemoryStream(Bytes);

        }
    }
}
