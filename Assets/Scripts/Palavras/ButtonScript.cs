using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{

    public static event Action<GameObject> OnButtonClick;

    public void HandleButtonClick()
    {
        OnButtonClick?.Invoke(gameObject);
    }
}
