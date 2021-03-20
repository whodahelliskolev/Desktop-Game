using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;

public class FreeAimClamp : MonoBehaviour
{
    public GameObject FreeLookTarget;
    public GameObject Spine;
    public GunMechanics GunMechanicsRef;
    private ArmIK Arm;
    public LookAtIK LookAtIk_Ref;
    public LimbIK lefthandIK;
    
    public float DeactivateSpeedHand;
    public float DeactivateSpeedBody;
    
    public float LowerLimitArm;
    public float LowerLimitBody;
    
    [Range(0,1)]
    public float ArmPosWeight;
    [Range(0, 1)]
    public float ArmRotWeight;

    
    private void Awake()
    {
        Arm = this.gameObject.GetComponent<ArmIK>();
        

    }

    public void CalculateAnglesArm()
    {
        
        float AngleDot = Vector3.Dot(Vector3.forward, Spine.transform.InverseTransformPoint(FreeLookTarget.transform.position));
        

        if (AngleDot < LowerLimitArm)
        {
            GunMechanicsRef.AcceptableShootAngle = false;
            Arm.solver.IKPositionWeight = Mathf.Lerp(Arm.solver.IKPositionWeight, 0, Time.deltaTime * DeactivateSpeedHand);
            Arm.solver.IKRotationWeight = Mathf.Lerp(Arm.solver.IKRotationWeight, 0, Time.deltaTime * DeactivateSpeedHand);
           
        }

        else if(AngleDot > LowerLimitArm)
        {
            GunMechanicsRef.AcceptableShootAngle = true;
            Arm.solver.IKPositionWeight = Mathf.Lerp(Arm.solver.IKPositionWeight, ArmPosWeight, Time.deltaTime * DeactivateSpeedHand);
            Arm.solver.IKRotationWeight = Mathf.Lerp(Arm.solver.IKRotationWeight, ArmRotWeight, Time.deltaTime * DeactivateSpeedHand);
           
        }
        
        
        
        
    }

    public void LerpLeftHand(float rot, float pos)
    {
        lefthandIK.solver.IKPositionWeight = Mathf.Lerp(lefthandIK.solver.IKPositionWeight, pos, Time.deltaTime * DeactivateSpeedHand);
        lefthandIK.solver.IKRotationWeight = Mathf.Lerp(lefthandIK.solver.IKRotationWeight, rot, Time.deltaTime * DeactivateSpeedHand);
    }

    public void CalculateAnglesBody()
    {
        float AngleDot = Vector3.Dot(Vector3.forward, Spine.transform.InverseTransformPoint(FreeLookTarget.transform.position));
        

        if (AngleDot < LowerLimitBody)
        {
            
             LookAtIk_Ref.solver.bodyWeight = Mathf.Lerp(LookAtIk_Ref.solver.bodyWeight, 0, Time.deltaTime * DeactivateSpeedBody);
             
        }

        else if (AngleDot > LowerLimitBody )
        {
            
            
             LookAtIk_Ref.solver.bodyWeight = Mathf.Lerp(LookAtIk_Ref.solver.bodyWeight, 1, Time.deltaTime * DeactivateSpeedBody);
               
        }
    }

    public void LookAtIkControl(float head,float body)
    {
        LookAtIk_Ref.solver.clampWeight = body;
        LookAtIk_Ref.solver.headWeight = head;
    }

    // Update is called once per frame
    void Update()
    {

        if (GunMechanicsRef.isAiming)
        {
            GunMechanicsRef.AcceptableShootAngle = true;
            LerpLeftHand(1, 1);
            LookAtIkControl(0, 0);

        }
       else if (!GunMechanicsRef.isAiming)
        {
            CalculateAnglesBody();           
            CalculateAnglesArm();
            LerpLeftHand(0, 0);
            LookAtIkControl(0.7f, 0.8f);
            
        }
       
    }
}
