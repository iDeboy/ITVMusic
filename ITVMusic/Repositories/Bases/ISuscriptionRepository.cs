using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface ISuscriptionRepository {

        Task<bool> AutenticateUserSuscription(UserModel? user);
        Task<bool> Add(SuscriptionModel? suscription);
        Task<bool> Edit(SuscriptionModel? suscription);
        Task<bool> Remove(uint id);
        Task<SuscriptionModel?> GetById(uint id);
        Task<IEnumerable<SuscriptionModel>> GetByAll();
    }
}
