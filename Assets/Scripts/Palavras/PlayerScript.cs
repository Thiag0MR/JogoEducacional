using UnityEngine;
using UnityEngine.InputSystem;

namespace Palavras
{
    public class PlayerScript : MonoBehaviour
    {
        [SerializeField]
        private float speed;

        private Rigidbody2D rb;

        private Vector2 screenBounds;
        public Vector3 movementInput;
        private Vector2 smoothedMovementInput;
        private Vector2 smoothedMovementInputVelocity;
        private float playerPaddingX, playerPaddingY;
        private GameObject carryLocation;

        [SerializeField]
        private FixedJoystick fixedJoystick;

        private PlayerInputActions playerInputActions;

        private Animator animator;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            playerPaddingX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2.0f;
            playerPaddingY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y / 2.4f;
            carryLocation = gameObject.transform.GetChild(0).gameObject;
            playerInputActions = new PlayerInputActions();

            animator = GetComponent<Animator>();
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
            screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            //verifica se o player est� nadando para realizar a anima��o
            if (movementInput != Vector3.zero || fixedJoystick.Direction != Vector2.zero)
            {
                //esta nadando
                animator.SetBool("estaNadando", true);
            }
            else
            {
                //nao esta nadando
                animator.SetBool("estaNadando", false);
            }
        }
        private void OnMove(InputValue value)
        {
            // M�todo chamado quando o player � movimentado
            movementInput = value.Get<Vector2>();
        }
        void FixedUpdate()
        {
            if (fixedJoystick.Direction != Vector2.zero)
            {
                rb.velocity = fixedJoystick.Direction * speed * Time.fixedDeltaTime;
            } else
            {
                smoothedMovementInput = Vector2.SmoothDamp(smoothedMovementInput, movementInput,
                    ref smoothedMovementInputVelocity, 0.1f);
                rb.velocity = smoothedMovementInput * speed * Time.fixedDeltaTime;
            }
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

        private void OnTriggerStay2D(Collider2D other)
        {
            bool isSpaceKeyPressed = playerInputActions.Player.SpaceKey.IsPressed();

            if (other.CompareTag("Letter") && isSpaceKeyPressed && carryLocation.transform.childCount == 0 
                && !other.gameObject.GetComponent<LetterScript>().IsFalling)
            {
                other.transform.SetParent(gameObject.transform.GetChild(0), true);

                // O centro da letra � posicionado 
                other.transform.localPosition = new Vector3(0, other.transform.localPosition.y, 0);

                // O centro da letra � posicionado no centro do objeto carryLocation.
                //other.transform.localPosition = Vector3.zero;

                other.GetComponent<LetterScript>().PlayLetterSound();

                animator.SetBool("pegouLetra", true);//pegou a letra
            }
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Letter"))
            {
                animator.SetBool("pegouLetra", false);//soltou a letra
            }
        }
    }
}