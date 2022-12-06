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
    public class UserModel : ModelBase {

        public UserModel()
            : base() { }

        public UserModel(MySqlDataReader reader)
            : base(reader) {

            NoControl = Convert.ToString(reader["Usuario_NoControl"]);
            SuscriptionId = Convert.ToUInt16(reader["Suscripcion_CodigoSuscripcion"]);
            Icon = reader["Usuario_Icono"].ToImage();
            Name = Convert.ToString(reader["Usuario_Nombre"]);
            LastNamePat = Convert.ToString(reader["Usuario_ApellidoPaterno"]);
            LastNameMat = Convert.ToString(reader["Usuario_ApellidoMaterno"]);
            Nickname = Convert.ToString(reader["Usuario_Nickname"]);
            Gender = Convert.ToString(reader["Usuario_Genero"]);
            Email = Convert.ToString(reader["Usuario_Correo"]);
            Birthday = Convert.ToDateTime(reader["Usuario_FechaNacimiento"]).ToDateOnly();
            PhoneNumber = Convert.ToString(reader["Usuario_Telefono"]);
            Password = string.Empty;
            ContratationDate = Convert.ToDateTime(reader["Usuario_FechaContratacion"]);

        }

        public string? NoControl { get; set; }
        public ushort SuscriptionId { get; set; }
        public ImageSource? Icon { get; set; }
        public string? Name { get; set; }
        public string? LastNamePat { get; set; }
        public string? LastNameMat { get; set; }
        public string? Nickname { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public DateOnly Birthday { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Password { get; set; }
        public DateTime ContratationDate { get; set; }

        public ObservableCollection<PlaylistModel> Playlists { get; } = new();
        public ObservableCollection<AlmacenModel> Songs { get; } = new();
    }
}
