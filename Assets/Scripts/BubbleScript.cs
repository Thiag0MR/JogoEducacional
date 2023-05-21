using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleScript : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip pickUpClip;
    [SerializeField]
    private AudioClip dropClip;

    [SerializeField]
    private Rigidbody2D rb;

    private bool dragging;

    private Vector2 mousePosition, originalPosition, offSet;

    void Start() {
        audioSource = GetComponent<AudioSource>();
        originalPosition = transform.position;
    }

    void Update() {
        if (!dragging) return;

        mousePosition = GetMousePosition() - offSet;
    }

    void FixedUpdate() {
        if (dragging) {
            rb.MovePosition(mousePosition);
        }
    }

    void OnMouseDown() {
        dragging = true;
        audioSource.PlayOneShot(pickUpClip);

        offSet = GetMousePosition() - (Vector2) transform.position;
    }

    void OnMouseUp() {
        transform.position = originalPosition;
        dragging = false;
        audioSource.PlayOneShot(dropClip);
    }

    private Vector2 GetMousePosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}


