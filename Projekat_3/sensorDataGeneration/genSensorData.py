
import requests
import json
import random
import time
import os
import csv

edgexip = '127.0.0.1'
aTempList = list()
mTempList = list()
irrList = list()
timeList = list()

cwd = os.getcwd()  # Get the current working directory (cwd)
files = os.listdir(cwd)  # Get all the files in that directory
print("Files in %r: %s" % (cwd, files))

with open("/mnt/c//Users/Jelena/Desktop/SOA/Projekat_3/sensorDataGeneration/data.csv", "r") as file:
    csv_reader = csv.DictReader(file)
    for row in csv_reader:
        if row is not None :
            aTempList.append(row['AMBIENT_TEMPERATURE'])
            mTempList.append(row['MODULE_TEMPERATURE'])
            irrList.append(row['IRRADIATION'])
            timeList.append(row['DATE_TIME'])

def generateSensorData():
    print("Sending values: Irradiation")
    
    i = random.randint(1, 3183)
    aTemp = round(float(aTempList[i]))
    mTemp = round(float(mTempList[i]))
    irr = round(float(irrList[i]))
    timee = timeList[i]

    print("Sending values: Irradiation %s, Temperature %sC, Module Temperature %sC" % (irr, aTemp, mTemp))

    return (irr, aTemp, mTemp)



if __name__ == "__main__":

    sensorTypes = ["irradiation", "temperature", "moduleTemperature"]

    while(1):

        (irradiation, aTemp, mTemp) = generateSensorData()
        
        url = 'http://%s:49986/api/v1/resource/Sensor_cluster_01/temperature' % edgexip
        payload = mTemp
        headers = {'content-type': 'application/json'}
        response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)

        url = 'http://%s:49986/api/v1/resource/Sensor_cluster_01/moduleTemperature' % edgexip
        payload = aTemp
        headers = {'content-type': 'application/json'}
        response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)

        url = 'http://%s:49986/api/v1/resource/Sensor_cluster_01/irradiation' % edgexip
        payload = irradiation
        headers = {'content-type': 'application/json'}
        response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)

        time.sleep(5)
