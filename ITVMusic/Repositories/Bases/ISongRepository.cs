using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface ISongRepository : IRepository<SongModel> {

        public Task<bool> AttatchArtist(SongModel? song, ArtistModel? artist);

    }
}
