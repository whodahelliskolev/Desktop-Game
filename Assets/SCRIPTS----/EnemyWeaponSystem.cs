using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyWeaponSystem : MonoBehaviour
{

    [Header("References")]
    public Rigidbody HandRb;
    public GameObject Gun;
    public EnemyAI EnemyAI_Ref;
    public GameObject BulletOrigin;
    public EnemyHealth EnemyHealthRef;
    public AudioSource BulletShootSource;
    public WFX_LightFlicker LightFlicker;
    public Animator enemyAnimator;
   
    [Header("Shooting Parameters")]

    public float[] pistolFireRates;
    public float arFireRate;
   
    public float RecoilAmount;
    public float BulletDistance;
    public float Aim_Speed;
    
    public enum WeaponType { Pistol, Shotgun, AssaultRifle, Other };
    public WeaponType Weapon;


    private float FireTimer;
    private bool GunDropped = false;
    public bool PlayerSeen = true;
    private bool CanFire;
    private float pistolFireRate;
   
    public GameObject NormalBulletPrefabFX;
    public GameObject NormalBulletPrefab;


   

    public   void DropGunOnDeath()
    {
        if (!GunDropped)
        {
            GunDropped = true;
            Gun.GetComponent<Rigidbody>().isKinematic = false;
            Gun.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            Gun.GetComponent<BoxCollider>().isTrigger = false;
            Gun.transform.parent = null;
            
        }
    }

    public void CanFireWeapon(bool canFire)
    {
        CanFire = canFire;
    }
   
    public void Fire()
    {


        switch (Weapon)
        {
            case (WeaponType.Pistol):
                PistolFireParameters();
                enemyAnimator.SetInteger("WeaponType", 0);
                
                break;
            case (WeaponType.AssaultRifle):
                AssaultRifleParameters();
                enemyAnimator.SetInteger("WeaponType", 1);
                break;

        }


            
       
       
            
    }
  
    public void PistolFireParameters()
    {
        FireTimer += Time.deltaTime;
        if (FireTimer > pistolFireRate && CanFire)
            NormalBullet();
    }

    public void AssaultRifleParameters()
    {
        FireTimer += Time.deltaTime;
        if (FireTimer > arFireRate && CanFire)
            arBullet();
    }


    void arBullet()
    {
        LightFlicker.StartFlicker();      
        FireTimer = 0;
        BulletShootSource.PlayOneShot(BulletShootSource.clip);
        NormalBulletPrefabFX.GetComponent<ParticleSystem>().Play();
        var bullet = Instantiate(NormalBulletPrefab, BulletOrigin.transform.position, BulletOrigin.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = BulletOrigin.transform.forward * BulletDistance;
        HandRb.AddForce(Vector3.up * RecoilAmount, ForceMode.Impulse);
    }

    void NormalBullet()
    {
        LightFlicker.StartFlicker();
        pistolFireRate = pistolFireRates[Random.Range(0, pistolFireRates.Length - 1)];   // Pick a random fire rate from the database;  
        FireTimer = 0;
        BulletShootSource.PlayOneShot(BulletShootSource.clip);
        NormalBulletPrefabFX.GetComponent<ParticleSystem>().Play();
        var bullet = Instantiate(NormalBulletPrefab, BulletOrigin.transform.position, BulletOrigin.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = BulletOrigin.transform.forward * BulletDistance;
        HandRb.AddForce(Vector3.up * RecoilAmount, ForceMode.Impulse);
    }

    void Update()
    {
        if (!EnemyHealthRef.isDead && !EnemyHealthRef.lostBalance)
        {
            Fire();
        }
        
         
    }
}
