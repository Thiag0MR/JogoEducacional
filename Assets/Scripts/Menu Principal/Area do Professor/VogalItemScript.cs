using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VogalItemScript : MonoBehaviour
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
