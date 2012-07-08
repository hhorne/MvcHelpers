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
        public void CreateMap(Type source, Type dest)
        {
            Mapper.CreateMap(source, dest);
        }

        public void CreateMap<TSource, TDest>()
        {
            Mapper.CreateMap<TSource, TDest>();
        }

        public bool TypeMapExists(Type source, Type dest)
        {
            return Mapper.FindTypeMapFor(source, dest) != null;
        }

        public bool TypeMapExists<TSource, TDest>()
        {
            return Mapper.FindTypeMapFor<TSource, TDest>() != null;
        }

        public void Reset()
        {
            Mapper.Reset();
        }
    }

    public interface ITypeMapService
    {
        void CreateMap(Type source, Type dest);
        void CreateMap<TSource, TDest>();
        bool TypeMapExists(Type source, Type dest);
        bool TypeMapExists<TSource, TDest>();
        void Reset();
    }
}
