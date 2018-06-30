using AutoMapper;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtistWebService.Data
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Artist, ArtistDto>();
            CreateMap<Album, AlbumDto>().ReverseMap();
        }
    }
}
