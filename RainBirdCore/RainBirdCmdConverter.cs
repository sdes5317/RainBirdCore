using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RainBirdCore
{
    public class RainBirdCmdConverter
    {
        private dynamic _controlJson;

        public RainBirdCmdConverter(string jsonFilePath)
        {
            _controlJson = JsonConvert.DeserializeObject(File.ReadAllText(jsonFilePath));
        }

        public RainBirdPacket.ParametersInfo ToRainBirdCommand(string cmd, params RainBirdPacket.ParametersInfo.Parameter[] paras)
        {
            string contorlCode = _controlJson.ControllerCommands[$"{cmd}Request"]["command"];
            byte length = _controlJson.ControllerCommands[$"{cmd}Request"]["length"];
            var parameters = new List<RainBirdPacket.ParametersInfo.Parameter>();
            return new RainBirdPacket.ParametersInfo(contorlCode, length, paras);
        }

        public string GetRainBirdRequestJsonString(string cmd, params RainBirdPacket.ParametersInfo.Parameter[] paras)
        {
            var rainBirdCommand = ToRainBirdCommand(cmd, paras);
            var rainBirdRequest = new RainBirdPacket(rainBirdCommand);
            return JsonConvert.SerializeObject(rainBirdRequest);
        }

        public string TakeCmdRaw(string jsonStringData)
        {
            //just take data field,and drop other
            return JsonConvert.DeserializeObject<dynamic>(jsonStringData).result.data;
        }

        public static byte[] StringToBytes(string str)
        {
            var bytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}