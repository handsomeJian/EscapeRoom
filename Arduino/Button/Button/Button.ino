// constants won't change. They're used here to set pin numbers:
const int buttonPin[3] = {11, 7, 2};  // the number of the pushbutton pin
const int ledPin[3] = {13, 9, 4};    // the number of the LED pin
const String buttonName[3] = {"Jump", "Right", "Left"};

// Variables will change:
int ledState[3] = {LOW, LOW, LOW};        // the current state of the output pin
int buttonState[3] = {LOW, LOW, LOW};            // the current reading from the input pin
int lastButtonState[3] = {LOW, LOW, LOW};  // the previous reading from the input pin

// the following variables are unsigned longs because the time, measured in
// milliseconds, will quickly become a bigger number than can be stored in an int.
unsigned long lastDebounceTime[3] = {0, 0, 0};  // the last time the output pin was toggled
unsigned long debounceDelay = 50;    // the debounce time; increase if the output flickers

void setup() {
  Serial.begin(9600);

  for (int i = 0; i < 3; i++) {
    pinMode(buttonPin[i], INPUT_PULLUP);
    pinMode(ledPin[i], OUTPUT);
    digitalWrite(ledPin[i], ledState[i]);
  }
}

void loop() {
  for (int i = 0; i < 3; i++) {
    UpdateButtonState(i);
  }
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
}
