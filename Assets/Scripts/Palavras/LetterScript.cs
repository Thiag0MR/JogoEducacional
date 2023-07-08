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

        // Para fazer a rota��o da letra
        private int rotate = 0;
        private bool increment = true;
        private readonly int limit = 30;
        public bool IsFalling { get; set; } = false;

        private PlayerInputActions playerInputActions;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            playerInputActions = new PlayerInputActions();
        }
        private void OnEnable()
        {
            playerInputActions.Enable();
        }

        private void OnDisable()
        {
            playerInputActions.Disable();
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
            if (other.CompareTag("Chest") && playerInputActions.Player.SpaceKey.IsPressed())
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