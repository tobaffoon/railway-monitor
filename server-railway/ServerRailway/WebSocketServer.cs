using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SolverLibrary;
using SolverLibrary.Model;

namespace WpfWebSocketServer
{
    public class WebSocketServer
    {
        private readonly HttpListener _httpListener;

        public event Action<string> OnClientConnected;
        public event Action<string> OnClientDisconnected;
        public event Action<string> OnEmergencyReceived;

        public WebSocketServer(int port)
        {
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add($"http://localhost:{port}/");
        }

        public void Start()
        {
            _httpListener.Start();
            Console.WriteLine("WebSocket server started");

            Task.Run(async () =>
            {
                while (true)
                {
                    var context = await _httpListener.GetContextAsync();
                    if (context.Request.IsWebSocketRequest)
                    {
                        ProcessWebSocketRequest(context);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
            });

            // Выводим сообщение о состоянии клиента при старте сервера
            OnClientDisconnected?.Invoke("Client disconnected");
        }

        public void Stop()
        {
            _httpListener.Stop();
            _httpListener.Close();
            Console.WriteLine("WebSocket server stopped");
        }

        private async void ProcessWebSocketRequest(HttpListenerContext context)
        {
            var webSocketContext = await context.AcceptWebSocketAsync(null);
            var webSocket = webSocketContext.WebSocket;

            OnClientConnected?.Invoke("Client connected");

            // Handle incoming messages from the client
            while (webSocket.State == WebSocketState.Open)
            {
                var message = await ReceiveWebSocketMessage(webSocket);
                if (message != null)
                {
                    Console.WriteLine($"Received message: {message}");

                    if (message.Contains("Emergency situation: Random platform broken"))
                    {
                        OnEmergencyReceived?.Invoke("Emergency situation received, recalculating schedule");
                        var schedule = GenerateSchedule();
                        await SendWebSocketMessage(webSocket, schedule);
                    }
                }
            }


            OnClientDisconnected?.Invoke("Client disconnected");
        }

        private string GenerateSchedule()
        {
            StationWorkPlan plan = GenerateStationWorkPlan(); // Assuming this method exists and returns a StationWorkPlan object
            TrainSchedule trainSchedule = GenerateTrainSchedule(); // Assuming this method exists and returns a TrainSchedule object

            // Update plan with train platforms, traffic lights, switches, etc.
            // Assuming these methods exist and update the plan accordingly
            UpdatePlanWithTrains(plan, trainSchedule);
            UpdatePlanWithTrafficLights(plan);
            UpdatePlanWithSwitches(plan);

            return "Generated Schedule";
        }



        private async Task SendWebSocketMessage(WebSocket webSocket, string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task<string> ReceiveWebSocketMessage(WebSocket webSocket)
        {
            var buffer = new byte[1024];
            try
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.EndOfMessage)
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer, result.Count, buffer.Length - result.Count), CancellationToken.None);
                }
                return Encoding.UTF8.GetString(buffer, 0, result.Count);
            }
            catch (WebSocketException ex)
            {
                // Обработка ошибки
                Console.WriteLine($"WebSocketException: {ex.Message}");
                return null;
            }
        }


    }
}
