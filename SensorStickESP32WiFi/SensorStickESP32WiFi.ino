#include <Wire.h>
#include <I2CSoilMoistureSensor.h>
#include <WiFi.h>

#define ZERO 0
#define CELSIUSOFFSET 10

#define SSID DSS
#define PW DSSwifi020!
#define PORT 10000
#define host "192.168.0.105"

int light;
int moisture;
int temperature;

int identifier;

WiFiMulti WiFiMulti;

I2CSoilMoistureSensor sensor;

void setup()
{
  //  Wire.begin();
  //  sensor.begin();
  //  Serial.begin(9600);
  //  writeI2CRegister8bit(0x20, 6); //reset
  //  Serial.print("I2C Soil Moisture Sensor Address: ");
  //  Serial.print(sensor.getAddress(), HEX);
  //  Serial.print("Sensor Firmware Version: ");
  //  Serial.println(sensor.getVersion(),HEX);
  //  Serial.println();

  Wire.begin();
  Serial.begin(9600);

  sensor.begin(); // reset sensor
  delay(1000); // give some time to boot up
  Serial.print("I2C Soil Moisture Sensor Address: ");
  Serial.println(sensor.getAddress(), HEX);
  Serial.print("Sensor Firmware version: ");
  Serial.println(sensor.getVersion(), HEX);
  Serial.println();

  Serial.print("Change address to 0x21 ...");
  if (sensor.setAddress(0x21, true)) // set Sensor Address to 0x21 and reset
    Serial.println("... DONE");
  else
    Serial.println("... ERROR");
  Serial.println();
}

void startSensors()
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
}

bool sendData()
{

}

void loop()
{
  startSensors();
  
}
