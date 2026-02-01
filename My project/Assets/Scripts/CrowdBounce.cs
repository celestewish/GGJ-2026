using UnityEngine;

public class CrowdBounce : MonoBehaviour
{
    public float amplitude = 0.5f; // How high the character goes
    public float speed = 1.0f;     // How fast they bounce
    private float startY;

    void Start()
    {
        startY = transform.position.y;
        // Optional: add a random offset so they don't all move in sync
        speed += Random.Range(-0.5f, 0.5f);
    }

    void Update()
    {
        // Use a sine wave to create a smooth up and down motion
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, startY + yOffset, transform.position.z);
    }
}

