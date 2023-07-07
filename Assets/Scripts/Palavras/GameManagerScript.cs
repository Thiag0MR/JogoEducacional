using Assets.SimpleSpinner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Palavras
{
    public class GameManagerScript : MonoBehaviour, IGameManager
    {
        public static GameManagerScript Instance;

        public static event Action<int> OnGameStateChange;

        private AudioSource audioSource;
        private enum Warning
        {
            NoGroupOfWords,
            NoWords
        }

        private Warning currentWarningToShow = Warning.NoGroupOfWords;

        [SerializeField]
        private GameObject gameCanvas, mainMenu, pauseMenu, selectGroupMenu, wordVictoryMenu, groupVictoryMenu, instructionMenu, wordPanel;

        [SerializeField]
        private GameObject warningNoGroupOfWords, warningNoWords;

        [SerializeField]
        private AudioClip wordVictoryClip, rightLetterClip, wrongLetterClip;

        [SerializeField]
        private LetterGeneratorScript letterGeneratorScript;

        [SerializeField]
        private WordPanelScript wordPanelScript;

        [SerializeField]
        private SimpleSpinner spinner;

        [SerializeField]
        private ParticleSystem fireworkEffect;

        
        // Estado do jogo
        private readonly Dictionary<string, Letter> letters = new();
        private List<Word> words;
        private Word currentWord;
        private string wordNameWithoutDiacritics;
        private int currentWordIndex;
        private bool groupOfWordsCreated;
        private bool contentFinishedLoading;
        private bool gameStarted;
        private int numberOfLettersRemaining;
        private string lettersRemaining;

        public async void Awake()
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            GroupItemScript.OnSelecionarButtonClick += GroupItemScript_OnSelecionarButtonClick;
            await LoadManager.LoadVowels(letters);
            await LoadManager.LoadConsonants(letters);
        }
        private void Start()
        {
            currentWordIndex = -1;
            groupOfWordsCreated = false;
            contentFinishedLoading = false;
            gameStarted = false;
        }

        void OnDestroy()
        {
            GroupItemScript.OnSelecionarButtonClick -= GroupItemScript_OnSelecionarButtonClick;
        }

        private async void GroupItemScript_OnSelecionarButtonClick(string groupName)
        {
            words = await LoadManager.LoadWords(groupName);
            if (words != null && words.Count > 0)
            {
                ShuffleWords();
                UpdateGameState(GameState.StartGame);
            } else
            {
                ShowWarning(Warning.NoWords);
            }
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
                case GameState.ContentFinishedLoading:
                    HandleContentFinishedLoading();
                    break;
                case GameState.GroupOfWordsCreated:
                    HandleGroupOfWordsCreated();
                    break;
                case GameState.Warning:
                    HandleWarning();
                    break;
                case GameState.Instructions:
                    HandleInstructions();
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private async void HandlePlay()
        {
            mainMenu.SetActive(false);
            spinner.gameObject.SetActive(true);
            while (!contentFinishedLoading)
            {
                await Task.Delay(500);
            }
            spinner.gameObject.SetActive(false);

            if (groupOfWordsCreated)
            {
                selectGroupMenu.transform.GetChild(0).gameObject.SetActive(true);
            } 
            else
            {
                ShowWarning(Warning.NoGroupOfWords);
            }
        }

        private void HandleStartGame()
        {
            selectGroupMenu.transform.GetChild(0).gameObject.SetActive(false);
            gameCanvas.SetActive(true);
            gameStarted = true;
            UpdateGameState(GameState.NextWord);
        }
        private async void HandleNextWord()
        {
            currentWordIndex++;
            if (currentWordIndex < words.Count)
            {
                currentWord = words[currentWordIndex];
                numberOfLettersRemaining = CalculateNumberOfLettersRemaining(currentWord.name);
                wordNameWithoutDiacritics = Util.RemoveDiacritics(currentWord.name);
                lettersRemaining = wordNameWithoutDiacritics;
                if (currentWordIndex != 0)
                {
                    wordPanelScript.CleanWordPanel();
                    wordVictoryMenu.SetActive(false);
                    gameCanvas.transform.GetChild(1).Find("Player").gameObject.SetActive(true);
                }
                wordPanelScript.GenerateWordPanel(currentWord);
                letterGeneratorScript.GenerateLettersOnView(letters, wordNameWithoutDiacritics);

                await Task.Delay(1000);
                PlayCurrentWord();
            } else
            {
                Instance.UpdateGameState(GameState.EndGame);
            }
        }

        private void HandleEndGame()
        {
            if (gameStarted)
            {
                gameStarted = false;
                gameCanvas.SetActive(false);
                wordVictoryMenu.SetActive(false);
                groupVictoryMenu.SetActive(true);
            } else
            {
                groupVictoryMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }

        private void HandlePause()
        {
            if (gameStarted)
            {
                if (!pauseMenu.activeSelf && !selectGroupMenu.transform.GetChild(0).gameObject.activeSelf 
                    && !wordVictoryMenu.activeSelf && !groupVictoryMenu.activeSelf && !instructionMenu.activeSelf)
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
        
        private void HandleRightLetter()
        {
            numberOfLettersRemaining--;
            if (numberOfLettersRemaining == 0)
            {
                Instance.UpdateGameState(GameState.WordVictory);
            }
            audioSource.PlayOneShot(rightLetterClip, 0.2f);
        }

        private void HandleWrongLetter()
        {
            audioSource.PlayOneShot(wrongLetterClip, 0.2f);
        }
        private void HandleWordVictory()
        {
            wordVictoryMenu.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = currentWord.image;
            wordVictoryMenu.transform.GetChild(1).transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentWord.name;
            wordVictoryMenu.transform.GetChild(1).transform.GetChild(2).GetComponent<AudioSource>().clip = currentWord.audioClip;
            wordVictoryMenu.SetActive(true);
            audioSource.PlayOneShot(wordVictoryClip, 0.2f);
            if (!fireworkEffect.gameObject.activeSelf)
            {
                fireworkEffect.gameObject.SetActive(true);
            }
            fireworkEffect.Play();
            gameCanvas.transform.GetChild(1).Find("Player").gameObject.SetActive(false);
        }
        private void HandleLose()
        {

        }
        private void HandleContentFinishedLoading()
        {
            contentFinishedLoading = true;
        }

        private void HandleGroupOfWordsCreated()
        {
            groupOfWordsCreated = true;
        }

        private void ShowWarning(Warning warning)
        {
            currentWarningToShow = warning;
            UpdateGameState(GameState.Warning);
        }
        public void HandleWarning()
        {
            if (currentWarningToShow.Equals(Warning.NoGroupOfWords))
            {
                if (!warningNoGroupOfWords.activeSelf)
                {
                    warningNoGroupOfWords.SetActive(true);
                } else
                {
                    warningNoGroupOfWords.SetActive(false);
                    mainMenu.SetActive(true);
                }
            } else if (currentWarningToShow.Equals(Warning.NoWords))
            {
                if (!warningNoWords.activeSelf)
                {
                    selectGroupMenu.transform.GetChild(0).gameObject.SetActive(false);
                    warningNoWords.SetActive(true);
                } else
                {
                    warningNoWords.SetActive(false);
                    mainMenu.SetActive(true);
                }
            }
        }
        private void HandleInstructions()
        {
            if (!instructionMenu.activeSelf)
            {
                mainMenu.SetActive(false);
                instructionMenu.SetActive(true);
            } else
            {
                instructionMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
        }
        public void HandleWordAudioButtonClick()
        {
            PlayCurrentWord();
        }

        private void PlayCurrentWord()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            audioSource.PlayOneShot(currentWord.audioClip);
        }

        public bool IsCorrectLetter(string letter)
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