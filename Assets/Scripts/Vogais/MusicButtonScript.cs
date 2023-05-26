using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicButtonScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Sprite musicOn;

    [SerializeField]
    private Sprite musicOff;

    private Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayStopMusic() {
        if (audioSource.isPlaying) {
            audioSource.Pause();
            image.sprite = musicOff;
        } else {
            audioSource.Play();
            image.sprite = musicOn;
        }
    }
}
