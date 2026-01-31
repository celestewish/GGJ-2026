using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultiplayerCamera : MonoBehaviour
{
    [Header("Targets")]
    public List<Transform> targets = new List<Transform>();

    [Header("Position")]
    public Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.2f;

    [Header("Zoom")]
    public float minZoom = 5f;
    public float maxZoom = 15f;
    public float zoomLimiter = 10f; // bigger = less zooming

    private Vector3 velocity;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void LateUpdate()
    {
        if (targets.Count == 0) return;

        Move();
        Zoom();
    }

    void Move()
    {
        Bounds bounds = GetTargetsBounds();
        Vector3 centerPoint = bounds.center;

        Vector3 desiredPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }

    void Zoom()
    {
        Bounds bounds = GetTargetsBounds();

        // Use the largest extent (x or y) to determine zoom
        float greatestDistance = Mathf.Max(bounds.size.x, bounds.size.y);
        float targetZoom = Mathf.Lerp(maxZoom, minZoom, greatestDistance / zoomLimiter);
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            Time.deltaTime * 5f
        );
    }

    Bounds GetTargetsBounds()
    {
        if (targets.Count == 0)
            return new Bounds(transform.position, Vector3.zero);

        // Start with first active target
        int i = 0;
        while (i < targets.Count && (targets[i] == null || !targets[i].gameObject.activeInHierarchy))
            i++;

        Bounds bounds = new Bounds(targets[i].position, Vector3.zero);

        for (int j = i + 1; j < targets.Count; j++)
        {
            var t = targets[j];
            if (t == null || !t.gameObject.activeInHierarchy) continue;
            bounds.Encapsulate(t.position); // grow box to include this target
        }

        return bounds;
    }
}
