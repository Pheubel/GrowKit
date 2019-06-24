// All of the JSON deserialization and serialization happens here. An example of the JSON Data Transfer Objects is on github for when you want to make a new plant profile. For debugging purposes, we use a
// Chamomile profile. All the data that gets sent and received are in JSON format to safe space and keep things readable for us humans.
StaticJsonDocument<120> InJSONdoc;
StaticJsonDocument<120> OutJSONdoc;
char chamomileDebugProfile[] = "{\"Type\":\"profile\":\"plantID\":1,\"Light\":1000,\"TemperatureMin\":15,\"TemperatureMax\":25,\"SuntimeMin\":6\"SuntimeMax\":8,\"MoistureMin\":100,\"MoistureMax\":2000}";

//plantprofile
int plantID;
int lightMin = 200;
int lightMax = 1500;
int tempMin = 20;
int tempMax = 30;
int suntimeMin = 6;
int suntimeMax = 8;
int moistureMin = 250;
int moistureMax = 900;



bool decodeJSON()
{
  auto error = deserializeJson(InJSONdoc, chamomileDebugProfile);

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
  if (InJSONdoc["Type"] == "profile")
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
  else if (InJSONdoc["Type"] == "command")
  {
  }
}

String encodeJSON()
{
String JSON;
JsonObject root = OutJSONdoc.to<JsonObject>();
root["Type"] = "Update";
root["ID_Stick"] = 0;
root["ID_Master"] = 0;
JsonObject message = root.createNestedObject("Message");
message["Light"] = light;
message["Moisture"] = moisture;
message["Temperature"] = temperature;
message["LightTime"] = 0;
message["Timestamp"] = 0;
serializeJson(root, JSON);
return JSON;
}
