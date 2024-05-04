using MeasurementSystem.Server.Models;
using MeasurementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
        public ActionResult<IEnumerable<Message>> Get()
        {
            var messages = monitoring.GetMessages();
            return messages.IsNullOrEmpty() ? NoContent() : Ok(messages);
        }
    }
}
