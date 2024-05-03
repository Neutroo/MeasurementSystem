﻿using MeasurementSystem.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace MeasurementSystem.Server.Dto
{
    public class POSTDeviceInfoDto
    {
        /// <summary>
        /// Имя
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Серийний номер
        /// </summary>
        public string? Serial { get; set; }

        /// <summary>
        /// Ключ аутентификации
        /// </summary>
        [Required]
        public string AuthKey { get; set; }

        /// <summary>
        /// Координата X
        /// </summary>
        [Required]
        public double X { get; set; }

        /// <summary>
        /// Координата Y
        /// </summary>
        [Required]
        public double Y { get; set; }

        /// <summary>
        /// Расположение прибора
        /// </summary>
        [Required]
        public string Location { get; set; }

        /// <summary>
        /// Удален ли прибор
        /// </summary>
        [Required]
        public bool IsDeleted { get; set; }

        public DeviceInfo ToDomain()
            => new()
            {
                Name = Name,
                Serial = Serial,
                AuthKey = AuthKey,
                X = X,
                Y = Y,
                Location = Location
            };
    }
}
