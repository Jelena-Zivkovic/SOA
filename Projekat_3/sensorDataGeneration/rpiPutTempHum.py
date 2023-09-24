import sys, time, requests, json, Adafruit_DHT

edgexip = "<edgex ip>"

while True:

    # Update to match DHT sensor type and GPIO pin
    # DHT11 and GPIO pin 6 used in example
    rawHum, rawTmp = Adafruit_DHT.read_retry(11, 6)

    urlTemp = 'http://%s:49986/api/v1/resource/Sensor_cluster_01/temperature' % edgexip
    urlMTemp  = 'http://%s:49986/api/v1/resource/Sensor_cluster_01/irradiation' % edgexip
    urlIrr = 'http://%s:49986/api/v1/resource/Sensor_cluster_01/moduleTemperature' % edgexip

    humval  = str(urlMTemp)
    tempval = str(rawTmp)
    humval  = str(rawHum)

    headers = {'content-type': 'application/json'}

    if(float(humval) < 100):
        response = requests.post(urlTemp, data=json.dumps(int(rawTmp)), headers=headers,verify=False)
        response = requests.post(urlHum, data=json.dumps(int(rawHum)), headers=headers,verify=False)

        print("Temp: %s\N{DEGREE SIGN}C, humidity: %s%%" % (tempval, humval))


    time.sleep(2)
