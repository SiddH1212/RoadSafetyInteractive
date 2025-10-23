using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    public Transform vrCamera;     // Assign your XR Origin’s main camera here (usually "Main Camera")
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.03f;
    public float dampingSpeed = 1.0f;

    private Vector3 initialPos;
    // private bool isShaking = false;

    public void TriggerShake()
    {
        // if (!isShaking)
        //     StartCoroutine(Shake());
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        // isShaking = true;
        initialPos = vrCamera.localPosition;
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            // Random offset within a small sphere
            Vector3 randomPoint = Random.insideUnitSphere * shakeMagnitude;
            vrCamera.localPosition = initialPos + randomPoint;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Smoothly return camera to normal
        float t = 0f;
        while (t < 1f)
        {
            vrCamera.localPosition = Vector3.Lerp(vrCamera.localPosition, initialPos, t);
            t += Time.deltaTime * dampingSpeed;
            yield return null;
        }

        vrCamera.localPosition = initialPos;
        // isShaking = false;
    }
}
