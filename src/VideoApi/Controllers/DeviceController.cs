using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetMQ;
using NetMQ.Sockets;
using VideoApi.Services;

namespace VideoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        ZeroMqDeviceService commandSocket_;
        public DeviceController(ZeroMqDeviceService commandSocket)
        {
            commandSocket_ = commandSocket;
        }
        // GET: api/Device
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return commandSocket_.GetDevices();
        }

        // GET: api/Device/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Device
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // POST: api/Device
        [HttpPost("{id}/{command}")]
        public void Post(string id, string command)
        {
            //using (var commandSocket = new PushSocket(">tcp://StreamingVideoDevice:5558"))
            {
                if (command == "Start")
                    commandSocket_.StartDevice(id);
                else if (command == "Stop")
                    commandSocket_.StopDevice(id);
            }
        }
    }
}
