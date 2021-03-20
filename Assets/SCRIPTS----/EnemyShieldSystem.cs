using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShieldSystem : MonoBehaviour
{

    
    private Animator animator;
    public EnemyHealth enemyHealth;
    public FixedJoint shieldJoint;

    // Start is called before the first frame update
    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(3, 1);
    }
    void Start()
    {
        animator.SetLayerWeight(3, 1);
    }
    
    public void breakJoint()
    {
        Destroy(shieldJoint);
    }
   
}
