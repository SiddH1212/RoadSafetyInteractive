using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartSceneOnKey : MonoBehaviour
{
    public Button retry;
    void Start()
    {
        retry.onClick.AddListener(RestartScene);
    }

    public void RestartScene()
    {
        // reset time scale (in case it’s paused)
        Time.timeScale = 1f;

        // reload current scene
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
}
