using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using StreamingVideoDevice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoApi.Services
{
    public class ZeroMqDeviceService
    {
        RouterSocket queryRouterSocket = new RouterSocket("@inproc://queryRouter");
        PublisherSocket querySocket = new PublisherSocket("@tcp://*:5557");
        RouterSocket commandSocket = new RouterSocket("@tcp://*:5558");
        NetMQPoller poller;
        public ZeroMqDeviceService()
        {
            //We are using NetMQ to retrieve the available devices in the following manner:
            // queryRouterSocket -> querySocket -> All Devices In -> All Device out -> commandSocket -> queryRouterSocket
            // Router            -> Publisher   -> Subscriber     -> Dealer         -> Router        -> Router
            //This pattern allows any client to request the list of Clients in the commandSocket. So we can send messages directly to clients at another point in time.
            queryRouterSocket.ReceiveReady += (o, e) =>
            {
                //Send a message to all clients with the following fields:
                // 1 - Subscription Name
                // 2 - queryRouterSocket id
                // 3 - command type
                var incomingMsg = e.Socket.ReceiveMultipartBytes();
                var messageToServer = new NetMQMessage();
                messageToServer.Append("All");
                incomingMsg.ForEach((val) => { messageToServer.Append(val); });
                querySocket.SendMultipartMessage(messageToServer);
            };
            commandSocket.ReceiveReady += (o, e) =>
            {
                //Recieve a message from the clients with the following fields:
                // 1 - Client Id
                // 2 - queryRouterSocket id
                // 3 - response
                var msg = commandSocket.ReceiveMultipartBytes();
                msg.RemoveAt(0); //Remove the Client id
                var messageToClient = new NetMQMessage();
                msg.ForEach((val) => { messageToClient.Append(val); });
                queryRouterSocket.SendMultipartMessage(messageToClient); //forward the message
            };
            poller = new NetMQPoller{ queryRouterSocket, commandSocket };
            poller.RunAsync();
        }

        public IEnumerable<DeviceStatus> GetDevices()
        {
           var reqSock = new DealerSocket(">inproc://queryRouter");
            reqSock.SendReady += (o, e) =>
            {
                e.Socket.SendFrame("SendStatus");
            };
            reqSock.Poll();
            
            string msg;
            var results = new List<DeviceStatus>();
            while (reqSock.TryReceiveFrameString(TimeSpan.FromSeconds(1), out msg))
            {
                results.Add(JsonConvert.DeserializeObject<DeviceStatus>(msg));
            }
            return results;
        }
        public void StartDevice(string name)
        {
            var messageToServer = new NetMQMessage();
            messageToServer.Append(name);
            messageToServer.Append("Start");
            commandSocket.SendMultipartMessage(messageToServer);
        }
        public void StopDevice(string name)
        {
            var messageToServer = new NetMQMessage();
            messageToServer.Append(name);
            messageToServer.Append("Stop");
            commandSocket.SendMultipartMessage(messageToServer);
        }
    }
}
