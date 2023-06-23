using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Vogais
{
    public class BubbleScript : MonoBehaviour
    {
        private AudioSource audioSource;

        private GameObject gameManager;
        private GameManagerScript gameManagerScript;

        [SerializeField]
        private AudioClip pickUpClip;

        [SerializeField]
        private AudioClip dropClip;

        [SerializeField]
        private Rigidbody2D rb;

        private bool dragging;

        private Vector2 mousePosition, originalPosition, offSet;

        private void Awake()
        {
            gameManager = GameObject.FindGameObjectWithTag("GameManager").gameObject;
            gameManagerScript = gameManager.GetComponent<GameManagerScript>();
            audioSource = GetComponent<AudioSource>();
        }

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

            //PlayVowel();
        }

        void OnMouseUp()
        {
            if ((Vector2)transform.position == originalPosition) PlayVowelSound();

            transform.position = originalPosition;

            dragging = false;
        }

        private Vector2 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void PlayVowelSound()
        {
            string vowelName = transform.GetChild(0).name;
            gameManagerScript.PlayVowel(vowelName);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.tag == "BubbleSlot")
            {
                string vowelName = transform.GetChild(0).name;
                
                // Calcula a distância da bolha para o slot
                if (Vector2.Distance(this.transform.position, other.transform.position) < 3 
                        && gameManagerScript.IsCorrectVowel(vowelName))
                {

                    StartCoroutine(waitForSound(other));
                    
                    gameObject.SetActive(false);

                    // Destrói a bolha
                    Destroy(gameObject);

                    // Pega a letra dentro da bolha
                    GameObject bubbleLetter = this.transform.GetChild(0).gameObject;

                    // Cria uma cópia da letra que está dentro da bolha e coloca ela dentro do slot.
                    GameObject letter = Instantiate(bubbleLetter, other.transform.position, Quaternion.identity);

                    // Modifica o scale da letra dentro do slot
                    letter.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                    // Atualiza o score
                    GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Score);
                    
                    // Destrói a letra após 2 segundos
                    Destroy(letter, 2);

                    GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Victory);

                    GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.NextVowel);
                } else
                {
                    GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Lose);
                }
            }
        }
        IEnumerator waitForSound(Collider2D other)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }
}