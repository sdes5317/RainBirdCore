using System;

namespace RainBirdCore.ResponsePacket
{
    public class ModelAndVersion : ResponsePacket
    {
        public override string CmdCode => "82";
        public string ModelID { get; set; }
        public string ProtocolRevisionMajor { get; set; }
        public string ProtocolRevisionMinor { get; set; }

        public override void Initial(byte[] data)
        {
            ModelID = BitConverter.ToString(data, 1, 2).Replace("-", "");
            ProtocolRevisionMajor = BitConverter.ToString(data, 3, 1);
            ProtocolRevisionMinor = BitConverter.ToString(data, 4, 1);
        }
    }
}