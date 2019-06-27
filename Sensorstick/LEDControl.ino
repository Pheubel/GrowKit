

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
      this->startColour = startColour;
      newColour = startColour;
    }


    CRGB blinkLED()
    {
      if (!ledOff)
      {
        offLED();
      }
      else
      {
        return startColour;
      }
    }

    CRGB pulseLED()
    {
      CRGB thisColour(newColour.r * fade, newColour.g * fade, newColour.b * fade);
      if (!bounce)
        fade -= 0.05f;
      if (bounce)
        fade += 0.05f;

      if (fade = 0.0f)
        bounce = true;
      if (fade = 1.0f)
        bounce = false;

      return thisColour;
    }

    CRGB offLED()
    {
      CRGB thisColour(ZERO,ZERO,ZERO);
      ledOff = true;
      return thisColour;
    }
};
