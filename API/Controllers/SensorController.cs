using OilGas.Data;
using OilGas.Models;
using Microsoft.AspNetCore.Mvc;
using OilGas.DTO;
using OilGas.Services;

namespace OilGas.Controllers
{
    public class SensorController : ControllerBase
    {
        private readonly SensorContext _sensorContext;
        private readonly SensorDataService _sensorDataService;
        public SensorController(SensorContext sensorContext, SensorDataService sensorDataService)
        {
            _sensorContext = sensorContext;
            _sensorDataService = sensorDataService;
        }

        [HttpPost("sensor")]
        public async Task<IActionResult> PostSensorData([FromBody] SensorDataDTO data)
        {
            await _sensorDataService.AddSensorData(data);
            return Ok();
        }

        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("O Arquivo não foi enviado");

            await _sensorDataService.AddSensorDataCSV(file);
            return Ok("Dados dos sensores importados com sucesso");
        }
    }
}
