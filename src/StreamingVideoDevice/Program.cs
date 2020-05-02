using NetMQ;
using NetMQ.Sockets;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading;

namespace StreamingVideoDevice
{
    class Program
    {
        static string hostName = Environment.GetEnvironmentVariable("HOST_NAME");
        static void Main(string[] args)
        {
            Process videoLoop = null;
            using (var responseSocket = new SubscriberSocket(">tcp://VideoApi:5557"))
            using (var requestSocket = new DealerSocket(">tcp://VideoApi:5558"))
            using (var poller = new NetMQPoller { responseSocket, requestSocket })
            {
                requestSocket.Options.Identity = Encoding.ASCII.GetBytes(hostName);
                responseSocket.Subscribe("All");
                responseSocket.ReceiveReady += (o, e) =>
                {
                    var msg = e.Socket.ReceiveMultipartStrings();
                    if (msg[1] == "SendStatus")
                    {
                        requestSocket.SendFrame("MyData");
                    }
                };
                requestSocket.ReceiveReady += (o,e) =>
                {
                    var msg = e.Socket.ReceiveFrameString();
                    if (msg == "Start")
                    {
                        if (videoLoop == null)
                        {
                            videoLoop = StartVideo();
                        }
                    }
                    else if (msg == "Stop")
                    {
                        StopVideo(videoLoop);
                        videoLoop = null;
                    }
                };
                poller.Run();
            }
        }

        private static Process StartVideo()
        {
            var vid = new Process();
            vid.StartInfo.FileName = "ffmpeg";
            vid.StartInfo.Arguments = $"-re -i /Videos/sample.mp4 -c copy -f flv rtmp://wowza:1935/live/{hostName}";
            vid.StartInfo.UseShellExecute = true;
            vid.EnableRaisingEvents = true;
            vid.Exited += (o, e) =>
            {
                if (o is Process)
                {
                    var myProc = o as Process;
                    myProc.Start();
                }
            };
            vid.Start();

            return vid;
        }
        private static void StopVideo(Process vidProcess)
        {
            if (vidProcess != null)
            {
                vidProcess.Kill();
                vidProcess.Dispose();
            }
        }
    }
}
