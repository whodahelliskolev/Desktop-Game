using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltSpine : MonoBehaviour
{
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(Camera.main.transform.eulerAngles.x,
                                                       Camera.main.transform.eulerAngles.y,
                                                       gameObject.transform.eulerAngles.z);
    }
}
