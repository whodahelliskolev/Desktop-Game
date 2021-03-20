using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidCompanion : MonoBehaviour
{
    public AudioClip[] VoiceLines;
    private AudioSource companionAudiosource;
    public float greetDelay;
    public float goodbyeDelay;

    private void Awake()
    {
        companionAudiosource = GetComponent<AudioSource>();
    }
    public IEnumerator Greet()
    {
        yield return new WaitForSeconds(greetDelay);
        companionAudiosource.clip = VoiceLines[0];
        companionAudiosource.Play();
    }

    public void PointOfInterestVoiceLine(int Voiceline)
    {
        companionAudiosource.clip = VoiceLines[Voiceline];
        companionAudiosource.Play();
    }
    public IEnumerator Goodbye()
    {
        yield return new WaitForSeconds(goodbyeDelay);
        companionAudiosource.clip = VoiceLines[1];
        companionAudiosource.Play();
    }








    public void Start()
    {
        StartCoroutine(Greet());
    }


}
