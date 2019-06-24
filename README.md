# 'Groene Vingers'
The 'Groene Vingers', or Green Thumbs in English, are an ESP32 Microcontroller with a set of sensors that sense ground moisture, the temperature of the area and light exposure levels.
This code is for the ESP32 Wemos Lolin32 Lite in combination with the I2C Soil Moisture Sensor by Catnip Technologies.

## Installation
In order to run the code you need the Arduino IDE or the Arduino extension for VisualStudio. 

The following libraries are required:
*[ArduinoJson 6](https://arduinojson.org/)
*[I2CSoilMoistureSensor](https://github.com/Apollon77/I2CSoilMoistureSensor)
*[FastLED](https://github.com/FastLED/FastLED)
*[ESP32 BLE Arduino](https://github.com/nkolban/ESP32_BLE_Arduino)

The Arduino IDE doesn't have ESP32 boards included, this needs to be added manually.
ESP32 boards can be found at this `https://dl.espressif.com/dl/package_esp32_index.json` and needs to be added to the Arduino IDE's preferences
![Image](https://i.imgur.com/997Bp8s.png)

## Usage Tips
When you change the pins the SCL and SDA wires of the I2C Soil Moisture Sensor on the ESP32 board, make sure you change `#define SDAPIN` and `#define SCLPIN` values.
Otherwise the microcontroller won't detect them.




 