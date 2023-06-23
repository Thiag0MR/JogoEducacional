using Assets.SimpleSpinner;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Palavras
{
    public class GameManagerScript : MonoBehaviour
    {
        public static GameManagerScript Instance;

        public enum GameState
        {
            Play,
            StartGame,
            EndGame,
            Pause,
            Score,
            NextWord,
            RightLetter,
            WrongLetter,
            WordVictory,
            Lose, 
            GroupOfWordsCreated
        }

        public GameState state;

        public static GameState CurrentState;

        public static event Action<GameState> OnGameStateChange;

        private AudioSource audioSource;

        [SerializeField]
        private GameObject gameCanvas, mainMenu, pauseMenu, selectGroupMenu, wordPanel, wordVictoryPanel;

        [SerializeField]
        private AudioClip wordVictoryClip;

        [SerializeField]
        private LetterGeneratorScript letterGeneratorScript;

        [SerializeField]
        private WordPanelScript wordPanelScript;

        
        // Estado do jogo
        private Dictionary<string, Letter> letters = new Dictionary<string, Letter>();
        private List<Word> words;
        private Word currentWord;
        private string wordNameWithoutDiacritics;
        private int currentWordIndex = 0;
        private bool groupOfWordsCreated = false;
        private bool gameStarted = false;
        private int numberOfLettersRemaining;
        private string lettersRemaining;

        public async void Awake()
        {
            Instance = this;
            GroupItemScript.OnSelecionarButtonClick += GroupItemScript_OnSelecionarButtonClick;
            await LoadManager.LoadVowels(letters);
            await LoadManager.LoadConsonants(letters);
            audioSource = GetComponent<AudioSource>();
        }

        void OnDestroy()
        {
            GroupItemScript.OnSelecionarButtonClick -= GroupItemScript_OnSelecionarButtonClick;
        }

        private async void GroupItemScript_OnSelecionarButtonClick(string groupName)
        {
            words = await LoadManager.LoadWords(groupName);
            ShuffleWords();
            UpdateGameState(GameState.StartGame);
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
                case GameState.StartGame:
                    HandleStartGame();
                    break;
                case GameState.EndGame:
                    HandleEndGame();
                    break;
                case GameState.Pause:
                    HandlePause();
                    break;
                case GameState.Score:
                    HandleScore();
                    break;
                case GameState.NextWord:
                    HandleNextWord();
                    break;
                case GameState.RightLetter:
                    HandleRightLetter();
                    break;
                case GameState.WrongLetter:
                    HandleWrongLetter();
                    break;
                case GameState.WordVictory:
                    HandleWordVictory();
                    break;
                case GameState.Lose:
                    break;
                case GameState.GroupOfWordsCreated:
                    HandleGroupOfWordsCreated();
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private void HandlePlay()
        {
            if (groupOfWordsCreated)
            {
                selectGroupMenu.transform.GetChild(0).gameObject.SetActive(true);
            } 
            else
            {
                selectGroupMenu.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        private void HandleStartGame()
        {
            currentWord = words[currentWordIndex];
            numberOfLettersRemaining = CalculateNumberOfLettersRemaining(currentWord.name);
            wordNameWithoutDiacritics = Util.RemoveDiacritics(currentWord.name);
            lettersRemaining = wordNameWithoutDiacritics;
            gameCanvas.SetActive(true);
            selectGroupMenu.transform.GetChild(0).gameObject.SetActive(false);
            letterGeneratorScript.GenerateLettersOnView(letters, wordNameWithoutDiacritics);
            wordPanelScript.GenerateWordPanel(currentWord);
            gameStarted = true;
        }

        private void HandleEndGame()
        {
            gameStarted = false;
            gameCanvas.SetActive(false);
            mainMenu.SetActive(true);
        }

        private void HandlePause()
        {
            if (gameStarted)
            {
                if (!pauseMenu.activeSelf)
                {
                    pauseMenu.SetActive(true);
                    gameCanvas.SetActive(false);
                }
                else
                {
                    pauseMenu.SetActive(false);
                    gameCanvas.SetActive(true);
                }
            }
        }
        private void HandleScore()
        {
            // ScoreScript
        }
        private void HandleNextWord()
        {
            currentWordIndex++;
            if (currentWordIndex > words.Count - 1)
            {
                // Chegou no final da lista de palavras. Mostrar painel jogar novamente
                Instance.UpdateGameState(GameState.EndGame);
            }
            currentWord = words[currentWordIndex];
            numberOfLettersRemaining = CalculateNumberOfLettersRemaining(currentWord.name);
            wordNameWithoutDiacritics = Util.RemoveDiacritics(currentWord.name);
            lettersRemaining = wordNameWithoutDiacritics;
            letterGeneratorScript.GenerateLettersOnView(letters, wordNameWithoutDiacritics);
            wordPanelScript.CleanWordPanel();
            wordPanelScript.GenerateWordPanel(currentWord);
            wordVictoryPanel.SetActive(false);
            gameCanvas.transform.GetChild(1).Find("Player").gameObject.SetActive(true);
        }
        private void HandleRightLetter()
        {
            numberOfLettersRemaining--;
            if (numberOfLettersRemaining == 0)
            {
                Instance.UpdateGameState(GameState.WordVictory);
            }
        }

        private void HandleWrongLetter()
        {
            Debug.Log("Wrong letter");
        }

        private void HandleWordVictory()
        {
            wordVictoryPanel.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = currentWord.image;
            wordVictoryPanel.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentWord.name;
            wordVictoryPanel.transform.GetChild(1).transform.GetChild(2).GetComponent<AudioSource>().clip = currentWord.audioClip;
            wordVictoryPanel.SetActive(true);
            audioSource.PlayOneShot(wordVictoryClip);
            gameCanvas.transform.GetChild(1).Find("Player").gameObject.SetActive(false);
        }
        private void HandleLose()
        {

        }
        private void HandleGroupOfWordsCreated()
        {
            groupOfWordsCreated = true;
        }
        public void HandleCloseButtonClickNoGroupOfWordsWarning()
        {
            selectGroupMenu.transform.GetChild(1).gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        public void HandleWordAudioButtonClick()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(currentWord.audioClip);
        }

        internal bool IsCorrectLetter(string letter)
        {
            return wordNameWithoutDiacritics.Contains(letter);
        }

        internal bool IsLetterRemaining(string letter)
        {
            return lettersRemaining.Contains(letter);
        }

        internal void UpdateLettersRemaining(string letter)
        {
            int index = lettersRemaining.IndexOf(letter);
            if (index != -1)
            {
                lettersRemaining = lettersRemaining.Substring(0, index) + lettersRemaining.Substring(index + 1);
            }
        }

        internal void UpdateWordPanel(string letter)
        {
            wordPanelScript.UpdateWordPanel(currentWord, wordNameWithoutDiacritics, letter);
        }

        private int CalculateNumberOfLettersRemaining(string wordName)
        {
            int count = 0;
            for(int i = 0; i < wordName.Length; i++)
            {
                if (IsLetterValid(wordName[i]))
                {
                    count++;
                }
            }
            return count;
        }

        internal bool IsLetterValid(char letter)
        {
            switch (letter)
            {
                case ' ':
                    return false;
                default:
                    return true;
            }
        }

        private void ShuffleWords()
        {
            Word[] wordsArray = words.ToArray();
            Util.Shuffle(wordsArray);
            words = wordsArray.ToList();
        }
    }
}