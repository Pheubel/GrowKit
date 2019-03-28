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

//First RGB LED test, GPIO B25, R26, G32

//LIGHT LED
#define redLED 32
#define greenLED 26
#define blueLED 25

#define LEDCHANNEL 0
#define LEDCHANNEL1 1
#define LEDCHANNEL2 2

//TEMPERATURE LED
#define redLED1 14
#define greenLED1 27
#define blueLED1 33

#define LEDCHANNEL3 3
#define LEDCHANNEL4 4
#define LEDCHANNEL5 5

//MOISTURE LED
#define redLED2 2
#define greenLED2 13
#define blueLED2 15

//BATTERy LED
#define BATLED 0

#define LEDCHANNEL6 6
#define LEDCHANNEL7 7
#define LEDCHANNEL8 8

#define LEDFREQ 5000
#define LEDRES 8

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
  pinMode(BATLED, OUTPUT);

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
  int data[2];
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

  String moistureStr = String(data[1], DEC);
  String temperatureStr = String(data[2], DEC);
  String lightStr = String(data[3], DEC);

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

// Check if the 3 parameters exceed acceptable values
void checkStatus(String dataPacket)
{
  Serial.print("Checking data status!");
}

//Set the parameters depending on what kind of plant. Data received from Hub
void setParameters(int lightMin, int lightMax, int moistureMin, int moistureMax, int temperatureMin, int temperatureMax, int moistureTime)
{
  
}
//Pulse the LEDs
void ledPulse(int r, int g, int b, int id)
{
  int LED;
  int LED1;
  int LED2;

  if (id == 1)
  {
    LED = LEDCHANNEL;
    LED1 = LEDCHANNEL1;
    LED2 = LEDCHANNEL2;
  }

  else if (id == 2)
  {
    LED = LEDCHANNEL3;
    LED1 = LEDCHANNEL4;
    LED2 = LEDCHANNEL5;
  }

  else
  {
    LED = LEDCHANNEL6;
    LED1 = LEDCHANNEL7;
    LED2 = LEDCHANNEL8;
  }

  for (float fade = 1.0; fade >= 0.0; fade -= 0.002)
  {
    ledcWrite(LED, (float) r * fade);
    ledcWrite(LED1, (float) g * fade);
    ledcWrite(LED2, (float) b * fade);
    delay(10);
  }
}

//Set the PWM settings for the RGB LEDs
void ledSetup()
{
  //LIGHT
  ledcSetup(LEDCHANNEL, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL1, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL2, LEDFREQ, LEDRES);

  ledcAttachPin(redLED, LEDCHANNEL);
  ledcAttachPin(greenLED, LEDCHANNEL1);
  ledcAttachPin(blueLED, LEDCHANNEL2);

  //TEMPERATURE

  ledcSetup(LEDCHANNEL3, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL4, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL5, LEDFREQ, LEDRES);

  ledcAttachPin(redLED1, LEDCHANNEL3);
  ledcAttachPin(greenLED1, LEDCHANNEL4);
  ledcAttachPin(blueLED1, LEDCHANNEL5);

  //MOISTURE

  ledcSetup(LEDCHANNEL6, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL7, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL8, LEDFREQ, LEDRES);

  ledcAttachPin(redLED2, LEDCHANNEL6);
  ledcAttachPin(greenLED2, LEDCHANNEL7);
  ledcAttachPin(blueLED2, LEDCHANNEL8);


}

void loop()
{
  ledSetup();
  digitalWrite(BATLED, HIGH);
  dataPacket = startSensors();
  checkStatus(dataPacket);
//  if (connectWiFi())
//    sendData(dataPacket);
}
