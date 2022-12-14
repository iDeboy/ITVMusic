using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IArtistRepository : IRepository<ArtistModel> {

        Task<IEnumerable<ArtistModel>?> GetFromAsync(SongModel? song);

        IEnumerable<ArtistModel>? GetFrom(SongModel? song);


    }
}
