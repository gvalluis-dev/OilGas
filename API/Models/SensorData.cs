
using System.ComponentModel.DataAnnotations;

namespace OilGas.Models
{
    public class SensorData
    {
        [Key]
        public int Id { get; set; }
        public string EquipmentId { get; set; }
        public DateTime TimeStamp { get; set; }
        public decimal Value { get; set; }
    }
}
