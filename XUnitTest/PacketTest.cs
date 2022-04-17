using System.Collections.Generic;
using System.Linq;
using RainBirdCore;
using Xunit;

namespace XUnitTest
{
    public class PacketTest
    {
        [Theory]
        [MemberData(nameof(FakeRequest))]
        public void RequestTest(string except, string cmd, params RainBirdPacket.ParametersInfo.Parameter[] paras)
        {
            var converter = new RainBirdCmdConverter("sipcommands.json");
            var actually = converter.ToRainBirdCommand(cmd, paras);
            Assert.Equal(except, actually.Data);
        }

        public static IEnumerable<object[]> FakeRequest =>
            new List<object[]>
            {
                new object[] {"02", "ModelAndVersion"},
                new object[] {"030C", "AvailableStations", new RainBirdPacket.ParametersInfo.Parameter("", 12)},
                new object[] {"040B", "CommandSupport", new RainBirdPacket.ParametersInfo.Parameter("", 11)},
                new object[] {"05", "SerialNumber"},
                new object[] {"10", "CurrentTime"},
                new object[] {"12", "CurrentDate"},
                new object[] {"300D", "WaterBudget", new RainBirdPacket.ParametersInfo.Parameter("", 13)},
                new object[] {"3E", "CurrentRainSensorState"},
                new object[] {"3F10", "CurrentStationsActive", new RainBirdPacket.ParametersInfo.Parameter("", 16)},
                new object[] {"3811", "ManuallyRunProgram", new RainBirdPacket.ParametersInfo.Parameter("", 17)},
                new object[] {"39000612", "ManuallyRunStation", new RainBirdPacket.ParametersInfo.Parameter("", 6), new RainBirdPacket.ParametersInfo.Parameter("", 18)},
                new object[] {"3A17", "TestStations", new RainBirdPacket.ParametersInfo.Parameter("", 23)},
                new object[] {"40", "StopIrrigation"},
                new object[] {"36", "RainDelayGet"},
                new object[] {"37000F", "RainDelaySet", new RainBirdPacket.ParametersInfo.Parameter("", 15)},
                new object[] {"4208", "AdvanceStation", new RainBirdPacket.ParametersInfo.Parameter("", 8)},
                new object[] {"48", "CurrentIrrigationState"},
            };

        public class ExceptReplyMessage
        {
            public ExceptReplyMessage(string type, params Parameters[] paras)
            {
                Type = type;
                Paras = paras.ToList();
            }

            public string Type { get; set; }
            public List<Parameters> Paras { get; set; }

            public class Parameters
            {
                public Parameters(string name, int value)
                {
                    Name = name;
                    Value = value;
                }

                public string Name { get; set; }
                public int Value { get; set; }
            }
        }
    }
}
