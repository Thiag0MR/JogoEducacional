using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorScript : MonoBehaviour
{
    [SerializeField]
    private Texture2D normal, hand;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.Auto);
    }

    void OnMouseEnter()
    {
        Cursor.SetCursor(hand, Vector2.zero, CursorMode.Auto);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.Auto);
    }
}
