using MeasurementSystem.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeasurementSystem.Server.Controllers
{
    [Route("websocket/[controller]")]
    [ApiController]
    public class MonitoringController : ControllerBase
    {
        private readonly MonitoringService monitoring;

        public MonitoringController(MonitoringService monitoring)
        {
            this.monitoring = monitoring;
        }

        [HttpGet]
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await monitoring.HandleWebSocketConnection(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
                await Response.WriteAsync("Expected a WebSocket request");
            }
        }
    }
}
