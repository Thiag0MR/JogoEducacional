using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnMouseOver : MonoBehaviour
{
    private float originalFontSize;
    
    public void PointerEnterScaleButton() {
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public void PointerExitScaleButton() {
        transform.localScale = new Vector2(1.0f, 1.0f);
    }

    public void PointerEnterScaleFont()
    {
        TextMeshProUGUI txt = GetComponentInChildren<TextMeshProUGUI>();
        originalFontSize = txt.fontSize;
        txt.fontSize = originalFontSize + 2;
    }

    public void PointerExitScaleFont()
    {
        TextMeshProUGUI txt = GetComponentInChildren<TextMeshProUGUI>();
        txt.fontSize = originalFontSize;

    }
}
