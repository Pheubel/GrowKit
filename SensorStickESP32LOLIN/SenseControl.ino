
String startSensors()
{
  Serial.println("<<<<<< STARTING SENSORS >>>>>>");
  sensor.begin();

  delay(1000);
  sensor.startMeasureLight();
  delay(3000);
  int data[2];
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
  Serial.print("Temperature: ");
  Serial.println(temperature / CELSIUSOFFSET);
  Serial.print("Moisture: ");
  Serial.println(moisture);
  Serial.print("Light: ");
  Serial.println(light);
  Serial.println();

  //Clamp values to keep value size restricted
  if ( moisture < 0)
    moisture = 0;

  if (light > 10000)
    light = 10000;

  if (light < 0)
    light = 0;

  if (temperature > 100)
    temperature = 100;

  //  Serial.println(ID + moisture + ":" + light + ":" + temperature);
  //  return (ID + moisture + ":" + light + ":" + temperature);
}




// Check if the 3 parameters exceed acceptable values
void checkStatus()
{
  Serial.println("<<<<<< CHECKING STATUS >>>>>>");

  Serial.println(light);
  if (moisture < moistureMin)
  {
    if (blinkPhase)
      leds[1] = moistLED.onLED();
    else
      leds[1] = moistLED.offLED();

    Serial.print("Moisture: ");
    Serial.print(moisture);
    Serial.print(" < ");
    Serial.println(moistureMin);
    Serial.println("TOO DRY");
  }
  if (moisture > moistureMax)
  {
    leds[1] = moistLED.pulseLED();
    Serial.print("Moisture: ");
    Serial.print(moisture);
    Serial.print(" > ");
    Serial.println(moistureMax);
    Serial.println("TOO MOIST");
  }
  if (moisture >= moistureMin && moisture <= moistureMax)
  {
    leds[1] = moistLED.offLED();
    Serial.print("Moisture: ");
    Serial.println(moisture);
    Serial.println("Good");
  }
  // Light is flipped because Dark = higher number, light = lower number

  if (light < lightMax)
  {
    leds[2] = lightLED.pulseLED();
    Serial.print("Light: ");
    Serial.print(light);
    Serial.print(" <");
    Serial.println(lightMin);
    Serial.println("TOO LIGHT");
  }
  if (light > lightMin)
  {
    if (blinkPhase)
      leds[2] = lightLED.onLED();
    else
      leds[2] = lightLED.offLED();
    Serial.print("Light: ");
    Serial.print(light);
    Serial.print(" > ");
    Serial.println(lightMax);
    Serial.println("TOO DARK");
  }
  if (light >= lightMin && light <= moistureMax)
  {
    leds[2] = lightLED.offLED();
    Serial.print("Light: ");
    Serial.println(light);
    Serial.println("Good");
  }

  if ((temperature) < tempMin)
  {
    leds[0] = tempLED.pulseLED();
    Serial.print("Temperature: ");
    //Divide by 10 to make sure the value is in normal Celsius range
    Serial.print(temperature);
    Serial.print(" <");
    Serial.println(tempMin);
    Serial.println("TOO COLD");
  }
  if ((temperature) > tempMax)
  {
    if (blinkPhase)
      leds[0] = tempLED.onLED();
    else
      leds[0] = tempLED.offLED();

    Serial.print("Temperature: ");
    Serial.print(temperature);
    Serial.print(" > ");
    Serial.println(tempMax);
    Serial.println("TOO HOT");
  }
  if (temperature >= tempMin && temperature <= tempMax)
  {
    leds[0] = tempLED.offLED();
    Serial.print("Temperature: ");
    Serial.print(temperature);
    Serial.println("Good");
  }

}
