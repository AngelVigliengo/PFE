using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundCollision : MonoBehaviour
{
    public AudioSource sourceson;
    public AudioClip clip1, clip2;
    
    void Start()
    {
        sourceson = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ZoneSon")
        {
            sourceson.clip = clip1;
            sourceson.Play();
        }
        if (collision.gameObject.tag == "ZoneSonPeur")
        {
            sourceson.clip = clip2;
            sourceson.Play();
        }
    }
}
