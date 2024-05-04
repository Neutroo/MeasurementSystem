using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementSystem.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly MonitoringService monitoring;

        public MonitoringController(MonitoringService monitoring)
        {
            this.monitoring = monitoring;
        }

        [HttpGet]
        public ActionResult<Device> Get()
        {
            var device = monitoring.GetMessage();
            return device;
        }
    }
}
