using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyName : MonoBehaviour
{
    public EnemyHealth EnemyHealth;
    public GameObject Player;
    public GameObject Origin;
    public GameObject MicrochipStash;
    public TextMeshPro Text;
    public GameObject Image;
    public LayerMask NameMask;
    public float DistanceToShowPrompt;
    private bool Identified;
    private bool AlreadyIdentified;
    public float floatValue = 0.7f;
    public float RayLength;
    private RaycastHit Hit;
    private Vector3 newLocation;
    

    public void ShowEnemyName()
    {
        gameObject.transform.LookAt(2 * transform.position - Player.transform.position);
        Text.fontSize = 2;
        if (Identified)
        {
            Image.SetActive(false);
            MicrochipStash.SetActive(true);
            if (EnemyHealth.isTarget)
            {

                Text.fontSize = 1;
                Text.color = Color.red;
                Text.text = EnemyHealth.TargetName;
                if (!AlreadyIdentified)
                {
                    AlreadyIdentified = true;                  
                    EnemyHealth.TargetKilled.Invoke();
                    

                }
                
            }
            else if (!EnemyHealth.isTarget)
            {
                Text.fontSize = 1;
                Text.text = EnemyHealth.Name;
            }
        }
       

    }
   
    public void Float()
    {
        Physics.Raycast(transform.position, Vector3.down, out Hit, RayLength, NameMask, QueryTriggerInteraction.Ignore);
        
        if (Hit.collider != null)
        {
            newLocation = new Vector3(Origin.transform.position.x, Hit.point.y + floatValue, Origin.transform.position.z);
            gameObject.transform.position = newLocation;
        }
           
            
    }
   
    public void CheckDistance()
    {
 
        float Distance = Vector3.Distance(gameObject.transform.position, Player.transform.position);
        
        if(Distance <= DistanceToShowPrompt && EnemyHealth.isDead)
        {
            Text.enabled = true;
            Image.SetActive(true);
            Text.text = "F";
            if (Input.GetKeyDown(KeyCode.F))
            {
               Identified = true;
               
            }
        }

        else if( Distance > DistanceToShowPrompt && EnemyHealth.isDead )
        {
            Image.SetActive(false);
            Text.enabled = false;
            Text.text = " ";
        }
    }
  
    void Update()
    {

       
            Float();
            CheckDistance();
            ShowEnemyName();
        
        
    }
}
