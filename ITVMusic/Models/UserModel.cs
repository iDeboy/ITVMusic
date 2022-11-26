using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class UserModel {
        public string? NoControl { get; set; }
        public ImageSource? Icon { get; set; }
        public string? Name { get; set; }
        public string? LastNamePat { get; set; }
        public string? LastNameMat { get; set; }
        public string? Nickname { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public DateOnly Birthday { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public DateTime ContratationDate { get; set; }

    }
}
