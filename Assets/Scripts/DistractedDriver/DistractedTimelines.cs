using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class DistractedTimelines : MonoBehaviour
{
    public Button textBack;
    public Button callNow;
    public Button ignore;
    public GameObject options;
    public GameObject title;
    public Button exit1;
    public Button exit2;
    public Button Play;
    // public GameObject dots;
    public GameObject textBackObj;
    public GameObject callNowObj;
    public GameObject image;
    public GameObject StartCanvas;
    public GameObject Canvas1;
    public GameObject Canvas2;
    public GameObject Canvas3;
    public PlayableDirector approachTimeline;
    public PlayableDirector crashTimeline;
    public PlayableDirector stopTimeline;
    public GameObject TrafficManager;
    public GameObject crack;
    public CameraShake cameraShake;
    private float x;
    public AudioSource textSound, callSound, crashSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TrafficManager.SetActive(false);
        Canvas1.SetActive(false);
        Canvas2.SetActive(false);
        Canvas3.SetActive(false);
        textBackObj.SetActive(false);
        // dots.SetActive(false);
        callNowObj.SetActive(false);
        StartCanvas.SetActive(true);
        crack.SetActive(false);
        Play.onClick.AddListener(onClickPlay);
        approachTimeline.stopped += OnApproachFinished;
        stopTimeline.stopped += OnStopFinished;
        crashTimeline.stopped += onCrashFinished;
        Time.timeScale = 0f; // Pause the game initially
    }
    public void onClickPlay()
    {
        Time.timeScale = 1f; // Resume the game
        approachTimeline.Play();
        StartCanvas.SetActive(false);
        TrafficManager.SetActive(true);
    }
    void OnApproachFinished(PlayableDirector director)
    {
        Time.timeScale = 0f;
        Canvas1.SetActive(true);
        options.SetActive(true);
        title.SetActive(true);
        textBack.onClick.AddListener(() => onClickWrong(textBackObj));
        callNow.onClick.AddListener(() => onClickWrong(callNowObj));
        ignore.onClick.AddListener(onClickCorrect);
    }
    void OnStopFinished(PlayableDirector director)
    {
        Canvas2.SetActive(true);
        exit1.onClick.AddListener(onClickExit);
        Time.timeScale = 0f;
    }
    public void onCrashFinished(PlayableDirector director)
    {
        StartCoroutine(HandleCrashSequence());
    }

    private IEnumerator HandleCrashSequence()
    {
        crack.SetActive(true);
        TriggerHaptic(0.8f, 0.2f);
        triggerShake();

        // Let physics play out for 2 seconds
        yield return new WaitForSeconds(1f);

        Canvas3.SetActive(true);
        exit2.onClick.AddListener(onClickExit);
        Time.timeScale = 0f; // Pause after crash is complete
    }
    public void TriggerHaptic(float amplitude, float duration)
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        InputDevice leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (rightHand.TryGetHapticCapabilities(out HapticCapabilities capabilities) && capabilities.supportsImpulse)
        {
            rightHand.SendHapticImpulse(0, amplitude, duration);
        }
        if (leftHand.TryGetHapticCapabilities(out capabilities) && capabilities.supportsImpulse)
        {
            leftHand.SendHapticImpulse(0, amplitude, duration);
        }
    }
    public void onClickWrong(GameObject enabledObject)
    {
        options.SetActive(false);
        title.SetActive(false);
        StartCoroutine(HandleWrongChoice(enabledObject));
    }
    void triggerShake()
    {
        cameraShake.TriggerShake();
    }
    private IEnumerator HandleWrongChoice(GameObject enabledObject)
    {
        // wait 1 second before showing anything
        // yield return new WaitForSecondsRealtime(1f);

        if (enabledObject == textBackObj)
        {
            // show dots for 2s before textBackObj
            // dots.SetActive(true);
            // yield return new WaitForSecondsRealtime(1f);
            // dots.SetActive(false);
            enabledObject.SetActive(true);
            textSound.Play();
            TMP_Text tmp = enabledObject.GetComponentInChildren<TMP_Text>();
            if (tmp != null)
            {
                string fullText = tmp.text;  // store full text
                tmp.text = "";  // clear before typing

                // Type the text over 2 seconds
                StartCoroutine(TypeTextOverTime(tmp, fullText, 4f));
            }
            x = 4f;
        }
        else
        {
            // callNowObj — skip dots
            enabledObject.SetActive(true);
            callSound.Play();
            image.SetActive(true);
            x = 4f;
        }

        // now continue the crash sequence
        StartCoroutine(PlayCrashTimelineWithDelay());
        yield break;
    }
    private IEnumerator TypeTextOverTime(TMP_Text textComponent, string fullText, float totalDuration)
    {
        textComponent.text = ""; // clear existing text

        int length = fullText.Length;
        if (length == 0) yield break;

        float delay = totalDuration / length; // time per character

        foreach (char c in fullText)
        {
            textComponent.text += c;
            yield return new WaitForSecondsRealtime(delay);
        }
    }
    private IEnumerator PlayCrashTimelineWithDelay()
    {
        Time.timeScale = 1f;
        
        crashTimeline.Play();
        yield return new WaitForSecondsRealtime(x);
        callSound.Stop();
        textSound.Stop();
        Canvas1.SetActive(false);
        crashSound.Play();
    }
    public void onClickCorrect()
    {
        Time.timeScale = 1f;
        Canvas1.SetActive(false);
        stopTimeline.Play();
    }
    public void onClickExit()
    {
        SceneManager.LoadScene("Menu");
    }
    void OnDestroy()
    {
        approachTimeline.stopped -= OnApproachFinished;
        stopTimeline.stopped -= OnStopFinished;
        crashTimeline.stopped -= onCrashFinished;
    }
}
