using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class FishScript : MonoBehaviour
{
    public static event Action<GameObject> OnFishCollide;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "FishCollider")
        {
            // Notifica os interessados quando o peixe colidir com o collider 
            OnFishCollide?.Invoke(gameObject);
        }
    }
}
