using AutoMapper;

namespace DsiCode.Micro.Product.API.Model.Dto
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Model.Product>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
