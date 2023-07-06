using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionMenuScript : MonoBehaviour
{
    [SerializeField] GameObject gameManager;
    private IGameManager gameManagerScript;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<IGameManager>();
    }

    public void HandleCloseButtonClick()
    {
        gameManagerScript.UpdateGameState(GameState.Instructions);
    }
}
