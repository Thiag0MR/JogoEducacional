using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Palavras
{
    public class MusicButtonScript : MonoBehaviour
    {
        [SerializeField]
        private AudioSource audioSource;

        
        public void PlayStopMusic()
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
                gameObject.transform.GetChild(0).transform.gameObject.SetActive(false);
                gameObject.transform.GetChild(1).transform.gameObject.SetActive(true);
            }
            else
            {
                audioSource.Play();
                gameObject.transform.GetChild(0).transform.gameObject.SetActive(true);
                gameObject.transform.GetChild(1).transform.gameObject.SetActive(false);
            }
        }
    }
}