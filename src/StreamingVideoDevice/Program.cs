using NetMQ;
using NetMQ.Sockets;
using System;
using System.Diagnostics;
using System.Threading;

namespace StreamingVideoDevice
{
    class Program
    {
        static void Main(string[] args)
        {
            Process videoLoop = null;
            using (var commandSocket = new PullSocket("@tcp://StreamingVideoDevice:5558"))
            {
                while (true)
                {
                    string msg = commandSocket.ReceiveFrameString();
                    if (msg == "Start")
                    {
                        if (videoLoop == null)
                        {
                            videoLoop = StartVideo();
                        }
                    }
                    else
                    {
                        StopVideo(videoLoop);
                        videoLoop = null;
                    }
                }
            }
        }

        private static Process StartVideo()
        {
            var vid = new Process();
            vid.StartInfo.FileName = "ffmpeg";
            vid.StartInfo.Arguments = "-re -i /Videos/sample.mp4 -c copy -f flv rtmp://wowza:1935/live/myStream";
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
