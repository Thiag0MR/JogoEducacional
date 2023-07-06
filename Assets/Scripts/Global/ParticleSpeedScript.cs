using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeedScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 2;
    ParticleSystem ps;
    void Start() {
        ps = GetComponent<ParticleSystem>();
        ps.playbackSpeed = speed;
    }
}