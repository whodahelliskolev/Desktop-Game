using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class Richochet_bullet_script : MonoBehaviour
{

    /// <summary>
    /// Layer ints
    /// Concrete - 13
    /// Metal - 11
    /// Wood - 12
    /// </summary>


    public float Damage;
    public int BounceLimit;
    private int Bounces;
    
    public void OnCollisionEnter(Collision collision)
    {
        Bounces++;

        Vector3 contactPosition = collision.GetContact(0).point;
       
        if (collision.collider == null)
        {

            return;
        }
      
        else if (collision.collider.CompareTag("Enemy"))
        {
            if (!collision.gameObject.GetComponent<EnemyLimbDamage>())
            
                return;         
            collision.gameObject.GetComponent<EnemyLimbDamage>().TakeDamage(Damage); // was 0.5
            GameObject BloodImpact =
            EasyObjectPool.instance.GetObjectFromPool("BloodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
            
            GameObject BodyHitAudio =
            EasyObjectPool.instance.GetObjectFromPool("BodyHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
            Destroy(gameObject);


        }
        /*
        else if (collision.collider.CompareTag("Player"))
        {

            if (!collision.gameObject.GetComponent<PlayerTakeDamage>())

                return;


            collision.gameObject.GetComponent<PlayerTakeDamage>().DamagePlayer();

            GameObject BloodImpact =
            EasyObjectPool.instance.GetObjectFromPool("BloodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

            GameObject BodyHitAudio1 =
            EasyObjectPool.instance.GetObjectFromPool("BodyHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

            Destroy(gameObject);


        }
        */
        else
        {
            /// No layer assigner, spawning default particle;
            

        }

        switch (collision.gameObject.layer)

        {
            case (13): //Concrete
                GameObject ConcreteImpact =
                EasyObjectPool.instance.GetObjectFromPool("ConcreteImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
                
                GameObject ConcreteHitSound =
                EasyObjectPool.instance.GetObjectFromPool("ConcreteHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                GameObject ConcreteHole =
               EasyObjectPool.instance.GetObjectFromPool("ConcreteHole", contactPosition, Quaternion.LookRotation(-collision.GetContact(0).normal));
                break;

            case (11): //Metal
                GameObject MetalImpact =
                EasyObjectPool.instance.GetObjectFromPool("MetalImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal)); ;

                GameObject MetalImpactSound =
                EasyObjectPool.instance.GetObjectFromPool("MetalHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));


                if (collision.gameObject.CompareTag("Radio"))
                    collision.gameObject.GetComponent<AudioSource>().Stop();
                break;
           
            case (18): //Metal , EnemyWeapon
                GameObject MetalImpact2 =
                EasyObjectPool.instance.GetObjectFromPool("MetalImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                GameObject MetalImpactSound2 =
                EasyObjectPool.instance.GetObjectFromPool("MetalHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                break;

            case (12): //Wood
                GameObject WoodImpact =
                EasyObjectPool.instance.GetObjectFromPool("WoodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                break;

            case (14):  //Glass   
                GameObject WoodImpact2 =
                EasyObjectPool.instance.GetObjectFromPool("WoodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
                Vector3 Direction = collision.transform.position - transform.position;
                ShatterableGlassInfo GlassInfo = new ShatterableGlassInfo(collision.GetContact(0).point, Direction);
                GlassInfo.HitPoint = collision.GetContact(0).point;
                if (collision.gameObject.GetComponent<ShatterableGlass>() != null)
                {
                    collision.gameObject.GetComponent<ShatterableGlass>().Shatter3D(GlassInfo);
                }           
                break;
        }

    }
        

    

    public void Update()
    {
        if (Bounces >= BounceLimit)
        {
            Destroy(gameObject);
        }
    }
}
