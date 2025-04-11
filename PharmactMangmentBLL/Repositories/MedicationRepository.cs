using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentBLL.Repositories
{
    public class MedicationRepository : GenaricRepository<Medication>, IMedicationRepository
    {
        public MedicationRepository(PharmaceDbContext PharmaceDb) : base(PharmaceDb)
        {
        }
    }
}
