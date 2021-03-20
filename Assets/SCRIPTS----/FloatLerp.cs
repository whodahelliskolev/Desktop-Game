using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using BehaviorDesigner.Runtime;

public class FloatLerp : Action
{

    public  SharedFloat CurrentValue;
    public SharedFloat NewValue;
    
   
   
    


    public float LerpSpeed;

  
   
    public override TaskStatus OnUpdate()
    {
        CurrentValue.Value = Mathf.Lerp(CurrentValue.Value, NewValue.Value, Time.deltaTime * LerpSpeed);



        
        return TaskStatus.Running;
    }
}
