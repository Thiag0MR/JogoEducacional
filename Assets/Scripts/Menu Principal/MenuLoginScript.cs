using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuLoginScript : MonoBehaviour
{
    [SerializeField]
    private GameObject menus;

    [SerializeField]
    private TMP_InputField username, password;

    [SerializeField]
    private GameObject professorArea, objetosTelaPrincipal;

    [SerializeField]
    private AudioSource audioSource;

    public void HandleCloseButton()
    {
        gameObject.SetActive(false);
        menus.SetActive(true);
    }
    public void HandleEntrarButton ()
    {
        if (username.text.Equals("admin") &&  password.text.Equals("admin"))
        {
            professorArea.SetActive(true);
            objetosTelaPrincipal.SetActive(false);
            gameObject.SetActive(false);
            username.text = "";
            password.text = "";
            audioSource.Stop();
        }
    }

}
