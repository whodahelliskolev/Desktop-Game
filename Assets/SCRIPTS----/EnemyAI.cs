using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using RootMotion.Dynamics;
using UnityEngine.AI;
using BehaviorDesigner;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.ObjectDrawers;



public class EnemyAI : MonoBehaviour
{

    [Header("References")]
    public NavMeshAgent agent;  
    public EnemyHealth EnemyHealthRef;   
    public GameObject LastSightingLocation;
    public GameObject Player;
    public PuppetMaster puppetMaster;
    
    
    [Header("Behavior Options")]
    public bool Balanced;
    public float LerpSpeed;



    private BehaviorTree BehaviorTree;
    private Animator Animator;
    private float DesiredSpeed;
    private float DesiredStrafe;
    private float CurrentSpeed;
    private float CurrentStrafe;
    bool useAgentVelocity = true;
    

    


 
    
   
    /// Private Variables

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Balanced = true;   
        BehaviorTree = GetComponent<BehaviorTree>();
        BehaviorTree.SetVariableValue("Balanced", Balanced);
    }

    public void reducePin()
    {
        puppetMaster.pinWeight = 0;
    }

    public void AgentParameters(float speed, float angularSpeed, float acceleration, float stoppingDistance)
    {

        agent.speed = speed;
        agent.angularSpeed = angularSpeed;
        agent.acceleration = acceleration;
        agent.stoppingDistance = stoppingDistance;
      

    }
    
    public void SetStoppingDistance(float StoppingDistance)
    {
        agent.stoppingDistance = StoppingDistance;
    }
    public void MoveLastSightingPosition() 
    {
        LastSightingLocation.transform.position = Player.transform.position;
    
    }
    public void StopMovingLastSightingPosition()
    {
        LastSightingLocation.transform.position = LastSightingLocation.transform.position;

    }
    public void Crouch()
    {
        Animator.SetBool("canCrouch", true);
    }
    public void StandUp()
    {
        Animator.SetBool("canCrouch", false);
    }
    public void ChangeSpeedAndDirection(float speed, float strafe)
    {
        DesiredSpeed = speed;
        DesiredStrafe = strafe;
        
    }
    public void ChangeSpeed(float speed)
    {
        DesiredSpeed = speed;
        
    }
    public void UseAgentVelocity(bool agentVelocity)
    {
        useAgentVelocity = agentVelocity;
        Animator.stabilizeFeet = true;
        Animator.feetPivotActive = 1;
    }
    void MoveWithVelocity()
    {
        Animator.SetFloat("Speed", agent.velocity.magnitude);
    }
    public void LoseBalance()
    {
        Balanced = false;
        BehaviorTree.SetVariableValue("Balanced", Balanced);     
    }
    public void RegainBalance()
    {
        Balanced = true;
        BehaviorTree.SetVariableValue("Balanced", Balanced);      
    }
    public void Lerp()
    {     
        CurrentSpeed = Mathf.Lerp(CurrentSpeed, DesiredSpeed, Time.deltaTime * LerpSpeed);
        CurrentStrafe = Mathf.Lerp(CurrentStrafe, DesiredStrafe, Time.deltaTime * LerpSpeed);
        Animator.SetFloat("Speed", CurrentSpeed);
        Animator.SetFloat("Strafe", CurrentStrafe);
        Animator.stabilizeFeet = true;
        Animator.feetPivotActive = 1;
    }
    public void Update()
    {
        
        
        if (useAgentVelocity)
        {
           MoveWithVelocity();
           CurrentStrafe = Mathf.Lerp(CurrentStrafe, 0, Time.deltaTime * LerpSpeed);
        }
            
       
        else if (!useAgentVelocity)
            Lerp();
    }
}
