using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface ISongRepository : IRepository<SongModel> {

        bool AttatchArtist(SongModel? song, ArtistModel? artist);
        IEnumerable<SongModel>? GetFrom(ArtistModel? artist);
        SongModel? GetFrom(AlmacenModel? almacen);

        Task<bool> AttatchArtistAsync(SongModel? song, ArtistModel? artist);
        Task<IEnumerable<SongModel>?> GetFromAsync(ArtistModel? artist);
        Task<SongModel?> GetFromAsync(AlmacenModel? almacen);
    }
}
