using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro CounterText;
    [SerializeField] private GameObject stickman;
    [Range(0f, 1f)] [SerializeField] private float distanceFactor, radius;
    public Transform player;


    void Start()
    {
        for(int i = 0 ; i < Random.Range(20,120) ; i++)
        {
            Instantiate(stickman , transform.position , new Quaternion(0f , 100f , 0f , 1f) , transform);
        }

        CounterText.text = (transform.childCount - 1).ToString();

        PlayerManager.instance.FormatStickman(player , distanceFactor , radius);
    }        

}
