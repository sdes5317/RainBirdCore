namespace RainBirdCore.ResponsePacket
{
    public abstract class ResponsePacket
    {
        public abstract string CmdCode { get; }
        public abstract void Initial(byte[] data);
    }
}
