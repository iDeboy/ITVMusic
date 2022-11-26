using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Models {
    public class PlaylistModel : IMusicModelBase {
        public uint Id { get; set; }
        public ImageSource? Icon { get; set; }
        public string Type => "Playlist";
        public string? Title { get; set; }
        public string Description { get; set; } = string.Empty;

        public DateTime Creationdate { get; set; }
    }
}
