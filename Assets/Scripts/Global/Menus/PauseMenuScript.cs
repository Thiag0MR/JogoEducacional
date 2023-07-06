using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    private IGameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<IGameManager>();
    }
    public void HandleCloseGameButtonClick(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void HandleCloseButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Pause);
    }

    public void HandleContinueButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Pause);
    }
}