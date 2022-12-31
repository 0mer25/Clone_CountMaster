using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDie : MonoBehaviour
{
    bool dead = false;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("enemy") && !dead)
        {
            dead = true;
            Destroy(other.gameObject);
            Destroy(this.gameObject);

            // Ölüm sesi
        }
    }
}
