using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VideoApi.Services
{
    public class ZeroMqDeviceService
    {
        PublisherSocket querySocket = new PublisherSocket("@tcp://*:5557");
        RouterSocket commandSocket = new RouterSocket("@tcp://*:5558");
        public ZeroMqDeviceService()
        {
            
        }

        public IEnumerable<string> GetDevices()
        {
            var messageToServer = new NetMQMessage();
            messageToServer.Append("All");
            messageToServer.Append("SendStatus");
            querySocket.SendMultipartMessage(messageToServer);
            List<string> msg = new List<string>();
            List<string> results = new List<string>();
            //var response = querySocket.ReceiveFrameString();
            while (commandSocket.TryReceiveMultipartStrings(TimeSpan.FromSeconds(1), ref msg))
            {
                results.Add(msg[0]);
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
