using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public string levelName;

    void Start()
    {
        Vibration.Init();
    }

    public void nextLevel()
    {
        SceneManager.LoadScene(levelName);
    }
}
