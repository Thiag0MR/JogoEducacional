using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PalavraEntry : Entry
{
    public string imageName;
    public string groupName;
    public PalavraEntry(string name, string audioName, string imageName, string groupName) : base(name, audioName)
    {
        this.imageName = imageName;
        this.groupName = groupName;
    }
}
