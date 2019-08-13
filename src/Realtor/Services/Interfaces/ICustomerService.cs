using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> FindCustomerAsync(int id);
    }
}
