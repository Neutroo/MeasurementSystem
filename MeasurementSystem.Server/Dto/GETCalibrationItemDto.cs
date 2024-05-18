namespace MeasurementSystem.Server.Dto
{
    public class GETCalibrationItemDto
    {
        /// <summary>
        /// Датчик
        /// </summary>
        public required string Sensor { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public required string CreationDate { get; set; }

        /// <summary>
        /// Коэффициенты
        /// </summary>
        public required double[] Coefficients { get; set; }
    }
}
