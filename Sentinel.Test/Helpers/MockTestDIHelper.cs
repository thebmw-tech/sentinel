using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Sentinel.Test.Helpers
{
    /// <summary>
    /// Dependency Injection Helper for use when doing Mocked Testing
    /// </summary>
    /// <typeparam name="TTarget">The Class Being Tested</typeparam>
    public class MockTestDIHelper<TTarget> : BaseTestDIHelper<TTarget>
    {
        private Dictionary<Type, Mock> mocks;
        

        public MockTestDIHelper() : this(new Dictionary<Type, object>())
        {
            
        }

        public MockTestDIHelper(Dictionary<Type, object> customObjects) : base(customObjects)
        {
            mocks = new Dictionary<Type, Mock>();

            GenerateMocks();
        }

        public Mock<TMock> GetMock<TMock>() where TMock : class
        {
            return (Mock<TMock>) mocks[typeof(TMock)];
        }

        protected override object GetObjectOfType(Type type)
        {
            var overrideObject = base.GetObjectOfType(type);
            if (overrideObject != null)
            {
                return overrideObject;
            }
            
            if (mocks.ContainsKey(type))
            {
                return mocks[type].Object;
            }

            return null;
        }

        private void GenerateMocks()
        {
            var t = typeof(TTarget);
            var constructorParams = t.GetConstructors().First().GetParameters();

            foreach (var constructorParam in constructorParams)
            {
                if (!objects.ContainsKey(constructorParam.ParameterType))
                {
                    var mockType = typeof(Mock<>).MakeGenericType(constructorParam.ParameterType);
                    var mockInstance = (Mock) Activator.CreateInstance(mockType);
                    mocks.Add(constructorParam.ParameterType, mockInstance);
                }
            }
        }
    }
}