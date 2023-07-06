using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameManager
{
    void UpdateGameState(int state);
    bool IsCorrectLetter(string letter);
}
