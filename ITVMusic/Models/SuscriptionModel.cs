using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Models {
    public class SuscriptionModel {

        public int Id { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Suscription => $"{PaymentMethod} ({Type})";
    }
}
