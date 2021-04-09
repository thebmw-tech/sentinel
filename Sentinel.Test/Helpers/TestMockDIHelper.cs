using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Sentinel.Test.Helpers
{
    public class TestMockDIHelper<TTarget>
    {
        private Dictionary<Type, Mock> mocks;
        private Dictionary<Type, object> objects;
        private TTarget instance;

        public TestMockDIHelper() : this(new Dictionary<Type, object>())
        {
            
        }

        public TestMockDIHelper(Dictionary<Type, object> customObjects)
        {
            objects = customObjects;
            mocks = new Dictionary<Type, Mock>();

            GenerateMocks();
            GenerateInstance();
        }

        public TTarget GetInstance()
        {
            return instance;
        }

        public Mock<TMock> GetMock<TMock>() where TMock : class
        {
            return (Mock<TMock>) mocks[typeof(TMock)];
        }

        public TObject GetObject<TObject>()
        {
            return (TObject) objects[typeof(TObject)];
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
                    objects.Add(constructorParam.ParameterType, mockInstance?.Object);
                }
            }
        }

        private void GenerateInstance()
        {
            var t = typeof(TTarget);
            var constructorParams = t.GetConstructors().First().GetParameters();
            var args = new List<object>();

            foreach (var constructorParam in constructorParams)
            {
                args.Add(objects[constructorParam.ParameterType]);
            }

            instance = (TTarget) Activator.CreateInstance(t, args.ToArray());
        }
    }
}