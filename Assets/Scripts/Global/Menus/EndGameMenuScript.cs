using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenuScript : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    private IGameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<IGameManager>();
    }

    public void HandleNextPhaseButtonClick(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void HandlePlayAgainButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Play);
        gameObject.SetActive(false);
    }

    public void HandleCloseGameButtonClick(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
