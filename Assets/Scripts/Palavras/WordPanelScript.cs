using Palavras;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WordPanelScript : MonoBehaviour
{
    [SerializeField]
    private GameObject letterPanel, letterPanelTextPrefab, wordImage;

    internal void CleanWordPanel()
    {
        if (letterPanel.transform.childCount > 0)
        {
            for (int i = 0; i < letterPanel.transform.childCount; i++)
            {
                Destroy(letterPanel.transform.GetChild(i).gameObject);
            }
        }
    }
    internal void GenerateWordPanel(Word currentWord)
    {
        string word = currentWord.name;
        for (int i = 0; i < word.Length; i++)
        {
            GameObject obj = Instantiate(letterPanelTextPrefab, letterPanel.transform);
            if (!GameManagerScript.Instance.IsLetterValid(word[i]))
            {
                obj.transform.GetChild(0).gameObject.SetActive(false);
            }
            obj.name = word[i].ToString();
        }
        wordImage.GetComponent<Image>().sprite = currentWord.image;
    }

    internal void UpdateWordPanel(Word currentWord, string wordNameWithoutDiacritics, string letter)
    {
        string word = wordNameWithoutDiacritics;
        for (int i = 0; i < word.Length; i++)
        {
            Transform letterPanelText = letterPanel.transform.GetChild(i);

            if (word[i].ToString().Equals(letter) && letterPanelText.GetComponent<TextMeshProUGUI>().text.Equals(""))
            {
                letterPanelText.GetComponent<TextMeshProUGUI>().text = currentWord.name[i].ToString();
                letterPanelText.transform.GetChild(0).gameObject.SetActive(false); // Desativa a borda embaixo da letra
                break;
            }
        }
    }
}
