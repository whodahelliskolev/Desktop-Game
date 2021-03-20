using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Audio;



public class UIController : MonoBehaviour
{
    [Header("References")]
    [Space(5)]
   
    public GameObject RadialMenu; 
    public CinemachineFreeLook FreeLookCam;
    public GunMechanics GunMechanicsRef;    
    public bool inMenu;


    [Header("Audio Options")]
    [Space(5)]
   
    public AudioMixerGroup SFX_Mixer;
    public AudioMixerSnapshot PitchdownSnapshot;
    public AudioMixerSnapshot PitchupSnapshot;
    public AudioClip[] RadialMenuSounds;

    [Header("PostProcessing Options")]
    [Space(5)]
   
    public Volume PP_Volume;
    public float VisualEffectSpeed;




    private float Saturation;
    private float XspeedStore;
    private float YspeedStore;  
    private AudioSource AudioSource;
    private ColorAdjustments colorAdjustmentsRef;
    

   

    public void PitchDownAudio()
    {

        PitchdownSnapshot.TransitionTo(0.1f);
    }
    public void PitchUpAudio()
    {
        PitchupSnapshot.TransitionTo(0.1f);
    }


    public void Awake()
    {
        
        AudioSource = GetComponent<AudioSource>();                
        XspeedStore = FreeLookCam.m_XAxis.m_MaxSpeed;
        YspeedStore = FreeLookCam.m_YAxis.m_MaxSpeed;
        

    }
   

    void Start()
    {
        Cursor.visible = false;
    }

   
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {

            
            AudioSource.PlayOneShot(RadialMenuSounds[0]);
            inMenu = true;
            PitchDownAudio();
            RadialMenu.SetActive(true);
            Time.timeScale = 0.2f;
            Time.fixedDeltaTime = Time.timeScale * .01f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
          
            FreeLookCam.m_XAxis.m_MaxSpeed = 0;
            FreeLookCam.m_YAxis.m_MaxSpeed = 0;
            
        }



        else if (Input.GetKeyUp(KeyCode.Tab) && !FocusSystem.focusActive)
        {

           
            AudioSource.PlayOneShot(RadialMenuSounds[1]);
            inMenu = false;
            PitchUpAudio();
            RadialMenu.SetActive(false);
            Time.timeScale = 1;
            Time.fixedDeltaTime = Time.timeScale * .01f;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            FreeLookCam.m_XAxis.m_MaxSpeed = XspeedStore;
            FreeLookCam.m_YAxis.m_MaxSpeed = YspeedStore;

        }

        else if (Input.GetKeyUp(KeyCode.Tab) && FocusSystem.focusActive)
        {


            AudioSource.PlayOneShot(RadialMenuSounds[1]);
            inMenu = false;
            PitchUpAudio();
            RadialMenu.SetActive(false);        
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            FreeLookCam.m_XAxis.m_MaxSpeed = XspeedStore;
            FreeLookCam.m_YAxis.m_MaxSpeed = YspeedStore;

        }
    }
}
