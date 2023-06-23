using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Palavras
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
            SceneManagerScript.Instance.LoadScene("Menu Principal");
        }
    }
}