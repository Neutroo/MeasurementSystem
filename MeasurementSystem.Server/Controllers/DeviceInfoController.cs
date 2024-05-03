using InfluxDB.Client.Core.Exceptions;
using MeasurementSystem.Server.Dto;
using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Repositories.DeviceInfoRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementSystem.Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController, Authorize]
    public class DeviceInfoController : ControllerBase
    {
        private readonly IDeviceInfoRepository deviceInfoRepository;

        public DeviceInfoController(IDeviceInfoRepository deviceInfoRepository)
        {
            this.deviceInfoRepository = deviceInfoRepository;
        }

        /// <summary>
        /// Получить информацию по зарегистрированным приборам
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<DeviceInfo>> Get()
        {
            var infos = deviceInfoRepository.Select();
            return Ok(infos);
        }

        /// <summary>
        /// Зарегистрировать прибор
        /// </summary>
        /// <param name="deviceInfoDto">Информация о приборе</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post(POSTDeviceInfoDto deviceInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var deviceInfo = deviceInfoDto.ToDomain();
                deviceInfoRepository.Insert(deviceInfo);
                deviceInfoRepository.Save();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Изменить информацию о регистрации прибора
        /// </summary>
        /// <param name="id">Id прибора</param>
        /// <param name="deviceInfoDto">Словарь с полями и значениями для обновления</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Put(int id, PUTDeviceInfoDto deviceInfoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                deviceInfoRepository.Update(id, deviceInfoDto.Items);
                deviceInfoRepository.Save();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Удалить прибор
        /// </summary>
        /// <param name="id">Id прибора</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            deviceInfoRepository.Delete(id);
            deviceInfoRepository.Save();
            return Ok();
        }
    }
}
