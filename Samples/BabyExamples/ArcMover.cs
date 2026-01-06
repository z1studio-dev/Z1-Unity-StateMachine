using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ArcMover : MonoBehaviour
{
    [Header("Target & Timing")]
    [Tooltip("Where you want the object to end up.")]
    public Transform target;

    [Tooltip("How long (in seconds) the move should take.")]
    public float duration = 1.0f;

    [Header("Path Shape")]
    [Tooltip("Maximum height above the straight line.")]
    public float arcHeight = 1.0f;

    [Tooltip("If you prefer full control over the height curve, assign one here.")]
    public AnimationCurve heightCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Physics vs Kinematic")]
    [Tooltip("If true, uses Rigidbody.MovePosition; otherwise sets transform.position directly.")]
    public bool usePhysics = true;
    
    [Header("Draw debug Gizmos")]
    [Tooltip("If true, draw debug gizmos on the scene.")]
    public bool drawDebugGizmos = true;

    private Rigidbody _rb;
    private Vector3 _startPos;
    private bool _isMoving = false;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // If you want true physics collisions en route, keep this false until the end.
        _rb.isKinematic = usePhysics == false;
    }

    /// <summary>
    /// Call this to kick off the movement.
    /// </summary>
    public void BeginMove()
    {
        if (target == null || duration <= 0f || _isMoving) return;
        _startPos = transform.position;
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        _isMoving = true;

        float elapsed = 0f;
        // Temporarily make kinematic if using physics, to avoid unwanted forces during the tween.
        if (usePhysics) _rb.isKinematic = true;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            // soften start and end
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            // straight interpolation between A and B
            Vector3 flatPos = Vector3.Lerp(_startPos, target.position, smoothT);

            // add arc in Y
            float heightOffset = heightCurve != null
                ? heightCurve.Evaluate(smoothT) * arcHeight
                : Mathf.Sin(Mathf.PI * smoothT) * arcHeight;

            Vector3 nextPos = flatPos + Vector3.up * heightOffset;

            if (usePhysics)
            {
                _rb.MovePosition(nextPos);
            }
            else
            {
                transform.position = nextPos;
            }

            yield return null;
        }

        // ensure exact final placement
        if (usePhysics)
        {
            _rb.MovePosition(target.position);
            // if you want it to resume physics interactions at the end:
            _rb.isKinematic = false;
        }
        else
        {
            transform.position = target.position;
        }

        _isMoving = false;
    }

    // Optional: editor helper to visualize the arc in Scene view
    void OnDrawGizmosSelected()
    {
        if (target == null || !drawDebugGizmos) return;
        const int steps = 20;
        Vector3 prev = transform.position;
        for (int i = 1; i <= steps; i++)
        {
            float t = i / (float)steps;
            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            Vector3 flat = Vector3.Lerp(transform.position, target.position, smoothT);
            float h = heightCurve != null
                ? heightCurve.Evaluate(smoothT) * arcHeight
                : Mathf.Sin(Mathf.PI * smoothT) * arcHeight;
            Vector3 next = flat + Vector3.up * h;
            Gizmos.color = Color.Lerp(Color.green, Color.red, t);
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
