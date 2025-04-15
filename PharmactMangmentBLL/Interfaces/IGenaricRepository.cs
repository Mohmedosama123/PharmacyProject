  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentBLL.Interfaces
{
    public interface IGenaricRepository<T> where T : Base
    {
        Task<IEnumerable<T>> GetAllMedicanAsync();
        Task<T?> GetMedicanAbyIdAsync(int id);
        Task AddPharmacyAsync(T department);

        Task UpdateMedican(T department);
        Task DeleteMedican(T department);
    }
}
