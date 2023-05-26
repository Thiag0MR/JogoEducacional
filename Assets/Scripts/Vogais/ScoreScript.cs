using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{   
    [SerializeField]
    private Sprite[] numbers;

    private int score = 0;

    void Awake()
    {
        GameManagerScript.OnGameStateChange += GameManagerScript_OnGameStateChange;
    }

    void OnDestroy()
    {
        GameManagerScript.OnGameStateChange -= GameManagerScript_OnGameStateChange;
    }

    private void GameManagerScript_OnGameStateChange(GameManagerScript.GameState state)
    {
        if (state == GameManagerScript.GameState.Score)
        {
            UpdateScore();
        } else if (state == GameManagerScript.GameState.Play)
        {
            CreateScoreUI(0);
        }
    }
    private void UpdateScore()
    {
        this.score++;
        CreateScoreUI(score);
    }

    private void CreateScoreUI(int score)
    {
        LinkedList<int> algarismos = new LinkedList<int>();

        if (score == 0)
        {
            algarismos.AddFirst(0);
        } else
        {
            while (score % 10 != 0){
                algarismos.AddFirst(score % 10);
                score /= 10;
            }
        }

        int childIndex = 2;
        while(algarismos.Count > 0)        
        {
            int alg = algarismos.Last.Value;
            algarismos.RemoveLast();
            GameObject child = this.gameObject.transform.GetChild(childIndex--).gameObject;
            child.GetComponent<Image>().sprite = numbers[alg];           
            //Debug.Log(alg.ToString());
        }
    }

}
