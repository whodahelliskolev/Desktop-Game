using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class ParticleOnEnableCustom : MonoBehaviour
{
    private ParticleSystem Particle;
   
    public float BackToPoolDelay;
  
    private void Awake()
    {
       
        Particle = this.GetComponent<ParticleSystem>();

    }

    private void OnEnable()
    {
       
        Particle.Play();
    
        StartCoroutine(delayBeforeReturnToPool());
    }
  

    public IEnumerator delayBeforeReturnToPool()
    {
        yield return new WaitForSeconds(BackToPoolDelay);
        EasyObjectPool.instance.ReturnObjectToPool(this.gameObject);
    }


   
}
