const express = require('express');
const mqtt = require('mqtt');
const fs = require('fs');
const csv = require('csv-parser');

// Create an Express app
const app = express();

// Define MQTT settings
const mqttOptions = {
    host: 'mqtt://test.mosquitto.org', // MQTT broker host
    port: 1883,                        // MQTT broker port
};

// Create an MQTT client
const mqttClient = mqtt.connect(mqttOptions);

// Function to send sensor data via MQTT
function sendSensorDataToMQTT(data) {
    const topic = 'sensor/data'; // MQTT topic to publish data

    mqttClient.publish(topic, JSON.stringify(data), (err) => {
        if (err) {
            console.error(`Error sending data via MQTT: ${err}`);
        } else {
            console.log(`Data sent via MQTT: ${JSON.stringify(data)}`);
        }
    });
}

// Read data from the CSV file and send it
fs.createReadStream('Plant_1_Weather_Sensor_Data.csv')
    .pipe(csv())
    .on('data', (row) => {
        // Assuming the CSV columns are named 'timestamp', 'temperature', and 'humidity'
        const sensorData = {
            DATE_TIME: row.DATE_TIME,
            PLANT_ID: parseInt(row.PLANT_ID),
            SOURCE_KEY: row.SOURCE_KEY,
            AMBIENT_TEMPERATURE: parseFloat(row.AMBIENT_TEMPERATURE),
            MODULE_TEMPERATURE: parseFloat(row.MODULE_TEMPERATURE),
            IRRADIATION: parseFloat(row.IRRADIATION)
        };

        sendSensorDataToMQTT(sensorData);
    })
    .on('end', () => {
        console.log('All data from CSV file sent to MQTT');
    });

// Start the Express server
const port = 3000; // Change to your desired port
app.listen(port, () => {
    console.log(`Dummy sensor is running. HTTP server is listening on port ${port}`);
});