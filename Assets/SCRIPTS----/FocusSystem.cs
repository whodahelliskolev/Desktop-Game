using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Audio;

public class FocusSystem : MonoBehaviour
{
    public Volume ppVolume;
    public float abilityDrain;
    public float postProcessSpeed;
    public static bool focusActive;
    public AudioMixerSnapshot PitchdownSnapshot;
    public AudioMixerSnapshot PitchupSnapshot;
    public UIController uiController;
    
   
    public void enableFocus()
    {
        Time.timeScale = 0.2f;
        Time.fixedDeltaTime = Time.timeScale * .01f;
        PitchdownSnapshot.TransitionTo(0.1f);
        
    }
    public void disableFocus()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * .01f;
        PitchupSnapshot.TransitionTo(0.1f);
        focusActive = false;
        
    }
    public void PitchDownAudio()
    {

        PitchdownSnapshot.TransitionTo(0.1f);
    }
    public void PitchUpAudio()
    {
        PitchupSnapshot.TransitionTo(0.1f);
    }
    public void focusSystem()
    {

        if (!uiController.inMenu)
        {
            switch (focusActive)
            {
                case (true):

                    PlayerAbilityBar.Ability -= abilityDrain * Time.deltaTime;
                    ppVolume.weight = Mathf.Lerp(ppVolume.weight, 1, Time.deltaTime * postProcessSpeed);
                    break;

                case (false):

                    ppVolume.weight = Mathf.Lerp(ppVolume.weight, 0, Time.deltaTime * postProcessSpeed);
                    break;
            }
        }
    }
    public void abilityDepleted()
    {

       
            if (PlayerAbilityBar.Ability <= 0)
            {
                PlayerAbilityBar.Ability = 0;
                disableFocus();
            }
       
    }
    public void focusToggle()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            focusActive = !focusActive;
            
            if (focusActive && !uiController.inMenu)
            {
                enableFocus();
            }
            else if (!focusActive || uiController.inMenu)
            {

                disableFocus();
            }




        }


    }
    private void Update()
    {
        if (PlayerUpgrades.focusUnlocked)
        {
            focusToggle();
            focusSystem();
            abilityDepleted();
        }
        
    }
}
