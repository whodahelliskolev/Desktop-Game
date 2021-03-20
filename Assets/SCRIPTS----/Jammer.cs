using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jammer : MonoBehaviour
{
    // Start is called before the first frame update

    public bool JammerActive;
    

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && JammerActive)
        {
            WeaponHeatSystem.inJammerRange = true;
            
        }

        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")|| !JammerActive)
        {
            WeaponHeatSystem.inJammerRange = false;
        }
    }

   void OnDestroy()
    {
        WeaponHeatSystem.inJammerRange = false;
    }


}
