using Palavras;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordVictoryPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject wordAudio;

    public void HandlePlayWordButtonClick()
    {
        wordAudio.GetComponent<AudioSource>().Play();
    }

    public void HandleNextWordButtonClick()
    {
        GameManagerScript.Instance.UpdateGameState(GameManagerScript.GameState.NextWord);
    }
}
