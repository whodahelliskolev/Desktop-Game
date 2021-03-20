using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Audio;


public class PlayerAbilityBar : MonoBehaviour
{
   
    public static float Ability;
    
    public float RestoreAbilityRate;
    public TextMeshProUGUI AbilityPercentage;
    public Slider AbilitySlider;


    // Start is called before the first frame update
    void Start()
    {
        Ability = 100;

    }
    void UpdateBar()
    {
        AbilitySlider.value = Ability / 100;


        AbilityPercentage.text = Mathf.RoundToInt(Ability).ToString();
    }





    void RestoreBar()
    {
        if (Ability < 100)
        {
            Ability += Time.deltaTime * PlayerUpgrades.abilityRechargeRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
        RestoreBar();
        
    }
}
