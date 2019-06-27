#include <Wire.h>
#include <I2CSoilMoistureSensor.h>
#include <WiFi.h>

#define ZERO 0
#define CELSIUSOFFSET 10

#define SETSSID "DSS_2"
#define PW "DSSwifi020!"
#define PORT 10000
#define HOST "192.168.0.105"
#define ID "1:"

WiFiClient client;
WiFiServer wifiServer(10000);

int light;
int moisture;
int temperature;

int identifier;

int counter = 0;

String dataPacket;

I2CSoilMoistureSensor sensor;

void setup()
{
  Wire.begin();
  Serial.begin(9600);

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
}

String startSensors()
{
  sensor.begin();
  delay(5000);
  sensor.startMeasureLight();
  delay(3000);
  Serial.print("Temperature: ");
  Serial.println(temperature = sensor.getTemperature() / CELSIUSOFFSET);
  Serial.print("Moisture: ");
  Serial.println(moisture = sensor.getCapacitance());
  Serial.print("Light: ");
  Serial.println(light = sensor.getLight());
  Serial.println();
  sensor.sleep();

  moisture = map(moisture, 200, 722, 0, 100);
  temperature = map(temperature, 255, 305, 0, 100);
  light = map(light, 10000, 0, 0, 100);

  if ( moisture < 0)
    moisture = 0;

  if (light > 10000)
    light = 10000;

  if (light < 0)
    light = 0;

  if (temperature > 100)
    temperature = 100;

  String moistureStr = String(moisture, DEC);
  String temperatureStr = String(temperature, DEC);
  String lightStr = String(light, DEC);



  Serial.println(ID + moistureStr + ":" + lightStr + ":" + temperatureStr);
  return (ID + moistureStr + ":" + lightStr + ":" + temperatureStr);
}

bool connectWiFi()
{
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
  dataPacket = startSensors();
  if (connectWiFi())
    sendData(dataPacket);
}
