using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using ES3Internal;
using ES3Types;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("RicochetUpgrades")]
   
    [ShowInInspector]
    public static int Ricochets;
   
    [ShowInInspector]
    public static bool ricochetTrajectoryUnlocked;

    
    [ShowInInspector]
    public static bool controllableDistanceUnlocked;
    [ShowInInspector]
    public static bool controllableAutoLockOnUnlocked;
   
   
    [ShowInInspector]
    public static bool gravityAbilityUnlocked;

    [ShowInInspector]
    public static float gravityAbilityDuration;
    
    
    [ShowInInspector]
    public static bool focusUnlocked;
    [ShowInInspector]
    public static bool noMovementCost;
    [ShowInInspector]
    public static bool healthRegen;
    [ShowInInspector]
    public static bool healthOnHeadshot;

    [ShowInInspector]  
    public static float abilityRechargeRate;


    
    [ShowInInspector]
    public static float weaponCooldownRate;

   
    [ShowInInspector]
    public static int currency;

    private void Start()
    {
        LoadUpgrades();
    }



    public void LoadUpgrades()
    {
        if (!ES3.KeyExists("ricochetTrajectory"))
        {
            ES3.Save("ricochetTrajectory", false);
            ricochetTrajectoryUnlocked = ES3.Load<bool>("ricochetTrajectory");
        }
        else if (ES3.KeyExists("ricochetTrajectory"))
            {
            ricochetTrajectoryUnlocked = ES3.Load<bool>("ricochetTrajectory");
            }

        if (!ES3.KeyExists("noMovementCost"))
        {
            ES3.Save("noMovementCost", false);
            noMovementCost = ES3.Load<bool>("noMovementCost");
        }
        else if (ES3.KeyExists("noMovementCost"))
        {
            noMovementCost = ES3.Load<bool>("noMovementCost");
        }

        if (!ES3.KeyExists("healthRegen"))
        {
            ES3.Save("healthRegen", false);
            healthRegen = ES3.Load<bool>("healthRegen");
        }
        else if (ES3.KeyExists("healthRegen"))
        {
            healthRegen = ES3.Load<bool>("healthRegen");
        }

        if (!ES3.KeyExists("healthOnHeadshot"))
        {
            ES3.Save("healthOnHeadshot", false);
            healthOnHeadshot = ES3.Load<bool>("healthOnHeadshot");
        }
        else if (ES3.KeyExists("healthOnHeadshot"))
        {
            healthOnHeadshot = ES3.Load<bool>("healthOnHeadshot");
        }





        if (!ES3.KeyExists("currency"))
        {
            ES3.Save<int>("currency", 0);
            currency = ES3.Load<int>("currency");
        }
        else if (ES3.KeyExists("currency"))
        {
            currency = ES3.Load<int>("currency");
        }




        if (!ES3.KeyExists("controllableDistance"))
        {
            ES3.Save("controllableDistance", false);
            controllableDistanceUnlocked = ES3.Load<bool>("controllableDistance");
        }
        else if (ES3.KeyExists("controllableDistance"))
        {
            controllableDistanceUnlocked = ES3.Load<bool>("controllableDistance");
        }


        if (!ES3.KeyExists("controllableAutoLockOn"))
        {
            ES3.Save("controllableAutoLockOn", false);
            controllableAutoLockOnUnlocked = ES3.Load<bool>("controllableAutoLockOn");
        }
        else if (ES3.KeyExists("controllableAutoLockOn"))
        {
            controllableAutoLockOnUnlocked = ES3.Load<bool>("controllableAutoLockOn");
        }

        if (!ES3.KeyExists("gravityAbility"))
        {
            ES3.Save("gravityAbility", false);
            gravityAbilityUnlocked = ES3.Load<bool>("gravityAbility");
        }
        else if (ES3.KeyExists("gravityAbility"))
        {
            gravityAbilityUnlocked = ES3.Load<bool>("gravityAbility");
        }

        if (!ES3.KeyExists("gravityAbilityDuration"))
        {
            ES3.Save<float>("gravityAbilityDuration", 3);
            gravityAbilityDuration = ES3.Load<float>("gravityAbilityDuration");
        }
        else if (ES3.KeyExists("gravityAbilityDuration"))
        {
            gravityAbilityDuration = ES3.Load<float>("gravityAbilityDuration");
        }

        if (!ES3.KeyExists("focusUnlocked"))
        {
            ES3.Save("focusUnlocked", false);
            focusUnlocked = ES3.Load<bool>("focusUnlocked");
        }
        else if (ES3.KeyExists("focusUnlocked"))
        {
            focusUnlocked = ES3.Load<bool>("focusUnlocked");
        }

        if (!ES3.KeyExists("abilityRechargeRate"))
        {
            ES3.Save<float>("abilityRechargeRate", 5);
            abilityRechargeRate = ES3.Load<float>("abilityRechargeRate");
        }
        else if (ES3.KeyExists("abilityRechargeRate"))
        {
            abilityRechargeRate = ES3.Load<float>("abilityRechargeRate");
        }


        if (!ES3.KeyExists("weaponCooldownRate"))
        {
            ES3.Save<float>("weaponCooldownRate", 2);
            weaponCooldownRate = ES3.Load<float>("weaponCooldownRate");
        }
        else if (ES3.KeyExists("weaponCooldownRate"))
        {
            weaponCooldownRate = ES3.Load<float>("weaponCooldownRate");
        }


    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ES3.Save<int>("currency", 500);
            currency = ES3.Load<int>("currency");
        }
    }
}
