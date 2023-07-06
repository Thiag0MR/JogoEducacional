using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    public void HandleJogarVogal()
    {
        SceneManagerScript.Instance.LoadScene("Vogais");
    }
    public void HandleJogarConsoantes()
    {
        SceneManagerScript.Instance.LoadScene("Consoantes");
    }
    public void HandleJogarPalavras()
    {
        SceneManagerScript.Instance.LoadScene("Palavras");
    }
}
