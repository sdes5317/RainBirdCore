using System.Collections.Generic;
using RainBirdCore.RequestPacket;
using Xunit;

namespace XUnitTest
{
    public class RequestPacketTest
    {
        [Theory]
        [MemberData(nameof(FakeRequest))]
        public void Test(string except, RequestBase request)
        {
            var actually = request.Data;
            Assert.Equal(except, actually);
        }

        public static IEnumerable<object[]> FakeRequest =>
            new List<object[]>
            {
                new object[] {"39000612", new ManuallyRunStationCmd(6, 18)},
                new object[] {"02", new ModelAndVersionCmd()},
                new object[] {"030C", new AvailableStationsCmd(12), },
                new object[] {"040B", new CommandSupportCmd(11), },
                //new object[] {"05", "SerialNumber"},
                //new object[] {"10", "CurrentTime"},
                //new object[] {"12", "CurrentDate"},
                //new object[] {"300D", "WaterBudget", new RainBirdPacket.ParametersInfo.Parameter("", 13)},
                //new object[] {"3E", "CurrentRainSensorState"},
                //new object[] {"3F10", "CurrentStationsActive", new RainBirdPacket.ParametersInfo.Parameter("", 16)},
                //new object[] {"3811", "ManuallyRunProgram", new RainBirdPacket.ParametersInfo.Parameter("", 17)},
                //new object[] {"3A17", "TestStations", new RainBirdPacket.ParametersInfo.Parameter("", 23)},
                //new object[] {"40", "StopIrrigation"},
                //new object[] {"36", "RainDelayGet"},
                //new object[] {"37000F", "RainDelaySet", new RainBirdPacket.ParametersInfo.Parameter("", 15)},
                //new object[] {"4208", "AdvanceStation", new RainBirdPacket.ParametersInfo.Parameter("", 8)},
                //new object[] {"48", "CurrentIrrigationState"},
            };
    }

}
