using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PedestrianTimelines : MonoBehaviour
{
    public GameObject startCanvas;
    public GameObject Canvas1;
    public GameObject Canvas2;
    public GameObject Canvas3;
    public Button play;
    public Button cross;
    public Button walk;
    public Button exit1;
    public Button exit2;
    public PlayableDirector approachTimeline;
    public PlayableDirector walkTimeline;
    public PlayableDirector crossTimeline;
    public GameObject pedestrian;
    public GameObject car;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startCanvas.SetActive(true);
        Canvas1.SetActive(false);
        Canvas2.SetActive(false);
        Canvas3.SetActive(false);
        play.onClick.AddListener(onClickPlay);
        approachTimeline.stopped += OnApproachFinished;
        walkTimeline.stopped += OnWalkFinished;
        crossTimeline.stopped += OnCrossFinished;
        pedestrian.SetActive(false);
        car.SetActive(false);
        Time.timeScale = 0f;
    }

    public void onClickPlay()
    {
        Time.timeScale = 1f;
        approachTimeline.Play();
        startCanvas.SetActive(false);
    }
    void OnApproachFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas1.SetActive(true);
        cross.onClick.AddListener(onClickCross);
        walk.onClick.AddListener(onClickWalk);
    }
    void OnWalkFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas2.SetActive(true);
        exit1.onClick.AddListener(onClickExit);
    }
    void OnCrossFinished(PlayableDirector director)
    {
        StartCoroutine(ShowCrashCanvasWithDelay());
    }

    IEnumerator ShowCrashCanvasWithDelay()
    {
        yield return new WaitForSecondsRealtime(2f); // waits 1 second, even if timeScale is 0
        Time.timeScale = 0f;
        Canvas3.SetActive(true);
        exit2.onClick.AddListener(onClickExit);
    }
    public void onClickExit()
    {
        SceneManager.LoadScene("Menu");
    }
    public void onClickWalk()
    {
        Time.timeScale = 1.00f;
        Canvas1.SetActive(false);
        walkTimeline.Play();
    }
    public void onClickCross()
    {
        Time.timeScale = 1.00f;
        Canvas1.SetActive(false);
        crossTimeline.Play();
    }
}
