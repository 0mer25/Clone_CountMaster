using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerFight : MonoBehaviour
{
    [SerializeField] Transform point;
    [SerializeField] Transform enemy , player;
    
    private void Update() {
        if(PlayerManager.instance.isFighting)
        {
            Fight(point , enemy , player);
        }
    }

    private void Awake() {
        player = this.transform;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("enemyArea"))
        {
            enemy = other.transform.parent.GetComponent<Transform>();

            point = other.transform.GetChild(0).GetComponent<Transform>();
            PlayerManager.instance.isFighting = true;
        }
    }

    public void Fight(Transform point , Transform enemy , Transform player)
    {
        for(int i = 1 ; i <= player.childCount - 1 ; i++)
        {
            player.transform.GetChild(i).DOMove(point.position + new Vector3(0f , 0f , 1f), 1f);
        }

        for(int i = 1 ; i <= enemy.childCount - 1 ; i++)
        {
            enemy.transform.GetChild(i).DOMove(point.position - new Vector3(0f , 0f , 1f) , 1f);
        }
    }


}
