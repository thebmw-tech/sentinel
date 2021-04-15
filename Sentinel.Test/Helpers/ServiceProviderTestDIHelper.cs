using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Sentinel.Core;

namespace Sentinel.Test.Helpers
{
    /// <summary>
    /// Dependency Injection Helper for use when doing Integration Testing
    /// </summary>
    /// <typeparam name="TTarget">The Class Being Tested</typeparam>
    public class ServiceProviderTestDIHelper<TTarget> : BaseTestDIHelper<TTarget>
    {
        private readonly IServiceProvider services;

        public ServiceProviderTestDIHelper() : this(new Dictionary<Type, object>())
        {

        }

        public ServiceProviderTestDIHelper(Dictionary<Type, object> customObjects) : base(customObjects)
        {
            services = new ServiceCollection()
                .RegisterSentinelCore()
                //.RegisterSentinelCoreCommand()
                .BuildServiceProvider();
        }

        protected override object GetObjectOfType(Type type)
        {
            var overrideObject = base.GetObjectOfType(type);
            if (overrideObject != null)
            {
                return overrideObject;
            }

            var service = services.GetService(type);
            if (service != null)
            {
                return service;
            }

            throw new Exception($"Could not find instance of type '{type.FullName}'");
        }
    }
}