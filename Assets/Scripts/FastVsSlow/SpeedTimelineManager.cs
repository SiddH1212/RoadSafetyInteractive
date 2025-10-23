using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpeedTimelineManager : MonoBehaviour
{
    public GameObject startCanvas, Canvas1, Canvas2, Canvas3, Canvas4, Canvas5, Canvas6;
    public Button playButton, fastenButton, ignoreButton, fastButton, slowButton;
    public Button exit1, exit2, exit3, exit4;
    // public GameObject options;
    // public TexmeshProUGUI fastenText;
    private bool seatbelt;
    public GameObject seatBeltWarning;
    public GameObject topHalf1, bottomHalf1, topHalf2, bottomHalf2;
    private Coroutine flashingCoroutine;
    public CameraShake cameraShake;
    public PlayableDirector approachTimeline, slowTimeline, crashDeathTimeline, crashInjuryTimeline, safeTimeline, seatbeltTimeline;

    void Start()
    {
        startCanvas.SetActive(true);
        Canvas1.SetActive(false);
        seatBeltWarning.SetActive(false); // make sure it’s off at start
        playButton.onClick.AddListener(onClickPlay);
        approachTimeline.stopped += onApproachFinished;
        slowTimeline.stopped += onSlowFinished;
        crashDeathTimeline.stopped += onDeathFinished;
        crashInjuryTimeline.stopped += onInjuryFinished;
        safeTimeline.stopped += onSafeFinished;
        seatbeltTimeline.stopped += onSeatFinished;
        // fastenText.text = "";
        Time.timeScale = 0f; // Pause the game initially
    }

    void onClickPlay()
    {
        startCanvas.SetActive(false);
        Canvas1.SetActive(true);
        UpdateRings();
        fastenButton.onClick.AddListener(onClickFasten);
        ignoreButton.onClick.AddListener(onClickIgnore);
    }

    void onSeatFinished(PlayableDirector director)
    {
        approachTimeline.Play();
    }

    void onClickFasten()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        seatbelt = true;
        seatbeltTimeline.Play();

        // stop flashing if it was running
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;
        }
        seatBeltWarning.SetActive(false);
    }

    void onClickIgnore()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        seatbelt = false;
        approachTimeline.Play();

        // start flashing warning
        if (flashingCoroutine == null)
            flashingCoroutine = StartCoroutine(FlashSeatbeltWarning());
    }

    IEnumerator FlashSeatbeltWarning()
    {
        seatBeltWarning.SetActive(true);
        Image warningImage = seatBeltWarning.GetComponent<Image>();

        while (true)
        {
            if (warningImage != null)
                warningImage.enabled = !warningImage.enabled;
            else
                seatBeltWarning.SetActive(!seatBeltWarning.activeSelf);

            yield return new WaitForSeconds(0.5f);
        }
    }

    void onApproachFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas2.SetActive(true);
        fastButton.onClick.AddListener(onClickFast);
        slowButton.onClick.AddListener(onClickSlow);
    }

    void onClickFast()
    {
        Time.timeScale = 1f;
        Canvas2.SetActive(false);
        if (seatbelt)
        {
            ScenarioProgress.seenSeatbeltFast = true;
            crashInjuryTimeline.Play();
            StartCoroutine(TriggerCameraShakeAfterDelay(6.75f));
        }
        else
        {
            ScenarioProgress.seenNoSeatbeltFast = true;
            crashDeathTimeline.Play();
            StartCoroutine(TriggerCameraShakeAfterDelay(6.75f));
        }
    }

    void onClickSlow()
    {
        Time.timeScale = 1f;
        Canvas2.SetActive(false);
        if (seatbelt)
        {
            ScenarioProgress.seenSeatbeltSlow = true;
            safeTimeline.Play();
            StartCoroutine(TriggerCameraShakeAfterDelay(22f));
        }
        else
        {
            ScenarioProgress.seenNoSeatbeltSlow = true;
            slowTimeline.Play();
            StartCoroutine(TriggerCameraShakeAfterDelay(22f));
        }
    }
    IEnumerator TriggerCameraShakeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (cameraShake == null) yield break;

        PlayableDirector activeTimeline = null;
        if (safeTimeline.state == PlayState.Playing)
            activeTimeline = safeTimeline;
        else if (slowTimeline.state == PlayState.Playing)
            activeTimeline = slowTimeline;
        else if (crashDeathTimeline.state == PlayState.Playing)
            activeTimeline = crashDeathTimeline;
        else if (crashInjuryTimeline.state == PlayState.Playing)
            activeTimeline = crashInjuryTimeline;

        // Timeline suspend logic
        if (activeTimeline != null)
        {
            // 1️⃣ Pause Timeline evaluation (stops animation updates)
            activeTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);

            // 2️⃣ Shake camera while timeline is frozen
            cameraShake.TriggerShake();
            yield return new WaitForSeconds(cameraShake.shakeDuration);

            // 3️⃣ Resume Timeline playback
            activeTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
        else
        {
            // No timeline currently running
            cameraShake.TriggerShake();
        }
    }

    void onSlowFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas5.SetActive(true);
        exit3.onClick.AddListener(onClickExit);
    }

    void onDeathFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas3.SetActive(true);
        exit1.onClick.AddListener(onClickExit);
    }

    void onInjuryFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas4.SetActive(true);
        exit2.onClick.AddListener(onClickExit);
    }

    void onSafeFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas6.SetActive(true);
        exit4.onClick.AddListener(onClickExit);
    }
    void UpdateRings()
    {
        topHalf1.SetActive(ScenarioProgress.seenSeatbeltFast);
        bottomHalf1.SetActive(ScenarioProgress.seenSeatbeltSlow);

        topHalf2.SetActive(ScenarioProgress.seenNoSeatbeltFast);
        bottomHalf2.SetActive(ScenarioProgress.seenNoSeatbeltSlow);
    }

    public void onClickExit()
    {
        // stop coroutine before scene reload
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;
        }
        ScenarioProgress.ResetProgress();
        SceneManager.LoadScene("Menu");
    }
}
