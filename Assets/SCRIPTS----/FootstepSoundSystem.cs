using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSoundSystem : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource FootstepAudioSource;
    private void Awake()
    {
        FootstepAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == null)
            return;
        else
        {
            FootstepAudioSource.Play();
            Debug.Log("Collision");
        }
    }
}
