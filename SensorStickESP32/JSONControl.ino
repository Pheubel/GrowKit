StaticJsonDocument<120> jsonDoc;
char chamomileDebugProfile[] = "{\"Type\":\"profile\":\"plantID\":1,\"Light\":1000,\"TemperatureMin\":15,\"TemperatureMax\":25,\"SuntimeMin\":6\"SuntimeMax\":8,\"MoistureMin\":100,\"MoistureMax\":2000}";

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
