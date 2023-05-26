using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipalScript : MonoBehaviour
{
    public void HandleJogarVogal()
    {
        SceneManager.LoadSceneAsync("Vogais");
    }
}
