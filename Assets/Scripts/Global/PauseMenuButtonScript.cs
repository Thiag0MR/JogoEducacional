using Palavras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButtonScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameManagerScript gameManagerScript;
    public void HandlePauseMenuButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Pause);
    }
}
