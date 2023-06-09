using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Palavras
{
    public class GameManagerScript : MonoBehaviour
    {
        public static GameManagerScript Instance;

        public enum GameState
        {
            Play,
            Pause,
            Score,
            NextWord,
            Victory,
            Lose
        }

        public GameState state;

        public static GameState CurrentState;

        public static event Action<GameState> OnGameStateChange;

        [SerializeField]
        private AudioSource audioSource;

        [SerializeField]
        private GameObject mainMenu, pauseMenu, chest, player, letters;

        public void Awake()
        {
            Instance = this;
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
                case GameState.NextWord:
                    break;
                case GameState.Victory:
                    break;
                case GameState.Lose:
                    break;
            }

            OnGameStateChange?.Invoke(newState);
        }

        private void HandlePlay()
        {
            player.SetActive(true);
            chest.SetActive(true);
            letters.SetActive(true);
        }

        private void HandlePause()
        {
            if (!mainMenu.activeSelf && !pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                pauseMenu.SetActive(false);
            }
        }
    }
}