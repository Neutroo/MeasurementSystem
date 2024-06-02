using InfluxDB.Client.Core.Exceptions;
using MeasurementSystem.Server.Dto;
using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Repositories.CalibrationItemRepository;
using MeasurementSystem.Server.Repositories.DeviceInfoRepository;
using MeasurementSystem.Server.Repositories.DeviceRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MeasurementSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpGet, Authorize]
        public ActionResult<Dictionary<string, Dictionary<string, IEnumerable<object>>>> Get()
        {
            var items = calibrationItemRepository.Select();
            var deviceInfos = deviceInfoRepository.Select();
            var deviceInfo = deviceInfos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));

            var result = items.GroupBy(i => i.AuthKey)
                .ToDictionary(
                    g => $"{deviceInfo[g.Key].Name}({deviceInfo[g.Key].Serial})",
                    g => g.GroupBy(r => r.Sensor)
                        .ToDictionary(
                            sg => sg.Key,
                            sg => sg.Select(x => new
                            {
                                CreationDate = x.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                x.Coefficients
                            }).OrderByDescending(x => x.CreationDate).AsEnumerable<object>()
                        )
                );

            return Ok(result);
        }

        [HttpGet("LastItems"), Authorize]
        public ActionResult<Dictionary<string, IEnumerable<GETCalibrationItemDto>>> GetLast()
        {
            var items = calibrationItemRepository.Select();
            var deviceInfos = deviceInfoRepository.Select();
            var deviceInfo = deviceInfos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));
            var grouped = items.GroupBy(i => i.AuthKey);
            var result = new Dictionary<string, IEnumerable<GETCalibrationItemDto>>();

            foreach (var group in grouped)
            {
                var records = new List<GETCalibrationItemDto>();
                var latestRecords = group.GroupBy(i => i.Sensor).Select(s => s.OrderByDescending(i => i.CreationDate).First());

                foreach (var item in latestRecords)
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

        [HttpGet("Table")]
        public ActionResult<IEnumerable<CalibrationTableItem>> GetCalibrationTable()
        {
            var items = calibrationItemRepository.Select();
            var deviceInfos = deviceInfoRepository.Select();
            var deviceInfo = deviceInfos.ToDictionary(i => i.AuthKey, i => (i.Name, i.Serial));

            var result = items.GroupBy(i => i.AuthKey)
                .Select(g => new CalibrationTableItem
                {
                    DeviceName = deviceInfo[g.Key].Name,
                    DeviceSerial = deviceInfo[g.Key].Serial,
                    Sensors = g.GroupBy(r => r.Sensor)
                        .Select(sg => new Sensor
                        {
                            Name = sg.Key,
                            Calibrations = sg.OrderByDescending(x => x.CreationDate)
                                .Select(x => new Calibration
                                {
                                    CreationDate = x.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    Data = new
                                    {
                                        n = x.Coefficients.Length - 1,
                                        ai = x.Coefficients
                                    }
                                })
                        })
                });

            return Ok(JsonConvert.SerializeObject(result));
        }

        [HttpGet("SensorsByDevice"), Authorize]
        public async Task<ActionResult<Dictionary<string, IEnumerable<string>>>> GetSensorsByDevice()
        {
            var sensorsByDevice = await deviceRepository.SelectSensorsByDeviceAsync();
            return Ok(sensorsByDevice);
        }

        [HttpGet("DevicesBySensor"), Authorize]
        public async Task<ActionResult<Dictionary<string, IEnumerable<string>>>> GetDevicesBySensor()
        {
            var devicesBySensors = await deviceRepository.SelectDevicesBySensorAsync();
            return Ok(devicesBySensors);
        }

        [HttpPost, Authorize]
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
