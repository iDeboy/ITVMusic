using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IAlmacenRepository : IRepository<AlmacenModel> {

        Task<IEnumerable<AlmacenModel>?> GetFromAsync(UserModel? user);
        Task<IEnumerable<AlmacenModel>?> GetFromAsync(AlbumModel? album);
        Task<IEnumerable<AlmacenModel>?> GetFromAsync(PlaylistModel? playlist);
        Task<AlmacenModel?> GetFromAsync(SongModel? song, AlbumModel? album);
        Task<uint> GetReproductionsAsync(AlmacenModel? almacen);

        IEnumerable<AlmacenModel>? GetFrom(UserModel? user);
        IEnumerable<AlmacenModel>? GetFrom(AlbumModel? album);
        IEnumerable<AlmacenModel>? GetFrom(PlaylistModel? playlist);
        AlmacenModel? GetFrom(SongModel? song, AlbumModel? album);
        uint GetReproductions(AlmacenModel? almacen);



    }
}
