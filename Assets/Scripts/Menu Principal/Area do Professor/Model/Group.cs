using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Group : IEquatable<Group>
{
    public string name;

    public Group(string name)
    {
        this.name = name;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Group);
    }

    public bool Equals(Group other)
    {
        return other is not null &&
               name == other.name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(name);
    }

    public static bool operator ==(Group left, Group right)
    {
        return EqualityComparer<Group>.Default.Equals(left, right);
    }

    public static bool operator !=(Group left, Group right)
    {
        return !(left == right);
    }
}
