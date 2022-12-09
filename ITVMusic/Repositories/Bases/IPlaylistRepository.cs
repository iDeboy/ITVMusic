using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IPlaylistRepository : IRepository<PlaylistModel> {

        bool AttatchAuthor(UserModel? user, PlaylistModel? playlist);
        bool AttatchSong(AlmacenModel? song, PlaylistModel? playlist);
        bool UnattatchSong(AlmacenModel? song, PlaylistModel? playlist);
        IEnumerable<PlaylistModel>? GetFrom(UserModel? user);

        Task<bool> AttatchAuthorAsync(UserModel? user, PlaylistModel? playlist);
        Task<bool> AttatchSongAsync(AlmacenModel? song, PlaylistModel? playlist);
        Task<bool> UnattatchSongAsync(AlmacenModel? song, PlaylistModel? playlist);
        Task<IEnumerable<PlaylistModel>?> GetFromAsync(UserModel? user);

    }
}
