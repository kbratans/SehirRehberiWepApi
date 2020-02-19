using AutoMapper;
using SehirRehberi.Dtos;
using SehirRehberi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Helper
{
    public class AutoMapperProfiles:Profile
    {

        public AutoMapperProfiles()
        {
            CreateMap<City, CityForList>().ForMember(dest => dest.PhotoUrl, opt =>
              {

                  opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);

              });


            CreateMap<City, CityForDetail>();
            CreateMap<Photo, PhotoForCreation>();
            CreateMap<PhotoForReturn, Photo>();

        }




    }
}
