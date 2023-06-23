using Palavras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Letter"))
        {
            animator.SetBool("isLetterAbove", true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Letter"))
        {
            animator.SetBool("isLetterAbove", false);
            if (other.gameObject.GetComponent<LetterScript>().IsFalling)
            {
                other.gameObject.SetActive(false);
                Destroy(other.gameObject);
                other.gameObject.GetComponent<LetterScript>().IsFalling = false;
                
                if (GameManagerScript.Instance.IsCorrectLetter(other.name))
                {
                    if (GameManagerScript.Instance.IsLetterRemaining(other.name))
                    {
                        GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.RightLetter);
                        GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Score);
                        GameManagerScript.Instance.UpdateWordPanel(other.name);
                        GameManagerScript.Instance.UpdateLettersRemaining(other.name);
                    }
                } 
                else
                {
                    GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.WrongLetter);
                }
            }
        }
    }
}
