using Sentinel.Core.Entities;
using Sentinel.Core.Services.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Models;

namespace Sentinel.Core.Services
{
    public class InterfaceService : IInterfaceService
    {
        private readonly IInterfaceRepository interfaceRepository;

        private readonly IMapper mapper;

        public InterfaceService(IInterfaceRepository interfaceRepository, IMapper mapper)
        {
            this.interfaceRepository = interfaceRepository;
            this.mapper = mapper;
        }

        public InterfaceDTO GetInterfaceWithName(int revisionId, string name)
        {
            var @interface = interfaceRepository.Find(i => i.Name == name && i.RevisionId == revisionId);
            return mapper.Map<InterfaceDTO>(@interface);
        }
    }
}