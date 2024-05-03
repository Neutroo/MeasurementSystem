﻿using MeasurementSystem.Server.Services;
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
        public async Task Connect()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                await Console.Out.WriteLineAsync("connected");
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
