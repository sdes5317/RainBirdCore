using System;
using System.Linq;
using RainBirdCore;

namespace Sample
{
    class Program
    {
        private static RainBirdClient _rainbird = new RainBirdClient("192.168.1.248", "12345");

        static void Main(string[] args)
        {
            //模擬RainBird加密過後的封包解密
            var py =
                "cad2cf3187d835baf9588785c9d2eb408dc1cb18a91af5eba6d48b8508a42806d79824e4579dfadce37c0b369c7d7156947401f14bf157a7efaf9d0291bea0c130161772872eae00d25abdf503a2efe2095f064a00d672db807855aae4b6f67b0a4603a4892b301b21418d4cdc197ceeee60f8cddcbf9fb8b9d25c20f9ca4dc5f176efac68b2f9c2125e46fe846e7e83";
            var pyBytes = StringToByteArray(py);
            //Console.WriteLine(BitConverter.ToString(pysSplit));
            var pyResult = AesEncryption.Decryptor(pyBytes, "12345");
            Console.WriteLine(pyResult);
            
            //Api使用的範例
            //CommonSample();
        }
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                .ToArray();
        }
        //支援的api基本範例
        //v代表在機器上實測過
        private static void CommonSample()
        {
            var serialNumber = _rainbird.GetSerialNumber();

            var availableStation = _rainbird.GetAvailableStation();//v

            var modelAndVersion = _rainbird.GetModelAndVersion();

            var commandSupport = _rainbird.GetCommandSupport(0x83);//v

            var currentDate = _rainbird.GetCurrentDate();
            var currentTime = _rainbird.GetCurrentTime();

            var status = _rainbird.GetRainSensorStatus();

            var irrigationState = _rainbird.GetIrrigationState();//v

            var stopIrrigationState = _rainbird.StopIrrigationState();//v
        }
    }
}
