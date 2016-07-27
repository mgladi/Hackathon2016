using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{

    public class AgentData
    {
        public DeviceType DeviceType { get; set; }

        public string DeviceName { get; set; }
    }
    public enum DeviceType
    {
        Windows,
        Android
    }
}
