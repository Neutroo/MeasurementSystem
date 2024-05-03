using MeasurementSystem.Server.Services;
using MeasurementSystem.Server.Repositories.DeviceRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MeasurementSystem.Server.Models;

namespace MeasurementSystem.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IDeviceRepository deviceRepository;
        private readonly MonitoringService monitoring;

        public DeviceController(ILogger<DeviceController> logger, IDeviceRepository deviceRepository, 
            MonitoringService monitoring)
        {
            this.logger = logger;
            this.deviceRepository = deviceRepository;
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

        [HttpPost("mon")]
        public async void Postt()
        {
            await monitoring.WriteMessageAsync(null);
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
                await monitoring.WriteMessageAsync(device);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}