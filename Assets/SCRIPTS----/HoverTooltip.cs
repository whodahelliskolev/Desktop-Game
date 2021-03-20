using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverTooltip : MonoBehaviour
{
    public string GravityAbility;
    public string GravityAbilityDuration;
    public string FocusAbility;
    public string NoMovementCost;
    public string HealthOnHeadshot;
    public string HealthRegen;
    public string AbilityRecharge;
    public string RicochetTrajectory;
    public string RicochetBounces;
    public string ControllableLockOn;
    public string ControllableDistance;
    public string WeaponHeatReducer;
    public GameObject TipWindow;
    public Text tipText;
    

    public void ShowTooltip(int tip)
    {
        TipWindow.SetActive(true);
        switch (tip)
        {
            case (0):          
                tipText.text = GravityAbility;
                break;
            case (1):
                tipText.text = GravityAbilityDuration;
                break;
            case (2):
                tipText.text = FocusAbility;
                break;
            case (3):
                tipText.text = NoMovementCost;
                break;
            case (4):
                tipText.text = HealthOnHeadshot;
                break;
            case (5):
                tipText.text = HealthRegen;
                break;
            case (6):
                tipText.text = AbilityRecharge;
                break;
            case (7):
                tipText.text = RicochetTrajectory;
                break;
            case (8):
                tipText.text = RicochetBounces;
                break;
            case (9):
                tipText.text = ControllableLockOn;
                break;
            case (10):
                tipText.text = ControllableDistance;
                break;
            case (11):
                tipText.text = WeaponHeatReducer;
                break;
        }
    }

    public void HideTooltip()
    {
        TipWindow.SetActive(false);
    }

}
