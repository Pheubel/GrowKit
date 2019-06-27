//Hi! This section is used for the Bluetooth functionalities of the ESP32 chip. The idea is to have a single device that serves as a server and the others connect to it. This server then connects externally to
//send data to the database. Unfortunately, due to time constraints, we never got around to implementing this! This is some example code that lets you make a server broadcast itself on bluetooth. 
//The SERVICE_UUID and CHARACTERISTIC_UUID have been generated at https://www.uuidgenerator.net/ and for projects like these they don't really hold any meaning other than being a unique identifier

//#define SERVICE_UUID        "c5fbf89d-c6df-4128-a380-bb89161a130d"
//#define CHARACTERISTIC_UUID "5ec99e63-8492-4fa1-97bd-188607cf3464"
//
//void BLEServerSetup() {
//  Serial.begin(115200);
//  Serial.println("Starting BLE work!");
//
//  BLEDevice::init("Groene Vingers");
//  BLEServer *pServer = BLEDevice::createServer();
//  BLEService *pService = pServer->createService(SERVICE_UUID);
//  BLECharacteristic *pCharacteristic = pService->createCharacteristic(
//                                         CHARACTERISTIC_UUID,
//                                         BLECharacteristic::PROPERTY_READ |
//                                         BLECharacteristic::PROPERTY_WRITE
//                                       );
//
//  pCharacteristic->setValue("ESP32 Stick is go!");
//  pService->start();
//  // BLEAdvertising *pAdvertising = pServer->getAdvertising();  // this still is working for backward compatibility
//  BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
//  pAdvertising->addServiceUUID(SERVICE_UUID);
//  pAdvertising->setScanResponse(true);
//  pAdvertising->setMinPreferred(0x06);  // functions that help with iPhone connections issue
//  pAdvertising->setMinPreferred(0x12);
//  BLEDevice::startAdvertising();
//  Serial.println("Characteristic defined! Now you can read it in your phone!");
//}
//
//
//void BScan()
//{
//  BLEScan* pMyScan = BLEDevice::getScan();
//  pMyScan->setAdvertisedDeviceCallbacks
//}
