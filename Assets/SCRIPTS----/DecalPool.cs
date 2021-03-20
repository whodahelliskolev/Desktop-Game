using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class DecalPool : MonoBehaviour
{



    public float BackToPoolDelay;
   


    

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
