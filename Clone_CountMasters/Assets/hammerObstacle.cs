using UnityEngine;

public class hammerObstacle : MonoBehaviour
{
    void Start()
    {
        ileriDön();
    }

    private void ileriDön()
    {
        LeanTween.rotateLocal(this.gameObject, new Vector3(-90f, -90f, 90f), 1f).setOnComplete(geriDön);
    }

    private void geriDön()
    {
        LeanTween.rotateLocal(this.gameObject, new Vector3(-20f, -90f, 90f), 1f).setOnComplete(ileriDön);
    }
}
