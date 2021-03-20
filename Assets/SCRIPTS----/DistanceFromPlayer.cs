using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DistanceFromPlayer : MonoBehaviour
{
    public GameObject Player;
    public TextMeshPro Text;
    // Update is called once per frame
   
    public void DistanceFromPlayerText()
    {
        
        
        if(ChangeAmmo.CURRENT_BULLET_TYPE == 1)
        {
            Text.enabled = true;
            gameObject.transform.LookAt(2 * transform.position - Player.transform.position);
            float Distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
            Text.text = Mathf.Round(Distance).ToString();
        }
        else if(ChangeAmmo.CURRENT_BULLET_TYPE != 1)
        {
            Text.enabled = false;
        }
        
    }
    
    
    void Update()
    {
        if(PlayerUpgrades.controllableDistanceUnlocked)
        DistanceFromPlayerText();
    }
}
