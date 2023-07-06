using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Vogais
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

        void Awake()
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            InitializeBubbleSlots();
        }

        private void InitializeBubbleSlots()
        {
            bubbleSlots = new GameObject[bubbleGroup.transform.childCount];

            for (int i = 0; i < bubbleSlots.Length; i++)
            {
                bubbleSlots[i] = bubbleGroup.transform.GetChild(i).gameObject;
            }
        }
        public async Task GenerateBubbles(Dictionary<string, Letter> vowels)
        {
            Util.Shuffle(bubbleSlots);

            await Task.Delay(100);

            for (int i = 0; i < bubbleSlots.Length; i++)
            {
                string vowelName = lettersPrefab[i].name.ToLower();
                if (vowels.ContainsKey(vowelName))
                {
                    GameObject bubble = Instantiate(bubblePrefab, bubbleSlots[i].transform);
                    GameObject letter = Instantiate(lettersPrefab[i], bubble.transform);

                    letter.name = vowelName;
                    letter.GetComponent<AudioSource>().clip = vowels[vowelName].audioClip;

                    if (SettingsMenuScript.BUBBLE_CREATION_SOUND)
                    {
                        audioSource.PlayOneShot(creationSound);
                    }
                    await Task.Delay(500);
                } else
                {
                    Debug.Log("A vogal " + vowelName + " não foi adicionada na área do professor!");
                }
            }
        }
    }
}