using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;


public class PlayerFlashlight : MonoBehaviour
{

    public AimIK aimIK;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(aimIK.solver.target);
    }
}
