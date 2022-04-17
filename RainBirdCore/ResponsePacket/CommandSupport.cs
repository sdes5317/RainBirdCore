using System;

namespace RainBirdCore.ResponsePacket
{
    public class CommandSupport : ResponsePacket
    {
        public override string CmdCode => "84";
        public string CommandEcho { get; set; }
        public string Support { get; set; }
        public override void Initial(byte[] data)
        {
            CommandEcho = BitConverter.ToString(data, 1, 1);
            Support = BitConverter.ToString(data, 2, 1);
        }
    }
}