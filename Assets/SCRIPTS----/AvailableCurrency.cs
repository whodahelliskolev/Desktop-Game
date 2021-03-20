using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ES3Internal;
using ES3Types;
using TMPro;

public class AvailableCurrency : MonoBehaviour
{
    private TextMeshProUGUI currencyText;

    private void Awake()
    {
        currencyText = GetComponent<TextMeshProUGUI>();
    }
    // Start is called before the first frame update
   
    // Update is called once per frame
    void Update()
    {
        currencyText.text = ES3.Load<int>("currency").ToString() + " " + "MICROCHIPS";
    }
}
