using ITVMusic.Models;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IUserRepository : IRepository<UserModel> {

        Task<bool> AutenticateUser(NetworkCredential credential);
        Task<bool> ListenToSong(UserModel? user, AlmacenModel? song);
        Task<UserModel?> GetByUsername(string? username);
        Task<UserModel?> GetByNoControlOrUsername(string? key);


    }
}
