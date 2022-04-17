using System;
using System.Text;
using Newtonsoft.Json;

namespace RainBirdCore
{
    //{"id":1588036947,"jsonrpc":"2.0","method":"tunnelSip","params":{"data":"39000405","length":4}}

    public class RainBirdPacket
    {
        [JsonProperty("id")]
        public int Id { get; set; } = (int)DateTimeOffset.Now.ToUnixTimeSeconds();//TimeStamp

        [JsonProperty("jsonrpc")]
        public string JsonRpc { get; set; } = "2.0";

        [JsonProperty("method")]
        public string Method { get; set; } = "tunnelSip";

        [JsonProperty("params")]
        public ParametersInfo ParamsInfo { get; set; }

        public RainBirdPacket()
        {
            
        }

        public RainBirdPacket(ParametersInfo parametersInfo)
        {
            ParamsInfo = parametersInfo;
        }

        public class ParametersInfo
        {
            [JsonProperty("data")]
            public string Data { get; set; }
            [JsonProperty("length")]
            public int Length { get; set; }
            [JsonIgnore]
            public string CmdCode { get; set; }
            [JsonIgnore]
            public Parameter[] Parameters { get; }
            
            public ParametersInfo(){}
            public ParametersInfo(string cmdCode, byte length, params Parameter[] paras)
            {
                Parameters = paras;
                var stringBuilder = new StringBuilder();
                stringBuilder.Append(cmdCode);
                //由於反編譯資訊不足，如果長度不夠，這裡會補0
                stringBuilder.Append("".PadLeft((length - 1 - paras.Length) * 2, '0'));
                foreach (var para in paras)
                {
                    stringBuilder.Append(para.Value.ToString("X2"));
                }

                Data = stringBuilder.ToString();
                CmdCode = cmdCode;
                Length = length;
            }
        
            public class Parameter
            {
                public Parameter(string name, int value)
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
