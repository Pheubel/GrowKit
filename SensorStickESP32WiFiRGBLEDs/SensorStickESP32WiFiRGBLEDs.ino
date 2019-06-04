#include <Wire.h>
#include <I2CSoilMoistureSensor.h>
#include <WiFi.h>
#include <ArduinoJson.h>

#define ZERO 0
#define CELSIUSOFFSET 10

#define SETSSID "DSS_2"
#define PW "DSSwifi020!"
#define PORT 10000
#define HOST "192.168.0.105"
#define ID "1:"

//LIGHT LED
#define redLED 15
#define greenLED 13
#define blueLED 2

#define LEDCHANNEL 0
#define LEDCHANNEL1 1
#define LEDCHANNEL2 2

//MOISTURE LED
#define redLED1 27
#define greenLED1 14
#define blueLED1 33

#define LEDCHANNEL3 3
#define LEDCHANNEL4 4
#define LEDCHANNEL5 5

//TEMPERATURE LED
#define redLED2 26
#define greenLED2 25
#define blueLED2 32

//BATTERY LED
#define BATLED 0

#define LEDCHANNEL6 6
#define LEDCHANNEL7 7
#define LEDCHANNEL8 8

#define LEDFREQ 5000
#define LEDRES 8

WiFiClient client;
WiFiServer wifiServer(10000);

bool profileLoaded = false;

int light;
int moisture;
int temperature;

//plantprofile
int plantID;
int lightMin = 4500;
int lightMax = 300;
int tempMin = 15;
int tempMax = 25;
int suntimeMin = 6;
int suntimeMax = 8;
int moistureMin = 250;
int moistureMax = 900;

int identifier;

int counter = 0;

String dataPacket;

StaticJsonDocument<120> jsonDoc;
char chamomileDebugProfile[] = "{\"Type\":\"profile\":\"plantID\":1,\"Light\":1000,\"TemperatureMin\":15,\"TemperatureMax\":25,\"SuntimeMin\":6\"SuntimeMax\":8,\"MoistureMin\":100,\"MoistureMax\":2000}";


I2CSoilMoistureSensor sensor;

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

void setup()
{
  Wire.begin();
  Serial.begin(9600);
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

String startSensors()
{
  Serial.println("<<<<<< STARTING SENSORS >>>>>>");
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

//  moisture = map(moisture, 200, 722, 0, 100);
//  temperature = map(temperature, 255, 305, 0, 100);
//  light = map(light, 10000, 0, 0, 100);
//
//  Serial.println("Mapped -");
//  Serial.print("Temperature: ");
//  Serial.println(temperature / CELSIUSOFFSET);
//  Serial.print("Moisture: ");
//  Serial.println(moisture);
//  Serial.print("Light: ");
//  Serial.println(light);
//  Serial.println();

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

bool decodeJson()
{
  auto error = deserializeJson(jsonDoc, chamomileDebugProfile);

  if (error)
  {
    Serial.println("Deserialization failed, code: ");
    Serial.println(error.c_str());
    return false;
  }
  else
  {
    return true;
  }

}

void extractJson()
{
  decodeJson();
  Serial.println("<<<<<< JSON DESERIALIZATION >>>>>>");
  if (jsonDoc["Type"] == "profile")
  {
    Serial.println("JSON deserializing!");
    
    //plantID, light, tempmin, tempmax,suntimemin,suntimemax,moisturemin,moisturemax
    plantID = jsonDoc["plantID"];
    lightMin = jsonDoc["Light"][0];
    lightMax = jsonDoc["Light"][1];
    tempMin = jsonDoc["Temperature"][0];
    tempMax = jsonDoc["Temperature"][1];
    suntimeMin = jsonDoc["Suntime"][0];
    suntimeMax = jsonDoc["Suntime"][1];
    moistureMin = jsonDoc["Moisture"][0];
    moistureMax = jsonDoc["Moisture"][1];
    Serial.println(plantID);
    Serial.println(lightMin);
    Serial.println(lightMax);
    Serial.println(tempMin);
    Serial.println(tempMax);
    Serial.println(suntimeMin);
    Serial.println(suntimeMax);
    Serial.println(moistureMin);
    Serial.println(moistureMax);
    profileLoaded = true;
  }
  else if (jsonDoc["Type"] == "command")
  {
  }
}


// Check if the 3 parameters exceed acceptable values
void checkStatus()
{
  Serial.println("<<<<<< CHECKING STATUS >>>>>>");

  if (moisture < moistureMin)
  {
    ledActivation(139, 69, 19, 2, PULSE);

    Serial.print("Moisture: ");
    Serial.print(moisture);
    Serial.print(" < ");
    Serial.println(moistureMin);
    Serial.println("TOO DRY");
  }
  if (moisture > moistureMax)
  {
    ledActivation(0, 0, 255, 2, PULSE);

    Serial.print("Moisture: ");
    Serial.print(moisture);
    Serial.print(" > ");
    Serial.println(moistureMax);
    Serial.println("TOO MOIST");
  }

  if (light > lightMin)
  {
    ledActivation(0, 100, 100, 1, PULSE);

    Serial.print("Light: ");
    Serial.print(light);
    Serial.print(" <");
    Serial.println(lightMin);
    Serial.println("TOO DARK");
  }
  if (light < lightMax)
  {
    ledActivation(0, 255, 255, 1, PULSE);

    Serial.print("Light: ");
    Serial.print(light);
    Serial.print(" > ");
    Serial.println(lightMax);
    Serial.println("TOO LIGHT");
  }

  if ((temperature) < tempMin)
  {
    ledActivation(0, 0, 200, 3, PULSE);

    Serial.print("Temperature: ");
    //Divide by 10 to make sure the value is in normal Celsius range
    Serial.print(temperature);
    Serial.print(" <");
    Serial.println(tempMin);
    Serial.println("TOO COLD");
  }
  if ((temperature) > tempMax)
  {
    ledActivation(200, 0, 0, 3, PULSE);

    Serial.print("Temperature: ");
    Serial.print(temperature);
    Serial.print(" > ");
    Serial.println(tempMax);
    Serial.println("TOO HOT");

  }

}

//Pulse the LEDs
void ledActivation(int r, int g, int b, int id, ledStates ledState)
{
  //Light
  int LED;
  int LED1;
  int LED2;
  //Moisture
  int LED3;
  int LED4;
  int LED5;
  //Temperature
  int LED6;
  int LED7;
  int LED8;

  if (id == 1)
  {
    LED = LEDCHANNEL;
    LED1 = LEDCHANNEL1;
    LED2 = LEDCHANNEL2;
  }

  else if (id == 2)
  {
    LED = LEDCHANNEL6;
    LED1 = LEDCHANNEL7;
    LED2 = LEDCHANNEL8;
  }

  else if (id == 3);
  {
    LED = LEDCHANNEL3;
    LED1 = LEDCHANNEL4;
    LED2 = LEDCHANNEL5;
  }

  switch (ledState) {
    case BLINK:
      ledcWrite(LED, r);
      ledcWrite(LED1, g);
      ledcWrite(LED2, b);
      delay(1);
      ledcWrite(LED, ZERO);
      ledcWrite(LED1, ZERO);
      ledcWrite(LED2, ZERO);
      break;

    case PULSE:
      for (float fade = 1.0; fade >= 0.0; fade -= 0.002)
      {
        ledcWrite(LED, (float) r * fade);
        ledcWrite(LED1, (float) g * fade);
        ledcWrite(LED2, (float) b * fade);
        delay(10);
      }
      break;

    case PULSE_ALL:
      for (float fade = 1.0; fade >= 0.0; fade -= 0.002)
      {
        ledcWrite(LEDCHANNEL1, (float) r * fade);
        ledcWrite(LEDCHANNEL4, (float) r * fade);
        ledcWrite(LEDCHANNEL7, (float) r * fade);
        ledcWrite(LEDCHANNEL2, (float) g * fade);
        ledcWrite(LEDCHANNEL5, (float) g * fade);
        ledcWrite(LEDCHANNEL8, (float) g * fade);
        ledcWrite(LEDCHANNEL,  (float) b * fade);
        ledcWrite(LEDCHANNEL3, (float) b * fade);
        ledcWrite(LEDCHANNEL6, (float) b * fade);
        delay(10);
      }
      break;

    case ON:
      ledcWrite(LED, r);
      ledcWrite(LED1, g);
      ledcWrite(LED2, b);
      break;

    case OFF:
      ledcWrite(LED, ZERO);
      ledcWrite(LED1, ZERO);
      ledcWrite(LED2, ZERO);
      break;

    case TEST:
      Serial.println("BLUE LED TEST");
      ledcWrite(LEDCHANNEL, 255);
      ledcWrite(LEDCHANNEL3, 255);
      ledcWrite(LEDCHANNEL6, 255);
      delay(5);
      ledcWrite(LEDCHANNEL, ZERO);
      ledcWrite(LEDCHANNEL3, ZERO);
      ledcWrite(LEDCHANNEL6, ZERO);
      delay(5);

      Serial.println("RED LED TEST");
      ledcWrite(LEDCHANNEL1, 255);
      ledcWrite(LEDCHANNEL4, 255);
      ledcWrite(LEDCHANNEL7, 255);
      delay(5);
      ledcWrite(LEDCHANNEL1, ZERO);
      ledcWrite(LEDCHANNEL4, ZERO);
      ledcWrite(LEDCHANNEL7, ZERO);
      delay(5);

      Serial.println("GREEN LED TEST");
      ledcWrite(LEDCHANNEL2, 255);
      ledcWrite(LEDCHANNEL5, 255);
      ledcWrite(LEDCHANNEL8, 255);
      delay(5);
      ledcWrite(LEDCHANNEL2, ZERO);
      ledcWrite(LEDCHANNEL5, ZERO);
      ledcWrite(LEDCHANNEL8, ZERO);
      delay(5);

      break;
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

  //MOISTURE

  ledcSetup(LEDCHANNEL3, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL4, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL5, LEDFREQ, LEDRES);

  ledcAttachPin(redLED1, LEDCHANNEL3);
  ledcAttachPin(greenLED1, LEDCHANNEL4);
  ledcAttachPin(blueLED1, LEDCHANNEL5);

  //TEMPERATURE

  ledcSetup(LEDCHANNEL6, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL7, LEDFREQ, LEDRES);
  ledcSetup(LEDCHANNEL8, LEDFREQ, LEDRES);

  ledcAttachPin(redLED2, LEDCHANNEL6);
  ledcAttachPin(greenLED2, LEDCHANNEL7);
  ledcAttachPin(blueLED2, LEDCHANNEL8);


}

void loop()
{
  dataPacket = startSensors();
  checkStatus();

  //if (connectWiFi())
  //  sendData(dataPacket);
  Serial.println("<<<<<< DATA PACKET >>>>>>");  
  Serial.println(dataPacket);
}
