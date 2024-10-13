using AutoMapper;
using OilGas.Data;
using OilGas.DTO;
using OilGas.Models;
using System.Reflection.PortableExecutable;

namespace OilGas.Services
{
    public class SensorDataService
    {
        private readonly SensorContext _context;
        private readonly IMapper _mapper;

        public SensorDataService(SensorContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddSensorData(SensorDataDTO sensorDataDTO)
        {
            //Converts the DTO into an entity
            var sensorData = new SensorData
            {
                EquipmentId = sensorDataDTO.EquipmentId,
                Value = sensorDataDTO.Value,
                TimeStamp = sensorDataDTO.TimeStamp
            };

            //Inserts entity into the database
            _context.SensorData.Add(sensorData);
            await _context.SaveChangesAsync();
            
        }

        public async Task AddSensorDataCSV(IFormFile file)
        {
            using (var stream = new StreamReader(file.OpenReadStream()))
            {
                //Skip the first line - header
                stream.ReadLine();

                while (!stream.EndOfStream)
                {
                    
                    var line = await stream.ReadLineAsync();
                    var values = line.Split(';');
                    

                    var sensorData = new SensorData
                    {
                        EquipmentId = values[0],
                        TimeStamp = DateTime.Parse(values[1]),
                        Value = decimal.Parse(values[2])
                    };

                    _context.SensorData.Add(sensorData);
                }
                await _context.SaveChangesAsync();
            }
        }
    }
}
