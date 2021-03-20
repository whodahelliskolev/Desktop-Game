using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreGravityScript : MonoBehaviour
{

    private Rigidbody rb;
    private ConstantForce constantForceRef;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.useGravity = false;
        StartCoroutine(RestoreGravity());
    }

    public IEnumerator RestoreGravity()
    {
        yield return new WaitForSeconds(PlayerUpgrades.gravityAbilityDuration);
        rb.useGravity = true;
        constantForceRef = GetComponent<ConstantForce>();
        Destroy(constantForceRef);
        Destroy(this);
    }
  
}
