#ifndef ADAFRUIT_NEOPIXEL_H
#define ADAFRUIT_NEOPIXEL_H
#endif

#include <FastLED.h>

//LED Class that returns CRGB values for the NeoPixel LED array
//Different functions have different LED flashing patterns

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

	CRGB blinkLED();
	CRGB pulseLED();
	CRGB offLED();
};
