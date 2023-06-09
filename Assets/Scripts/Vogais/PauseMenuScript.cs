using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Vogais
{
    public class PauseMenuScript : MonoBehaviour
    {
        public void HandleCloseButton()
        {
            SceneManager.LoadSceneAsync("Vogais");
        }
    }
}