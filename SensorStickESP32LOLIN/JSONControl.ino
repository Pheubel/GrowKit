StaticJsonDocument<120> JSONDoc;
char chamomileDebugProfile[] = "{\"Type\":\"profile\":\"plantID\":1,\"Light\":1000,\"TemperatureMin\":15,\"TemperatureMax\":25,\"SuntimeMin\":6\"SuntimeMax\":8,\"MoistureMin\":100,\"MoistureMax\":2000}";

//plantprofile
int plantID;
int lightMin = 9000;
int lightMax = 4000;
int tempMin = 20;
int tempMax = 30;
int suntimeMin = 6;
int suntimeMax = 8;
int moistureMin = 250;
int moistureMax = 900;



bool decodeJSON()
{
  auto error = deserializeJson(JSONDoc, chamomileDebugProfile);

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
  decodeJSON();
  Serial.println("<<<<<< JSON DESERIALIZATION >>>>>>");
  if (JSONDoc["Type"] == "profile")
  {
    Serial.println("JSON deserializing!");
    
    //plantID, light, tempmin, tempmax,suntimemin,suntimemax,moisturemin,moisturemax
    plantID = JSONDoc["plantID"];
    lightMin = JSONDoc["Light"][0];
    lightMax = JSONDoc["Light"][1];
    tempMin = JSONDoc["Temperature"][0];
    tempMax = JSONDoc["Temperature"][1];
    suntimeMin = JSONDoc["Suntime"][0];
    suntimeMax = JSONDoc["Suntime"][1];
    moistureMin = JSONDoc["Moisture"][0];
    moistureMax = JSONDoc["Moisture"][1];
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
  else if (JSONDoc["Type"] == "command")
  {
  }
}

String encodeJSON()
{


}
