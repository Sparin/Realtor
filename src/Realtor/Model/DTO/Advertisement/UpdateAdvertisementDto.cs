using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.DTO.Advertisement
{
    public class UpdateAdvertisementDto
    {
        public AdvertisementType Type { get; set; }
        public string ShortDescription { get; set; }
        public int RoomsCount { get; set; }
        public double Price { get; set; }
    }
}
