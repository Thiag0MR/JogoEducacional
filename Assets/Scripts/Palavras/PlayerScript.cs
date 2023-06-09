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

        private Vector2 screenBounds;
        private float playerPaddingX, playerPaddingY;

        void Awake()
        {
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            playerPaddingX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2.2f;
            playerPaddingY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2.4f;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }
        void LateUpdate()
        {
            // Impede o player de sair da tela.
            Vector2 positionWithinScreen = transform.position;
            positionWithinScreen.x = Mathf.Clamp(positionWithinScreen.x, -screenBounds.x + playerPaddingX, 
                screenBounds.x - playerPaddingX);
            positionWithinScreen.y = Mathf.Clamp(positionWithinScreen.y, -screenBounds.y + playerPaddingY,
                screenBounds.y - playerPaddingY);
            transform.position = positionWithinScreen;
        }
        public void Move()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            Vector3 movement = new(x, z, 0);
            transform.Translate(speed * Time.deltaTime * movement);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Letter") && Input.GetKey(KeyCode.Space))
            {
                other.transform.SetParent(gameObject.transform.GetChild(0), true);
                // O centro da letra é posicionado 
                other.transform.localPosition = new Vector3(0, other.transform.localPosition.y, 0);
                // O centro da letra é posicionado no centro do objeto carryLocation.
                //other.transform.localPosition = Vector3.zero;
            }
        }
    }
}