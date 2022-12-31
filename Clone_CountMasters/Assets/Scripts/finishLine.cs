using UnityEngine;

public class finishLine : MonoBehaviour
{

    public GameObject levelSlider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("npc"))
        {
            levelSlider.GetComponent<levelProgressUI>().levelFinished = true;
        }
    }
}
