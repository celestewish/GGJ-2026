using System.Collections;
using UnityEngine;

public class DrunkSpecial : CharacterParentClass
{
    [Header("Inversion Settings")]
    [SerializeField] private float inversionChancePerSecond = 0.8f; // 0�1
    [SerializeField] private float inversionDuration = 5f;

    private bool controlsInverted = false;

    //RAAAAAAAAAAAAAAAAAAAAAAAAAAAA
    private void Start()
    {
        hitStrength = 20f;
    }

    protected override void Update()
    {
        base.Update();

        // Randomly decide to invert controls while playing
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

    // Overrides move to invert controls
    protected override void Move()
    {
        Vector2 finalInput = moveInput;

        if (controlsInverted)
        {
            finalInput *= -1f;
        }

        rb.linearVelocity = finalInput * Speed;
    }

    // Inverts controls for a set number of seconds
    private IEnumerator InvertControlsTemporarily()
    {
        controlsInverted = true;
        yield return new WaitForSeconds(inversionDuration);
        controlsInverted = false;
    }

}
