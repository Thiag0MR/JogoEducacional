using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Vogais
{
    public class GameManagerScript : MonoBehaviour, IGameManager
    {
        public static GameManagerScript Instance;

        private readonly string[] vowelsArray = { "a", "e", "i", "o", "u" };

        [SerializeField]
        private BubbleGeneratorScript bubbleGeneratorScript;

        [SerializeField]
        private SettingsMenuScript settingsMenuScript;

        [SerializeField]
        private ParticleSystem fireworkEffect;

        [SerializeField]
        private GameObject mainMenu, pauseMenu, settingsMenu, endGameMenu, warningMenu;

        [SerializeField]
        private Canvas gameCanvas;

        [SerializeField]
        private AudioClip rightLetterAudioClip, wrongLetterAudioClip;

        private readonly Dictionary<string, Letter> vowels = new();
        private int currentVowelIndex = 0;

        public static event Action<int> OnGameStateChange;

        private AudioSource audioSource;

        async void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            settingsMenuScript.LoadSettings();
            await LoadManager.LoadVowels(vowels);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UpdateGameState(GameState.Pause);
            }
        }

        public void UpdateGameState(int newState)
        {
            switch (newState)
            {
                case GameState.Play:
                    HandlePlay();
                    break;
                case GameState.Pause:
                    HandlePause();
                    break;
                case GameState.Settings:
                    HandleSettings();
                    break;
                case GameState.Score:
                    break;
                case GameState.NextLetter:
                    HandleNextLetter();
                    break;
                case GameState.Victory:
                    HandleVictory();
                    break;
                case GameState.Lose:
                    HandleLose();
                    break;
                case GameState.EndGame:
                    HandleEndGame();
                    break;
                case GameState.Warning:
                    HandleWarning();
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private async void HandlePlay()
        {
            if (vowels.Count == 0)
            {
                UpdateGameState(GameState.Warning);
                return;
            }
            currentVowelIndex = -1;
            Util.Shuffle(vowelsArray);
            gameCanvas.gameObject.SetActive(true);
            await bubbleGeneratorScript.GenerateBubbles(vowels);
            UpdateGameState(GameState.NextLetter);
        }

        private void HandlePause()
        {
            if (!mainMenu.activeSelf && !pauseMenu.activeSelf && !settingsMenu.activeSelf && !warningMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
                //fireworkEffect.gameObject.SetActive(false);
                //gameCanvas.gameObject.SetActive(false);
            }
            else if (!mainMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                //gameCanvas.gameObject.SetActive(true);
            }
        }

        private void HandleSettings()
        {
            if (!settingsMenu.activeSelf)
            {
                mainMenu.SetActive(false);
                settingsMenu.SetActive(true);
            } else
            {
                mainMenu.SetActive(true);
                settingsMenu.SetActive(false);
            }
        }

        private async void HandleNextLetter()
        {
            do
            {
                currentVowelIndex++;
            } while (currentVowelIndex < vowelsArray.Length && !vowels.ContainsKey(vowelsArray[currentVowelIndex]));

            if (currentVowelIndex < vowelsArray.Length)
            {
                await Task.Delay(1000);
                PlaySound(vowelsArray[currentVowelIndex]);
            } else
            {
                Instance.UpdateGameState(GameState.EndGame);
            }
        }

        private void HandleVictory()
        {
            if (!fireworkEffect.gameObject.activeSelf)
            {
                fireworkEffect.gameObject.SetActive(true);
            }
            fireworkEffect.Play();
            audioSource.PlayOneShot(rightLetterAudioClip, 0.2f);
        }
        private void HandleLose()
        {
            audioSource.PlayOneShot(wrongLetterAudioClip, 0.2f);
        }

        private void HandleEndGame()
        {
            gameCanvas.gameObject.SetActive(false);
            endGameMenu.SetActive(true);
            fireworkEffect.gameObject.SetActive(false);
        }

        private void HandleWarning()
        {
            if (!warningMenu.activeSelf)
            {
                warningMenu.SetActive(true);
            } else
            {
                warningMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
        public void PlaySound()
        {
            PlaySound(vowelsArray[currentVowelIndex]);
        }
        private void PlaySound(string letter)
        {
            if (vowels.ContainsKey(letter)) {
                AudioClip audioClip = vowels[letter].audioClip;
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.PlayOneShot(audioClip);
            }
        }
        public bool IsCorrectLetter(string vowel)
        {
            return vowel.Equals(vowelsArray[currentVowelIndex]);
        }
    }
}