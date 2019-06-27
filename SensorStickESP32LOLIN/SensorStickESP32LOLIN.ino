#include <Wire.h>
#include <I2CSoilMoistureSensor.h>
#include <WiFi.h>
#include <FastLED.h>
#include <ArduinoJson.h>
#include "LED.h"

#include <BLEDevice.h>
#include <BLEUtils.h>
#include <BLEScan.h>
#include <BLEAdvertisedDevice.h>

#define ZERO 0
#define CELSIUSOFFSET 10

#define SDAPIN 15
#define SCLPIN 2

//WiFi settings
#define SETSSID "DSS_2"
#define PW "DSSwifi020!"
#define PORT 5000
#define HOST "https://vps29.dss.cloud"
#define ID "1:"
#define HTTPPOST "http://vps29.dss.cloud/api/UpdateStick"

//LED settings
#define NUM_LEDS 3
#define LEDPIN 4

WiFiClient client;
WiFiServer wifiServer(10000);

bool profileLoaded = false;
bool blinkPhase = false;

int light;
int moisture;
int temperature;
int identifier;
int counter = 0;

//Delay variables
unsigned long p_LEDMillis = 0;
unsigned long p_sensorMillis;
unsigned long LEDMillis;
unsigned long sensorMillis;
const long LEDInterval = 500;
const long sensorInterval = 1000;

//bool BLEServer;

String dataPacket;

//LED Colours
CRGB lightCol(100, 100, 0);
CRGB moistCol(0, 50, 100);
CRGB tempCol(100, 0, 0);

LED lightLED(lightCol);
LED moistLED(moistCol);
LED tempLED(tempCol);

CRGB leds[NUM_LEDS];

void LEDSetup()
{
  FastLED.addLeds<NEOPIXEL, LEDPIN>(leds, NUM_LEDS);
}

I2CSoilMoistureSensor sensor;

void setup()
{
  Wire.begin(SDAPIN, SCLPIN);
  Serial.begin(115200);
  FastLED.addLeds<NEOPIXEL, LEDPIN>(leds, NUM_LEDS);

  sensor.begin(); // reset sensor
  delay(1000); // give some time to boot up
  Serial.print("I2C Soil Moisture Sensor Address: ");
  Serial.println(sensor.getAddress(), HEX);
  Serial.print("Sensor Firmware version: ");
  Serial.println(sensor.getVersion(), HEX);
  Serial.println();

  //BLEServerSetup();
  //Makes sure the I2C sensors' address are set to 0x21
  Serial.print("Change address to 0x21 ...");
  if (sensor.setAddress(0x21, true)) // set Sensor Address to 0x21 and reset
    Serial.println("... DONE");
  else
    Serial.println("... ERROR");
  Serial.println("<<<<<< SETUP DONE >>>>>>");
}

bool connectWiFi()
{
  Serial.println("<<<<<< CONNECTING WIFI >>>>>>");
  Serial.println("Connecting for data transmission...");
  Serial.print("SSID: ");
  Serial.println(SETSSID);
  Serial.print("Port: ");
  Serial.println(PORT);
  Serial.println();
  WiFi.disconnect(true);
  WiFi.mode(WIFI_STA);
  WiFi.begin(SETSSID, PW);
  while (WiFi.status() != WL_CONNECTED)
  {
    delay(500);
    Serial.println(WiFi.status());

    counter++;
    if (counter >= 60)
    {
      ESP.restart();
      return false;
    }
    if (WiFi.status() == 1)
    {
      Serial.println(PW);
      Serial.println(SETSSID);
    }
  }
  Serial.println();
  Serial.println("WiFi connected");
  Serial.print("IP address set: ");
  Serial.println(WiFi.localIP());
  return true;
}

void connectServer()
{
  WiFiClient client = wifiServer.available();
  if (!client.connect(HOST, PORT))
  {
    Serial.println("Connection to web server");
  }
  else if (client.connect(HOST, PORT))
  {
    delay(500);
  }
  //void sendData(client, startSensor());
}

void sendData(WiFiClient wifiServer)
{

  Serial.println("Connected to webserver! Sending data..");
  client.print(HTTPPOST);
  //client.print(encodeJSON());
  Serial.println("Data sent, disconnecting!");
  client.stop();
}
}

void loop()
{
  sensorMillis = millis();
  LEDMillis = millis();

  if (connectWiFi())
    connectServer();

  //Avoid using Delay() too much, it will make the blinking of the LEDs slower.

  if (sensorMillis - p_sensorMillis >= sensorInterval)
  {
    p_sensorMillis = sensorMillis;
    startSensors();
    checkStatus();
  }
  if (LEDMillis - p_LEDMillis >= LEDInterval)
  {
    p_LEDMillis = LEDMillis;
    FastLED.show();

    if (blinkPhase)
      blinkPhase = false;
    else
      blinkPhase = true;
  }

}
