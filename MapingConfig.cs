using AutoMapper;
using DsiCode.Micro.Product.API.Model.Dto;

namespace DsiCode.Micro.Product.API
{
    public class MapingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, DsiCode.Micro.Product.API.Model.Product>()
                .ReverseMap();
            });
            return mappingConfig;
        }
    }
}
