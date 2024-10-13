using System.ComponentModel.DataAnnotations;

namespace OilGas.DTO
{
    /// <summary>
    /// Attribute to insert sensor reading unit into the Database
    /// </summary>
    public class SensorDataDTO
    {

        /// <summary>
        /// Equipment ID
        /// </summary>
        [Required]
        public string EquipmentId { get; set; }
        /// <summary>
        /// Reading time with the appropriate time zone
        /// </summary>
        [Required]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Value of reading
        /// </summary>
        [Required]
        public decimal Value { get; set; }
    }
}
