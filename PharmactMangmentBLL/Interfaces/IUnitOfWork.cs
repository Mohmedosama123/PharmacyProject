using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmactMangmentBLL.Interfaces
{
    public interface IUnitOfWork
    {
        IPharmaceRepository pharmaceRepository { get; }
        Task<int> completeAsync();
    }
}
