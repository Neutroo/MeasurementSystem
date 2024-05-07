using MeasurementSystem.Server.Services;
using MeasurementSystem.Server.Repositories.DeviceRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Repositories.DeviceInfoRepository;

namespace MeasurementSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDeviceInfoRepository deviceInfoRepository;
        private readonly MonitoringService monitoring;

        public DeviceController(ILogger<DeviceController> logger, IDeviceRepository deviceRepository,
            IDeviceInfoRepository deviceInfoRepository, MonitoringService monitoring)
        {
            this.logger = logger;
            this.deviceRepository = deviceRepository;
            this.deviceInfoRepository = deviceInfoRepository;
            this.monitoring = monitoring;
        }

        /// <summary>
        /// Получить данные по приборам за интервал времени
        /// </summary>
        /// <param name="from">От</param>
        /// <param name="to">До</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<string>> GetAsync(DateTime from, DateTime to)
        {
            if (!Request.QueryString.HasValue)
            {
                return BadRequest("No query string");
            }

            if (from > to)
            {
                return BadRequest("Время начала не может превышать время окончания");
            }

            try
            {
                await Console.Out.WriteLineAsync("get");
                var devices = await deviceRepository.SelectAsync(from, to);
                return Ok(JsonConvert.SerializeObject(devices));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получить данные по приборам за интервал времени в виде json файла
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        [HttpGet("JsonFile")]
        public async Task<IActionResult> GetFileAsync(DateTime from, DateTime to) 
        {
            if (!Request.QueryString.HasValue)
            {
                return BadRequest("No query string");
            }

            if (from > to)
            {
                return BadRequest("Время начала не может превышать время окончания");
            }

            try
            {
                await Console.Out.WriteLineAsync("get as file");
                var fileContents = await deviceRepository.SelectAsBytesAsync(from, to);
                return File(fileContents, "text/plain", "content.json");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавить данные по прибору
        /// </summary>
        /// <param name="json">Json-объект с данными</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(object json)
        {
            if (json == null)
            {
                return BadRequest("Пустой json");
            }

            try
            {
                var data = json.ToString();

                if (string.IsNullOrEmpty(data))
                {
                    return BadRequest("Пустой json");
                }

                Device device = deviceRepository.Insert(data);

                WriteRecordToMonitoringService(device);
            }
            catch (Exception ex)
            {
                monitoring.WriteMessage(new Message()
                {
                    Type = "Error",
                    Content = ex.Message
                });
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        private void WriteRecordToMonitoringService(Device device)
        {
            var infos = deviceInfoRepository.Select();
            var deviceInfo = infos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));

            var record = new Record()
            {
                DeviceName = deviceInfo[device.AKey].Name,
                DeviceSerial = deviceInfo[device.AKey].Serial,
                Date = device.Date,
                Data = device.Data
            };

            var message = new Message()
            {
                Type = "Record",
                Content = record
            };

            monitoring.WriteMessage(message);
        }
    }
}