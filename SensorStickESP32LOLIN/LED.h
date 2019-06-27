#ifndef ADAFRUIT_NEOPIXEL_H
#define ADAFRUIT_NEOPIXEL_H
#endif

#include <FastLED.h>

//LED Class that returns CRGB values for the NeoPixel LED array
//Different functions have different LED flashing patterns
//The reason behind this class is to both pass along LED states and unique colours.

//Alternate between onLED() and offLED() to blink.
//pulseLED() turns the LED on and slowly decreases the colour values until it turns off, then it slowly increases till it's back at its starting value. 

class LED
{
private:
	CRGB startColour;
	CRGB newColour;
  CRGB off;
	float fade = 1.0f;
	bool LEDOn =  false;
	bool bounce = false;

public:
	LED(CRGB startColour);

	CRGB onLED();
	CRGB pulseLED();
	CRGB offLED();
};
