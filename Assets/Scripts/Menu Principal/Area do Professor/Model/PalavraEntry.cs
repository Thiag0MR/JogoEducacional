using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PalavraEntry : Entry, IEquatable<PalavraEntry>
{
    public string imageName;
    public string groupName;
    public PalavraEntry(string name, string audioName, string imageName, string groupName) : base(name, audioName)
    {
        this.imageName = imageName;
        this.groupName = groupName;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as PalavraEntry);
    }

    public bool Equals(PalavraEntry other)
    {
        return other is not null &&
               base.Equals(other) &&
               groupName == other.groupName;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), groupName);
    }

    public static bool operator ==(PalavraEntry left, PalavraEntry right)
    {
        return EqualityComparer<PalavraEntry>.Default.Equals(left, right);
    }

    public static bool operator !=(PalavraEntry left, PalavraEntry right)
    {
        return !(left == right);
    }
}
