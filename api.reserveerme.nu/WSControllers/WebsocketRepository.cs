using WebSocketSharp.Server;

namespace api.reserveerme.nu.WSControllers
{
    public static class WebsocketRepository
    {
        private static WebSocketServer _webSocketServer;

        public static WebSocketServer GetWebSocketServer()
        {
            if (_webSocketServer == null)
            {
                _webSocketServer = new WebSocketServer(6969);
            }
            return _webSocketServer;
        }
    }
}