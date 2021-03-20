using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactAudio : MonoBehaviour
{
    
    public AudioClip[] clips;
    public AudioSource AudioSource;
    

    public void OnEnable()
    {
        AudioSource.PlayOneShot(clips[Random.Range(0, clips.Length - 1)]);
    }
   
}
