using System;
using System.Linq;

namespace RainBirdCore.ResponsePacket
{
    public class ProcessStatus : ResponsePacket
    {
        public override string CmdCode => "01";
        public string Result { get; set; }

        public override void Initial(byte[] data)
        {
            Result = Convert.ToString(data.Skip(1).Take(1).First(), 16);
        }
    }
}