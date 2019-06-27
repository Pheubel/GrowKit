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
