using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFinish : MonoBehaviour
{
    [SerializeField] GameObject nextLevel;
    [SerializeField] float waitSec;
    IEnumerator WaitForTower(float sec)
    {
        yield return new WaitForSecondsRealtime(sec);
        nextLevel.SetActive(true);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("npc"))
        {
            PlayerManager.instance.roadSpeed = 9.31f;
            StartCoroutine(WaitForTower(waitSec));
        }
    }
}
