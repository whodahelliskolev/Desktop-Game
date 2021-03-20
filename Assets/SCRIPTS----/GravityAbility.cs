using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using Cinemachine;

public class GravityAbility : MonoBehaviour
{

    private CinemachineImpulseSource impulseSource;
    public List<GameObject> objects = new List<GameObject>();
    private Vector3 fakeGravity = new Vector3(0, 1, 0);

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }


    public void OnTriggerEnter(Collider other)
    {
        objects.Add(other.gameObject);
        
    }


    public void UseAbility()
    {

        
        impulseSource.GenerateImpulse(Camera.main.transform.position);
        foreach (GameObject obj in objects)
        {
            if (obj.GetComponent<Rigidbody>() != null)
            {
                obj.AddComponent<RestoreGravityScript>();
                obj.AddComponent<ConstantForce>();
                obj.GetComponent<ConstantForce>().force = fakeGravity;

            }

            if (obj.GetComponent<EnemyAI>() != null)
            {
                obj.GetComponent<EnemyAI>().reducePin();
                
            }
        }

    }

    public void OnTriggerExit(Collider other)
    {
   
            objects.Remove(other.gameObject);
            
    }

    public void Update()
    {
        objects.RemoveAll(GameObject => GameObject == null);
    }

}
