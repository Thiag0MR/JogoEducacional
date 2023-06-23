using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vogais
{
    public class PauseMenuScript : MonoBehaviour
    {
        public void HandleCloseGameButton()
        {
            SceneManager.LoadSceneAsync("Vogais");
        }

        public void HandleCloseButton()
        {
            gameObject.SetActive(false);
        }
    }
}