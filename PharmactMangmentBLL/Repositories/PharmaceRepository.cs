using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentBLL.Repositories
{
    public class PharmaceRepository : GenaricRepository<Medication>, IPharmaceRepository
    {
        //private readonly PharmaceDbContext _pharmaceDb;

        public PharmaceRepository(PharmaceDbContext PharmaceDb) : base(PharmaceDb)
        {
            //_pharmaceDb = PharmaceDb;
        }
        //public async Task<IEnumerable<Medication>> GetDepartmentByNameAsync(string name)
        //{
        //    return await _pharmaceDb.Medications
        //         .Where(e => e.Name.ToLower()
        //         .Contains(name.ToLower())).ToListAsync();
        //}
    }
}
