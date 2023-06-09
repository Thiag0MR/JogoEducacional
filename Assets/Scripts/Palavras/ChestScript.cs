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
            other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Victory);
            GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.NextWord);
        }
    }
}
