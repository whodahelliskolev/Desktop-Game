using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayerAugmentator : MonoBehaviour
{

    [Header("REFERENCES")]
    public GameObject UpgradeMenu;
    public GameObject Tooltips;
    public PlayerUpgrades playerUpgrades;
    public CustomCharacterController customCC;
    public GameObject hologram;

    public float lerpSpeed;
    public Animator animator;
    public CinemachineBrain cineBrain;
    public CinemachineFreeLook playerCam;
    public CinemachineFreeLook augmentatorCam;

    [Header("POSITION PLAYER")]
    public float lerpPosSpeed;
    public float turningSpeed;
    public GameObject player;
    public GameObject lerpPosIn;
    public GameObject lerpPosOut;
    private Coroutine playerPosCoroutine;
    

    public Text gravityAbilityText; 
    public Text gravityAbilityDurationText; 
    public Text focusAbilityText; 
    public Text abilityRechargeText; 
    public Text noMovementPenaltyText;
    public Text healthOnHeadshotText;
    public Text healthRegenText;

    public int focusUnlockCOST;
    public int gravityUnlockCOST;
    public int gravityDurationImproveCOST;
    public int abilityRechargeRateImproveCOST;
    public int noMovementPenaltyCOST;
    public int healthOnHeadshotCOST;
    public int healthRegenCOST;

    public int MenuQuitDelay = 1;
    public bool menuOpened = false;
    public bool inputBlocked = false;

    public AudioClip[] Sounds;

    private AudioSource source;


    public void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void Start()
    {

        if (ES3.Load<bool>("gravityAbility") == true)
        {
            gravityAbilityText.text = "//UPGRADE EQUIPPED";
        }

        if (ES3.Load<float>("abilityRechargeRate") != 5)
        {
            abilityRechargeText.text = "//UPGRADE EQUIPPED";
        }


        if (ES3.Load<bool>("focusUnlocked") == true)
        {
            focusAbilityText.text = "//UPGRADE EQUIPPED";
        }

        if (ES3.Load<float>("gravityAbilityDuration") != 3)
        {
            gravityAbilityDurationText.text = "//UPGRADE EQUIPPED";
        }

       
        if (ES3.Load<bool>("noMovementCost") == true)
        {
            noMovementPenaltyText.text = "//UPGRADE EQUIPPED";
        }

        if (ES3.Load<bool>("healthRegen") == true)
        {
            healthRegenText.text = "//UPGRADE EQUIPPED";
        }

        if (ES3.Load<bool>("healthOnHeadshot") == true)
        {
            healthOnHeadshotText.text = "//UPGRADE EQUIPPED";
        }

    }



    public IEnumerator PositionPlayer(GameObject obj)
    {

        float elapsedTime = 0;
        float waitTime = lerpSpeed;
        while (elapsedTime < waitTime)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(obj.transform.position.x, player.transform.position.y, obj.transform.position.z), (elapsedTime / waitTime));
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, obj.transform.rotation, turningSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime * lerpPosSpeed;
            

            yield return null;
        }
    }

    public IEnumerator BlockInput()
    {
        inputBlocked = true;
        yield return new WaitForSeconds(MenuQuitDelay);
        inputBlocked = false;
    }


    public void HealthOnHeadshotUpgrade()
    {
        if (!ES3.Load<bool>("healthOnHeadshot") && PlayerUpgrades.currency >= healthOnHeadshotCOST)
        {
            PlayerUpgrades.currency -= healthOnHeadshotCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("healthOnHeadshot", true);
            healthOnHeadshotText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }

    }
    public void HealthRegenUpgrade()
    {
        if (!ES3.Load<bool>("healthRegen") && PlayerUpgrades.currency >= healthRegenCOST)
        {
            PlayerUpgrades.currency -= healthRegenCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("healthRegen", true);
            healthRegenText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }

    }

    public void NoMovementCostUpgrade()
    {
        if (!ES3.Load<bool>("noMovementCost") && PlayerUpgrades.currency >= noMovementPenaltyCOST)
        {
            PlayerUpgrades.currency -= noMovementPenaltyCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("noMovementCost", true);
            noMovementPenaltyText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }

    }

    public void FocusAbilityUpgrade()
    {
        if (!ES3.Load<bool>("focusUnlocked") && PlayerUpgrades.currency >= focusUnlockCOST)
        {
            PlayerUpgrades.currency -= focusUnlockCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("focusUnlocked", true);
            focusAbilityText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }

    }

    public void GravityAbilityDurationImprove()
    {
        if (ES3.Load<float>("gravityAbilityDuration") == 3 && PlayerUpgrades.currency >= gravityDurationImproveCOST)
        {
            PlayerUpgrades.currency -= gravityDurationImproveCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<float>("gravityAbilityDuration", 5);
            gravityAbilityDurationText.text = "UPGRADE UNLOCKED";
            source.clip = Sounds[2];
            source.Play();
        }
    }

    public void GravityAbilityUpgrade()
    {
        if (!ES3.Load<bool>("gravityAbility") && PlayerUpgrades.currency >= gravityUnlockCOST)
        {
            PlayerUpgrades.currency -= gravityUnlockCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("gravityAbility", true);
            gravityAbilityText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }
    }

    public void abilityRechargeRateImprove()
    {
        if (ES3.Load<float>("abilityRechargeRate") == 5 && PlayerUpgrades.currency >= abilityRechargeRateImproveCOST)
        {
            PlayerUpgrades.currency -= abilityRechargeRateImproveCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<float>("abilityRechargeRate", 4);
            abilityRechargeText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }
    }

    public void UnlockUpgrade(int upgradeNumber)
    {
        switch (upgradeNumber)
        {
            case (0):
                FocusAbilityUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (1):
                GravityAbilityDurationImprove();
                playerUpgrades.LoadUpgrades();
                break;
            case (2):
                GravityAbilityUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (3):
                abilityRechargeRateImprove();
                playerUpgrades.LoadUpgrades();
                break;
            case (4):
                NoMovementCostUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (5):
                HealthRegenUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (6):
                HealthOnHeadshotUpgrade();
                playerUpgrades.LoadUpgrades();
                break;

        }

    }

    public void FocusOnMenu()
    {
        StartCoroutine(BlockInput());
        playerPosCoroutine = StartCoroutine(PositionPlayer(lerpPosIn));
        menuOpened = true;      
        UpgradeMenu.SetActive(true);
        animator.SetBool("isSprinting", false);
        animator.SetBool("isJogging", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isCrouching", false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        augmentatorCam.m_Priority = 10;
        playerCam.m_Priority = 5;
        customCC.enabled = false;
        source.clip = Sounds[0];
        source.Play();
        hologram.SetActive(false);
    }


    
    public void LeaveMenu()
    {
        StartCoroutine(PositionPlayer(lerpPosOut));
        UpgradeMenu.SetActive(false);
        menuOpened = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        augmentatorCam.m_Priority = 5;
        playerCam.m_Priority = 10;
        source.clip = Sounds[1];
        source.Play();
        customCC.enabled = true;
        Tooltips.SetActive(false);
        hologram.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !inputBlocked && menuOpened)
        {
            
            LeaveMenu();
            StopCoroutine(playerPosCoroutine);
        }

    }

}

