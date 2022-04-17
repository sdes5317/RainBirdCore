using RainBirdCore;
using Xunit;

namespace XUnitTest
{
    public class RainBirdCmdConvertTest
    {
        private string _json = "{\"jsonrpc\": \"2.0\", \"result\":{\"length\":6, \"data\":\"BF0000000100\"}, \"id\": 19474}";
        private RainBirdCmdConverter _cmd = new RainBirdCmdConverter("sipcommands.json");

        [Fact]
        public void TakeCmdRawTest()
        {
            var data= _cmd.TakeCmdRaw(_json);
            Assert.Equal("BF0000000100",data);
        }

    }
}
