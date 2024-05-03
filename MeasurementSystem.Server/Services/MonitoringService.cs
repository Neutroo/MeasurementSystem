using MeasurementSystem.Server.Models;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace MeasurementSystem.Server.Services
{
    public class MonitoringService
    {
        private readonly List<WebSocket> sockets;

        public MonitoringService()
        {
            sockets = [];
        }

        public async Task HandleWebSocketConnection(WebSocket socket)
        {
            sockets.Add(socket);
            var buffer = new byte[1024 * 2];
            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), default);
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, default);
                    break;
                }

                foreach (var s in sockets)
                {
                    await s.SendAsync(buffer[..result.Count], WebSocketMessageType.Text, true, default);
                }
            }
            sockets.Remove(socket);
        }

        public async Task WriteMessageAsync(Device device)
        {
            var json = JsonConvert.SerializeObject(device);
            var data = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(data);

            foreach (var socket in sockets)
            {
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
