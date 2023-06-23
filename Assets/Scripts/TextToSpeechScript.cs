using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpeechLib;

public class TextToSpeechScript : MonoBehaviour
{
    SpVoice spVoice = new SpVoice();

    public void Speak(string message)
    {
        spVoice.Speak(message);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.A)) {
            spVoice.Speak("A");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.B)) {
            spVoice.Speak("B");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.C)) {
            spVoice.Speak("C");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.D)) {
            spVoice.Speak("D");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E)) {
            spVoice.Speak("E");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F)) {
            spVoice.Speak("F");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad1)) {
            spVoice.Speak("Olá, tudo bem ?");
        }else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Keypad2)) {
            spVoice.Speak("Olá, como posso te ajudar ?");
        }
    }
}
