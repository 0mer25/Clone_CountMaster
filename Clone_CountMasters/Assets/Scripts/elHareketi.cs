using UnityEngine;

public class elHareketi : MonoBehaviour
{
    void Start()
    {
        ileriGit();
    }

    private void ileriGit()
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(90f, -61f, 0f), 1f).
                                                                setEase(LeanTweenType.easeInOutCubic)
                                                                .setOnComplete(geriyeGit);
    }

    private void geriyeGit()
    {
        LeanTween.moveLocal(this.gameObject, new Vector3(-90f, -61f, 0f), 1f).
                                                                setEase(LeanTweenType.easeInOutCubic)
                                                                .setOnComplete(ileriGit);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            this.gameObject.transform.parent.gameObject.SetActive(false);
        }
    }
}
