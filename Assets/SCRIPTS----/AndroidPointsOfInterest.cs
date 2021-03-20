using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidPointsOfInterest : MonoBehaviour
{

    public AndroidCompanion androidCompanion;
    public int voiceLine;
    public int chance; // higher the rarer


    private void OnTriggerEnter(Collider other)
    {
        int chanceofPlay = Random.Range(0, chance);
            if (other.gameObject.CompareTag("PlayerCollider")&& chanceofPlay == 1)
        {
            androidCompanion.PointOfInterestVoiceLine(voiceLine);
        }
    }

    
}
