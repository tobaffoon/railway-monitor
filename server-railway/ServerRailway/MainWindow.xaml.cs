using System.Windows;

namespace WpfWebSocketServer
{
    public partial class MainWindow : Window
    {
        private WebSocketServer _webSocketServer;

        public MainWindow()
        {
            InitializeComponent();

            _webSocketServer = new WebSocketServer(8080);
            _webSocketServer.OnClientConnected += message => Dispatcher.Invoke(() => StatusLabel.Content = message);
            _webSocketServer.OnClientDisconnected += message => Dispatcher.Invoke(() => StatusLabel.Content = message);
            _webSocketServer.OnEmergencyReceived += message => Dispatcher.Invoke(() => StatusLabel.Content = message);
            _webSocketServer.Start();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            _webSocketServer.Stop();
        }
    }
}

