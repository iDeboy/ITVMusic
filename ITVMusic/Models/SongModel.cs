using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class SongModel : ModelBase, IMusicModelBase {

        public SongModel() : base() { }
        public SongModel(MySqlDataReader reader)
            : base(reader) {

            //var table = reader.GetSchemaTable();

            Id = Convert.ToUInt32(reader["Cancion_Codigo"]);
            Title = Convert.ToString(reader["Cancion_Titulo"]);
            Genders = Convert.ToString(reader["Cancion_Genero"])?.Split(",", StringSplitOptions.TrimEntries);
            Duration = (TimeSpan)reader["Cancion_Duracion"];
            Bytes = (byte[])reader["Cancion_Bytes"];
            Icon = reader["Cancion_Icono"].ToImage();

        }

        public uint Id { get; set; }
        public string? Title { get; set; }
        public string[]? Genders { get; set; }
        public byte[]? Bytes { get; set; }
        public Duration Duration { get; set; }
        public ImageSource? Icon { get; set; }

        public ObservableCollection<ArtistModel> Artists { get; } = new();

        public string Type => "Canción";
        public string Description => "";//$"De {string.Join(", ", ArtistsNames)}";
        public string Information => Description;

        public string[] ArtistsNames => GetArtistsNames();

        public Stream GetStream() {

            if (Bytes is null) return Stream.Null;

            return new MemoryStream(Bytes);

        }
        private string[] GetArtistsNames() {


            List<string> names = new();

            names.AddRange(from artist in Artists
                           where artist.Name is not null
                           select artist.Name);

            return names.ToArray();
        }
    }
}
