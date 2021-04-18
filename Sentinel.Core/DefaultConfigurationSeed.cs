using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core.Entities;
using Sentinel.Core.Enums;
using Sentinel.Core.Repository.Interfaces;
using Sentinel.Core.Services.Interfaces;

namespace Sentinel.Core
{
    public class DefaultConfigurationSeed
    {
        public static void Seed(ServiceProvider services)
        {
            var dbContext = services.GetService<SentinelDatabaseContext>();

            using var transaction = dbContext.Database.BeginTransaction();

            var revisionRepo = services.GetService<IRevisionRepository>();
            var revision = revisionRepo.CreateNewRevision();
            revision.CommitDate = DateTime.UtcNow;
            revision.ConfirmDate = DateTime.UtcNow;
            revisionRepo.Update(revision);

            var userRepo = services.GetService<IUserRepository>();
            var adminUser = userRepo.CreateUser("admin", "admin");

            var defaultSystemConfig = new SystemConfiguration()
            {
                RevisionId = revision.Id,
                Hostname = "sentinel",
                Domain = "local"
            };

            var systemConfigurationRepo = services.GetService<ISystemConfigurationRepository>();
            systemConfigurationRepo.Create(defaultSystemConfig);

            var kernelInterfaceService = services.GetService<IKernelInterfaceService>();
            var kernelInterfaces = kernelInterfaceService.GetPhysicalInterfaceNames();

            if (kernelInterfaces.Count == 0)
            {
                throw new Exception("No Kernel Interfaces Found");
            }


            // TODO: Review this as it may find a wireless device first on real hardware when one exists
            var lanInterface = new Interface()
            {
                RevisionId = revision.Id,
                Name = kernelInterfaces.First(),
                Description = "LAN",
                InterfaceType = InterfaceType.Ethernet,
                Enabled = true
            };

            var interfaceRepo = services.GetService<IInterfaceRepository>();
            lanInterface = interfaceRepo.Create(lanInterface);

            var lanAddress = new InterfaceAddress()
            {
                RevisionId = revision.Id,
                InterfaceName = lanInterface.Name,
                Address = "192.168.1.1",
                SubnetMask = 24
            };

            var interfaceAddressRepo = services.GetService<IInterfaceAddressRepository>();
            interfaceAddressRepo.Create(lanAddress);


            revisionRepo.SaveChanges();

            transaction.Commit();
        }
    }
}