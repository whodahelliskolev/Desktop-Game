using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class objectiveTimer : MonoBehaviour
{

    public float ElapsedTime;
    public bool StopTimer;
    private TextMeshProUGUI TextMesh;
    public string DigitalTimer;
    



    private void Awake()
    {
        TextMesh = GetComponent<TextMeshProUGUI>();
    }
    public void DisplayTimer()
    {
        if (!StopTimer)
        {
            ElapsedTime += Time.deltaTime;
            float minutes = Mathf.FloorToInt(ElapsedTime / 60);
            float seconds = Mathf.FloorToInt(ElapsedTime % 60);
            DigitalTimer = string.Format("{0:00}:{1:00}", minutes, seconds);

        }
        else if (StopTimer)
        {
            
        }


    }
    public void StopTimerVoid()
    {
        StopTimer = true;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        DisplayTimer();
    }
}
