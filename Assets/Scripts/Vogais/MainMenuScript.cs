using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vogais
{
    public class MainMenuScript : MonoBehaviour
    {
        public void HandlePlayButton()
        {
            gameObject.SetActive(false);
            GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.Play);
        }

        public void HandleCloseButton()
        {
            //SceneManager.LoadScene("Menu Principal");
            SceneManager.LoadSceneAsync("Menu Principal");
        }
    }
}