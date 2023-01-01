using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StickmanManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        switch(other.tag)
        {
            case "enemy":
                if(other.transform.parent.childCount > 0)
                {
                    Destroy(other.gameObject);
                    Destroy(this.gameObject);
                }
                break;
            
            case "jump":
                transform.DOJump(transform.position , 3f , 1 , 2f).SetEase(Ease.Flash).OnComplete(PlayerManager.instance.FormatStickman);
                break;
        }
    }
}
