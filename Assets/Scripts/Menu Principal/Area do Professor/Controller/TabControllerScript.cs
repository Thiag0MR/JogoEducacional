using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabControllerScript : MonoBehaviour
{
    public static event Action<int> OnTabClick;

    public void HandleTabClickButton()
    {
        int index = transform.GetSiblingIndex();
        OnTabClick?.Invoke(index);
    }
}
