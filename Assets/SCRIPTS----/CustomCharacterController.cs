using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Cinemachine;



public class CustomCharacterController : MonoBehaviour
{




    [Header("References")]
    
   
    public Transform cam;      
    public FreeAimClamp FreeAimClampRef;
    public CinemachineFreeLook FreeLookCam;
    public GunMechanics gunMechanics;

    [Header("Ability")]
    public GravityAbility gravityAbility;
    public GameObject particleObject;
    public ParticleSystem AbilityParticle;
    public GameObject particlePosition;
    public AudioSource abilityAudioSource;
    public AudioClip[] abilitySounds;
    
    [Header("Crouching Options")]
    
    public float LerpSpeed;
    public Vector3 CrouchedColliderCenterOffset;

    [Header("Vaulting Options")]
   
    public float VaultRaycast;   
    public Vector3 VaultColliderCenterOffset;
    public Vector3 NormalColliderCenter = new Vector3(0, 0.85f, 0);
    public LayerMask VaultMask;
    public GameObject ObstacleDetector;

    [Header("MovementAbilityCosts")]
    public float vaultCost;
    public float slideCost;
    public float rollCost;
    public float sprintCost;


    [Header("LocomotionParameters")]   
    public bool isGrounded;
    public float rotationSpeed;
    public float RaycastLength;
    [Range(0,1)]
    public float turnSmoothTime = 0.1f;
    [Range(0, 10)]
    public float dampInput;
    public float Acceleration;
    public float turnSpeed;
    public float fallDamage;
    
    [Header("AirControl")]
    public float moveSpeed;
    public float gravity;
    public float airRotation;

    public LayerMask Mask;
   
    
    
    

    
    
    [HideInInspector]
    public bool CanStrafe;  
    [HideInInspector]
    public bool isJogging; 
    [HideInInspector]
    public bool isCrouching;
    [HideInInspector]
    public bool MenuActive;
    [HideInInspector]
    public bool isSprinting;
    [HideInInspector]
    public bool movementBlocked;



    private Animator playerAnimator;
    private PlayerHealth playerHealth;
    float airTime;  
    CharacterController cc;
    RaycastHit RayHit;
    RaycastHit VaultHit;      
    bool canVault;     
    bool obstacleInfront;
    bool FallingCoroutineStarted;
    private int VertHash = Animator.StringToHash("Y_axis");
    private int HorizHash = Animator.StringToHash("X_axis");  
    private Vector3 rotationOffset;
    float storedVelocity;
  
    public void Awake()
    {
      Cursor.visible = false;
      Cursor.lockState = CursorLockMode.Locked;
      cc = gameObject.GetComponent<CharacterController>();
      playerAnimator = GetComponent<Animator>();
      playerHealth = GetComponent<PlayerHealth>();
        
    }

    public void Start()
    {
        if (PlayerUpgrades.noMovementCost)
        {
            vaultCost = 0;
            rollCost = 0;
            slideCost = 0;
        }
    }


    public void VaultCheck()
    {
        Vector3 fwd = ObstacleDetector.transform.TransformDirection(Vector3.forward); 
       
        Physics.Raycast(ObstacleDetector.transform.position, fwd, out VaultHit, VaultRaycast ,VaultMask);
        
      
      
        if(VaultHit.collider == null) // if there is no collider forbid vaulting
        {
            canVault = false;
            obstacleInfront = false;
            playerAnimator.SetBool("CanVault", false);
        }
        
        else if (VaultHit.collider.CompareTag("Vaultable")) // allow vaulting
        {
            canVault = true;
            playerAnimator.SetBool("CanVault", true);

        }

        else if (VaultHit.collider !=null && !VaultHit.collider.CompareTag("Vaultable"))// forbid vaulting and rolling
        {
            canVault = false;
            obstacleInfront = true;
            playerAnimator.SetBool("CanVault", false);
        }
        else  // forbid vaulting
        {
            canVault = false;
            obstacleInfront = false;
            playerAnimator.SetBool("CanVault", false);
        }
    }
    public void handleInput()
    {

       if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            
            playerAnimator.SetBool("isJogging", true);
        }

       else if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            playerAnimator.SetBool("isJogging", false);
            
            
        }

       if (Input.GetKeyDown(KeyCode.LeftControl) )
        {
            playerAnimator.SetBool("isWalking", true);
            
            

        }
      
       else if (Input.GetKeyUp(KeyCode.LeftControl))
          {
            playerAnimator.SetBool("isWalking", false);
            
        }

       if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isJogging = true;
            isSprinting = true;
            playerAnimator.SetBool("isSprinting", true);

            
        }

       else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isJogging = false;
            isSprinting = false;
            playerAnimator.SetBool("isSprinting", false);
           


        }
       
       if (Input.GetKeyDown(KeyCode.C))
        {
            if (isSprinting && !movementBlocked && !gunMechanics.isAiming && !obstacleInfront && PlayerAbilityBar.Ability >= slideCost)
            {
                isCrouching = !isCrouching;
                StartCoroutine(Slide());
            }

            else if (!movementBlocked)
            {
                isCrouching = !isCrouching;
            }
            else if(movementBlocked)
            {
                isCrouching = false;
            }
        }


        if (Input.GetKeyDown(KeyCode.Space) && !canVault && isGrounded
                                                  && !isCrouching && !movementBlocked
                                                  && !obstacleInfront && !playerHealth.Injured
                                                  && gameObject.GetComponent<CharacterController>().velocity.magnitude > 0.5f
                                                  && PlayerAbilityBar.Ability >= rollCost)
                                                     StartCoroutine(Jump());

       else if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !movementBlocked
                                                 && canVault 
                                                 && !playerHealth.Injured
                                                 && PlayerAbilityBar.Ability >= vaultCost)                                                    
                                                    StartCoroutine(Vault());

        if (Input.GetKeyDown(KeyCode.G))
        {
            if ((PlayerAbilityBar.Ability - 50) > 0 && PlayerUpgrades.gravityAbilityUnlocked)
            {
                StartCoroutine(UseAbility());
                
            }
        }

    }  
    public void Ability()
    {
        StartCoroutine(AbilitySounds());
        particleObject.transform.position = particlePosition.transform.position;
        AbilityParticle.Play();
        gravityAbility.UseAbility();
        PlayerAbilityBar.Ability -= 50;

    }
   
    public IEnumerator AbilitySounds()
    {
        abilityAudioSource.clip = abilitySounds[0];
        abilityAudioSource.Play();
        yield return new WaitForSeconds(PlayerUpgrades.gravityAbilityDuration);
        abilityAudioSource.clip = abilitySounds[1];
        abilityAudioSource.Play();
    }
    public void StrafeMove()
    {
        
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(gameObject.transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, 0f), Time.deltaTime * rotationSpeed);
    }
    public void FreeMove()
    {
        
        transform.forward += Vector3.Lerp(transform.forward, rotationOffset, Time.deltaTime * turnSpeed);
    }
    public void GetDirection()
    {
       
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 Direction = new Vector3(horizontal, 0, vertical).normalized;
        rotationOffset = cam.transform.TransformDirection(Direction);
        rotationOffset.y = 0;
        
    }
    public void PlayerLocomotion()
    {

       
       
        if (!isJogging && !isCrouching) /// Walk
        {
            float localX = Mathf.Clamp(Input.GetAxis("Horizontal"), -0.5f, 0.5f);
            float localY = Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 0.5f);
            
            playerAnimator.SetFloat(HorizHash, localX, dampInput, Time.deltaTime * Acceleration);
            playerAnimator.SetFloat(VertHash, localY, dampInput, Time.deltaTime * Acceleration);

            

        }


       else if (isJogging && !isCrouching) /// Jog
        {

            float localX = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);
            float localY = Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 1f);
            playerAnimator.SetFloat(HorizHash, localX, dampInput, Time.deltaTime * Acceleration);
            playerAnimator.SetFloat(VertHash, localY, dampInput, Time.deltaTime * Acceleration);


           

        }


        else if (isJogging && isCrouching) /// Fast Crouch
        {
            float localX = Mathf.Clamp(Input.GetAxis("Horizontal"), -1f, 1f);
            float localY = Mathf.Clamp(Input.GetAxis("Vertical"), -1f, 1f);
            playerAnimator.SetFloat(HorizHash, localX, dampInput, Time.deltaTime * Acceleration);
            playerAnimator.SetFloat(VertHash, localY, dampInput, Time.deltaTime * Acceleration);
            

        }

        else if (!isJogging && isCrouching) /// Crouch
        {
            float localX = Mathf.Clamp(Input.GetAxis("Horizontal"), -0.5f, 0.5f);
            float localY = Mathf.Clamp(Input.GetAxis("Vertical"), -0.5f, 0.5f);
            playerAnimator.SetFloat(HorizHash, localX, dampInput, Time.deltaTime * Acceleration);
            playerAnimator.SetFloat(VertHash, localY, dampInput, Time.deltaTime * Acceleration);

           

        }



        
       
     

    }
    public void CrouchCollider()
    {
        if (isCrouching)
        // cc.center = iTween.Vector3Update(cc.center, CrouchedColliderCenterOffset, LerpSpeed);
        {
            playerAnimator.SetBool("isCrouching", true);
            
        }
        else if (!isCrouching)
            // cc.center = iTween.Vector3Update(cc.center, NormalColliderCenter, LerpSpeed);
            playerAnimator.SetBool("isCrouching", false);
    }
    public void ApplyGravity()
    {
        Vector3 forward = storedVelocity * transform.TransformDirection(Vector3.forward) * moveSpeed;
        transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * airRotation * Time.deltaTime, 0));
        if (!cc.isGrounded)
            forward.y -= gravity * Time.deltaTime;

        cc.Move(forward * Time.deltaTime);
    }
    public void RaycastGround()
    {



        Physics.SphereCast(this.transform.position + Vector3.up * (cc.radius + Physics.defaultContactOffset), cc.radius - Physics.defaultContactOffset, Vector3.down, out RayHit, RaycastLength, Mask);



        if (RayHit.collider == null)
        {
            storedVelocity = cc.velocity.magnitude;
            isGrounded = false;
            playerAnimator.applyRootMotion = false;
            ApplyGravity();
            airTime += Time.deltaTime;
            playerAnimator.SetBool("isGrounded", false);
            playerAnimator.SetBool("isAiming", false);
            playerAnimator.SetFloat("AirTime", airTime);
            

        }

        else if(RayHit.collider.CompareTag("HealthPickup"))
        {
            RayHit.collider.gameObject.SetActive(false);
            PlayerHealth.Syringes++;
        }

        else if(RayHit.collider != null)
        {          
            isGrounded = true;
            playerAnimator.applyRootMotion = true;
            
            airTime = 0;
            playerAnimator.SetBool("isGrounded", true);
            playerAnimator.SetFloat("AirTime", airTime);

            if (!FallingCoroutineStarted && airTime >= 0.5f && airTime < 0.8f)
            {
                StartCoroutine(RollAfterFall());
                airTime = 0;
                
            }
            else if(airTime >= 0.8f)
            {

                StartCoroutine(RollAfterFall());
                PlayerHealth.Health -= airTime * fallDamage;
                airTime = 0;
            }
            

        }
    }
    public void LerpColliderVauilt()
    {   
           if(!movementBlocked)
            cc.center = iTween.Vector3Update(cc.center, NormalColliderCenter, 30f);
        
    }

    public IEnumerator UseAbility()
    {

        movementBlocked = true;
        playerAnimator.SetBool("useAbility", true);
        yield return new WaitForSeconds(0.3f);   
        playerAnimator.SetBool("useAbility", false);
        yield return new WaitForSeconds(0.3f);
        movementBlocked = false;


    }


    public IEnumerator Vault() /// CHANGE to movement blocked
    {
        PlayerAbilityBar.Ability -= vaultCost;
        movementBlocked = true;
        playerAnimator.SetBool("isVaulting", true);   
        yield return new WaitForSeconds(0.3f);
        cc.center = VaultColliderCenterOffset;
        playerAnimator.SetBool("isVaulting", false);     
        yield return new WaitForSeconds(0.3f);
        movementBlocked = false;



    }
    public IEnumerator Slide()
    {
        PlayerAbilityBar.Ability -= slideCost;
        movementBlocked = true;
        playerAnimator.SetBool("canSlide", true);
        yield return new WaitForSeconds(0.2f);
        playerAnimator.SetBool("canSlide", false);
        yield return new WaitForSeconds(1.0f);
        movementBlocked = false;
    }
    public IEnumerator Jump()
    {

        PlayerAbilityBar.Ability -= rollCost;
        movementBlocked = true;
            playerAnimator.SetBool("isJumping", true);
            yield return new WaitForSeconds(0.3f);
            playerAnimator.SetBool("isJumping", false);         
            yield return new WaitForSeconds(1f);
            movementBlocked = false;

    }
    public IEnumerator RollAfterFall()
    {
        FallingCoroutineStarted = true;
        movementBlocked = true;
        yield return new WaitForSeconds(1f);
        movementBlocked = false;
        FallingCoroutineStarted = false;
    }
    public void StrafeOrFreeMovement()
    {

        switch (CanStrafe   && !movementBlocked) 
        {
            case (true):
                StrafeMove();        
                break;

            case (false):
                FreeMove();
                break;
        }

       

    }
    public void LookForward()
    {
        Vector3 rotationOffset = Camera.main.transform.position - transform.position;
        rotationOffset.y = 0;
        float lookDirection = Vector3.Angle(transform.forward, rotationOffset);
        float Angle = Vector3.Dot(Vector3.forward, transform.InverseTransformPoint(Camera.main.transform.position));
        
       
    }

    private void FixedUpdate()
    {
        StrafeOrFreeMovement();
    }


    void Update()
    {


      
        VaultCheck();
        LerpColliderVauilt();
        RaycastGround();   
        handleInput();
        PlayerLocomotion();
        CrouchCollider();
        LookForward();
        GetDirection();

    }

    
}
