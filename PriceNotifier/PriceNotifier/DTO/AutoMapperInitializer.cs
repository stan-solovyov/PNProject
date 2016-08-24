using AutoMapper;

namespace PriceNotifier.DTO
{
    public class AutoMapperInitializer
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<MyMappingProfile>();
            });
        }
    }
}