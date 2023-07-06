using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    private bool dragging;

    private Vector2 mousePosition, originalPosition, offSet;

    void Start()
    {
        originalPosition = transform.position;
    }
    void Update()
    {
        if (!dragging) return;

        mousePosition = GetMousePosition() - offSet;
    }

    void FixedUpdate()
    {
        if (dragging)
        {
            rb.MovePosition(mousePosition);
        }
    }

    void OnMouseDown()
    {
        dragging = true;

        offSet = GetMousePosition() - (Vector2)transform.position;

        //PlaySound();
    }

    void OnMouseUp()
    {
        if ((Vector2)transform.position == originalPosition)
            PlayVowelSound();

        transform.position = originalPosition;

        dragging = false;
    }

    private Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void PlayVowelSound()
    {
        transform.GetChild(0).GetComponent<AudioSource>().Play();
    }
}