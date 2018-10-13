using UnityEngine;
using System.Collections;
using Uniduino;

public class BlinkyLight : MonoBehaviour {

    public Arduino arduino;

    void Start () {
        arduino = Arduino.global;
        arduino.Setup(ConfigurePins);
        StartCoroutine(BlinkLoop());
	}

    void ConfigurePins()
    {
        arduino.pinMode(6,PinMode.OUTPUT);
    }

    IEnumerator BlinkLoop()
    {
        while (true)
        {
          arduino.digitalWrite(6, Arduino.HIGH);
          yield return new WaitForSeconds(1);

          arduino.digitalWrite(6, Arduino.LOW);
          yield return new WaitForSeconds(1);
        }
    }
}
