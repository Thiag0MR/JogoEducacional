using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectorScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject score;

    public void ChangeScore()
    {
        this.score.GetComponent<ScoreScript>().UpdateScore();
    }
}
