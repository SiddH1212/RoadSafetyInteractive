using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PedestrianTimelines : MonoBehaviour
{
    [Header("UI References")]
    public GameObject startCanvas;
    public GameObject Canvas1;
    public GameObject Canvas2;
    public GameObject Canvas3;
    public Button play;
    public Button cross;
    public Button walk;
    public Button exit1;
    public Button exit2;
    public Button retry1, retry2;

    [Header("Scene Objects")]
    public PlayableDirector approachTimeline;
    public PlayableDirector walkTimeline;
    public PlayableDirector crossTimeline;
    public GameObject pedestrian;
    public GameObject ped;
    public GameObject car;

    [Header("Audio Sources")]
    public AudioSource crashSound;
    public AudioSource hornSound;
    public AudioSource trafficSound;
    public AudioSource crosswalkSound;
    public AudioSource buttonSound;
    private Coroutine timelineSoundCoroutine;

    void Start()
    {
        // Initial UI setup
        startCanvas.SetActive(true);
        Canvas1.SetActive(false);
        Canvas2.SetActive(false);
        Canvas3.SetActive(false);

        play.onClick.AddListener(onClickPlay);
        retry1.onClick.AddListener(onClickRetry);
        retry2.onClick.AddListener(onClickRetry);

        approachTimeline.stopped += OnApproachFinished;
        walkTimeline.stopped += OnWalkFinished;
        crossTimeline.stopped += OnCrossFinished;

        pedestrian.SetActive(false);
        car.SetActive(false);

        // Start looping traffic sound
        if (trafficSound != null)
        {
            trafficSound.loop = true;
            trafficSound.Play();
        }

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
        StopTimelineSounds();

        Time.timeScale = 0f;
        Canvas2.SetActive(true);
        exit1.onClick.AddListener(onClickExit);
    }

    void OnCrossFinished(PlayableDirector director)
    {
        StopTimelineSounds();
        StartCoroutine(ShowCrashCanvasWithDelay());
    }

    IEnumerator ShowCrashCanvasWithDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
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
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        walkTimeline.Play();

        // Start sound trigger coroutine
        if (timelineSoundCoroutine != null)
            StopCoroutine(timelineSoundCoroutine);

        timelineSoundCoroutine = StartCoroutine(PlayWalkTimelineSounds());
    }

    public void onClickCross()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        crossTimeline.Play();

        // Start horn/crash for cross timeline
        if (timelineSoundCoroutine != null)
            StopCoroutine(timelineSoundCoroutine);

        timelineSoundCoroutine = StartCoroutine(PlayCrossTimelineSounds());
    }

    private IEnumerator PlayWalkTimelineSounds()
    {
        // Wait 17.5 seconds to play crosswalk sound
        yield return new WaitForSecondsRealtime(16.5f);
        if (crosswalkSound != null)
            crosswalkSound.Play();
        if (buttonSound != null)
            buttonSound.Play();
        // Wait until 39 seconds to play horn
        yield return new WaitForSecondsRealtime(39f - 16.5f);
        if (hornSound != null)
            hornSound.Play();
        yield return new WaitForSecondsRealtime(1f);
        if (crashSound != null)
            crashSound.Play();
    }

    private IEnumerator PlayCrossTimelineSounds()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        if (hornSound != null)
            hornSound.Play();

        // Wait remaining 2 seconds, then play crash at the end
        yield return new WaitForSecondsRealtime(1f);
        if (crashSound != null)
            crashSound.Play();
    }

    private void StopTimelineSounds()
    {
        if (timelineSoundCoroutine != null)
        {
            StopCoroutine(timelineSoundCoroutine);
            timelineSoundCoroutine = null;
        }

        if (hornSound != null && hornSound.isPlaying)
            hornSound.Stop();

        // if (crosswalkSound != null && crosswalkSound.isPlaying)
        //     crosswalkSound.Stop();
    }

    public void onClickRetry()
    {
        Canvas1.SetActive(true);
        Canvas2.SetActive(false);
        Canvas3.SetActive(false);


        // Move the approach timeline to its end state
        approachTimeline.time = approachTimeline.duration;
        approachTimeline.Evaluate();

        // Reset sounds
        StopTimelineSounds();
        if (crashSound != null && crashSound.isPlaying)
            crashSound.Stop();

        if (trafficSound != null && !trafficSound.isPlaying)
            trafficSound.Play();

        // Show Canvas1 again
        OnApproachFinished(approachTimeline);
        ped.GetComponent<PedestrianController>().ResetPedestrian();

    }
}
