using System.Collections;
using System.Linq;

namespace RainBirdCore.ResponsePacket
{
    public class AvailableStations : ResponsePacket
    {
        public override string CmdCode => "83";
        public int PageNumber { get; set; }
        public BitArray Result { get; set; }
        public bool this[int index] => Result.Get(index - 1);
        public override void Initial(byte[] data)
        {
            PageNumber = data.Skip(1).Take(1).First();
            Result = new BitArray(data.Skip(2).Take(4).ToArray());
        }
    }
}