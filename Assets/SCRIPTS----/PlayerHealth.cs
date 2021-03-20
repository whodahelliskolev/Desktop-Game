using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Audio;


public class PlayerHealth : MonoBehaviour
{
    [Header("Parameters")]
    [Space(5)]   
    public static float Health = 100;
    public float RegenRate;
    public static float Syringes = 0;
    public bool Injured;
    public float RestartDelay;   
    [Space(5)]
    
    [Header("IK REFERENCES")] 
    
    public LookAtIK LookAtIkREF;
    public AimIK AimIkRef;
    public ArmIK FreeAimArmIK;

    [Space(5)]
    [Header("OTHER")]
    [Space(5)]
    public Volume Volume;
    public GameObject Crosshair;
    public Slider HealthSlider;
    public Text SyringeCount;
    public LineRenderer BulletTrajectory;
    public PuppetMaster PM_REF;     
    public GameObject DeathTitleObject;
    
    
   
    private bool PlayerDead = false;
    private bool DeathCoroutineStarted = false;
    private Animator PlayerAnimator;
    private GunMechanics gunMechanics;
    private CustomCharacterController customCharController;
    


    public void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
        gunMechanics = GetComponent<GunMechanics>();
        customCharController = GetComponent<CustomCharacterController>();
        Health = 100;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * .01f;
        RestorePostProcess();
        
    }

    private void OnApplicationQuit()
    {
        RestorePostProcess();
    }

    public IEnumerator DeathCoroutine()
    {
        DeathCoroutineStarted = true;
        DeathTitleObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(RestartDelay);
        SceneManager.LoadScene("Dystopia Prototype Level");
    }
   
    public void HealthRegen()
    {
        if(PlayerUpgrades.healthRegen)
        Health += Time.deltaTime * RegenRate;
    }
    public void RagdollPlayer()
    {
        Crosshair.SetActive(false);
        PlayerDead = true;
        PM_REF.mappingWeight = 1;
        PM_REF.state = PuppetMaster.State.Dead;
        PM_REF.internalCollisions = true;           
        LookAtIkREF.enabled = false;
        customCharController.enabled = false;
        gunMechanics.enabled = false;
        BulletTrajectory.enabled = false;
        FreeAimArmIK.enabled = false;
        Time.timeScale = 0.5f;
        Time.fixedDeltaTime = Time.timeScale * .01f;
    }
    public void DisableIK()
    {
           
        LookAtIkREF.enabled = false;
        gunMechanics.enabled = false;
    }
    public void EnableIK()
    {
            
        LookAtIkREF.enabled = true;  
        gunMechanics.enabled = true;
    }
    public void PostProcess()
    {
        VolumeProfile profile = Volume.sharedProfile;
        if (!profile.TryGet<ColorAdjustments>(out var ColorAdj))
        {
            ColorAdj = profile.Add<ColorAdjustments>(false);
        }

        if (Health >= 100)
        {
            ColorAdj.saturation.value = 0;
        }
        else if (Health < 100)
        {
            ColorAdj.saturation.value = Health - 100;
        }

    }
    public void RestorePostProcess()
    {
        VolumeProfile profile = Volume.sharedProfile;
        if (!profile.TryGet<ColorAdjustments>(out var ColorAdj))
        {
            ColorAdj = profile.Add<ColorAdjustments>(false);
        }

        ColorAdj.saturation.value = 0;
    }
    public void ValuesToUI()
    {
        HealthSlider.value = Health / 100;
        SyringeCount.text = Syringes.ToString();
    }
    public void PlayerState()
    {
        if (Health <= 0 && !PlayerDead)
        {
            RagdollPlayer();
            if (!DeathCoroutineStarted)
                StartCoroutine(DeathCoroutine());

        }


        if (Health <= 25)
        {
            Injured = true;
            PlayerAnimator.SetBool("Injured", true);
        }

        if (Health > 25)
        {
            Injured = false;
            PlayerAnimator.SetBool("Injured", false);
        }
    }
    public void SyringeSystem()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && Syringes > 0) ///Syringe
        {

            if (Health < 100)
            {
                Syringes--;
                Health += 50;
            }

            if (Health > 100)
            {
                Health = 100;
            }

        }
    }
   
    void Update()
    {
        PostProcess();
        HealthRegen();
        ValuesToUI();
        PlayerState();
        SyringeSystem();
        

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Dystopia Prototype Level");
        }
    
        
    }
    }

