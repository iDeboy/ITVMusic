using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface ISuscriptionRepository : IRepository<SuscriptionModel> {

        bool AutenticateUserSuscription(UserModel? user);
        Task<bool> AutenticateUserSuscriptionAsync(UserModel? user);

    }
}
