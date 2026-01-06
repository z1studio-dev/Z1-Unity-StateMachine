using UnityEngine;

public class MoveUpAndDown : MonoBehaviour
{
    public float speed = 1.0f;
    public float distance = 1.0f;

    public bool enableMouvement = true;

    public Rigidbody rb;

    public void EnableMouvement(bool enable)
    {
        enableMouvement = enable;
    }

    void Update()
    {
        if (enableMouvement)
        {
            rb.AddForce(Vector3.up * Mathf.Sin(Time.time * speed) * distance);
        }
    }
}
