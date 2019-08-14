using Microsoft.EntityFrameworkCore;
using Realtor.Model.Context;
using Realtor.Model.Entities;
using Realtor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly RealtorDbContext _dbContext;
        public CustomerService(RealtorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Customer> FindCustomerAsync(int id)
        {
            return await _dbContext.Customers
                .Include(x => x.Phones)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
