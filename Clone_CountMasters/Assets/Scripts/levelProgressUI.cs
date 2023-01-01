using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class levelProgressUI : MonoBehaviour
{
    [Header("UI Ref")]
    [SerializeField] private Image uiFillImage;
    [SerializeField] private TMPro.TextMeshProUGUI levelText;

    // Player'ın rigidbody, collider olması ve isTrigger = true olması gerekiyor.
    [Header("Player ve Finish Ref")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform finishLineTransform;

    private Vector3 finishLinePosition;
    private float fullDistance;

    public bool levelFinished = false;

    void Start()
    {
        uiFillImage.fillAmount = 0f;

        // Sıfırıncı index'ten başlıyor +1 ekleyip 1.level olarak gösteriyorum.
        levelText.text = (SceneManager.GetActiveScene().buildIndex + 1).ToString();

        finishLinePosition = finishLineTransform.position;
        fullDistance = getDistance();
    }

    void Update()
    {
        if (levelFinished == false)
        {
            float newDistance = getDistance();
            float progressValue = Mathf.InverseLerp(fullDistance, 0.249f, newDistance);
            updateProgressFill(progressValue);
        }
    }

    private void updateProgressFill(float value)
    {
        /*
        // fill, finish ikonunun arkasına gelmesin.
        if (value >= 0.75f)
        {
            value = 0.75f;
        }
        */

/*
        // fill, baştaki ui topunu doldursun
        if (value <= 0.249f)
        {
            value = 0.249f;
        }
        */

        uiFillImage.fillAmount = value;
    }

    private float getDistance()
    {
        return Vector3.Distance(playerTransform.position, finishLinePosition);
    }
}
