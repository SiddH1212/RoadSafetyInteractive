using UnityEngine;
using System.Collections;

public class TrafficManager : MonoBehaviour
{
    public HaloOnlyTrafficLightController lightA1;
    public HaloOnlyTrafficLightController lightA2;
    public HaloOnlyTrafficLightController lightB1;
    public HaloOnlyTrafficLightController lightB2;
    public HaloOnlyTrafficLightController lightC1;
    public HaloOnlyTrafficLightController lightC2;
    public HaloOnlyTrafficLightController lightD1;
    public HaloOnlyTrafficLightController lightD2;

    private Coroutine trafficRoutine;

    void Start()
    {
        // Initialize baseline
        lightC1.SetRed();
        lightC2.SetRed();
        lightD1.SetRed();
        lightD2.SetRed();

        // Start main cycle
        trafficRoutine = StartCoroutine(RunTrafficLights());
    }

    IEnumerator RunTrafficLights()
    {
        // t = 0s — Player's light is green, opposite direction is red
        lightA1.SetGreen();
        lightA2.SetGreen();
        lightB1.SetRed();
        lightB2.SetRed();
        Debug.Log("1");
        yield return new WaitForSeconds(7f);

        // t = 7s — Player's light turns yellow
        lightA1.SetYellow();
        lightA2.SetYellow();
        Debug.Log("2");
        yield return new WaitForSeconds(3f); // now t = 10s

        // t = 10s — Player's light is red, opposite turns green
        lightA1.SetRed();
        lightA2.SetRed();
        lightB1.SetGreen();
        lightB2.SetGreen();
        Debug.Log("3");
        yield return new WaitForSeconds(10f); // now t = 20s

        // t = 20s — Player's light is green again
        lightA1.SetGreen();
        lightA2.SetGreen();
        lightB1.SetRed();
        lightB2.SetRed();
        Debug.Log("4");
    }

    public void ResetToStateAt10s()
    {
        // Stop any existing traffic coroutine
        if (trafficRoutine != null)
        {
            StopCoroutine(trafficRoutine);
            trafficRoutine = null;
        }

        // Set lights to the 10-second state (red for A, green for B)
        lightA1.SetRed();
        lightA2.SetRed();
        lightB1.SetGreen();
        lightB2.SetGreen();
        lightC1.SetRed();
        lightC2.SetRed();
        lightD1.SetRed();
        lightD2.SetRed();
        Debug.Log("Traffic reset to 10-second state.");

        // Restart coroutine from "after 10s"
        trafficRoutine = StartCoroutine(ResumeFrom10s());
    }

    private IEnumerator ResumeFrom10s()
    {
        // Continue from t = 10s to t = 20s
        yield return new WaitForSeconds(10f);

        // At 20s — Player's light turns green, opposite red again
        lightA1.SetGreen();
        lightA2.SetGreen();
        lightB1.SetRed();
        lightB2.SetRed();
        Debug.Log("Resumed to 20s state.");

        // You could loop this again if you want a full cycle:
        // trafficRoutine = StartCoroutine(RunTrafficLights());
    }
}
