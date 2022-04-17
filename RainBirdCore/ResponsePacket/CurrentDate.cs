namespace RainBirdCore.ResponsePacket
{
    public class CurrentDate : ResponsePacket
    {
        public override string CmdCode => "92";
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public override void Initial(byte[] data)
        {
            Day = data[1];
            Month = data[2] >> 4;
            Year = data[3] + ((data[2] & 0x0f) << 8);
        }
    }
}