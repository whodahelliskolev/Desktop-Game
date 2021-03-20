using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Audio;


public class WeaponHeatSystem : MonoBehaviour
{
    public Volume Volume;
    public static float Heat;
    public static bool inJammerRange;
    
    public TextMeshProUGUI AbilityPercentage;
    public Slider HeatBarSlider;
    

    // Start is called before the first frame update
    void Start()
    {
        Heat = 0;

    }
    void UpdateBar()
    {
        HeatBarSlider.value = Heat / 100;


        AbilityPercentage.text = Mathf.RoundToInt(Heat).ToString() + " °";
    }




    public void JammerGlitch()
    {

        if (inJammerRange)
        {
            Volume.enabled = true;
            Heat = 100;


        }

        else if (!inJammerRange)
        {
            Volume.enabled = false;
        }


    }



    void RestoreBar()
    {
        if (Heat > 0)
        {
            Heat -= Time.deltaTime * PlayerUpgrades.weaponCooldownRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
        RestoreBar();
        JammerGlitch();
    }
}
