using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDeactivator : MonoBehaviour
{

    public int SecondsUntilDeactivation;   
    public IEnumerator DeactivateParticle()
    {
        yield return new WaitForSeconds(SecondsUntilDeactivation);
        Destroy(gameObject);
    }
    public void Start()
    {
        StartCoroutine(DeactivateParticle());
    }

}
