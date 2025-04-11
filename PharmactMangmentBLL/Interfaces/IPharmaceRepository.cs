using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentBLL.Interfaces
{
    public interface IPharmaceRepository : IGenaricRepository<Medication>
    {
        //Task<IEnumerable<Medication>> GetDepartmentByNameAsync(string name);
    }
}
