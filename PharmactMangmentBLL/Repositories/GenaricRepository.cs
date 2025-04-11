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
    public class GenaricRepository<T> : IGenaricRepository<T> where T : Base
    {
        private readonly PharmaceDbContext _PharmaceDb;

        public GenaricRepository(PharmaceDbContext PharmaceDb)
        {
            _PharmaceDb = PharmaceDb;
        }



        public async Task<IEnumerable<T>> GetAllMedicanAsync()
        {
            return await _PharmaceDb.Set<T>().ToListAsync();
        }

        //public async Task<T?> GetMedicanAbyIdAsync(int id)
        //{
        //    return await _PharmaceDb.Set<T>().FindAsync(id);
        //}

        public async Task AddPharmacyAsync(T medican)
        {
            await _PharmaceDb.Set<T>().AddAsync(medican);

        }

        //public async Task UpdateMedican(T medican)
        //{
        //    _PharmaceDb.Set<T>().Update(medican);
        //}

        //public async Task DeleteMedican(T medican)
        //{
        //    _PharmaceDb.Set<T>().Remove(medican);
        //}

       
    }
}
