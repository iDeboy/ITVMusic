using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ITVMusic.Models {
    public interface IMusicModelBase {

        public uint Id { get; }
        public ImageSource? Icon { get; }
        public string Type { get; }
        public string? Title { get; }
        public string Description { get; }

    }
}
