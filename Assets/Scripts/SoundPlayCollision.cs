using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayCollision : MonoBehaviour
{
    public AudioSource sourceson;
    public AudioClip clip1, clip2;

    // Start is called before the first frame update
    void Start()
    {
        sourceson = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter (Collision collision)
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
