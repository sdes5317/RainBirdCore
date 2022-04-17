using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using RainBirdCore.ResponsePacket;
using RestSharp;

namespace RainBirdCore
{
    public class RainBirdClient
    {
        private const int DefaultPage = 0;
        private readonly string _ip;
        private readonly string _password;
        private readonly RainBirdCmdConverter _rainBirdCmdConverter;
        private readonly RestClient _client;
        private readonly Dictionary<string, string> _defaultHeader = new Dictionary<string, string>();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly object Lock = new object();

        public RainBirdClient(RainBirdClientOptions options) : this(options.Ip, options.Password) { }
        public RainBirdClient(string ip, string password)
        {
            _ip = ip;
            _password = password;
            _rainBirdCmdConverter = new RainBirdCmdConverter($@".\sipcommands.json");
            _client = new RestClient($"http://{ip}/stick");
            _client.Timeout = 5000;

            #region DefaultHeader

            _defaultHeader.Add("Accept-Language", "en");
            _defaultHeader.Add("Accept-Encoding", "gzip, deflate");
            //_defaultHeader.Add("User-Agent", "RainBird/2.0 CFNetwork/811.5.4 Darwin/16.7.0");//這個欄位RestSharp要用另一種方式設定
            _defaultHeader.Add("Accept", " */*");
            _defaultHeader.Add("Connection", "keep-alive");
            _defaultHeader.Add("Content-Type", "application/octet-stream");

            #endregion

            #region Logging

            _logger.Debug("RainBirdClinet Initial");
            _logger.Debug($"BaseUrl:{_client.BaseUrl}");
            _logger.Debug($"Timeout:{_client.Timeout}");
            _logger.Debug($"Password:{password}");
            _logger.Debug($"Default Header");
            _defaultHeader.ToList().ForEach(header => _logger.Debug($"{header.Key}:{header.Value}"));

            #endregion
        }

        private RestRequest GetRequestWithDefaultHeader()
        {
            var request = new RestRequest(Method.POST);
            foreach (var header in _defaultHeader)
            {
                request.AddHeader(header.Key, header.Value);
            }

            return request;
        }

        public TResponse ProcessCmd<TResponse>(string cmd, params RainBirdPacket.ParametersInfo.Parameter[] paras)
        where TResponse : ResponsePacket.ResponsePacket, new()
        {
            var request = _rainBirdCmdConverter.GetRainBirdRequestJsonString(cmd, paras);
            _logger.Debug($"Request:{request}");
            var encryptRequest = AesEncryption.Encryptor(request, _password);
            _logger.Debug($"EncryptRequest:{BitConverter.ToString(encryptRequest)}");
            var httpRequest = GetRequestWithDefaultHeader();
            httpRequest.AddParameter("application/octet-stream", encryptRequest, ParameterType.RequestBody);
            _client.UserAgent = "RainBird/2.0 CFNetwork/811.5.4 Darwin/16.7.0";

            IRestResponse response;
            lock (Lock)
            {
                response = _client.Execute(httpRequest);
            }
            if (response.RawBytes == null)
            {
                _logger.Warn("Http Post 無回應，請確認RainBird是否可連線");
                throw new ArgumentNullException("Http Post 無回應，請確認RainBird是否可連線");
            }

            _logger.Debug($"EncryptReply:{BitConverter.ToString(response.RawBytes)}");
            string decryptReply;
            try
            {
                decryptReply = AesEncryption.Decryptor(response.RawBytes, _password);
            }
            catch (ArgumentException ex)
            {
                _logger.Warn($"封包解譯失敗(一個已知的狀況為同時間對RainBird進行兩個http請求):{ex}");
                throw new ArgumentException($"封包解譯失敗(一個已知的狀況為同時間對RainBird進行兩個http請求):{ex}");
            }
            catch (Exception ex)
            {
                _logger.Warn($"封包解譯失敗:{ex}");
                throw new Exception($"封包解譯失敗:{ex}");
            }
            _logger.Debug($"DecryptReply:{decryptReply}");
            var data = _rainBirdCmdConverter.TakeCmdRaw(decryptReply);
            var bytes = RainBirdCmdConverter.StringToBytes(data);
            try
            {
                var obj = new TResponse();
                obj.Initial(bytes);
                return obj;
            }
            catch (Exception)
            {
                _logger.Warn($"收到未定義指令碼的封包:{decryptReply}");
                throw new ArgumentNullException($"收到未定義指令碼的封包:{decryptReply}");
            }
        }

        public ModelAndVersion GetModelAndVersion()
        {
            return ProcessCmd<ModelAndVersion>("ModelAndVersion");
        }

        public AvailableStations GetAvailableStation(int page = DefaultPage)
        {
            return ProcessCmd<AvailableStations>("AvailableStations", new RainBirdPacket.ParametersInfo.Parameter("page", page));
        }

        public CommandSupport GetCommandSupport(int command)
        {
            return ProcessCmd<CommandSupport>("CommandSupport", new RainBirdPacket.ParametersInfo.Parameter("command", command));
        }
        public string GetSerialNumber()
        {
            return ProcessCmd<SerialNumber>("SerialNumber").Number;
        }

        public CurrentTime GetCurrentTime()
        {
            return ProcessCmd<CurrentTime>("CurrentTime");
        }
        public CurrentDate GetCurrentDate()
        {
            return ProcessCmd<CurrentDate>("CurrentDate");
        }

        public WaterBudget SetWaterBudget(int budget)
        {
            return ProcessCmd<WaterBudget>("WaterBudget", new RainBirdPacket.ParametersInfo.Parameter("budget", budget));
        }
        //定期要
        public bool GetRainSensorStatus()
        {
            return ProcessCmd<RainSensorStatus>("CurrentRainSensorState").Status;
        }

        public void SetProgram(int program)
        {
            ProcessCmd<NoneDefineResponse>(
                "ManuallyRunProgram", new RainBirdPacket.ParametersInfo.Parameter("program", program));
        }
        public void TestZone(int zone)
        {
            ProcessCmd<NoneDefineResponse>("TestStations", new RainBirdPacket.ParametersInfo.Parameter("zone", zone));
        }
        /// <summary>
        /// 控制指定的zone澆水幾分鐘
        /// </summary>
        /// <param name="zone">不須要-1</param>
        /// <param name="minute"></param>
        /// <returns></returns>
        public virtual string IrrigateZone(int zone, int minute)
        {
            return ProcessCmd<ProcessStatus>("ManuallyRunStation",
                new RainBirdPacket.ParametersInfo.Parameter("zone", zone),
                new RainBirdPacket.ParametersInfo.Parameter("minute", minute)).Result;
        }
        public virtual IrrigationState GetIrrigationState(int page = DefaultPage)
        {
            return ProcessCmd<IrrigationState>("CurrentStationsActive",
                new RainBirdPacket.ParametersInfo.Parameter("page", page));
        }
        public virtual string StopIrrigationState()
        {
            return ProcessCmd<ProcessStatus>("StopIrrigation").Result;
        }
    }
}
