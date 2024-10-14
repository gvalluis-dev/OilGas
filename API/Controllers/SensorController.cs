using OilGas.Data;
using OilGas.Models;
using Microsoft.AspNetCore.Mvc;
using OilGas.DTO;
using OilGas.Services;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace OilGas.Controllers
{
    /// <summary>
    /// Controller regarding the handling of data from Oil and Gas plant sensors
    /// </summary>
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
        /// Inserts data from a sensor, in a unitary form
        /// </summary>
        ///   /// <remarks>
        /// Payload Example:
        /// 
        ///     POST /api/sensor
        ///     {
        ///        "equipmentId": "EQ-12495",
        ///        "timestamp": "2023-02-15T01:30:00.000-05:00",
        ///        "value": 78.42
        ///     }
        /// </remarks>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("sensor")]
        public async Task<IActionResult> PostSensorData([FromBody] SensorDataDTO data)
        {
            await _sensorDataService.AddSensorData(data);
            return Ok();
        }

        /// <summary>
        /// Batch insert data using a CSV file
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [SwaggerOperation(Summary = "Upload a CSV file with missing sensor data",
        Description = "This endpoint allows you to send a CSV file containing lost sensor data to be stored in the database.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("upload-csv")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("The file was not sent");

            await _sensorDataService.AddSensorDataCSV(file);
            return Ok("Sensor data imported successfully");
        }

        /// <summary>
        /// Calculates the average of sensor values ​​over a given period.
        /// </summary>
        /// <param name="period">The period for calculating the average. Possible values: 24h, 48h, 1w (1 week), 1m (1 month).</param>
        /// <returns>The average of the sensor values ​​over the specified period.</returns>
        [HttpGet("average")]
        [SwaggerOperation(Summary = "Calculates the average of sensor values ​​over a given period.",
                      Description = "Available periods: 24h, 48h, 1w (1 week), 1m (1 month).")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSensorAverage([FromQuery] string period)
        {
            // Set time interval based on 'period' parameter
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
                    return BadRequest("Invalid period. Use 24h, 48h, 1w, or 1m.");
            }

            // Fetch sensor data from time range
            var sensorData = _sensorContext.SensorData
                                     .Where(s => s.TimeStamp >= fromDate)
                                     .ToList();



            if (sensorData.Count == 0)
            {
                return NotFound("No data found for the specified period.");
            }

            // Calculate the average of sensor values
            var averageValues = sensorData
            .GroupBy(s => s.EquipmentId)
            .Select(g => new SensorAverageDTO
            {
                EquipmentId = g.Key,
                AverageValue = g.Average(s => s.Value)
            })
            .ToList();


            return Ok(averageValues);
        }
    }
}
