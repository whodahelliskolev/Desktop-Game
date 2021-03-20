using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSight : MonoBehaviour
{
    // Start is called before the first frame update
    public LineRenderer LaserSightRenderer;

    RaycastHit LaserSightEnd;

    void Start()
    {
        
    }

    public void Raycast()
    {
        Physics.Raycast(gameObject.transform.position, this.transform.forward * 1000, out LaserSightEnd);
        LaserSightRenderer.SetPosition(0, this.transform.position);
        LaserSightRenderer.SetPosition(1, LaserSightEnd.point);

    }


    // Update is called once per frame
    void LateUpdate()
    {
        Raycast();
    }
}
