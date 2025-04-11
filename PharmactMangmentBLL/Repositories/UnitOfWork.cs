using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentDAL.Data.Contexts;

namespace PharmactMangmentBLL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public IMedicationRepository medicationRepository { get; set; }
        public IPharmaceRepository pharmaceRepository { get; }

        public PharmaceDbContext _context { get; }

        public UnitOfWork(PharmaceDbContext Context)
        {
            _context = Context;
            medicationRepository = new MedicationRepository(_context);
            pharmaceRepository = new PharmaceRepository(_context);
        }

        async Task<int> IUnitOfWork.completeAsync()
        {
            return await _context.SaveChangesAsync();
        }

    }
}
