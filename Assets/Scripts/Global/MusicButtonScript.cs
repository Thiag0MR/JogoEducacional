using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    
    public void PlayStopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            audioSource.Play();
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            gameObject.transform.GetChild(1).gameObject.SetActive(false);
        }
        Debug.Log("chamou");
    }
}