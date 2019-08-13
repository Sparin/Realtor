using AutoMapper;
using Realtor.Model.DTO.Advertisement;
using Realtor.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Realtor.Configuration
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Advertisement, AdvertisementDto>();
            CreateMap<CreateAdvertisementDto, Advertisement>();
            CreateMap<UpdateAdvertisementDto, Advertisement>();
        }
    }
}
