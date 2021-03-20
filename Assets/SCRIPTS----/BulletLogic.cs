using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using MarchingBytes;



public class BulletLogic : MonoBehaviour
{
    public float Damage;
    private TrailRenderer normalTrail;
    public TrailRenderer slowmoTrail;


    private void Awake()
    {
        normalTrail = GetComponent<TrailRenderer>();
    }

    public void Update()
    {
        if (FocusSystem.focusActive)
        {
            normalTrail.enabled = false;
            slowmoTrail.enabled = true;
        }

        else if (!FocusSystem.focusActive)
        {
            normalTrail.enabled = true;
            slowmoTrail.enabled = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {


        Vector3 contactPosition = collision.GetContact(0).point;
       
        if (collision.collider == null)
        {
            
            return;
        }
      
        else if (collision.collider.CompareTag("Enemy"))
        {

            if (!collision.gameObject.GetComponent<EnemyLimbDamage>())

                return;

            collision.gameObject.GetComponent<EnemyLimbDamage>().TakeDamage(Damage); //was 0.15f
            
            GameObject BloodImpact =
            EasyObjectPool.instance.GetObjectFromPool("BloodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

            GameObject BodyHitAudio =
            EasyObjectPool.instance.GetObjectFromPool("BodyHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

            Destroy(gameObject);


        }

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

        else
        {
            /// No layer assigner, spawning default particle;
            
            Destroy(gameObject);
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
                
                Destroy(gameObject);
                break;
           
            case (11): //Metal

                GameObject MetalImpact =
                EasyObjectPool.instance.GetObjectFromPool("MetalImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                GameObject MetalImpactSound =
                EasyObjectPool.instance.GetObjectFromPool("MetalHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                Destroy(gameObject);
                if (collision.gameObject.CompareTag("Radio"))
                    collision.gameObject.GetComponent<AudioSource>().Stop();
                break;




            case (22): //Jammer

                GameObject MetalImpact3 =
                EasyObjectPool.instance.GetObjectFromPool("MetalImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                GameObject MetalImpactSound3 =
                EasyObjectPool.instance.GetObjectFromPool("MetalHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                
                Destroy(collision.gameObject);
                Destroy(gameObject);
                
                break;

            case (18): //Metal


                GameObject MetalImpact2 =
                EasyObjectPool.instance.GetObjectFromPool("MetalImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
                
                GameObject MetalImpactSound2 =
                EasyObjectPool.instance.GetObjectFromPool("MetalHitSound", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));

                Destroy(gameObject);
                break;

            case (12): //Wood

                GameObject WoodImpact =
                EasyObjectPool.instance.GetObjectFromPool("WoodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
                Destroy(gameObject);
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
                Destroy(gameObject);
                break;
        }

       



    }


}
