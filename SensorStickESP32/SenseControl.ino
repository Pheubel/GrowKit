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
