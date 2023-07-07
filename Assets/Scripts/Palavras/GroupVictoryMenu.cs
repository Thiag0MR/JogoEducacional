using Palavras;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupVictoryMenu : MonoBehaviour
{
    public void HandleCloseButtonClick()
    {
        GameManagerScript.Instance.UpdateGameState(GameState.EndGame);
    }
}
