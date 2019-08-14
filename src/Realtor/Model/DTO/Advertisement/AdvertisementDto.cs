using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Model.DTO.Advertisement
{
    public class AdvertisementDto
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }

        public AdvertisementType Type { get; set; }
        public string ShortDescription { get; set; }
        public int RoomsCount { get; set; }
        public double Price { get; set; }
    }
}
