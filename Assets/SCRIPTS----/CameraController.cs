using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{


    public float StopRecenteringDelay;
    public CinemachineFreeLook FreeLookCam;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StopCameraRecentering());
    }


    public IEnumerator StopCameraRecentering()
    {
        FreeLookCam.m_YAxisRecentering.m_enabled = true;
        yield return new WaitForSeconds(StopRecenteringDelay);
        FreeLookCam.m_YAxisRecentering.m_enabled = false;
    }

    
}
