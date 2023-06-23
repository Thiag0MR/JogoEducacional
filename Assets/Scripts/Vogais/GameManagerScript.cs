using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Vogais
{
    public class GameManagerScript : MonoBehaviour
    {
        public static GameManagerScript Instance;

        [SerializeField]
        private GameObject textToSpeech;
        private TextToSpeechScript textToSpeechScript;

        private String[] vowelsArray = { "a", "e", "i", "o", "u" };

        /*
        private String[] victoryPhrases =
        {
            "Muito bem",
            "Isso mesmo",
            "Parabéns",
            "Ótimo",
            "Demais",
            "Você está indo muito bem",
            "Você acertou"
        };
        */

        private Dictionary<string, Letter> vowels = new Dictionary<string, Letter>();
        private int currentVowelIndex = 0;

        public enum GameState
        {
            Play,
            Pause,
            Score,
            NextVowel,
            Victory,
            Lose,
            GameEnd
        }

        public GameState state;

        public static event Action<GameState> OnGameStateChange;

        private AudioSource audioSource;

        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject bubbleSlot;

        void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            LoadManager.LoadVowels(vowels);
            textToSpeechScript = textToSpeech.GetComponent<TextToSpeechScript>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UpdateGameState(GameState.Pause);
            }
        }

        public void UpdateGameState(GameState newState)
        {
            state = newState;

            switch (newState)
            {
                case GameState.Play:
                    HandlePlay();
                    break;
                case GameState.Pause:
                    HandlePause();
                    break;
                case GameState.Score:
                    break;
                case GameState.NextVowel:
                    HandleNextLetter();
                    break;
                case GameState.Victory:
                    HandleVictory();
                    break;
                case GameState.Lose:
                    break;
                case GameState.GameEnd:
                    HandleGameEnd();
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private async void HandlePlay()
        {
            Util.Shuffle(vowelsArray);
            bubbleSlot.SetActive(true);
            await Task.Delay(1000);
            PlayVowel(vowelsArray[currentVowelIndex]);
        }

        // Como o painel de pausa inicia desativado na cena o mesmo não pode receber um evento. Sendo assim, é preciso manter uma
        // referência para o mesmo para poder ativá-lo.
        private void HandlePause()
        {
            if (!mainMenu.activeSelf && !pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                bubbleSlot.SetActive(false);
            }
            else
            {
                pauseMenu.SetActive(false);
                bubbleSlot.SetActive(true);
            }
        }

        private async void HandleNextLetter()
        {
            currentVowelIndex++;
            await Task.Delay(1000);
            if (currentVowelIndex < vowelsArray.Length)
            {
                PlayCurrentVowel();
            } else
            {
                Instance.UpdateGameState(GameState.GameEnd);
            }
        }

        private void HandleVictory()
        {
            //textToSpeechScript.Speak(victoryPhrases[UnityEngine.Random.Range(0, victoryPhrases.Length)]);
        }

        private async void HandleGameEnd()
        {
            await Task.Delay(2000);
            bubbleSlot.SetActive(false);
            pauseMenu.SetActive(true);
        }
        public void PlayCurrentVowel()
        {
            if (currentVowelIndex < vowelsArray.Length)
                PlayVowel(vowelsArray[currentVowelIndex]);
        }
        public void PlayVowel(string vowel)
        {
            AudioClip audioClip = vowels[vowel].audioClip;
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(audioClip);
        }
        public bool IsCorrectVowel(string vowel)
        {
            return vowel.Equals(vowelsArray[currentVowelIndex]);
        }
    }
}