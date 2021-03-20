using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkinnedDecals;
using TMPro;

public class PlayerTakeDamage : MonoBehaviour
{
    
    public float DamageAmount;
    public SkinnedDecalSystem DecalSystemBody;
    public SkinnedDecalSystem DecalSystemShirt;
    public SkinnedDecalSystem DecalSystemPants;
    public SkinnedDecal[] Decals;
    public string BodyPart;
    public TextMeshProUGUI Text;
    public int decalLimit;
    private int currentDecals;
    public void DamagePlayer()
    {
        PlayerHealth.Health -= DamageAmount;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)// bullet
        {
            currentDecals++;

            if (PlayerHealth.Health > 0  )
            {
                Text.text = "DEAD" + Environment.NewLine + "FATAL WOUND TO THE " + Environment.NewLine + "//" + BodyPart;
               
                if(currentDecals < decalLimit)
                {
                    DecalSystemBody.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
                    DecalSystemShirt.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
                    DecalSystemPants.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
                }
                
                
            }
            
            

            if (PlayerHealth.Health <= 0 && currentDecals < decalLimit)
            {
                DecalSystemBody.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
                DecalSystemShirt.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
                DecalSystemPants.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
            }
        }
        
    }


}
