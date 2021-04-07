using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnSound : MonoBehaviour
{
    AudioSource clip;
    void Awake()
    {
        clip = GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        clip.Play();
    }
}
