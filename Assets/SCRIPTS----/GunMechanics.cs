using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using Cinemachine;
using MarchingBytes;

public class GunMechanics : MonoBehaviour
{
    [Header("References")]
    public CinemachineFreeLook FreeLookRef;
    public CinemachineImpulseListener impulseListener;
    public AimIK Aim_IK_Reference;
    public LookAtIK Lookat_IK_Reference;   
    public ArmIK RightHandIK_REF;
    public Rigidbody HandRb;
    public Camera Camera;
    public GameObject BulletOrigin;
    public GameObject RicochetHitPoint;
    public LineRenderer RicochetTrajectory;
    public RicochetTrajectory RicochetScript;
    public Light Flashlight;
    public Animator GunAnimator;
    public PlayerHealth PlayerHealthRef;
    public UIController uiController;



    [Header("RECOIL PATTERNS")]
    public NoiseSettings NormalAmmoRecoil;
    public NoiseSettings RapidAmmoRecoil;
    public NoiseSettings ControllableAmmoRecoil;
    public NoiseSettings RicochetAmmoRecoil;

    [Header("Bullet Prefabs")]
    public GameObject ControllableBulletPrefab;
    public GameObject NormalBulletPrefab;
    public GameObject RicochetBulletPrefab;
    public GameObject RapidBulletPrefab;

    [Header("Bullet FXPrefabs")]
    public GameObject ControllableBulletPrefabFX;
    public GameObject NormalBulletPrefabFX;
    public GameObject RicochetBulletPrefabFX;
    public GameObject RapidBulletPrefabFX;

    [Header("MUZZLE LIGHTS")]
    public WFX_LightFlicker NormalBulletLIGHT;
    public WFX_LightFlicker ControllableBulletLIGHT;
    public WFX_LightFlicker RicochetBulletLIGHT;
    public WFX_LightFlicker RapidBulletLight;



    [Header("Bullet Type")]
    public int BulletType;  //0 - standart //1 - controllable // 2 - wallbang // 3 - ricochet
    public AudioSource BulletShootSource;
    public AudioClip[] BulletSounds;



    [Header("AbilityDrainByBulletType")]
    public float RichochetBulletHeatUp;
    public float ControllableBulletHeatUp;
    public float RapidBulletHeatUp;

    [Header("REAL TRAJECTORY")]
    public GameObject RealTrajectoryMarker;
    public float RealTrajectoryRayLength;
    public float DifferenceThreshold;

    [Header("Shooting Parameters")]
    
    public float FireRate;
    public float RapidFireRate;
    public float RecoilAmount;
    public float BulletDistance;
   
    public float Aim_Speed;
    public float FovSwitchSpeed; 
    public float AimFOV;
    public float NormalFOV;
   
    public float RAYDISTANCE;
    public LayerMask LayerMask;
   
    public GameObject ADS_TARGET;
    public GameObject FREEAIM_TARGET;


    
    [HideInInspector]
    public bool AcceptableShootAngle;
    [HideInInspector]
    public bool isAiming;
    [HideInInspector]
    public bool ADS_Blocked;

     
    [HideInInspector]
    public CinemachineImpulseSource RecoilSource;
   
    bool ReadyToShoot;
    float defaultFireRate;
    float FireTimer;
  
    RaycastHit realTrajHit;
    Vector3 CrosshairPos;

    CustomCharacterController customCC_script;
    Animator Player_Animator;


    private void Awake()
    {
        customCC_script = this.GetComponent<CustomCharacterController>();
        Player_Animator = this.GetComponent<Animator>();
        RecoilSource = this.GetComponent<CinemachineImpulseSource>();
        defaultFireRate = FireRate;
    }

    public void FiringRate()
    {
        FireTimer += Time.deltaTime;
        if (FireTimer > FireRate)
        {
            ReadyToShoot = true;
        }

    }
   
    void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Flashlight.enabled = !Flashlight.enabled;
        }
    }
    private void Shoot()
    {

        ///ADS


        if (!uiController.inMenu && AcceptableShootAngle && Input.GetKeyDown(KeyCode.Mouse0) && ReadyToShoot)
        {
            switch (BulletType)

            {
                case (0):
                    NormalBullet();
                    
                    break;
                case (1):
                    ControllableBullet();
                   
                    break;
                case (2):
                    RapidBullet();
                   
                    break;
                case (3):
                    RicochetBullet();
                    
                    break;

            }


        }

        else if (!uiController.inMenu && AcceptableShootAngle && Input.GetKey(KeyCode.Mouse0) && ReadyToShoot)
        {
            if (BulletType == 2)
            {
                RapidBullet();
            }
        }




    }
    public void RealTrajectory()
    {
        


            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, RealTrajectoryRayLength, LayerMask, QueryTriggerInteraction.Ignore))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(RealTrajectoryRayLength);

            Physics.Raycast(BulletOrigin.transform.position, (targetPoint - BulletOrigin.transform.position).normalized, out realTrajHit, RealTrajectoryRayLength, LayerMask, QueryTriggerInteraction.Ignore);

            
            
        


            if (realTrajHit.collider != null )
            {
                RealTrajectoryMarker.SetActive(true);
                RealTrajectoryMarker.transform.position = realTrajHit.point;
                RealTrajectoryMarker.transform.eulerAngles = realTrajHit.normal;
            }

           else if (realTrajHit.collider == null)
            {
                RealTrajectoryMarker.SetActive(false);
                
            }

        
    }
    public void RestoreAudioPitch()
    {    
            BulletShootSource.pitch = Mathf.Lerp(BulletShootSource.pitch, 1, Time.deltaTime * 2);
    }
    void RicochetBullet()
    {

        FireRate = defaultFireRate;
        BulletShootSource.pitch = 1;
        if (WeaponHeatSystem.Heat + RichochetBulletHeatUp > 100)
        {
            BulletShootSource.PlayOneShot(BulletSounds[3]);
        }
        else
        {

            ReadyToShoot = false;
            FireTimer = 0;
            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, RAYDISTANCE, LayerMask, QueryTriggerInteraction.Ignore))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(RAYDISTANCE);

            CrosshairPos = hit.point;
            RecoilSource.m_ImpulseDefinition.m_RawSignal = RicochetAmmoRecoil;
            RecoilSource.GenerateImpulse(Camera.main.transform.forward);
            RicochetBulletLIGHT.StartFlicker();
            BulletShootSource.PlayOneShot(BulletSounds[2]);
            WeaponHeatSystem.Heat += RichochetBulletHeatUp;
            RicochetBulletPrefabFX.GetComponent<ParticleSystem>().Play();
            var bullet = Instantiate(RicochetBulletPrefab, BulletOrigin.transform.position, BulletOrigin.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = (targetPoint - BulletOrigin.transform.position).normalized * BulletDistance;
            HandRb.AddForce(Vector3.up * RecoilAmount, ForceMode.Impulse);
        }

    }
    void RapidBullet()
    {

        FireRate = RapidFireRate;
        if (WeaponHeatSystem.Heat + RapidBulletHeatUp > 100)
        {
            


        }

        else
        {

            ReadyToShoot = false;
            FireTimer = 0;
            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit, RAYDISTANCE, LayerMask, QueryTriggerInteraction.Ignore))
            {
                targetPoint = hit.point;
                
            }
            else
                targetPoint = ray.GetPoint(RAYDISTANCE);
            
            CrosshairPos = hit.point;
            RecoilSource.m_ImpulseDefinition.m_RawSignal = RapidAmmoRecoil;
            RecoilSource.GenerateImpulse(Camera.main.transform.forward);
            BulletShootSource.pitch += RapidFireRate;
            BulletShootSource.PlayOneShot(BulletSounds[0]);
            NormalBulletLIGHT.StartFlicker();
            WeaponHeatSystem.Heat += RapidBulletHeatUp;
            NormalBulletPrefabFX.GetComponent<ParticleSystem>().Play();
            var bullet = Instantiate(RapidBulletPrefab, BulletOrigin.transform.position, BulletOrigin.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = (targetPoint - BulletOrigin.transform.position).normalized * BulletDistance;
            HandRb.AddForce(Vector3.up * RecoilAmount, ForceMode.Impulse);
        }

    }
    void NormalBullet()
    {

        FireRate = defaultFireRate;
        ReadyToShoot = false;
        FireTimer = 0;

        Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;



        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit, RAYDISTANCE, LayerMask, QueryTriggerInteraction.Ignore))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(RAYDISTANCE);


        CrosshairPos = hit.point;
        RecoilSource.m_ImpulseDefinition.m_RawSignal = NormalAmmoRecoil;
        RecoilSource.GenerateImpulse(Camera.main.transform.forward);
        NormalBulletLIGHT.StartFlicker();
        BulletShootSource.PlayOneShot(BulletSounds[0]);
        NormalBulletPrefabFX.GetComponent<ParticleSystem>().Play();

        var bullet = Instantiate(NormalBulletPrefab, BulletOrigin.transform.position, BulletOrigin.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = (targetPoint - BulletOrigin.transform.position).normalized * BulletDistance;
        HandRb.AddForce(Vector3.up * RecoilAmount, ForceMode.Impulse);
    }
    void ControllableBullet()
    {

        FireRate = defaultFireRate;
        BulletShootSource.pitch = 1;
        if (WeaponHeatSystem.Heat + ControllableBulletHeatUp > 100)
        {
            //not enough ability;
            BulletShootSource.PlayOneShot(BulletSounds[3]);
        }

        else
        {
            ReadyToShoot = false;
            FireTimer = 0;
            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit, RAYDISTANCE, LayerMask, QueryTriggerInteraction.Ignore))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(RAYDISTANCE);

            CrosshairPos = hit.point;
            RecoilSource.m_ImpulseDefinition.m_RawSignal = ControllableAmmoRecoil;
            RecoilSource.GenerateImpulse(Camera.main.transform.forward);
            ControllableBulletLIGHT.StartFlicker();

            WeaponHeatSystem.Heat += ControllableBulletHeatUp;
            BulletShootSource.PlayOneShot(BulletSounds[1]);
            ControllableBulletPrefabFX.GetComponent<ParticleSystem>().Play();
            var Controllable_bullet = Instantiate(ControllableBulletPrefab, BulletOrigin.transform.position, BulletOrigin.transform.rotation);
            HandRb.AddForce(Vector3.up * RecoilAmount, ForceMode.Impulse);
        }

    } 
   
    public void HideRealTrajectory()
    {
        RealTrajectoryMarker.SetActive(false);
    }
    public void ADS_Blockers()
    {
        if (customCC_script.movementBlocked || PlayerHealthRef.Injured  || !customCC_script.isGrounded)
            ADS_Blocked = true;      
      
        else if(!customCC_script.movementBlocked && !PlayerHealthRef.Injured && customCC_script.isGrounded)
            ADS_Blocked = false;
   

    }
  
    public void AimLogic()
    {
                
            if (Input.GetKey(KeyCode.Mouse1) && !ADS_Blocked)
            {
                
                RealTrajectory();
                Aim_IK_Reference.solver.IKPositionWeight = Mathf.Lerp(Aim_IK_Reference.solver.IKPositionWeight, 1, Time.deltaTime * Aim_Speed);              
                Lookat_IK_Reference.solver.bodyWeight = Mathf.Lerp(Lookat_IK_Reference.solver.bodyWeight, 1, Time.deltaTime * Aim_Speed);
                Lookat_IK_Reference.solver.target = ADS_TARGET.transform;
                FreeLookRef.m_Lens.FieldOfView = Mathf.Lerp(FreeLookRef.m_Lens.FieldOfView, AimFOV, Time.deltaTime * FovSwitchSpeed);
                RightHandIK_REF.solver.IKPositionWeight = Mathf.Lerp(RightHandIK_REF.solver.IKPositionWeight, 0.1f, Time.deltaTime * Aim_Speed);
                RightHandIK_REF.solver.IKRotationWeight = Mathf.Lerp(RightHandIK_REF.solver.IKRotationWeight, 1, Time.deltaTime * Aim_Speed);
                
                float AnimatorADSLerp = Player_Animator.GetLayerWeight(1);
                float AnimatorFreeAimLerp = Player_Animator.GetLayerWeight(2);
                
                impulseListener.m_Gain = 0.4f;
                Player_Animator.SetLayerWeight(1, Mathf.Lerp(AnimatorADSLerp, 1, Time.deltaTime * Aim_Speed));
                Player_Animator.SetLayerWeight(2, Mathf.Lerp(AnimatorFreeAimLerp, 0, Time.deltaTime * Aim_Speed));           
                Player_Animator.SetBool("isAiming", true);
                Player_Animator.SetBool("canResumeAiming", true);
                
                isAiming = true;
                customCC_script.CanStrafe = true;

                if (BulletType == 3 && RicochetScript.canShowTrajectory && PlayerUpgrades.ricochetTrajectoryUnlocked)
                {
                    ShowRicochetTrajectory();
                }
                else if (BulletType == 3 && !RicochetScript.canShowTrajectory)
                {
                    HideRicochetTrajectory();
                }

            }




            else if (Input.GetKey(KeyCode.Mouse1) && ADS_Blocked)
            {

                HideRealTrajectory();
               
                Aim_IK_Reference.solver.IKPositionWeight = Mathf.Lerp(Aim_IK_Reference.solver.IKPositionWeight, 0, Time.deltaTime * Aim_Speed);          
                FreeLookRef.m_Lens.FieldOfView = Mathf.Lerp(FreeLookRef.m_Lens.FieldOfView, NormalFOV, Time.deltaTime * FovSwitchSpeed);
                Lookat_IK_Reference.solver.target = FREEAIM_TARGET.transform;
                float AnimatorFreeAimLerp = Player_Animator.GetLayerWeight(2);
                float AnimatorADSLerp = Player_Animator.GetLayerWeight(1);
                impulseListener.m_Gain = 1f;
                Player_Animator.SetLayerWeight(1, Mathf.Lerp(AnimatorADSLerp, 0, Time.deltaTime * Aim_Speed));
                Player_Animator.SetLayerWeight(2, Mathf.Lerp(AnimatorFreeAimLerp, 0, Time.deltaTime * Aim_Speed));             
                Player_Animator.SetBool("isAiming", false);
                Player_Animator.SetBool("canResumeAiming", true);
              
                isAiming = false;
                customCC_script.CanStrafe = false;
                
                if (BulletType == 3)
                {
                    HideRicochetTrajectory();
                }
            }

          
      
            else
        {

            HideRealTrajectory();

            Aim_IK_Reference.solver.IKPositionWeight = Mathf.Lerp(Aim_IK_Reference.solver.IKPositionWeight, 0, Time.deltaTime * Aim_Speed);          
            FreeLookRef.m_Lens.FieldOfView = Mathf.Lerp(FreeLookRef.m_Lens.FieldOfView, NormalFOV, Time.deltaTime * FovSwitchSpeed);
            Lookat_IK_Reference.solver.target = FREEAIM_TARGET.transform;
            
            float AnimatorFreeAimLerp = Player_Animator.GetLayerWeight(2);
            float AnimatorADSLerp = Player_Animator.GetLayerWeight(1);
           
            impulseListener.m_Gain = 1f;
            Player_Animator.SetLayerWeight(1, Mathf.Lerp(AnimatorADSLerp, 0, Time.deltaTime * Aim_Speed));
         
            if (customCC_script.movementBlocked)
            {
                Player_Animator.SetLayerWeight(2, Mathf.Lerp(AnimatorFreeAimLerp, 0, Time.deltaTime * Aim_Speed));
            }
            else
            {
                Player_Animator.SetLayerWeight(2, Mathf.Lerp(AnimatorFreeAimLerp, 1, Time.deltaTime * Aim_Speed));
            }
            Player_Animator.SetBool("isAiming", false);
            Player_Animator.SetBool("canResumeAiming", false);
           
            isAiming = false;
            customCC_script.CanStrafe = false;
            if (BulletType == 3)
            {
                HideRicochetTrajectory();
            }
        }
      
    }
  
   public void ShowRicochetTrajectory()
    {
        RicochetHitPoint.SetActive(true);
        RicochetTrajectory.enabled = true;
    }
  
    public void HideRicochetTrajectory()
    {
        RicochetHitPoint.SetActive(false);
        RicochetTrajectory.enabled = false;
    }
   
    void Update()
    {
        ADS_Blockers();
        ToggleFlashlight();
        FiringRate();
        AimLogic();
        Shoot();
        RestoreAudioPitch();
        
    }

}
