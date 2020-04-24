using System;
using System.Diagnostics;

namespace StreamingVideoDevice
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; ++i)
            {
                using (var vid = new Process())
                {
                    vid.StartInfo.FileName = "ffmpeg";
                    vid.StartInfo.Arguments = "-re -i /Videos/sample.mp4 -c copy -f flv rtmp://wowza:1935/live/myStream";
                    vid.StartInfo.UseShellExecute = true;
                    vid.Start();
                    vid.WaitForExit();
                }
            }
        }
    }
}
