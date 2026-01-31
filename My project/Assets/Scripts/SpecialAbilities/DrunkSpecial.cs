using System.Collections;
using UnityEngine;

public class DrunkSpecial : CharacterParentClass
{
    [Header("Inversion Settings")]
    [SerializeField] private float inversionChancePerSecond = 0.8f; // 0–1
    [SerializeField] private float inversionDuration = 2f;

    private bool controlsInverted = false;


    protected override void Update()
    {
        base.Update();

        // Randomly decide to invert controls while playing
        // (only when not already inverted)
        if (!controlsInverted)
        {
            float p = inversionChancePerSecond * Time.deltaTime;
            if (Random.value < p)
            {
                Debug.Log("Inverted");
                StartCoroutine(InvertControlsTemporarily());
            }
        }
    }

    void Inversion()
    {
        // Get raw input from parent style
        Vector2 raw = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        // Apply inversion
        if (controlsInverted)
        {
            raw *= -1f;
        }

        inputVector = raw;
    }

    private IEnumerator InvertControlsTemporarily()
    {
        controlsInverted = true;
        Inversion();
        yield return new WaitForSeconds(inversionDuration);
        controlsInverted = false;
        Inversion();
    }

    protected override void PerformAttack()
    {
        float original = hitStrength;
        float boosted = hitStrength * 2f;
        hitStrength = boosted;

        base.PerformAttack();

        hitStrength = original;
    }
}
