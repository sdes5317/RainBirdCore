using System;

namespace RainBirdCore.ResponsePacket
{
    public class NoneDefineResponse : ResponsePacket
    {
        public override string CmdCode => "01";
        public string HexString { get; set; }
        public override void Initial(byte[] data)
        {
            HexString = BitConverter.ToString(data);
        }
    }
}