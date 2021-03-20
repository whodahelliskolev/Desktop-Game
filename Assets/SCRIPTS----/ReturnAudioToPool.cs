using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class ReturnAudioToPool : MonoBehaviour
{
    public float BackToPoolDelay;
    private AudioSource AudioSource;


    private void Awake()
    {

        AudioSource = this.GetComponent<AudioSource>();

    }

    private void OnEnable()
    {      
        StartCoroutine(delayBeforeReturnToPool());
    }


    public IEnumerator delayBeforeReturnToPool()
    {
        yield return new WaitForSeconds(BackToPoolDelay);
        EasyObjectPool.instance.ReturnObjectToPool(this.gameObject);
    }
}
