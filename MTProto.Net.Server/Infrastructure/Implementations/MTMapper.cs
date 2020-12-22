using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MTProto.NET.Server.Infrastructure.Implementations
{
	class MTMapper : IMTMapper
	{
        public static MTMapper Instance = new MTMapper();
        private MapperConfigurationExpression mappings = new MapperConfigurationExpression() { CreateMissingTypeMaps = true };
        private MapperConfiguration configuration;
        private IMapper mapper;
        public MTMapper()
        {
            Clear();
        }
        private IMapper GetMapper(bool refresh = false)
        {
            if (mapper == null || refresh)
                mapper = new Mapper(new MapperConfiguration(mappings));
            return mapper;
        }
        public IMappingExpression<T1, T2> CreateMap<T1, T2>()
        {
            var result = mappings.CreateMap<T1, T2>();

            mapper = null;
            return result;
        }
        public T2 Map<T1, T2>(T1 value)
        {
            return GetMapper().Map<T1, T2>(value);
        }

        public void Clear()
        {
            this.mappings = new MapperConfigurationExpression
            {
                 CreateMissingTypeMaps = true,
            };
            
            this.configuration = new MapperConfiguration(this.mappings);
            
            this.mapper = null;
        }
    }
  
}
