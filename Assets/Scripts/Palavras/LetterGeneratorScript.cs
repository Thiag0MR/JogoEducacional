using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

public class LetterGeneratorScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lettersPrefab;

    /*
    [SerializeField]
    private GameObject letterParent;
    */

    [SerializeField]
    private GameObject letters1, letters2, waitUntilDestroy;

    private GameObject[] letterSlots;
    private int qtdLetterSlots;

    private Dictionary<string, GameObject> letterNameToLetterPrefab = new Dictionary<string, GameObject>();

    void Awake()
    {
        InitializeLetterNameToLetterPrefabDictionary();
        InitializeLetterSlots();
    }

    private void InitializeLetterNameToLetterPrefabDictionary()
    {
        foreach(var letter in lettersPrefab)
        {
            letterNameToLetterPrefab.Add(letter.name.ToLower(), letter);
        }
    }

    private void InitializeLetterSlots()
    {
        qtdLetterSlots = letters1.transform.childCount + letters2.transform.childCount; 
        letterSlots = new GameObject[qtdLetterSlots];
        int index = 0;
        for (int i = 0; i < letters1.transform.childCount; i++)
        {
            letterSlots[index++] = letters1.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < letters2.transform.childCount; i++)
        {
            letterSlots[index++] = letters2.transform.GetChild(i).gameObject;
        }
    }
    public void CleanLettersOnView()
    {
        for (int i = 0; i < qtdLetterSlots; i++)
        {
            if (letterSlots[i].transform.childCount > 0)
            {
                GameObject letterInView = letterSlots[i].transform.GetChild(0).gameObject;
                letterInView.SetActive(false);
                letterInView.transform.SetParent(waitUntilDestroy.transform, false);
                Destroy(letterInView);
            }
        }
    }

    public void GenerateLettersOnView(Dictionary<string, Letter> letters, string word)
    {
        CleanLettersOnView();

        System.Random random = new System.Random();
        char[] wordArray = word.ToCharArray();
        Util.Shuffle(wordArray);
        Util.Shuffle(letterSlots);
        int index = 0;

        // Gera primeiro as letras que compõem a palavra
        foreach(var letter in wordArray)  
        {
            if (letterNameToLetterPrefab.ContainsKey(letter.ToString()))
            {
                GameObject obj = Instantiate(letterNameToLetterPrefab[letter.ToString()], letterSlots[index++].transform);
                obj.name = letter.ToString();

                // Verifica se a letra foi adicionada na área do professor e existe o audioclip.
                if (letters.ContainsKey(obj.name)) {
                    obj.GetComponent<AudioSource>().clip = letters[obj.name].audioClip;
                } else {
                    Debug.Log("A letra " + obj.name + " não foi adicionada na área do professor. Favor adicionar!");
                }
            } else
            {
                Debug.Log("Não existe prefab para a respectiva letra: " + letter.ToString());
            }
        }

        // Complementa com letras aleatórias
        for (int i = 0; i < qtdLetterSlots; i++)
        {
            //Debug.Log(letterSlots[i].name + " " + letterSlots[i].transform.childCount);
            if (letterSlots[i].transform.childCount == 0)
            {
                int randomIndex = random.Next(lettersPrefab.Length);
                GameObject obj = Instantiate(lettersPrefab[randomIndex], letterSlots[i].transform);
                obj.name = lettersPrefab[randomIndex].name.ToLower();
                if (letters.ContainsKey(obj.name))
                {
                    obj.GetComponent<AudioSource>().clip = letters[obj.name].audioClip;
                }
                else
                {
                    Debug.Log("A letra " + obj.name + " não foi adicionada. Favor adicionar!");
                }
            }
        }
    }
}
