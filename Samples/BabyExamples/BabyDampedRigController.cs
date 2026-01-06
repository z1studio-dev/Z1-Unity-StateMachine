using UnityEngine;
using UnityEngine.Animations.Rigging;
using System.Collections;

public class BabyDampedRigController : MonoBehaviour
{
    public Rig rig;
    public float defaultTransitionDuration = 1f;
    public float defaultWieghtValue = 1f;
    
    private Coroutine currentTransition;

    /// <summary>
    /// Lance la transition du weight du rig.
    /// </summary>
    /// <param name="follow">true pour weight à 1, false pour weight à 0</param>
    /// <param name="duration">Durée de la transition, ou null pour utiliser la valeur par défaut</param>
    public void SetWeight(float weight, float? duration = null)
    {
        if (currentTransition != null)
            StopCoroutine(currentTransition);

        currentTransition = StartCoroutine(AnimateRigWeight(weight, duration ?? defaultTransitionDuration));
    }

    private IEnumerator AnimateRigWeight(float weight, float duration)
    {
        float startWeight = rig.weight;
        float targetWeight = weight;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // Smoothstep pour interpolation douce
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            rig.weight = Mathf.Lerp(startWeight, targetWeight, smoothT);
            yield return null;
        }

        rig.weight = targetWeight;
        currentTransition = null;
    }
}