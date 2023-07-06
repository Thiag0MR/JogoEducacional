using System.Collections;
using UnityEngine;

public class BubbleDragSlotScript : MonoBehaviour
{

    [SerializeField] GameObject gameManager;
    private IGameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<IGameManager>();
    }

    private IEnumerator OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Bubble"))
        {
            string vowelName = other.transform.GetChild(0).name;

            // Calcula a distância da bolha para o slot
            if (Vector2.Distance(this.transform.position, other.transform.position) < 3
                    && gameManagerScript.IsCorrectLetter(vowelName))
            {

                // Desativa a letra de interrogação
                transform.GetChild(1).gameObject.SetActive(false);

                // Desativa a bolha
                other.gameObject.SetActive(false);

                // Destrói a bolha
                Destroy(other.gameObject);

                // Pega a letra dentro da bolha
                GameObject bubbleLetter = other.transform.GetChild(0).gameObject;

                // Cria uma cópia da letra que está dentro da bolha e coloca ela dentro do slot.
                GameObject letter = transform.Find("Letter").gameObject;
                letter.SetActive(true);
                letter.GetComponent<SpriteRenderer>().sprite = bubbleLetter.GetComponent<SpriteRenderer>().sprite;

                gameManagerScript.UpdateGameState(GameState.Victory);
                gameManagerScript.UpdateGameState(GameState.Score);

                yield return StartCoroutine(ActivateInterrogationLetterAfterScale(gameObject));

                gameManagerScript.UpdateGameState(GameState.NextLetter);

            }
            else
            {
                gameManagerScript.UpdateGameState(GameState.Lose);
            }
        }
    }

    private IEnumerator ActivateInterrogationLetterAfterScale(GameObject obj)
    {
        yield return StartCoroutine(obj.transform.GetChild(0).GetComponent<ScaleAnimationScript>().Scale());

        // Desativa a letra
        obj.transform.GetChild(0).gameObject.SetActive(false);

        // Ativa a letra de interrogação
        obj.transform.GetChild(1).gameObject.SetActive(true);
    }
}
