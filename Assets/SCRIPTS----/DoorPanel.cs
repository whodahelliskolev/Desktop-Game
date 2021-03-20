using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPanel : MonoBehaviour
{

    public bool panelHit;
    public Light panelLight;
    public Material openedMaterial;
    public Material[] mats;
    private MeshRenderer skinnedmesh;

    private void Awake()
    {
        skinnedmesh = GetComponent<MeshRenderer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 8)
        {
            panelHit = true;
            mats = skinnedmesh.materials;
            mats[1] = openedMaterial;
            skinnedmesh.materials = mats;
            panelLight.color = Color.blue;
            Debug.Log("Hit");
        }
    }
}
