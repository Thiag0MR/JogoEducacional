using System;
using UnityEngine;

namespace Vogais
{
    public class GameManagerScript : MonoBehaviour
    {
        public static GameManagerScript Instance;

        private String[] vogais = { "A", "E", "I", "O", "U" };
        private int round = 0;

        public enum GameState
        {
            Play,
            Pause,
            Score,
            NextLetter,
            Victory,
            Lose
        }

        public GameState state;

        public static event Action<GameState> OnGameStateChange;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject bubbleSlot;

        public void Awake()
        {
            Instance = this;
            Util.Shuffle(vogais);
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
                case GameState.NextLetter:
                    HandleNextLetter();
                    break;
                case GameState.Victory:
                    HandleVictory();
                    break;
                case GameState.Lose:
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private void HandlePlay()
        {
            bubbleSlot.SetActive(true);
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

        private void HandleNextLetter()
        {
            round++;
            Debug.Log(System.Environment.Version);
            //TextToSpeech.Speak (vogais[round]);
        }

        private void HandleVictory()
        {
            // Executar áudio
        }
    }
}