using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public static event Action<string> OnPlayAudioButtonClick;
    public static event Action<string> OnDeleteButtonClick;
    public static event Action<string> OnEditButtonClick;

    public void HandlePlayAudioButtonClick()
    {
        OnPlayAudioButtonClick?.Invoke(gameObject.name);
    }

    public void HandleDeleteButtonClick()
    {
        OnDeleteButtonClick?.Invoke(gameObject.name);
    }
    public void HandleEditButtonClick()
    {
        OnEditButtonClick?.Invoke(gameObject.name);
    }
}
