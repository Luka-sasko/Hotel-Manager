using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IRoleTypeRepository
    {
        Task<string> GetByIdAsync(Guid id);
    }
}
