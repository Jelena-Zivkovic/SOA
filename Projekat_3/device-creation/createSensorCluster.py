import requests, json, sys, re, time, os, warnings, argparse
from requests_toolbelt.multipart.encoder import MultipartEncoder
from datetime import datetime

warnings.filterwarnings("ignore")

# Gather information from arguments
parser=argparse.ArgumentParser(description="Python script for creating a new device from scratch in EdgeX Foundry")
parser.add_argument('-ip',help='EdgeX Foundry IP address', required=True)

args=vars(parser.parse_args())

edgex_ip=args["ip"]


# Value descriptors are what they sound like: Describing data values
# For this use case temperature and irradiation are the two value types required
# Note that these correspond to the same values in the device profile YAML file
def createValueDescriptors():
    url = 'http://%s:48080/api/v1/valuedescriptor' % edgex_ip

    payload =   {
                    "name":"irradiation",
                    "description":"Irradiation in kW/m²", 
                    "min":"0",
                    "max":"2",
                    "type":"Float32",
                    "uomLabel":"count",
                    "defaultValue":"0",
                    "formatting":"%s",
                    "labels":["environment","irradiation"]
                }
    headers = {'content-type': 'application/json'}
    response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)
    print("Result for createValueDescriptors #1: %s - Message: %s" % (response, response.text))


    payload =   {
                    "name":"temperature",
                    "description":"Ambient temperature in Celcius", 
                    "min":"-50",
                    "max":"100",
                    "type":"Float32",
                    "uomLabel":"count",
                    "defaultValue":"0",
                    "formatting":"%s",
                    "labels":["environment","temperature"]
                }
    headers = {'content-type': 'application/json'}
    response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)
    print("Result for createValueDescriptors #2: %s - Message: %s" % (response, response.text))

    payload =   {
                    "name":"moduleTemperature",
                    "description":"Module temperature in Celcius", 
                    "min":"-50",
                    "max":"100",
                    "type":"Float32",
                    "uomLabel":"count",
                    "defaultValue":"0",
                    "formatting":"%s",
                    "labels":["environment","temperature"]
                }
    headers = {'content-type': 'application/json'}
    response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)
    print("Result for createValueDescriptors #3: %s - Message: %s" % (response, response.text))



# To create a device we need a device profile in YAML format. This function uploads and registers
# the device profile with EdgeX Foundry. Based on the content of the device profile, EdgeX Foundry
# may create entries for the device in the command module as well as meta data. Our device doesn't 
# support commands so the command module will not be updated for this use case
def uploadDeviceProfile():
    multipart_data = MultipartEncoder(
        fields={
                'file': ('sensorClusterDeviceProfile.yaml', open('sensorClusterDeviceProfile.yaml', 'rb'), 'text/plain')
               }
        )

    url = 'http://%s:48081/api/v1/deviceprofile/uploadfile' % edgex_ip
    response = requests.post(url, data=multipart_data,
                      headers={'Content-Type': multipart_data.content_type})

    print("Result of uploading device profile: %s with message %s" % (response, response.text))



# Finally we can create the actual device. It will be named and will also reference both the 
# device service it supports as well as the device profile it corresponds to
# The device creation requires a protocols section. Perhaps it can be expanded to include
# information about the device, like IP, port, etc. but isn't actively used for these tutorials
def addNewDevice():
    url = 'http://%s:48081/api/v1/device' % edgex_ip

    payload =   {
                    "name":"Sensor_cluster_01",
                    "description":"Raspberry Pi sensor cluster",
                    "adminState":"unlocked",
                    "operatingState":"enabled",
                    "protocols": {
                        "example": {
                        "host": "dummy",
                        "port": "1234",
                        "unitID": "1"
                        }
                    },
                    "labels": ["Irradiation sensor","Temperature sensor","DHT11", "Module temperature sensor"],
                    "location":"Nis",
                    "service": {
                        "name":"edgex-device-rest"
                    },
                    "profile": {
                        "name":"SensorCluster01"
                    }
                }
    headers = {'content-type': 'application/json'}
    response = requests.post(url, data=json.dumps(payload), headers=headers, verify=False)
    print("Result for addNewDevice: %s - Message: %s" % (response, response.text))



if __name__ == "__main__":
    createValueDescriptors()
    uploadDeviceProfile()
    addNewDevice()
