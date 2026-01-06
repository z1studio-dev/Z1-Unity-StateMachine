using UnityEngine;
using System.Collections;

public class ToolTipController : MonoBehaviour
{
    [Tooltip("Unique key registered with TooltipManager")]
    public string tooltipID;

    [Header("References")]
    public GameObject toolTipAnimatedChild;
    public Material toolTipMaterialGameObject;

    [Header("Animation Settings")]
    public float animationDuration = 1f;
    public float animationHeight = 1f;

    public enum MoveAxis { X, Y, Z }
    public MoveAxis axis;
    public bool invertAxis = false;

    [Header("Show/Hide Options")]
    public bool lookAtCamera = false;
    public bool showOnLookOnly = true;
    public bool enableOnlyOnce = false;

    [Header("Combine Effects")]
    public bool useFadeOnShow = true;
    public bool useMoveOnShow = true;
    public bool useFadeOnHide = true;
    public bool useMoveOnHide = true;

    [Header("Floating Effect")]
    public bool useFloating = false;
    public enum FloatType { Bounce, Scale }
    public FloatType floatingType = FloatType.Bounce;
    public float floatingAmplitude = 0.1f;
    public float floatingFrequency = 1f;

    private bool hasShown = false;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Color tooltipColor;
    private Coroutine fadeCoroutine;
    private Coroutine moveCoroutine;
    private Coroutine floatingCoroutine;

    void Start()
    {
        // Register with manager
        TooltipManager.Instance.RegisterTooltip(tooltipID, this);

        originalPosition = toolTipAnimatedChild.transform.localPosition;
        originalScale = toolTipAnimatedChild.transform.localScale;

        // Initialize alpha to zero
        if (toolTipMaterialGameObject != null)
        {
            tooltipColor = toolTipMaterialGameObject.GetColor("_BaseColor");
            tooltipColor.a = 0f;
            AssignColor(tooltipColor);
        }

        // Auto-show if desired
        if (!showOnLookOnly)
            AnimateToolTipUp();
    }

    public void AnimateToolTipUp()
    {
        if (enableOnlyOnce && hasShown) return;

        // Reset position & scale before any move/floating
        toolTipAnimatedChild.transform.localPosition = originalPosition;
        toolTipAnimatedChild.transform.localScale = originalScale;

        // Stop ongoing coroutines
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (floatingCoroutine != null) StopCoroutine(floatingCoroutine);

        // Start requested effects
        if (useFadeOnShow) fadeCoroutine = StartCoroutine(FadeCoroutine(true));
        if (useMoveOnShow) moveCoroutine = StartCoroutine(MoveCoroutine(true));
        if (useFloating) floatingCoroutine = StartCoroutine(FloatingCoroutine());

        hasShown = true;
    }

    public void AnimateToolTipDown()
    {
        // Stop ongoing coroutines
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        if (floatingCoroutine != null) StopCoroutine(floatingCoroutine);

        // Reset to base on hide
        toolTipAnimatedChild.transform.localScale = originalScale;

        // Start requested effects
        if (useFadeOnHide) fadeCoroutine = StartCoroutine(FadeCoroutine(false));
        if (useMoveOnHide) moveCoroutine = StartCoroutine(MoveCoroutine(false));
    }

    void LateUpdate()
    {
        if (lookAtCamera)
            toolTipAnimatedChild.transform.LookAt(Camera.main.transform);
    }

    private IEnumerator FadeCoroutine(bool fadeIn)
    {
        float elapsed = 0f;
        float startAlpha = tooltipColor.a;
        float targetAlpha = fadeIn ? 1f : 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / animationDuration));
            tooltipColor.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            AssignColor(tooltipColor);
            yield return null;
        }

        tooltipColor.a = targetAlpha;
        AssignColor(tooltipColor);
    }

    private IEnumerator MoveCoroutine(bool moveUp)
    {
        Vector3 startPos = toolTipAnimatedChild.transform.localPosition;
        Vector3 direction = axis == MoveAxis.X ? Vector3.right
                         : axis == MoveAxis.Y ? Vector3.up
                         : Vector3.forward;
        if (invertAxis) direction = -direction;

        Vector3 targetPos = moveUp ? originalPosition + direction * animationHeight
                                   : originalPosition;

        float elapsed = 0f;
        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / animationDuration));
            toolTipAnimatedChild.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        toolTipAnimatedChild.transform.localPosition = targetPos;
    }

    private IEnumerator FloatingCoroutine()
    {
        Vector3 basePos = toolTipAnimatedChild.transform.localPosition;
        Vector3 baseScale = toolTipAnimatedChild.transform.localScale;
        float phase = 0f;

        while (true)
        {
            phase += Time.deltaTime * floatingFrequency * Mathf.PI * 2f;
            float offset = Mathf.Sin(phase) * floatingAmplitude;

            if (floatingType == FloatType.Bounce)
                toolTipAnimatedChild.transform.localPosition = basePos + Vector3.up * offset;
            else
                toolTipAnimatedChild.transform.localScale = baseScale * (1f + offset);

            yield return null;
        }
    }

    private void AssignColor(Color color)
    {
        toolTipMaterialGameObject.SetColor("_BaseColor", color);
    }
}
