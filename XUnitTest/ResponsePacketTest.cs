using RainBirdCore;
using RainBirdCore.ResponsePacket;
using Xunit;

namespace XUnitTest
{
    public class ResponsePacketTest
    {

        [Fact]
        public void ProcessStatusTest()
        {
            var packet = RainBirdCmdConverter.StringToBytes("0139");
            var status = new ProcessStatus();
            status.Initial(packet);
            Assert.Equal("01", status.CmdCode);
            Assert.Equal("39", status.Result);
        }
        [Fact]
        public void IrrigationStateTest()
        {
            var packet = RainBirdCmdConverter.StringToBytes("BF0000000100");
            var status = new IrrigationState();
            status.Initial(packet);
            Assert.Equal("BF", status.CmdCode);
            Assert.True(status[17]);

            packet = RainBirdCmdConverter.StringToBytes("BF0000001000");
            status.Initial(packet);
            Assert.True(status[21]);
            packet = RainBirdCmdConverter.StringToBytes("BF0000010000");
            status.Initial(packet);
            Assert.True(status[9]);
            packet = RainBirdCmdConverter.StringToBytes("BF0000100000");
            status.Initial(packet);
            Assert.True(status[13]);
            packet = RainBirdCmdConverter.StringToBytes("BF0001000000");
            status.Initial(packet);
            Assert.True(status[1]);
            packet = RainBirdCmdConverter.StringToBytes("BF0010000000");
            status.Initial(packet);
            Assert.True(status[5]);
        }

        [Theory]
        [InlineData("820006090C")]
        public void ModelAndVersionTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var modelAndVersion = new ModelAndVersion();
            modelAndVersion.Initial(packet);
            Assert.Equal("82", modelAndVersion.CmdCode);
            Assert.Equal("0006", modelAndVersion.ModelID);
            Assert.Equal("09", modelAndVersion.ProtocolRevisionMajor);
            Assert.Equal("0C", modelAndVersion.ProtocolRevisionMinor);
        }

        [Theory]
        [InlineData("8300FFFF0700")]
        public void AvailableStationsTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var availableStations = new AvailableStations();
            availableStations.Initial(packet);
            Assert.Equal("83", availableStations.CmdCode);
            Assert.Equal(0, availableStations.PageNumber);
            Assert.True(availableStations[1]);
            Assert.True(availableStations[2]);
            Assert.True(availableStations[3]);
            Assert.True(availableStations[4]);
            Assert.True(availableStations[5]);
            Assert.True(availableStations[6]);
            Assert.True(availableStations[7]);
            Assert.True(availableStations[8]);
            Assert.True(availableStations[9]);
            Assert.True(availableStations[10]);
            Assert.True(availableStations[11]);
            Assert.True(availableStations[12]);
            Assert.True(availableStations[13]);
            Assert.True(availableStations[14]);
            Assert.True(availableStations[15]);
            Assert.True(availableStations[16]);
            Assert.True(availableStations[17]);
            Assert.True(availableStations[18]);
            Assert.True(availableStations[19]);
            Assert.False(availableStations[20]);
            Assert.False(availableStations[21]);
            Assert.False(availableStations[22]);
            Assert.False(availableStations[23]);
        }

        [Theory]
        [InlineData("848201")]
        public void CommandSupportTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var commandSupport = new CommandSupport();
            commandSupport.Initial(packet);
            Assert.Equal("84", commandSupport.CmdCode);
            Assert.Equal("82", commandSupport.CommandEcho);
            Assert.Equal("01", commandSupport.Support);
        }
        [Theory]
        [InlineData("850000000000008963")]
        public void SerialNumberTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var serialNumber = new SerialNumber();
            serialNumber.Initial(packet);
            Assert.Equal("85", serialNumber.CmdCode);
            Assert.Equal("0000000000008963", serialNumber.Number);
        }
        [Theory]
        [InlineData("9212B7E2")]
        public void CurrentDateTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var currentDate = new CurrentDate();
            currentDate.Initial(packet);
            Assert.Equal("92", currentDate.CmdCode);
            Assert.Equal(2018,currentDate.Year);
            Assert.Equal(11,currentDate.Month);
            Assert.Equal(18,currentDate.Day);
        }
        [Theory]
        [InlineData("900C3623")]
        public void CurrentTimeTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var currentTime = new CurrentTime();
            currentTime.Initial(packet);
            Assert.Equal("90", currentTime.CmdCode);
            Assert.Equal(12,currentTime.Hour);
            Assert.Equal(54,currentTime.Minute);
            Assert.Equal(35,currentTime.Second);
        }
        [Theory]
        [InlineData("BF01")]
        public void RainSensorStatusTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var rainSensorStatus = new RainSensorStatus();
            rainSensorStatus.Initial(packet);
            Assert.Equal("BF", rainSensorStatus.CmdCode);
            Assert.True(rainSensorStatus.Status);
        }
        [Theory]
        [InlineData("B0030083")]
        public void WaterBudgetTest(string test)
        {
            var packet = RainBirdCmdConverter.StringToBytes(test);
            var waterBudget = new WaterBudget();
            waterBudget.Initial(packet);
            Assert.Equal("B0", waterBudget.CmdCode);
            Assert.Equal("03", waterBudget.ProgramCode);
            Assert.Equal("0083", waterBudget.SeasonalAdjust);
        }
    }
}
