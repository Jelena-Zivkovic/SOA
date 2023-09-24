using AnalyticsService;
using AnalyticsService.Service;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var sensorDummyTopic = "sensor_dummy/values";
        var eKuiperTopic = "eKuiper/anomalies"; // broker.emqx.io
        string address = "tcp://test.mosquitto.org";//"10.14.42.11";
        var port = 1883;
        var client = InfluxDBClientFactory.Create(url: "http://127.0.0.1:8086", "admin", "adminadmin123".ToCharArray());
        int i = 1;

        var mqttService = AnalyticsServiceMqtt.Instance();

        await mqttService.ConnectAsync(address, port);
        await mqttService.SubsribeToTopicsAsync(new List<string> { sensorDummyTopic, eKuiperTopic });
        async Task ApplicationMessageRecievedAsync(MqttApplicationMessageReceivedEventArgs e)
        {
            string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            if (e.ApplicationMessage.Topic == sensorDummyTopic)
            {
                mqttService.PublishMessage("analytics/values", payload);
                return;
            }
            Console.WriteLine($"eKuiper send: {payload}");
            var data = (JObject)JsonConvert.DeserializeObject(payload);
            Console.WriteLine($"Podaci sa eKuiper-a: {data}");
            string dateTime = data.SelectToken("DATE_TIME")?.Value<string>();
            string plId = data.SelectToken("PLANT_ID")?.Value<string>();
            string ipSrc = data.SelectToken("SOURCE_KEY")?.Value<string>();
            string aTemp = data.SelectToken("AMBIENT_TEMPERATURE")?.Value<string>();
            string mTemp = data.SelectToken("MODULE_TEMPERATURE").Value<string>();
            string irr = data.SelectToken("IRRADIATION")?.Value<string>();

            await WriteToDatabase(dateTime, plId, ipSrc, aTemp, mTemp, irr);
        }

        async Task WriteToDatabase(string dateTime, string plId, string ipSrc, string aTemp, string mTemp, string irr)
        {
            var point = PointData
                .Measurement("generation")
                .Tag("frame_number", dateTime)
                .Field("frame_len", plId)
                .Field("frame_time", ipSrc)
                .Field("eth_src", aTemp)
                .Field("eth_dst", mTemp)
                .Field("eth_dst", irr)
                // .Tag("timestamp", timestamp.ToString())
                // // .Field("id", ID)
                // .Field("temperature", temperature)
                // .Field("humidity", humidity)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
            Console.WriteLine($"Write in InfluxDb CHECK");

            await client.GetWriteApiAsync().WritePointAsync(point, "jelena", "organization");
            Console.WriteLine($"Write in InfluxDb: log{i}");
            i++;
        }

        mqttService.AddApplicationMessageReceived(ApplicationMessageRecievedAsync);

        while (true) ;
    }
}