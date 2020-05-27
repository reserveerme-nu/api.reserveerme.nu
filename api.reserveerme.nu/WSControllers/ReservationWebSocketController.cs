using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace api.reserveerme.nu.WSControllers
{
    public class ReservationWebSocketController : WebSocketBehavior
    {
        protected override void OnError(ErrorEventArgs e)
        {
            Debug.WriteLine(e.Message);
        }

        protected override void OnOpen()
        {
            Debug.WriteLine("Websocket connection established with " + Context.Origin);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast("Message received.");
        }
    }
}