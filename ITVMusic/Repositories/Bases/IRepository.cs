using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IRepository<T> {

        Task<bool> Add(T? obj);
        Task<bool> Edit(T? obj);
        Task<bool> RemoveById(object id);
        Task<T?> GetById(object id);
        Task<IEnumerable<T>> GetByAll();

    }
}
