namespace MeasurementSystem.Server.Models
{
    public class CalibrationItem
    {
        /// <summary>
        /// Id калибровочной записи
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Ключ аутентификации
        /// </summary>
        public required string AuthKey { get; set; }

        /// <summary>
        /// Датчик
        /// </summary>
        public required string Sensor { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public required DateTime CreationDate { get; set; }

        /// <summary>
        /// Коэффициенты
        /// </summary>
        public required double[] Coefficients { get; set; }
    }
}
