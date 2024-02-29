#include <WiFiS3.h>

// WIFI
#define DESTINATION_IP "192.168.0.235"
#define DESTINATION_PORT 8002

#define SECRET_SSID "ETC_Test_Wi-Fi"
#define SECRET_PASS "etcdemo123"

int status = WL_IDLE_STATUS;
char ssid[] = SECRET_SSID;  // your network SSID (name)
char pass[] = SECRET_PASS;  // your network password (use for WPA, or use as key for WEP)
int keyIndex = 0;           // your network key index number (needed only for WEP)

unsigned int localPort = 2390;  // local port to listen on
char packetBuffer[256];                 //buffer to hold incoming packet

WiFiUDP Udp;

// Button
const int buttonPin[3] = {11, 7, 2};  // the number of the pushbutton pin
const int ledPin[3] = {13, 9, 4};    // the number of the LED pin
const String buttonName[3] = {"Jump", "Right", "Left"};

int ledState[3] = {LOW, LOW, LOW};        // the current state of the output pin
int buttonState[3] = {LOW, LOW, LOW};            // the current reading from the input pin
int lastButtonState[3] = {LOW, LOW, LOW};  // the previous reading from the input pin

// the following variables are unsigned longs because the time, measured in
// milliseconds, will quickly become a bigger number than can be stored in an int.
unsigned long lastDebounceTime[3] = {0, 0, 0};  // the last time the output pin was toggled
unsigned long debounceDelay = 50;    // the debounce time; increase if the output flickers

bool sendingState = false;
int indexToSend = -1;
unsigned long lastSendTime = 0;  // the last time the output pin was toggled
unsigned long sendDelay = 50;    // the debounce time; increase if the output flickers

void SetupButtons() {
  for (int i = 0; i < 3; i++) {
    pinMode(buttonPin[i], INPUT_PULLUP);
    pinMode(ledPin[i], OUTPUT);
    digitalWrite(ledPin[i], ledState[i]);
  }
}

void SetupWifi() {
  if (WiFi.status() == WL_NO_MODULE) {
    Serial.println("Communication with WiFi module failed!");
    // don't continue
    while (true);
  }

  String fv = WiFi.firmwareVersion();
  if (fv < WIFI_FIRMWARE_LATEST_VERSION) {
    Serial.println("Please upgrade the firmware");
  }

  // attempt to connect to WiFi network:
  while (status != WL_CONNECTED) {
    Serial.print("Attempting to connect to SSID: ");
    Serial.println(ssid);
    // Connect to WPA/WPA2 network. Change this line if using open or WEP network:
    status = WiFi.begin(ssid, pass);

    // wait 10 seconds for connection:
    delay(10000);
  }
  Serial.println("Connected to WiFi");
  printWifiStatus();

  Serial.println("\nStarting connection to server...");
  // if you get a connection, report back via serial:
  Udp.begin(localPort);
}

void setup() {
  Serial.begin(9600);

  SetupButtons();
  SetupWifi();  
}

void loop() {
  
  for (int i = 0; i < 3; i++) {
    UpdateButtonState(i);
  }
  UpdateSendState();
}

void UpdateButtonState(int index) {

  // read the state of the switch into a local variable:
  int reading = digitalRead(buttonPin[index]);

  // check to see if you just pressed the button
  // (i.e. the input went from LOW to HIGH), and you've waited long enough
  // since the last press to ignore any noise:

  // If the switch changed, due to noise or pressing:
  if (reading != lastButtonState[index]) {
    // reset the debouncing timer
    lastDebounceTime[index] = millis();
  }

  if ((millis() - lastDebounceTime[index]) > debounceDelay) {
    // whatever the reading is at, it's been there for longer than the debounce
    // delay, so take it as the actual current state:

    // if the button state has changed:
    if (reading != buttonState[index]) {
      buttonState[index] = reading;
      if (buttonState[index] == LOW) {
        Serial.println(buttonName[index] + " Pressed");
        SendButtonPressedEvent(index);
        //messageSent = true;
        ledState[index] = HIGH;
      }
      else {
        ledState[index] = LOW;
      }
    }
  }

  // set the LED:
  digitalWrite(ledPin[index], ledState[index]);

  // save the reading. Next time through the loop, it'll be the lastButtonState:
  lastButtonState[index] = reading;

  //return messageSent;
}

void UpdateSendState() {
  if (sendingState) {
    if ((millis() - lastSendTime) > sendDelay) {
      sendingState = false;
    }
    return;
  }
  if (indexToSend != -1) {
    SendEvent(buttonName[indexToSend]);
    sendingState = true;
    lastSendTime = millis();
    indexToSend = -1;
  }
}

void SendButtonPressedEvent(int index) {
  indexToSend = index;
}

void SendEvent(String msg) {
  char eventBuffer[10];
  msg.toCharArray(eventBuffer, sizeof(eventBuffer));
  Serial.print(Udp.beginPacket(DESTINATION_IP, DESTINATION_PORT));
  Udp.write(eventBuffer);
  Serial.print(Udp.endPacket());
  Serial.println(eventBuffer);
}

void printWifiStatus() {
  // print the SSID of the network you're attached to:
  Serial.print("SSID: ");
  Serial.println(WiFi.SSID());

  // print your board's IP address:
  IPAddress ip = WiFi.localIP();
  Serial.print("IP Address: ");
  Serial.println(ip);

  // print the received signal strength:
  long rssi = WiFi.RSSI();
  Serial.print("signal strength (RSSI):");
  Serial.print(rssi);
  Serial.println(" dBm");
}
