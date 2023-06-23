using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Letter {
    public string name;
    public AudioClip audioClip;

    public Letter(string name, AudioClip audioClip)
    {
        this.name = name;
        this.audioClip = audioClip;
    }
}
