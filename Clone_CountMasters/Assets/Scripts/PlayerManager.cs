using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    private int numberOfStickmans,numberOfEnemyStickmans;
    public string currentScene;
    public Transform player;
    [SerializeField] private TextMeshPro CounterText;
    [SerializeField] private GameObject stickman;
    [Range(0f, 1f)] public float distanceFactor, radius;

    public bool moveByTouch, gameState;
    private Vector3 mouseStartPos, playerStartPos;
    public float playerSpeed, roadSpeed , firstRoadSpeed;
    private Camera cam;  
    [SerializeField] private Transform road;
    [SerializeField] private Transform enemy;
    public bool attack;
    public GameObject SecondCam , firstCam;
    public bool FinishLine=false,moveTheCamera;


    void Awake() 
    {
        instance = this;
    }
    void Start() {
        currentScene = SceneManager.GetActiveScene().name;
        firstRoadSpeed = roadSpeed;
        cam = Camera.main;
        firstCam.SetActive(true);
        player = transform;
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
                                Time.deltaTime * 2f);
                        
                        
                    }
                }
            }
            else
            {
                attack = false;
                roadSpeed = firstRoadSpeed;

                for (int i = 1 ; i < transform.childCount ; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.Euler(-90 , 0 , 180);
                }

                FormatStickman(player , distanceFactor , radius);

                enemy.gameObject.SetActive(false);
            }
            if (transform.childCount == 1)
            {
                enemy.transform.GetChild(1).GetComponent<EnemyManager>().StopAttacking();
                gameObject.SetActive(false);
                SceneManager.LoadScene(currentScene);
            }
            
        }
        else
        {
            MoveThePlayer();
        }

        if (transform.childCount == 1)
        {
            gameState = false;
        }

        
        if(gameState)
        {
            road.Translate(-road.forward * Time.deltaTime * roadSpeed);
        }

        if (moveTheCamera && transform.childCount > 1)
        {
            var cinemachineTransposer = SecondCam.GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineTransposer>();

            var cinemachineComposer = SecondCam.GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineComposer>();

            cinemachineTransposer.m_FollowOffset = new Vector3(4.5f, Mathf.Lerp(cinemachineTransposer.m_FollowOffset.y,
                transform.GetChild(1).position.y + 10f, Time.deltaTime * 1f), -25f);
          
            cinemachineComposer.m_TrackedObjectOffset = new Vector3(0f,Mathf.Lerp(cinemachineComposer.m_TrackedObjectOffset.y,
                4f,Time.deltaTime * 1f),0f);
          
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
        for (int i = 1; i < player.childCount ; i++)
        {
            var x = distanceFactor * Mathf.Sqrt(i) * Mathf.Cos(i * radius);
            var z = distanceFactor * Mathf.Sqrt(i) * Mathf.Sin(i * radius);

            var newPos = new Vector3(x, 0f, z);
            player.transform.GetChild(i).DOLocalMove(newPos, 1f).SetEase(Ease.OutBack);
        }
    }

    public void SpawnStickmans(int count)
    {
        for(int i = 0 ; i < count ; i++)
        {
            Instantiate(stickman , transform.position , Quaternion.Euler(-90 , 0 , 180) , transform);
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

            roadSpeed = firstRoadSpeed;

            other.transform.GetChild(1).GetComponent<EnemyManager>().AttackThem(transform);
            StartCoroutine(UpdateTheEnemyAndPlayerStickMansNumbers());
        }

        if (other.CompareTag("Finish"))
        {
            firstCam.SetActive(false);
            SecondCam.SetActive(true);
            FinishLine = true;
            Tower.TowerInstance.CreateTower(transform.childCount - 1);
            transform.GetChild(0).gameObject.SetActive(false);
            roadSpeed = 8f;
        }

    IEnumerator UpdateTheEnemyAndPlayerStickMansNumbers()
    {

        numberOfEnemyStickmans = enemy.transform.GetChild(1).childCount - 1;
        numberOfStickmans = transform.childCount - 1;

        while (numberOfEnemyStickmans > 0 && numberOfStickmans > 0)
        {
            numberOfEnemyStickmans--;
            numberOfStickmans--;

            enemy.transform.GetChild(1).GetComponent<EnemyManager>().CounterText.text = numberOfEnemyStickmans.ToString();
            CounterText.text = numberOfStickmans.ToString();
            
            yield return null;
        }

        if (numberOfEnemyStickmans == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).rotation = Quaternion.identity;
            }
        }
    }
}
}
