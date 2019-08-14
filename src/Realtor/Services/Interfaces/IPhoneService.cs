using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Services.Interfaces
{
    public interface IPhoneService
    {
        Task<Phone> CreatePhoneAsync(Phone phone);
        Task<Phone> FindPhoneAsync(int id);
        Task<IEnumerable<Phone>> GetPhonesAsync(int customerId);
        Task<int> GetPhonesCountAsync(int customerId);
        Task<Phone> UpdatePhoneAsync(Phone phone);
        Task DeletePhoneAsync(Phone phone);
    }
}
