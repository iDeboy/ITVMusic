using ITVMusic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITVMusic.Repositories.Bases {
    public interface IRepository<T> {

        Task<bool> AddAsync(T? obj);
        Task<bool> EditAsync(T? obj);
        Task<bool> RemoveByIdAsync(object? id);
        Task<T?> GetByIdAsync(object? id);
        Task<IEnumerable<T>> GetByAllAsync();

        bool Add(T? obj);
        bool Edit(T? obj);
        bool RemoveById(object? id);
        T? GetById(object? id);
        IEnumerable<T> GetByAll();

    }
}
