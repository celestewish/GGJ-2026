using UnityEngine;

public class FloatingTitle : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatHeight = 10f;     // up/down distance
    public float floatSpeed = 0.3f;       // cycles/sec
    public float startOffset = 0f;      // phase shift

    private Vector2 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float newY = Mathf.Sin(Time.time * floatSpeed * Mathf.PI * 2f + startOffset) * floatHeight;
        transform.localPosition = new Vector2(startPos.x, startPos.y + newY);
    }
}

