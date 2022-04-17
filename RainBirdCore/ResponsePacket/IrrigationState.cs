using System.Collections;
using System.Linq;

namespace RainBirdCore.ResponsePacket
{
    public class IrrigationState : ResponsePacket
    {
        public override string CmdCode => "BF";
        public BitArray ActiveStations { get; set; }
        public bool this[int index] => ActiveStations.Get(index - 1);
        public override void Initial(byte[] data)
        {
            var bytes = data.Skip(2).Take(4).ToArray();
            ActiveStations = new BitArray(bytes);
        }

        public string GetStation()
        {
            for (int i = 1; i <= 17; i++)
            {
                if (this[i] is true)
                {
                    return i.ToString();
                }
            }

            return "None";
        }
    }
}