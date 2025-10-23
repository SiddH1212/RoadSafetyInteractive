using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public Button driver, pedestrian, slowfast, Mistakes1, Mistakes2, quit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        driver.onClick.AddListener(onClickDriver);
        pedestrian.onClick.AddListener(onClickPedestrian);
        quit.onClick.AddListener(onClickQuit);
        slowfast.onClick.AddListener(onClickSlowFast);
        Mistakes1.onClick.AddListener(onClickMistakes1);
        Mistakes2.onClick.AddListener(onClickMistakes2);
    }
    void onClickDriver()
    {
        Debug.Log("Driver button clicked");
        SceneManager.LoadScene("DistractedDriver");
    }
    void onClickPedestrian()
    {
        Debug.Log("Pedestrian button clicked");
        SceneManager.LoadScene("PedestrianAccident");
    }
    void onClickQuit()
    {
        Application.Quit();
    }
    void onClickSlowFast()
    {
        Debug.Log("SlowFast button clicked");
        SceneManager.LoadScene("FastAndSlow");
    }
    void onClickMistakes1()
    {
        Debug.Log("Mistakes1 button clicked");
        SceneManager.LoadScene("Mistakes");
    }
    void onClickMistakes2()
    {
        Debug.Log("Mistakes2 button clicked");
        SceneManager.LoadScene("Mistakes2");
    }
}
