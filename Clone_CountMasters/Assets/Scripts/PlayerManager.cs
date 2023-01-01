using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Transform player;
    private int numberOfStickmans;
    [SerializeField] private TextMeshPro CounterText;
    [SerializeField] private GameObject stickman;
    [Range(0f, 1f)] public float distanceFactor, radius;

    public bool moveByTouch, gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed, firstRoadSpeed;
    private Camera cam;
    [SerializeField] private GameObject towerCam;  
    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    public bool attack = false , finish = false;


    void Awake() 
    {
        instance = this;
    }
    void Start() {
        cam = Camera.main;
        player = transform;
        firstRoadSpeed = roadSpeed;
        numberOfStickmans = transform.childCount - 1;
        CounterText.text = numberOfStickmans.ToString();
    }

    private void Update() 
    {
        if(attack)
        {
            var enemyDirection = new Vector3(enemy.position.x , transform.position.y , enemy.position.z) - transform.position;

            for (int i = 1 ; i < transform.childCount ; i++)
            {
                transform.GetChild(i).rotation = Quaternion.Slerp(transform.GetChild(i).rotation , 
                    Quaternion.LookRotation(enemyDirection , Vector3.up) , Time.deltaTime * 4f);
            }

            if(enemy.GetChild(1).childCount > 1)
            {
                for(int i = 0 ; i < transform.childCount ; i++)
                {
                    var distance = enemy.GetChild(1).GetChild(0).position - transform.GetChild(i).position;
                    if(distance.magnitude < 10f)
                    {
                        transform.GetChild(i).position = Vector3.Lerp(transform.GetChild(i).position , 
                            new Vector3(enemy.GetChild(1).GetChild(0).position.x , transform.GetChild(i).position.y , transform.GetChild(i).position.z) , 
                                Time.deltaTime * 1f);
                        
                        
                    }
                }
            }
            else
            {
                attack = false;
                roadSpeed = firstRoadSpeed;

                for (int i = 1; i < transform.childCount ; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.identity;
                }
                FormatStickman(player , distanceFactor , radius);

                enemy.gameObject.SetActive(false);
            }

            if(transform.childCount == 1)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttack();
                gameObject.SetActive(false);

                // Kaybettin ekranı
            }
            
        }
        else
        {
            MoveThePlayer();

            if(transform.childCount == 1)
            {
                gameObject.SetActive(false);

                // Kaybettin ekranı
            }
        }

        
        if(gameState)
        {
            road.Translate(-road.forward * Time.deltaTime * roadSpeed);
        }

    }

    void MoveThePlayer()
    {
        if(Input.GetMouseButtonDown(0) && gameState)
        {
            moveByTouch = true; 

            var plane = new Plane(Vector3.up, 0f);
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray, out var distance))
            {
                mouseStartPos = ray.GetPoint(distance + 1f);
                playerStartPos = transform.position;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            moveByTouch = false;
        }

        if(moveByTouch)
        {
            var plane = new Plane(Vector3.up , 0f);
            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray, out var distance))
            {
                var mousePos = ray.GetPoint(distance + 1f);
                var move = mousePos - mouseStartPos;
                var control = playerStartPos + move;

                if(numberOfStickmans < 50)
                    control.x = Mathf.Clamp(control.x , -8f , 8f);
                else if(numberOfStickmans > 100)
                    control.x = Mathf.Clamp(control.x , -3f , 3f);
                else
                    control.x = Mathf.Clamp(control.x , -5f , 5f);

                transform.position = new Vector3(Mathf.Lerp(transform.position.x, control.x, Time.deltaTime * playerSpeed)
                    , transform.position.y, transform.position.z);
            }
        }
        
    }

    public void FormatStickman(Transform player , float distanceFactor , float radius)
    {
        for (int i = 1; i < player.childCount ; i++)
        {
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var newPos = new Vector3(x, 0f, z);
            player.transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
        }  
        
    }
    public void FormatStickman()
    {
        for (int i = 1; i < this.player.childCount ; i++)
        {
            var x = this.distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * this.radius);
            var z = this.distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * this.radius);

            var newPos = new Vector3(x, 0f, z);
            this.player.transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
        }  
    }

    public void SpawnStickmans(int count)
    {
        for(int i = 0 ; i < count ; i++)
        {
            Instantiate(stickman , transform.position , Quaternion.identity , transform);
        }

        numberOfStickmans = transform.childCount - 1;
        CounterText.text = numberOfStickmans.ToString();

        FormatStickman(player , distanceFactor , radius);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("gate"))
        {
            other.transform.parent.GetChild(0).GetComponent<BoxCollider>().enabled = false; // gate 1
            other.transform.parent.GetChild(1).GetComponent<BoxCollider>().enabled = false; // gate 2

            var gateManager = other.GetComponent<GateManager>();
            if(gateManager.multiply)
            {
                SpawnStickmans((numberOfStickmans * gateManager.randomNumber) - numberOfStickmans);
            }
            else
            {
                SpawnStickmans((numberOfStickmans + gateManager.randomNumber) - numberOfStickmans);
            }
        }

        if(other.CompareTag("enemyArea"))
        {
            enemy = other.transform;
            attack = true;

            roadSpeed = 5f;

            other.transform.GetChild(1).GetComponent<EnemyManager>().AttackThem(transform);
        }


        if(other.CompareTag("Finish"))
        {
            towerCam.SetActive(true);
            finish = true;
            FinishLineTower.instance.CreateTower(transform.childCount - 1);
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
