using MeasurementSystem.Server.Models;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace MeasurementSystem.Server.Services
{
    public class MonitoringService
    {
        //private readonly List<WebSocket> sockets;
        private readonly ConcurrentQueue<Device> devices;


        public MonitoringService()
        {
            //sockets = [];
            devices = [];
        }

        /*public async Task HandleWebSocketConnection(WebSocket socket)
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
        }*/

        public void WriteMessage(Device device)
        {
            devices.Enqueue(device);
            /*var json = JsonConvert.SerializeObject(device);
            var data = Encoding.UTF8.GetBytes(json);
            var buffer = new ArraySegment<byte>(data);

            foreach (var socket in sockets)
            {
                await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
            }*/
        }

        public Device GetMessage()
        {
            devices.TryDequeue(out Device device);
            return device;
        }
    }
}
