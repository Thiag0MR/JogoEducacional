using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalavraItemScript : MonoBehaviour
{
    public static event Action<string> OnPlayAudioButtonClick;
    public static event Action<string> OnDeleteButtonClick;
    public void HandlePlayAudioButtonClick()
    {
        OnPlayAudioButtonClick?.Invoke(gameObject.name);
    }

    public void HandleDeleteButtonClick()
    {
        OnDeleteButtonClick?.Invoke(gameObject.name);
    }
}
