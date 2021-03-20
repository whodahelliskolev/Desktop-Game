using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using UnityEngine.AI;
using RootMotion.FinalIK;
using BehaviorDesigner.Runtime;
using UnityEngine.Events;



public class EnemyHealth : MonoBehaviour
{
    
    public float Health;  
    public float AimSpeed;

    public string Name;
    public string TargetName;

    public string[] FirstNames;
    public string[] LastNames;
    public bool isTarget;
    
   
    
    public UnityEvent TargetKilled;  
    public CharacterController enemyCC;
    public NavMeshAgent agent;
    public PuppetMaster enemyPM;
    public AimIK aimIK;
    public ArmIK armIK;
    public LookAtIK lookatIK;
    public EnemyAI EnemyAI_Ref;
    public BehaviorTree BehaviorTreeRef;
    public EnemyWeaponSystem WeaponSystem;
    public Animator anim;
    public Jammer JammerRef;
    public GameObject MicrochipStash;


    [HideInInspector]
    public bool isDead;
    [HideInInspector]
    public bool lostBalance;
    public EnemyShieldSystem enemyShieldSystem;
    public bool hasShield;
    public void Awake()
    {
        lookatIK.solver.IKPositionWeight = 0;      
        aimIK.solver.IKPositionWeight = 0;       
        armIK.solver.IKPositionWeight = 0;
        armIK.solver.IKRotationWeight = 0;
        Health = (float)BehaviorTreeRef.GetVariable("Health").GetValue();

        if (isTarget)
        {
            Name = TargetName;
        }

        else if (!isTarget)
        {
            Name = FirstNames[Random.Range(0, FirstNames.Length - 1)] + " " + LastNames[Random.Range(0, LastNames.Length - 1)];
        }

        
    }

    public void CheckEnemyStatus()
    {
        
        
        
        if (!EnemyAI_Ref.Balanced && !lostBalance)
        {
            lostBalance = true;         
            armIK.enabled = false;
            enemyPM.angularLimits = true;
            enemyPM.internalCollisions = true;
            lookatIK.solver.headWeight = 0.4f;
            lookatIK.solver.bodyWeight = 0.2f;
            aimIK.enabled = false;                  
            lookatIK.solver.clampWeightHead = 0;

            if (WeaponSystem != null)
            {
                if (!hasShield)
                    WeaponSystem.DropGunOnDeath();
                else if (hasShield)
                    enemyShieldSystem.breakJoint();
            }
        }

        
    }
 
    public void KillEnemy()
    {
        Health = 0;
        isDead = true;
        enemyPM.state = PuppetMaster.State.Dead;
        enemyCC.enabled = false;
        lookatIK.enabled = false;
        aimIK.enabled = false;                   
        enemyCC.enabled = false;   
        BehaviorTreeRef.SetVariableValue("AgentEnabled", false);
        BehaviorTreeRef.enabled = false;
        StopJammerIfEnemyHasOne();
        agent.enabled = false;

        if (WeaponSystem != null)
        {
            if (!hasShield)
                WeaponSystem.DropGunOnDeath();
            else if (hasShield)
                enemyShieldSystem.breakJoint();
        }
    }

  public void StopJammerIfEnemyHasOne()
    {
        if (JammerRef != null)
        {
            JammerRef.JammerActive = false;
            JammerRef.GetComponent<SphereCollider>().enabled = false;                   
            Destroy(JammerRef.gameObject);
        }
    }
  
    public void EnableEnemyIK()
    {
        
            lookatIK.solver.IKPositionWeight = Mathf.Lerp(lookatIK.solver.IKPositionWeight, 1, Time.deltaTime * AimSpeed);         
            aimIK.solver.IKPositionWeight = Mathf.Lerp(lookatIK.solver.IKPositionWeight, 1, Time.deltaTime * AimSpeed);       
            armIK.solver.IKPositionWeight = Mathf.Lerp(armIK.solver.IKPositionWeight, 1, Time.deltaTime * AimSpeed);
            armIK.solver.IKRotationWeight = Mathf.Lerp(armIK.solver.IKRotationWeight, 1, Time.deltaTime * AimSpeed);
        
    }
   
    public void DisableEnemyIK()
    {
        
            lookatIK.solver.IKPositionWeight = Mathf.Lerp(lookatIK.solver.IKPositionWeight, 0, Time.deltaTime * AimSpeed);       
            aimIK.solver.IKPositionWeight = Mathf.Lerp(lookatIK.solver.IKPositionWeight, 0, Time.deltaTime * AimSpeed);
            armIK.solver.IKPositionWeight = Mathf.Lerp(armIK.solver.IKPositionWeight, 0, Time.deltaTime * AimSpeed);
            armIK.solver.IKRotationWeight = Mathf.Lerp(armIK.solver.IKRotationWeight, 0, Time.deltaTime * AimSpeed);     
    }
  
    public void SharpStopIK()
    {
        lookatIK.solver.IKPositionWeight = 0;
        aimIK.solver.IKPositionWeight = 0;
        armIK.solver.IKPositionWeight = 0;
        armIK.solver.IKRotationWeight= 0;
        lookatIK.enabled = false;
        aimIK.enabled = false;
        armIK.enabled = false;
    }


   


    private void Update()
    {

        BehaviorTreeRef.GetVariable("Health").SetValue(Health);
        CheckEnemyStatus();

      
        if ((float)BehaviorTreeRef.GetVariable("Health").GetValue() != 0)
        {
            

            if ((bool)BehaviorTreeRef.GetVariable("PlayerInSight").GetValue() == true)
            {
               EnableEnemyIK();
            }

            else if ((bool)BehaviorTreeRef.GetVariable("PlayerInSight").GetValue() == false)
            {
                DisableEnemyIK();
            }

        }
            
        else if (Health <= 0 && !isDead)
        {
            isDead = true;          
            KillEnemy();

            
               
                
            
        }

        

        

    }
}


