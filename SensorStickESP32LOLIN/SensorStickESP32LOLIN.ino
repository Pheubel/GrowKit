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

//WiFi settings
#define SETSSID "DSS_2"
#define PW "DSSwifi020!"
#define PORT 10000
#define HOST "192.168.0.105"
#define ID "1:"

//LED settings
#define NUM_LEDS 3
#define LEDPIN 4

WiFiClient client;
WiFiServer wifiServer(10000);

bool profileLoaded = false;

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

bool BLEServer;

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

enum LEDStates
{
  OFF,
  ON,
  BLINK,
  PULSE
};

enum LEDStates LEDState;


I2CSoilMoistureSensor sensor;

void setup()
{
  Wire.begin(15, 2);
  Serial.begin(115200);
  //pinMode(BATLED, OUTPUT);
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

void sendData(String dataPacket)
{
  WiFiClient client = wifiServer.available();

  if (!client.connect(HOST, PORT))
  {
    Serial.println("Connection to HUB failed");
  }
  else
  {
    delay(500);
    Serial.println("Connected to HUB! Sending data..");
    client.print(dataPacket);
    Serial.println("Data sent, disconnecting!");
    client.stop();
  }
}

void loop()
{
  sensorMillis = millis();
  LEDMillis = millis();


  //LEDTest();

  //if (connectWiFi())
  //  sendData(dataPacket);
  if (sensorMillis - p_sensorMillis >= sensorInterval)
  {
    p_sensorMillis = sensorMillis;
    startSensors();
    checkStatus();
  }
  if (LEDMillis - p_LEDMillis >= LEDInterval)
  {
    p_LEDMillis = LEDMillis;
    Serial.println("LEDS: ");
    Serial.print("Light: ");
    Serial.print(leds[0].r);
    Serial.print(leds[0].g);
    Serial.println(leds[0].b);
    Serial.print("Moisture: ");
    Serial.print(leds[1].r);
    Serial.print(leds[1].g);
    Serial.println(leds[1].b);
    Serial.print("Temperature: ");
    Serial.print(leds[2].r);
    Serial.print(leds[2].g);
    Serial.println(leds[2].b);
    FastLED.show();
  }
  
}
