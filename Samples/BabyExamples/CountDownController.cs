using System;
using UnityEngine;
using System.Collections;

public static class CountDownController
{
    // Internal helper MonoBehaviour that actually runs the coroutines
    private class TimerBehaviour : MonoBehaviour { }

    // The hidden GameObject/MonoBehaviour we use to run coroutines
    private static TimerBehaviour _runner;

    /// <summary>
    /// Calls your callback after delaySeconds have elapsed.
    /// </summary>
    public static void StartTimer(Action callback, float delaySeconds)
    {
        // Make sure we have a runner in the scene
        if (_runner == null)
        {
            Debug.Log("Starting CountDownController in: " + delaySeconds + " seconds with methode: " + callback.Method.ToString());
            var go = new GameObject("__CountDownController");
            // hide in hierarchy, survives scene loads
            go.hideFlags = HideFlags.HideAndDontSave;
            UnityEngine.Object.DontDestroyOnLoad(go);
            _runner = go.AddComponent<TimerBehaviour>();
        }
        _runner.StartCoroutine(RunTimer(callback, delaySeconds));
    }

    private static IEnumerator RunTimer(Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}