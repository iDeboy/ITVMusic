using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IAlmacenRepository : IRepository<AlmacenModel> {

        public Task<uint> GetReproductions(AlmacenModel? almacen);

        public Task<AlbumModel?> GetAlbum(AlmacenModel? almacen);
        public Task<SongModel?> GetSong(AlmacenModel? almacen);
    }
}
