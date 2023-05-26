using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    Dictionary<string, int> vogalPosition = new Dictionary<string, int> {
        {"A", 0},
        {"E", 1},
        {"I", 2},
        {"O", 3},
        {"U", 4},
    };

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
    }

    private Vector2 GetMousePosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnTriggerEnter2D(Collider2D other) {

        // Se o objeto que a bolha colidir for o BubbleSlot
        if (other.tag == "BubbleSlot") {

            // Calcula a distância da bolha para o slot
            if (Vector2.Distance(this.transform.position, other.transform.position) < 3)
            {
                
                // Toca o som de drop
                audioSource.PlayOneShot(dropClip);

                StartCoroutine(waitForSound(other));
                
            }
             // Aumentar a pontuação
            // Tocar som de vitoria
        }
    }
     IEnumerator waitForSound(Collider2D other) {
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        gameObject.SetActive(false);
        Destroy(gameObject);
        
        // Pega a letra dentro da bolha
        GameObject bubbleLetter = this.transform.GetChild(0).gameObject;

        // Cria uma cópia da letra que está dentro da bolha e coloca ela dentro do slot.
        GameObject letter = Instantiate(bubbleLetter, other.transform.position, Quaternion.identity);

        // Modifica o scale da letra dentro do slot
        letter.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        
        // Atualiza o score
        GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Score); 
    }
}


