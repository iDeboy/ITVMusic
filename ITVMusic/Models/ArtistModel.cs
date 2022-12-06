using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class ArtistModel : ModelBase {

        public ArtistModel() : base() { }

        public ArtistModel(MySqlDataReader reader)
            : base(reader) {

            Id = Convert.ToUInt32(reader["Artista_Codigo"]);
            Name = Convert.ToString(reader["Artista_Nombre"]);
            Description = Convert.ToString(reader["Artista_Descripcion"]);
            Icon = reader["Artista_Icono"].ToImage();

        }

        public uint Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ImageSource? Icon { get; set; }

        public ObservableCollection<SongModel> Songs { get; } = new();

    }
}
