#include <Wire.h>
#include <I2CSoilMoistureSensor.h>
#include <WiFi.h>
#include <FastLED.h>
#include <ArduinoJson.h>

#include <BLEDevice.h>
#include <BLEUtils.h>
#include <BLEScan.h>
#include <BLEAdvertisedDevice.h>

#define ZERO 0
#define CELSIUSOFFSET 10

#define SETSSID "DSS_2"
#define PW "DSSwifi020!"
#define PORT 10000
#define HOST "192.168.0.105"
#define ID "1:"

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

unsigned long previousMillis = 0;
unsigned long currentMillis;
const long interval = 1000;

bool BLEServer;

String dataPacket;

CRGB lightCol(150,150,0);
CRGB moistCol(0,50,150);
CRGB tempCol(150,0,0);

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
  FastLED.addLeds<NEOPIXEL, DATAPIN>(leds, NUM_LEDS);

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
  currentMillis = millis();
//  dataPacket = startSensors();
//  checkStatus();
  //LEDTest();
  
  //if (connectWiFi())
  //  sendData(dataPacket);
  if(currentMillis - previousMillis >= interval)
  {
    
    previousMillis = currentMillis;
    FastLED.show();
  }
  
  
  Serial.println("<<<<<< DATA PACKET >>>>>>");  
  Serial.println(dataPacket);
}
