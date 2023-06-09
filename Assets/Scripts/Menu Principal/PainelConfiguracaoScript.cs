using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainelConfiguracaoScript : MonoBehaviour
{
    [SerializeField]
    private GameObject menuLogin;

    [SerializeField]
    private GameObject menus;

    private GameObject botaoAreaProfessor;

    private void Awake()
    {
        botaoAreaProfessor = gameObject.transform.GetChild(0).gameObject;
        botaoAreaProfessor.SetActive(!Application.isMobilePlatform);
    }
    public void HandleConfigButtonClick()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        } else
        {
            gameObject.SetActive(true);
        }
    }

    public void HandleCloseButtonClick()
    {
        Application.Quit();
    }

    public void HandleProfessorAreaClick()
    {
        menuLogin.SetActive(true);
        gameObject.SetActive(!gameObject.activeSelf);
        menus.SetActive(false);
    }
}
