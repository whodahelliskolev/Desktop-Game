using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    // Start is called before the first frame update

    public EnemyHealth enemyHealthRef;
    public SkinnedMeshRenderer Mesh;
    public Material[] Materials;
    public Material mat1;
    public Material mat2;
    public Material Hologram;
    
    void Start()
    {
        
    }


    public void ChangeToHologram()
    {
        Materials = Mesh.materials;
        Materials[1] = Hologram;
        Materials[0] = Hologram;
        Mesh.materials = Materials;

    }
    public void RevertMaterials()
    {
        Materials = Mesh.materials;
        Materials[0] = mat1;
        Materials[1] = mat2;
        Mesh.materials = Materials;
    }




    // Update is called once per frame
    void Update()
    {
        if(ChangeAmmo.CURRENT_BULLET_TYPE == 1)
        {
            ChangeToHologram();
        }

        if (ChangeAmmo.CURRENT_BULLET_TYPE != 1 )
        {
            RevertMaterials();
        }

        if (enemyHealthRef.Health <= 20)
        {
            RevertMaterials();
        }
    }
}
