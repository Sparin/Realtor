using LinqKit;
using Microsoft.EntityFrameworkCore;
using Realtor.Model.Context;
using Realtor.Model.DTO.Advertisement;
using Realtor.Model.Entities;
using Realtor.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Services
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly RealtorDbContext _dbContext;

        public AdvertisementService(RealtorDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Advertisement> CreateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            _dbContext.Advertisements.Add(advertisement);
            await _dbContext.SaveChangesAsync();

            return advertisement;
        }

        public async Task DeleteAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            _dbContext.Advertisements.Remove(advertisement);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Advertisement> FindAdvertisementAsync(int id)
        {
            return await _dbContext.Advertisements.FindAsync(id);
        }

        public async Task<IEnumerable<Advertisement>> GetAdvertisementsAsync(SearchRequest request, int page, int limit)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = BuildSearchQuery(request);

            var orders = await query
                .Skip(page * limit)
                .Take(limit)
                .ToArrayAsync();

            return orders;
        }

        public async Task<int> GetAdvertisementsCountAsync(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var query = BuildSearchQuery(request);

            var totalItems = await query.CountAsync();

            return totalItems;
        }

        public async Task<Advertisement> UpdateAdvertisementAsync(Advertisement advertisement)
        {
            if (advertisement == null)
                throw new ArgumentNullException(nameof(advertisement));

            _dbContext.Advertisements.Update(advertisement);
            await _dbContext.SaveChangesAsync();

            return advertisement;
        }

        private IQueryable<Advertisement> BuildSearchQuery(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var predicate = PredicateBuilder.New<Advertisement>();

            predicate = PredicateBuilder.Or<Advertisement>(predicate, advertisement =>
                (request.MinimumPrice == null || advertisement.Price >= request.MinimumPrice) &&
                (request.MaximumPrice == null || advertisement.Price <= request.MaximumPrice) &&
                (request.MinimumRooms == null || advertisement.RoomsCount >= request.MinimumRooms) &&
                (request.MaximumRooms == null || advertisement.RoomsCount <= request.MaximumRooms) &&
                (request.Type == null || advertisement.Type == request.Type)
            );

            IQueryable<Advertisement> query = _dbContext.Advertisements
                .Where(predicate);

            // Custom ordering 
            query = query.OrderByDescending(x => x.Id);

            return query;
        }
    }
}
