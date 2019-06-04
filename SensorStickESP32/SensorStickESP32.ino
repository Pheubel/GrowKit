#include <Wire.h>
#include <I2CSoilMoistureSensor.h>
#include <WiFi.h>
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

WiFiClient client;
WiFiServer wifiServer(10000);

bool profileLoaded = false;

int light;
int moisture;
int temperature;
int identifier;
int counter = 0;

bool BLEServer;

String dataPacket;

enum ledStates
{
  BLINK,
  PULSE,
  PULSE_ALL,
  ON,
  OFF,
  TEST
};

enum ledStates ledState;


I2CSoilMoistureSensor sensor;

void setup()
{
  Wire.begin();
  Serial.begin(115200);
  //pinMode(BATLED, OUTPUT);
  ledSetup();

  sensor.begin(); // reset sensor
  delay(1000); // give some time to boot up
  Serial.print("I2C Soil Moisture Sensor Address: ");
  Serial.println(sensor.getAddress(), HEX);
  Serial.print("Sensor Firmware version: ");
  Serial.println(sensor.getVersion(), HEX);
  Serial.println();

  //Makes sure the I2C sensors' address are set to 0x21
  Serial.print("Change address to 0x21 ...");
  if (sensor.setAddress(0x21, true)) // set Sensor Address to 0x21 and reset
    Serial.println("... DONE");
  else
    Serial.println("... ERROR");
  Serial.println();
  Serial.println("3 Pulse test");
  ledActivation(0, 255, 0, 3, PULSE_ALL);
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
      ledActivation(255, 0, 0, 1, BLINK);
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
  dataPacket = startSensors();
  //checkStatus();

  //if (connectWiFi())
  //  sendData(dataPacket);
  //Serial.println("<<<<<< DATA PACKET >>>>>>");  
  //Serial.println(dataPacket);
}
