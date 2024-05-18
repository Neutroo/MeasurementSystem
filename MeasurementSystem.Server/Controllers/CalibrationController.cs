using InfluxDB.Client.Core.Exceptions;
using MeasurementSystem.Server.Dto;
using MeasurementSystem.Server.Repositories.CalibrationItemRepository;
using MeasurementSystem.Server.Repositories.DeviceInfoRepository;
using MeasurementSystem.Server.Repositories.DeviceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeasurementSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class CalibrationController : ControllerBase
    {
        private readonly ICalibrationItemRepository calibrationItemRepository;
        private readonly IDeviceRepository deviceRepository;
        private readonly IDeviceInfoRepository deviceInfoRepository;

        public CalibrationController(ICalibrationItemRepository calibrationItemRepository,
            IDeviceRepository deviceRepository,
            IDeviceInfoRepository deviceInfoRepository)
        {
            this.calibrationItemRepository = calibrationItemRepository;
            this.deviceRepository = deviceRepository;
            this.deviceInfoRepository = deviceInfoRepository;
        }

        [HttpGet("Fields")]
        public async Task<ActionResult<Dictionary<string, IEnumerable<string>>>> GetFields()
        {
            var deviceFields = await deviceRepository.SelectDeviceFieldsAsync();
            return Ok(deviceFields);
        }

        [HttpGet]
        public ActionResult<Dictionary<string, IEnumerable<GETCalibrationItemDto>>> Get()
        {
            var items = calibrationItemRepository.Select();
            var deviceInfos = deviceInfoRepository.Select();
            var deviceInfo = deviceInfos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));
            var grouped = items.GroupBy(i => i.AuthKey);

            var result = new Dictionary<string, IEnumerable<GETCalibrationItemDto>>();

            foreach (var group in grouped)
            {
                var records = new List<GETCalibrationItemDto>();
                foreach (var item in group)
                {
                    records.Add(new GETCalibrationItemDto()
                    {
                        Sensor = item.Sensor,
                        CreationDate = item.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        Coefficients = item.Coefficients
                    });
                }
                result.TryAdd($"{deviceInfo[group.Key].Name}({deviceInfo[group.Key].Serial})", records);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult Post(POSTCalibrationItemDto calibrationItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var deviceInfos = deviceInfoRepository.Select();
                var authKey = deviceInfos.FirstOrDefault(i => $"{i.Name}({i.Serial})" == calibrationItemDto.NameSerial)?.AuthKey
                    ?? throw new NotFoundException($"Невалидное имя прибора: {calibrationItemDto.NameSerial}");

                var calibrationItem = calibrationItemDto.ToDomain(authKey);

                Console.WriteLine(JsonConvert.SerializeObject(calibrationItem));

                calibrationItemRepository.Insert(calibrationItem);
                calibrationItemRepository.Save();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }
    }
}
