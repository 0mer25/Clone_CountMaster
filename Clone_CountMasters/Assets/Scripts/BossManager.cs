using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour



{
    private Rigidbody _rb;
    public GameObject boss;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody>();  
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Check");
        boss.transform.Translate(0, 0, -10);
    }
}


