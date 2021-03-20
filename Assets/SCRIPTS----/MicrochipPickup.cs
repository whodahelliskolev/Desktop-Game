using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES3Internal;
using ES3Types;

public class MicrochipPickup : MonoBehaviour
{

    public float Speed;
    public float destroyDelay;
    public int Amount;
    public bool playerNearby;
    private int bodypartsInContact;
    GameObject player;
    Rigidbody rb;
    AudioSource audiosrc;
    MeshRenderer meshr;
    BoxCollider boxCol;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audiosrc = GetComponent<AudioSource>();
        boxCol = GetComponent<BoxCollider>();
        meshr = GetComponent<MeshRenderer>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bodypartsInContact++;
            player = other.gameObject;
            if (bodypartsInContact >= 1)
            {
                playerNearby = true;
                
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bodypartsInContact--;
            if (bodypartsInContact <= 0)
            {
                bodypartsInContact = 0;
                playerNearby = false;
                
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerUpgrades.currency += Amount;
            ES3.Save<int>("currency", PlayerUpgrades.currency);
            audiosrc.Play();
            meshr.enabled = false;
            boxCol.enabled = false;
            StartCoroutine(delay());
        }
    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (playerNearby)
        {
            
            rb.AddForce((player.transform.position - transform.position) * Speed);
        }
    }


   
}
