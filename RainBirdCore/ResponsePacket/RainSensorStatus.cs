namespace RainBirdCore.ResponsePacket
{
    public class RainSensorStatus : ResponsePacket
    {
        public override string CmdCode => "BF";
        public bool Status { get; set; }
        public override void Initial(byte[] data)
        {
            Status = data[1] == 1;
        }
    }
}