using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using MQTTnet.Client.Connecting;
using Newtonsoft.Json;
using System.Net;

namespace WaterIot.OthersServices
{
    public class MQTTClient : IMqttClientConnectedHandler, IUploadEnable
    {
        IMqttClient MqttClient { get; set; }
        IMqttClientOptions Options { get; set; }
        public event EventHandler ConnectedHandler;
        public event EventHandler DisconnectedHandler;
        public bool IsConnected => MqttClient.IsConnected;

        public MQTTClient(string IP)
        {        
            Options = new MqttClientOptionsBuilder()
                    .WithClientId($"{Convert.ToBase64String(Guid.NewGuid().ToByteArray())}")
                    .WithTcpServer(IP, 1883)
                    .WithCredentials("scsa", "24900184")
                    .WithKeepAlivePeriod(new TimeSpan(200000000))
                    .WithCommunicationTimeout(TimeSpan.FromSeconds(3000))
                    .WithCleanSession()
                    .WithProtocolVersion(MQTTnet.Formatter.MqttProtocolVersion.V311)
                    .Build();
            MqttClient = new MqttFactory().CreateMqttClient();
            MqttClient.UseDisconnectedHandler(args =>
            {
                DisconnectedHandler.Invoke(this, null);
            });

            MqttClient.UseConnectedHandler(args =>
            {
                ConnectedHandler.Invoke(this, null);
            });
        }

        public async Task<bool> Connect()
        {
            try
            {
                var result = await MqttClient.ConnectAsync(Options);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Disconnect()
        {
            try
            {
                await MqttClient.DisconnectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Send(string Topic, string message)
        {
            var payload = Encoding.UTF8.GetBytes(message);

            var result = MqttClient.PublishAsync(new MqttApplicationMessage
            {
                Topic = Topic,
                QualityOfServiceLevel = MQTTnet.Protocol.MqttQualityOfServiceLevel.AtMostOnce,
                Retain = true,
                Payload = payload,
            });

            return result.Result.ReasonCode == MQTTnet.Client.Publishing.MqttClientPublishReasonCode.Success;
        }

        public Task HandleConnectedAsync(MqttClientConnectedEventArgs eventArgs)
        {
            return null;
            //throw new NotImplementedException();
        }


    }
}
