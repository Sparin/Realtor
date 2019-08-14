using Realtor.Model.DTO.Advertisement;
using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Services.Interfaces
{
    public interface IAdvertisementService
    {
        Task<Advertisement> CreateAdvertisementAsync(Advertisement advertisement);
        Task<Advertisement> FindAdvertisementAsync(int id);
        Task<IEnumerable<Advertisement>> GetAdvertisementsAsync(SearchRequest request, int page, int limit);
        Task<int> GetAdvertisementsCountAsync(SearchRequest request);
        Task<Advertisement> UpdateAdvertisementAsync(Advertisement advertisement);
        Task DeleteAdvertisementAsync(Advertisement advertisement);
    }
}
