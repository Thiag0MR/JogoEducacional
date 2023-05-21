using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseOver : MonoBehaviour
{
    public void PointerEnter() {
        transform.localScale = new Vector2(1.1f, 1.1f);
    }

    public void PointerExit() {
        transform.localScale = new Vector2(1.0f, 1.0f);
    }
}
