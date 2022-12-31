using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GateManager : MonoBehaviour
{
    public TextMeshPro gateNo;
    public int randomNumber;
    public bool multiply;

    private void Start() {
        if(multiply)
        {
            randomNumber = Random.Range(2,5);
            gateNo.text = "x" + randomNumber.ToString();
        }
        else
        {
            randomNumber = Random.Range(1,11) * 10;
            gateNo.text = "+" + randomNumber.ToString();
        }
    }
}
