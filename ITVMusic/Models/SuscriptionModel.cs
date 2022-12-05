using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Models {
    public class SuscriptionModel : ModelBase {

        public SuscriptionModel() : base() { }
        public SuscriptionModel(MySqlDataReader reader) : base(reader) {

            Id = Convert.ToUInt16(reader["Suscripcion_CodigoSuscripcion"]);
            PaymentMethod = Convert.ToString(reader["Suscripcion_MetodoPago"]);
            Type = Convert.ToString(reader["Suscripcion_Tipo"]);

        }

        public ushort Id { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Type { get; set; }
        public string Suscription => $"{PaymentMethod} ({Type})";
    }
}
