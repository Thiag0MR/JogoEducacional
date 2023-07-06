using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


namespace Consoantes
{
    public class BubbleGeneratorScript : MonoBehaviour
    {
        [SerializeField]
        private GameObject bubbleGroup, bubblePrefab;

        [SerializeField]
        private GameObject[] lettersPrefab;

        [SerializeField]
        private AudioClip creationSound;

        private AudioSource audioSource;
        private GameObject[] bubbleSlots;
        private readonly Dictionary<string, GameObject> letterNameToLetterPrefab = new();

        void Awake()
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            InitializeBubbleSlots();
            InitializeLetterNameToLetterPrefabDictionary();
        }

        private void InitializeLetterNameToLetterPrefabDictionary()
        {
            foreach(var letter in lettersPrefab)
            {
                letterNameToLetterPrefab.Add(letter.name.ToLower(), letter);
            }
        }

        private void InitializeBubbleSlots()
        {
            bubbleSlots = new GameObject[bubbleGroup.transform.childCount];

            for (int i = 0; i < bubbleSlots.Length; i++)
            {
                bubbleSlots[i] = bubbleGroup.transform.GetChild(i).gameObject;
            }
        }
        public async Task GenerateBubbles( Dictionary<string, Letter> consonants, string[] consonantsArray, int start)
        {
            Util.Shuffle(bubbleSlots);

            await Task.Delay(100);

            int index = start;
            for(int i = 0; i < bubbleSlots.Length; i++)
            {
                if (index == consonantsArray.Length) break;

                string consonantName = consonantsArray[index++];

                if (letterNameToLetterPrefab.ContainsKey(consonantName)) {
                    if (consonants.ContainsKey(consonantName))
                    {
                        GameObject bubble = Instantiate(bubblePrefab, bubbleSlots[i].transform);
                        GameObject letter = Instantiate(letterNameToLetterPrefab[consonantName], bubble.transform);

                        letter.name = consonantName;
                        letter.GetComponent<AudioSource>().clip = consonants[consonantName].audioClip;

                        if (SettingsMenuScript.BUBBLE_CREATION_SOUND)
                        {
                            audioSource.PlayOneShot(creationSound);
                        }
                        await Task.Delay(500);
                    } else
                    {
                        Debug.Log("A consoante " + consonantName + " não foi adicionada na área do professor!");
                    }
                } else
                {
                    Debug.Log("Não existe prefab para a respectiva consoante: " + consonantName);
                }
            }
        }
    }
}