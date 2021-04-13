using System;
using System.Collections.Generic;
using System.Linq;

namespace Sentinel.Test.Helpers
{
    public abstract class BaseTestDIHelper<TTarget>
    {
        protected Dictionary<Type, object> objects;
        protected TTarget instance;

        public BaseTestDIHelper(Dictionary<Type, object> customObjects)
        {
            objects = customObjects;
        }

        public TTarget GetInstance()
        {
            if (instance == null)
            {
                GenerateInstance();
            }
            return instance;
        }

        protected virtual object GetObjectOfType(Type type)
        {
            return objects.ContainsKey(type) ? objects[type] : null;
        }

        public TObject GetObject<TObject>()
        {
            return (TObject)GetObjectOfType(typeof(TObject));
        }

        private void GenerateInstance()
        {
            var t = typeof(TTarget);
            var constructorParams = t.GetConstructors().First().GetParameters();
            var args = new List<object>();

            foreach (var constructorParam in constructorParams)
            {
                args.Add(GetObjectOfType(constructorParam.ParameterType));
            }

            instance = (TTarget)Activator.CreateInstance(t, args.ToArray());
        }
    }
}