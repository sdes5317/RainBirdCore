using System;

namespace RainBirdCore.ResponsePacket
{
    public class SerialNumber : ResponsePacket
    {
        public override string CmdCode => "85";
        public string Number { get; set; }
        public override void Initial(byte[] data)
        {
            Number = BitConverter.ToString(data, 1, 8).Replace("-", "");
        }
    }
}