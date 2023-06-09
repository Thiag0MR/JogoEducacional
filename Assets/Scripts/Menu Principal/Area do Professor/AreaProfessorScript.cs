using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaProfessorScript : MonoBehaviour
{
    [SerializeField]
    private GameObject objetosTelaPrincipal;

    [SerializeField]
    private GameObject[] tabs, pages;

    private Color selectedColor, notSelectedColor;

    void Awake()
    {
        TabControllerScript.OnTabClick += TabController_OnTabClick;
        selectedColor = new Color(0.000f, 0.361f, 0.776f, 0.392f);
        notSelectedColor = new Color(0.000f, 0.361f, 0.776f, 0.157f);
    }
    void OnDestroy()
    {
        TabControllerScript.OnTabClick -= TabController_OnTabClick;
    }

    private void TabController_OnTabClick(int index)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == index)
            {
                pages[i].SetActive(true);
                tabs[i].GetComponent<Image>().color = selectedColor;
            } else
            {
                pages[i].SetActive(false);
                tabs[i].GetComponent<Image>().color = notSelectedColor;
            }
        }
    }

    public void HandleBotaoFecharClick()
    {
        gameObject.SetActive(false);
        objetosTelaPrincipal.SetActive(true);
        objetosTelaPrincipal.transform.Find("Menus").gameObject.SetActive(true);
    }
}
