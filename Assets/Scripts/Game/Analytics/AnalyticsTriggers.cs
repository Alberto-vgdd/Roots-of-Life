using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsTriggers : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Main area trigger 1"))
        {
            Debug.Log(Analytics.CustomEvent("time between start and first trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            }));
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("Main area trigger 2"))
        {
            Analytics.CustomEvent("time between first and second trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("Puzzle 1 trigger 1"))
        {
            Analytics.CustomEvent("time between Puzzle 1 and first trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("SpringJump"))
        {
            Analytics.CustomEvent("time between P1 first and second trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("Puzzle 1 trigger 3"))
        {
            Analytics.CustomEvent("time between P1 second and third trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("Puzzle 1 trigger 4"))
        {
            Analytics.CustomEvent("time between third and final trigger", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("Puzzle 1 secret"))
        {
            Analytics.CustomEvent("Found secret area P1", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
        if (other.CompareTag("Puzzle 2 secret 1"))
        {
            Analytics.CustomEvent("Found secret area 1 P2", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;

        }
        if (other.CompareTag("Puzzle 2 secret 2"))
        {
            Analytics.CustomEvent("Found secret area 2 P2", new Dictionary<string, object>
            {
                {"time elapsed:", GlobalData.GameManager.timeSinceStart}
            });
            GlobalData.GameManager.timeSinceStart = 0f;
        }
    }
}
