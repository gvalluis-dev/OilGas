using OilGas.Data;
using OilGas.Models;
using Microsoft.AspNetCore.Mvc;
using OilGas.DTO;
using OilGas.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Insere o dado de um sensor, de forma unitária
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("sensor")]
        public async Task<IActionResult> PostSensorData([FromBody] SensorDataDTO data)
        {
            await _sensorDataService.AddSensorData(data);
            return Ok();
        }

        /// <summary>
        /// Insere dados em lote usando um arquivo CSV
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("O Arquivo não foi enviado");

            await _sensorDataService.AddSensorDataCSV(file);
            return Ok("Dados dos sensores importados com sucesso");
        }

        /// <summary>
        /// Calcula a média dos valores dos sensores em um determinado período.
        /// </summary>
        /// <param name="period">O período para o cálculo da média. Valores possíveis: 24h, 48h, 1w (1 semana), 1m (1 mês).</param>
        /// <returns>A média dos valores dos sensores no período especificado.</returns>
        [HttpGet("average")]
        [SwaggerOperation(Summary = "Calcula a média dos valores dos sensores em um determinado período.",
                      Description = "Períodos disponíveis: 24h, 48h, 1w (1 semana), 1m (1 mês).")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSensorAverage([FromQuery] string period)
        {
            // Definir o intervalo de tempo com base no parâmetro 'period'
            DateTime fromDate = DateTime.Now;

            switch (period.ToLower())
            {
                case "24h":
                    fromDate = DateTime.Now.AddHours(-24);
                    break;
                case "48h":
                    fromDate = DateTime.Now.AddHours(-48);
                    break;
                case "1w":
                    fromDate = DateTime.Now.AddDays(-7); // 1 semana
                    break;
                case "1m":
                    fromDate = DateTime.Now.AddMonths(-1); // 1 mês
                    break;
                default:
                    return BadRequest("Período inválido. Use 24h, 48h, 1w, ou 1m.");
            }

            // Buscar os dados dos sensores a partir do intervalo de tempo
            var sensorData = _sensorContext.SensorData
                                     .Where(s => s.TimeStamp >= fromDate)
                                     .ToList();

            if (sensorData.Count == 0)
            {
                return NotFound("Nenhum dado encontrado para o período especificado.");
            }

            // Calcular a média dos valores dos sensores
            var averageValue = sensorData.Average(s => s.Value);

            return Ok(new
            {
                period = period,
                average = averageValue
            });
        }
    }
}
