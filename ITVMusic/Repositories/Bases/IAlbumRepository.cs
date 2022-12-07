using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IAlbumRepository : IRepository<AlbumModel> {

        Task<AlbumModel?> GetFromAsync(AlmacenModel? almacen);

        AlbumModel? GetFrom(AlmacenModel? almacen);

    }
}
