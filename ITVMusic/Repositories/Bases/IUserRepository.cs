using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IUserRepository {

        Task<bool> AutenticateUser(NetworkCredential credential);
        Task<bool> Add(UserModel? user);
        Task<bool> Edit(UserModel? user);
        Task<bool> Remove(string? noControl);
        Task<UserModel?> GetByNoControl(string? noControl);
        Task<UserModel?> GetByUsername(string? username);
        Task<UserModel?> GetByNoControlOrUsername(string? key);
        Task<IEnumerable<UserModel>> GetByAll();

    }
}
