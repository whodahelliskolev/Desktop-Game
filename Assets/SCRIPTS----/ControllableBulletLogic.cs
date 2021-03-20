using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using TMPro;
using MarchingBytes;

public class ControllableBulletLogic : MonoBehaviour
{

    [Header("Parameters")]

    public float Damage;
    public float bulletSpeed;
    public float lockedOnSpeed;
    public float trajectoryLength;
    public TextMeshPro Text;
    public LayerMask LayersToDetect;
   
    private Vector3 startingPos;
    private GameObject CAM;
    private bool enemyLimbInRange;
    private Vector3 LimbDirection;

    public void Awake()
    {
      CAM = GameObject.FindGameObjectWithTag("MainCamera");
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && PlayerUpgrades.controllableAutoLockOnUnlocked)
        {
            enemyLimbInRange = true;
            LimbDirection = (other.transform.position - gameObject.transform.position).normalized;
        }
    }
    public void Start()
    {
        startingPos = gameObject.transform.position;
    }
    public void FixedUpdate()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, trajectoryLength, LayersToDetect, QueryTriggerInteraction.Ignore))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(trajectoryLength);

        if (enemyLimbInRange)
            this.GetComponent<Rigidbody>().velocity = LimbDirection * lockedOnSpeed;
        else
        this.GetComponent<Rigidbody>().velocity = (targetPoint - startingPos).normalized * bulletSpeed ;
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

            collision.gameObject.GetComponent<EnemyLimbDamage>().TakeDamage(Damage); 
            GameObject BloodImpact =
            EasyObjectPool.instance.GetObjectFromPool("BloodImpactFX", contactPosition, Quaternion.LookRotation(collision.GetContact(0).normal));
            
            GameObject BodyHitAudio =
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
    public void BulletDistanceFromPlayerText()
    {


        if (ChangeAmmo.CURRENT_BULLET_TYPE == 1)
        {
            Text.enabled = true;
            gameObject.transform.LookAt(2 * transform.position - CAM.transform.position);
            float Distance = Vector3.Distance(gameObject.transform.position, CAM.transform.position);
            Text.text = Mathf.Round(Distance).ToString();
        }
        else if (ChangeAmmo.CURRENT_BULLET_TYPE != 1)
        {
            Text.enabled = false;
        }

    }
    public void Update()
    {
       if(PlayerUpgrades.controllableDistanceUnlocked)
        BulletDistanceFromPlayerText();
    }
}
