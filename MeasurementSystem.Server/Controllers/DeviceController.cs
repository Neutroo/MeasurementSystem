using MeasurementSystemWebAPI.Repositories.DeviceInfoRepository;
using MeasurementSystemWebAPI.Repositories.DeviceRepository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeasurementSystemWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly ILogger<DeviceController> logger;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDeviceInfoRepository deviceInfoRepository;

        public DeviceController(ILogger<DeviceController> logger, IDeviceRepository deviceRepository,
            IDeviceInfoRepository deviceInfoRepository)
        {
            this.logger = logger;
            this.deviceRepository = deviceRepository;           
            this.deviceInfoRepository = deviceInfoRepository;
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
        public IActionResult Post(object json)
        {
            if (json == null)
            {
                return BadRequest("Пустой json");
            }

            try
            {
                var device = json.ToString();

                if (string.IsNullOrEmpty(device))
                {
                    return BadRequest("Пустой json");
                }

                deviceRepository.Insert(device);
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