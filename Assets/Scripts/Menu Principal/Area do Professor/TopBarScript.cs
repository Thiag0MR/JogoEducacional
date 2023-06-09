using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TopBarScript : MonoBehaviour
{
    public static event Action OnAdicionarButtonClick;

    [SerializeField]
    private GameObject[] pages;

    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private Button botaoLimparBusca;

    void Update()
    {
        if (inputField.text != "" && !botaoLimparBusca.gameObject.activeSelf)
        {
            botaoLimparBusca.gameObject.SetActive(true);
        } else if (inputField.text == "" && botaoLimparBusca.gameObject.activeSelf)
        {
            botaoLimparBusca.gameObject.SetActive(false);
        }
    }
    public void HandleLimparBuscaButtonClick()
    {
        foreach(var page in pages)
        {
            if (page.activeSelf && page.name.Equals("Page 1 - Vogais")) {
                page.GetComponent<VogalControllerScript>().ResetSearch();
            } else if (page.activeSelf && page.name.Equals("Page 2 - Consoantes")) {
                page.GetComponent<ConsoanteControllerScript>().ResetSearch();
            }
            if (page.activeSelf && page.name.Equals("Page 3 - Sílabas")) {
                page.GetComponent<SilabaControllerScript>().ResetSearch();
            }
            if (page.activeSelf && page.name.Equals("Page 4 - Palavras")) {
                page.GetComponent<PalavraControllerScript>().ResetSearch();
            }
        }
        inputField.text = "";
        botaoLimparBusca.gameObject.SetActive(false);
    }

    public void HandleBuscarButtonClick ()
    {
        foreach(var page in pages)
        {
            if (page.activeSelf && page.name.Equals("Page 1 - Vogais")) {
                page.GetComponent<VogalControllerScript>().HandleBuscarButtonClick(inputField.text);
            } else if (page.activeSelf && page.name.Equals("Page 2 - Consoantes")) {
                page.GetComponent<ConsoanteControllerScript>().HandleBuscarButtonClick(inputField.text);
            }
            if (page.activeSelf && page.name.Equals("Page 3 - Sílabas")) {
                page.GetComponent<SilabaControllerScript>().HandleBuscarButtonClick(inputField.text);
            }
            if (page.activeSelf && page.name.Equals("Page 4 - Palavras")) {
                page.GetComponent<PalavraControllerScript>().HandleBuscarButtonClick(inputField.text);
            }
        }
    }
    public void HandleAdicionarButtonClick ()
    {
        OnAdicionarButtonClick?.Invoke();
    }
}
