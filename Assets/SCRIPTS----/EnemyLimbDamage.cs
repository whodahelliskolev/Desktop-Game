using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using SkinnedDecals;
using MarchingBytes;

public class EnemyLimbDamage : MonoBehaviour
{
    public PuppetMaster PM_Ref;
    public EnemyAI EnemyAI_Ref;
    public EnemyHealth EnemyHealthRef;
    public SkinnedDecalSystem SkinnedDecalSystem;
    public SkinnedDecal[] Decals;
    public bool instaKill;
    public GameObject bloodSpill;
    public int maxSpills;
    private int currentSpills;
 

    

    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)// bullet
        {
            currentSpills++;
            Vector3 contactPosition = collision.GetContact(0).point;
            SkinnedDecalSystem.CreateDecal(Decals[0], collision.transform.position, collision.GetContact(0).normal);
            if (currentSpills < maxSpills)
            {
                GameObject BloodSpillFX =
                EasyObjectPool.instance.GetObjectFromPool("BloodSpillFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
                BloodSpillFX.transform.parent = gameObject.transform;
                BloodSpillFX.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
       
    }


    public void TakeDamage(float DamageAmount)
    {
        
        
        EnemyHealthRef.Health -= DamageAmount;
       
        


        if (instaKill && !EnemyHealthRef.isDead)
        {
            EnemyHealthRef.KillEnemy();
            
            if (PlayerUpgrades.healthOnHeadshot)
            {
                PlayerHealth.Health += 10;
            }
            

        }



    }

}
