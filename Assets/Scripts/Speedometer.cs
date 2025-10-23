using UnityEngine;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public RectTransform speedNeedle;
    [SerializeField] private float needleMinAngle = 127f;
    [SerializeField] private float needleMaxAngle = -127f;
    [SerializeField] private float maxSpeed = 220f;
    [SerializeField] private float smoothing = 5f;

    private Vector3 prevPos;
    private float currentSpeed = 0f;
    private float displayedSpeed = 0f;

    void Start()
    {
        prevPos = transform.position;
    }

    void LateUpdate()
    {
        // calculate raw speed along car's forward
        Vector3 delta = transform.position - prevPos;
        float rawSpeed = Vector3.Project(delta, transform.forward).magnitude * 3.6f / Time.unscaledDeltaTime;

        // only update speed if car actually moved
        if (Time.timeScale >0f)
        {
            currentSpeed = rawSpeed;
            displayedSpeed = Mathf.Lerp(displayedSpeed, currentSpeed, Time.unscaledDeltaTime * smoothing);
        }
        // else keep displayedSpeed as-is, don't let it fall to 0

        UpdateSpeedometer(displayedSpeed);
        prevPos = transform.position;
    }

    void UpdateSpeedometer(float speed)
    {
        float clampedSpeed = Mathf.Clamp(speed, 0, maxSpeed);
        float t = clampedSpeed / maxSpeed;
        float angle = Mathf.Lerp(needleMinAngle, needleMaxAngle, t);
        speedNeedle.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
