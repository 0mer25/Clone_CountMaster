using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StickmanManager : MonoBehaviour
{
    [SerializeField] AudioSource audioo;
    [SerializeField] AudioClip olum , merdiven , kazanma , coin;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("enemy") && other.transform.parent.childCount > 0)
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        switch (other.tag)
        {
           case "enemy":
               if (other.transform.parent.childCount > 0)
               {
                   Destroy(other.gameObject);
                   Destroy(gameObject);
                   //audioo.clip = olum;
                   //audioo.Play();
               }
               
               break;
           
           /* case "jump":          // Rampa kodu

               transform.DOJump(transform.position, 1f, 1, 1f).SetEase(Ease.Flash).OnComplete(PlayerManager.instance.FormatStickman);
               
               break; */
        }

        if(other.CompareTag("stairs"))
        {
            transform.parent.parent = null;
            transform.parent = null;
            GetComponent<Rigidbody>().isKinematic = GetComponent<Collider>().isTrigger = false;

            if (!PlayerManager.instance.moveTheCamera)
                PlayerManager.instance.moveTheCamera = true;
                //audioo.clip = merdiven;
                //audioo.Play();

        }
    }
}
