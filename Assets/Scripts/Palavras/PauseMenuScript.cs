using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Palavras
{
    public class PauseMenuScript : MonoBehaviour
    {
        public void HandleCloseButton()
        {
            GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.EndGame);
            SceneManager.LoadSceneAsync("Palavras");
        }

        public void HandleFecharPainelButtonClick()
        {
            GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Pause);
        }
    }
}