using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WaterIot.Base;
using WaterIot.Enum;
using WaterIot.Models;
using WaterIot.OthersServices;
using Newtonsoft.Json;
using MQTTnet.Client.Connecting;

namespace WaterIot
{
    public class MainWindowViewModel : Base.NotificationObject
    {
        ///WRA User API Root 網址
        private readonly Uri _wraOAuth2UriRoot = new Uri("https://iapi.wra.gov.tw/v3/oauth2/");

        ///OAuth2 認證所需之金鑰ID與密碼
        private readonly Uri _wraApiUriRoot = new Uri("https://iapi.wra.gov.tw/v3/api/");

        //private readonly string _clientId = "c9OeU08DNFnoR+bkUwdEr4UGZHUhX8rh";	//南投
        //private readonly string _clientSecret = "bS402lYhz5LJbx13Mwnkww==";		//南投

        //private readonly string _clientId = "sgUm53TxhyPC86YM0efLe6oTqbIJmAUm+N9C5sg6gAM=";     //桃園
        //private readonly string _clientSecret = "6h5GTkAhwMgZgmYYS0a6gg==";			//桃園

        private string _response;
        private System.Timers.Timer _timer;

        /// <summary>
        /// 現在時間
        /// </summary>
        public DateTime NowTime => DateTime.Now;

        /// <summary>
        /// 設定
        /// </summary>
        public Config Config { get; set; }

        /// <summary>
        /// Iot API
        /// </summary>
        public OthersServices.WaterIot WaterIot { get; set; }

        /// <summary>
        /// 水利署 User Token
        /// </summary>
        public OAuth2Client OAuth2Client { get; set; }

        /// <summary>
        /// 水利署 API
        /// </summary>
        //public UserApiClient UserApiClient { get; set; }

        public MQTTClient MQTTClient { get; set; }

        public IUploadEnable UploadController { get; set; }

        public ObservableCollection<string> History { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// 資料
        /// </summary>
        public IEnumerable<SensorDevice> SensorDevices { get; set; }

        /// <summary>
        /// 設備
        /// </summary>
        public List<Device> Devices { get; set; }

        /// <summary>
        /// 自動下載、上傳
        /// </summary>
        public bool AutoEnabled
        {
            get
            {
                return Config.AutoMode;
            }
            set
            {
                Config.AutoMode = value;
                if (value)
                {
                    AutoCounter = NowTime;
                }
            }
        }

        public bool FakeDataEnabled
        {
            get
            {
                return Config.FakeData;
            }
            set
            {
                Config.FakeData = value;
            }
        }

        /// <summary>
        /// 自動間隔
        /// </summary>
        public int AutoInterval
        {
            get
            {
                return Config.UploadInterval;
            }
            set
            {
                Config.UploadInterval = value;
            }
        }

        /// <summary>
        /// 淹水間隔
        /// </summary>
        public int FloodingAutoInterval
        {
            get
            {
                return Config.FloodingUploadInterval;
            }
            set
            {
                Config.FloodingUploadInterval = value;
            }
        }

        /// <summary>
        /// 淹水中
        /// </summary>
        public bool Flooding { get; set; } = false;

        /// <summary>
        /// 自動倒數
        /// </summary>
        public DateTime AutoCounter { get; set; } = DateTime.Now;

        /// <summary>
        /// 離開
        /// </summary>
        public ICommand Exit => new DelegateCommand(ExitExecute);

        /// <summary>
        /// 刷新資料
        /// </summary>
        public ICommand RefreshRecord => new DelegateCommand<Record>(RefreshRecordExecute);

        /// <summary>
        /// 儲存設定
        /// </summary>
        public ICommand SaveConfig => new DelegateCommand(SaveConfigExecute);

        /// <summary>
        /// 上傳水利署
        /// </summary>
        public ICommand UploadRecord => new DelegateCommand<Record>(UploadRecordExecute, CanUploadRecord);

        /// <summary>
        /// 下載值
        /// </summary>
        //public ICommand DownloadRecord => new DelegateCommand<Record>(DownloadRecordExecute, CanDownloadRecord);

        /// <summary>
        /// 刷新全部
        /// </summary>
        public ICommand RefreshAll => new DelegateCommand(RefreshAllExecute);

        /// <summary>
        /// 上傳所有已啟用
        /// </summary>
        public ICommand UploadAll => new DelegateCommand(UploadAllExecute);

        public ICommand TestConnect => new DelegateCommand(async () => { await MQTTClient.Connect(); });
        public ICommand TestDisconnect => new DelegateCommand(async () => { await MQTTClient.Disconnect(); });
        public bool IsConnected
        {
            get => MQTTClient.IsConnected;
            set
            {

            }
        }

        private bool IsSendLineNotify { get; set; } = true;

        private Thread Thread { get; set; }

        public MainWindowViewModel()
        {
            string localPath = Path.Combine(Environment.CurrentDirectory, @"Data\Config.json");
            Config = ClsJson.LoadFromFile<Config>(localPath);
            WaterIot = new OthersServices.WaterIot(Config.ID, Config.Password);
            Devices = Config.Devices ?? new List<Device>();
            //InitialWaterIot();
            OAuth2Client = new OAuth2Client(_wraOAuth2UriRoot, Config.clientId, Config.clientSecret);

            if(Config.UploadType == UploadType.API)
                UploadController = new UserApiClient(_wraApiUriRoot, OAuth2Client);
            else if(Config.UploadType == UploadType.MQTT)
            {
                IPHostEntry wraIP = Dns.GetHostEntry("114.32.143.214");
                MQTTClient = new MQTTClient(wraIP.AddressList[0].ToString());
                MQTTClient.Connect();
            }

            MQTTClient.DisconnectedHandler += (sender, obj) =>
            {
                RaisePropertyChanged("IsConnected");
                //MQTTClient.Connect();
            };

            MQTTClient.ConnectedHandler += (sender, obj) =>
            {
                RaisePropertyChanged("IsConnected");
            };

            _timer = new System.Timers.Timer()
            {
                AutoReset = true,
                Enabled = true,
                Interval = 1000,
            };
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private async Task<bool> MQTTInitial()
        {
            IPHostEntry wraIP = Dns.GetHostEntry("114.32.143.214");
            MQTTClient = new MQTTClient(wraIP.AddressList[0].ToString());
            bool result = await MQTTClient.Connect();
            return result;
        }

        private void ExitExecute()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void RefreshRecordExecute(Record obj)
        {
            CreateRecords(obj.Parents);
        }

        private void SaveConfigExecute()
        {
            Config.Devices = Devices;
            string localPath = Path.Combine(Environment.CurrentDirectory, @"Data\Config.json");
            ClsJson.SaveToFile(Config, localPath);
        }

        private bool CanUploadRecord(Record arg)
        {
            return arg.Guid != Guid.Empty;
        }

        private void UploadRecordExecute(Record obj)
        {
            var tmp = TimeSeriesDataWrite(obj);
            if (tmp)
            {
                SaveConfigExecute();
            }
        }

        private bool CanDownloadRecord(Record arg)
        {
            return arg.Guid != Guid.Empty;
        }

        //private void DownloadRecordExecute(Record obj)
        //{
        //    var tmp = LatestDataReadPhysicalQuantity(obj.Guid);
        //    obj.LastUploadTime = tmp.TimeStamp.DateTime;
        //    //obj.UploadValue = tmp.Value;
        //}

        private void RefreshAllExecute()
        {
            InitialWaterIot();
        }

        private void UploadAllExecute()
        {
            CreateDevice();

            var tmp = Devices.SelectMany(t => t.Records).Where(t => t.Value != null && t.Guid != Guid.Empty && t.Upload);
            var ok = TimeSeriesDataWrite(tmp.ToArray());
            if (ok)
            {
                SaveConfigExecute();
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            RaisePropertyChanged(nameof(NowTime));
            string HHmm = NowTime.ToString("HHmm");
            if (HHmm == "0800" && IsSendLineNotify == false)
            {
                IsSendLineNotify = true;
            }
            if (AutoEnabled)
            {
                var interval = NowTime - AutoCounter;
                if (interval.TotalSeconds > (Flooding ? FloodingAutoInterval : AutoInterval))
                {
                    Console.WriteLine(interval.TotalSeconds);
                    Console.WriteLine(NowTime);
                    AutoCounter = NowTime;
                    CreateDevice();

                    var tmp = Devices.SelectMany(t => t.Records).Where(t => t.Value != null && t.Guid != Guid.Empty && t.Upload);
                    var count = tmp.Count(t => (t.Value is double v) && v < 10000 && v != 0);
                    if (count > 0)
                    {
                        Flooding = true;
                    }
                    else
                    {
                        Flooding = false;
                    }
                    var ok = TimeSeriesDataWrite(tmp.ToArray());
                    if (ok)
                    {
                        SaveConfigExecute();
                    }
                }
            }
        }

        /// <summary>
        /// 初始化資料來源
        /// </summary>
        public void InitialWaterIot()
        {
            if (Login())
            {
                ListSensor();
                CreateDevice();
            }
        }

        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            bool tmp = WaterIot.Login(out string status);
            History.Add($"{nameof(Login)} : {status}");
            return tmp;
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            bool tmp = WaterIot.Logout(out string status);
            History.Add($"{nameof(Logout)} : {status}");
            return tmp;
        }

        /// <summary>
        /// 下載Sensor資料
        /// </summary>
        public void ListSensor()
        {
            var tmp = WaterIot.ListSensor(out string status);
            History.Add($"{nameof(ListSensor)} : {status}");
            SensorDevices = tmp.SelectMany(t => t.Sensors);
        }

        /// <summary>
        /// 建立Device
        /// </summary>
        public void CreateDevice()
        {
            bool result = true;
            foreach (SensorDevice item in SensorDevices)
            {
                var tmp = Devices.FirstOrDefault(t => t.DeviceID.Equals(item.DeviceID));
                if (tmp is null)
                {
                    tmp = new Device(item.DeviceID);
                    Devices.Add(tmp);
                }
                tmp.CName = item.CName;
                tmp.Latitude = item.Latitude;
                tmp.Longitude = item.Longitude;
                tmp.Name = item.Name;
                result = result & CreateRecords(tmp);
            }
            if (!result)
            {
                IsSendLineNotify = false;
            }
        }

        /// <summary>
        /// 建立Records
        /// </summary>
        /// <param name="device">設備</param>
        public bool CreateRecords(Device device)
        {
            List<SensorRecord> record = WaterIot.QuerySensor(out string status, device.DeviceID);
            History.Add($"{nameof(CreateRecords)} : {device.DeviceID} - {status}");
            SensorRecord last = record.LastOrDefault();

            if (last is null)
            {
                if (FakeDataEnabled)
                {
                    // 200820 .cbc* V1.0.1  修改若是null 則作假資料上報

                    last = new SensorRecord();
                    last.Create = DateTime.Now;
                    last.Water = 0;

                    device.CreateTime = last.Create;
                    var water = device.Records.FirstOrDefault(t => t.Name is "water");

                    water.Value = last.Water;
                    water.Parents = device;

                    if (IsSendLineNotify)
                    {
                        WaterIot.LineNorifyPostResponse("https://notify-api.line.me/api/notify",
                            $"\r\nName: {device.Name} \r\n" +
                            $"DeviceID: {device.DeviceID} \r\n" +
                            $"資料收集異常，模擬資料上傳\r\n" +
                            $"請找人員確認");
                        //IsSendLineNotify = false;
                        return false;
                    }
                }
                else
                {
                    if (IsSendLineNotify)
                    {
                        WaterIot.LineNorifyPostResponse("https://notify-api.line.me/api/notify",
                            $"\r\nName: {device.Name} \r\n" +
                            $"DeviceID: {device.DeviceID} \r\n" +
                            $"資料收集異常\r\n" +
                            $"請找人員確認");
                        //IsSendLineNotify = false;
                        return false;
                    }
                }
            }
            else
            {
                device.CreateTime = last.Create;
                var water = device.Records.FirstOrDefault(t => t.Name is "water");
                if (water is null)
                {
                    water = new Record("water");
                    device.Records.Add(water);
                }
                if (last.Water < 0) last.Water = 0;

                //// 200820 .cbc* V1.0.1  修改若是設備沒上報了 則作假資料上報
                //if ((DateTime.Now - device.CreateTime).TotalHours > 1)
                //{
                //	device.CreateTime = DateTime.Now;
                //	last.Water = 0;
                //}

                if (device.Name == "SHJ-0010" || device.Name == "SHJ-0011")
                {
                    water.Value = last.Water;
                }
                else
                {
                    water.Value = Math.Round(last.Water / 1000, 1); //SCC* 從客戶伺服器抓下來為 xxx.x cm ，傳給水利署是 m
                }

                water.Parents = device;
                return true;
            }
            return true;
        }

        //public IEnumerable<StationGroupInfo> StationGroupGetAll()
        //{
        //    if (UserApiClient.StationGroupGetAll(out IEnumerable<StationGroupInfo> stationGroupInfos, SenslinkInfoTypes.None) == System.Net.HttpStatusCode.OK)
        //    {
        //        foreach (StationGroupInfo cunStationGroupInfo in stationGroupInfos)
        //        {
        //            Console.WriteLine($"監測站群組 {cunStationGroupInfo.Name} - {cunStationGroupInfo.Id}");
        //        }
        //    }
        //    return stationGroupInfos;
        //}

        //public StationGroupInfo StationGroupGet(Guid id)
        //{
        //    if (UserApiClient.StationGroupGet(out StationGroupInfo stationGroupInfo, id, SenslinkInfoTypes.Station) == System.Net.HttpStatusCode.OK)
        //    {
        //        Console.Write($"監測站群組 {stationGroupInfo.Name}");
        //        return stationGroupInfo;
        //    }
        //    return null;
        //}

        //public IEnumerable<StationInfo> StationGetAll()
        //{
        //    if (UserApiClient.StationGetAll(out IEnumerable<StationInfo> stationInfos, SenslinkInfoTypes.PhysicalQuantity) == System.Net.HttpStatusCode.OK)
        //    {
        //        foreach (StationInfo cunInfo in stationInfos)
        //        {
        //            Console.WriteLine($"\r\n監測站 {cunInfo.Name}");

        //            if (cunInfo.LinkedPhysicalQuantities != null)
        //            {
        //                foreach (PhysicalQuantityInfo cunPqInfo in cunInfo.LinkedPhysicalQuantities)
        //                {
        //                    Console.WriteLine($"-{cunPqInfo.Name}");
        //                }
        //            }
        //        }
        //    }
        //    return stationInfos;
        //}

        //public StationInfo StationGet(Guid id)
        //{
        //    if (UserApiClient.StationGet(out StationInfo stationInfo, id, SenslinkInfoTypes.PhysicalQuantity) == System.Net.HttpStatusCode.OK)
        //    {
        //        Console.WriteLine($"監測站 {stationInfo.Name}");
        //        if (stationInfo.LinkedPhysicalQuantities != null)
        //        {
        //            foreach (PhysicalQuantityInfo cunPqInfo in stationInfo.LinkedPhysicalQuantities)
        //            {
        //                Console.WriteLine($"-{cunPqInfo.Name}");
        //            }
        //        }
        //    }
        //    return stationInfo;
        //}

        ///// <summary>
        ///// 6.7.1
        ///// </summary>
        ///// <returns></returns>
        //public IEnumerable<PhysicalQuantityInfo> PhysicalQuantityGetAll()
        //{
        //    if (UserApiClient.PhysicalQuantityGetAll(out IEnumerable<PhysicalQuantityInfo> physicalQuantityInfos, SenslinkInfoTypes.None) == System.Net.HttpStatusCode.OK)
        //    {
        //        foreach (PhysicalQuantityInfo pqInfo in physicalQuantityInfos)
        //        {
        //            Console.WriteLine($"物理量 {pqInfo.Name}");
        //        }
        //    }
        //    return physicalQuantityInfos;
        //}

        //public PhysicalQuantityInfo PhysicalQuantityGet(Guid id)
        //{
        //    if (UserApiClient.PhysicalQuantityGet(out PhysicalQuantityInfo pqInfo, id, SenslinkInfoTypes.None) == System.Net.HttpStatusCode.OK)
        //    {
        //        Console.WriteLine($"物理量 {pqInfo.Name}");
        //    }

        //    return pqInfo;
        //}

        //public IEnumerable<DataPoint> LastestDataReadAll()
        //{
        //    if (UserApiClient.LatestDataReadAll(out IEnumerable<DataPoint> dataPoints) == System.Net.HttpStatusCode.OK)
        //    {
        //        foreach (DataPoint cunDp in dataPoints)
        //        {
        //            Console.WriteLine($"{cunDp.Id} {cunDp.TimeStamp} {cunDp.Value}");
        //        }
        //    }
        //    return dataPoints;
        //}

        //public DataPoint LatestDataReadPhysicalQuantity(Guid id)
        //{
        //    if (UserApiClient.LastestDataReadPhysicalQuantity(out DataPoint dataPoint, id) == System.Net.HttpStatusCode.OK)
        //    {
        //        Console.WriteLine($"{id} {dataPoint.TimeStamp} {dataPoint.Value}");
        //    }
        //    return dataPoint;
        //}

        //public IEnumerable<DataPoint> LastestDataReadStation(Guid id)
        //{
        //    if (UserApiClient.LastestDataReadStation(out IEnumerable<DataPoint> dataPoints, id) == System.Net.HttpStatusCode.OK)
        //    {
        //        foreach (DataPoint cunDp in dataPoints)
        //        {
        //            Console.WriteLine($"{cunDp.Id} {cunDp.TimeStamp} {cunDp.Value}");
        //        }
        //    }
        //    return dataPoints;
        //}

        //public IEnumerable<DataPoint> LastestDataReadStationGroup(Guid id)
        //{
        //    if (UserApiClient.LatestDataReadStationGroup(out IEnumerable<DataPoint> dataPoints, id) == System.Net.HttpStatusCode.OK)
        //    {
        //        foreach (DataPoint cunDp in dataPoints)
        //        {
        //            Console.WriteLine($"{cunDp.Id} {cunDp.TimeStamp} {cunDp.Value}");
        //        }
        //    }
        //    return dataPoints;
        //}

        //public DataSeriesEs TimeSeriesDataReadAggregratedData(Guid id)
        //{
        //    DateTimeOffset eDt = DateTimeOffset.Now;
        //    DateTimeOffset sDt = eDt.AddDays(-7);
        //    if (UserApiClient.TimeSeriesDataReadAggregateData(out DataSeriesEs dataSeriesEs, id, sDt, eDt, AggregateCalculationMethods.avg, 600, 480) == System.Net.HttpStatusCode.OK)
        //    {
        //        Console.WriteLine($"{dataSeriesEs.Id}");
        //        Console.WriteLine($"{dataSeriesEs.StartTimeStamp}");

        //        foreach (DataPoint dp in dataSeriesEs.ToDataPointArray(true))
        //        {
        //            Console.WriteLine($"{dp.TimeStamp} {dp.Value}");
        //        }
        //    }
        //    return dataSeriesEs;
        //}

        //public DataSeriesUs TimeSeriesDataReadRawData(Guid id)
        //{
        //    DateTimeOffset eDt = DateTimeOffset.Now;
        //    DateTimeOffset sDt = eDt.AddDays(-7);
        //    if (UserApiClient.TimeSeriesDataReadRawData(out DataSeriesUs dataSeriesUs, id, sDt, eDt, true, 480) == System.Net.HttpStatusCode.OK)
        //    {
        //        Console.WriteLine($"{dataSeriesUs.Id}");
        //        foreach (DataPoint dp in dataSeriesUs.DataPoints)
        //        {
        //            Console.WriteLine($"{dp.TimeStamp} {dp.Value}");
        //        }
        //    }
        //    return dataSeriesUs;
        //}

        #region ETL API - 歷史資料

        public bool TimeSeriesDataWrite(params Record[] records)
        {
            try
            {
                var tmp = records.Select(t => new DataPoint
                {
                    //Id = t.Guid,
                    TimeStamp = t.Parents.CreateTime,
                    Value = Convert.ToDouble(t.Value)
                });

                if (tmp.Count() == 0)
                {
                    return false;
                }

                string message = JsonConvert.SerializeObject(tmp, Formatting.Indented);

                if (MQTTClient.Send(Config.Topic, message) == true)
                {
                    Console.WriteLine($"Upload OK : {string.Join(",", tmp.Select(t => t.Id))}");
                    foreach (var item in records)
                    {
                        item.LastUploadTime = item.Parents.CreateTime;
                        //item.UploadValue = item.Value;
                    }
                    return true;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion ETL API - 歷史資料
    }
}