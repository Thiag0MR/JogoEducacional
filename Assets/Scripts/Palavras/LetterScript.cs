using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Palavras
{
    public class LetterScript : MonoBehaviour
    {
        private Rigidbody2D rb;
        // Para fazer a rotação da letra
        private int rotate = 0;
        private bool increment = true;
        private readonly int limit = 30;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            GameManagerScript.OnGameStateChange += GameManagerScript_OnGameStateChange;
        }

        private void OnDestroy()
        {
            GameManagerScript.OnGameStateChange -= GameManagerScript_OnGameStateChange;
        }

        private void GameManagerScript_OnGameStateChange(GameManagerScript.GameState state)
        {
            if (state == GameManagerScript.GameState.Victory)
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
                transform.SetParent(gameObject.transform.root, true);
                rb.gravityScale = 1;
            }
        }
    }
}