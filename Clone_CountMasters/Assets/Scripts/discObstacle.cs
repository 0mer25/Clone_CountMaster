using UnityEngine;

public class discObstacle : MonoBehaviour
{
    void Start()
    {
        LeanTween.rotateAround(this.gameObject, Vector3.up, -360f, 6f).setLoopClamp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("npc"))
        {
            //TODO: İleride patlama partikülü koyup sonra yok edicez. Fakat şimdilik bu yeterli.
            Vibration.VibratePop();
            Destroy(other.gameObject);
        }
    }
}
