
using System.ComponentModel.DataAnnotations;

namespace OilGas.Models
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Unique identifier of the equipment that sent the data.
        /// </summary>
        public string EquipmentId { get; set; }
        /// <summary>
        /// Date and time (including time zone) when the value was collected.
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// The sensor value, represented in two decimal places.
        /// </summary>
        public decimal Value { get; set; }
    }
}
