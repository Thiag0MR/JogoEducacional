using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    public void HandleJogarVogal()
    {
        SceneManager.LoadSceneAsync("Vogais");
    }
    public void HandleJogarPalavras()
    {
        SceneManager.LoadSceneAsync("Palavras");
    }
}
