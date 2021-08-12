using System;

namespace Sentinel.Core.Helpers
{
    public static class MappingHelper
    {
        public static TEntity MapOnToEntity<TEntity, TViewModel>(TEntity entity, TViewModel viewModel)
        {
            var entityType = typeof(TEntity);
            var modelType = typeof(TViewModel);

            foreach (var entityProperty in entityType.GetProperties())
            {
                var modelProperty = modelType.GetProperty(entityProperty.Name);
                if (modelProperty != null && (modelProperty.PropertyType == entityProperty.PropertyType || modelProperty.PropertyType == Nullable.GetUnderlyingType(entityProperty.PropertyType)))
                {
                    entityProperty.SetValue(entity, modelProperty.GetValue(viewModel));
                }
            }

            return entity;
        }
    }
}