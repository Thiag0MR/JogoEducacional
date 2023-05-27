using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainelConfiguracaoScript : MonoBehaviour
{
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

    }
}
