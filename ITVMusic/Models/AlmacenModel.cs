using ITVMusic.Util;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class AlmacenModel : ModelBase, IMusicModelBase {
        public AlmacenModel() : base() { }
        public AlmacenModel(MySqlDataReader reader)
            : base(reader) {

            Id = Convert.ToUInt32(reader["Almacena_Codigo"]);
            Date = Convert.ToDateTime(reader["Fecha"]);

            Reproductions = reader["CantidadEscuchada"].ToUInt32();

            Song = new SongModel(reader);
            Album = new AlbumModel(reader);
        }

        public uint Id { get; set; }
        public DateTime Date { get; set; }
        public SongModel? Song { get; set; }
        public AlbumModel? Album { get; set; }

        public uint Reproductions { get; set; }

        public ImageSource? Icon => Song?.Icon;

        public string? Type => Song?.Type;

        public string? Title => Song?.Title;

        public string? Description => $"{Song?.Description} - {Album?.Title}";

        public string? Information => Description;
    }
}
