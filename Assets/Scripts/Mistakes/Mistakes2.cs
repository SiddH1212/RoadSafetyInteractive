using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mistakes2 : MonoBehaviour
{
    public GameObject startCanvas, Canvas1, Canvas2, Canvas3, Canvas4;
    public Button playButton, option1Button, option2Button, option3Button;
    public Button exit1, exit2, exit3;
    public PlayableDirector introTimeline, option1Timeline, option2Timeline, option3Timeline;
    public CameraShake cameraShake;
    // public Rigidbody prb; // Reference to the Rigidbody component
    // public Rigidbody brb; // Reference to the other Rigidbody component
    // public WheelRotator wheelRotator; // Reference to the WheelRotator script
    void Start()
    {
        startCanvas.SetActive(true);
        Canvas1.SetActive(false);
        playButton.onClick.AddListener(onClickPlay);
        introTimeline.stopped += onIntroFinished;
        option1Timeline.stopped += onOption1Finished;
        option2Timeline.stopped += onOption2Finished;
        option3Timeline.stopped += onOption3Finished;
        // prb.isKinematic = true; // Ensure Rigidbody is kinematic at start
        // brb.isKinematic = true; // Ensure Rigidbody is kinematic at start
        Time.timeScale = 0f; // Pause the game initially
        // wheelRotator.enabled = true; // Ensure wheel rotation starts
    }
    void onClickPlay()
    {
        startCanvas.SetActive(false);
        Time.timeScale = 1f;
        introTimeline.Play();
    }
    void onIntroFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas1.SetActive(true);
        option1Button.onClick.AddListener(onClickOption1);
        option2Button.onClick.AddListener(onClickOption2);
        option3Button.onClick.AddListener(onClickOption3);
    }
    void onClickOption1()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        // prb.isKinematic = false; // Make Rigidbody non-kinematic to enable physics
        option1Timeline.Play();
    }
    void onClickOption2()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        option2Timeline.Play();
        StartCoroutine(cameraShakeCoroutine(2f));
    }
    void onClickOption3()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        option3Timeline.Play();
        StartCoroutine(cameraShakeCoroutine(2f));
    }
    void onOption1Finished(PlayableDirector director)
    {
        StartCoroutine(ShowCrashCanvasWithDelay(Canvas2));
    }
    void onOption2Finished(PlayableDirector director)
    {
        // Time.timeScale = 0f;
        // Canvas3.SetActive(true);
        // exit2.onClick.AddListener(onClickExit);
        StartCoroutine(ShowCrashCanvasWithDelay(Canvas3));
    }
    void onOption3Finished(PlayableDirector director)
    {
        // wheelRotator.enabled = false; // Stop the wheel rotation
        // brb.isKinematic = false; // Make Rigidbody non-kinematic to enable physics
        StartCoroutine(ShowCrashCanvasWithDelay(Canvas4));
        
    }
    IEnumerator ShowCrashCanvasWithDelay(GameObject crashCanvas)
    {
        yield return new WaitForSecondsRealtime(1f); // waits 1 second, even if timeScale is 0
        Time.timeScale = 0f;
        if (crashCanvas == Canvas2)
        {
            Canvas2.SetActive(true);
            exit1.onClick.AddListener(onClickExit);
        }
        else if (crashCanvas == Canvas3)
        {
            Canvas3.SetActive(true);
            exit2.onClick.AddListener(onClickExit);
        }
        else if (crashCanvas == Canvas4)
        {
            Canvas4.SetActive(true);
            exit3.onClick.AddListener(onClickExit);
        }

    }
    IEnumerator cameraShakeCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        cameraShake.TriggerShake();
    }
    public void onClickExit()
    {
        SceneManager.LoadScene("Menu");
    }
}