using UnityEngine;

public class FinishLine : MonoBehaviour
{

    public GameObject levelSlider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("npc"))
        {
            levelSlider.GetComponent<LevelProgressUI>().levelFinished = true;
        }
    }
}
