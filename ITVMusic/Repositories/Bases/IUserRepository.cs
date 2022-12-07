using ITVMusic.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IUserRepository : IRepository<UserModel> {

        Task<bool> AutenticateUserAsync(NetworkCredential credential);
        Task<bool> ListenToSongAsync(UserModel? user, AlmacenModel? song);
        Task<UserModel?> GetByUsernameAsync(string? username);
        Task<UserModel?> GetByEmailAsync(string? email);
        Task<UserModel?> GetByNoControlOrUsernameAsync(string? key);
        Task<IEnumerable<UserModel>?> GetFromAsync(PlaylistModel playlist);

        bool AutenticateUser(NetworkCredential credential);
        bool ListenToSong(UserModel? user, AlmacenModel? song);
        UserModel? GetByUsername(string? username);
        UserModel? GetByEmail(string? email);
        UserModel? GetByNoControlOrUsername(string? key);
        IEnumerable<UserModel>? GetFrom(PlaylistModel playlist);

    }
}
