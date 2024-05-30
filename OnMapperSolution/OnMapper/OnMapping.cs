using OnMapper.Common.Exceptions;
using OnMapper.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OnMapper
{
    public class OnMapping : IMappingService
    {
        //public async Task<Result<TDestination>> Map<TSource, TDestination>(TSource source) where TDestination : new()
        //{
        //    try
        //    {
        //        if (source == null)
        //        {
        //            return await Result<TDestination>.FaildAsync(false, "Source model is Null");
        //        }

        //        var customMapper = CustomMapperRegistry.GetMapper<TSource, TDestination>();
        //        if (customMapper != null)
        //        {
        //            var mappedResult = await customMapper.MapAsync(source);
        //            return await Result<TDestination>.SuccessAsync(mappedResult, "Model mapped Successfully", true);
        //        }

        //        TDestination destination = new TDestination();
        //        PropertyInfo[] sourceProperties = typeof(TSource).GetProperties();
        //        PropertyInfo[] destinationProperties = typeof(TDestination).GetProperties();

        //        foreach (PropertyInfo sourceProperty in sourceProperties)
        //        {
        //            PropertyInfo destinationProperty = Array.Find(destinationProperties, p => p.Name == sourceProperty.Name);

        //            if (destinationProperty != null && destinationProperty.CanWrite)
        //            {
        //                // Handle collections
        //                if (typeof(IEnumerable).IsAssignableFrom(destinationProperty.PropertyType) && destinationProperty.PropertyType != typeof(string))
        //                {
        //                    var sourceCollection = sourceProperty.GetValue(source) as IEnumerable;
        //                    if (sourceCollection != null)
        //                    {
        //                        var destinationCollectionType = destinationProperty.PropertyType.GetGenericArguments()[0];
        //                        var listType = typeof(List<>).MakeGenericType(destinationCollectionType);
        //                        var destinationList = (IList)Activator.CreateInstance(listType);

        //                        foreach (var sourceItem in sourceCollection)
        //                        {
        //                            var mapMethod = typeof(OnMapping).GetMethod(nameof(Map)).MakeGenericMethod(sourceItem.GetType(), destinationCollectionType);
        //                            var mapResultTask = (Task<Result<object>>)mapMethod.Invoke(this, new object[] { sourceItem });
        //                            var mapResult = await mapResultTask;

        //                            if (!mapResult.IsSuccess)
        //                            {
        //                                return await Result<TDestination>.FaildAsync(false, $"Mapping failed for collection item: {mapResult.Message}");
        //                            }

        //                            destinationList.Add(mapResult.Data);
        //                        }

        //                        destinationProperty.SetValue(destination, destinationList);
        //                    }
        //                }
        //                else
        //                {
        //                    // Handle nested objects
        //                    if (destinationProperty.PropertyType.IsClass && destinationProperty.PropertyType != typeof(string))
        //                    {
        //                        var mapMethod = typeof(OnMapping).GetMethod(nameof(Map)).MakeGenericMethod(sourceProperty.PropertyType, destinationProperty.PropertyType);
        //                        var mapResultTask = (Task<Result<object>>)mapMethod.Invoke(this, new object[] { sourceProperty.GetValue(source) });
        //                        var mapResult = await mapResultTask;

        //                        if (!mapResult.IsSuccess)
        //                        {
        //                            return await Result<TDestination>.FaildAsync(false, $"Mapping failed for nested object: {mapResult.Message}");
        //                        }

        //                        destinationProperty.SetValue(destination, mapResult.Data);
        //                    }
        //                    else
        //                    {
        //                        // Handle simple properties
        //                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
        //                    }
        //                }
        //            }
        //        }

        //        return await Result<TDestination>.SuccessAsync(destination, "Model mapped Successfully", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Result<TDestination>.FaildAsync(false, $"Mapping failed: {ex.Message}");
        //    }
        //}


        //public async Task<Result<TDestination>> ConvertByPropertyNameOrDisplayName<TSource, TDestination>(TSource source) where TDestination : new()
        //{
        //    try
        //    {
        //        var destination = Activator.CreateInstance<TDestination>();
        //        var sourceType = typeof(TSource);
        //        var destinationType = typeof(TDestination);

        //        foreach (var sourceProperty in sourceType.GetProperties())
        //        {
        //            PropertyInfo destinationProperty = GetSimilarProperty(sourceProperty, destinationType);

        //            if (destinationProperty != null && destinationProperty.CanWrite)
        //            {
        //                try
        //                {
        //                    var sourceValue = sourceProperty.GetValue(source);
        //                    var destinationValue = MapPropertyValue(sourceValue, destinationProperty.PropertyType);
        //                    destinationProperty.SetValue(destination, destinationValue);
        //                }
        //                catch
        //                {
        //                    return await Result<TDestination>.FaildAsync(false, $"{sourceProperty.Name} + Can not convert to ");
        //                }
        //            }
        //        }

        //        return await Result<TDestination>.SuccessAsync(destination, "Mapped Successfully", true);
        //    }
        //    catch (Exception ex)
        //    {
        //        return await Result<TDestination>.FaildAsync(false, $"{ex.Message} + Can not convert to ");
        //    }
        //}

        //private PropertyInfo GetSimilarProperty(PropertyInfo sourceProperty, Type destinationType)
        //{
        //    PropertyInfo destinationProperty;

        //    var destinationProperties = destinationType.GetProperties();
        //    var hasDestinationCustomAttributes = destinationProperties.Where(x => x.GetCustomAttributes().Any()).ToList();
        //    var hasSourceCustomAttributes = sourceProperty.GetCustomAttributes().Any();

        //    if (hasDestinationCustomAttributes.Count() > 0 && hasSourceCustomAttributes)
        //    {
        //        var matchingProperties = hasDestinationCustomAttributes.Where(x =>
        //            x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == sourceProperty.Name ||
        //            sourceProperty.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == x.Name ||
        //            x.Name == sourceProperty.Name ||
        //            x.GetCustomAttributes<DisplayNameAttribute>().Any(attr =>
        //                attr.DisplayName == sourceProperty.Name)).ToList();

        //        destinationProperty = matchingProperties.FirstOrDefault() ??
        //                              destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
        //    }
        //    else if (hasDestinationCustomAttributes.Count() > 0 && !hasSourceCustomAttributes)
        //    {
        //        var matchingProperties = hasDestinationCustomAttributes.Where(x =>
        //            x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == sourceProperty.Name ||
        //            x.Name == sourceProperty.Name ||
        //            x.GetCustomAttributes<DisplayNameAttribute>().Any(attr =>
        //                attr.DisplayName == sourceProperty.Name)).ToList();

        //        destinationProperty = matchingProperties.FirstOrDefault() ??
        //                              destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
        //    }
        //    else if (hasDestinationCustomAttributes.Count() <= 0 && hasSourceCustomAttributes)
        //    {
        //        var matchingProperties = destinationProperties.Where(x =>
        //            x.Name == sourceProperty.Name ||
        //            x.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName == sourceProperty.Name).ToList();

        //        destinationProperty = matchingProperties.FirstOrDefault() ??
        //                              destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
        //    }
        //    else
        //    {
        //        destinationProperty = destinationProperties.FirstOrDefault(x => x.Name == sourceProperty.Name);
        //    }

        //    return destinationProperty;
        //}

        //private object MapPropertyValue(object sourceValue, Type destinationType)
        //{
        //    if (sourceValue == null || destinationType.IsAssignableFrom(sourceValue.GetType()))
        //        return sourceValue;

        //    if (destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(List<>))
        //    {
        //        var destinationList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(destinationType.GetGenericArguments()));
        //        var sourceList = (IEnumerable)sourceValue;
        //        foreach (var item in sourceList)
        //        {
        //            var mappedItem = MapPropertyValue(item, destinationType.GetGenericArguments()[0]);
        //            destinationList.Add(mappedItem);
        //        }
        //        return destinationList;
        //    }

        //    if (destinationType.IsInterface && destinationType.IsGenericType && destinationType.GetGenericTypeDefinition() == typeof(ICollection<>))
        //    {
        //        var listType = typeof(List<>).MakeGenericType(destinationType.GetGenericArguments());
        //        var destinationCollection = (IList)Activator.CreateInstance(listType);
        //        var sourceCollection = (IEnumerable)sourceValue;
        //        foreach (var item in sourceCollection)
        //        {
        //            var mappedItem = MapPropertyValue(item, destinationType.GetGenericArguments()[0]);
        //            destinationCollection.Add(mappedItem);
        //        }
        //        return destinationCollection;
        //    }

        //    dynamic destination = ConvertValueType(sourceValue, destinationType);
        //    var sourceProperties = sourceValue.GetType().GetProperties();
        //    foreach (var sourceProperty in sourceProperties)
        //    {
        //        PropertyInfo destinationProperty = GetSimilarProperty(sourceProperty, destination.GetType());
        //        if (destinationProperty != null && destinationProperty.CanWrite)
        //        {
        //            var sourcePropertyValue = sourceProperty.GetValue(sourceValue);
        //            var destinationPropertyValue = MapPropertyValue(sourcePropertyValue, destinationProperty.PropertyType);
        //            var convertValue = System.Convert.ChangeType(destinationPropertyValue, destinationProperty.PropertyType);
        //            destinationProperty.SetValue(destination, convertValue);
        //        }
        //    }
        //    return destination;
        //}

        //public async Task<Result<List<TDestination>>> ConvertList<TSource, TDestination>(IEnumerable<TSource> source) where TDestination : new()
        //{
        //    var res = new List<TDestination>();
        //    foreach (var item in source)
        //    {
        //        var mappedResult = await ConvertByPropertyNameOrDisplayName<TSource, TDestination>(item);
        //        if (mappedResult.IsSuccess)
        //        {
        //            res.Add(mappedResult.Data);
        //        }
        //        else
        //        {
        //            return await Result<List<TDestination>>.FaildAsync(false, mappedResult.Message);
        //        }
        //    }
        //    return await Result<List<TDestination>>.SuccessAsync(res, "Collection Mapped Successfully", true);
        //}

        //public dynamic ConvertValueType(object sourceValue, Type destinationType)
        //{
        //    dynamic destination;

        //    if (destinationType.Namespace == "System")
        //    {
        //        if (sourceValue is TimeSpan && destinationType != typeof(string))
        //        {
        //            destination = ((TimeSpan)sourceValue).Ticks;
        //        }
        //        else if (sourceValue is DateTime && destinationType != typeof(string))
        //        {
        //            destination = ((DateTime)sourceValue).Ticks;
        //        }
        //        else if ((sourceValue is DateTime || sourceValue is TimeSpan) && destinationType == typeof(string))
        //        {
        //            destination = System.Convert.ToString(sourceValue);
        //        }
        //        else if (destinationType == typeof(TimeSpan))
        //        {
        //            destination = TimeSpan.FromTicks(System.Convert.ToInt64(sourceValue));
        //        }
        //        else if (destinationType == typeof(DateTime))
        //        {
        //            DateTime date = new DateTime(System.Convert.ToInt64(sourceValue));
        //            destination = date;
        //        }
        //        else
        //        {
        //            var convertedValue = System.Convert.ChangeType(sourceValue, destinationType);
        //            destination = convertedValue;
        //        }
        //    }
        //    else
        //    {
        //        destination = Activator.CreateInstance(destinationType);
        //    }

        //    return destination;
        //}

        public async Task<Result<TDestination>> Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            try
            {
                if (source == null)
                {
                    return await Result<TDestination>.FaildAsync(false, "Source model is Null");
                }

                TDestination destination = new TDestination();
                InitializeCollections(destination);
                await MapProperties(source, destination);

                return await Result<TDestination>.SuccessAsync(destination, "Model mapped Successfully", true);
            }
            catch (Exception ex)
            {
                return await Result<TDestination>.FaildAsync(false, $"Mapping failed: {ex.Message}");
            }
        }

        private async Task MapProperties(object source, object destination)
        {
            PropertyInfo[] sourceProperties = source.GetType().GetProperties();
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();

            foreach (PropertyInfo sourceProperty in sourceProperties)
            {
                PropertyInfo destinationProperty = Array.Find(destinationProperties, p => p.Name == sourceProperty.Name);

                if (destinationProperty != null && destinationProperty.CanWrite)
                {
                    if (typeof(IEnumerable).IsAssignableFrom(destinationProperty.PropertyType) && destinationProperty.PropertyType != typeof(string))
                    {
                        var sourceCollection = (IEnumerable)sourceProperty.GetValue(source);
                        if (sourceCollection != null)
                        {
                            var destinationCollectionType = destinationProperty.PropertyType.GetGenericArguments()[0];
                            var destinationCollection = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(destinationCollectionType));

                            foreach (var sourceItem in sourceCollection)
                            {
                                var destinationItem = Activator.CreateInstance(destinationCollectionType);
                                await MapProperties(sourceItem, destinationItem);
                                destinationCollection.Add(destinationItem);
                            }

                            destinationProperty.SetValue(destination, destinationCollection);
                        }
                    }
                    else
                    {
                        destinationProperty.SetValue(destination, sourceProperty.GetValue(source));
                    }
                }
            }
        }

        private void InitializeCollections(object destination)
        {
            PropertyInfo[] destinationProperties = destination.GetType().GetProperties();

            foreach (PropertyInfo destinationProperty in destinationProperties)
            {
                if (typeof(IEnumerable).IsAssignableFrom(destinationProperty.PropertyType) && destinationProperty.PropertyType != typeof(string))
                {
                    var destinationCollectionType = destinationProperty.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)
                        ? typeof(List<>).MakeGenericType(destinationProperty.PropertyType.GetGenericArguments()[0])
                        : destinationProperty.PropertyType;
                    var destinationCollection = Activator.CreateInstance(destinationCollectionType);
                    destinationProperty.SetValue(destination, destinationCollection);
                }
            }
        }

        public async Task<Result<List<TDestination>>> MapCollection<TSource, TDestination>(IEnumerable<TSource> sourceCollection) where TDestination : new()
        {

            var res = new List<TDestination>();
            foreach (var item in sourceCollection)
            {
                var mappedResult = await Map<TSource, TDestination>(item);
                if (mappedResult.IsSuccess)
                {
                    res.Add(mappedResult.Data);
                }
                else
                {
                    return await Result<List<TDestination>>.FaildAsync(false, mappedResult.Message);
                }
            }
            return await Result<List<TDestination>>.SuccessAsync(res, "Collection Mapped Successfully", true);
        }

    }
}
