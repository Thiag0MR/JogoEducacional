using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Entry : IEquatable<Entry>
{
    public string name;
    public string audioName;
    public Entry (string name, string audioName)
    {
        this.name = name;
        this.audioName = audioName;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Entry);
    }

    public bool Equals(Entry other)
    {
        return other is not null &&
               name == other.name &&
               audioName == other.audioName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name, audioName);
    }
}
