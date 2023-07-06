using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    private IGameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<IGameManager>();
    }
    public void HandlePlayButtonClick()
    {
        gameObject.SetActive(false);
        gameManagerScript.UpdateGameState(GameState.Play);
    }

    public void HandleSettingsButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Settings);
    }

    public void HandleCloseGameButtonClick(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}