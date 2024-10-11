using AutoMapper;
using OilGas.DTO;
using OilGas.Models;

namespace OilGas.AutoMapper
{
    public class SensorProfile : Profile
    {
        public SensorProfile()
        {
            CreateMap<SensorData, SensorDataDTO>();

            CreateMap<SensorDataDTO, SensorData>();
        }
    }
}
