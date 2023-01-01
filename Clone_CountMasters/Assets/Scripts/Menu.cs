using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI coinText;
    public TMPro.TextMeshProUGUI resourcePanelText;
    public GameObject imleç;

    private int id;
    private float coinAmount;

    public string levelName;

    private bool isButtonClicked = false;

    void Awake()
    {
        Vibration.Init();
    }

    void Start()
    {
        coinAmount = (int)Random.Range(20, 100);
        coinText.text = coinAmount.ToString();

        resourcePanelText.text = PlayerPrefs.GetFloat("COIN", 0f).ToString();

        ileriDön();
    }

    private void geriDön()
    {
        id = LeanTween.rotateAround(imleç, Vector3.forward, 140, 0.5f).setOnComplete(ileriDön).id;
    }

    private void ileriDön()
    {
        id = LeanTween.rotateAround(imleç, Vector3.forward, -140, 0.5f).setOnComplete(geriDön).id;
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(levelName);
        Debug.Log("SAHNE YÜKLENDİ");
    }

    public void durdurButton()
    {
        if (!isButtonClicked)
        {
            LeanTween.cancel(id);

            if (imleç.transform.rotation.z * 100f <= 140f && imleç.transform.rotation.z * 100f >= 80f)
            {
                // x2
                coinAmount = coinAmount * 2f;
                coinText.text = coinAmount.ToString();

                PlayerPrefs.SetFloat("COIN", PlayerPrefs.GetFloat("COIN", 0f) + coinAmount);
                PlayerPrefs.Save();

                resourcePanelText.text = PlayerPrefs.GetFloat("COIN", 0f).ToString();
            }
            else if (imleç.transform.rotation.z * 100f <= 81f && imleç.transform.rotation.z * 100f >= 40f)
            {
                // x5
                coinAmount = coinAmount * 5f;
                coinText.text = coinAmount.ToString();

                PlayerPrefs.SetFloat("COIN", PlayerPrefs.GetFloat("COIN", 0f) + coinAmount);
                PlayerPrefs.Save();

                resourcePanelText.text = PlayerPrefs.GetFloat("COIN", 0f).ToString();
            }
            else if (imleç.transform.rotation.z * 100f <= 41f && imleç.transform.rotation.z * 100 >= 0f)
            {
                // x3
                coinAmount = coinAmount * 3f;
                coinText.text = coinAmount.ToString();

                PlayerPrefs.SetFloat("COIN", PlayerPrefs.GetFloat("COIN", 0f) + coinAmount);
                PlayerPrefs.Save();

                resourcePanelText.text = PlayerPrefs.GetFloat("COIN", 0f).ToString();
            }

            isButtonClicked = true;
        }
    }
}
