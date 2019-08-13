using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.DTO.Advertisement
{
    public class SearchRequest
    {
        public AdvertisementType? Type { get; set; }
        public int? MinimumRooms { get; set; }
        public int? MaximumRooms { get; set; }
        public double? MinimumPrice { get; set; }
        public double? MaximumPrice { get; set; }
    }
}
