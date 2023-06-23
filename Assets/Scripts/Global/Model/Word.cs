using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Word 
{
    public string name;
    public AudioClip audioClip;
    public Sprite image;

    public Word(string name, AudioClip audioClip, Sprite image)
    {
        this.name = name;
        this.audioClip = audioClip;
        this.image = image;
    }
}
