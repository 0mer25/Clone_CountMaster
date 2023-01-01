using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("enemy") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
