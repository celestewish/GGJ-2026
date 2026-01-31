using UnityEngine;

public class ShockwaveVisual : MonoBehaviour
{
    public float maxRadius = 3f;
    public float duration = 0.25f;
    public AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private float time;

    void Update()
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);

        // Scale from 0 to maxRadius*2
        float s = scaleCurve.Evaluate(t) * maxRadius * 2f;
        transform.localScale = new Vector3(s, s, 1f);

        // Fade out
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color c = sr.color;
            c.a = 1f - t;
            sr.color = c;
        }

        if (t >= 1f)
            Destroy(gameObject);
    }
}
