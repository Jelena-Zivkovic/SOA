using MQTTnet.Client;
using MQTTnet;

namespace AnalyticsService.Service
{
    public class AnalyticsServiceMqtt
    {
        private static AnalyticsServiceMqtt instance;
        private static IMqttClient mqttClient;
        private static object lockObj = new object();
        public static AnalyticsServiceMqtt Instance()
        {
            if (mqttClient == null)
            {
                lock (lockObj)
                {
                    if (mqttClient == null)
                    {
                        instance = new AnalyticsServiceMqtt();
                    }
                }
            }

            return instance;
        }

        private AnalyticsServiceMqtt()
        {
            var mqttFactory = new MqttFactory();
            mqttClient = mqttFactory.CreateMqttClient();
        }

        public async Task ConnectAsync(string brokerAddress, int port)
        {
            var mqttClientOptions = new MqttClientOptionsBuilder()
                                        .WithTcpServer(brokerAddress, port)
            .Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            Console.WriteLine("The MQTT client is connected.");
        }

        public async Task SubsribeToTopicsAsync(List<string> topics)
        {
            topics.ForEach(async t => await mqttClient.SubscribeAsync(t));
            Console.WriteLine("MQTT client subscribed to topics.");
        }

        public async Task PublishMessage(string topic, string payload)
        {
            var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(payload)
                .Build();

            await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            Console.WriteLine("MQTT client publibshed message.");
        }

        public void AddApplicationMessageReceived(Func<MqttApplicationMessageReceivedEventArgs, Task> callback)
        {
            mqttClient.ApplicationMessageReceivedAsync += callback;
        }
    }
}
