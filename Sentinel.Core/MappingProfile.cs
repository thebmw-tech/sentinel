using AutoMapper;
using Sentinel.Core.Entities;
using Sentinel.Models;

namespace Sentinel.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Revision, RevisionDTO>().ReverseMap();
            CreateMap<Interface, InterfaceDTO>().ReverseMap();
            CreateMap<FirewallTable, FirewallTableDTO>().ReverseMap();
        }
    }
}