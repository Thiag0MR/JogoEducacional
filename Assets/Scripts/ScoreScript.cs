using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreScript : MonoBehaviour
{   
    [SerializeField]
    private Sprite[] numbers;

    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Image>().sprite = numbers[0];
    }
    
    public void UpdateScore()
    {
        this.score++;
        this.GetComponent<Image>().sprite = numbers[this.score];
    }
}
