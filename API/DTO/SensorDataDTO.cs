using System.ComponentModel.DataAnnotations;

namespace OilGas.DTO
{
    /// <summary>
    /// Atributo para inserir unidade de leitura do sensor no banco
    /// </summary>
    public class SensorDataDTO
    {

        /// <summary>
        /// ID do equipamento
        /// </summary>
        [Required]
        public string EquipmentId { get; set; }
        /// <summary>
        /// Momento da leitura com o devido fuso horário
        /// </summary>
        [Required]
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Valor da leitura
        /// </summary>
        [Required]
        public decimal Value { get; set; }
    }
}
