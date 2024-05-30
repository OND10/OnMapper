using OnMapper.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnMapper.Services.Interfaces
{
    public interface IMappingService
    {
        public Task<Result<TDestination>> Map<TSource, TDestination>(TSource source) where TDestination : new();
        public Task<Result<List<TDestination>>> MapCollection<TSource, TDestination>(IEnumerable<TSource> sourceCollection) where TDestination : new();
    }
}
