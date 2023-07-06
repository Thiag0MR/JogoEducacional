using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Palavras
{
    public class LetterScript : MonoBehaviour
    {
        private Rigidbody2D rb;
        private AudioSource audioSource;

        // Para fazer a rotação da letra
        private int rotate = 0;
        private bool increment = true;
        private readonly int limit = 30;
        public bool IsFalling { get; set; } = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            GameManagerScript.OnGameStateChange += GameManagerScript_OnGameStateChange;
        }

        private void OnDestroy()
        {
            GameManagerScript.OnGameStateChange -= GameManagerScript_OnGameStateChange;
        }

        private void GameManagerScript_OnGameStateChange(int state)
        {
            if (state == GameState.RightLetter)
            {
                // Tocar som quando acerta a letra
            }
        }

        void Update()
        {
            if (rb.gravityScale != 0)
            {
                if (rotate > limit)
                {
                    increment = false;
                } else if (rotate < -limit)
                {
                    increment = true;
                }
                rotate = increment ? rotate += 1 : rotate -= 1;
                transform.eulerAngles = new Vector3 (0, 0, transform.position.z + rotate);
            }
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Chest") && Input.GetKey(KeyCode.Space))
            {
                Transform wordSpaceCanvasTransform = gameObject.transform.root.GetChild(1);
                transform.SetParent(wordSpaceCanvasTransform, true);
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1;
                IsFalling = true;
            }
        }
        public void PlayLetterSound() {
            audioSource.Play();
        }
    }
}