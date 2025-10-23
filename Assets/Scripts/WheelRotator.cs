using UnityEngine;

public class WheelRotator : MonoBehaviour
{
    public Transform[] wheels; // assign your wheel meshes here
    public float wheelRadius = 0.35f; // in meters, adjust for your model
    public bool rotateSteering = false; // for front wheels if needed
    public Transform steeringTarget; // optional, for turning

    private Vector3 lastPos;

    void Start()
    {
        lastPos = transform.position;
    }

    void Update()
    {
        // distance moved since last frame
        Vector3 movement = transform.position - lastPos;
        float distance = movement.magnitude;

        // angle = distance / circumference * 360
        float rotationAngle = (distance / (2 * Mathf.PI * wheelRadius)) * 360f;

        // rotate each wheel forward
        foreach (Transform wheel in wheels)
        {
            wheel.Rotate(Vector3.right, rotationAngle, Space.Self);

            // optionally align front wheels to direction of motion
            if (rotateSteering && steeringTarget != null)
                wheel.localRotation = Quaternion.Euler(
                    wheel.localRotation.eulerAngles.x,
                    steeringTarget.localEulerAngles.y,
                    0
                );
        }

        lastPos = transform.position;
    }
}
