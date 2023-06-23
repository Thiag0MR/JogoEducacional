using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Palavras
{
    public class PlayerScript : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        private Rigidbody2D rb;

        private Vector2 screenBounds;
        private Vector3 movement;
        private float playerPaddingX, playerPaddingY;
        private GameObject carryLocation;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            playerPaddingX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
            playerPaddingY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2.4f;
            carryLocation = gameObject.transform.GetChild(0).gameObject;
        }

        void Update()
        {
            movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        }
        void FixedUpdate()
        {
            MovePlayer();
        }
        void LateUpdate()
        {
            // Impede o player de sair da tela.
            Vector2 positionWithinScreen = transform.position;
            positionWithinScreen.x = Mathf.Clamp(positionWithinScreen.x, -screenBounds.x + playerPaddingX, 
                screenBounds.x - playerPaddingX);
            if (carryLocation.transform.childCount == 0)
            {
                positionWithinScreen.y = Mathf.Clamp(positionWithinScreen.y, -screenBounds.y + playerPaddingY,
                    screenBounds.y - playerPaddingY);
            } else
            {
                GameObject carryLocationChild = carryLocation.transform.GetChild(0).gameObject;
                float carryLocationYPostion = carryLocation.transform.position.y;
                float playerLetterDistance = Vector3.Distance(transform.position, carryLocationChild.transform.position);
                float letterPadding = carryLocationChild.GetComponent<SpriteRenderer>().bounds.size.y / 2;
                positionWithinScreen.y = Mathf.Clamp(positionWithinScreen.y, 
                    -screenBounds.y + (playerLetterDistance + letterPadding),
                    screenBounds.y - playerPaddingY);
            }
            transform.position = positionWithinScreen;
        }
        public void MovePlayer()
        {
            rb.velocity = movement * speed * Time.fixedDeltaTime;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Letter") && Input.GetKey(KeyCode.Space) && carryLocation.transform.childCount == 0)
            {
                other.transform.SetParent(gameObject.transform.GetChild(0), true);
                // O centro da letra é posicionado 
                other.transform.localPosition = new Vector3(0, other.transform.localPosition.y, 0);
                // O centro da letra é posicionado no centro do objeto carryLocation.
                //other.transform.localPosition = Vector3.zero;

                other.GetComponent<LetterScript>().PlayLetterSound();
            }
        }
    }
}