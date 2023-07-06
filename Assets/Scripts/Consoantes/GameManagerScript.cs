using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Consoantes
{
    public class GameManagerScript : MonoBehaviour, IGameManager
    {
        public static GameManagerScript Instance;

        private readonly string[] consonantsArray = { 
            "b", "c", "d", 
            "f", "g", "h",
            "j", "k", "l",
            "m", "n", "p", 
            "q", "r", "s",
            "t", "v", "w",
            "x", "y", "z",
        };
        [SerializeField]
        private BubbleGeneratorScript bubbleGeneratorScript;

        [SerializeField]
        private SettingsMenuScript settingsScript;

        [SerializeField]
        private ParticleSystem firewordEffect;

        [SerializeField]
        private GameObject mainMenu, pauseMenu, settingsMenu, warningMenu, endGameMenu;

        [SerializeField]
        private Canvas gameCanvas;

        [SerializeField]
        private AudioClip rightLetterAudioClip, wrongLetterAudioClip;

        private readonly Dictionary<string, Letter> consonants = new();
        private int currentConsonantIndex = 0;

        public static event Action<int> OnGameStateChange;

        private AudioSource audioSource;

        async void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            settingsScript.LoadSettings();
            await LoadManager.LoadConsonants(consonants);
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
                    HandleGameEnd();
                    break;
                case GameState.Warning:
                    HandleWarning();
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private async void HandlePlay()
        {
            if (consonants.Count == 0)
            {
                UpdateGameState(GameState.Warning);
                return;
            }
            currentConsonantIndex = -1;
            Util.Shuffle(consonantsArray);
            gameCanvas.gameObject.SetActive(true);
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
                currentConsonantIndex++;
            } while (currentConsonantIndex < consonantsArray.Length && !consonants.ContainsKey(consonantsArray[currentConsonantIndex]));

            if (currentConsonantIndex < consonantsArray.Length)
            {
                await Task.Delay(1000);
                if (currentConsonantIndex % 3 == 0)
                {
                    await bubbleGeneratorScript.GenerateBubbles(consonants, consonantsArray, currentConsonantIndex);
                    await Task.Delay(500);
                }
                PlaySound(consonantsArray[currentConsonantIndex]);
            } else
            {
                Instance.UpdateGameState(GameState.EndGame);
            }
        }

        private void HandleVictory()
        {
            if (!firewordEffect.gameObject.activeSelf)
            {
                firewordEffect.gameObject.SetActive(true);
            }
            firewordEffect.Play();
            audioSource.PlayOneShot(rightLetterAudioClip, 0.2f);
        }
        private void HandleLose()
        {
            audioSource.PlayOneShot(wrongLetterAudioClip, 0.2f);
        }

        private void HandleGameEnd()
        {
            gameCanvas.gameObject.SetActive(false);
            endGameMenu.SetActive(true);
            firewordEffect.gameObject.SetActive(false);
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
            PlaySound(consonantsArray[currentConsonantIndex]);
        }

        private void PlaySound(string letter)
        {
            if (consonants.ContainsKey(letter))
            {
                AudioClip audioClip = consonants[letter].audioClip;
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
                audioSource.PlayOneShot(audioClip);
            }
        }
        public bool IsCorrectLetter(string vowel)
        {
            return vowel.Equals(consonantsArray[currentConsonantIndex]);
        }
    }
}