using System.Text;

namespace RainBirdCore.RequestPacket
{
    public abstract class RequestBase
    {
        public string Data { get; protected set; }
        public int Length => Data.Length / 2;
        public abstract string ControlCode { get; }
    }

    public class ManuallyRunStationCmd : RequestBase
    {
        public override string ControlCode =>"39";
        public string UnknownCode => "00";
        public int Zone { get; }
        public int Minute { get; }

        public ManuallyRunStationCmd(int zone, int minute)
        {
            Zone = zone;
            Minute = minute;
            var builder = new StringBuilder();
            builder.Append(ControlCode);
            builder.Append(UnknownCode);
            builder.Append(Zone.ToString("X2"));
            builder.Append(Minute.ToString("X2"));
            Data = builder.ToString();
        }
    }
    public class ModelAndVersionCmd : RequestBase
    {
        public override string ControlCode =>"02";

        public ModelAndVersionCmd()
        {
            Data = ControlCode;
        }
    }

    public class AvailableStationsCmd : RequestBase
    {
        public override string ControlCode => "03";
        public int Page { get; }

        public AvailableStationsCmd(int page)
        {
            Page = page;
            var builder = new StringBuilder();
            builder.Append(ControlCode);
            builder.Append(Page.ToString("X2"));
            Data = builder.ToString();
        }
    }
    public class CommandSupportCmd : RequestBase
    {
        public override string ControlCode => "04";
        public byte Command { get; }

        public CommandSupportCmd(byte command)
        {
            Command = command;
            var builder = new StringBuilder();
            builder.Append(ControlCode);
            builder.Append(Command.ToString("X2"));
            Data = builder.ToString();
        }
    }
}
