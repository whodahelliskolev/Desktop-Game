using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractionSystem : MonoBehaviour
{


    public WeaponUpgradeMenu weaponMenu;
    public PlayerAugmentator playerAugmentator;
    public ApartmentShutters shutters;
    public TargetMonitor targetMonitor;


    
    
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("WeaponRack"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = true;

            if (Input.GetKeyDown(KeyCode.F) && !weaponMenu.menuOpened )
            {
                weaponMenu.FocusOnMenu();
            }
        }
        else if (other.gameObject.CompareTag("PlayerAugmentator"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = true;

            if (Input.GetKeyDown(KeyCode.F) && !playerAugmentator.menuOpened)
            {
                playerAugmentator.FocusOnMenu();
                
            }
        }
        else if (other.gameObject.CompareTag("TargetMonitor"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = true;

            if (Input.GetKeyDown(KeyCode.F) && !targetMonitor.menuOpened)
            {
                targetMonitor.FocusOnMenu();
                

            }
        }

        else if (other.gameObject.CompareTag("Shutters"))
        {

            

                other.GetComponentInChildren<TextMeshPro>().enabled = true;

                if (Input.GetKeyDown(KeyCode.F))
                {
                    shutters.Shutters = !shutters.Shutters;

                }
            
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WeaponRack"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = false;
           
        }
        if (other.gameObject.CompareTag("PlayerAugmentator"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = false;

        }
       
        else if (other.gameObject.CompareTag("Shutters"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = false;

        }
        else if (other.gameObject.CompareTag("TargetMonitor"))
        {

            other.GetComponentInChildren<TextMeshPro>().enabled = false;

        }

    }

}
