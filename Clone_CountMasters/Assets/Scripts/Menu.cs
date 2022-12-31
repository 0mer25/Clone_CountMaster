using UnityEngine;

public class Menu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI coinText;

    private int coinAmount;

    void Start()
    {
        coinAmount = (int)Random.Range(100, 300);
        coinText.text = coinAmount.ToString();
    }
}
