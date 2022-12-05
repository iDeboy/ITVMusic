using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IPlaylistRepository : IRepository<PlaylistModel> {

        Task<bool> AttatchAuthor(UserModel? user, PlaylistModel? playlist);
        Task<bool> AttatchSong(AlmacenModel? song, PlaylistModel? playlist);

    }
}
