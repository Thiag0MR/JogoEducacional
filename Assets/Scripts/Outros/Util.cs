using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    public static void Shuffle(string[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            string temp = array[i];
            int randomIndex = UnityEngine.Random.Range(0, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
