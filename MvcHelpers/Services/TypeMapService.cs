using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace MvcHelpers.Services
{
    public class TypeMapService : ITypeMapService
    {
        public void AssertConfigurationIsValid()
        {
            Mapper.AssertConfigurationIsValid();
        }

        public IMappingExpression CreateMap(Type source, Type dest)
        {
            return Mapper.CreateMap(source, dest);
        }

        public IMappingExpression<TSource, TDest> CreateMap<TSource, TDest>()
        {
            return Mapper.CreateMap<TSource, TDest>();
        }

        public object Map(object model, Type source, Type dest)
        {
            return Mapper.Map(model, source, dest);
        }

        public void Reset()
        {
            Mapper.Reset();
        }

        public bool TypeMapExists(Type source, Type dest)
        {
            return Mapper.FindTypeMapFor(source, dest) != null;
        }

        public bool TypeMapExists<TSource, TDest>()
        {
            return Mapper.FindTypeMapFor<TSource, TDest>() != null;
        }
    }

    public interface ITypeMapService
    {
        void AssertConfigurationIsValid();
        IMappingExpression CreateMap(Type source, Type dest);
        IMappingExpression<TSource, TDest> CreateMap<TSource, TDest>();
        object Map(object model, Type source, Type dest);
        void Reset();
        bool TypeMapExists(Type source, Type dest);
        bool TypeMapExists<TSource, TDest>();
    }
}
