namespace RainBirdCore.ResponsePacket
{
    public class CurrentTime : ResponsePacket
    {
        public override string CmdCode => "90";
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }
        public override void Initial(byte[] data)
        {
            Hour = data[1];
            Minute = data[2];
            Second = data[3];
        }
    }
}