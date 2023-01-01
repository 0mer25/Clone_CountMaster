using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] public TextMeshPro CounterText;
    [SerializeField] private GameObject stickman;
    [Range(0f, 1f)] [SerializeField] private float distanceFactor, radius;
    public Transform player;
    public Transform enemy;
    public bool attack;


    void Start()
    {
        for(int i = 0 ; i < Random.Range(20,120) ; i++)
        {
            Instantiate(stickman , transform.position , Quaternion.Euler(-90 , 0 , 0) , transform);
        }

        CounterText.text = (transform.childCount - 1).ToString();

        PlayerManager.instance.FormatStickman(player , distanceFactor , radius);
    }   

    private void Update() {
            //var enemyPos = new Vector3(enemy.position.x , -transform.position.y , enemy.position.z);

        if (attack && transform.childCount > 1)
        {
            var enemyDirection = enemy.position - transform.position;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation,Quaternion.LookRotation(enemyDirection,Vector3.up),
                    Time.deltaTime * 3f);

                if (enemy.childCount > 1)
                {
                    var distance = enemy.GetChild(1).position - transform.GetChild(i).position;

                    if (distance.magnitude < 10f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position,
                            enemy.GetChild(1).position,Time.deltaTime * 2f);
                    } 
                }
              
            }
        }
    }

    public void AttackThem(Transform enemyForce)
    {
        enemy = enemyForce;
        attack = true;

        /* for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run",true);
        } */
    }     

    public void StopAttacking()
    {
         PlayerManager.instance.gameState =  attack = false;
        
        /* for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("run",false);
        } */
        
    }
}
