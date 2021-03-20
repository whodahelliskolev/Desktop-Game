using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApartmentShutters : MonoBehaviour
{
    // Start is called before the first frame update

    public Material shuttersMat;
    public bool Shutters;
    public float Speed;
    public float shuttersON;
    public float shuttersOFF;
   

    // Update is called once per frame
    void Update()
    {
        if (Shutters)
        {
            shuttersMat.SetFloat("_Smoothness", Mathf.Lerp(shuttersMat.GetFloat("_Smoothness"), shuttersON, Time.deltaTime * Speed));
           
        }
        else if (!Shutters)
        {
            shuttersMat.SetFloat("_Smoothness", Mathf.Lerp(shuttersMat.GetFloat("_Smoothness"), shuttersOFF, Time.deltaTime * Speed));
            
        }
    }
}
