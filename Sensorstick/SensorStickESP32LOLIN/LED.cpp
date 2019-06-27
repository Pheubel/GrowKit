#include "LED.h"

	LED::LED(CRGB startColour)
	{
		this->startColour = startColour;
		newColour = startColour;
    off = CRGB(0,0,0);
	}


	CRGB LED::onLED()
	{
      LEDOn = true;
			return startColour;
	}

	CRGB LED::pulseLED()
	{
		CRGB thisColour((int)newColour.r * fade, (int)newColour.g * fade, (int)newColour.b * fade);
    
		if (!bounce)
			fade -= 0.15f;
		if (bounce)
			fade += 0.15f;

		if (fade <= 0.0f)
			bounce = true;
		if (fade >= 1.0f)
			bounce = false;

		return thisColour;
	}

	CRGB LED::offLED()
	{
		return off;
	}
