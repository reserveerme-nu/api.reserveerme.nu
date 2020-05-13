using System;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace api.reserveerme.nu.WSControllers
{
    public class ReservationWebSocketController : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            var message = "TestReturnMessage";
            Sessions.Broadcast(message);
        }
    }
}