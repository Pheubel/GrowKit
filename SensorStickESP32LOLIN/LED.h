#ifndef ADAFRUIT_NEOPIXEL_H
#define ADAFRUIT_NEOPIXEL_H

#include <FastLED.h>

class LED
{
private:
	CRGB startColour;
	CRGB newColour;
	float fade = 1.0f;
	bool ledOff;
	bool bounce;

public:
	LED(CRGB startColour)
	{
		CRGB startColour;
		CRGB newColour;
	}

	CRGB blinkLED();
	CRGB pulseLED();
	CRGB offLED();
};
