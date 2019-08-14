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
    public class PhoneService : IPhoneService
    {
        private readonly RealtorDbContext _dbContext;
        public PhoneService(RealtorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Phone> CreatePhoneAsync(Phone phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            _dbContext.Phones.Add(phone);
            await _dbContext.SaveChangesAsync();

            return phone;
        }

        public async Task DeletePhoneAsync(Phone phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            _dbContext.Phones.Remove(phone);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Phone> FindPhoneAsync(int id)
        {
            return await _dbContext.Phones.FindAsync(id);
        }

        public async Task<IEnumerable<Phone>> GetPhonesAsync(int customerId)
        {
            var orders = await _dbContext.Phones
                .Where(x => x.CustomerId == customerId)
                .ToArrayAsync();

            return orders;
        }

        public async Task<int> GetPhonesCountAsync(int customerId)
        {
            return await _dbContext.Phones.CountAsync(x => x.CustomerId == customerId);
        }

        public async Task<Phone> UpdatePhoneAsync(Phone phone)
        {
            if (phone == null)
                throw new ArgumentNullException(nameof(phone));

            _dbContext.Phones.Update(phone);
            await _dbContext.SaveChangesAsync();

            return phone;
        }
    }
}
