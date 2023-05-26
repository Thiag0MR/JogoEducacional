using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;

    public enum GameState {
        Play,
        Pause,
        Score,
        SelectedWord,
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
                break;
            case GameState.Pause:
                HandlePause();
                break;
            case GameState.Score: 
                break;
            case GameState.SelectedWord:
                break;
            case GameState.Victory:
                break;
            case GameState.Lose:
                break;
        }

        OnGameStateChange?.Invoke(newState);
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
