using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class WeaponUpgradeMenu : MonoBehaviour
{
    [Header("POSITION PLAYER")]
    public GameObject UpgradeMenu;
    public PlayerUpgrades playerUpgrades;
    public CustomCharacterController customCC;


    [Header("POSITION PLAYER")]
    public float lerpSpeed;
    public float turningSpeed;
    public GameObject player;
    public GameObject lerpPos;
    private Coroutine playerPosCoroutine;
    public Animator animator;
   
    public CinemachineBrain cineBrain;
    public CinemachineFreeLook playerCam;
    public CinemachineFreeLook gunCam;

    public Text controllableLockOnText; //0
    public Text controllableDistanceText; //1
    public Text ricochetBouncesText; //2
    public Text ricochetTrajectoryText;//3
    public Text weaponCooldownText;//4


    public int controllableLockOnCOST;
    public int controllableDistanceCOST;
    public int ricochetBouncesCOST;
    public int ricochetTrajectoryCOST;
    public int weaponCooldownCost;
    
    public int MenuQuitDelay = 1;
    public bool menuOpened = false;
    public bool inputBlocked = false;
    public GameObject Tooltips;

    public AudioClip[] Sounds;

    private AudioSource source;
    

    public void Awake()
    {
        source = GetComponent<AudioSource>();

    }

  


    public IEnumerator PositionPlayer()
    {

        float elapsedTime = 0;
        float waitTime = 3;
        while (elapsedTime < waitTime)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(lerpPos.transform.position.x, player.transform.position.y, lerpPos.transform.position.z), (elapsedTime / waitTime));
            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, lerpPos.transform.rotation, turningSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime * lerpSpeed;
            

            yield return null;
        }
    }

    public void Start()
    {
        
        if (ES3.Load<bool>("controllableAutoLockOn") == true)
        {
            controllableLockOnText.text = "//UPGRADE EQUIPPED";
        }

        if (ES3.Load<bool>("controllableDistance") == true)
        {
            controllableDistanceText.text = "//UPGRADE EQUIPPED";
        }


        if (ES3.Load<bool>("ricochetTrajectory") == true)
        {
            ricochetTrajectoryText.text = "//UPGRADE EQUIPPED";
        }

        if (ES3.Load<float>("weaponCooldownRate") != 2)
        {
            weaponCooldownText.text = "//UPGRADE EQUIPPED";
        }
       
    }

    
    public IEnumerator BlockInput()
    {
        inputBlocked = true;
        yield return new WaitForSeconds(MenuQuitDelay);
        inputBlocked = false;
    }

    public void controllableLockOnUpgrade()
    {
        if(!ES3.Load<bool>("controllableAutoLockOn") && PlayerUpgrades.currency >= controllableLockOnCOST)
        {
            PlayerUpgrades.currency -= controllableLockOnCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("controllableAutoLockOn", true);
            controllableLockOnText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }
        
    }


    public void controllableDistanceUpgrade()
    {
        if (!ES3.Load<bool>("controllableDistance") && PlayerUpgrades.currency >= controllableDistanceCOST)
        {
            PlayerUpgrades.currency -= controllableDistanceCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("controllableDistance", true);
            controllableDistanceText.text = "UPGRADE UNLOCKED";
            source.clip = Sounds[2];
            source.Play();
        }
    }

    public void RicochetTrajectoryUpgrade()
    {
        if (!ES3.Load<bool>("ricochetTrajectory") && PlayerUpgrades.currency >= ricochetTrajectoryCOST)
        {
            PlayerUpgrades.currency -= ricochetTrajectoryCOST;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<bool>("ricochetTrajectory", true);
            ricochetTrajectoryText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }
    }

    public void WeaponCooldownUpgrade()
    {
        if (ES3.Load<float>("weaponCooldownRate") ==2 && PlayerUpgrades.currency >= weaponCooldownCost)
        {
            PlayerUpgrades.currency -= weaponCooldownCost;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            ES3.Save<float>("weaponCooldownRate", 4);
            weaponCooldownText.text = "//UPGRADE EQUIPPED";
            source.clip = Sounds[2];
            source.Play();
        }
    }



    public void UnlockUpgrade(int upgradeNumber)
    {
        switch (upgradeNumber)
        {
            case (0):
                controllableLockOnUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (1):
                controllableDistanceUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (2):
                RicochetTrajectoryUpgrade();
                playerUpgrades.LoadUpgrades();
                break;
            case (3):
                WeaponCooldownUpgrade();
                playerUpgrades.LoadUpgrades();
                break;


        }
        
    }

    public void FocusOnMenu()
    {
        StartCoroutine(BlockInput());
        playerPosCoroutine = StartCoroutine(PositionPlayer());
        menuOpened = true;
        animator.SetBool("isSprinting", false);
        animator.SetBool("isJogging", false);
        animator.SetBool("isWalking", false);
        animator.SetBool("isCrouching", false);
        UpgradeMenu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gunCam.m_Priority = 10;
        playerCam.m_Priority = 5;
        customCC.enabled = false;
        source.clip = Sounds[0];
        source.Play();
    }

    public void LeaveMenu()
    {
        
        
        UpgradeMenu.SetActive(false);
        menuOpened = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gunCam.m_Priority = 5;
        playerCam.m_Priority = 10;
        customCC.enabled = true;
        source.clip = Sounds[1];
        source.Play();
        Tooltips.SetActive(false);
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
