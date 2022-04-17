using System;

namespace RainBirdCore.ResponsePacket
{
    public class WaterBudget : ResponsePacket
    {
        public override string CmdCode => "B0";
        public string ProgramCode { get; set; }
        public string SeasonalAdjust { get; set; }
        public override void Initial(byte[] data)
        {
            ProgramCode = BitConverter.ToString(data, 1, 1);
            SeasonalAdjust = BitConverter.ToString(data, 2, 2).Replace("-", "");
        }
    }
}