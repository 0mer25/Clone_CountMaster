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
    public float playerSpeed, roadSpeed;
    private Camera cam;  
    [SerializeField] private Transform road;
    public bool isFighting;


    void Awake() 
    {
        instance = this;
    }
    void Start() {
        isFighting = false;
        cam = Camera.main;
        player = transform;
        numberOfStickmans = transform.childCount - 1;
        CounterText.text = numberOfStickmans.ToString();
    }

    private void Update() 
    {
        MoveThePlayer();
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

        if(gameState && (isFighting == false))
        {
            road.Translate(-road.forward * Time.deltaTime * roadSpeed);


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
    }
}
