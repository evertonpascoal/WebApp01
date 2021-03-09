using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp01.Shared.Domain;
using WebApp01.Web.DTOs;
using WebApp01.Web.Helpers;

namespace WebApp01.Web.Profiles
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<Person, PersonDTO>()
                .ForMember(
                    dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBith.GetCurrentAge()));

            CreateMap<PersonForCreationDTO, Person>();
        }
    }
}
